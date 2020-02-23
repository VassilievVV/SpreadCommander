using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsCommon.Clear, "Spreadsheet")]
    public class ClearSpreadsheetCmdlet: BaseSpreadsheetCmdlet
    {
        protected override void EndProcessing()
        {
            var spread = GetCmdletSpreadsheet();
            ExecuteSynchronized(() => DoClearSpreadSheet(spread));
        }

        protected virtual void DoClearSpreadSheet(IWorkbook spread)
        {
            var newSheet = spread.Worksheets.Add();

            while (spread.ChartSheets.Count > 0)
                spread.ChartSheets.RemoveAt(0);

            while (spread.Worksheets.Count > 1)
                spread.Worksheets.RemoveAt(0);

            newSheet.Name = "Sheet1";
        }
    }
}
