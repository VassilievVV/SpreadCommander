using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    public class BaseSpreadsheetCmdlet : SCCmdlet
    {
        [Parameter(HelpMessage = "Target spreadsheet. By default - write into host's spreadsheet")]
        public SCSpreadsheetContext Spreadsheet { get; set; }

        [Parameter(HelpMessage = "Comment for the table. In case of multiple data sources - comment is added to each one")]
        public string Comment { get; set; }

        protected IWorkbook GetCmdletSpreadsheet() => Spreadsheet?.Workbook ?? HostSpreadsheet;

        protected override bool NeedSynchronization() => (Spreadsheet == null || Spreadsheet == HostSpreadsheet);

        protected void AddComments(CellRange range)
        {
            var comment = Comment;
            if (!string.IsNullOrWhiteSpace(comment))
                range.Worksheet.Comments.Add(range, Environment.UserName, comment);
        }
    }
}
