using DevExpress.Spreadsheet.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions
{
    public interface ICustomFunctionDescription
    {
        string Name             { get; }
        string Description      { get; }
        CustomFunctionArgumentsDescriptionsCollection Arguments { get; }
    }
}
