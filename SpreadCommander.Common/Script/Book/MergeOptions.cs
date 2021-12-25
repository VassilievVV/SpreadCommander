using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class MergeOptions: CommentOptions
    {
    }

    public partial class SCBook
    {
        public SCBook Merge(SCBook anotherBook, MergeOptions options = null)
        {
            ExecuteSynchronized(options, () => DoMerge(anotherBook, options));
            return this;
        }

        protected virtual void DoMerge(SCBook anotherBook, MergeOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                anotherBook.Document.UpdateAllFields();

                DocumentRange range;
                using (var stream = new MemoryStream())
                {
                    anotherBook.Document.SaveDocument(stream, DocumentFormat.OpenXml);
                    stream.Seek(0, SeekOrigin.Begin);
                    range = book.AppendDocumentContent(stream, DocumentFormat.OpenXml);
                }

                if (rangeStart == null)
                    rangeStart = range.Start;

                rangeEnd = range.End;

                if (rangeStart != null && rangeEnd != null)
                {
                    var rangeComments = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());
                    AddComments(book, rangeComments, options);
                }

                book.CaretPosition = rangeEnd;
                Script.Book.SCBook.ResetBookFormatting(book, rangeEnd);
                ScrollToCaret();
            }
        }
    }
}
