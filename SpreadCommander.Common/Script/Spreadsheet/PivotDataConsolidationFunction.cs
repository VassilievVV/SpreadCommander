using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public enum PivotDataConsolidationFunction
    {
        Sum          = 4,
        Count        = 8,
        Average      = 16,
        Max          = 32,
        Min          = 64,
        Product      = 128,
        CountNumbers = 256,
        StdDev       = 512,
        StdDevp      = 1024,
        Var          = 2048,
        Varp         = 4096
    }
}
