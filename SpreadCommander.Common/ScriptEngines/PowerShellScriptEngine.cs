using SpreadCommander.Common.PowerShell;
using System;
using System.Collections.Generic;
using System.Linq;
using Automation = System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Management.Automation;
using SpreadCommander.Common.PowerShell.Host;
using SpreadCommander.Common.PowerShell.CmdLets;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Spreadsheet;
using System.Drawing;
using System.Data;
using System.Threading;
using SpreadCommander.Common.Code;
using System.ComponentModel;
using DevExpress.XtraRichEdit;
using DevExpress.XtraGauges.Core.Base;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class PowerShellScriptEngine : BaseScriptEngine, ISpreadCommanderHostOwner
    {
        public const string ScriptEngineName = "PowerShell";

        public override string EngineName => ScriptEngineName;
        public override string DefaultExt => ".ps1";
        public override string FileFilter => "PowerShell files (*.ps1)|*.ps1";
        public override string SyntaxFile => "PowerShell";

        public readonly static ConsoleColor EchoColor = ConsoleColor.Gray;

        public bool ShouldExit { get; set; }

        public int ExitCode { get; set; }

        IRichEditDocumentServer ISpreadCommanderHostOwner.BookServer => this.BookServer;
        IWorkbook ISpreadCommanderHostOwner.Spreadsheet              => this.Workbook;
        DataSet ISpreadCommanderHostOwner.GridDataSet                => this.GridDataSet;
        IFileViewer ISpreadCommanderHostOwner.FileViewer             => this.FileViewer;

        public int DefaultChartDPI                                  { get; set; } = 300;

        public bool Silent                                          { get; set; }

        private SpreadCommanderHost _Host;
        private Runspace _Runspace;
        private Pipeline _Pipe;

        /// Used to serialize access to instance data.
        private readonly object instanceLock = new object();


        public static string StartupCommand =>
$@"$global:ErrorActionPreference = 'Break';
$ErrorActionPreference = 'Break';

Set-ExecutionPolicy Bypass -Scope:Process;

cd {Utils.QuoteString(Project.Current.ProjectPath, "\'")};";

        public override void Start()
        {
            lock (instanceLock)
            {
                _Host = new SpreadCommanderHost(this);

                _Runspace = CreateHostedRunspace();

                _Runspace.Debugger.DebuggerStop += Debugger_DebuggerStop;

                if (ExecutionType == ScriptExecutionType.Interactive)
                    _Host.UI.WriteLine("SpreadCommander>");
            }
        }

        private void Debugger_DebuggerStop(object sender, DebuggerStopEventArgs e)
        {
            //if (!(sender is System.Management.Automation.Debugger debugger))
            //    throw new Exception("Cannot determine script debugger.");

            e.ResumeAction = DebuggerResumeAction.Stop;

            //Write exception message
            using var pipe = _Runspace.CreateNestedPipeline("$error", false);

            if (_Runspace.RunspaceAvailability != RunspaceAvailability.Available &&
                _Runspace.RunspaceAvailability != RunspaceAvailability.AvailableForNestedCommand)
            {
                WriteDefaulterrorMessage();
                return;
            }

            var result = pipe.Invoke();
            if ((result?.Count ?? 0) > 0 && result[0]?.BaseObject is ErrorRecord error)
            {
                try
                {
                    //Write error
                    var cmdlet          = error.InvocationInfo?.InvocationName;
                    var message         = GetExceptionMesage(error.Exception);
                    var positionMessage = error.InvocationInfo?.PositionMessage;
                    var categoryInfo    = error.CategoryInfo?.ToString();
                    var errorId         = error.FullyQualifiedErrorId?.ToString();

                    var errorMessage =
$@"{cmdlet} : {message}
{positionMessage}
+ CategoryInfo          : {categoryInfo}
+ FullyQualifiedErrorId : {errorId}";

                    _Host.UI.WriteErrorLine(errorMessage);
                }
                catch (Exception)
                {
                    WriteDefaulterrorMessage();
                }
            }
            else
                WriteDefaulterrorMessage();


            void WriteDefaulterrorMessage()
            {
                var errorMessage =
$@"Unknown error : {e.InvocationInfo?.InvocationName}
{e.InvocationInfo?.PositionMessage}";

                _Host.UI.WriteErrorLine(errorMessage);
            }

            static string GetExceptionMesage(Exception ex)
            {
                var output = new StringBuilder();

                while (ex != null)
                {
                    if (output.Length > 0)
                        output.AppendLine();
                    output.Append(ex.Message);

                    ex = ex.InnerException;
                }

                return output.ToString();
            }
        }

        public override void Stop()
        {
            lock (instanceLock)
            {
                // close the runspace to free resources.
                _Runspace?.Close();
                _Runspace?.Dispose();
                _Runspace = null;
            }

            _Host = null;
        }

        public void AddVariable(string name, object value)
        {
            var runspace = _Runspace ?? throw new Exception("Runspace is not initialized.");
            runspace.SessionStateProxy.PSVariable.Set(name, value);
        }

        public override void SendCommand(string command)  => SendCommand(command, ExecutionType == ScriptExecutionType.Script);
        public void SendCommand(string command, bool silent)
        {
            if (string.IsNullOrEmpty(command))
            {
                FireExecutionFinished();
                return;
            }

            if (!command.EndsWith(Environment.NewLine))
                command += Environment.NewLine;

            ProgressType = ProgressKind.Undetermined;
            try
            {
                if (!(silent || Silent))
                    Write(EchoColor, ConsoleColor.White, $">> {GetFirstLine(command, 50)}{Environment.NewLine}");

                if (_Pipe != null)
                {
                    if (_Pipe.PipelineStateInfo.State == PipelineState.Running)
                    {
                        _Pipe.Input.Write(command);
                        _Pipe.Input.Close();
                        return;
                    }

                    _Pipe = null;
                }

                _Pipe = _Runspace.CreatePipeline();

                _Pipe.StateChanged += ((s, e) =>
                {
                    switch (e.PipelineStateInfo.State)
                    {
                        case PipelineState.Running:
                            //Do nothing
                            break;
                        case PipelineState.Stopping:
                            if (!(silent || Silent))
                                _Host.UI.WriteErrorLine($"{Environment.NewLine}SpreadCommander> Stopping ...");
                            break;
                        case PipelineState.Completed:
                        case PipelineState.Stopped:
                        default:
                            _Pipe?.Dispose();
                            _Pipe = null;
                            ProgressType = ProgressKind.None;

                            if (e.PipelineStateInfo.State == PipelineState.Failed)
                                _Host.UI.WriteErrorLine($"Failed: {e.PipelineStateInfo.Reason?.Message}");

                            if (!(silent || Silent))
                                _Host.UI.Write($"{Environment.NewLine}SpreadCommander>{Environment.NewLine}");

                            FireExecutionFinished();
                            break;
                    }
                });

                _Pipe.Commands.AddScript(command);
                _Pipe.Commands.Add("out-host");
                _Pipe.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);

                if (_Runspace.RunspaceAvailability != RunspaceAvailability.Available)
                    throw new Exception("Another PowerShell command is already executing.");

                _Pipe.InvokeAsync();
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }


            static string GetFirstLine(string value, int maxLength)
            {
                if (string.IsNullOrEmpty(value))
                    return string.Empty;

                var p = value.IndexOf('\n');
                bool addDots = false;
                if (p >= 0)
                {
                    value = value.Substring(0, p + 1);
                    addDots = true;
                }
                value = value.Trim();

                if (value.Length > maxLength)
                {
                    value = value.Substring(0, maxLength);
                    addDots = true;
                }

                if (addDots)
                    value += " ...";

                return value;
            }
        }

        //Unlike usual command this one is executed synchronously.
        protected static void SendServiceCommand(Runspace runspace, string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            if (!command.EndsWith(Environment.NewLine))
                command += Environment.NewLine;

            using var pipe = runspace.CreatePipeline();
            pipe.Commands.AddScript(command);
            pipe.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);

            if (runspace.RunspaceAvailability != RunspaceAvailability.Available)
                throw new Exception("Another PowerShell command is already executing.");

            pipe.Invoke();
        }

        public void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            ScriptOutputAvailable(ScriptOutputType.Text, value, 
                SpreadCommanderHost.ConvertConsoleColor(foregroundColor, false), SpreadCommanderHost.ConvertConsoleColor(backgroundColor, true));
        }

        public override void ScriptOutputAvailable(ScriptOutputType outputType, string output,
            Color foregroundColor, Color backgroundColor)
        {
            switch (outputType)
            {
                case ScriptOutputType.Text:
                    FlushTextBufferSynchronized(Document, SynchronizeInvoke, foregroundColor, backgroundColor, new StringBuilder(output));
                    break;
                case ScriptOutputType.Error:
                    ReportErrorSynchronized(Document, SynchronizeInvoke, output);
                    break;
            }
        }

        public void DisplayProgress(long sourceID, ProgressRecord progress)
        {
            switch (progress.RecordType)
            {
                case ProgressRecordType.Completed:
                    ProgressType = ProgressKind.Undetermined;	//script continues to run
                    break;
                case ProgressRecordType.Processing:
                    UpdateProgress(ProgressKind.Value, progress.PercentComplete, 100, progress.StatusDescription);
                    break;
            }
        }

        public string ReadLine()
        {
            var result = RequestInputLine();
            return Utils.NonNullString(result);
        }

        private void ReportException(Exception ex)
        {
            if (ex == null)
                return;

            object error;
            if (ex is Automation.IContainsErrorRecord icer)
                error = icer.ErrorRecord;
            else
                error = (object)new Automation.ErrorRecord(ex, "Host.ReportException", Automation.ErrorCategory.NotSpecified, null);

            Automation.PowerShell ps = null;
            try
            {
                lock (instanceLock)
                {
                    ps = Automation.PowerShell.Create();

                    ps.Runspace = _Runspace;

                    ps.AddScript("$input").AddCommand("out-string");

                    // Do not merge errors, this function will swallow errors.
                    Collection<Automation.PSObject> result;
                    var inputCollection = new Automation.PSDataCollection<object>
                    {
                        error
                    };
                    inputCollection.Complete();
                    result = ps.Invoke(inputCollection);

                    if (result.Count > 0)
                    {
                        string str = result[0].BaseObject as string;
                        // Remove \r\n that is added by Out-string.
                        if (!string.IsNullOrEmpty(str) && str.EndsWith("\r\n"))
                            str = str[0..^2];
                        _Host.UI.WriteErrorLine(str);
                    }
                }
            }
            catch (Exception exOutput)
            {
                _Host.UI.WriteErrorLine(exOutput.Message);
            }
            finally
            {
                // Dispose of the pipeline line and set it to null, locked because currentPowerShell
                // may be accessed by the ctrl-C handler.
                lock (this.instanceLock)
                    ps?.Dispose();
            }
        }

        public static Runspace CreateRunspace()
        {
            var sessionState = InitialSessionState.CreateDefault();
            InitializeRunspaceConfiguration(sessionState);

            var result = RunspaceFactory.CreateRunspace(sessionState);
            result.Open();

            SendServiceCommand(result, StartupCommand);

            return result;
        }

        public static int DefaultMaxRunspaces => Environment.ProcessorCount;

        //PowerShellScriptEngine shall be started
        public Runspace CreateHostedRunspace(bool open = true)
        {
            var sessionState = InitialSessionState.CreateDefault();
            InitializeRunspaceConfiguration(sessionState);

            var result = RunspaceFactory.CreateRunspace(_Host, sessionState);
            if (open)
            {
                result.Open();
                SendServiceCommand(result, StartupCommand);
            }

            return result;
        }

        //When using runspace - may need to call SendServiceCommand(runspace, StartupCommand)
        public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces)
        {
            var sessionState = InitialSessionState.CreateDefault();
            InitializeRunspaceConfiguration(sessionState);

            var result = RunspaceFactory.CreateRunspacePool(sessionState);
            result.SetMinRunspaces(minRunspaces);
            result.SetMaxRunspaces(maxRunspaces);
            result.Open();

            return result;
        }

        //PowerShellScriptEngine shall be started
        //When using runspace - may need to call SendServiceCommand(runspace, StartupCommand)
        public RunspacePool CreateHostedRunspacePool(int minRunspaces, int maxRunspaces, bool open = true)
        {
            var sessionState = InitialSessionState.CreateDefault();
            InitializeRunspaceConfiguration(sessionState);

            var result = RunspaceFactory.CreateRunspacePool(minRunspaces, maxRunspaces,
                sessionState, _Host);
            if (open)
                result.Open();

            return result;
        }
    }
}
