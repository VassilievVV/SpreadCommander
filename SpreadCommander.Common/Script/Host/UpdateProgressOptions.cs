using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public enum ProgressKind { None, Undetermined, Value }

    public partial class ScriptHost
    {
        public virtual void UpdateProgress(ProgressKind progressType, int value, int max, string status)
        {
            Engine?.UpdateProgress((ScriptEngines.BaseScriptEngine.ProgressKind)progressType, value, max, status);
        }
    }
}
