using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class ClearOptions: BookOptions
    {
    }

    public partial class SCBook
    {
        public SCBook Clear(ClearOptions options = null)
        {
            ExecuteSynchronized(options, () => DoClear(options));
            return this;
        }

        protected virtual void DoClear(ClearOptions options)
        {
            var book  = options?.Book?.Document ?? Document;
            book.Text = string.Empty;
        }
    }
}
