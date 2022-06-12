using FSharp.Compiler.CodeAnalysis;
using FSharp.Compiler.Diagnostics;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class FSharpScriptEngine
    {
        public override void ListScriptParseErrors(string text, List<ScriptParseError> errors)
        {
            var session = _Session;
            if (session == null)
                return;

            (var fileResults, var _, var _) = session.ParseAndCheckInteraction(text);

            var scriptErrors = new List<ScriptParseError>();
            
            if (fileResults.Diagnostics != null)
                foreach (var scriptError in fileResults.Diagnostics)
                    AddDiagnostic(scriptError);

            errors.AddRange(scriptErrors.Distinct().OrderBy(err => err.StartLineNumber).ThenBy(err => err.StartColumnNumber));


            void AddDiagnostic(FSharpDiagnostic diagnostic)
            {
                var error = new ScriptParseError()
                {
                    ErrorID           = diagnostic.ErrorNumberText,
                    Message           = diagnostic.Message,
                    StartLineNumber   = diagnostic.StartLine,
                    StartColumnNumber = diagnostic.StartColumn + 1,
                    EndLineNumber     = diagnostic.EndLine + 1,
                    EndColumnNumber   = diagnostic.EndColumn
                };
                scriptErrors.Add(error);
            }
        }
    }
}
