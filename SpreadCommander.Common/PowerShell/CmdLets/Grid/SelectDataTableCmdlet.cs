using DevExpress.Mvvm;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Messages;
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
    [Cmdlet(VerbsCommon.Select, "DataTable")]
    public class SelectDataTableCmdlet : BaseGridCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Table name to make active in grid.")]
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
            SelectTable();
        }

        protected void SelectTable()
        {
            var dataSet = CheckExternalHost().GridDataSet;
            if (dataSet == null)	//Some hosts may have no grids, in this case - do not output anywhere
                return;

            ExecuteSynchronized(() => DoSelectTable(dataSet, TableName));
        }

        protected virtual void DoSelectTable(DataSet dataSet, string tableName)
        {
            var table = dataSet.Tables[tableName];
            if (table != null)
                Messenger.Default.Send(new SelectDataSetTableMessage() { DataSet = dataSet, SelectedDataTable = table });
        }
    }
}
