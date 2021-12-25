using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using System.Collections;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "ErrorMessage")]
    public class WriteErrorMessageCmdlet : BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Text to write into book")]
        public string[] Text { get; set; }

        [Parameter(HelpMessage = "Write each string individually, without using cache")]
        public SwitchParameter Stream { get; set; }


        private readonly StringBuilder _Output = new ();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var text = Text;
            if (text == null || text.Length <= 0)
                text = new string[] { string.Empty };

            foreach (var line in text)
            {
                if (line != null)
                    _Output.Append(line);

                if (Stream)
                    WriteBuffer(false);
            }
        }

        protected override void EndProcessing()
        {
            WriteBuffer(true);
        }

        protected void WriteBuffer(bool lastBlock)
        {
            WriteText(GetCmdletBook(), _Output, lastBlock);
            _Output.Clear();
        }

        protected void WriteText(Document book, StringBuilder buffer, bool lastBlock)
        {
            if (buffer.Length <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteText(book, buffer, lastBlock));
        }

        protected virtual void DoWriteText(Document book, StringBuilder buffer, bool lastBlock)
        {
            if (buffer.Length <= 0 && !lastBlock)
                return;

            book.BeginUpdate();
            try
            {
                var text = buffer.ToString();

                var range = book.AppendText(text);

                var foregroundColor = Color.Red;
                var backgroundColor = SystemColors.Window;

                var cp = book.BeginUpdateCharacters(range);
                try
                {

                    if (foregroundColor != SystemColors.WindowText)
                        cp.ForeColor = foregroundColor;
                    if (backgroundColor != SystemColors.Window)
                        cp.BackColor = backgroundColor;
                }
                finally
                {
                    book.EndUpdateCharacters(cp);
                }

                if (lastBlock)
                    AddComments(book, range);

                if (range?.End != null)
                {
                    book.CaretPosition = range.End;
                    ScrollToCaret();
                }

                WriteErrorToConsole(text);
            }
            finally
            {
                ResetBookFormatting(book);
                book.EndUpdate();
            }
        }
    }
}
