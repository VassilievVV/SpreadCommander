using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsCommon.Get, "SpreadTable")]
    [OutputType(typeof(DataTable))]
    public class GetSpreadTableCmdlet: BaseSpreadsheetWithCopyToBookCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Table name, defined range name or range to generate DataTable from")]
        public string TableName { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(TableName))
                throw new Exception("Table name is not specified.");

            DataTable dataTable = null;
            ExecuteSynchronized(() => DoCreateDataTable(out dataTable));

            WriteObject(dataTable);
        }

        protected void DoCreateDataTable(out DataTable dataTable)
        {
            var workbook  = GetCmdletSpreadsheet();

            var range = SpreadsheetUtils.GetWorkbookRange(workbook, TableName);
            dataTable = SpreadsheetUtils.GetDataTable(range);

            CopyRangeToBook(range);
            AddComments(range);
        }
    }
}
