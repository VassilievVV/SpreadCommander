using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public enum ParagraphLineSpacing
    {
        Single        = 0,
#pragma warning disable CRRSP08 // A misspelled word has been found
        Sesquialteral = 1,
#pragma warning restore CRRSP08 // A misspelled word has been found
        Double        = 2,
        Multiple      = 3,
        Exactly       = 4,
        AtLeast       = 5
    }
}
