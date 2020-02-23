using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Grid
{
    [Cmdlet(VerbsCommon.Remove, "DataTable")]
    public class RemoveDataTableCmdlet : BaseGridCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Table name")]
        [Alias("Table")]
        public string TableName { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
        }

        protected override void EndProcessing()
        {
            RemoveTable();
        }

        protected void RemoveTable()
        {
            var dataSet = CheckExternalHost().GridDataSet;
            if (dataSet == null)	//Some hosts may have no grids, in this case - do not output anywhere
                return;

            ExecuteSynchronized(() => DoRemoveTable(dataSet, TableName));
        }

        protected virtual void DoRemoveTable(DataSet dataSet, string tableName)
        {
            var table = dataSet.Tables[tableName];
            if (table != null)
            {
                for (int i = dataSet.Relations.Count - 1; i >= 0; i--)
                {
                    var relation = dataSet.Relations[i];
                    if (relation.ParentTable == table || relation.ChildTable == table)
                        dataSet.Relations.RemoveAt(i);
                }

                dataSet.Tables.Remove(table);
            }
        }
    }
}
