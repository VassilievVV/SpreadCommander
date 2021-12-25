using DevExpress.XtraRichEdit.API.Native;
using Markdig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class ContentOptions : CommentOptions
    {
        [Description("Add line breaks after each line or no.")]
        public bool NoLineBreaks { get; set; }
    }

    public partial class SCBook
    {
        public SCBook WriteContent(string fileName, ContentOptions options = null)
        {
            ExecuteSynchronized(options, () => DoWriteContent(fileName, options));
            return this;
        }

        protected void DoWriteContent(string fileName, ContentOptions options)
        {
            options ??= new ContentOptions();

            var book = options.Book?.Document ?? Document;

            book.BeginUpdate();
            try
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                fileName = Project.Current.MapPath(fileName);

                DocumentRange range;

                var ext = Path.GetExtension(fileName)?.ToLower();
                switch (ext)
                {
#pragma warning disable CRRSP06 // A misspelled word has been found
                    case "markdown":
                    case "mdown":
                    case "md":
                        var content     = File.ReadAllText(fileName);
                        var htmlContent = Markdown.ToHtml(content);
                        range           = book.AppendHtmlText(htmlContent, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
                        break;
                    default:
                        range = book.AppendDocumentContent(fileName);
                        break;
#pragma warning restore CRRSP06 // A misspelled word has been found
                }

                if (rangeStart == null)
                    rangeStart = range.Start;

                if (!options.NoLineBreaks)
                    range = book.AppendText(Environment.NewLine);

                rangeEnd = range.End;

                if (rangeStart != null && rangeEnd != null)
                {
                    var rangeComment = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());
                    Script.Book.SCBook.AddComments(book, rangeComment, options);
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
