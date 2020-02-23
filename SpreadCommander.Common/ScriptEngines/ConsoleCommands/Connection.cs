using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.ScriptEngines.ConsoleCommands
{
    public class Connection: BaseCommand
    {
        public string Name { get; set; }

        public override void Clear()
        {
            base.Clear();

            Name = null;
        }
    }
}
