using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public enum BingRouteOptimization
    {
        MinimizeTime             = 0,
        MinimizeDistance         = 1,
        MinimizeTimeWithTraffic  = 2,
        MinimizeTimeAvoidClosure = 3
    }
}
