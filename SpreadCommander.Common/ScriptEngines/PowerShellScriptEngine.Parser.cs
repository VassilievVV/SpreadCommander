using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class PowerShellScriptEngine
    {
        public override void ListScriptParseErrors(string text, List<ScriptParseError> errors)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            Parser.ParseInput(text, out Token[] _, out ParseError[] scriptErrors);
            if ((scriptErrors?.Length ?? 0) <= 0)
                return;

            foreach (var scriptError in scriptErrors)
            {
                var error = new ScriptParseError()
                {
                    ErrorID           = scriptError.ErrorId,
                    Message           = scriptError.Message,
                    StartLineNumber   = scriptError.Extent.StartLineNumber,
                    StartColumnNumber = scriptError.Extent.StartColumnNumber,
                    EndLineNumber     = scriptError.Extent.EndLineNumber,
                    EndColumnNumber   = scriptError.Extent.EndColumnNumber,
                    IncompleteInput   = scriptError.IncompleteInput
                };
                errors.Add(error);
            }
        }
    }
}
