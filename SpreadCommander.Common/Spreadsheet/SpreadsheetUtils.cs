#pragma warning disable CRR0047

using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.DataAccess.Excel;
using DevExpress.LookAndFeel;
using DevExpress.Spreadsheet;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Helpers;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Extensions;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Spreadsheet.Export;

namespace SpreadCommander.Common.Spreadsheet
{
    public static partial class SpreadsheetUtils
    {
        #region XlColumnPositionConverter
        public class XlColumnPositionConverter : DevExpress.Export.Xl.IXlColumnPositionConverter
        {
            private readonly List<string> _ColumnNames = new List<string>();

            public XlColumnPositionConverter(IList<string> columnNames)
            {
                _ColumnNames.AddRange(columnNames);
            }

            public int GetColumnIndex(string name)
            {
                for (int i = 0; i < _ColumnNames.Count; i++)
                {
                    if (string.Compare(_ColumnNames[i], name, true) == 0)
                        return i;
                }

                return -1;
            }

            public int GetRowOffset(string columnName) => 0;
        }
        #endregion

        static SpreadsheetUtils()
        {
            RegisterCustomSpreadsheetFunctions();
        }

        public static void InitializeWorkbook(IWorkbook workbook)
        {
            //workbook.Options.Culture = CultureInfo.InvariantCulture;
            workbook.DocumentSettings.ShowPivotTableFieldList = false;

            workbook.Options.Import.Csv.Encoding = Encoding.UTF8;
            workbook.Options.Import.Txt.Encoding = Encoding.UTF8;
            workbook.Options.Export.Csv.Encoding = Encoding.UTF8;
            workbook.Options.Export.Txt.Encoding = Encoding.UTF8;

            switch (ApplicationSettings.Default.SpreadsheetCulture)
            {
                case SpreadsheetCulture.Default:
                case SpreadsheetCulture.Invariant:
                    workbook.Options.Culture = CultureInfo.InvariantCulture;
                    break;
                case SpreadsheetCulture.OS:
                    workbook.Options.Culture = CultureInfo.DefaultThreadCurrentCulture;
                    break;
                default:
                    workbook.Options.Culture = CultureInfo.InvariantCulture;
                    break;
            }

            RegisterCustomSpreadsheetFunctionDescriptions(workbook);
        }

        public static Workbook CreateWorkbook()
        {
            var workbook = new Workbook();
            workbook.History.IsEnabled = false; //History is not needed in non-UI spreadsheet, disabling it improves performance.
            InitializeWorkbook(workbook);
            return workbook;
        }

        public static IList<string> GetTableNames(IWorkbook workbook, bool includeSheetName = true)
        {
            if (workbook == null)
                return null;

            var tableNames = new List<string>();

            foreach (var worksheet in workbook.Worksheets)
            { 
                foreach (var table in worksheet.Tables)
                {
                    var tableName = includeSheetName ? $"{worksheet.Name}!{table.Name}" : table.Name;
                    tableNames.Add(tableName);
                }

                //Add also defined names, table name cannot be equal to defined name
                foreach (var definedName in worksheet.DefinedNames)
                {
                    var defName = includeSheetName ? $"{worksheet.Name}!{definedName.Name}" : definedName.Name;
                    tableNames.Add(defName);
                }
            }

            return tableNames;
        }

        public static CellRange GetWorkbookRange(IWorkbook workbook, string tableName)
        {
            if (workbook == null || string.IsNullOrWhiteSpace(tableName))
                throw new Exception($"Invalid range: {tableName}");

            string worksheetName, worksheetTableName;

            int p = tableName.IndexOf('!');
            if (p >= 0)
            {
                worksheetName = tableName.Substring(0, p);
                worksheetTableName = tableName.Substring(p + 1);

                var worksheet = workbook.Worksheets[worksheetName];
                if (worksheet == null)
                    return null;

                foreach (var table in worksheet.Tables)
                {
                    if (string.Compare(table.Name, worksheetTableName, true) == 0)
                        return table.Range.Exclude(table.TotalRowRange);
                }

                foreach (var definedName in worksheet.DefinedNames)
                {
                    if (string.Compare(definedName.Name, worksheetTableName, true) == 0)
                        return definedName.Range;
                }

                try
                {
                    //Try to get range that is not table or named range, may be it is Table!A1:C10 or similar
                    var range = worksheet.Range[worksheetTableName];
                    if (range != null)
                        return range;
                }
                catch (Exception)
                {
                    //Invalid range
                    throw new Exception($"Invalid range '{worksheetTableName}' in table '{tableName}'");
                }
            }
            else
            {
                foreach (var definedName in workbook.DefinedNames)
                {
                    if (string.Compare(definedName.Name, tableName, true) == 0)
                        return definedName.Range;
                }

                foreach (var worksheet in workbook.Worksheets)
                {
                    if (string.Compare(worksheet.Name, tableName, true) == 0)
                        return worksheet.GetDataRange();
                }
            }

            throw new Exception($"Invalid range: {tableName}");
        }

        public static DataTable GetDataTable(IWorkbook workbook, string tableName)
        {
            var tableRange = GetWorkbookRange(workbook, tableName);
            var dataTable  = GetDataTable(tableRange);
            return dataTable;
        }

        public static DbDataReader GetTableDataReader(IWorkbook workbook, string tableName)
        {
            var dataTable = GetDataTable(workbook, tableName);
            var reader    = dataTable.CreateDataReader();
            return reader;
        }

        public static DataTable GetDataTable(Table table)
        {
            var range = table.Range.Exclude(table.TotalRowRange);
            var result = GetDataTable(range);
            return result;
        }

        public static DataTable GetDataTable(CellRange range)
        {
            var dataTable = range.Worksheet.CreateDataTable(range, true);

            for (int col = 0; col < range.ColumnCount; col++)
            {
                CellValueType cellType = range[0, col].Value.Type;
                for (int r = 1; r < range.RowCount; r++)
                {
                    if (cellType != range[r, col].Value.Type)
                    {
                        dataTable.Columns[col].DataType = typeof(string);
                        break;
                    }
                }
            }

            var exporter = range.Worksheet.CreateDataTableExporter(range, dataTable, true);
            exporter.Export();
            return dataTable;
        }

        public static string RemoveInvalidRangeNameCharacters(string rangeName)
        {
            var result = new StringBuilder();
            if (string.IsNullOrWhiteSpace(rangeName))
                rangeName = "Table";

            foreach (char c in rangeName)
            {
                if (result.Length <= 0)
                {
                    if (char.IsLetter(c) || c == '_' || c == '\\')
                        result.Append(c);
                    else
                        result.Append('_');
                }
                else
                {
                    if (char.IsLetterOrDigit(c) || c == '.' || c == '_')
                        result.Append(c);
                    else
                        result.Append('_');
                }
            }

            return result.ToString();
        }

        public static string GetUniqueTableName(IWorkbook workbook, string baseTableName)
        {
            baseTableName = RemoveInvalidRangeNameCharacters(baseTableName);
            var result    = Utils.GetExcelSheetName(baseTableName, GetTableNames(workbook, false));
            result        = RemoveInvalidRangeNameCharacters(result);
            return result;
        }

        public static Table AppendDataSource(IWorkbook workbook, object dataSource, string sheetName = "Table")
        {
            var newSheetName = Utils.GetExcelSheetName(sheetName, workbook.Worksheets.Select(sheet => sheet.Name).ToList());

            Worksheet worksheet;
            if (workbook.Worksheets.Count == 1 && IsRangeEmpty(workbook.Worksheets[0].GetDataRange()))
            {
                worksheet = workbook.Worksheets[0];
                worksheet.Name = newSheetName;
            }
            else 
                worksheet = workbook.Worksheets.Add(newSheetName);

            var result = AppendDataSource(worksheet, dataSource);
            return result;

            static bool IsRangeEmpty(CellRange range)
            {
                if (range.TopRowIndex == 0 && range.BottomRowIndex == 0 &&
                    range.LeftColumnIndex == 0 && range.RightColumnIndex == 0)
                    return true;
                return false;
            }
        }

        public static Table AppendDataSource(Worksheet worksheet, object dataSource, bool createTable = true,
            int firstRowIndex = 0, int firstColumnIndex = 0)
        {
            if (firstRowIndex == 0)
                worksheet.FreezeRows(0);

            var source = dataSource;
            if (dataSource is DataTable dataTable)
            {
                var resultTable = AppendDataTable(worksheet, dataTable, createTable, firstRowIndex, firstColumnIndex);
                return resultTable;
            }
            if (dataSource is IList)
            {
                //Do nothing, use as is
            }
            else if (dataSource is IListSource listSource)
            {
                source = listSource.GetList();
            }

            var typedList = source as ITypedList ?? throw new ArgumentException("Data source must be a typed list", nameof(dataSource));

            var result = AppendTypedDataSource(worksheet, typedList, createTable, firstRowIndex, firstColumnIndex);
            return result;
        }

        public static Table AppendTypedDataSource(Worksheet worksheet, ITypedList dataSource, bool createTable = true,
            int firstRowIndex = 0, int firstColumnIndex = 0)
        { 
            var tableName     = GetUniqueTableName(worksheet.Workbook, "Table1");
            var propertyNames = new List<string>();
            foreach (PropertyDescriptor property in dataSource.GetItemProperties(null))
                propertyNames.Add(property.Name);	

            var options = new DataSourceImportOptions()
            {
                ImportFormulas = true,
                FormulaCulture = CultureInfo.InvariantCulture,
                PropertyNames  = propertyNames.ToArray()
            };

            var reader = new TypedListDataReader(dataSource);
            worksheet.Import(reader, true, firstRowIndex, firstColumnIndex, options);
            
            if (createTable)
            {
                var tableRange = worksheet.Range.FromLTRB(firstColumnIndex, firstRowIndex, firstColumnIndex + propertyNames.Count, firstRowIndex + reader.ProcessedRowCount + 1);

                //var result = worksheet.Tables.Add(worksheet.GetDataRange(), tableName, true);
                var result = worksheet.Tables.Add(tableRange, tableName, true);
                return result;
            }

            return null;
        }

        public static Table AppendDataTable(Worksheet worksheet, DataTable dataTable, bool createTable = true,
            int firstRowIndex = 0, int firstColumnIndex = 0)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));
            if (dataTable.Columns.Count <= 0)
                throw new Exception("DataTable shall contain at least one column.");

            var tableName = GetUniqueTableName(worksheet.Workbook, "Table");
            var options = new DataSourceImportOptions()
            {
                ImportFormulas = true,
                FormulaCulture = CultureInfo.InvariantCulture
            };

            worksheet.Import(dataTable, true, firstRowIndex, firstColumnIndex, options);

            if (createTable)
            {
                var tableRange = worksheet.Range.FromLTRB(firstColumnIndex, firstRowIndex, firstColumnIndex + dataTable.Columns.Count - 1, firstRowIndex + dataTable.Rows.Count);

                //var result = worksheet.Tables.Add(worksheet.GetDataRange(), tableName, true);
                var result = worksheet.Tables.Add(tableRange, tableName, true);
                return result;
            }

            return null;
        }

        public static Table AppendDataView(Worksheet worksheet, DataView dataView, bool createTable = true,
            int firstRowIndex = 0, int firstColumnIndex = 0)
        {
            var tableName = GetUniqueTableName(worksheet.Workbook, "Table");
            var options = new DataSourceImportOptions()
            {
                ImportFormulas = true,
                FormulaCulture = CultureInfo.InvariantCulture
            };

            using (var reader = new DataViewReader(dataView))
                worksheet.Import(reader, true, firstRowIndex, firstColumnIndex, options);

            if (createTable)
            {
                var tableRange = worksheet.Range.FromLTRB(firstColumnIndex, firstRowIndex, firstColumnIndex + dataView.Table.Columns.Count - 1, firstRowIndex + dataView.Table.Rows.Count);

                //var result = worksheet.Tables.Add(worksheet.GetDataRange(), tableName, true);
                var result = worksheet.Tables.Add(tableRange, tableName, true);
                return result;
            }

            return null;
        }

        public static void ApplyGridFormatting(Table table, GridData data, bool applyTableStyle = true)
        {
            var worksheet = table.Range.Worksheet;

            using (new UsingProcessor(() => worksheet.Workbook.BeginUpdate(), () => worksheet.Workbook.EndUpdate()))
            {
                var columnNames = new List<string>();
                foreach (var column in table.Columns)
                    columnNames.Add(column.Name);

                var positionConverter = new XlColumnPositionConverter(columnNames);
                var operatorConverter = new DevExpress.Export.Xl.CriteriaOperatorToXlExpressionConverter(positionConverter);

                if (data.ColumnOrder != null && data.ColumnOrder.Count > 0)
                {
                    bool hasSummaries = false;

                    foreach (var formatColumn in data.ColumnOrder)
                    {
                        if (!string.IsNullOrWhiteSpace(formatColumn.Summary))
                        {
                            hasSummaries = true;

                            var tableColumn = FindTableColumn(formatColumn.ColumnName);

                            var columnSummary = formatColumn.Summary;
                            //If there are multiple summaries - use first one only
                            var commaIndex = columnSummary.IndexOf(',');
                            if (commaIndex >= 0)
                                columnSummary = columnSummary.Substring(0, commaIndex);

                            if (tableColumn != null && (Enum.TryParse<SummaryItemType>(columnSummary, out SummaryItemType summaryType)))
                            {
                                switch (summaryType)
                                {
                                    case SummaryItemType.Sum:
                                        tableColumn.TotalRowFunction = TotalRowFunction.Sum;
                                        break;
                                    case SummaryItemType.Min:
                                        tableColumn.TotalRowFunction = TotalRowFunction.Min;
                                        break;
                                    case SummaryItemType.Max:
                                        tableColumn.TotalRowFunction = TotalRowFunction.Max;
                                        break;
                                    case SummaryItemType.Count:
                                        tableColumn.TotalRowFunction = TotalRowFunction.Count;
                                        break;
                                    case SummaryItemType.Average:
                                        tableColumn.TotalRowFunction = TotalRowFunction.Average;
                                        break;
                                }
                            }
                        }
                    }

                    if (hasSummaries)
                        table.ShowTotals = true;
                }


                var sortFields = new List<SortField>();

                int groupColumnCount = 0;

                if (data.GroupBy != null && data.GroupBy.Count > 0)
                {
                    foreach (var group in data.GroupBy)
                    {
                        if (string.IsNullOrWhiteSpace(group.ColumnName))
                            continue;

                        var sortAscending = true;
                        if (string.Compare(group.SortOrder, "desc", true) == 0)
                            sortAscending = false;

                        var columnIndex = positionConverter.GetColumnIndex(group.ColumnName);
                        if (columnIndex >= 0 && columnIndex < table.Columns.Count)
                        {
                            var colSortField = new SortField()
                            {
                                ColumnOffset = columnIndex,
                                Comparer     = sortAscending ? worksheet.Comparers.Ascending : worksheet.Comparers.Descending
                            };
                            sortFields.Add(colSortField);
                            groupColumnCount++;
                        }
                    }
                }

                if (data.OrderBy != null && data.OrderBy.Count > 0)
                {
                    foreach (var orderBy in data.OrderBy)
                    {
                        if (string.IsNullOrWhiteSpace(orderBy.ColumnName))
                            continue;

                        var sortAscending = true;
                        if (string.Compare(orderBy.SortOrder, "desc", true) == 0)
                            sortAscending = false;

                        var columnIndex = positionConverter.GetColumnIndex(orderBy.ColumnName);
                        if (columnIndex >= 0 && columnIndex < table.Columns.Count)
                        {
                            var colSortField = new SortField()
                            {
                                ColumnOffset = columnIndex,
                                Comparer     = sortAscending ? worksheet.Comparers.Ascending : worksheet.Comparers.Descending
                            };
                            sortFields.Add(colSortField);
                            groupColumnCount++;
                        }
                    }
                }

                if (sortFields.Count > 0)
                    table.Range.Worksheet.Sort(table.DataRange, sortFields);

                if (data.FormatConditions != null && data.FormatConditions.Count > 0)
                {
                    //Remove conditional formatting that was applied to whole table, leave other formattings.
                    //If there was no conditional formatting specified - leave existing ones.
                    for (int i = worksheet.ConditionalFormattings.Count-1; i >= 0; i--)
                    {
                        if (worksheet.ConditionalFormattings[i].Range == table.DataRange)
                            worksheet.ConditionalFormattings.RemoveAt(i);
                    }

                    foreach (var formatCondition in data.FormatConditions)
                    {
                        var columnIndex = positionConverter.GetColumnIndex(formatCondition.ColumnName);
                        var column      = columnIndex >= 0 && columnIndex < table.Columns.Count ? table.Columns[columnIndex] : null;
                        var formatRange = column != null ? column.DataRange : table.DataRange;  //apply format to whole table if column is not specified

                        ApplyGridFormatCondition(worksheet, formatCondition, formatRange, operatorConverter);
                    }
                }

                FinishTableFormatting(table, applyTableStyle);
            }


            TableColumn FindTableColumn(string name) =>
                table.Columns.FirstOrDefault(c => string.Compare(c.Name, name, true) == 0);
        }
    }
}
