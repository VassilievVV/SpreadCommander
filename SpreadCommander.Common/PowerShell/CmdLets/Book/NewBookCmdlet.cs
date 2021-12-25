using DevExpress.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.New, "Book")]
    [OutputType(typeof(SCBookContext))]
    public class NewBookCmdlet: SCCmdlet
    {
        [Parameter(Position = 0, HelpMessage = "Name of file to load content from")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }

        protected override void EndProcessing()
        {
            var book = new SCBookContext();

            if (!string.IsNullOrWhiteSpace(FileName))
            {
                var fileName = Project.Current.MapPath(FileName);

                var ext = Path.GetExtension(fileName)?.ToLower();
                if (string.Compare(ext, ".pdf", true) == 0) //When loading PDF - extract text.
                {
                    string documentText;

                    using (var documentProcessor = new PdfDocumentProcessor())
                    {
                        ExecuteLocked(() => documentProcessor.LoadDocument(fileName), LockFiles ? LockObject : null);
                        documentText = documentProcessor.Text;
                    }

                    book.Document.AppendText(documentText);
                }
                else 
                {
                    ExecuteLocked(() => book.Document.LoadDocument(fileName), LockFiles ? LockObject : null);
                }
            }

            WriteObject(book);
        }
    }
}
