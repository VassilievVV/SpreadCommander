#pragma warning disable CRR0047

using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Spreadsheet;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    #region TableStyleId
    public enum TableStyleId
    {
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
        Medium1  = 22,
        Medium2  = 23,
        Medium3  = 24,
        Medium4  = 25,
        Medium5  = 26,
        Medium6  = 27,
        Medium7  = 28,
        Medium8  = 29,
        Medium9  = 30,
        Medium10 = 31,
        Medium11 = 32,
        Medium12 = 33,
        Medium13 = 34,
        Medium14 = 35,
        Medium15 = 36,
        Medium16 = 37,
        Medium17 = 38,
        Medium18 = 39,
        Medium19 = 40,
        Medium20 = 41,
        Medium21 = 42,
        Medium22 = 43,
        Medium23 = 44,
        Medium24 = 45,
        Medium25 = 46,
        Medium26 = 47,
        Medium27 = 48,
        Medium28 = 49,
        Dark1    = 50,
        Dark2    = 51,
        Dark3    = 52,
        Dark4    = 53,
        Dark5    = 54,
        Dark6    = 55,
        Dark7    = 56,
        Dark8    = 57,
        Dark9    = 58,
        Dark10   = 59,
        Dark11   = 60
    }
    #endregion

    #region SubtotalFunction
    public enum SubtotalFunction {
        Average = 1,
        Count   = 2,
        CountA  = 3,
        Max     = 4,
        Min     = 5,
        Product = 6,
        StdDev  = 7,
        StdDevp = 8,
        Sum     = 9,
        Var     = 10,
        Varp    = 11
    }
    #endregion

    [Cmdlet(VerbsData.Out, "SpreadTable")]
    public class OutSpreadTableCmdlet: BaseSpreadsheetWithCopyToBookCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "Sheet name for the data source. Unique sheet name will be generated if sheet already exists or in case of multiple data sources")]
        [Alias("Sheet")]
        public string SheetName { get; set; }

        [Parameter(HelpMessage = "Table name")]
        [Alias("Table")]
        public string TableName { get; set; }

        [Parameter(HelpMessage = "Replace existing sheet if it exists")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Format conditions for spreadsheet table")]
        public string Formatting { get; set; }

        [Parameter(HelpMessage = "Table style")]
        [Alias("Style")]
        public TableStyleId? TableStyle { get; set; }

        [Parameter(HelpMessage = "Row index of the start cell in which the imported data will be inserted")]
        [Alias("TopRow", "FirstRow", "TopRowIndex")]
        public int FirstRowIndex { get; set; }

        [Parameter(HelpMessage = "Column index of the start cell in which the imported data will be inserted")]
        [Alias("LeftColumn", "FirstColumn", "TopColumnIndex")]
        public int FirstColumnIndex { get; set; }

        [Parameter(HelpMessage = "Freeze top row")]
        public SwitchParameter FreezeTopRow { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Convert table into rage. This property is required to add SubTotals.")]
        public SwitchParameter AsRange { get; set; }

        [Parameter(HelpMessage = "Subtotal GroupBby column. Requires AsRange to be set.")]
        public string SubtotalGroupBy { get; set; }

        [Parameter(HelpMessage = "Subtotal columns. Require AsRanges to be set.")]
        public string[] SubtotalColumns { get; set; }

        [Parameter(HelpMessage = "Subtotal function. Default is SUM. Requires AsRange to be set.")]
        [DefaultValue(SubtotalFunction.Sum)]
        [PSDefaultValue(Value = SubtotalFunction.Sum)]
        public SubtotalFunction SubtotalFunction { get; set; } = SubtotalFunction.Sum;

        [Parameter(HelpMessage = "If set - subtotal function will ignore hidden values.")]
        public SwitchParameter SubtotalIgnoreHiddenValues { get; set; }

        [Parameter(HelpMessage = "A string that defines the text to be displayed in the subtotal summary rows.")]
        [DefaultValue("Total")]
        [PSDefaultValue(Value = "Total")]
        public string SubtotalFunctionText { get; set; } = "Total";

        [Parameter(HelpMessage = "Calculated columns to add to the table. Formulas are in format '[CalcColumn]=[Column1]*[Column2]'.")]
        public string[] CalculatedColumns { get; set; }

        [Parameter(HelpMessage = "Format string used to display numeric values (e.g., dates or times). Hashtable shall contain column names as keys and format string as values.")]
        public Hashtable ColumnNumberFormats { get; set; }

        [Parameter(HelpMessage = "Make all content visible within a cell by displaying it on multiple lines.")]
        public SwitchParameter WrapText { get; set; }

        [Parameter(HelpMessage = "Column widths in characters. Hashtable shall contain column names as keys and widths as values.")]
        public Hashtable ColumnWidths { get; set; }

        [Parameter(HelpMessage = "Horizontal alignment of values in data area.")]
        public SpreadsheetHorizontalAlignment? HorizontalAlignment { get; set; }

        [Parameter(HelpMessage = "Vertical alignment of values in data area.")]
        public SpreadsheetVerticalAlignment? VerticalAlignment { get; set; }
        
        [Parameter(HelpMessage = "If set - sheet will be removed after generating table. This can be used, for example, together with CopyBook, to copy a table into a Book and then remove sheet.")]
        public SwitchParameter TemporarySheet { get; set; }


        private readonly List<PSObject> _Output = new List<PSObject>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Output.Add(obj);
        }

        protected override void EndProcessing()
        {
            WriteTable();

            if (PassThru)
                WriteObject(_Output, true);
        }

        protected void WriteTable()
        {
            WriteTable(GetCmdletSpreadsheet());
        }

        protected void WriteTable(IWorkbook spreadsheet)
        {
            var dataSource = GetDataSource(_Output, DataSource, new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns });

            ExecuteSynchronized(() => DoWriteTable(spreadsheet, dataSource));
        }

        protected virtual void DoWriteTable(IWorkbook spreadsheet, object dataSource)
        {
            Worksheet worksheet = null;

            using (new UsingProcessor(() => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
            {
                if (!string.IsNullOrWhiteSpace(SheetName))
                {
                    var sheet = spreadsheet.Worksheets.Where(s => string.Compare(s.Name, SheetName, true) == 0).FirstOrDefault();
                    if (sheet != null)
                    {
                        if (!Replace)
                            throw new Exception($"Cannot create spreadsheet table: sheet '{SheetName}' already exists.");

                        var sheetName = sheet.Name;

                        worksheet = spreadsheet.Worksheets.Insert(sheet.Index);
                        spreadsheet.Worksheets.Remove(sheet);

                        worksheet.Name = sheetName;
                    }
                }

                if (worksheet == null)
                {
                    var newSheetName = !string.IsNullOrWhiteSpace(SheetName) ? SheetName :
                        Utils.AddUniqueString(spreadsheet.Worksheets.Select(sheet => sheet.Name).ToList(),
                            "Sheet1", StringComparison.CurrentCultureIgnoreCase, false);

                    if (spreadsheet.Worksheets.Count == 1 && IsRangeEmpty(spreadsheet.Worksheets[0].GetUsedRange()))
                    {
                        worksheet = spreadsheet.Worksheets[0];
                        worksheet.Name = newSheetName;
                    }
                    else
                        worksheet = spreadsheet.Worksheets.Add(newSheetName);
                }

                var table = SpreadsheetUtils.AppendDataSource(worksheet, dataSource, true, Math.Max(FirstRowIndex, 0), Math.Max(FirstColumnIndex, 0));
                if (!string.IsNullOrWhiteSpace(TableName))
                    table.Name = TableName;

                if (FreezeTopRow)
                    worksheet.FreezeRows(0);

                if (table.Range != null)
                    AddComments(table.Range);

                if (!string.IsNullOrWhiteSpace(Formatting))
                {
                    var scanner = new Scanner();
                    var parser = new Parser(scanner);

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
                        var gridData = new GridData();
                        gridData.ApplyGridFormatConditions(commands);
                        SpreadsheetUtils.ApplyGridFormatting(table, gridData, false);
                    }
                }

                if (CalculatedColumns != null && CalculatedColumns.Length > 0)
                {
                    for (int i = 0; i < CalculatedColumns.Length; i++)
                    {
                        var calculatedColumn = CalculatedColumns[i];
                        if (string.IsNullOrWhiteSpace(calculatedColumn))
                            continue;

                        var p = calculatedColumn.IndexOf('=');
                        if (p < 0)
                        {
                            ReportError("Cannot add calculated column: cannot get column name. Calculated columns have format '[CalcColumn1]=[Column1]*[Column2]'.");
                            continue;
                        }

                        var columnName    = calculatedColumn.Substring(0, p - 1).Trim();
                        var columnFormula = calculatedColumn.Substring(p + 1).Trim();

                        var column     = table.Columns.Add();
                        column.Name    = columnName;
                        column.Formula = columnFormula;
                    }
                }

                var columns = new List<string>();
                foreach (var column in table.Columns)
                    columns.Add(column.Name);

                if (ColumnNumberFormats != null && ColumnNumberFormats.Count > 0)
                {
                    foreach (DictionaryEntry columnNumberFormat in ColumnNumberFormats)
                    {
                        var columnName   = Convert.ToString(columnNumberFormat.Key);
                        var columnFormat = Convert.ToString(columnNumberFormat.Value);

                        int columnIndex = GetColumnIndex(columns, columnName);
                        if (columnIndex >= 0)
                            table.Columns[columnIndex].DataRange.NumberFormat = columnFormat;
                    }
                }

                if (ColumnWidths != null && ColumnWidths.Count > 0)
                { 
                     foreach (DictionaryEntry columnWidth in ColumnWidths)
                    {
                        var columnName = Convert.ToString(columnWidth.Key);
                        var width      = Convert.ToDouble(columnWidth.Value);

                        int columnIndex = GetColumnIndex(columns, columnName);
                        if (columnIndex >= 0)
                            table.Columns[columnIndex].DataRange.ColumnWidthInCharacters = width;
                    }               
                }

                if (WrapText)
                    table.DataRange.Alignment.WrapText = true;
                if (HorizontalAlignment.HasValue)
                    table.DataRange.Alignment.Horizontal = HorizontalAlignment.Value;
                if (VerticalAlignment.HasValue)
                    table.DataRange.Alignment.Vertical = VerticalAlignment.Value;

                if (TableStyle.HasValue)
                    table.Style = spreadsheet.TableStyles[(BuiltInTableStyleId)TableStyle.Value];

                if (AsRange)
                {
                    var tableRange     = table.Range;
                    var tableDataRange = table.DataRange;
                    table.ConvertToRange();

                    if (!string.IsNullOrWhiteSpace(table.Name))
                        tableRange.Name = table.Name;

                    worksheet.AutoFilter.Apply(tableRange);

                    if (!string.IsNullOrWhiteSpace(SubtotalGroupBy))
                    {
                        try
                        {
                            int iSubtotalGroupBy = GetColumnIndex(columns, SubtotalGroupBy);
                            if (iSubtotalGroupBy < 0)
                                throw new Exception($"Cannot find subtotal group GroupBy column '{SubtotalGroupBy}'.");

                            var iSubtotalColumns = new List<int>();
                            if (SubtotalColumns != null && SubtotalColumns.Length > 0)
                            {
                                foreach (var subtotalColumn in SubtotalColumns)
                                {
                                    var iSubtotalColumn = GetColumnIndex(columns, subtotalColumn);
                                    if (iSubtotalColumn >= 0)
                                        iSubtotalColumns.Add(iSubtotalColumn);
                                    else
                                        ReportError($"Cannot find subtotal column '{subtotalColumn}'.");
                                }
                            }

                            var iSubtotalFunction = Convert.ToInt32(SubtotalFunction);
                            if (iSubtotalFunction < 1 || iSubtotalFunction > 11)
                                iSubtotalFunction = 9;
                            if (SubtotalIgnoreHiddenValues)
                                iSubtotalFunction += 100;

                            worksheet.Subtotal(tableDataRange, iSubtotalGroupBy, iSubtotalColumns, iSubtotalFunction, SubtotalFunctionText);
                        }
                        catch (Exception ex)
                        {
                            ReportError(ex);
                        }
                    }
                }
            }

            CopyRangeToBook(worksheet.GetDataRange());

            if (TemporarySheet)
            {
                if (spreadsheet.Worksheets.Count <= 1)
                    spreadsheet.Worksheets.Add();
                spreadsheet.Worksheets.Remove(worksheet);
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
