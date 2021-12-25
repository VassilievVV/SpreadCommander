using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
#pragma warning disable CA1069 // Enums values should not be duplicated
    public enum HeaderFooterType
    {
        First   = 0,
        Odd     = 1,
        Primary = 1,
        Even    = 2
    }
#pragma warning restore CA1069 // Enums values should not be duplicated
}
