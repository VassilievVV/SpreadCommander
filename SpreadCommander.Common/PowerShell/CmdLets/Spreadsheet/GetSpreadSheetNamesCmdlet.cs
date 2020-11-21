using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsCommon.Get, "SpreadSheetNames")]
    [OutputType(typeof(string))]
    public class GetSpreadSheetNamesCmdlet : BaseSpreadsheetCmdlet
    {
        protected override void ProcessRecord()
        {
            var sheetNames = new List<string>();
            ExecuteSynchronized(() => DoGetSpreadTableNames(sheetNames));

            WriteObject(sheetNames, true);
        }

        protected void DoGetSpreadTableNames(List<string> sheetNames)
        {
            var workbook = GetCmdletSpreadsheet();

            if (workbook == null)
                return;

            foreach (var worksheet in workbook.Worksheets)
                sheetNames.Add(worksheet.Name);
        }
    }
}
