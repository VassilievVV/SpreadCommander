using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Controls
{
    public interface IScriptEditorHost
    {
        ScriptEditorControl ScriptEditor { get; }
    }
}
