using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum SeriesAggregateFunction
    {
        Default   = -1,
        None      = 0,
        Average   = 1,
        Minimum   = 2,
        Maximum   = 3,
        Sum       = 4,
        Count     = 5,
        Financial = 6
    }
}
