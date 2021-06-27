using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsCommon.Select, "SpreadSheet")]
    public class SelectSpreadSheetCmdlet : BaseSpreadsheetCmdlet
    {
        [Parameter(ParameterSetName = "Name", Position = 0, Mandatory = true, HelpMessage = "Sheet name to make active in spreadsheet.")]
        public string SheetName { get; set; }

        [Parameter(ParameterSetName = "Index", Position = 0, Mandatory = true, HelpMessage = "Sheet index (1-based) to make active in spreadsheet.")]
        public int SheetIndex { get; set; }

        protected override void EndProcessing()
        {
            var spread = GetCmdletSpreadsheet();
            ExecuteSynchronized(() => DoSelectSpreadSheet(spread));
        }

        protected virtual void DoSelectSpreadSheet(IWorkbook spread)
        {
            Sheet sheet = null;

            switch (ParameterSetName)
            {
                case "Name":
                    sheet = spread.Sheets[SheetName];
                    break;
                case "Index":
                    int sheetCount = spread.Sheets.Count;
                    int index      = SheetIndex;
                    if (index < 0)
                        index = sheetCount + 1 - index;    //-1 means last sheet, -2 - one before last etc.

                    index = Code.Utils.ValueInRange(index, 1, sheetCount);

                    sheet = spread.Sheets[index - 1];  //Use 1-based sheet index
                    break;
            }

            if (sheet != null)
                spread.Sheets.ActiveSheet = sheet;
        }
    }
}
