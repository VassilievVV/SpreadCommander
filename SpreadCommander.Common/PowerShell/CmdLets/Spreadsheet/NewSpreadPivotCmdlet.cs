#pragma warning disable CRR0047

using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    #region PivotTableStyleId
    public enum PivotTableStyleId
    {
        None     = 0,
        Light1   = 1,
        Light2   = 2,
        Light3   = 3,
        Light4   = 4,
        Light5   = 5,
        Light6   = 6,
        Light7   = 7,
        Light8   = 8,
        Light9   = 9,
        Light10  = 10,
        Light11  = 11,
        Light12  = 12,
        Light13  = 13,
        Light14  = 14,
        Light15  = 15,
        Light16  = 16,
        Light17  = 17,
        Light18  = 18,
        Light19  = 19,
        Light20  = 20,
        Light21  = 21,
        Light22  = 22,
        Light23  = 23,
        Light24  = 24,
        Light25  = 25,
        Light26  = 26,
        Light27  = 27,
        Light28  = 28,
        Medium1  = 29,
        Medium2  = 30,
        Medium3  = 31,
        Medium4  = 32,
        Medium5  = 33,
        Medium6  = 34,
        Medium7  = 35,
        Medium8  = 36,
        Medium9  = 37,
        Medium10 = 38,
        Medium11 = 39,
        Medium12 = 40,
        Medium13 = 41,
        Medium14 = 42,
        Medium15 = 43,
        Medium16 = 44,
        Medium17 = 45,
        Medium18 = 46,
        Medium19 = 47,
        Medium20 = 48,
        Medium21 = 49,
        Medium22 = 50,
        Medium23 = 51,
        Medium24 = 52,
        Medium25 = 53,
        Medium26 = 54,
        Medium27 = 55,
        Medium28 = 56,
        Dark1    = 57,
        Dark2    = 58,
        Dark3    = 59,
        Dark4    = 60,
        Dark5    = 61,
        Dark6    = 62,
        Dark7    = 63,
        Dark8    = 64,
        Dark9    = 65,
        Dark10   = 66,
        Dark11   = 67,
        Dark12   = 68,
        Dark13   = 69,
        Dark14   = 70,
        Dark15   = 71,
        Dark16   = 72,
        Dark17   = 73,
        Dark18   = 74,
        Dark19   = 75,
        Dark20   = 76,
        Dark21   = 77,
        Dark22   = 78,
        Dark23   = 79,
        Dark24   = 80,
        Dark25   = 81,
        Dark26   = 82,
        Dark27   = 83,
        Dark28   = 84
    }
    #endregion

    #region PivotShowValueAsItemType
    public enum PivotShowValueAsItemType
    {
        Previous = 1,
        Next     = 2
    }
    #endregion

    [Cmdlet(VerbsCommon.New, "SpreadPivot")]
    public class NewSpreadPivotCmdlet: BaseSpreadsheetWithCopyToBookCmdlet
    {
        [Parameter(HelpMessage = "Name of sheet with data. If not specified - DataTableName is searching in all sheets.")]
        public string DataSheetName { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Name of table with data.")]
        public string DataTableName { get; set; }

        [Parameter(HelpMessage = "Name of new sheet with pivot table.")]
        public string PivotSheetName { get; set; }

        [Parameter(HelpMessage = "Name of pivot table.")]
        public string PivotTableName { get; set; }

        [Parameter(HelpMessage = "Replace existing sheet if it exists")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "List of row fields.")]
        public string[] RowFields { get; set; }

        [Parameter(HelpMessage = "List of column fields.")]
        public string[] ColumnFields { get; set; }

        [Parameter(HelpMessage = "List of page/filter fields.")]
        public string[] PageFields { get; set; }

        [Parameter(HelpMessage = "List of data fields.")]
        public string[] DataFields { get; set; }

        [Parameter(HelpMessage = "List of calculated row fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedRowFields { get; set; }

        [Parameter(HelpMessage = "List of calculated column fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedColumnFields { get; set; }

        [Parameter(HelpMessage = "List of calculated page/filter fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedPageFields { get; set; }

        [Parameter(HelpMessage = "List of calculated data fields in format '[CalcColumn1]=[Column1]*[Column2]'.")]
        public string[] CalculatedDataFields { get; set; }

        [Parameter(HelpMessage = "List of number formats for row fields.")]
        public string[] RowFieldNumberFormats { get; set; }

        [Parameter(HelpMessage = "List of number formats for column fields.")]
        public string[] ColumnFieldNumberFormats { get; set; }

        [Parameter(HelpMessage = "List of number formats for page/filter fields.")]
        public string[] PageFieldNumberFormats { get; set; }

        [Parameter(HelpMessage = "List of number formats for data fields.")]
        public string[] DataFieldNumberFormats { get; set; }

        [Parameter(HelpMessage = "Specifies how summary values should be displayed within the data field.")]
        public PivotShowValuesAsType[] ShowValuesAs { get; set; }

        [Parameter(HelpMessage = "Specifies a base field for a custom calculation. Shall be used with ShowValuesAs. Defaults to same field.")]
        public string[] ShowValuesAsBaseFields { get; set; }

        [Parameter(HelpMessage = "Specifies whether the previous or next item in the base field should be used as the basis for calculation. Shall be used with ShowValuesAs. Default to Previous.")]
        public PivotShowValueAsItemType[] ShowValuesAsBaseTypes { get; set; }

        [Parameter(HelpMessage = "Summary function used to calculate values in the data field.")]
        public PivotDataConsolidationFunction[] SummarizeValuesBy { get; set; }

        [Parameter(HelpMessage = "Format conditions for pivot table")]
        public string Formatting { get; set; }

        #region Layout
        [Parameter(HelpMessage = "Applies the specified report layout to a pivot table.")]
        public PivotReportLayout? Layout { get; set; }

        [Parameter(HelpMessage = "Value indicating whether new fields should have their Compact property set to true.")]
        public SwitchParameter CompactNewFields { get; set; }

        [Parameter(HelpMessage = "Value indicating whether multiple data fields in a pivot table should be displayed in rows down the report.")]
        public SwitchParameter DataOnRows { get; set; }

        [Parameter(HelpMessage = "Indent increment for items from different row fields when a pivot table is shown in compact form.")]
        [ValidateRange(0, 127)]
        public int? IndentInCompactForm { get; set; }

        [Parameter(HelpMessage = "Value indicating whether to merge and center cells containing item labels for the outer row and column fields, subtotal and grand total captions.")]
        public SwitchParameter MergeTitles { get; set; }

        [Parameter(HelpMessage = "Value indicating whether new fields should have their PivotFieldLayout.Outline property set to true.")]
        public SwitchParameter OutlineNewFields { get; set; }

        [Parameter(HelpMessage = "Order in which multiple page fields are displayed in the PivotTable report filter area.")]
        public PivotPageOrder? PageOrder { get; set; }

        [Parameter(HelpMessage = "Number of page fields to display before starting another column or row based on the PageOrder property value.")]
        [ValidateRange(0, 255)]
        public int? PageWrap { get; set; }

        [Parameter(HelpMessage = "Value indicating whether all totals columns should be hidden. Cumulative switch that can be used to replace other HideTotals switches.")]
        public SwitchParameter HideAllSubtotals { get; set; }

        [Parameter(HelpMessage = "Value indicating whether grand totals should be displayed for columns in the PivotTable report.")]
        public SwitchParameter HideColumnGrandTotals { get; set; }

        [Parameter(HelpMessage = "Value indicating whether grand totals should be displayed for rows in the PivotTable report.")]
        public SwitchParameter HideRowGrandTotals { get; set; }

        [Parameter(HelpMessage = "Value indicating whether hidden page field items in a pivot table should be included in subtotals and grand totals.")]
        public SwitchParameter SubtotalIncludeHiddenItems { get; set; }
        #endregion

        [Parameter(HelpMessage = "Value indicating whether to apply the style formatting to column headers.")]

        public SwitchParameter HideColumnHeaders { get; set; }

        [Parameter(HelpMessage = "Value indicating whether to apply the style formatting to row headers.")]
        public SwitchParameter HideRowHeaders { get; set; }

        [Parameter(HelpMessage = "Style applied to the pivot table.")]
        public PivotTableStyleId? Style { get; set; }

        #region View
        [Parameter(HelpMessage = "Alternative description of the information in a PivotTable report.")]
        public string AltTextDescription { get; set; }

        [Parameter(HelpMessage = "Alternative text for a PivotTable report.")]
        public string AltTextTitle { get; set; }

        [Parameter(HelpMessage = "Text to be displayed in the column header of a pivot table shown in compact form.")]
        public string ColumnHeaderCaption { get; set; }

        [Parameter(HelpMessage = "Caption for a virtual field named Data (Values in the UI) which appears in a pivot table containing two or more data fields.")]
        public string DataCaption { get; set; }

        [Parameter(HelpMessage = "Text to be displayed in cells that contain errors.")]
        public string ErrorCaption { get; set; }

        [Parameter(HelpMessage = "Text to be displayed in grand totals for rows and columns in a pivot table.")]
        public string GrandTotalCaption { get; set; }

        [Parameter(HelpMessage = "Text to be displayed in cells with no values.")]
        public string MissingCaption { get; set; }

        [Parameter(HelpMessage = "Text to be displayed in the row header of a pivot table shown in compact form.")]
        public string RowHeaderCaption { get; set; }

        [Parameter(HelpMessage = "Value indicating whether the expand/collapse buttons should be displayed in a pivot table.")]
        public SwitchParameter HideDrillIndicators { get; set; }

        [Parameter(HelpMessage = "Value indicating whether to show custom error messages in cells.")]
        public SwitchParameter ShowError { get; set; }

        [Parameter(HelpMessage = "Value indicating whether to display the row and column field captions and filter drop-down arrows in a pivot table.")]
        public SwitchParameter HideFieldHeaders { get; set; } 

        [Parameter(HelpMessage = "Value indicating whether to display a custom string in cells that contain no values.")]
        public SwitchParameter HideMissing { get; set; } 

        [Parameter(HelpMessage = "Value indicating whether to display the \"(Multiple Items)\" string in the report filter area when multiple items, but not all, are selected in a page field.")]
        public SwitchParameter HideMultipleLabels { get; set; } 

        [Parameter(HelpMessage = "Value indicating whether to display the Values row that may appear when there are multiple fields in the PivotTable data area.")]
        public SwitchParameter HideValuesRow { get; set; } 
        #endregion

        #region Behavior
        [Parameter(HelpMessage = "Value indicating whether fields in the pivot table can have multiple filters applied to them at the same time.")]
        public SwitchParameter AllowMultipleFieldFilters { get; set; }

        [Parameter(HelpMessage = "Value indicating whether column widths should be automatically resized when the pivot table is recalculated or refreshed.")]
        public SwitchParameter DontAutoFitColumns { get; set; }
        #endregion

        [Parameter(HelpMessage = "If set - only data range of the pivot table will be copied to Book")]
        public SwitchParameter CopyToBookDataOnly { get; set; }


        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
        }

        protected override void EndProcessing()
        {
            WritePivotTable();
        }

        protected void WritePivotTable()
        {
            WriteTable(GetCmdletSpreadsheet());
        }

        protected void WriteTable(IWorkbook spreadsheet)
        {
            ExecuteSynchronized(() => DoWriteTable(spreadsheet));
        }

        protected virtual void DoWriteTable(IWorkbook spreadsheet)
        {
            PivotTable pivotTable;

            using (new UsingProcessor(() => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
            {
                //Disable automatic show of pivot table field list.
                spreadsheet.DocumentSettings.ShowPivotTableFieldList = false;

                var table = FindDataTable();

                Worksheet pivotSheet = null;

                if (!string.IsNullOrWhiteSpace(PivotSheetName))
                {
                    for (int i = 0; i < spreadsheet.Worksheets.Count; i++)
                    {
                        var sheet = spreadsheet.Worksheets[i];

                        if (string.Compare(sheet.Name, PivotSheetName, true) == 0)
                        {
                            if (Replace)
                            {
                                spreadsheet.Worksheets.RemoveAt(i);
                                var newSheet = spreadsheet.Worksheets.Insert(i, PivotSheetName);
                                pivotSheet = newSheet;
                            }
                            else
                                throw new Exception($"Cannot create pivot sheet table: sheet '{PivotSheetName}' already exists.");

                            break;
                        }
                    }
                }

                if (pivotSheet == null)
                    pivotSheet = spreadsheet.Worksheets.Add(PivotSheetName);
                pivotTable = pivotSheet.PivotTables.Add(table.Range, pivotSheet[0, 0], PivotTableName);

                using (new UsingProcessor(() => pivotTable.BeginUpdate(), () => pivotTable.EndUpdate()))
                {
                    if (RowFields != null && RowFields.Length > 0)
                    {
                        foreach (var fieldName in RowFields)
                            pivotTable.RowFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (ColumnFields != null && ColumnFields.Length > 0)
                    {
                        foreach (var fieldName in ColumnFields)
                            pivotTable.ColumnFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (PageFields != null && PageFields.Length > 0)
                    {
                        foreach (var fieldName in PageFields)
                            pivotTable.PageFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (DataFields != null && DataFields.Length > 0)
                    {
                        foreach (var fieldName in DataFields)
                            pivotTable.DataFields.Add(pivotTable.Fields[fieldName]);
                    }

                    if (CalculatedRowFields != null && CalculatedRowFields.Length > 0)
                    {
                        foreach (var fieldDefinition in CalculatedRowFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.RowFields.Add(calcField);
                        }
                    }

                    if (CalculatedColumnFields != null && CalculatedColumnFields.Length > 0)
                    {
                        foreach (var fieldDefinition in CalculatedColumnFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.ColumnFields.Add(calcField);
                        }
                    }

                    if (CalculatedPageFields != null && CalculatedPageFields.Length > 0)
                    {
                        foreach (var fieldDefinition in CalculatedPageFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.PageFields.Add(calcField);
                        }
                    }

                    if (CalculatedDataFields != null && CalculatedDataFields.Length > 0)
                    {
                        foreach (var fieldDefinition in CalculatedDataFields)
                        {
                            var calcField = AddCalculatedField(pivotTable, fieldDefinition);
                            pivotTable.DataFields.Add(calcField);
                        }
                    }

                    if (RowFieldNumberFormats != null && RowFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(RowFieldNumberFormats.Length, pivotTable.RowFields.Count); i++)
                            pivotTable.RowFields[i].Field.NumberFormat = RowFieldNumberFormats[i];
                    }

                    if (ColumnFieldNumberFormats != null && ColumnFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(ColumnFieldNumberFormats.Length, pivotTable.ColumnFields.Count); i++)
                            pivotTable.ColumnFields[i].Field.NumberFormat = ColumnFieldNumberFormats[i];
                    }

                    if (PageFieldNumberFormats != null && PageFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(PageFieldNumberFormats.Length, pivotTable.PageFields.Count); i++)
                            pivotTable.PageFields[i].Field.NumberFormat = PageFieldNumberFormats[i];
                    }

                    if (DataFieldNumberFormats != null && DataFieldNumberFormats.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(DataFieldNumberFormats.Length, pivotTable.DataFields.Count); i++)
                            pivotTable.DataFields[i].Field.NumberFormat = DataFieldNumberFormats[i];
                    }

                    if (ShowValuesAs != null && ShowValuesAs.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(ShowValuesAs.Length, pivotTable.DataFields.Count); i++)
                        {
                            var baseFieldName = ShowValuesAsBaseFields != null && i < ShowValuesAsBaseFields.Length ? ShowValuesAsBaseFields[i] : null;
                            var baseType      = ShowValuesAsBaseTypes != null && i < ShowValuesAsBaseTypes.Length ? ShowValuesAsBaseTypes[i] : PivotShowValueAsItemType.Previous;

                            var baseField = !string.IsNullOrWhiteSpace(baseFieldName) ? pivotTable.Fields[baseFieldName] : null;
                            if (!string.IsNullOrWhiteSpace(baseFieldName) && baseField == null)
                                throw new Exception($"Cannot configure pivot table: cannot find base field '{baseFieldName}'.");

                            var dataField = pivotTable.DataFields[i];
                            dataField.ShowValuesWithCalculation(ShowValuesAs[i], baseField, (PivotBaseItemType)(int)baseType);
                        }
                    }

                    if (SummarizeValuesBy != null && SummarizeValuesBy.Length > 0)
                    {
                        for (int i = 0; i < Math.Min(SummarizeValuesBy.Length, pivotTable.DataFields.Count); i++)
                            pivotTable.DataFields[i].SummarizeValuesBy = SummarizeValuesBy[i];
                    }

                    pivotTable.Layout.SetReportLayout(Layout ?? PivotReportLayout.Compact);

                    if (CompactNewFields)
                        pivotTable.Layout.CompactNewFields = true;
                    if (DataOnRows)
                        pivotTable.Layout.DataOnRows = true;
                    if (IndentInCompactForm.HasValue)
                        pivotTable.Layout.IndentInCompactForm = IndentInCompactForm.Value;
                    if (MergeTitles)
                        pivotTable.Layout.MergeTitles = true;
                    if (OutlineNewFields)
                        pivotTable.Layout.OutlineNewFields = true;
                    if (PageOrder.HasValue)
                        pivotTable.Layout.PageOrder = PageOrder.Value;
                    if (PageWrap.HasValue)
                        pivotTable.Layout.PageWrap = PageWrap.Value;
                    if (HideAllSubtotals)
                    {
                        pivotTable.Layout.HideAllSubtotals();

                        pivotTable.Layout.ShowColumnGrandTotals = false;
                        pivotTable.Layout.ShowRowGrandTotals    = false;
                    }
                    if (HideColumnGrandTotals)
                        pivotTable.Layout.ShowColumnGrandTotals = false;
                    if (HideRowGrandTotals)
                        pivotTable.Layout.ShowRowGrandTotals = false;
                    if (SubtotalIncludeHiddenItems)
                        pivotTable.Layout.SubtotalIncludeHiddenItems = true;
                    if (HideColumnHeaders)
                        pivotTable.ShowColumnHeaders = false;
                    if (HideRowHeaders)
                        pivotTable.ShowRowHeaders = false;

                    if (Style != PivotTableStyleId.None)
                        pivotTable.Style = spreadsheet.TableStyles[(BuiltInPivotStyleId)Style];

                    if (AltTextDescription != null)
                        pivotTable.View.AltTextDescription = AltTextDescription;
                    if (AltTextTitle != null)
                        pivotTable.View.AltTextTitle = AltTextTitle;
                    if (ColumnHeaderCaption != null)
                        pivotTable.View.ColumnHeaderCaption = ColumnHeaderCaption;
                    if (DataCaption != null)
                        pivotTable.View.DataCaption = DataCaption;
                    if (ErrorCaption != null)
                        pivotTable.View.ErrorCaption = ErrorCaption;
                    if (GrandTotalCaption != null)
                        pivotTable.View.GrandTotalCaption = GrandTotalCaption;
                    if (MissingCaption != null)
                        pivotTable.View.MissingCaption = MissingCaption;
                    if (RowHeaderCaption != null)
                        pivotTable.View.RowHeaderCaption = RowHeaderCaption;
                    if (HideDrillIndicators)
                        pivotTable.View.ShowDrillIndicators = false;
                    if (ShowError)
                        pivotTable.View.ShowError = true;
                    if (HideFieldHeaders)
                        pivotTable.View.ShowFieldHeaders = false;
                    if (HideMissing)
                        pivotTable.View.ShowMissing = false;
                    if (HideMultipleLabels)
                        pivotTable.View.ShowMultipleLabels = false;
                    if (HideValuesRow)
                        pivotTable.View.ShowValuesRow = false;

                    if (AllowMultipleFieldFilters)
                        pivotTable.Behavior.AllowMultipleFieldFilters = true;
                    if (DontAutoFitColumns)
                        pivotTable.Behavior.AutoFitColumns = false;
                }

                if (!string.IsNullOrWhiteSpace(Formatting))
                {
                    var scanner = new Scanner();
                    var parser  = new Parser(scanner);

                    var tree = parser.Parse(Formatting);
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
                        var formatRange          = pivotTable.Location.DataRange;
                        var gridFormatConditions = GridData.LoadGridFormatConditions(commands);

                        foreach (var gridFormatCondition in gridFormatConditions)
                            SpreadsheetUtils.ApplyGridFormatCondition(pivotSheet, gridFormatCondition, formatRange);
                    }
                }
            }

            CopyRangeToBook(CopyToBookDataOnly ? pivotTable.Location.DataRange : pivotTable.Location.Range);
            AddComments(pivotTable.Location.WholeRange);


            DevExpress.Spreadsheet.Table FindDataTable()
            {
                Worksheet sheet                        = null;
                DevExpress.Spreadsheet.Table table     = null;

                if (!string.IsNullOrWhiteSpace(DataSheetName))
                {
                    sheet = spreadsheet.Worksheets[DataSheetName];
                    if (sheet == null)
                        throw new Exception($"Cannot find sheet '{DataSheetName}'.");
                }

                if (!string.IsNullOrWhiteSpace(DataTableName))
                {
                    if (sheet != null)
                        table = sheet.Tables.Where(t => string.Compare(t.Name, DataTableName, true) == 0).FirstOrDefault();
                    else
                    {
                        foreach (var worksheet in spreadsheet.Worksheets)
                        {
                            table = worksheet.Tables.Where(t => string.Compare(t.Name, DataTableName, true) == 0).FirstOrDefault();
                            if (table != null)
                                break;
                        }
                    }
                }

                if (table == null)
                    throw new Exception($"Cannot find table '{DataTableName}'.");

                return table;
            }

            static PivotField AddCalculatedField(PivotTable pvtTable, string fieldDefinition)
            {
                if (string.IsNullOrWhiteSpace(fieldDefinition))
                    throw new Exception("Calculated field cannot have empty definition.");

                int p = fieldDefinition.IndexOf('=');
                if (p < 0)
                    throw new Exception($"Invalid calculated field definition: '{fieldDefinition}'. Field definition shall be in form 'CalcColumn=Column1*Column2'.");

                var fieldName    = fieldDefinition.Substring(0, p - 1).Trim();
                var fieldFormula = fieldDefinition.Substring(p + 1).Trim();

                var result = pvtTable.CalculatedFields.Add(fieldFormula, fieldName);
                return result;
            }
        }
    }
}
