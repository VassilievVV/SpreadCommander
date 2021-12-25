using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum PieExplodeMode
    {
        None       = 0,
        All        = 1,
        MinValue   = 2,
        MaxValue   = 3,
        UsePoints  = 4,
        UseFilters = 5,
        Others     = 6
    }
}
