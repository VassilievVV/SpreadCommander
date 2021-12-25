using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public enum SectionStartType
    {
        NextPage   = 0,
        OddPage    = 1,
        EvenPage   = 2,
        Continuous = 3,
        Column     = 4
    }
}
