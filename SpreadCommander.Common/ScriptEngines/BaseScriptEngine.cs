using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class BaseScriptEngine: IDisposable
    {
        public enum ScriptOutputType        { Text, Error }
        public enum ProgressKind            { None, Undetermined, Value }

        public enum ScriptExecutionType     { Interactive, Script }

        public enum ScriptApplicationType   { UI, Console }

        #region ScriptOutputArgs
        public class ScriptOutputMessage
        {
            public ScriptOutputType OutputType { get; set; }
            public string Text { get; set; }
            public Color ForegroundColor { get; set; }
            public Color BackgroundColor { get; set; }

            public bool IsFormattingEqual(ScriptOutputMessage args2)
            {
                return (args2 != null && args2.ForegroundColor == this.ForegroundColor && args2.BackgroundColor == this.BackgroundColor);
            }
        }
        #endregion

        #region ViewFileArgs
        public class ViewFileRequestArgs : EventArgs
        {
            public string FileName { get; set; }
            public StringNoCaseDictionary<string> Parameters { get; set; }
            public List<BaseCommand> Commands { get; set; }
        }
        #endregion

        #region ReadLineArgs
        public class ReadLineArgs : EventArgs
        {
            public string Line { get; set; }
        }
        #endregion

        public const int DefaultBufferCapacity = 65536;

        public static readonly Color DefaultBackgroundColor      = SystemColors.Window;
        public static readonly Color DefaultForegroundColor      = SystemColors.WindowText;
        public static readonly Color DefaultForegroundErrorColor = Color.Red;

        public event EventHandler ScriptOutput;
        public event EventHandler TitleChanged;
        public event EventHandler NeedScrollToCaret;
        public event EventHandler<ViewFileRequestArgs> ViewFileRequest;
        public event EventHandler ProgressChanged;
        public event EventHandler<ReadLineArgs> RequestLine;
        public event EventHandler ExecutionFinished;    //Required for script mode, optional for interactive mode

        public static ScriptApplicationType ApplicationType { get; private set; }

        public static ISynchronizeInvoke StaticSynchronizeInvoke { get; set; }

        public virtual ScriptExecutionType ExecutionType	{ get; set; }

        public virtual string EngineName					{ get; }
        public virtual string DefaultExt					{ get; }
        public virtual string FileFilter					{ get; }
        public virtual string SyntaxFile					{ get; }
        public virtual bool LfInSendCommand					{ get; }
        public virtual ISynchronizeInvoke SynchronizeInvoke { get; set; }

        public virtual bool Silent                          { get; set; }

        public static void SetApplicationType(ScriptApplicationType value)
        {
            ApplicationType = value;
        }

        private ProgressKind _ProgressType;
        public virtual ProgressKind ProgressType
        {
            get => _ProgressType;
            set
            {
                if (_ProgressType == value)
                    return;

                _ProgressType = value;
                OnProgressChanged();
            }
        }

        private int _ProgressValue;
        public virtual int ProgressValue
        {
            get => _ProgressValue;
            set
            {
                if (_ProgressValue == value)
                    return;

                _ProgressValue = value;
                OnProgressChanged();
            }
        }

        private string _ProgressStatus;
        public virtual string ProgressStatus
        {
            get => _ProgressStatus;
            set
            {
                if (_ProgressStatus == value)
                    return;

                _ProgressStatus = value;
                OnProgressChanged();
            }
        }

        private int _ProgrssMax;
        public virtual int ProgressMax
        {
            get => _ProgrssMax;
            set
            {
                if (_ProgrssMax == value)
                    return;

                _ProgrssMax = value;
                OnProgressChanged();
            }
        }

        public virtual void UpdateProgress(ProgressKind progressType, int value, int max, string status)
        {
            _ProgressType   = progressType;
            _ProgrssMax     = max;
            _ProgressValue  = value;
            _ProgressStatus = status;

            OnProgressChanged();
        }

        private IRichEditDocumentServer _BookServer;
        public virtual IRichEditDocumentServer BookServer
        {
            get => _BookServer;
            set { _BookServer = value; ProcessOutputQueue(); }
        }

        public virtual Document Document => BookServer?.Document;

        public virtual IWorkbook Workbook	    { get; set; }
        public virtual DataSet GridDataSet	    { get; set; }
        public virtual IFileViewer FileViewer   { get; set; }

        private string _Title;
        public virtual string Title
        {
            get => _Title;
            set { _Title = value; TitleChanged?.Invoke(this, EventArgs.Empty); }
        }

        public virtual Queue<ScriptOutputMessage> OutputQueue { get; } = new Queue<ScriptOutputMessage>();

        public virtual void Start() { }

        public virtual void Stop() 
        {
            GC.Collect();
        }

        public virtual void Dispose()
        {
            Stop(); 
            GC.SuppressFinalize(this);
        }

        public virtual void SendCommand(string command) { }

        public virtual void ScriptOutputAvailable(ScriptOutputType outputType, string output,
            Color foregroundColor, Color backgroundColor)
        {
            OutputQueue.Enqueue(new ScriptOutputMessage() { OutputType = outputType, Text = output,
                ForegroundColor = foregroundColor, BackgroundColor = backgroundColor });
            ProcessOutputQueue();

            ScriptOutput?.Invoke(this, new EventArgs());
        }

        public virtual void ScrollToCaret()
        {
            NeedScrollToCaret?.Invoke(this, new EventArgs());
        }

        protected virtual void OnProgressChanged()
        {
            ProgressChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual string RequestInputLine()
        {
            var args = new ReadLineArgs();
            RequestLine?.Invoke(this, args);
            return args.Line;
        }

        protected virtual void FireViewFileRequest(ViewFileRequestArgs args)
        {
            ViewFileRequest?.Invoke(this, args);
        }

        public virtual void ListScriptIntellisenseItems(string fileName, string text, string[] lines, Point caretPosition, ScriptIntellisense intellisense) { }

        public virtual void ListScriptParseErrors(string text, List<ScriptParseError> errors) { }

        public virtual void FireExecutionFinished()
        {
            ExecutionFinished?.Invoke(this, EventArgs.Empty);
        }

        public virtual void ExecuteSynchronized(Action action)
        {
            var sync = SynchronizeInvoke ?? StaticSynchronizeInvoke;
            if (sync?.InvokeRequired ?? false)
                _ = sync.Invoke(action, Array.Empty<object>());
            else
                action();
        }
    }
}
