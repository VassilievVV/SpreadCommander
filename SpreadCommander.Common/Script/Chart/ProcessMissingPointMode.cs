using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum ProcessMissingPointsMode
    {
        Skip              = 0,
        InsertZeroValues  = 1,
        InsertEmptyPoints = 2
    }
}
