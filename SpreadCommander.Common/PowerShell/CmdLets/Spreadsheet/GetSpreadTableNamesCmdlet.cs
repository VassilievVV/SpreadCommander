using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsCommon.Get, "SpreadTableNames")]
    [OutputType(typeof(string))]
    public class GetSpreadTableNamesCmdlet: BaseSpreadsheetCmdlet
    {
        [Parameter(HelpMessage = "Whether to include defined ranges or no.")]
        public SwitchParameter IncludeDefinedRanges { get; set; }

        [Parameter(HelpMessage = "Whether to include sheet names or no. Sheet names are output with '!' symbol at end.")]
        public SwitchParameter IncludeSheetNames { get; set; }

        protected override void ProcessRecord()
        {
            var tableNames = new List<string>();
            ExecuteSynchronized(() => DoGetSpreadTableNames(tableNames));

            WriteObject(tableNames, true);
        }

        protected void DoGetSpreadTableNames(List<string> tableNames)
        {
            var workbook = GetCmdletSpreadsheet();

            if (workbook == null)
                return;

            foreach (var worksheet in workbook.Worksheets)
            {
                if (IncludeSheetNames)
                    tableNames.Add($"{worksheet.Name}!");

                foreach (var table in worksheet.Tables)
                {
                    var tableName = $"{worksheet.Name}!{table.Name}";
                    tableNames.Add(tableName);
                }

                if (IncludeDefinedRanges)
                {
                    foreach (var definedName in worksheet.DefinedNames)
                    {
                        var defName = $"{worksheet.Name}!{definedName.Name}";
                        tableNames.Add(defName);
                    }
                }
            }
        }
    }
}
