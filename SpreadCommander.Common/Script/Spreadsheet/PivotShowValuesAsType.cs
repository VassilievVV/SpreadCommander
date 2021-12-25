using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public enum PivotShowValuesAsType
    {
        NoCalculation         = 0,
        Difference            = 1,
        Percent               = 2,
        PercentDifference     = 3,
        RunningTotal          = 4,
        PercentOfRow          = 5,
        PercentOfColumn       = 6,
        PercentOfTotal        = 7,
        Index                 = 8,
        RankAscending         = 9,
        RankDescending        = 10,
        PercentOfRunningTotal = 11,
        PercentOfParent       = 12,
        PercentOfParentRow    = 13,
        PercentOfParentColumn = 14
    }
}
