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
        public SCBook SetSectionFooter(string text, HeaderFooterOptions options = null)
        {
            ExecuteSynchronized(options, () => DoSetSectionFooter(text, options));
            return this;
        }

        protected virtual void DoSetSectionFooter(string text, HeaderFooterOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
                SetHeaderFooterOptions(book, HeaderFooterKind.Footer, text, options);
        }
    }
}
