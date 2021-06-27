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
using System.Globalization;
using DevExpress.XtraBars.Ribbon;
using SpreadCommander.Documents.Services;
using SpreadCommander.Documents.Code;
using System.Data.Common;
using SpreadCommander.Common.SqlScript;
using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using DevExpress.XtraSpreadsheet;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Common.Spreadsheet;
using DevExpress.Utils.Animation;
using System.IO;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleSpreadsheetControl : ConsoleBaseControl, IRibbonHolder, IExportSource
    {
        public ConsoleSpreadsheetControl()
        {
            InitializeComponent();

            Spreadsheet.InitializeSpreadsheet();
            UIUtils.ConfigureRibbonBar(Ribbon);
            Ribbon.SelectedPage = homeRibbonPage1;

            //Restore when DevExpress will release Snap for .Net Core
            barBookTemplateEditor.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        public SpreadsheetControl SpreadsheetControl => Spreadsheet;
        public IWorkbook Document => SpreadsheetControl?.Document;

        RibbonControl IRibbonHolder.Ribbon            => Ribbon;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => ribbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => Ribbon.Visible;
            set
            {
                Ribbon.Visible          = value;
                ribbonStatusBar.Visible = value;
            }
        }

        public string[] GetTableNames()
        {
            var rootWorkbook = Spreadsheet.Document;
            if (rootWorkbook == null)
                return null;

            var result = SpreadsheetUtils.GetTableNames(rootWorkbook);
            return result.ToArray();
        }

        public DbDataReader GetDataTable(string tableName)
        {
            var rootWorkbook = Spreadsheet.Document;
            if (rootWorkbook == null || string.IsNullOrWhiteSpace(tableName))
                return null;

            var result = SpreadsheetUtils.GetTableDataReader(rootWorkbook, tableName);
            return result;
        }

        protected Table GetSelectedTable()
        {
            var selectedTable = Spreadsheet.Document.GetSelectedTable();
            if (selectedTable == null)
            {
                XtraMessageBox.Show(this, "Cannot find active table.", "No table", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return selectedTable;
        }

        private void BarExportToDatabase_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportTablesService.ExportDataTables(this, this);
        }

        private void Spreadsheet_ContentChanged(object sender, EventArgs e)
        {
            FireModified(true);
        }

        private void BarClone_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var workbook = Spreadsheet.Document ?? throw new NullReferenceException("Workbook is not loaded yet.");
            var modified = workbook.Modified;

            using (new UsingProcessor(
                () => transitionManager.StartWaitingIndicator(Spreadsheet, WaitingAnimatorType.Default), 
                () => transitionManager.StopWaitingIndicator()))
            {
                using var stream = new MemoryStream(65536);
                workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                stream.Seek(0, SeekOrigin.Begin);
                workbook.Modified = modified;

                var newSpreadsheetModel = BaseDocumentViewModel.StaticAddNewSpreadsheetModel();
                newSpreadsheetModel.LoadFromStream(stream);
                newSpreadsheetModel.Modified = true;
            }
        }

        private void BarImportSheets_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using var frm = new ImportExportSpreadsheetForm(ImportExportSpreadsheetForm.ImportExportSpreadsheetMode.Import, Spreadsheet.Document);
            frm.ShowDialog(this);
        }

        private void BarExportSheets_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using var frm = new ImportExportSpreadsheetForm(ImportExportSpreadsheetForm.ImportExportSpreadsheetMode.Export, Spreadsheet.Document);
            frm.ShowDialog(this);
        }

        private void BarSelectTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var table = sheet.Selection.GetRangeTable() ?? throw new Exception("Please select range inside a table.");
            sheet.Selection = table.Range;
        }

        private void BarSelectTableData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var table = sheet.Selection.GetRangeTable() ?? throw new Exception("Please select range inside a table.");
            sheet.Selection = table.DataRange;
        }

        private void BarSelectDataRange_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var range = sheet.GetDataRange();
            sheet.Selection = range;
        }

        private void BarExpandSelectionRows_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection.ExpandToTableRows();
            sheet.Selection = selection;
        }

        private void BarExpandSelectionColumns_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection.ExpandToTableColumn();
            sheet.Selection = selection;
        }

        private void BarCopySelectionToRows_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection;
            selection.CopyToTableRows();
        }

        private void BarCopySelectionToColumns_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection;
            selection.CopyToTableColumns();
        }

        private void BarBookTemplateEditor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Restore when DevExpress will release Snap for .Net Core
            /*
            var selectedTable = GetSelectedTable();
            if (selectedTable == null)
                return;

            var dataTable = selectedTable.ExportToDataTable();

#pragma warning disable IDE0067 // Dispose objects before losing scope
            var frm = new BookTemplateEditor();
#pragma warning restore IDE0067 // Dispose objects before losing scope
            frm.FormClosed += (s, arg) => { ((Form)s).Dispose(); };
            frm.SetupMergeMail(dataTable);
            frm.Show(this);
            */
        }

        private void BarSpreadsheetTemplateEditor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedTable = GetSelectedTable();
            if (selectedTable == null)
                return;

            var dataTable = selectedTable.ExportToDataTable();

            var frm = new SpreadsheetTemplateEditor();
            frm.FormClosed += (s, arg) => { ((Form)s).Dispose(); };
            frm.SetupMergeMail(dataTable, null);
            frm.Show(this);
        }
    }
}
