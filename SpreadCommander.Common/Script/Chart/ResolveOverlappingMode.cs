using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum ResolveOverlappingMode
    {
        None                  = 0,
        Default               = 1,
        HideOverlapped        = 2,
        JustifyAroundPoint    = 3,
        JustifyAllAroundPoint = 4
    }
}
