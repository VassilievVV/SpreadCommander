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
using DevExpress.XtraPrinting;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsData.Save, "Book")]
    public class SaveBookCmdlet: BaseBookCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "File name to save a Book")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "If set and file already exists - it will be overwritten")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "If set - fields in document will be recalculated. This may take some time.")]
        public SwitchParameter UpdateFields { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                throw new Exception("FileName cannot be empty.");

            var fileName = Project.Current.MapPath(FileName);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception($"FileName cannot be empty.");

            var bookServer = GetCmdletBookServer();

            if (UpdateFields)
                bookServer.Document.UpdateAllFields();

            if (File.Exists(fileName))
            {
                if (Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{FileName}' already exists.");
            }
            
            ExecuteSynchronized(() => SaveBookToFile(bookServer, fileName));
        }

        protected virtual void SaveBookToFile(IRichEditDocumentServer bookServer, string fileName)
        {
            var format = DocumentFormat.OpenXml;
            switch (Path.GetExtension(fileName)?.ToLower())
            {
                case ".docx":
                    format = DocumentFormat.OpenXml;
                    break;
                case ".doc":
                    format = DocumentFormat.Doc;
                    break;
                case ".txt":
                case ".text":
                    format = DocumentFormat.PlainText;
                    break;
                case ".rtf":
                    format = DocumentFormat.Rtf;
                    break;
                case ".html":
                case ".htm":
                    format = DocumentFormat.Html;
                    break;
                case ".mht":
                    format = DocumentFormat.Mht;
                    break;
                case ".odt":
                    format = DocumentFormat.OpenDocument;
                    break;
                case ".epub":
                    format = DocumentFormat.ePub;
                    break;
                case ".pdf":
                    format = DocumentFormat.Undefined;
                    break;
            }

            if (format == DocumentFormat.Undefined)
            {
                var pdfOptions = new PdfExportOptions()
                {
                    ConvertImagesToJpeg   = false,
                    ImageQuality          = PdfJpegImageQuality.Highest,
                    PdfACompatibility     = PdfACompatibility.None,
                    ShowPrintDialogOnOpen = false
                };

                using var ps = new PrintingSystem();
                using var link = new PrintableComponentLink(ps)
                {
                    Component = bookServer as IPrintable
                };
                link.CreateDocument();

                ExecuteLocked(() => ps.ExportToPdf(fileName, pdfOptions), LockFiles ? LockObject : null);
            }
            else
            {
                ExecuteLocked(() => bookServer.Document.SaveDocument(fileName, format), LockFiles ? LockObject : null);
            }
        }
    }
}
