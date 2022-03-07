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
        public void Clear(ClearOptions options = null) =>
            ExecuteSynchronized(options, () => DoClear(options));

        protected virtual void DoClear(ClearOptions options)
        {
            var book  = options?.Book?.Document ?? Document;
            book.Text = string.Empty;
        }
    }
}
