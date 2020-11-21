using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Spreadsheet;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Spreadsheet;

namespace SpreadCommander.Documents.Controls
{
    public partial class SpreadsheetTableSelectorControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region TableData
        public class TableData
        {
            public string SheetName { get; set; }
            public string TableName { get; set; }
            public string Range     { get; set; }
        }
        #endregion

        private readonly string _Filename;

        public SpreadsheetTableSelectorControl(string fileName)
        {
            InitializeComponent();

            _Filename = fileName;
        }

        private async void SpreadsheetTableSelectorControl_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_Filename))
                return;

            var listTables = new List<TableData>();

            try
            {
                await Task.Run(() =>
                {
                    using var workbook = SpreadsheetUtils.CreateWorkbook();
                    if (!workbook.LoadDocument(_Filename))
                        return;

                    foreach (Worksheet sheet in workbook.Sheets.Where(s => s is Worksheet))
                    {
                        foreach (var table in sheet.Tables)
                        {
                            var tableData = new TableData()
                            {
                                SheetName = sheet.Name,
                                TableName = table.Name,
                                Range = table.Range.GetReferenceA1()
                            };

                            listTables.Add(tableData);
                        }
                    }
                }).ConfigureAwait(true);
            }
            catch (Exception)
            {
                throw;
            }

            using (new UsingProcessor(() => gridTables.BeginUpdate(), () => gridTables.EndUpdate()))
            {
                foreach (var tblData in listTables)
                    bindingTables.Add(tblData);

                if (bindingTables.Count > 0)
                    bindingTables.Position = 0;
            }
        }

        public TableData SelectedTable => bindingTables.Current as TableData;
    }
}
