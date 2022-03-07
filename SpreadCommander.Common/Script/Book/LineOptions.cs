using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class LineOptions : TextOptions
    {
    }

    public partial class SCBook
    {
        public void WriteLine(object obj, TextOptions options = null) =>
            WriteLine(Convert.ToString(obj), options);

        public void WriteLine(string text, TextOptions options = null) =>
            WriteText(text + Environment.NewLine, options);
    }
}
