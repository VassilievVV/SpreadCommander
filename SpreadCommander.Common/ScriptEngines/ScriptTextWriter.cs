using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;

namespace SpreadCommander.Common.ScriptEngines
{
    public class ScriptTextWriter : TextWriter
    {
        private readonly BaseScriptEngine _Engine;
        protected readonly bool _IsError;

        protected Document Document => _Engine.Document;

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

            _Engine.ExecuteSynchronized(() => DoWrite(value));
        }

        protected void DoWrite(string value)
        {
            var doc   = Document;
            doc.BeginUpdate();
            try
            {
                var range = doc.AppendText(value);

                FormatText(doc, range.Start.ToInt(), range.End.ToInt() - range.Start.ToInt());

                doc.CaretPosition = range.End;
                DoResetBookFormatting(doc, range.End, false);
            }
            finally
            {
                doc.EndUpdate();
            }

            ScrollToCaret();
        }

        public override void WriteLine()
        {
            if (Silent)
                return;

            _Engine.ExecuteSynchronized(() => DoWriteLine());
        }

        protected void DoWriteLine()
        {
            var doc   = Document;
            doc.BeginUpdate();
            try
            {
                var range = doc.AppendText(NewLine);

                FormatText(doc, range.Start.ToInt(), range.End.ToInt() - range.Start.ToInt());

                doc.CaretPosition = range.End;
                DoResetBookFormatting(doc, range.End, false);
            }
            finally
            {
                doc.EndUpdate();
            }

            ScrollToCaret();
        }

        public override void WriteLine(string value)
        {
            if (Silent)
                return;

            _Engine.ExecuteSynchronized(() => DoWriteLine(value));
        }

        protected void DoWriteLine(string value)
        {
            var doc = Document;
            doc.BeginUpdate();
            try
            {
                var range        = doc.AppendText(value);
                var rangeNewLine = doc.AppendText(NewLine);
                
                FormatText(doc, range.Start.ToInt(), rangeNewLine.End.ToInt() - range.Start.ToInt());
                
                doc.CaretPosition = rangeNewLine.End;
                DoResetBookFormatting(doc, range.End, false);
            }
            finally
            {
                doc.EndUpdate();
            }

            ScrollToCaret();
        }

        public void WriteInvitation(string value = null)
        {
            if (Silent)
                return;

            _Engine.ExecuteSynchronized(() => DoWriteInvitation(value));
        }

        protected void DoWriteInvitation(string value = null)
        {
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
