using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class TableOptions: SpreadsheetWithCopyToBookOptions
    {
        [Description("Sheet name for the data source. Unique sheet name will be generated if sheet already exists or in case of multiple data sources")]
        public string SheetName { get; set; }

        [Description("Table name")]
        public string TableName { get; set; }

        [Description("Replace existing sheet if it exists")]
        public bool Replace { get; set; }

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values")]
        public bool IgnoreErrors { get; set; }

        [Description("Deedle frame keys.")]
        public string[] DeedleFrameKeys { get; set; }

        [Description("Format conditions for spreadsheet table")]
        public string Formatting { get; set; }

        [Description("Table style")]
        public TableStyleId? TableStyle { get; set; }

        [Description("Row index of the start cell in which the imported data will be inserted")]
        public int FirstRowIndex { get; set; }

        [Description("Column index of the start cell in which the imported data will be inserted")]
        public int FirstColumnIndex { get; set; }

        [Description("Freeze top row")]
        public bool FreezeTopRow { get; set; }

        [Description("Convert table into rage. This property is required to add SubTotals.")]
        public bool AsRange { get; set; }

        [Description("Subtotal GroupBby column. Requires AsRange to be set.")]
        public string SubtotalGroupBy { get; set; }

        [Description("Subtotal columns. Require AsRanges to be set.")]
        public string[] SubtotalColumns { get; set; }

        [Description("Subtotal function. Default is SUM. Requires AsRange to be set.")]
        [DefaultValue(SubtotalFunction.Sum)]
        public SubtotalFunction SubtotalFunction { get; set; } = SubtotalFunction.Sum;

        [Description("If set - subtotal function will ignore hidden values.")]
        public bool SubtotalIgnoreHiddenValues { get; set; }

        [Description("A string that defines the text to be displayed in the subtotal summary rows.")]
        [DefaultValue("Total")]
        public string SubtotalFunctionText { get; set; } = "Total";

        [Description("Calculated columns to add to the table. Formulas are in format '[CalcColumn]=[Column1]*[Column2]'.")]
        public string[] CalculatedColumns { get; set; }

        [Description("Format string used to display numeric values (e.g., dates or times). Hashtable shall contain column names as keys and format string as values.")]
        public Hashtable ColumnNumberFormats { get; set; }

        [Description("Make all content visible within a cell by displaying it on multiple lines.")]
        public bool WrapText { get; set; }

        [Description("Column widths in characters. Hashtable shall contain column names as keys and widths as values.")]
        public Hashtable ColumnWidths { get; set; }

        [Description("Horizontal alignment of values in data area.")]
        public SpreadsheetHorizontalAlignment? HorizontalAlignment { get; set; }

        [Description("Vertical alignment of values in data area.")]
        public SpreadsheetVerticalAlignment? VerticalAlignment { get; set; }

        [Description("If set - sheet will be removed after generating table. This can be used, for example, together with CopyBook, to copy a table into a Book and then remove sheet.")]
        public bool TemporarySheet { get; set; }
    }

    public partial class SCSpreadsheet
    {
        public void OutTable(object dataSource, TableOptions options = null) =>
            ExecuteSynchronized(options, () => DoOutTable(dataSource, options));

        protected virtual void DoOutTable(object dataSource, TableOptions options)
        {
            options ??= new TableOptions();

            var tableDataSource = GetDataSource(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns, DeedleFrameKeys = options.DeedleFrameKeys });

            var spread          = options?.Spreadsheet?.Workbook ?? Workbook;
            Worksheet worksheet = null;
            options             ??= new TableOptions();

            using (new UsingProcessor(() => spread.BeginUpdate(), () => spread.EndUpdate()))
            {
                if (!string.IsNullOrWhiteSpace(options.SheetName))
                {
                    var sheet = spread.Worksheets.Where(s => string.Compare(s.Name, options.SheetName, true) == 0).FirstOrDefault();
                    if (sheet != null)
                    {
                        if (!options.Replace)
                            throw new Exception($"Cannot create spreadsheet table: sheet '{options.SheetName}' already exists.");

                        var sheetName = sheet.Name;

                        worksheet = spread.Worksheets.Insert(sheet.Index);
                        spread.Worksheets.Remove(sheet);

                        worksheet.Name = sheetName;
                    }
                }

                if (worksheet == null)
                {
                    var newSheetName = !string.IsNullOrWhiteSpace(options.SheetName) ? options.SheetName :
                        Utils.AddUniqueString(spread.Worksheets.Select(sheet => sheet.Name).ToList(),
                            "Sheet1", StringComparison.CurrentCultureIgnoreCase, false);

                    if (spread.Worksheets.Count == 1 && IsRangeEmpty(spread.Worksheets[0].GetUsedRange()))
                    {
                        worksheet = spread.Worksheets[0];
                        worksheet.Name = newSheetName;
                    }
                    else
                        worksheet = spread.Worksheets.Add(newSheetName);
                }

                var table = SpreadsheetUtils.AppendDataSource(worksheet, tableDataSource, true, Math.Max(options.FirstRowIndex, 0), Math.Max(options.FirstColumnIndex, 0));
                if (!string.IsNullOrWhiteSpace(options.TableName))
                    table.Name = options.TableName;

                if (options.FreezeTopRow)
                    worksheet.FreezeRows(0);

                if (table.Range != null)
                    AddComments(table.Range, options.Comment);

                if (!string.IsNullOrWhiteSpace(options.Formatting))
                {
                    var scanner = new Scanner();
                    var parser  = new Parser(scanner);

                    var tree = parser.Parse(options.Formatting);
                    if (tree.Errors.Count > 0)
                    {
                        var strErrors = new StringBuilder();

                        foreach (var error in tree.Errors)
                        {
                            if (strErrors.Length > 0)
                                strErrors.AppendLine();
                            strErrors.Append(error.Message);
                        }

                        throw new Exception(strErrors.ToString());
                    }

                    List<BaseCommand> commands = null;
                    try
                    {
                        commands = tree.Eval() as List<BaseCommand>;
                    }
                    catch (Exception)
                    {
                        //Do nothing, skip invalid commands
                    }

                    if (commands != null)
                    {
                        var gridData = new GridData();
                        gridData.ApplyGridFormatConditions(commands);
                        SpreadsheetUtils.ApplyGridFormatting(table, gridData, false);
                    }
                }

                if (options.CalculatedColumns != null && options.CalculatedColumns.Length > 0)
                {
                    for (int i = 0; i < options.CalculatedColumns.Length; i++)
                    {
                        var calculatedColumn = options.CalculatedColumns[i];
                        if (string.IsNullOrWhiteSpace(calculatedColumn))
                            continue;

                        var p = calculatedColumn.IndexOf('=');
                        if (p < 0)
                        {
                            ReportError("Cannot add calculated column: cannot get column name. Calculated columns have format '[CalcColumn1]=[Column1]*[Column2]'.");
                            continue;
                        }

                        var columnName    = calculatedColumn[..(p - 1)].Trim();
                        var columnFormula = calculatedColumn[(p + 1)..].Trim();

                        var column = table.Columns.Add();
                        column.Name = columnName;
                        column.Formula = columnFormula;
                    }
                }

                var columns = new List<string>();
                foreach (var column in table.Columns)
                    columns.Add(column.Name);

                if (options.ColumnNumberFormats != null && options.ColumnNumberFormats.Count > 0)
                {
                    foreach (DictionaryEntry columnNumberFormat in options.ColumnNumberFormats)
                    {
                        var columnName   = Convert.ToString(columnNumberFormat.Key);
                        var columnFormat = Convert.ToString(columnNumberFormat.Value);

                        int columnIndex = GetColumnIndex(columns, columnName);
                        if (columnIndex >= 0)
                            table.Columns[columnIndex].DataRange.NumberFormat = columnFormat;
                    }
                }

                if (options.ColumnWidths != null && options.ColumnWidths.Count > 0)
                {
                    foreach (DictionaryEntry columnWidth in options.ColumnWidths)
                    {
                        var columnName = Convert.ToString(columnWidth.Key);
                        var width = Convert.ToDouble(columnWidth.Value);

                        int columnIndex = GetColumnIndex(columns, columnName);
                        if (columnIndex >= 0)
                            table.Columns[columnIndex].DataRange.ColumnWidthInCharacters = width;
                    }
                }

                if (options.WrapText)
                    table.DataRange.Alignment.WrapText = true;
                if (options.HorizontalAlignment.HasValue)
                    table.DataRange.Alignment.Horizontal = (DevExpress.Spreadsheet.SpreadsheetHorizontalAlignment)options.HorizontalAlignment.Value;
                if (options.VerticalAlignment.HasValue)
                    table.DataRange.Alignment.Vertical = (DevExpress.Spreadsheet.SpreadsheetVerticalAlignment)options.VerticalAlignment.Value;

                if (options.TableStyle.HasValue)
                    table.Style = spread.TableStyles[(BuiltInTableStyleId)options.TableStyle.Value];

                if (options.AsRange)
                {
                    var tableRange     = table.Range;
                    var tableDataRange = table.DataRange;
                    table.ConvertToRange();

                    if (!string.IsNullOrWhiteSpace(table.Name))
                        tableRange.Name = table.Name;

                    worksheet.AutoFilter.Apply(tableRange);

                    if (!string.IsNullOrWhiteSpace(options.SubtotalGroupBy))
                    {
                        try
                        {
                            int iSubtotalGroupBy = GetColumnIndex(columns, options.SubtotalGroupBy);
                            if (iSubtotalGroupBy < 0)
                                throw new Exception($"Cannot find subtotal group GroupBy column '{options.SubtotalGroupBy}'.");

                            var iSubtotalColumns = new List<int>();
                            if (options.SubtotalColumns != null && options.SubtotalColumns.Length > 0)
                            {
                                foreach (var subtotalColumn in options.SubtotalColumns)
                                {
                                    var iSubtotalColumn = GetColumnIndex(columns, subtotalColumn);
                                    if (iSubtotalColumn >= 0)
                                        iSubtotalColumns.Add(iSubtotalColumn);
                                    else
                                        ReportError($"Cannot find subtotal column '{subtotalColumn}'.");
                                }
                            }

                            var iSubtotalFunction = Convert.ToInt32(options.SubtotalFunction);
                            if (iSubtotalFunction < 1 || iSubtotalFunction > 11)
                                iSubtotalFunction = 9;
                            if (options.SubtotalIgnoreHiddenValues)
                                iSubtotalFunction += 100;

                            worksheet.Subtotal(tableDataRange, iSubtotalGroupBy, iSubtotalColumns, iSubtotalFunction, options.SubtotalFunctionText);
                        }
                        catch (Exception ex)
                        {
                            ReportError(ex);
                        }
                    }
                }
            }

            CopyRangeToBook(worksheet.GetDataRange(), options);


            if (options.TemporarySheet)
            {
                if (spread.Worksheets.Count <= 1)
                    spread.Worksheets.Add();
                spread.Worksheets.Remove(worksheet);
            }


            static bool IsRangeEmpty(CellRange range)
            {
                if (range.TopRowIndex == 0 && range.BottomRowIndex == 0 &&
                    range.LeftColumnIndex == 0 && range.RightColumnIndex == 0)
                    return true;
                return false;
            }

            static int GetColumnIndex(List<string> columns, string name)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (string.Compare(columns[i], name, true) == 0)
                        return i;
                }

                return -1;
            }
        }
    }
}
