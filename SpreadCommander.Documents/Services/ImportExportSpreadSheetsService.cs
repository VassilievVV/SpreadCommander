using DevExpress.Spreadsheet;
using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
    public class ImportExportSpreadSheetsService: IImportExportSpreadSheetsService
    {
        private readonly IWin32Window _Owner;

        public ImportExportSpreadSheetsService(IWin32Window owner)
        {
            _Owner = owner;
        }

        public void ImportSheets(IWorkbook workbook) => DoImportExport(ImportExportSpreadsheetForm.ImportExportSpreadsheetMode.Import, workbook);
        public void ExportSheets(IWorkbook workbook) => DoImportExport(ImportExportSpreadsheetForm.ImportExportSpreadsheetMode.Export, workbook);

        protected void DoImportExport(ImportExportSpreadsheetForm.ImportExportSpreadsheetMode mode, IWorkbook workbook)
        {
            using var frm = new ImportExportSpreadsheetForm(mode, workbook);
            frm.ShowDialog(_Owner);
        }
    }
}
