using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class SelectSheetOptions: SpreadsheetOptions
    {
    }

    public partial class SCSpreadsheet
    {
        public void SelectSheet(string sheetName, SpreadsheetOptions options = null) =>
            ExecuteSynchronized(options, () => DoSelectSheet(sheetName, null, options));

        public void SelectSheet(int sheetIndex, SpreadsheetOptions options = null) =>
            ExecuteSynchronized(options, () => DoSelectSheet(null, sheetIndex, options));

        protected virtual void DoSelectSheet(string sheetName, int? sheetIndex, SpreadsheetOptions options)
        {
            var spread = options?.Spreadsheet?.Workbook ?? Workbook;

            Sheet sheet = null;

            if (!string.IsNullOrWhiteSpace(sheetName))
                sheet = spread.Sheets[sheetName];
            else if (sheetIndex.HasValue)
            {
                int sheetCount = spread.Sheets.Count;
                int index = sheetIndex.Value;
                if (index < 0)
                    index = sheetCount + 1 - index;    //-1 means last sheet, -2 - one before last etc.

                index = Code.Utils.ValueInRange(index, 1, sheetCount);

                sheet = spread.Sheets[index - 1];  //Use 1-based sheet index
            }

            if (sheet != null)
                spread.Sheets.ActiveSheet = sheet;
        }
    }
}
