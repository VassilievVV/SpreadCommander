using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;

namespace SpreadCommander.Common.ScriptEngines
{
    public class ScriptTextWriter : TextWriter
    {
        private readonly BaseScriptEngine _Engine;
        protected readonly bool _IsError;
        private readonly StringBuilder _Output = new();

        protected readonly static TimeSpan _IntervalCache       = TimeSpan.FromMilliseconds(1);
        protected readonly static TimeSpan _IntervalForceOutput = TimeSpan.FromMilliseconds(100);

        protected Document Document => _Engine.Document;

        public object LockObject => _Output;

        protected Task _TaskWrite;
        protected DateTime _LastWrite;
        protected DateTime _LastOutput;

        public ScriptTextWriter(BaseScriptEngine engine, bool isError = false)
        {
            _Engine  = engine;
            _IsError = isError;
        }

        public void InitializeOutput()
        {
            Silent = false;
        }

        private readonly Encoding _Encoding = Encoding.Default;
        public override Encoding Encoding => _Encoding;

        public bool Silent { get; set; } = true;

        public bool IsEmpty
        {
            get
            {
                lock (LockObject)
                {
                    Flush();
                    return _Output.Length <= 0;
                }
            }
        }

        public override void Write(char value)
        {
            throw new NotImplementedException();
        }

        public override void Write(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override void Write(string value)
        {
            if (Silent)
                return;

            lock (LockObject)
            {
                _Output.Append(value);
            }

            _LastOutput = DateTime.Now;

            if (_TaskWrite != null)
                return;

            _TaskWrite = Task.Run(() =>
            {
                var now = DateTime.Now;

                //Wait next output
                while ((now - _LastOutput < _IntervalCache) && (now - _LastWrite < _IntervalForceOutput))
                    Thread.Sleep(_IntervalCache);

                lock (LockObject)
                    _TaskWrite = null;

                _Engine.ExecuteSynchronized(() => DoWrite());
            });
        }

        protected internal void DoWrite()
        {
            _LastWrite = DateTime.Now;

            if (IsEmpty)
                return;

            var doc = Document;
            doc.BeginUpdate();
            try
            {
                do
                {
                    lock (LockObject)
                    {
                        var backColor = BaseScriptEngine.DefaultBackgroundColor;
                        var foreColor = _IsError ? BaseScriptEngine.DefaultForegroundErrorColor : BaseScriptEngine.DefaultForegroundColor;
                        _Engine.FlushTextBuffer(doc, foreColor, backColor, _Output);
                        _Output.Clear();
                    }
                }
                while (!IsEmpty);
            }
            finally
            {
                doc.EndUpdate();
            }

            ScrollToCaret();

            if (!IsEmpty)
                DoWrite();
        }

        public override void WriteLine() =>
            Write("\n");

        public override void WriteLine(string value) =>
            Write(value + "\n");

        public void WriteInvitation(string value = null)
        {
            if (Silent)
                return;

            Write(string.Empty);

            _Engine.ExecuteSynchronized(() => DoWriteInvitation(value));
        }

        protected void DoWriteInvitation(string value = null)
        {
            DoWrite();

            var doc = Document;
            doc.BeginUpdate();
            try
            {
                if (value == null)
                    value = "\nSpreadCommander>\n";
                else
                    value = $"\n>> {value}\n";

                var range = doc.AppendText(value);

                var cp = doc.BeginUpdateCharacters(range);
                try
                {
                    cp.Reset();

                    cp.BackColor = SystemColors.Window;
                    cp.ForeColor = Color.Gray;
                }
                finally
                {
                    doc.EndUpdateCharacters(cp);
                }

                doc.CaretPosition = range.End;
                DoResetBookFormatting(doc, range.End, false);
            }
            finally
            {
                doc.EndUpdate();
            }

            ScrollToCaret();
        }

        protected void DoResetBookFormatting(Document book, DocumentPosition pos, bool start)
        {
            var cp = book.BeginUpdateCharacters(pos, 0);
            try
            {
                cp.Reset();

                if (start && _IsError)
                {
                    cp.BackColor = SystemColors.Window;
                    cp.ForeColor = Color.Red;
                }
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }
        }

        protected void FormatText(Document book, int start, int length)
        {
            var cp = book.BeginUpdateCharacters(start, length);
            try
            {
                cp.Reset();

                if (_IsError)
                {
                    cp.BackColor = SystemColors.Window;
                    cp.ForeColor = Color.Red;
                }
            }
            finally
            {
                book.EndUpdateCharacters(cp);
            }
        }

        protected void ScrollToCaret()
        {
            _Engine.ScrollToCaret();
        }
    }
}
