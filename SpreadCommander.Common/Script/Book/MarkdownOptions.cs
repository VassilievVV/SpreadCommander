using DevExpress.XtraRichEdit.API.Native;
using Markdig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class MarkdownOptions : CommentOptions
    {
        [Description("Add line breaks after each line or no.")]
        public bool NoLineBreaks { get; set; }

        [Description("Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Description("Convert '{...}' blocks into fields")]
        public bool ExpandFields { get; set; }

        [Description("Snippets contains Books, Spreadsheets, Images and are used with Fields.")]
        public Hashtable Snippets { get; set; }
    }

    public partial class SCBook
    {
        public SCBook WriteMarkdown(string text, MarkdownOptions options = null)
        {
            ExecuteSynchronized(options, () => DoWriteMarkdown(text, options));
            return this;
        }

        protected void DoWriteMarkdown(string text, MarkdownOptions options)
        {
            options ??= new MarkdownOptions();

            var book = options.Book?.Document ?? Document;

            book.BeginUpdate();
            try
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                var html = Markdown.ToHtml(text);

                var lineRange = book.AppendHtmlText(html, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

                if (rangeStart == null)
                    rangeStart = lineRange.Start;

                if (!options.NoLineBreaks)
                    lineRange = book.AppendText(Environment.NewLine);

                rangeEnd = lineRange.End;

                if (rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());

                    if (!string.IsNullOrWhiteSpace(options.ParagraphStyle))
                    {
                        var style = book.ParagraphStyles[options.ParagraphStyle] ?? throw new Exception($"Paragraph style '{options.ParagraphStyle}' does not exist.");
                        var pp = book.BeginUpdateParagraphs(range);
                        try
                        {
                            pp.Style = style;
                        }
                        finally
                        {
                            book.EndUpdateParagraphs(pp);
                        }
                    }

                    Script.Book.SCBook.AddComments(book, range, options);

                    if (options.ExpandFields)
                        ExpandFieldsInBookRange(range, Host?.Spreadsheet?.Workbook, options.Snippets);

                    WriteRangeToConsole(book, range);
                }

                if (rangeEnd != null)
                {
                    book.CaretPosition = rangeEnd;
                    Script.Book.SCBook.ResetBookFormatting(book, rangeEnd);
                    ScrollToCaret();
                }
            }
            finally
            {
                book.EndUpdate();
            }
        }
    }
}
