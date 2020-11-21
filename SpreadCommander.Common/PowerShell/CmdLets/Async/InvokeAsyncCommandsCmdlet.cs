using Microsoft.PowerShell.Commands;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Async
{
    [Cmdlet(VerbsLifecycle.Invoke, "AsyncCommands")]
    [OutputType(typeof(AsyncCommandData))]
    public class InvokeAsyncCommandsCmdlet : SCCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for data tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source.")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values.")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(Position = 0, HelpMessage = "Script block to be used for parallel processing of input objects.")]
        public ScriptBlock ScriptBlock { get; set; }

        [Parameter(HelpMessage = "Number of script blocks that in parallel. Default is number of processor cores, up to 16. Manually can be set in rage from 1 to 256.")]
        [ValidateRange(1, 256)]
        public int? ThrottleLimit { get; set; }

        [Parameter(HelpMessage = "Number of seconds to wait for all input to be processed in parallel. After the specified timeout time, all running scripts are stopped. And any remaining input objects to be processed are ignored.")]
        public int? TimeoutSeconds { get; set; }

        [Parameter(HelpMessage = "Common parameters for script block.")]
        public Hashtable CommonScriptParameters { get; set; }

        [Parameter(HelpMessage = "Custom runspace pool created with cmdlet New-SCRunspacePool. If not set - cmdlet will create own runspace pool.")]
        public RunspacePool RunspacePool { get; set; }


        private readonly List<AsyncCommandData> _Commands = new();
        private readonly List<PSObject> _Input            = new();
        private RunspacePool _RunspacePool;
        private bool _CloseRunspacePool;
        private string _ScriptText;

        private DateTime _Started;
        private DateTime? _Timeout;



        protected override void BeginProcessing()
        {
            _Started = DateTime.Now;
            if (TimeoutSeconds.HasValue)
                _Timeout = _Started.AddSeconds(TimeoutSeconds.Value);

            _Commands.Clear();

            _ScriptText = ScriptBlock?.ToString();
            if (string.IsNullOrWhiteSpace(_ScriptText))
                throw new ArgumentException("Script block is not defined.", nameof(ScriptBlock));

            if (RunspacePool == null)
            {
                int maxRunspaceCount = Utils.ValueInRange(ThrottleLimit ?? Math.Max(Environment.ProcessorCount, 16), 1, 256);
                _RunspacePool        = CheckExternalHost().Engine.CreateHostedRunspacePool(1, maxRunspaceCount);
                _CloseRunspacePool   = true;
            }
            else
                _RunspacePool        = RunspacePool;
        }

        private void CreateNewCommand(Hashtable parameters)
        {
            var command = new AsyncCommandData()
            {
                InputParameters = parameters
            };
            _Commands.Add(command);

            var powerShell = System.Management.Automation.PowerShell.Create();
            powerShell.RunspacePool = _RunspacePool;
            command.PowerShell = powerShell;
            powerShell.AddScript(PowerShellScriptEngine.StartupCommand, true);
            powerShell.AddScript(_ScriptText, true);

            if (CommonScriptParameters != null)
                foreach (DictionaryEntry pair in CommonScriptParameters)
                    powerShell.AddParameter(Convert.ToString(pair.Key), pair.Value);

            if (parameters != null)
                foreach (DictionaryEntry pair in parameters)
                    powerShell.AddParameter(Convert.ToString(pair.Key), pair.Value);

            var task          = powerShell.InvokeAsync();
            command.AsyncTask = task;
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Input.Add(obj);
        }

        protected override void EndProcessing()
        {
            try
            {
                using (var reader = GetDataSourceReader(_Input, DataSource,
                    new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns }))
                {
                    if (reader != null)
                    {
                        int fieldCount = reader.VisibleFieldCount;
                        var parameterNames = new List<string>();
                        for (int i = 0; i < fieldCount; i++)
                            parameterNames.Add(reader.GetName(i));

                        while (reader.Read())
                        {
                            var parameters = new Hashtable();
                            for (int i = 0; i < fieldCount; i++)
                            {
                                var parameterValue = reader.GetValue(i);
                                if (parameterValue == DBNull.Value)
                                    parameterValue = null;
                                parameters.Add(parameterNames[i], parameterValue);
                            }
                            CreateNewCommand(parameters);
                            parameters.Clear();
                        }
                    }
                    else  //No input parameters - execute script once without parameters (why single command runs async?)
                        CreateNewCommand(null);
                }

                var tasks = new List<Task>();
                do
                {
                    tasks.Clear();

                    foreach (var command in _Commands)
                    {
                        if (command.Processed)
                            continue;

                        if (command.AsyncTask.IsCompleted)
                            ProcessCommand(command);
                        else
                            tasks.Add(command.AsyncTask);
                    }

                    if (tasks.Count > 0)
                    {
                        if (_Timeout.HasValue)
                        {
                            if (_Timeout.Value > DateTime.Now)
                            {
                                foreach (var command in _Commands)
                                    StopCommand(command);

                                break;
                            }

                            int timeout = Convert.ToInt32(Math.Ceiling((_Timeout.Value - DateTime.Now).TotalMilliseconds));
                            Task.WaitAny(tasks.ToArray(), timeout);
                        }
                        else
                            Task.WaitAny(tasks.ToArray());
                    }
                } while (tasks.Count > 0);

                foreach (var command in _Commands)
                {
                    if (!command.Processed)
                        StopCommand(command);
                }
            }
            catch (Exception)
            {
                foreach (var command in _Commands)
                {
                    if (command.PowerShell != null)
                    {
                        command.PowerShell.Dispose();
                        command.PowerShell = null;
                    }
                }

                throw;
            }
            finally
            {
                if (_CloseRunspacePool)
                {
                    _RunspacePool?.Close();
                    _RunspacePool?.Dispose();
                }
            }


            void ProcessCommand(AsyncCommandData commandData)
            {
                if (commandData.Processed)
                    return;

                var output            = commandData.AsyncTask.Result;
                commandData.Output    = output;
                commandData.HadErrors = commandData.PowerShell.HadErrors;

                commandData.Processed = true;
                commandData.PowerShell?.Dispose();
                commandData.PowerShell = null;

                WriteObject(commandData);
            }

            void StopCommand(AsyncCommandData commandData)
            {
                if (commandData.Processed)
                    return;

                if (commandData.AsyncTask.IsCompleted)
                    ProcessCommand(commandData);
                else
                {
                    commandData.PowerShell.Stop();

                    commandData.Processed  = true;
                    commandData.TimedOut   = true;
                    commandData.PowerShell?.Dispose();
                    commandData.PowerShell = null;
                }
            }
        }
    }
}
