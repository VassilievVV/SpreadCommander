using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    public class BaseSpreadsheetWithCopyToBookCmdlet: BaseSpreadsheetCmdlet
    {
        [Parameter(HelpMessage = "When set - produced sheet will be copied to book.")]
        public SwitchParameter CopyToBook { get; set; }

        [Parameter(HelpMessage = "Book to copy table to. If NULL and CopyToBook is set - copy to host's Book.")]
        public Book.SCBookContext TargetBook { get; set; }

        [Parameter(HelpMessage = "Comment to add to Book")]
        public string BookComment { get; set; }

        [Parameter(HelpMessage = "If set - BookComment is treated as HTML")]
        public SwitchParameter BookCommentHtml { get; set; }

        [Parameter(HelpMessage = "Bookmark in the document")]
        public string BookBookmark { get; set; }

        [Parameter(HelpMessage = "Hyperlink to existing bookmark")]
        public string BookHyperlink { get; set; }

        [Parameter(HelpMessage = "Text for the tooltip displayed when the mouse hovers over a hyperlink")]
        public string BookHyperlinkTooltip { get; set; }

        [Parameter(HelpMessage = "Target window or frame in which to display the web page content when the hyperlink is clicked")]
        public string BookHyperlinkTarget { get; set; }


        protected void CopyRangeToBook(CellRange range)
        {
            CopyToBook |= (TargetBook != null);
            if (!CopyToBook)
                return;

            var book = TargetBook?.Document ?? HostBook;
            if (book == HostBook)
                base.ExecuteSynchronized(() => DoCopyRangeToBook(range, book));
            else
                DoCopyRangeToBook(range, book);
        }

        protected virtual void DoCopyRangeToBook(CellRange range, Document book)
        {
            string htmlTable;

            var sheet    = range.Worksheet;
            var workbook = sheet.Workbook;

            var options = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
            {
                SheetIndex = workbook.Sheets.IndexOf(sheet),
                Range      = range.Worksheet.GetDataRange().GetReferenceA1(),
                Encoding   = Encoding.Unicode
            };

            using (var stream = new MemoryStream())
            {
                workbook.ExportToHtml(stream, options);
                stream.Seek(0, SeekOrigin.Begin);

                using var reader = new StreamReader(stream, Encoding.UTF8);
                htmlTable = reader.ReadToEnd();
            }

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var documentRange = book.AppendHtmlText(htmlTable, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
                book.Paragraphs.Append();

                AddBookComments(book, documentRange);

                book.CaretPosition = documentRange.End;
                ScrollToCaret();
            }
        }

        protected void CopyImageToBook(Image image, float? scaleX, float? scaleY)
        {
            CopyToBook |= (TargetBook != null);
            if (!CopyToBook)
                return;

            var book = TargetBook?.Document ?? HostBook;
            if (book == HostBook)
                base.ExecuteSynchronized(() => DoCopyImageToBook(image, book, scaleX, scaleY));
            else
                DoCopyImageToBook(image, book, scaleX, scaleY);
        }

        protected virtual void DoCopyImageToBook(Image image, Document book, float? scaleX, float? scaleY)
        {
            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var docImage = book.Images.Append(image);

                if (scaleX.HasValue)
                    docImage.ScaleX = scaleX.Value;
                if (scaleY.HasValue)
                    docImage.ScaleY = scaleY.Value;

                book.Paragraphs.Append();

                AddBookComments(book, docImage.Range);

                book.CaretPosition = docImage.Range.End;
                ScrollToCaret();
            }
        }

        protected void AddBookComments(Document book, DocumentRange range)
        {
            var comment = BookComment;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var bookComment = book.Comments.Create(range, Environment.UserName);
                var docComment  = bookComment.BeginUpdate();
                try
                {
                    if (BookCommentHtml)
                        docComment.AppendHtmlText(comment, InsertOptions.KeepSourceFormatting);
                    else
                        docComment.AppendText(comment);
                }
                finally
                {
                    bookComment.EndUpdate(docComment);
                }
            }

            var bookmark = BookBookmark;
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                var oldBookmark = book.Bookmarks[bookmark];
                if (oldBookmark != null)
                    book.Bookmarks.Remove(oldBookmark);

                book.Bookmarks.Create(range, bookmark);
            }

            var hyperlink = BookHyperlink;
            if (!string.IsNullOrWhiteSpace(hyperlink))
            {
                var hyperlinkBookmark = book.Bookmarks[hyperlink];
                var link = book.Hyperlinks.Create(range);
                if (hyperlinkBookmark != null)
                    link.Anchor = hyperlinkBookmark.Name;
                else
                    link.NavigateUri = hyperlink;

                if (!string.IsNullOrWhiteSpace(BookHyperlinkTooltip))
                    link.ToolTip = BookHyperlinkTooltip;

                if (!string.IsNullOrWhiteSpace(BookHyperlinkTarget))
                    link.Target = BookHyperlinkTarget;
            }
        }

        protected void ScrollToCaret()
        {
            ExternalHost?.ScrollToCaret();
        }
    }
}
