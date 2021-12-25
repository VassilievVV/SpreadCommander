using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum CandleStickFillMode
    {
        FilledOnReduction = 0,
        FilledOnIncrease  = 1,
        AlwaysEmpty       = 2,
        AlwaysFilled      = 3
    }
}
