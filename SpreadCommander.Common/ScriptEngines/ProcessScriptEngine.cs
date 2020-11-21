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

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class ProcessScriptEngine: BaseScriptEngine
    {
        //Set buffer large enough for commands. For most programs should be divided by 80.
        public const int BufferWidth  = 80;
        public const int BufferHeight = 32;

        public static readonly Color DefaultBackgroundColor      = SystemColors.Window;
        public static readonly Color DefaultForegroundColor      = SystemColors.WindowText;
        public static readonly Color DefaultForegroundErrorColor = Color.Red;

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
        private string _ScriptTempFile           = null;
        private readonly object _SyncObject  = new object();

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

                Task.Run(() => ReadOutput(_WinPTY_StdOUT, ScriptOutputType.Text));
                //Task.Run(() => ReadOutput(stderr, ScriptOutputType.Error));
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
            var defaultBackgroungColor = DefaultBackgroundColor;

            var ForegroundColor = defaultForegroundColor;
            var BackgroundColor = defaultBackgroungColor;

            using var reader = new StreamReader(stream, true);
            var buffer = new StringBuilder(1024);

            int iChar = reader.Read();
            if (iChar < 0)
            {
                OutputFinished();
                return;
            }

            var c = (char)iChar;

            while (true)
            {
                if (c == '\u001B')   //ESC
                {
                    //https://en.wikipedia.org/wiki/ANSI_escape_code
                    //http://invisible-island.net/xterm/ctlseqs/ctlseqs.html
                    var c2 = (char)reader.Read();
                    switch (c2)
                    {
                        case '[':
                            /*
                            The ESC[ is followed by any number(including none) of "parameter bytes" in the range 0x30–0x3F(ASCII 0–9:;<=>?), 
                            then by any number of "intermediate bytes" in the range 0x20–0x2F(ASCII space and !"#$%&'()*+,-./), 
                            then finally by a single "final byte" in the range 0x40–0x7E (ASCII @A–Z[\]^_`a–z{|}~).[14]:5.4
                            */
                            var buffer2 = new StringBuilder(128);
                            char closingChar = '\0';

                            while (true)
                            {
                                if (reader.Peek() <= 0)
                                    break;
                                var cc3 = reader.Read();
                                if (cc3 >= 0x40 && cc3 <= 0x7E)
                                {
                                    closingChar = (char)cc3;
                                    break;
                                }

                                buffer2.Append((char)cc3);
                            }

                            switch (closingChar)
                            {

                               //A-D Moves the cursor n {\displaystyle n} n (default 1) cells in the given direction. 
                               //If the cursor is already at the edge of the screen, this has no effect. 
                                case 'A':
                               //CUU – Cursor Up
                                    break;
                                case 'B':
                                    //CUD – Cursor Down 
                                    break;
                                case 'C':
                                    //CUF – Cursor Forward 
                                    break;
                                case 'D':
                                    //CUB – Cursor Back
                                    break;
                                case 'E':
                                    //CNL – Cursor Next Line
                                    //Moves cursor to beginning of the line n {\displaystyle n} n (default 1) lines down. (not ANSI.SYS) 
                                    break;
                                case 'F':
                                    //CPL – Cursor Previous Line
                                    //Moves cursor to beginning of the line n {\displaystyle n} n (default 1) lines up. (not ANSI.SYS) 
                                    break;
                                case 'G':
                                    //CHA – Cursor Horizontal Absolute
                                    //Moves the cursor to column n (default 1). (not ANSI.SYS)
                                    break;
                                case 'H':
                                    //CUP – Cursor Position
                                    //Moves the cursor to row n, column m. 
                                    //The values are 1-based, and default to 1 (top left corner) if omitted. 
                                    //A sequence such as CSI ;5H is a synonym for CSI 1;5H as well as CSI 17;H is the same as CSI 17H and CSI 17;1H
                                    break;
                                case 'J':
                                    //ED – Erase in Display
                                    //Clears part of the screen.If n 
                                    //n is 0(or missing), clear from cursor to end of screen.If n 
                                    //n is 1, clear from cursor to beginning of the screen. If n 
                                    //n is 2, clear entire screen(and moves cursor to upper left on DOS ANSI.SYS).If n
                                    //n is 3, clear entire screen and delete all lines saved in the scrollback buffer(this feature was added for xterm and is supported by other terminal applications).
                                    break;
                                case 'K':
                                    //EL – Erase in Line
                                    // 	Erases part of the line. 
                                    //If n is 0 (or missing), clear from cursor to the end of the line.
                                    //If n is 1, clear from cursor to beginning of the line. 
                                    //If n is 2, clear entire line. Cursor position does not change. 
                                    break;
                                case 'S':
                                    //SU – Scroll Up
                                    //Scroll whole page up by n 
                                    //n(default 1) lines.New lines are added at the bottom. (not ANSI.SYS)
                                    break;
                                case 'T':
                                    //SD – Scroll Down
                                    //Scroll whole page down by n (default 1) lines.New lines are added at the top. (not ANSI.SYS)
                                    break;
                                case 'f':
                                    //HVP – Horizontal Vertical Position
                                    //Same as CUP 
                                    break;
                                case 'm':
                                    //SGR – Select Graphic Rendition
                                    //Sets the appearance of the following characters, see SGR parameters below. 

                                    //Send current output to console
                                    FlushBuffer();

                                    var strBuffer2 = buffer2.ToString()?.Trim() ?? string.Empty;
                                    if (strBuffer2 == string.Empty)
                                        strBuffer2 = "0";

                                    int sgrCode = 0;
                                    string sgrValue = null;

                                    var sgrP = strBuffer2.IndexOfAny(new char[] { ';', ':' });
                                    if (sgrP >= 0)
                                    {
                                        if (!int.TryParse(strBuffer2.Substring(0, sgrP), out sgrCode))
                                            sgrCode = 0;
                                        sgrValue = strBuffer2[(sgrP + 1)..].Trim();
                                    }
                                    else
                                    {
                                        if (!int.TryParse(strBuffer2, out sgrCode))
                                            sgrCode = 0;
                                    }

                                    /*
                                     Code 	Effect 	Note
                                    0 	Reset / Normal 	all attributes off
                                    1 	Bold or increased intensity 	
                                    2 	Faint (decreased intensity) 	
                                    3 	Italic 	Not widely supported. Sometimes treated as inverse.
                                    4 	Underline 	
                                    5 	Slow Blink 	less than 150 per minute
                                    6 	Rapid Blink 	MS-DOS ANSI.SYS; 150+ per minute; not widely supported
                                    7 	reverse video 	swap foreground and background colors
                                    8 	Conceal 	Not widely supported.
                                    9 	Crossed-out 	Characters legible, but marked for deletion.
                                    10 	Primary(default) font 	
                                    11–19 	Alternative font 	Select alternative font n − 10 {\displaystyle n-10} {\displaystyle n-10}
                                    20 	Fraktur 	Rarely supported
                                    21 	Bold off 	all popular terminals but xterm and PuTTY (which do underline)
                                    22 	Normal color or intensity 	Neither bold nor faint
                                    23 	Not italic, not Fraktur 	
                                    24 	Underline off 	Not singly or doubly underlined
                                    25 	Blink off 	
                                    27 	Inverse off 	
                                    28 	Reveal 	conceal off
                                    29 	Not crossed out 	
                                    30–37 	Set foreground color 	See color table below
                                    38 	Set foreground color 	Next arguments are 5;n or 2;r;g;b, see below
                                    39 	Default foreground color 	implementation defined (according to standard)
                                    40–47 	Set background color 	See color table below
                                    48 	Set background color 	Next arguments are 5;n or 2;r;g;b, see below
                                    49 	Default background color 	implementation defined (according to standard)
                                    51 	Framed 	
                                    52 	Encircled 	
                                    53 	Overlined 	
                                    54 	Not framed or encircled 	
                                    55 	Not overlined 	
                                    60 	ideogram underline or right side line 	Rarely supported
                                    61 	ideogram double underline or
                                    double line on the right side
                                    62 	ideogram overline or left side line
                                    63 	ideogram double overline or
                                    double line on the left side
                                    64 	ideogram stress marking
                                    65 	ideogram attributes off 	reset the effects of all of 60–64
                                    90–97 	Set bright foreground color 	aixterm (not in standard)
                                    100–107 	Set bright background color 	aixterm (not in standard) 
                                    */
                                    switch (sgrCode)
                                    {
                                        case 0:
                                            //Reset / Normal  all attributes off
                                            ForegroundColor = defaultForegroundColor;
                                            BackgroundColor = defaultBackgroungColor;
                                            break;
                                        //30–37   Set foreground color
                                        case 30:
                                            ForegroundColor = Color.Black;
                                            break;
                                        case 31:
                                            ForegroundColor = Color.Red;
                                            break;
                                        case 32:
                                            ForegroundColor = Color.Green;
                                            break;
                                        case 33:
                                            ForegroundColor = Color.Yellow;
                                            break;
                                        case 34:
                                            ForegroundColor = Color.Blue;
                                            break;
                                        case 35:
                                            ForegroundColor = Color.Magenta;
                                            break;
                                        case 36:
                                            ForegroundColor = Color.Cyan;
                                            break;
                                        case 37:
                                            ForegroundColor = Color.White;
                                            break;
                                        case 38:
                                            //Set foreground color. Next arguments are 5;n or 2;r;g;b
                                            //Not implemented, reset to default
                                            ForegroundColor = defaultForegroundColor;
                                            break;
                                        case 39:
                                            //Default foreground color
                                            ForegroundColor = defaultForegroundColor;
                                            break;
                                        //40-47	Set background color
                                        case 40:
                                            BackgroundColor = Color.Black;
                                            break;
                                        case 41:
                                            BackgroundColor = Color.Red;
                                            break;
                                        case 42:
                                            BackgroundColor = Color.Green;
                                            break;
                                        case 43:
                                            BackgroundColor = Color.Yellow;
                                            break;
                                        case 44:
                                            BackgroundColor = Color.Blue;
                                            break;
                                        case 45:
                                            BackgroundColor = Color.Magenta;
                                            break;
                                        case 46:
                                            BackgroundColor = Color.Cyan;
                                            break;
                                        case 47:
                                            BackgroundColor = Color.White;
                                            break;
                                        case 48:
                                            //Set background color. Next arguments are 5;n or 2;r;g;b
                                            //Not implemented, reset to default
                                            BackgroundColor = defaultForegroundColor;
                                            break;
                                        case 49:
                                            //Default foreground color
                                            BackgroundColor = defaultBackgroungColor;
                                            break;
                                        //Set bright foreground color 	aixterm (not in standard) 
                                        case 90:
                                            ForegroundColor = Color.DarkGray;
                                            break;
                                        case 91:
                                            ForegroundColor = Color.Pink;
                                            break;
                                        case 92:
                                            ForegroundColor = Color.LightGreen;
                                            break;
                                        case 93:
                                            ForegroundColor = Color.LightYellow;
                                            break;
                                        case 94:
                                            ForegroundColor = Color.LightBlue;
                                            break;
                                        case 95:
                                            ForegroundColor = Color.LightSteelBlue;
                                            break;
                                        case 96:
                                            ForegroundColor = Color.LightCyan;
                                            break;
                                        case 97:
                                            ForegroundColor = Color.LightGray;
                                            break;
                                        //Set bright background color 	aixterm (not in standard) 
                                        case 100:
                                            BackgroundColor = Color.DarkGray;
                                            break;
                                        case 109:
                                            BackgroundColor = Color.Pink;
                                            break;
                                        case 102:
                                            BackgroundColor = Color.LightGreen;
                                            break;
                                        case 103:
                                            BackgroundColor = Color.LightYellow;
                                            break;
                                        case 104:
                                            BackgroundColor = Color.LightBlue;
                                            break;
                                        case 105:
                                            BackgroundColor = Color.LightSteelBlue;
                                            break;
                                        case 106:
                                            BackgroundColor = Color.LightCyan;
                                            break;
                                        case 107:
                                            BackgroundColor = Color.LightGray;
                                            break;
                                    }
                                    break;
                                case 'i':
                                    //5i - AUX Port On
                                    //Enable aux serial port usually for local serial printer
                                    //4i - AUX Port Off
                                    //Disable aux serial port usually for local serial printer 
                                    break;
                                case 'n':
                                    //6n - DSR – Device Status Report
                                    //Reports the cursor position (CPR) to the application as (as though typed at the keyboard) 
                                    //ESC[n;mR, where n is the row and m is the column.) 
                                    break;
                                case 's':
                                    //SCP – Save Cursor Position
                                    //Saves the cursor position/state. 
                                    break;
                                case 'u':
                                    //RCP – Restore Cursor Position
                                    //Restores the cursor position/state. 
                                    break;
                                //private sequences
                                case 'h':
                                    //CSI ?25h 		DECTCEM Shows the cursor, from the VT320. 
                                    //CSI ?1049h 	Enable alternative screen buffer 
                                    //CSI ?2004h 	Turn on bracketed paste mode. Text pasted into the terminal will be surrounded by 
                                    //				ESC [200~ and ESC [201~, and characters in it should not be treated as commands 
                                    //				(for example in Vim).[16] From Unix terminal emulators. 
                                    break;
                                case 'l':
                                    //CSI ?25l 		DECTCEM Hides the cursor. 
                                    //CSI ?1049l 	Disable alternative screen buffer 
                                    //CSI ?2004l 	Turn off bracketed paste mode. 
                                    break;
                            }
                            break;
                        case ']':
                            /*
                            Starts a control string for the operating system to use, terminated by ST.[14]:8.3.89 
                            In xterm, they may also be terminated by BEL.[15] 
                            In xterm, the window title can be set by OSC 0; this is the window title BEL. 
                            */
                            var c3 = (char)reader.Read();
                            var c4 = (char)reader.Read();

                            if (c3 == '0' && c4 == ';')
                            {
                                var title = new StringBuilder(128);
                                while (true)
                                {
                                    if (reader.Peek() <= 0)
                                        break;
                                    var c5 = (char)reader.Read();
                                    if (c5 == '\u0007') //BEL
                                        break;
                                    title.Append(c5);
                                };
                                //Set caption
                                Title = title.ToString();
                            }
                            break;
                        case 'N':
                            /*
                            SS2 – Single Shift Two 	Select a single character from one of the alternative character sets. 
                            In xterm, SS2 selects the G2 character set, and SS3 selects the G3 character set.[15]
                            */
                            break;
                        case 'O':
                            /*
                            SS3 – Single Shift Three  	Select a single character from one of the alternative character sets. 
                            In xterm, SS2 selects the G2 character set, and SS3 selects the G3 character set.[15]
                            */
                            break;
                        case 'P':
                            /*
                            DCS – Device Control String 	Terminated by ST. 
                            Xterm's uses of this sequence include defining User-Defined Keys, 
                            and requesting or setting Termcap/Term-info data.[15]  
                            */
                            break;
                        case '\\':
                            /* 	
                            ST – String Terminator 	Terminates strings in other controls.[14]:8.3.143
                            */
                            break;
                        case 'X':
                            /*
                            SOS – Start of String 
                            Takes an argument of a string of text, terminated by ST.
                            The uses for these string control sequences are defined by the application[14]:8.3.2, 8.3.128 or 
                            privacy discipline.[14]:8.3.94 These functions are not implemented and the arguments are ignored by xterm.[15]
                            */
                            break;
                        case '^':
                            /*
                            PM – Privacy Message 
                            Takes an argument of a string of text, terminated by ST.
                            The uses for these string control sequences are defined by the application[14]:8.3.2, 8.3.128 or 
                            privacy discipline.[14]:8.3.94 These functions are not implemented and the arguments are ignored by xterm.[15]
                            */
                            break;
                        case '_':
                            /*
                            APC – Application Program Command
                            Takes an argument of a string of text, terminated by ST.
                            The uses for these string control sequences are defined by the application[14]:8.3.2, 8.3.128 or 
                            privacy discipline.[14]:8.3.94 These functions are not implemented and the arguments are ignored by xterm.[15]
                            */
                            break;
                        case 'c':
                            /*
                            RIS – Reset to Initial State 	
                            Resets the device to its original state. 
                            This may include (if applicable): reset graphic rendition, 
                            clear tabulation stops, reset to default font, and more. 
                            */
                            break;
                    }
                }
                else
                    buffer.Append(c);

                if (reader.Peek() < 0)
                    FlushBuffer();

                iChar = reader.Read();
                if (iChar < 0)
                    break;
                c = (char)iChar;
            }

            OutputFinished();


            void FlushBuffer()
            {
                if (buffer.Length > 0)
                    ScriptOutputAvailable(outputType, buffer.ToString(), ForegroundColor, BackgroundColor);

                buffer.Clear();
            }
        }
    }
}
