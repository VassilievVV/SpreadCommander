using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
    public interface IImportExportSpreadSheetsService
    {
        void ImportSheets(IWorkbook workbook);
        void ExportSheets(IWorkbook workbook);
    }
}
