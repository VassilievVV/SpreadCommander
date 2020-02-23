using DevExpress.Spreadsheet;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    public class SCSpreadsheetContext : IDisposable
    {
        protected internal IWorkbook Workbook { get; }

        public SCSpreadsheetContext()
        {
            Workbook = SpreadsheetUtils.CreateWorkbook();
        }

        public SCSpreadsheetContext(IWorkbook workbook)
        {
            Workbook = workbook;
        }

        public void Dispose()
        {
            if (!Workbook.IsDisposed)
                Workbook.Dispose();
        }
    }
}
