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
using DevExpress.XtraRichEdit;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsData.Merge, "Book")]
    public class MergeBookCmdlet : BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Document to write into book. Document shall be created using cmdlet New-Book.")]
        public SCBookContext Content { get; set; }

        [Parameter(HelpMessage = "Write each document individually, without using cache")]
        public SwitchParameter Stream { get; set; }


        protected override bool NeedSynchronization() => base.NeedSynchronization() || (Content == null || Content.BookServer.Document == HostBook);


        private readonly List<SCBookContext> _Output = new List<SCBookContext>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            if (Content == null)
                throw new Exception("Merging book cannot be NULL.");

            _Output.Add(Content);

            if (Stream)
                WriteBuffer(false);
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

        protected void WriteText(Document book, List<SCBookContext> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteText(book, buffer, lastBlock));
        }

        protected virtual void DoWriteText(Document book, List<SCBookContext> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            using (new UsingProcessor(
                () => book.BeginUpdate(),
                () => { ResetBookFormatting(book); book.EndUpdate(); }))
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                foreach (var content in buffer)
                {
                    content.Document.UpdateAllFields();

                    DocumentRange range;
                    using (var stream = new MemoryStream())
                    {
                        content.Document.SaveDocument(stream, DocumentFormat.OpenXml);
                        stream.Seek(0, SeekOrigin.Begin);
                        range = book.AppendDocumentContent(stream, DocumentFormat.OpenXml);
                    }

                    if (rangeStart == null)
                        rangeStart = range.Start;

                    rangeEnd = range.End;
                }

                if (lastBlock && rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());
                    AddComments(book, range);
                }

                book.CaretPosition = rangeEnd;
                ScrollToCaret();
            }
        }
    }
}
