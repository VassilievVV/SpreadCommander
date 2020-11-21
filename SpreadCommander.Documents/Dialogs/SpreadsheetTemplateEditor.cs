using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Common.Spreadsheet;
using DevExpress.Spreadsheet;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Extensions;
using DevExpress.XtraSpreadsheet.Commands;
using DevExpress.XtraSpreadsheet.UI;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class SpreadsheetTemplateEditor : BaseRibbonForm
    {
        private object _DataSource;
        private string _DataMember;

        public bool MasterDetailEnabled { get; set; }

        public SpreadsheetTemplateEditor()
        {
            InitializeComponent();

            UIUtils.ConfigureRibbonBar(Ribbon);
            Spreadsheet.InitializeSpreadsheet();

            ribbon.SelectedPage = mailMergeRibbonPage1;
            Spreadsheet.CreateCommand(SpreadsheetCommandId.MailMergeShowRanges).Execute();

            ActiveControl = Spreadsheet;
            Spreadsheet.Focus();
        }

        public IWorkbook Document => Spreadsheet.Document;

        public void SetupMergeMail(object dataSource, string dataMember)
        {
            _DataSource = dataSource;
            _DataMember = dataMember;

            InitializeMergeMail();
        }

        private void InitializeMergeMail()
        {
            Document.MailMergeDataSource = _DataSource;
            Document.MailMergeDataMember = _DataMember;

            Document.MailMergeOptions.UseTemplateSheetNames = true;

            var mergeMode = Document.DefinedNames.GetDefinedName("MAILMERGEMODE");
            if (mergeMode != null)
                mergeMode.RefersTo = "\"OneWorksheet\"";
        }

        private void Spreadsheet_UpdateUI(object sender, EventArgs e)
        {
            mailMergeDataRibbonPageGroup1.Visible = false;

            var mailMergeMasterDetailLinkItem = mailMergeExtendedRibbonPageGroup1.ItemLinks.
                Where(l => l.Item is SpreadsheetCommandBarSubItem spreadLink && spreadLink.CommandName == "EditingMailMergeMasterDetailCommandGroup").
                FirstOrDefault();
            if (mailMergeMasterDetailLinkItem != null)
                mailMergeMasterDetailLinkItem.Visible = MasterDetailEnabled;
        }

        private void BarSelectTable_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var table = sheet.Selection.GetRangeTable() ?? throw new Exception("Please select range inside a table.");
            sheet.Selection = table.Range;
        }

        private void BarSelectTableData_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var table = sheet.Selection.GetRangeTable() ?? throw new Exception("Please select range inside a table.");
            sheet.Selection = table.DataRange;
        }

        private void BarExpandSelectionRows_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection.ExpandToTableRows();
            sheet.Selection = selection;
        }

        private void BarExpandSelectionColumns_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection.ExpandToTableColumn();
            sheet.Selection = selection;
        }

        private void BarCopySelectionToRows_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection;
            selection.CopyToTableRows();
        }

        private void BarCopySelectionToColumns_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Spreadsheet.Document.Sheets.ActiveSheet is not Worksheet sheet)
                throw new Exception("Please select worksheet.");

            var selection = sheet.Selection;
            selection.CopyToTableColumns();
        }

        private void Spreadsheet_DocumentLoaded(object sender, EventArgs e)
        {
            InitializeMergeMail();
        }
    }
}