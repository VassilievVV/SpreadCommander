using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    [Flags]
    public enum BingAvoidRoads
    {
        None             = 0,
        Highways         = 1,
        Tolls            = 2,
        MinimizeHighways = 4,
        MinimizeTolls    = 8,
        Ferry            = 16,
        BorderCrossing   = 32
    }
}
