using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.Script.Spreadsheet;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpreadCommander.Common.Script.Book
{
    public class DataTableOptions : CommentOptions
    {
        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values")]
        public bool IgnoreErrors { get; set; }

        [Description("Format conditions for spreadsheet table")]
        public string Formatting { get; set; }

        [Description("Table style")]
        public TableStyleId? TableStyle { get; set; }

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
        public Spreadsheet.SpreadsheetHorizontalAlignment? HorizontalAlignment { get; set; }

        [Description("Vertical alignment of values in data area.")]
        public Spreadsheet.SpreadsheetVerticalAlignment? VerticalAlignment { get; set; }

        [Description("Alignment of a table as a whole within the document.")]
        public TableRowAlignment? TableAlignment { get; set; }

        [Description("Whether to auto-fit table width in book's window.")]
        public bool AutoFitTableWidth { get; set; }
    }

    public partial class SCBook
    {
        public void WriteDataTable(object dataSource, DataTableOptions options = null) =>
            ExecuteSynchronized(options, () => DoWriteDataTable(dataSource, options));

        protected void DoWriteDataTable(object dataSource, DataTableOptions options)
        {
            options ??= new DataTableOptions();

            var book = options.Book?.Document ?? Document;
            options ??= new DataTableOptions();

            var tableDataSource = GetDataSource(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns });

            string htmlTable;

            using (var spreadsheet = SpreadsheetUtils.CreateWorkbook())
            {
                Worksheet worksheet;

                using (new UsingProcessor(
                    () => spreadsheet.BeginUpdate(), () => spreadsheet.EndUpdate()))
                {
                    worksheet = spreadsheet.Worksheets[0];

                    var table = SpreadsheetUtils.AppendDataSource(worksheet, tableDataSource, true);

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

                            var column     = table.Columns.Add();
                            column.Name    = columnName;
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
                            var columnName = Convert.ToString(columnNumberFormat.Key);
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
                        table.Style = spreadsheet.TableStyles[(BuiltInTableStyleId)options.TableStyle.Value];

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

                var exportOptions = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
                {
                    SheetIndex = spreadsheet.Sheets.IndexOf(worksheet),
                    Range      = worksheet.GetDataRange().GetReferenceA1(),
                    Encoding   = Encoding.Unicode
                };

                using var stream = new MemoryStream();
                spreadsheet.ExportToHtml(stream, exportOptions);
                stream.Seek(0, SeekOrigin.Begin);

                using var streamReader = new StreamReader(stream, Encoding.UTF8);
                htmlTable = streamReader.ReadToEnd();
            }

            ExecuteSynchronized(options, () => DoWriteHtml(book, options, htmlTable));


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

        protected virtual void DoWriteHtml(Document book, DataTableOptions options, string htmlTable)
        {
            var documentRange = book.AppendHtmlText(htmlTable, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

            if (options.AutoFitTableWidth || options.TableAlignment.HasValue)
            {
                var tables = book.Tables.Get(documentRange);
                foreach (var table in tables)
                {
                    if (options.AutoFitTableWidth)
                    {
                        table.TableLayout = TableLayoutType.Autofit;

                        foreach (var row in table.Rows)
                            foreach (var cell in row.Cells)
                                cell.PreferredWidthType = WidthType.FiftiethsOfPercent;

                        table.PreferredWidthType = WidthType.FiftiethsOfPercent;
                        table.PreferredWidth = 5000;
                    }

                    if (options.TableAlignment.HasValue)
                        table.TableAlignment = (DevExpress.XtraRichEdit.API.Native.TableRowAlignment)options.TableAlignment.Value;
                }
            }

            var paragraph = book.Paragraphs.Append();

            book.CaretPosition = paragraph.Range.End;
            ScrollToCaret();

            Script.Book.SCBook.AddComments(book, documentRange, options);
        }
    }
}
