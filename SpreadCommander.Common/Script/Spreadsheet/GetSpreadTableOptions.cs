using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class GetSpreadTableOptions: SpreadsheetWithCopyToBookOptions
    {
    }

    public partial class SCSpreadsheet
    {
        public DataTable GetSpreadTable(string tableName, SpreadsheetWithCopyToBookOptions options = null)
        {
            DataTable result = null;
            ExecuteSynchronized(options, () => DoGetSpreadTable(options, tableName, out result));
            return result;
        }

        protected virtual void DoGetSpreadTable(SpreadsheetWithCopyToBookOptions options, string tableName, out DataTable result)
        {
            var spread = options?.Spreadsheet?.Workbook ?? Workbook;

            var range = SpreadsheetUtils.GetWorkbookRange(spread, tableName);
            result    = SpreadsheetUtils.GetDataTable(range);

            CopyRangeToBook(range, options);
            AddComments(range, options.Comment);
        }
    }
}
