using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FSharp.Compiler;
using FSharp.Compiler.Interactive;
using System.Threading;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Script;
using Microsoft.FSharp.Core;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class FSharpScriptEngine: BaseScriptEngine
    {
        public const string ScriptEngineName = "F#";

        public override string EngineName => ScriptEngineName;
        public override string DefaultExt => ".fsx";
        public override string FileFilter => "F# script files (*.fsx;*.fs)|*.fsx;*.fs";
        public override string SyntaxFile => "F#";


        protected Shell.FsiEvaluationSession _Session;

        protected ScriptTextWriter _Output;
        protected ScriptTextWriter _Error;

        protected ScriptHost _Host;

        static FSharpScriptEngine()
        {
            /*
            Shell.Settings.fsi.ShowDeclarationValues = false;
            Shell.Settings.fsi.ShowIEnumerable       = false;
            Shell.Settings.fsi.ShowProperties        = false;


// Display all evaluated strings in a message box
fsi.ShowDeclarationValues <- false
fsi.AddPrinter(fun (s:string) -> 
  System.Windows.Forms.MessageBox.Show(s) |> ignore; "")

let a = "foo" // Evaluating this line doesn't show message box
let b = "bar" // (ditto)
a + b         // .. but evaluating this line shows the message box!

            Func<string, string> printer = str => str;
            FSharpFunc<string, string> printer2 = FSharpFunc<string, string>.FromConverter(new Converter<string, string>(printer));

            Shell.Settings.fsi.AddPrinter(printer2);
             */
        }

        public override void Start() 
        {
            var inStream  = new StringReader(string.Empty);
            _Output       = new ScriptTextWriter(this, false);
            _Error        = new ScriptTextWriter(this, true);

            var config  = Shell.FsiEvaluationSession.GetDefaultConfiguration();
#pragma warning disable CRRSP06 // A misspelled word has been found
            _Session = Shell.FsiEvaluationSession.Create(config, new string[] { "fsi.exe", "--noninteractive" }, inStream, _Output, _Error, null, null);
#pragma warning restore CRRSP06 // A misspelled word has been found

            string prolog =
$@"#r ""SpreadCommander.Common.dll"";
#r ""MathNet.Numerics.dll"";
#r ""MathNet.Symbolics.dll"";
#r ""Deedle.dll"";

#I {QuotePath(Project.Current.ProjectPath)};
#I {QuotePath(Path.Combine(Project.Current.ProjectPath, "bin"))};
#I {QuotePath(Path.Combine(Project.Current.ProjectPath, "Modules"))};

open System;

open SpreadCommander.Common.Script;
open SpreadCommander.Common.Script.Book;
open SpreadCommander.Common.Script.Spreadsheet;
open SpreadCommander.Common.Script.Grid;
open SpreadCommander.Common.Script.Heap;
open SpreadCommander.Common.Script.Chart;
open SpreadCommander.Common.Script.Sankey;
open SpreadCommander.Common.Script.Map;

open MathNet.Numerics;
open MathNet.Symbolics;
open Deedle;

";

            //(var ex, var diagnostics) = 
            _Session.EvalInteraction(prolog, CancellationToken.None);
            
            _Output.InitializeOutput();
            _Error.InitializeOutput();

            _Host = new ScriptHost(this);
            _Session.AddBoundValue("Host", _Host);
            _Session.AddBoundValue("Book", _Host.Book);
            _Session.AddBoundValue("Spreadsheet", _Host.Spreadsheet);
            _Session.AddBoundValue("Grid", _Host.Grid);
            _Session.AddBoundValue("Heap", _Host.Heap);

            _Output.WriteInvitation();


            static string QuotePath(string path)
            {
                if (string.IsNullOrEmpty(path))
                    return null;

                string result = Utils.QuoteString(path.Replace("\\", "\\\\"), '"');
                return result;
            }
        }

        public override bool Silent 
        { 
            get => _Output.Silent; 
            set => _Output.Silent = value; 
        }


        public override void Stop() 
        {
            var session = _Session;
            if (session != null)
            {
                session.Interrupt();

                var values = session.GetBoundValues();
                foreach (var value in values)
                {
                    if (value?.Value is IDisposable disposable)
                        disposable.Dispose();
                    else if (value is Shell.FsiBoundValue fsiValue &&
                        fsiValue.Value?.ReflectionValue is IDisposable fsiDisposable)
                        fsiDisposable.Dispose();
                }
            }

            _Session = null;

            base.Stop();
        }

        public override void SendCommand(string command) => SendCommand(command, ExecutionType == ScriptExecutionType.Script);

        public void SendCommand(string command, bool silent) 
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                FireExecutionFinished();
                GC.Collect();
                return;
            }

            var session = _Session ?? throw new Exception("Session is not started.");
            try
            {
                ProgressType = ProgressKind.Undetermined;

                if (!(silent || Silent))
                {
                    var firstLine = GetFirstLine(command, 50);
                    _Output.WriteInvitation(firstLine);
                }

                session.EvalInteraction(command, CancellationToken.None);

                if (!(silent || Silent))
                    _Output.WriteInvitation();
            }
            catch (Exception)
            {
                //Do nothing. Error was reported in _Error writer.
            }
            finally
            {
                ProgressType = ProgressKind.None;
                FireExecutionFinished();
                GC.Collect();
            }


            static string GetFirstLine(string value, int maxLength)
            {
                if (string.IsNullOrEmpty(value))
                    return string.Empty;

                var p = value.IndexOf('\n');
                bool addDots = false;
                if (p >= 0)
                {
                    value = value[..(p + 1)];
                    addDots = true;
                }
                value = value.Trim();

                if (value.Length > maxLength)
                {
                    value = value[..maxLength];
                    addDots = true;
                }

                if (addDots)
                    value += " ...";

                return value;
            }
        }
    }
}
