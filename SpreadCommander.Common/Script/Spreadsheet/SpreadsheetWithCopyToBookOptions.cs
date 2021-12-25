using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class SpreadsheetWithCopyToBookOptions: SpreadsheetOptions
    {
        [Description("When set - produced sheet will be copied to book.")]
        public bool CopyToBook { get; set; }

        [Description("Book to copy table to. If NULL and CopyToBook is set - copy to host's Book.")]
        public Book.SCBook TargetBook { get; set; }

        [Description("Comment to add to Book")]
        public string BookComment { get; set; }

        [Description("If set - BookComment is treated as HTML")]
        public bool BookCommentHtml { get; set; }

        [Description("Bookmark in the document")]
        public string BookBookmark { get; set; }

        [Description("Hyperlink to existing bookmark")]
        public string BookHyperlink { get; set; }

        [Description("Text for the tooltip displayed when the mouse hovers over a hyperlink")]
        public string BookHyperlinkTooltip { get; set; }

        [Description("Target window or frame in which to display the web page content when the hyperlink is clicked")]
        public string BookHyperlinkTarget { get; set; }

        [Description("Whether to repeat a row as header at the top of each page.")]
        public bool RepeatAsHeaderRow { get; set; }

        [Description("Count of rows to repeat at the top of each page.")]
        [DefaultValue(1)]
        public int RepeatAsHeaderRowCount { get; set; } = 1;

        [Description("Whether the table row can break across pages.")]
        public bool BreakRowsAcrossPages { get; set; }
    }

    public partial class SCSpreadsheet
    {
        protected void CopyRangeToBook(CellRange range, SpreadsheetWithCopyToBookOptions options)
        {
            options.CopyToBook |= (options.TargetBook != null);
            if (!options.CopyToBook)
                return;

            var book = options.TargetBook?.Document ?? Host?.Book?.Document;
            if (book == Host?.Book?.Document)
                base.ExecuteSynchronized(() => DoCopyRangeToBook(range, book, options));
            else
                DoCopyRangeToBook(range, book, options);
        }

        protected virtual void DoCopyRangeToBook(CellRange range, Document book, SpreadsheetWithCopyToBookOptions options)
        {
            string htmlTable;

            var sheet    = range.Worksheet;
            var workbook = sheet.Workbook;

            var htmlOptions = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
            {
                SheetIndex = workbook.Sheets.IndexOf(sheet),
                Range      = range.Worksheet.GetDataRange().GetReferenceA1(),
                Encoding   = Encoding.Unicode
            };

            using (var stream = new MemoryStream())
            {
                workbook.ExportToHtml(stream, htmlOptions);
                stream.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(stream, Encoding.UTF8);
                htmlTable = reader.ReadToEnd();
            }

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var documentRange = book.AppendHtmlText(htmlTable, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
                book.Paragraphs.Append();

                if (options.RepeatAsHeaderRow || options.BreakRowsAcrossPages)
                {
                    options.RepeatAsHeaderRowCount = Utils.ValueInRange(options.RepeatAsHeaderRowCount, 1, 100);

                    var tables = book.Tables.Get(documentRange);
                    foreach (var table in tables)
                    {
                        using (new UsingProcessor(() => table.BeginUpdate(), () => table.EndUpdate()))
                        {
                            if (options.RepeatAsHeaderRow)
                            {
                                for (int i = 0; i < Math.Max(options.RepeatAsHeaderRowCount, table.Rows.Count); i++)
                                    table.Rows[i].RepeatAsHeaderRow = true;
                            }

                            if (options.BreakRowsAcrossPages)
                            {
                                for (int i = 0; i < table.Rows.Count; i++)
                                    table.Rows[i].BreakAcrossPages = true;
                            }
                        }
                    }
                }


                AddBookComments(book, documentRange, options);

                book.CaretPosition = documentRange.End;
                ScrollToCaret();
            }
        }

        protected void CopyImageToBook(Image image, float? scaleX, float? scaleY, SpreadsheetWithCopyToBookOptions options)
        {
            options.CopyToBook |= (options.TargetBook != null);
            if (!options.CopyToBook)
                return;

            var book = options.TargetBook?.Document ?? Host?.Book?.Document;
            if (book == Host?.Book?.Document)
                base.ExecuteSynchronized(() => DoCopyImageToBook(image, book, scaleX, scaleY, options));
            else
                DoCopyImageToBook(image, book, scaleX, scaleY, options);
        }

        protected virtual void DoCopyImageToBook(Image image, Document book, float? scaleX, float? scaleY, SpreadsheetWithCopyToBookOptions options)
        {
            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var docImage = book.Images.Append(image);

                if (scaleX.HasValue)
                    docImage.ScaleX = scaleX.Value;
                if (scaleY.HasValue)
                    docImage.ScaleY = scaleY.Value;

                book.Paragraphs.Append();

                AddBookComments(book, docImage.Range, options);

                book.CaretPosition = docImage.Range.End;
                Book.SCBook.ResetBookFormatting(book, book.CaretPosition);
                ScrollToCaret();
            }
        }

        protected static void AddBookComments(Document book, DocumentRange range, SpreadsheetWithCopyToBookOptions options)
        {
            var comment = options.BookComment;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var bookComment = book.Comments.Create(range, Environment.UserName);
                var docComment = bookComment.BeginUpdate();
                try
                {
                    if (options.BookCommentHtml)
                        docComment.AppendHtmlText(comment, InsertOptions.KeepSourceFormatting);
                    else
                        docComment.AppendText(comment);
                }
                finally
                {
                    bookComment.EndUpdate(docComment);
                }
            }

            var bookmark = options.BookBookmark;
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                var oldBookmark = book.Bookmarks[bookmark];
                if (oldBookmark != null)
                    book.Bookmarks.Remove(oldBookmark);

                book.Bookmarks.Create(range, bookmark);
            }

            var hyperlink = options.BookHyperlink;
            if (!string.IsNullOrWhiteSpace(hyperlink))
            {
                var hyperlinkBookmark = book.Bookmarks[hyperlink];
                var link = book.Hyperlinks.Create(range);
                if (hyperlinkBookmark != null)
                    link.Anchor = hyperlinkBookmark.Name;
                else
                    link.NavigateUri = hyperlink;

                if (!string.IsNullOrWhiteSpace(options.BookHyperlinkTooltip))
                    link.ToolTip = options.BookHyperlinkTooltip;

                if (!string.IsNullOrWhiteSpace(options.BookHyperlinkTarget))
                    link.Target = options.BookHyperlinkTarget;
            }
        }

        protected void ScrollToCaret()
        {
            Host?.Engine?.ScrollToCaret();
        }
    }
}
