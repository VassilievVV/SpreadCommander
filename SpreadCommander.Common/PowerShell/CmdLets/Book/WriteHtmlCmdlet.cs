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
    [Cmdlet(VerbsCommunications.Write, "Html")]
    public class WriteHtmlCmdlet : BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "HTML-formatted text to write into book")]
        public string[] Text { get; set; }

        [Parameter(HelpMessage = "Write each string individually, without using cache")]
        public SwitchParameter Stream { get; set; }

        [Parameter(HelpMessage = "Add line breaks after each line or no.")]
        public SwitchParameter NoLineBreaks { get; set; }

        [Parameter(HelpMessage = "Retains styles associated with the target document when style definition conflicts occur. Incompatible with 'KeepSourceFormatting'.")]
        [Alias("DestStyles", "ds")]
        public SwitchParameter UseDestinationStyles { get; set; }

        [Parameter(HelpMessage = "Retains character styles and direct formatting (font size, emphasis, etc.) applied to the inserted text. Incompatible with 'UseDestinationStyles'.")]
        [Alias("KeepFormatting", "ks")]
        public SwitchParameter KeepSourceFormatting { get; set; }

        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Parameter(HelpMessage = "Convert '{...}' blocks into fields")]
        public SwitchParameter ExpandFields { get; set; }

        [Parameter(HelpMessage = "Snippets contains Books, Spreadsheets, Images and are used with Fields.")]
        public Hashtable Snippets { get; set; }


        private readonly List<string> _Output = new List<string>();

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
                    _Output.Add(line);

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
                    var insertOptions = string.IsNullOrWhiteSpace(ParagraphStyle) ? InsertOptions.KeepSourceFormatting : InsertOptions.UseDestinationStyles;
                    if (UseDestinationStyles) insertOptions = InsertOptions.UseDestinationStyles;
                    if (KeepSourceFormatting) insertOptions = InsertOptions.KeepSourceFormatting;
                    var lineRange = book.AppendHtmlText(line, insertOptions);

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
                        ExpandFieldsInBookRange(range, Snippets);

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
