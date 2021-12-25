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
        public SCBook SetSectionHeader(string text, HeaderFooterOptions options = null)
        {
            ExecuteSynchronized(options, () => DoSetSectionHeader(text, options));
            return this;
        }

        protected virtual void DoSetSectionHeader(string text, HeaderFooterOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
                SetHeaderFooterOptions(book, HeaderFooterKind.Header, text, options);
        }
    }
}
