using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class MergeOptions: SpreadsheetOptions
    {
    }

    public partial class SCSpreadsheet
    {
        public void Merge(SCSpreadsheet spreadsheet, SpreadsheetOptions options = null) =>
            ExecuteSynchronized(options, () => DoMerge(spreadsheet, options));

        protected virtual void DoMerge(SCSpreadsheet spreadsheet, SpreadsheetOptions options)
        {
            var spread = options?.Spreadsheet?.Workbook ?? Workbook;
            spread.Append(spreadsheet.Workbook);
        }
    }
}
