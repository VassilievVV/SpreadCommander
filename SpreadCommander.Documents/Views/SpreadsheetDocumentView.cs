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
using DevExpress.Utils.MVVM.UI;
using System.Threading;
using System.IO;
using DevExpress.Spreadsheet;
using System.Globalization;
using DevExpress.Spreadsheet.Charts;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Extensions;
using SpreadCommander.Common.Spreadsheet;
using DevExpress.Utils.Animation;
using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;
using SpreadCommander.Documents.Messages;

namespace SpreadCommander.Documents.Views
{
    public partial class SpreadsheetDocumentView : DevExpress.XtraBars.Ribbon.RibbonForm, SpreadsheetDocumentViewModel.ICallback, IImageHolder
    {
        public SpreadsheetDocumentView()
        {
            using var _ = new DocumentAddingProcessor(this);

            InitializeComponent();

            UIUtils.ConfigureRibbonBar(Ribbon);
            Spreadsheet.InitializeSpreadsheet();
            ribbonControl.SelectedPage = homeRibbonPage1;

            //Restore when DevExpress will release Snap for .Net Core
            barBookTemplateEditor.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        public SvgImage GetControlImage() =>
            svgFormIcon.Count > 0 ? svgFormIcon[0] : null;

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<SpreadsheetDocumentViewModel>();
            fluent.ViewModel.InitializeBindings();
            fluent.ViewModel.Callback = this;

            fluent.BindCommand(barExportToDatabase, m => m.ExportToDatabase());
            fluent.BindCommand(barSpreadsheetTemplateEditor, m => m.ShowSpreadsheetTemplateEditor());
            fluent.BindCommand(barBookTemplateEditor, m => m.ShowBookTemplateEditor());
            fluent.BindCommand(barSelectTable, m => m.SelectTable());
            fluent.BindCommand(barSelectTableData, m => m.SelectTableData());
            fluent.BindCommand(barExpandSelectionRows, m => m.ExpandSelectionToRows());
            fluent.BindCommand(barExpandSelectionColumns, m => m.ExpandSelectionToColumns());
            fluent.BindCommand(barCopySelectionToRows, m => m.CopySelectionToRows());
            fluent.BindCommand(barCopySelectionToColumns, m => m.CopySelectionToColumns());
            fluent.BindCommand(barExportSheets, m => m.ExportSheets());
            fluent.BindCommand(barImportSheets, m => m.ImportSheets());
            fluent.BindCommand(barClone, m => m.CloneSpreadsheet());
        }

        private void SpreadsheetDocumentView_Load(object sender, EventArgs e)
        {
            ActiveControl = Spreadsheet;
            Spreadsheet.Focus();
        }

        private void MvvmContext_ViewModelCreate(object sender, DevExpress.Utils.MVVM.ViewModelCreateEventArgs e)
        {
            if (!mvvmContext.IsDesignMode)
                InitializeBindings();
        }

        private void MvvmContext_ViewModelSet(object sender, DevExpress.Utils.MVVM.ViewModelSetEventArgs e)
        {
            if (!mvvmContext.IsDesignMode)
                InitializeBindings();
        }

        public void LoadFromStream(Stream stream)
        {
            Spreadsheet.LoadDocument(stream);
            SpreadsheetUtils.InitializeWorkbook(Spreadsheet.Document);
        }

        public void SaveToStream(Stream stream)
        {
            var modified = Spreadsheet.Modified;
            try
            {
                Spreadsheet.SaveDocument(stream, DocumentFormat.Xlsx);
            }
            finally
            {
                //Restore Spreadsheet.Modified, do not treat this SaveToStream() as saving file.
                Spreadsheet.Modified = modified;
            }
        }

        public IWorkbook Workbook => Spreadsheet.Document;

        public object InvokeFunction(Func<object> method)
        {
            object result = null;
            Invoke((MethodInvoker)(() => result = method()));
            return result;
        }

        public void BeginWait()
        {
            transitionManager.StartWaitingIndicator(this, WaitingAnimatorType.Default);
        }

        public void EndWait()
        {
            transitionManager.StopWaitingIndicator();
        }

        public void SpreadsheetModified()
        {
            Spreadsheet.Modified = true;
        }

        public void CloseEditor()
        {
            if (Spreadsheet.IsCellEditorActive)
                Spreadsheet.CloseCellEditor(DevExpress.XtraSpreadsheet.CellEditorEnterValueMode.Default);
        }

        private void Spreadsheet_DocumentLoaded(object sender, EventArgs e)
        {
            SpreadsheetUtils.InitializeWorkbook(Spreadsheet.Document);

            var fluent = mvvmContext.OfType<SpreadsheetDocumentViewModel>();
            fluent.ViewModel.FileName = Spreadsheet.Document.Options.Save.CurrentFileName;
            fluent.ViewModel.Modified = false;
        }

        private void Spreadsheet_DocumentSaved(object sender, EventArgs e)
        {
            var fluent = mvvmContext.OfType<SpreadsheetDocumentViewModel>();
            fluent.ViewModel.FileName = Spreadsheet.Document.Options.Save.CurrentFileName;
            fluent.ViewModel.Modified = false;
        }

        private void Spreadsheet_ModifiedChanged(object sender, EventArgs e)
        {
            var modified = Spreadsheet.Modified;
            
            var fluent = mvvmContext.OfType<SpreadsheetDocumentViewModel>();
            fluent.ViewModel.Modified = modified;

            if (modified)
                Messenger.Default.Send(new ControlModifiedMessage() { Control = this, Modified = true });
        }
    }
}
