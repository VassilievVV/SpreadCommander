using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.ScriptEngines.ConsoleCommands
{
    public class Relation : BaseCommand
    {
        public string RelationName { get; set; }

        public string ChildTableName { get; set; }

        public List<string> ChildColumnNames { get; } = new List<string>();

        public string ParentTableName { get; set; }

        public List<string> ParentColumnNames { get; } = new List<string>();


        public override void Clear()
        {
            base.Clear();

            RelationName    = null;
            ChildTableName  = null;
            ParentTableName = null;

            ChildColumnNames.Clear();
            ParentColumnNames.Clear();
        }
    }
}
