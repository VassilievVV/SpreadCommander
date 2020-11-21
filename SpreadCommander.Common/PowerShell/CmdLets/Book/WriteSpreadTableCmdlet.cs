using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Spreadsheet;
using SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "SpreadTable")]
    public class WriteSpreadTableCmdlet: BaseBookWithCommentsCmdlet
    {
        [Parameter(HelpMessage = "Spreadsheet filename to import table or range from")]
        [Alias("SpreadFile")]
        public string SpreadsheetFile { get; set; }

        [Parameter(HelpMessage = "Spreadsheet to import table or range from. Spreadsheet must be created with cmdlet New-Spreadsheet")]
        public SCSpreadsheetContext Spreadsheet { get; set; }

        [Parameter(HelpMessage = "Table name of defined name or range to import content from")]
        [Alias("Table", "Range")]
        public string TableName { get; set; }


        protected override bool NeedSynchronization() => base.NeedSynchronization() || 
            (Spreadsheet != null && Spreadsheet == HostSpreadsheet) || 
            (Spreadsheet == null && string.IsNullOrWhiteSpace(SpreadsheetFile));


        protected override void ProcessRecord()
        {
            var book = GetCmdletBook();
            string htmlTable = null;

            if ((Spreadsheet == null && !string.IsNullOrWhiteSpace(SpreadsheetFile)) || Spreadsheet != HostSpreadsheet)
                htmlTable = GenerateTableHtml();

            ExecuteSynchronized(() =>
            {
                if (htmlTable == null)
                    htmlTable = GenerateTableHtml();
                DoWriteSpreadTable(book, htmlTable);
            });
        }
        
        protected virtual string GenerateTableHtml()
        {
            string htmlTable;
            IWorkbook workbook   = null;
            bool disposeWorkbook = false;
            try
            {
                if (Spreadsheet != null)
                    workbook = Spreadsheet.Workbook;
                else if (!string.IsNullOrWhiteSpace(SpreadsheetFile))
                {
                    disposeWorkbook = true;

                    var fileName = Project.Current.MapPath(SpreadsheetFile);
                    if (!File.Exists(fileName))
                        throw new Exception($"File '{SpreadsheetFile}' does not exist.");

                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.LoadDocument(fileName);
                }
                else
                    workbook = HostSpreadsheet;

                if (workbook == null)
                    throw new Exception("Spreadsheet is not specified");

                var range = SpreadsheetUtils.GetWorkbookRange(workbook, TableName);

                var options = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
                {
                    SheetIndex = workbook.Sheets.IndexOf(range.Worksheet),
                    Range      = range.GetReferenceA1(),
                    Encoding   = Encoding.Unicode
                };

                using var stream = new MemoryStream();
                workbook.ExportToHtml(stream, options);
                stream.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(stream, Encoding.UTF8);
                htmlTable = reader.ReadToEnd();
            }
            finally
            {
                if (disposeWorkbook)
                    workbook?.Dispose();
            }

            return htmlTable;
        }

    protected virtual void DoWriteSpreadTable(Document book, string htmlTable)
        { 
            var range = book.AppendHtmlText(htmlTable, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
            var paragraph = book.Paragraphs.Append();

            book.CaretPosition = paragraph.Range.End;
            ScrollToCaret();

            AddComments(book, range);

            WriteRangeToConsole(book, range);
        }
    }
}
