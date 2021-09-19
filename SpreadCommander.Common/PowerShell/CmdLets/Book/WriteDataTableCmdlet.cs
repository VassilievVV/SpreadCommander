using DevExpress.XtraRichEdit.API.Native;
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
using SpreadCommander.Common.PowerShell.CmdLets.Book;
using System.Collections;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsCommunications.Write, "DataTable")]
    public class WriteDataTableCmdlet : BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Format conditions for spreadsheet table")]
        public string Formatting { get; set; }

        [Parameter(HelpMessage = "Table style")]
        [Alias("Style")]
        public TableStyleId? TableStyle { get; set; }

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

        [Parameter(HelpMessage = "Alignment of a table as a whole within the document.")]
        public TableRowAlignment? TableAlignment { get; set; }

        [Parameter(HelpMessage = "Whether to auto-fit table width in book's window.")]
        public SwitchParameter AutoFitTableWidth { get; set; }


        private readonly List<PSObject> _Output = new();

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
            WriteTable(GetCmdletBook());
        }

        protected void WriteTable(Document book)
        {
            var dataSource = GetDataSource(_Output, DataSource, 
                new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns });

            string htmlTable;

            using (var spreadsheet = SpreadsheetUtils.CreateWorkbook())
            {
                Worksheet worksheet;

                using (new UsingProcessor(
                    () => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
                {
                    worksheet = spreadsheet.Worksheets[0];

                    var table = SpreadsheetUtils.AppendDataSource(worksheet, dataSource, true);

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

                            var columnName = calculatedColumn.Substring(0, p - 1).Trim();
                            var columnFormula = calculatedColumn[(p + 1)..].Trim();

                            var column = table.Columns.Add();
                            column.Name = columnName;
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
                            var columnName  = Convert.ToString(columnNumberFormat.Key);
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
                            var width = Convert.ToDouble(columnWidth.Value);

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

                var options = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
                {
                    SheetIndex = spreadsheet.Sheets.IndexOf(worksheet),
                    Range      = worksheet.GetDataRange().GetReferenceA1(),
                    Encoding   = Encoding.Unicode
                };

                using var stream = new MemoryStream();
                spreadsheet.ExportToHtml(stream, options);
                stream.Seek(0, SeekOrigin.Begin);

                using var streamReader = new StreamReader(stream, Encoding.UTF8);
                htmlTable = streamReader.ReadToEnd();
            }

            ExecuteSynchronized(() => DoWriteHtml(book, htmlTable));


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

        protected virtual void DoWriteHtml(Document book, string htmlTable)
        {
            var documentRange = book.AppendHtmlText(htmlTable, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

            if (AutoFitTableWidth || TableAlignment.HasValue)
            {
                var tables = book.Tables.Get(documentRange);
                foreach (var table in tables)
                {
                    if (AutoFitTableWidth)
                    {
                        table.TableLayout = TableLayoutType.Autofit;

                        foreach (var row in table.Rows)
                            foreach (var cell in row.Cells)
                                cell.PreferredWidthType = WidthType.FiftiethsOfPercent;

                        table.PreferredWidthType = WidthType.FiftiethsOfPercent;
                        table.PreferredWidth     = 5000;
                    }

                    if (TableAlignment.HasValue)
                        table.TableAlignment = TableAlignment.Value;
                }
            }

            var paragraph = book.Paragraphs.Append();
            
            book.CaretPosition = paragraph.Range.End;
            ScrollToCaret();

            AddComments(book, documentRange);
        }
    }
}
