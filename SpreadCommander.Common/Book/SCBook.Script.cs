using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.Host;
using SpreadCommander.Common.ScriptEngines;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Common.Book
{
    public partial class SCBook
    {
        //Do not allow execute scripts from Book for security reasons
        /*
        private enum ScriptEngine { Unknown, PowerShell, R, Python }

        protected IRichEditDocumentServer AddScript(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE SCRIPT' requires filename as first argument.");

            var fileName = Project.Current.MapPath(arguments[0].Value);
            if (!File.Exists(fileName))
                throw new Exception($"File '{fileName}' does not exist.");

            ScriptEngine engineType = ScriptEngine.Unknown;

            if (arguments.Count > 1)
            {
                var properties = Utils.SplitNameValueString(arguments[1].Value, ';');

                foreach (var prop in properties)
                {
                    if (string.IsNullOrWhiteSpace(prop.Key))
                        continue;

                    switch (prop.Key.ToLower())
                    {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                        case "engine":
                            var valueEngine = Utils.NonNullString(prop.Value).Trim();
                            switch (valueEngine.ToLower())
                            {
                                case "powershell":
                                case "ps":
                                    engineType = ScriptEngine.PowerShell;
                                    break;
                                case "r":
                                    engineType = ScriptEngine.R;
                                    break;
                                case "py":
                                    engineType = ScriptEngine.Python;
                                    break;
                                default:
                                    throw new Exception($"Unrecognized script engine - '{valueEngine}'.");
                            }
                            break;
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
                    }
                }
            }

            if (engineType == ScriptEngine.Unknown)
            {
                var ext = Utils.NonNullString(Path.GetExtension(fileName)).ToLower().Trim();
                switch (ext)
                {
                    case ".ps1":
                    case ".ps":
                        engineType = ScriptEngine.PowerShell;
                        break;
                    case ".r":
                        engineType = ScriptEngine.R;
                        break;
                    case ".python":
                    case ".py":
                        engineType = ScriptEngine.Python;
                        break;
                }
            }
            var engine = engineType switch
            {
                ScriptEngine.PowerShell => (BaseScriptEngine)new PowerShellScriptEngine(),
                ScriptEngine.R          => (BaseScriptEngine)new RScriptEngine(),
                ScriptEngine.Python     => (BaseScriptEngine)new PythonScriptEngine(),
                _                       => throw new Exception("Cannot determine script engine or unsupported script engine.")
            };
            try
            {
                var server = new RichEditDocumentServer();

                engine.RequestLine += (s, e) => e.Line = null;

                engine.BookServer  = server;
                engine.Workbook    = SpreadsheetUtils.CreateWorkbook();
                engine.GridDataSet = new DataSet("Script");

                engine.ExecutionType = BaseScriptEngine.ScriptExecutionType.Script;

                string script;
                using (var reader = File.OpenText(fileName))
                    script = reader.ReadToEnd();

                var taskSource = new TaskCompletionSource<bool>();

                engine.ExecutionFinished += callback;
                engine.Start();
                engine.SendCommand(script);

                taskSource.Task.Wait();     //Cannot await in event handler.

                return server;


                void callback(object s, EventArgs e)
                {
                    engine.ExecutionFinished -= callback;
                    taskSource.SetResult(true);
                }
            }
            finally
            {
                engine.BookServer = null;   //Do not dispose, it is returned from function
                engine.Workbook?.Dispose();
                engine.Workbook = null;
                engine.GridDataSet?.Dispose();
                engine.GridDataSet = null;

                engine.Dispose();
            }
        }
        */
    }
}
