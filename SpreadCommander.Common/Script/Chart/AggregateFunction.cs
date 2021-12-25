using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum AggregateFunction
    {
        None      = 0,
        Average   = 1,
        Minimum   = 2,
        Maximum   = 3,
        Sum       = 4,
        Count     = 5,
        Histogram = 6,
        Financial = 7,
        Custom    = 8
    }
}
