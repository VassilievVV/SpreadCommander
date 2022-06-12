using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static winpty.WinPty;
using SpreadCommander.Common.Code;

#pragma warning disable CRRSP05 // A misspelled word has been found

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class ProcessScriptEngine: BaseScriptEngine
    {
        //Set buffer large enough for commands. For most programs should be divided by 80.
        public const int BufferWidth  = 80;
        public const int BufferHeight = 32;

        public virtual string ProcessPath		{ get; }
        public virtual string ProcessArguments	{ get; }

        public virtual string ProcessScriptPath	     => ProcessPath;
        public virtual string ProcessScriptArguments => ProcessArguments;

        public virtual StringNoCaseDictionary<string> EnvironmentVariables { get; } = new StringNoCaseDictionary<string>();

        public virtual string FormatProcessScriptArguments(string scriptFileName)
        {
            return $"{ProcessScriptArguments} {Utils.QuoteString(scriptFileName, "\"")}";
        }


        private IntPtr _WinPTY_Handle        = IntPtr.Zero;
        private IntPtr _WinPTY_Cfg           = IntPtr.Zero;
        private IntPtr _WinPTY_SpawnCfg      = IntPtr.Zero;
        private IntPtr _WinPTY_ProcessHandle = IntPtr.Zero;
        private IntPtr _WinPTY_ThreadHandle  = IntPtr.Zero;
        private Stream _WinPTY_StdIN         = null;
        private Stream _WinPTY_StdOUT        = null;
        private Stream _WinPTY_StdERR        = null;
        private string _ScriptTempFile       = null;
        private readonly object _SyncObject  = new();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GetProcessId(IntPtr hWnd);

        private static (int code, string message) GetWinPtyErrorDetails(IntPtr err)
        {
            if (err == IntPtr.Zero)
                return (0, null);

            var code    = winpty_error_code(err);
            var message = winpty_error_msg(err);

            winpty_error_free(err);

            return (code, message);
        }

        private static void CheckWinPtyError(IntPtr err)
        {
            if (err == IntPtr.Zero)
                return;

            (var code, var message) = GetWinPtyErrorDetails(err);
            throw new Exception($"WinPTY error {code}: {message}");
        }

        public override void Start()
        {
            //When executing script - start new engine every time
            if (ExecutionType == ScriptExecutionType.Script)
                return;

            StartEngineProcess();
        }

        protected virtual void StartEngineProcess(string scriptFileName = null)
        {
            lock (_SyncObject)
            {
                string exe;
                string args;
                string cwd = Project.Current.ProjectPath; 

                switch (ExecutionType)
                {
                    case ScriptExecutionType.Interactive:
                        exe  = ProcessPath;
                        args = ProcessArguments;
                        break;
                    case ScriptExecutionType.Script:
                        exe  = ProcessScriptPath;
                        args = FormatProcessScriptArguments(scriptFileName);
                        break;
                    default:
                        throw new Exception("Unknown execution type.");
                }

                if (string.IsNullOrWhiteSpace(exe) || !File.Exists(exe))
                {
                    ScriptOutputAvailable(ScriptOutputType.Error, "ERROR: Cannot find executable file",
                        DefaultForegroundErrorColor, Color.White);

                    return;
                }

                var envVariables = EnvironmentVariables;
                var env = new StringBuilder();
                foreach (KeyValuePair<string, string> variable in envVariables)
                    env.Append($"{variable.Key}={variable.Value}").Append('\0');
                if (env.Length > 0)
                    env.Append('\0');

                _WinPTY_Cfg = winpty_config_new(WINPTY_FLAG_COLOR_ESCAPES, out IntPtr err1);
                CheckWinPtyError(err1);
                winpty_config_set_initial_size(_WinPTY_Cfg, BufferWidth, BufferHeight);

                _WinPTY_Handle = winpty_open(_WinPTY_Cfg, out IntPtr err2);
                CheckWinPtyError(err2);

                _WinPTY_SpawnCfg = winpty_spawn_config_new(WINPTY_SPAWN_FLAG_AUTO_SHUTDOWN | WINPTY_SPAWN_FLAG_EXIT_AFTER_SHUTDOWN, exe, args, cwd, 
                    env.Length > 0 ? env.ToString() : null, out IntPtr err3);
                CheckWinPtyError(err3);

                _WinPTY_StdIN  = CreatePipe(winpty_conin_name(_WinPTY_Handle), PipeDirection.Out);
                _WinPTY_StdOUT = CreatePipe(winpty_conout_name(_WinPTY_Handle), PipeDirection.In);
                _WinPTY_StdERR = CreatePipe(winpty_conerr_name(_WinPTY_Handle), PipeDirection.In);

                if (!winpty_spawn(_WinPTY_Handle, _WinPTY_SpawnCfg, out _WinPTY_ProcessHandle, out _WinPTY_ThreadHandle, out int _, out IntPtr err4))
                    CheckWinPtyError(err4);

                Task.Run(() => { ReadOutput(_WinPTY_StdOUT, ScriptOutputType.Text); OutputFinished(); });
                //Task.Run(() => { ReadOutput(stderr, ScriptOutputType.Error); OutputFinidhed(); });
            }
        }

        public override void Stop()
        {
            lock (_SyncObject)
            {
                _WinPTY_StdIN?.Dispose();
                _WinPTY_StdOUT?.Dispose();
                _WinPTY_StdERR?.Dispose();

                if (_WinPTY_Cfg != IntPtr.Zero)
                    winpty_config_free(_WinPTY_Cfg);
                if (_WinPTY_SpawnCfg != IntPtr.Zero)
                    winpty_spawn_config_free(_WinPTY_SpawnCfg);
                if (_WinPTY_Handle != IntPtr.Zero)
                    winpty_free(_WinPTY_Handle);

                _WinPTY_StdIN         = null;
                _WinPTY_StdOUT        = null;
                _WinPTY_StdERR        = null;
                _WinPTY_Handle        = IntPtr.Zero;
                _WinPTY_ProcessHandle = IntPtr.Zero;
                _WinPTY_ThreadHandle  = IntPtr.Zero;
                _WinPTY_Cfg           = IntPtr.Zero;
                _WinPTY_SpawnCfg      = IntPtr.Zero;

                if (!string.IsNullOrWhiteSpace(_ScriptTempFile) && File.Exists(_ScriptTempFile))
                {
                    try
                    {
                        File.Delete(_ScriptTempFile);
                    }
                    catch (Exception)
                    {
                        //Do nothing
                    }
                }
                _ScriptTempFile = null;
            }
        }

        public override void Cancel()
        {
            SendCommand("\u0003");	//Ctr+C
        }

        public override void SendCommand(string command)
        {
            lock (_SyncObject)
            {
                if (ExecutionType == ScriptExecutionType.Script)
                {
                    if (_WinPTY_StdIN != null)
                        throw new Exception("Script process already runs.");

                    //Save command into temp file
                    var tempPath = Path.Combine(Path.GetTempPath(), Parameters.ApplicationName);
                    Directory.CreateDirectory(tempPath);

                    var tempFile     = Path.Combine(tempPath, Path.ChangeExtension(Guid.NewGuid().ToString("N"), DefaultExt));
                    _ScriptTempFile  = tempFile;

                    using (var writer = File.CreateText(tempFile))
                        writer.WriteLine(command);

                    StartEngineProcess(tempFile);
                }

                if (_WinPTY_StdIN == null)
                    throw new Exception("Console process is not available");

                if (command == null)
                    return;

                if (LfInSendCommand)
                    command = $"{(command.Replace("\r\n", "\n"))}\n";
                else
                    command += "\r\n";

                var bytes = Encoding.ASCII.GetBytes(command + Environment.NewLine);
                _WinPTY_StdIN.Write(bytes, 0, bytes.Length);
            }
        }

        public virtual void ExecuteExitCommand()
        {
            //Do nothing, override in concrete engines
        }

        private static Stream CreatePipe(string pipeName, PipeDirection direction)
        {
            if (string.IsNullOrWhiteSpace(pipeName))
                return null;

            string serverName = ".";
            if (pipeName.StartsWith("\\"))
            {
                int slash3 = pipeName.IndexOf('\\', 2);
                if (slash3 != -1)
                    serverName = pipeName[2..slash3];
                int slash4 = pipeName.IndexOf('\\', slash3 + 1);
                if (slash4 != -1)
                    pipeName = pipeName[(slash4 + 1)..];
            }

            var pipe = new NamedPipeClientStream(serverName, pipeName, direction);
            pipe.Connect();
            return pipe;
        }

        protected virtual void OutputFinished()
        {
            if (ExecutionType == ScriptExecutionType.Script)
                Stop();

            FireExecutionFinished();
        }

        private void ReadOutput(Stream stream, ScriptOutputType outputType)
        {
            var defaultForegroundColor = outputType == ScriptOutputType.Error ? DefaultForegroundErrorColor : DefaultForegroundColor;
            var defaultBackgroundColor = DefaultBackgroundColor;

            var ForegroundColor = defaultForegroundColor;
            var BackgroundColor = defaultBackgroundColor;

            using var reader = new StreamReader(stream, true);
            var buffer = new StringBuilder(1024);

            int iChar = reader.Read();
            if (iChar < 0)
                return;

            var c = (char)iChar;

            while (true)
            {
                buffer.Append(c);

                if (reader.Peek() < 0)
                    FlushBuffer();

                try
                {
                    iChar = reader.Read();
                }
                catch (Exception)
                {
                    //Problems with pipe
                    iChar = -1;
                }
                if (iChar < 0)
                    break;
                c = (char)iChar;
            }

            void FlushBuffer()
            {
                if (buffer.Length > 0)
                    ScriptOutputAvailable(outputType, buffer.ToString(), ForegroundColor, BackgroundColor);

                buffer.Clear();
            }
        }
    }
}
