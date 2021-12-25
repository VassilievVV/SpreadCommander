using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class ClearOptions: SpreadsheetOptions
    {
    }

    public partial class SCSpreadsheet
    {
        public SCSpreadsheet Clear(SpreadsheetOptions options = null)
        {
            ExecuteSynchronized(options, () => DoClear(options));
            return this;
        }

        protected virtual void DoClear(SpreadsheetOptions options)
        {
            var spread = options?.Spreadsheet?.Workbook ?? Workbook;

            var newSheet = spread.Worksheets.Add();

            while (spread.ChartSheets.Count > 0)
                spread.ChartSheets.RemoveAt(0);

            while (spread.Worksheets.Count > 1)
                spread.Worksheets.RemoveAt(0);

            newSheet.Name = "Sheet1";
        }
    }
}
