using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.ScriptEngines.ConsoleCommands
{
    public class Table: BaseCommand
    {
        public string TableName { get; set; }

        public override void Clear()
        {
            base.Clear();

            TableName = null;
        }
    }

    public class TableWithColumns
    {
        public string TableName { get; set; }

        public List<string> ColumnNames { get; } = new List<string>();

        public void Clear()
        {
            TableName = null;
            ColumnNames.Clear();
        }
    }
}
