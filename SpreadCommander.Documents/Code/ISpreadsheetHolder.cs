using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Code
{
    public interface ISpreadsheetHolder
    {
        IWorkbook Workbook { get; }
    }
}
