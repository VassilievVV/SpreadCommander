using DevExpress.Spreadsheet;
using SpreadCommander.Common.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public partial class SCSpreadsheet : ScriptHostObject, IDisposable
    {
        protected internal IWorkbook Workbook { get; private set; }

        public SCSpreadsheet() : base()
        {
            Workbook = SpreadsheetUtils.CreateWorkbook();
        }

        public SCSpreadsheet(ScriptHost host) : base(host)
        {
            Workbook = host.Engine.Workbook;
        }

        public void Dispose()
        {
            Workbook?.Dispose();
            Workbook = null;
        }

        protected static void AddComments(CellRange range, string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment))
                range.Worksheet.Comments.Add(range, Environment.UserName, comment);
        }

        protected override bool NeedSynchronization(SpreadsheetOptions options)
        {
            bool result = base.NeedSynchronization(options);
            return result;
        }
    }
}
