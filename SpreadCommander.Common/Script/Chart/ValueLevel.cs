using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum ValueLevel
    {
        Value             = 0,
        Value_1           = 1,
        Value_2           = 2,
        Low               = 3,
        High              = 4,
        Open              = 5,
        Close             = 6,
        Weight            = 7,
        BoxPlotMin        = 8,
        BoxPlotQuartile_1 = 9,
        BoxPlotMedian     = 10,
        BoxPlotQuartile_3 = 11,
        BoxPlotMax        = 12,
        BoxPlotMean       = 13
    }
}
