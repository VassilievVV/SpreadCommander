using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Common.Script.Book
{
    public class SaveOptions: BookOptions
    {
        [Description("If set and file already exists - it will be overwritten")]
        public bool Replace { get; set; }

        [Description("If set - fields in document will be recalculated. This may take some time.")]
        public bool UpdateFields { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }
    }

    public partial class SCBook
    {
        public SCBook Save(string fileName, SaveOptions options = null)
        {
            ExecuteSynchronized(options, () => DoSave(fileName, options));
            return this;
        }

        protected virtual void DoSave(string fileName, SaveOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("FileName cannot be empty.");

            fileName = Project.Current.MapPath(fileName);

            if (File.Exists(fileName))
            {
                if (options?.Replace ?? false)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{fileName}' already exists.");
            }

            if (options?.UpdateFields ?? false)
                book.UpdateAllFields();

            SaveBookToFile(CommonBook.BookServer, fileName, options);
        }

        protected virtual void SaveBookToFile(IRichEditDocumentServer bookServer, string fileName, SaveOptions options)
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

                using var ps   = new PrintingSystem();
                using var link = new PrintableComponentLink(ps)
                {
                    Component = bookServer as IPrintable
                };
                link.CreateDocument();

                ExecuteLocked(() => ps.ExportToPdf(fileName, pdfOptions), (options?.LockFiles ?? false) ? LockObject : null);
            }
            else
            {
                ExecuteLocked(() => bookServer.Document.SaveDocument(fileName, format), (options?.LockFiles ?? false) ? LockObject : null);
            }
        }
    }
}
