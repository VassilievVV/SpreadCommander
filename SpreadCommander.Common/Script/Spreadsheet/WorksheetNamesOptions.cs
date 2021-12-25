using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class WorksheetNamesOptions : SpreadsheetOptions
    {
    }

    public partial class SCSpreadsheet 
    {
        public string[] GetWorksheetNames(SpreadsheetOptions options = null)
        {
            string[] sheetNames = null;
            ExecuteSynchronized(options, () => DoGetWorksheetNames(options, out sheetNames));
            return sheetNames;
        }

        protected virtual void DoGetWorksheetNames(SpreadsheetOptions options, out string[] result)
        {
            var spread = options?.Spreadsheet?.Workbook ?? Workbook;

            var sheetNames = new List<string>();
            foreach (var worksheet in spread.Worksheets)
                sheetNames.Add(worksheet.Name);

            result = sheetNames.ToArray();
        }
    }
}
