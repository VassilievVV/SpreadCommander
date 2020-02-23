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
using SpreadCommander.Documents.ViewModels;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Utils;
using System.Globalization;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Controls;
using DevExpress.XtraGrid.Views.Grid;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using DevExpress.XtraGrid.Menu;
using SpreadCommander.Common.Spreadsheet;
using System.Data.Common;
using SpreadCommander.Documents.Services;
using SpreadCommander.Documents.Console;
using DevExpress.XtraBars.Ribbon;
using DevExpress.Spreadsheet;
using SpreadCommander.Documents.Dialogs;
using DevExpress.Mvvm;

namespace SpreadCommander.Documents.Controls
{
    public partial class TableViewControl : XtraUserControl, IExportSource, IRibbonHolder
    {
        public TableViewControl()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(ribbonControl);
            gridViewer.SetFullMode(true);

            //Restore when DevExpress will release Snap for .Net Core
            barBookTemplateEditor.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        public object DataSource
        {
            get => gridViewer?.DataSource;
            set => gridViewer.AttachDataSource(value);
        }

        public void RefreshDataSource()
        {
            gridViewer?.RefreshDataSource();
        }

        RibbonControl IRibbonHolder.Ribbon => this.ribbonControl;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => this.ribbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => ribbonControl.Visible;
            set
            {
                ribbonControl.Visible = value;
                ribbonStatusBar.Visible = value;
            }
        }

        public ISpreadsheetHolder TargetSpreadsheet { get; set; }

        public GridData SaveGridData() =>
            gridViewer.SaveGridData();

        public void ExportGridToSpreadsheet(Stream stream) =>
            gridViewer.ExportGridToSpreadsheet(stream);

        public void LoadFromFile(string fileName)
        {
            gridViewer.LoadFile(fileName);
        }

        public void SaveToFile(string fileName)
        {
            gridViewer.SaveFile(fileName);
        }

        public IWorkbook GetTargetWorkbook() =>
            TargetSpreadsheet?.Workbook ?? BaseDocumentViewModel.StaticAddNewSpreadsheetModel().Workbook;

        public void ExportTable()
        {
            using var stream = new MemoryStream();
            ExportGridToSpreadsheet(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var workbook = GetTargetWorkbook();
            workbook.LoadDocument(stream);
        }

        protected void DoQuickExportTable(bool applyFormatting)
        {
            var dataSource = DataSource;

            if (dataSource == null)
            {
                XtraMessageBox.Show(this, "Data are not loaded", "No data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var gridData = SaveGridData();

            var workbook = GetTargetWorkbook();
            var table    = SpreadsheetUtils.AppendDataSource(workbook, dataSource);
            if (applyFormatting && gridData != null)
                SpreadsheetUtils.ApplyGridFormatting(table, gridData);
            else
                SpreadsheetUtils.FinishTableFormatting(table);
        }

        public void QuickExportTable()
        {
            DoQuickExportTable(true);
        }

        public void DataExportTable()
        {
            DoQuickExportTable(false);
        }

        public string[] GetTableNames()
        {
            return new string[] { "Table" };
        }

        public DbDataReader GetDataTable(string tableName)
        {
            if (tableName != "Table")
                throw new Exception("Invalid table name");

            var dataSource = DataSource;
            if (dataSource == null)
                throw new Exception("DataSource is not available");

            var result = new TypedListDataReader(dataSource);
            return result;
        }

        public void ExportToDatabase()
        {
            var tableExporterService = ServiceContainer.Default.GetService<IExportTablesService>();
            tableExporterService.ExportDataTables(this);
        }

        private void GridDocumentView_Load(object sender, EventArgs e)
        {
            gridViewer.SetFullMode(true);
            propertyGridProperties.SelectedObject = gridViewer.GridProperties;
        }

        private void BarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewer.Print();
        }

        private void BarPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewer.PrintPreview();
        }

        private void BarCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewer.CopyData(true, true);
        }

        private void BarFormatConditions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewer.ShowFormatRulesManager();
        }

        private void BarExportToSpreadsheet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportTable();
        }

        private void BarQuickExportToSpreadsheet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QuickExportTable();
        }

        private void BarDataExportToSpreadsheet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataExportTable();
        }

        private void BarExportToDatabase_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportToDatabase();
        }

        private void BarComputedColumns_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewer.ShowComputedColumnsEditor();
        }

        private void BarBookTemplateEditor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*
            var selectedTable = DataSource;
            if (selectedTable == null)
                return;

#pragma warning disable IDE0067 // Dispose objects before losing scope
            var frm = new BookTemplateEditor();
#pragma warning restore IDE0067 // Dispose objects before losing scope
            frm.FormClosed += (s, arg) => { ((Form)s).Dispose(); };
            frm.SetupMergeMail(selectedTable);
            frm.Show(this);
            */
        }

        private void BarSpreadsheetTemplateEditor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedTable = DataSource;
            if (selectedTable == null)
                return;

#pragma warning disable IDE0067 // Dispose objects before losing scope
            var frm = new SpreadsheetTemplateEditor()
            {
                MasterDetailEnabled = true
            };
#pragma warning restore IDE0067 // Dispose objects before losing scope
            frm.FormClosed += (s, arg) => { ((Form)s).Dispose(); };
            frm.SetupMergeMail(selectedTable, null);
            frm.Show(this);
        }
    }
}
