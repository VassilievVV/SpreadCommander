using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.Script.Spreadsheet;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Spreadsheet;
using Svg;
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
    public class SpreadTableOptions : CommentOptions
    {
        internal Spreadsheet.SCSpreadsheet _Spreadsheet;
        internal string _FileName;
        internal string _TableName;
    }

    public partial class SCBook
    {
        public void WriteSpreadTable(string fileName, string tableName, SpreadTableOptions options = null)                       => DoWriteSpreadTable(null, fileName, tableName, options); 
        public void WriteSpreadTable(Spreadsheet.SCSpreadsheet spreadsheet, string tableName, SpreadTableOptions options = null) => DoWriteSpreadTable(spreadsheet, null, tableName, options); 

        protected virtual void DoWriteSpreadTable(Spreadsheet.SCSpreadsheet spreadsheet, string fileName, string tableName, SpreadTableOptions options)
        {
            options ??= new SpreadTableOptions();

            options._Spreadsheet = spreadsheet;
            options._FileName    = fileName;
            options._TableName   = tableName;

            var book = options.Book?.Document ?? Document;

            string htmlTable = null;

            if ((spreadsheet == null && !string.IsNullOrWhiteSpace(fileName)) || spreadsheet != Host?.Spreadsheet)
                htmlTable = GenerateTableHtml(options);

            ExecuteSynchronized(() =>
            {
                if (htmlTable == null)
                    htmlTable = GenerateTableHtml(options);
                DoWriteSpreadTable(book, options, htmlTable);
            });
        }

        protected virtual string GenerateTableHtml(SpreadTableOptions options)
        {
            string htmlTable;
            IWorkbook workbook   = null;
            bool disposeWorkbook = false;
            try
            {
                if (options._Spreadsheet != null)
                    workbook = options._Spreadsheet.Workbook;
                else if (!string.IsNullOrWhiteSpace(options._FileName))
                {
                    disposeWorkbook = true;

                    string fileName = Project.Current.MapPath(options._FileName);
                    if (!File.Exists(fileName))
                        throw new Exception($"File '{fileName}' does not exist.");

                    workbook = SpreadsheetUtils.CreateWorkbook();
                    workbook.LoadDocument(fileName);
                }
                else
                    workbook = Host?.Spreadsheet?.Workbook;

                if (workbook == null)
                    throw new Exception("Spreadsheet is not specified");

                var range = SpreadsheetUtils.GetWorkbookRange(workbook, options._TableName);

                var optionsHtml = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
                {
                    SheetIndex = workbook.Sheets.IndexOf(range.Worksheet),
                    Range      = range.GetReferenceA1(),
                    Encoding   = Encoding.Unicode
                };

                using var stream = new MemoryStream();
                workbook.ExportToHtml(stream, optionsHtml);
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

        protected virtual void DoWriteSpreadTable(Document book, SpreadTableOptions options, string htmlTable)
        {
            var range     = book.AppendHtmlText(htmlTable, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
            var paragraph = book.Paragraphs.Append();

            book.CaretPosition = paragraph.Range.End;
            Script.Book.SCBook.ResetBookFormatting(book, book.CaretPosition);
            ScrollToCaret();

            Script.Book.SCBook.AddComments(book, range, options);

            WriteRangeToConsole(book, range);
        }
    }
}
