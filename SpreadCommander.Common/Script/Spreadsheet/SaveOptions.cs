using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class SaveOptions: SpreadsheetOptions
    {
        [Description("If set and file already exists - it will be overwritten")]
        public bool Replace { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }
    }

    public partial class SCSpreadsheet
    {
        public void Save(string fileName, SaveOptions options = null) 
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("FileName cannot be empty.");

            fileName = Project.Current.MapPath(fileName);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception($"FileName cannot be empty.");
            if (File.Exists(fileName))
            {
                if (options?.Replace ?? false)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{fileName}' already exists.");
            }

            var spread = options?.Spreadsheet?.Workbook ?? Workbook;

            ExecuteSynchronized(() => SaveSpreadsheetToFile(spread, fileName, options));
        }

        protected virtual void SaveSpreadsheetToFile(IWorkbook workbook, string fileName, SaveOptions options)
        {
            var format = DocumentFormat.Xlsx;
            switch (Path.GetExtension(fileName)?.ToLower())
            {
                case ".xlsx":
                    format = DocumentFormat.Xlsx;
                    break;
                case ".xls":
                    format = DocumentFormat.Xls;
                    break;
                case ".csv":
                    format = DocumentFormat.Csv;
                    break;
                case ".txt":
                case ".text":
                    format = DocumentFormat.Text;
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
                    Component = workbook
                };
                link.CreateDocument();

                ExecuteLocked(() => ps.ExportToPdf(fileName, pdfOptions), options?.LockFiles ?? false ? LockObject : null);
            }
            else
            {
                ExecuteLocked(() => workbook.SaveDocument(fileName, format), options?.LockFiles ?? false ? LockObject : null);
            }
        }
    }
}
