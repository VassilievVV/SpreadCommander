using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using System.IO;
using Markdig;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "Content")]
    public class WriteContentCmdlet : BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Filenames of documents to write into book. Word, Epub, HTML, RTF, Open Office documents are allowed.")]
        public string[] FileNames { get; set; }

        [Parameter(HelpMessage = "Write each file individually, without using cache")]
        public SwitchParameter Stream { get; set; }

        [Parameter(HelpMessage = "Add line breaks after each line or no.")]
        public SwitchParameter NoLineBreaks { get; set; }


        private readonly List<string> _Output = new ();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var text = FileNames;
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
                    var fileName = Project.Current.MapPath(line);

                    DocumentRange range;

                    var ext = Path.GetExtension(fileName)?.ToLower();
                    switch (ext)
                    {
#pragma warning disable CRRSP06 // A misspelled word has been found
                        case "markdown":
                        case "mdown":
                        case "md":
                            var content     = File.ReadAllText(fileName);
                            var htmlContent = Markdown.ToHtml(content);
                            range           = book.AppendHtmlText(htmlContent, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);
                            break;
                        default:
                            range = book.AppendDocumentContent(fileName);
                            break;
#pragma warning restore CRRSP06 // A misspelled word has been found
                    }

                    if (rangeStart == null)
                        rangeStart = range.Start;

                    if (!NoLineBreaks)
                        range = book.AppendText(Environment.NewLine);

                    rangeEnd = range.End;
                }

                if (lastBlock && rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());
                    AddComments(book, range);
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
