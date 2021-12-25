using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class PivotOptions: SpreadsheetWithCopyToBookOptions
    {
        [Description("Name of sheet with data. If not specified - DataTableName is searching in all sheets.")]
        public string DataSheetName { get; set; }

        [Description("Name of table with data.")]
        public string DataTableName { get; set; }

        [Description("Name of new sheet with pivot table.")]
        public string PivotSheetName { get; set; }

        [Description("Name of pivot table.")]
        public string PivotTableName { get; set; }

        [Description("Replace existing sheet if it exists")]
        public bool Replace { get; set; }

        [Description("List of row fields.")]
        public string[] RowFields { get; set; }

        [Description("List of column fields.")]
        public string[] ColumnFields { get; set; }

        [Description("List of page/filter fields.")]
        public string[] PageFields { get; set; }

        [Description("List of data fields.")]
        public string[] DataFields { get; set; }

        [Description("List of calculated row fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedRowFields { get; set; }

        [Description("List of calculated column fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedColumnFields { get; set; }

        [Description("List of calculated page/filter fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedPageFields { get; set; }

        [Description("List of calculated data fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedDataFields { get; set; }

        [Description("List of number formats for row fields.")]
        public string[] RowFieldNumberFormats { get; set; }

        [Description("List of number formats for column fields.")]
        public string[] ColumnFieldNumberFormats { get; set; }

        [Description("List of number formats for page/filter fields.")]
        public string[] PageFieldNumberFormats { get; set; }

        [Description("List of number formats for data fields.")]
        public string[] DataFieldNumberFormats { get; set; }

        [Description("Specifies how summary values should be displayed within the data field.")]
        public PivotShowValuesAsType[] ShowValuesAs { get; set; }

        [Description("Specifies a base field for a custom calculation. Shall be used with ShowValuesAs. Defaults to same field.")]
        public string[] ShowValuesAsBaseFields { get; set; }

        [Description("Specifies whether the previous or next item in the base field should be used as the basis for calculation. Shall be used with ShowValuesAs. Default to Previous.")]
        public PivotShowValueAsItemType[] ShowValuesAsBaseTypes { get; set; }

        [Description("Summary function used to calculate values in the data field.")]
        public PivotDataConsolidationFunction[] SummarizeValuesBy { get; set; }

        [Description("Format conditions for pivot table")]
        public string Formatting { get; set; }

        #region Layout
        [Description("Applies the specified report layout to a pivot table.")]
        public PivotReportLayout? Layout { get; set; }

        [Description("Value indicating whether new fields should have their Compact property set to true.")]
        public bool CompactNewFields { get; set; }

        [Description("Value indicating whether multiple data fields in a pivot table should be displayed in rows down the report.")]
        public bool DataOnRows { get; set; }

        [Description("Indent increment for items from different row fields when a pivot table is shown in compact form.")]
        public int? IndentInCompactForm { get; set; }

        [Description("Value indicating whether to merge and center cells containing item labels for the outer row and column fields, subtotal and grand total captions.")]
        public bool MergeTitles { get; set; }

        [Description("Value indicating whether new fields should have their PivotFieldLayout.Outline property set to true.")]
        public bool OutlineNewFields { get; set; }

        [Description("Order in which multiple page fields are displayed in the PivotTable report filter area.")]
        public PivotPageOrder? PageOrder { get; set; }

        [Description("Number of page fields to display before starting another column or row based on the PageOrder property value.")]
        public int? PageWrap { get; set; }

        [Description("Value indicating whether all totals columns should be hidden. Cumulative switch that can be used to replace other HideTotals switches.")]
        public bool HideAllSubtotals { get; set; }

        [Description("Value indicating whether grand totals should be displayed for columns in the PivotTable report.")]
        public bool HideColumnGrandTotals { get; set; }

        [Description("Value indicating whether grand totals should be displayed for rows in the PivotTable report.")]
        public bool HideRowGrandTotals { get; set; }

        [Description("Value indicating whether hidden page field items in a pivot table should be included in subtotals and grand totals.")]
        public bool SubtotalIncludeHiddenItems { get; set; }
        #endregion

        [Description("Value indicating whether to apply the style formatting to column headers.")]

        public bool HideColumnHeaders { get; set; }

        [Description("Value indicating whether to apply the style formatting to row headers.")]
        public bool HideRowHeaders { get; set; }

        [Description("Style applied to the pivot table.")]
        public PivotTableStyleId? Style { get; set; }

        #region View
        [Description("Alternative description of the information in a PivotTable report.")]
        public string AltTextDescription { get; set; }

        [Description("Alternative text for a PivotTable report.")]
        public string AltTextTitle { get; set; }

        [Description("Text to be displayed in the column header of a pivot table shown in compact form.")]
        public string ColumnHeaderCaption { get; set; }

        [Description("Caption for a virtual field named Data (Values in the UI) which appears in a pivot table containing two or more data fields.")]
        public string DataCaption { get; set; }

        [Description("Text to be displayed in cells that contain errors.")]
        public string ErrorCaption { get; set; }

        [Description("Text to be displayed in grand totals for rows and columns in a pivot table.")]
        public string GrandTotalCaption { get; set; }

        [Description("Text to be displayed in cells with no values.")]
        public string MissingCaption { get; set; }

        [Description("Text to be displayed in the row header of a pivot table shown in compact form.")]
        public string RowHeaderCaption { get; set; }

        [Description("Value indicating whether the expand/collapse buttons should be displayed in a pivot table.")]
        public bool HideDrillIndicators { get; set; }

        [Description("Value indicating whether to show custom error messages in cells.")]
        public bool ShowError { get; set; }

        [Description("Value indicating whether to display the row and column field captions and filter drop-down arrows in a pivot table.")]
        public bool HideFieldHeaders { get; set; }

        [Description("Value indicating whether to display a custom string in cells that contain no values.")]
        public bool HideMissing { get; set; }

        [Description("Value indicating whether to display the \"(Multiple Items)\" string in the report filter area when multiple items, but not all, are selected in a page field.")]
        public bool HideMultipleLabels { get; set; }

        [Description("Value indicating whether to display the Values row that may appear when there are multiple fields in the PivotTable data area.")]
        public bool HideValuesRow { get; set; }
        #endregion

        #region Behavior
        [Description("Value indicating whether fields in the pivot table can have multiple filters applied to them at the same time.")]
        public bool AllowMultipleFieldFilters { get; set; }

        [Description("Value indicating whether column widths should be automatically resized when the pivot table is recalculated or refreshed.")]
        public bool DontAutoFitColumns { get; set; }
        #endregion

        [Description("If set - only data range of the pivot table will be copied to Book")]
        public bool CopyToBookDataOnly { get; set; }
    }

    public partial class SCSpreadsheet
    {
        public SCSpreadsheet NewPivot(object dataSource, PivotOptions options = null)
        {
            ExecuteSynchronized(options, () => DoNewPivot(dataSource, options));
            return this;
        }

        protected virtual void DoNewPivot(object dataSource, PivotOptions options)
        {
            options ??= new PivotOptions();
            var spread = options.Spreadsheet?.Workbook ?? Workbook;

            PivotTable pivotTable;

            using (new UsingProcessor(() => spread.BeginUpdate(), () => spread.EndUpdate()))
            {
                //Disable automatic show of pivot table field list.
                spread.DocumentSettings.ShowPivotTableFieldList = false;

                var table = FindDataTable();

                Worksheet pivotSheet = null;

                if (!string.IsNullOrWhiteSpace(options.PivotSheetName))
                {
                    for (int i = 0; i < spread.Worksheets.Count; i++)
                    {
                        var sheet = spread.Worksheets[i];

                        if (string.Compare(sheet.Name, options.PivotSheetName, true) == 0)
                        {
                            if (options.Replace)
                            {
                                spread.Worksheets.RemoveAt(i);
                                var newSheet = spread.Worksheets.Insert(i, options.PivotSheetName);
                                pivotSheet   = newSheet;
                            }
                            else
                                throw new Exception($"Cannot create pivot sheet table: sheet '{options.PivotSheetName}' already exists.");

                            break;
                        }
                    }
                }

                if (pivotSheet == null)
                    pivotSheet = spread.Worksheets.Add(options.PivotSheetName);
                pivotTable = pivotSheet.PivotTables.Add(table.Range, pivotSheet[0, 0], options.PivotTableName);

                using (new UsingProcessor(() => pivotTable.BeginUpdate(), () => pivotTable.EndUpdate()))
                {
                    if (options.RowFields != null && options.RowFields.Length > 0)
                    {
                        foreach (var fieldName in options.RowFields)
                            pivotTable.RowFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (options.ColumnFields != null && options.ColumnFields.Length > 0)
                    {
                        foreach (var fieldName in options.ColumnFields)
                            pivotTable.ColumnFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (options.PageFields != null && options.PageFields.Length > 0)
                    {
                        foreach (var fieldName in options.PageFields)
                            pivotTable.PageFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (options.DataFields != null && options.DataFields.Length > 0)
                    {
                        foreach (var fieldName in options.DataFields)
                            pivotTable.DataFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (options.CalculatedRowFields != null && options.CalculatedRowFields.Length > 0)
                    {
                        foreach (var fieldDefinition in options.CalculatedRowFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.RowFields.Add(calcField);
                        }
                    }

                    if (options.CalculatedColumnFields != null && options.CalculatedColumnFields.Length > 0)
                    {
                        foreach (var fieldDefinition in options.CalculatedColumnFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.ColumnFields.Add(calcField);
                        }
                    }

                    if (options.CalculatedPageFields != null && options.CalculatedPageFields.Length > 0)
                    {
                        foreach (var fieldDefinition in options.CalculatedPageFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.PageFields.Add(calcField);
                        }
                    }

                    if (options.CalculatedDataFields != null && options.CalculatedDataFields.Length > 0)
                    {
                        foreach (var fieldDefinition in options.CalculatedDataFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.DataFields.Add(calcField);
                        }
                    }

                    if (options.RowFieldNumberFormats != null && options.RowFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(options.RowFieldNumberFormats.Length, pivotTable.RowFields.Count); i++)
                            pivotTable.RowFields[i].Field.NumberFormat = options.RowFieldNumberFormats[i];
                    }

                    if (options.ColumnFieldNumberFormats != null && options.ColumnFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(options.ColumnFieldNumberFormats.Length, pivotTable.ColumnFields.Count); i++)
                            pivotTable.ColumnFields[i].Field.NumberFormat = options.ColumnFieldNumberFormats[i];
                    }

                    if (options.PageFieldNumberFormats != null && options.PageFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(options.PageFieldNumberFormats.Length, pivotTable.PageFields.Count); i++)
                            pivotTable.PageFields[i].Field.NumberFormat = options.PageFieldNumberFormats[i];
                    }

                    if (options.DataFieldNumberFormats != null && options.DataFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(options.DataFieldNumberFormats.Length, pivotTable.DataFields.Count); i++)
                            pivotTable.DataFields[i].Field.NumberFormat = options.DataFieldNumberFormats[i];
                    }

                    if (options.ShowValuesAs != null && options.ShowValuesAs.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(options.ShowValuesAs.Length, pivotTable.DataFields.Count); i++)
                        {
                            var baseFieldName = options.ShowValuesAsBaseFields != null && i < options.ShowValuesAsBaseFields.Length ? options.ShowValuesAsBaseFields[i] : null;
                            var baseType      = options.ShowValuesAsBaseTypes != null  && i < options.ShowValuesAsBaseTypes.Length  ? options.ShowValuesAsBaseTypes[i] : PivotShowValueAsItemType.Previous;

                            var baseField = !string.IsNullOrWhiteSpace(baseFieldName) ? pivotTable.Fields[baseFieldName] : null;
                            if (!string.IsNullOrWhiteSpace(baseFieldName) && baseField == null)
                                throw new Exception($"Cannot configure pivot table: cannot find base field '{baseFieldName}'.");

                            var dataField = pivotTable.DataFields[i];
                            dataField.ShowValuesWithCalculation((DevExpress.Spreadsheet.PivotShowValuesAsType)options.ShowValuesAs[i], baseField, (PivotBaseItemType)(int)baseType);
                        }
                    }

                    if (options.SummarizeValuesBy != null && options.SummarizeValuesBy.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(options.SummarizeValuesBy.Length, pivotTable.DataFields.Count); i++)
                            pivotTable.DataFields[i].SummarizeValuesBy = (DevExpress.Spreadsheet.PivotDataConsolidationFunction)options.SummarizeValuesBy[i];
                    }

                    pivotTable.Layout.SetReportLayout((DevExpress.Spreadsheet.PivotReportLayout)(options.Layout ?? PivotReportLayout.Compact));

                    if (options.CompactNewFields)
                        pivotTable.Layout.CompactNewFields = true;
                    if (options.DataOnRows)
                        pivotTable.Layout.DataOnRows = true;
                    if (options.IndentInCompactForm.HasValue)
                        pivotTable.Layout.IndentInCompactForm = options.IndentInCompactForm.Value;
                    if (options.MergeTitles)
                        pivotTable.Layout.MergeTitles = true;
                    if (options.OutlineNewFields)
                        pivotTable.Layout.OutlineNewFields = true;
                    if (options.PageOrder.HasValue)
                        pivotTable.Layout.PageOrder = (DevExpress.Spreadsheet.PivotPageOrder)options.PageOrder.Value;
                    if (options.PageWrap.HasValue)
                        pivotTable.Layout.PageWrap = options.PageWrap.Value;
                    if (options.HideAllSubtotals)
                    {
                        pivotTable.Layout.HideAllSubtotals();

                        pivotTable.Layout.ShowColumnGrandTotals = false;
                        pivotTable.Layout.ShowRowGrandTotals    = false;
                    }
                    if (options.HideColumnGrandTotals)
                        pivotTable.Layout.ShowColumnGrandTotals = false;
                    if (options.HideRowGrandTotals)
                        pivotTable.Layout.ShowRowGrandTotals = false;
                    if (options.SubtotalIncludeHiddenItems)
                        pivotTable.Layout.SubtotalIncludeHiddenItems = true;
                    if (options.HideColumnHeaders)
                        pivotTable.ShowColumnHeaders = false;
                    if (options.HideRowHeaders)
                        pivotTable.ShowRowHeaders = false;

                    if (options.Style != PivotTableStyleId.None)
                        pivotTable.Style = spread.TableStyles[(BuiltInPivotStyleId)options.Style];

                    if (options.AltTextDescription != null)
                        pivotTable.View.AltTextDescription = options.AltTextDescription;
                    if (options.AltTextTitle != null)
                        pivotTable.View.AltTextTitle = options.AltTextTitle;
                    if (options.ColumnHeaderCaption != null)
                        pivotTable.View.ColumnHeaderCaption = options.ColumnHeaderCaption;
                    if (options.DataCaption != null)
                        pivotTable.View.DataCaption = options.DataCaption;
                    if (options.ErrorCaption != null)
                        pivotTable.View.ErrorCaption = options.ErrorCaption;
                    if (options.GrandTotalCaption != null)
                        pivotTable.View.GrandTotalCaption = options.GrandTotalCaption;
                    if (options.MissingCaption != null)
                        pivotTable.View.MissingCaption = options.MissingCaption;
                    if (options.RowHeaderCaption != null)
                        pivotTable.View.RowHeaderCaption = options.RowHeaderCaption;
                    if (options.HideDrillIndicators)
                        pivotTable.View.ShowDrillIndicators = false;
                    if (options.ShowError)
                        pivotTable.View.ShowError = true;
                    if (options.HideFieldHeaders)
                        pivotTable.View.ShowFieldHeaders = false;
                    if (options.HideMissing)
                        pivotTable.View.ShowMissing = false;
                    if (options.HideMultipleLabels)
                        pivotTable.View.ShowMultipleLabels = false;
                    if (options.HideValuesRow)
                        pivotTable.View.ShowValuesRow = false;

                    if (options.AllowMultipleFieldFilters)
                        pivotTable.Behavior.AllowMultipleFieldFilters = true;
                    if (options.DontAutoFitColumns)
                        pivotTable.Behavior.AutoFitColumns = false;
                }

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
                        var formatRange = pivotTable.Location.DataRange;
                        var gridFormatConditions = GridData.LoadGridFormatConditions(commands);

                        foreach (var gridFormatCondition in gridFormatConditions)
                            SpreadsheetUtils.ApplyGridFormatCondition(pivotSheet, gridFormatCondition, formatRange);
                    }
                }
            }

            CopyRangeToBook(options.CopyToBookDataOnly ? pivotTable.Location.DataRange : pivotTable.Location.Range, options);
            AddComments(pivotTable.Location.WholeRange, options.Comment);


            DevExpress.Spreadsheet.Table FindDataTable()
            {
                Worksheet sheet = null;
                DevExpress.Spreadsheet.Table table = null;

                if (!string.IsNullOrWhiteSpace(options.DataSheetName))
                {
                    sheet = spread.Worksheets[options.DataSheetName];
                    if (sheet == null)
                        throw new Exception($"Cannot find sheet '{options.DataSheetName}'.");
                }

                if (!string.IsNullOrWhiteSpace(options.DataTableName))
                {
                    if (sheet != null)
                        table = sheet.Tables.Where(t => string.Compare(t.Name, options.DataTableName, true) == 0).FirstOrDefault();
                    else
                    {
                        foreach (var worksheet in spread.Worksheets)
                        {
                            table = worksheet.Tables.Where(t => string.Compare(t.Name, options.DataTableName, true) == 0).FirstOrDefault();
                            if (table != null)
                                break;
                        }
                    }
                }

                if (table == null)
                    throw new Exception($"Cannot find table '{options.DataTableName}'.");

                return table;
            }

            static PivotField AddCalculatedField(PivotTable pvtTable, string fieldDefinition)
            {
                if (string.IsNullOrWhiteSpace(fieldDefinition))
                    throw new Exception("Calculated field cannot have empty definition.");

                int p = fieldDefinition.IndexOf('=');
                if (p < 0)
                    throw new Exception($"Invalid calculated field definition: '{fieldDefinition}'. Field definition shall be in form 'CalcColumn=Column1*Column2'.");

                var fieldName    = fieldDefinition[..(p - 1)].Trim();
                var fieldFormula = fieldDefinition[(p + 1)..].Trim();

                var result = pvtTable.CalculatedFields.Add(fieldFormula, fieldName);
                return result;
            }
        }
    }
}
