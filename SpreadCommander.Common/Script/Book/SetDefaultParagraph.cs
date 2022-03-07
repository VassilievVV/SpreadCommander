using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public partial class SCBook
    {
        public void SetDefaultParagraph(ParagraphStyleOptions options) =>
            ExecuteSynchronized(options, () => DoSetDefaultParagraph(options));

        protected virtual void DoSetDefaultParagraph(ParagraphStyleOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
                SetParagraphOptions(book.DefaultParagraphProperties, options);
        }
    }
}
