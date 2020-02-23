using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsData.Merge, "Spreadsheet")]
    public class MergeSpreadsheetCmdlet: BaseSpreadsheetCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Document to merge with spreadsheet. Document shall be created using cmdlet New-Spreadsheet.")]
        public SCSpreadsheetContext Content { get; set; }

        [Parameter(HelpMessage = "Write each document individually, without using cache")]
        public SwitchParameter Stream { get; set; }


        protected override bool NeedSynchronization() => base.NeedSynchronization() || (Content == null || Content.Workbook == HostSpreadsheet);
        

        private readonly List<SCSpreadsheetContext> _Output = new List<SCSpreadsheetContext>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            if (Content == null)
                throw new Exception("Merging spreadsheet cannot be NULL.");

            _Output.Add(Content);

            if (Stream)
                WriteBuffer(false);
        }

        protected override void EndProcessing()
        {
            WriteBuffer(true);
        }

        protected void WriteBuffer(bool lastBlock)
        {
            WriteText(GetCmdletSpreadsheet(), _Output, lastBlock);
            _Output.Clear();
        }

        protected void WriteText(IWorkbook workbook, List<SCSpreadsheetContext> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteText(workbook, buffer, lastBlock));
        }

        protected virtual void DoWriteText(IWorkbook workbook, List<SCSpreadsheetContext> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            using (new UsingProcessor(
                () => workbook.BeginUpdate(),
                () => workbook.EndUpdate()))
            {
                var workbooks =
                    from content in buffer
                    select content.Workbook;

                workbook.Append(workbooks.ToArray());
            }
        }
    }
}
