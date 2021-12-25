using SpreadCommander.Common.PowerShell.CmdLets.Book;
using SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines;
using System.Data;
using System.Collections;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsLifecycle.Invoke, "SCScript")]
    public class InvokeSCScriptCmdlet : SCCmdlet
    {
        public enum ScriptEngine { PowerShell, R, Python }

        [Parameter(Position = 0, HelpMessage = "Filename of the script to invoke")]
        public string ScriptFile { get; set; }

        [Parameter(HelpMessage = "Script text to execute")]
        public string Script { get; set; }

        [Parameter(HelpMessage = "Script engine (PowerShell, R or Python) that will be used to execute script")]
        public ScriptEngine? Engine { get; set; }

        [Parameter(HelpMessage = "Book that will be used to output script results")]
        public SCBookContext Book { get; set; }

        [Parameter(HelpMessage = "DataSet that will be used to output script results (with Out-Data, Out-DataSet cmdlets)")]
        public DataSet Data { get; set; }

        [Parameter(HelpMessage = "Spreadsheet that will be used to output script results")]
        public SCSpreadsheetContext Spreadsheet { get; set; }

        [Parameter(HelpMessage = "Parameters for the script. PowerShell script accepts any parameters, R and Python - parameters will be passed as environment strings, so only parameters that can be converted to string are supported")]
        public Hashtable Parameters { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        protected override bool NeedSynchronization() => false;

        protected override void EndProcessing()
        {
            var externalHost = CheckExternalHost();

            var scriptFile = Project.Current.MapPath(ScriptFile);

            if (!Engine.HasValue && !string.IsNullOrWhiteSpace(scriptFile))
            {
                var ext = Utils.NonNullString(Path.GetExtension(scriptFile)).ToLower().Trim();
                switch (ext)
                {
                    case ".ps1":
                    case ".psm1":
                    case ".psd1":
                    case ".ps":
                        Engine = ScriptEngine.PowerShell;
                        break;
                    case ".r":
                        Engine = ScriptEngine.R;
                        break;
                    case ".python":
                    case ".py":
                        Engine = ScriptEngine.Python;
                        break;
                }
            }

            BaseScriptEngine engine = (Engine ?? ScriptEngine.PowerShell) switch
            {
                ScriptEngine.PowerShell => new PowerShellScriptEngine(),
                ScriptEngine.R          => new RScriptEngine(),
                ScriptEngine.Python     => new PythonScriptEngine(),
                _                       => new PowerShellScriptEngine()
            };

            try
            {
                engine.RequestLine += (s, e) => e.Line = null;

                engine.BookServer  = Book?.BookServer ?? externalHost.BookServer;
                engine.Workbook    = Spreadsheet?.Workbook ?? externalHost.Spreadsheet;
                engine.GridDataSet = Data ?? new DataSet("Script");

                engine.ExecutionType     = BaseScriptEngine.ScriptExecutionType.Script;
                engine.SynchronizeInvoke = externalHost.SynchronizeInvoke;

                string script = Script;
                if (string.IsNullOrWhiteSpace(script))
                {
                    ExecuteLocked(() =>
                    {
                        using var reader = File.OpenText(scriptFile);
                        script           = reader.ReadToEnd();
                    }, LockFiles ? LockObject : null);
                }

                var taskSource = new TaskCompletionSource<bool>();

                engine.ExecutionFinished += callback;

                engine.Start();

                if (Parameters != null)
                {
                    if (engine is PowerShellScriptEngine psEngine)
                    {
                        foreach (DictionaryEntry parameter in Parameters)
                            psEngine.AddVariable(Convert.ToString(parameter.Key), parameter.Value);
                    }
                    else if (engine is ProcessScriptEngine procEngine)
                    {
                        foreach (DictionaryEntry parameter in Parameters)
                            procEngine.EnvironmentVariables[Convert.ToString(parameter.Key)] = Convert.ToString(parameter.Value);
                    }
                    else
                        throw new Exception("Parameters are not supported for selected script engine.");
                }

                engine.SendCommand(script);

                taskSource.Task.Wait();     //Cannot await in event handler.


                void callback(object s, EventArgs e)
                {
                    engine.ExecutionFinished -= callback;
                    taskSource.SetResult(true);
                }
            }
            finally
            {
                engine.BookServer = null;   //Do not dispose
                engine.Workbook   = null;   //Do not dispose
                engine.GridDataSet?.Dispose();
                engine.GridDataSet = null;

                engine.Dispose();
            }
        }
    }
}
