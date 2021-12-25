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
using Markdig;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "Markdown")]
    public class WriteMarkdownCmdlet : BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Markdown-formatted text to write into book")]
        public string[] Text { get; set; }

        [Parameter(HelpMessage = "Write each string individually, without using cache")]
        public SwitchParameter Stream { get; set; }

        [Parameter(HelpMessage = "Add line breaks after each line or no.")]
        public SwitchParameter NoLineBreaks { get; set; }

        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Parameter(HelpMessage = "Convert '{...}' blocks into fields")]
        public SwitchParameter ExpandFields { get; set; }

        [Parameter(HelpMessage = "Snippets contains Books, Spreadsheets, Images and are used with Fields.")]
        public Hashtable Snippets { get; set; }


        private readonly List<string> _Output = new ();

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
                {
                    var htmlLine = Markdown.ToHtml(line);
                    _Output.Add(htmlLine);
                }

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

        protected void WriteText(Document book, List<string> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteText(book, buffer, lastBlock));
        }

        protected virtual void DoWriteText(Document book, List<string> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            book.BeginUpdate();
            try
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                foreach (var line in buffer)
                {
                    var lineRange = book.AppendHtmlText(line, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

                    if (rangeStart == null)
                        rangeStart = lineRange.Start;

                    if (!NoLineBreaks)
                        lineRange = book.AppendText(Environment.NewLine);

                    rangeEnd = lineRange.End;
                }

                if (lastBlock && rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());

                    if (!string.IsNullOrWhiteSpace(ParagraphStyle))
                    {
                        var style = book.ParagraphStyles[ParagraphStyle] ?? throw new Exception($"Paragraph style '{ParagraphStyle}' does not exist.");
                        var pp = book.BeginUpdateParagraphs(range);
                        try
                        {
                            pp.Style = style;
                        }
                        finally
                        {
                            book.EndUpdateParagraphs(pp);
                        }
                    }

                    AddComments(book, range);

                    if (ExpandFields)
                        ExpandFieldsInBookRange(range, HostSpreadsheet, Snippets);

                    WriteRangeToConsole(book, range);
                }

                if (rangeEnd != null)
                {
                    book.CaretPosition = rangeEnd;
                    ScrollToCaret();
                }
            }
            finally
            {
                ResetBookFormatting(book);
                book.EndUpdate();
            }
        }
    }
}
