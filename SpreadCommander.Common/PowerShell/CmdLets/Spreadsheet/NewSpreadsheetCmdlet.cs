using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsCommon.New, "Spreadsheet")]
    [OutputType(typeof(SCSpreadsheetContext))]
    public class NewSpreadsheetCmdlet: SCCmdlet
    {
        [Parameter(Position = 0, HelpMessage = "Name of file to load content from")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        protected override void EndProcessing()
        {
            var fileName = Project.Current.MapPath(FileName);
            if (!string.IsNullOrWhiteSpace(fileName) && !File.Exists(fileName))
                throw new Exception($"Cannot find file '{FileName}'.");

            var spreadsheet = new SCSpreadsheetContext();

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                ExecuteLocked(() => spreadsheet.Workbook.LoadDocument(fileName), LockFiles ? LockObject : null);
            }

            WriteObject(spreadsheet);
        }
    }
}
