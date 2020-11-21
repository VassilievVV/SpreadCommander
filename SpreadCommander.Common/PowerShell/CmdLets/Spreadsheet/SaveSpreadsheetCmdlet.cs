using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet
{
    [Cmdlet(VerbsData.Save, "Spreadsheet")]
    public class SaveSpreadsheetCmdlet: BaseSpreadsheetCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "File name to save a Book")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "If set and file already exists - it will be overwritten")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public SwitchParameter LockFiles { get; set; }


        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                throw new Exception("FileName cannot be empty.");

            var fileName = Project.Current.MapPath(FileName);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception($"FileName cannot be empty.");
            if (File.Exists(fileName))
            {
                if (Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{FileName}' already exists.");
            }

            var workbook = GetCmdletSpreadsheet();
            ExecuteSynchronized(() => SaveSpreadsheetToFile(workbook, fileName));
        }

        protected virtual void SaveSpreadsheetToFile(IWorkbook workbook, string fileName)
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
                    ConvertImagesToJpeg = false,
                    ImageQuality = PdfJpegImageQuality.Highest,
                    PdfACompatibility = PdfACompatibility.None,
                    ShowPrintDialogOnOpen = false
                };

                using var ps = new PrintingSystem();
                using PrintableComponentLink link = new PrintableComponentLink(ps)
                {
                    Component = workbook
                };
                link.CreateDocument();

                ExecuteLocked(() => ps.ExportToPdf(fileName, pdfOptions), LockFiles ? LockObject : null);
            }
            else
            {
                ExecuteLocked(() => workbook.SaveDocument(fileName, format), LockFiles ? LockObject : null);
            }
        }
    }
}
