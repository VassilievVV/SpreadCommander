using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.ScriptEngines.ConsoleCommands
{
    public class ViewFile : BaseCommand
    {
        public string FileName { get; set; }

        public override void Clear()
        {
            base.Clear();

            FileName = null;
        }
    }
}
