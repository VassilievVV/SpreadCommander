﻿using System;
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

        protected StringReader     _InStream;
        protected ScriptTextWriter _Output;
        protected ScriptTextWriter _Error;

        protected CancellationTokenSource _CancellationTokenSource;

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

        public override void ExecuteSynchronized(Action action)
        {
            var action2 = () =>
            {
                _Output.DoWrite();
                _Error.DoWrite();
                try
                {
                    action();
                }
                finally
                {
                    _Output.DoWrite();
                    _Error.DoWrite();
                }
            };

            base.ExecuteSynchronized(action2);
        }

        public override void Start() 
        {
            _InStream  = new StringReader(string.Empty);
            _Output    = new ScriptTextWriter(this, false);
            _Error     = new ScriptTextWriter(this, true);

            var config  = Shell.FsiEvaluationSession.GetDefaultConfiguration();
#pragma warning disable CRRSP06 // A misspelled word has been found
#pragma warning disable CRRSP05 // A misspelled word has been found
            _Session = Shell.FsiEvaluationSession.Create(config, new string[] { "fsi.exe", "--noninteractive", "--multiemit-", "--optimize+", /*"--shadowcopyreferences+",*/
                "--consolecolors", "--define:SPREADCOMMANDER" }, _InStream, _Output, _Error, true, null);
#pragma warning restore CRRSP05 // A misspelled word has been found
#pragma warning restore CRRSP06 // A misspelled word has been found

            string prolog =
$@"#I {QuotePath(Path.GetDirectoryName(typeof(System.Drawing.Bitmap).Assembly.Location))};
#r ""System.Drawing.dll"";
#r ""System.Drawing.Common.dll"";

#r ""SpreadCommander.Common.dll"";
#r ""MathNet.Numerics.dll"";
#r ""MathNet.Symbolics.dll"";
#r ""MathNet.Numerics.Data.Text.dll"";
#r ""Deedle.dll"";
#r ""ILGPU.dll"";
#r ""ILGPU.Algorithms.dll"";

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
                        DisposeObject(disposable);
                    else if (value is Shell.FsiBoundValue fsiValue &&
                        fsiValue.Value?.ReflectionValue is IDisposable fsiDisposable)
                        DisposeObject(fsiDisposable);
                }
            }

            _Session = null;
            _CancellationTokenSource?.Dispose();
            _CancellationTokenSource = null;

            _Output?.Dispose();
            _Error?.Dispose();
            _InStream?.Dispose();

            base.Stop();


            void DisposeObject(IDisposable disposable)
            {
                if (disposable == _Host ||
                    disposable == _Host.Book || disposable == _Host.Spreadsheet ||
                    disposable == _Host.Grid || disposable == _Host.Heap) 
                    return;

                disposable.Dispose();
            }
        }

        public override void Cancel()
        {
            //_Session?.Interrupt();

            _CancellationTokenSource?.Cancel();
            _CancellationTokenSource?.Dispose();
            _CancellationTokenSource = null;

            GC.Collect();
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

                _CancellationTokenSource?.Dispose();
                _CancellationTokenSource = new CancellationTokenSource();

                session.EvalInteraction(command, _CancellationTokenSource.Token);
            }
            catch (Exception)
            {
                //Do nothing Error was reported in _Error writer.
            }
            finally
            {
                ProgressType = ProgressKind.None;
                FireExecutionFinished();
                GC.Collect();
            }

            if (!(silent || Silent))
                _Output.WriteInvitation();


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
