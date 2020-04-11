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
using SpreadCommander.Documents.Controls;
using SpreadCommander.Common;
using System.IO;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using System.Data.Common;
using SpreadCommander.Common.SqlScript;
using SpreadCommander.Documents.Viewers;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using SpreadCommander.Documents.Console;
using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;

namespace SpreadCommander.Documents.Views
{
    public partial class SqlScriptDocumentView : DevExpress.XtraBars.Ribbon.RibbonForm,
        SqlScriptDocumentViewModel.ICallback, IImageHolder
    {
#pragma warning disable IDE0069 // Disposable fields should be disposed
        private ScriptEditorControl _SqlEditor;
        private ConsoleInputControl _ConsoleInputControl;
        private ConsoleOutputControl _ConsoleOutputControl;
#pragma warning restore IDE0069 // Disposable fields should be disposed

        public SqlScriptDocumentView()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);
        }

        public SvgImage GetControlImage() =>
            svgFormIcon.Count > 0 ? svgFormIcon[0] : null;

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<SqlScriptDocumentViewModel>();
            fluent.ViewModel.InitializeBindings();
            fluent.ViewModel.Callback = this;

            mvvmContext.RegisterService(new SpreadCommander.Documents.Services.DbSchemaViewerService(this));
            mvvmContext.RegisterService(new SpreadCommander.Documents.Services.SqlExecutionPlanViewer(this));
            mvvmContext.RegisterService(new SpreadCommander.Documents.Services.SQLiteExecutionPlanViewer(this));
            mvvmContext.RegisterService(new SpreadCommander.Documents.Services.MySqlExecutionPlanViewer(this));

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            fluent.SetBinding(this, frm => frm.Connection, m => m.Connection);
            fluent.SetBinding(this, frm => frm.DataSet, m => m.DataSet);
            fluent.BindCommand(barChangeConnection, m => m.ChangeConnection());
            fluent.BindCommand(barSchemas, m => m.ShowSchemas());
            fluent.BindCommand(barExecute, m => m.Execute());
            fluent.BindCommand(barCancel, m => m.Cancel());
            fluent.BindCommand(barQueryInfo, m => m.ShowQueryInfo());

            fluent.BindCommand(barClearAll, m => m.ClearAllOutput());
            fluent.BindCommand(barClearBook, m => m.ClearBook());
            fluent.BindCommand(barClearSpreadsheet, m => m.ClearSpreadsheet());
            fluent.BindCommand(barClearGrid, m => m.ClearGrid());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void UpdateModelDocuments()
        {
            var model = mvvmContext.GetViewModel<SqlScriptDocumentViewModel>();

            if (model.BookServer == null)
                model.BookServer = _ConsoleOutputControl.Editor;
            if (model.Workbook == null)
                model.Workbook = _ConsoleOutputControl.Workbook;
            if (model.GridDataSet == null)
                model.GridDataSet = _ConsoleOutputControl.DataSet;
            if (model.FileViewer == null)
                model.FileViewer = _ConsoleOutputControl.FileViewer;
        }

        [Bindable(true)]
        public DbConnection Connection { get; set; }

        [Bindable(true)]
        public DataSet DataSet
        {
            get => _ConsoleOutputControl?.DataSet;
            set
            {
                if (_ConsoleOutputControl == null || _ConsoleOutputControl.DataSet == value)
                    return;

                _ConsoleOutputControl.DataSet = value;
            }
        }

        [Bindable(true)]
        public string CommandText => _SqlEditor.CommandText;
        public string ScriptText  => _SqlEditor.ScriptText;

        public void ClearSqlMessages()
        {
            _ConsoleInputControl.ClearSqlMessages();
        }

        public void AddSqlMessage(SqlMessage message)
        {
            _ConsoleInputControl?.AddSqlMessage(message);
        }

        public void LoadFromFile(string fileName)
        {
            _SqlEditor?.OpenDocument(fileName);
        }

        public void SaveToFile(string fileName)
        {
            _SqlEditor.SaveDocument(fileName);
        }

        public void ScriptModified()
        {
            _SqlEditor.IsModified = true;
        }

        public void DisableTransitions(bool value)
        {
        }

        public void ResetModifiedAll()         => _ConsoleOutputControl?.ResetModifiedAll();
        public void ResetModifiedBook()        => _ConsoleOutputControl?.ResetModifiedBook();
        public void ResetModifiedSpreadsheet() => _ConsoleOutputControl?.ResetModifiedSpreadsheet();
        public void ResetModifiedGrid()        => _ConsoleOutputControl?.ResetModifiedGrid();

        private void SqlScriptDocumentView_Load(object sender, EventArgs e)
        {
        }

        private void ConsoleOutputControl_ConsoleLoaded(object sender, EventArgs e)
        {
            UpdateModelDocuments();

            try
            {
                _ConsoleOutputControl.ConsoleDocumentLoaded();
                ResetModifiedAll();
            }
            catch (Exception ex)
            {
                Dialogs.ExceptionHandler.HandleException(ex);
            }
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

        private void ViewParts_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            switch (e.Document.ControlName)
            {
                case "Script":
                    if (_SqlEditor == null)
                    {
                        var model = mvvmContext.GetViewModel<SqlScriptDocumentViewModel>();

                        _SqlEditor = new ScriptEditorControl();
                        _SqlEditor.LoadSyntax("SqlScript");
                        _SqlEditor.DefaultExt       = ".sql";
                        _SqlEditor.FileFilter       = "SQL files (*.sql)|*.sql";
                        _SqlEditor.ModifiedChanged += (s, arg) =>
                        {
                            model.Modified = true;
                            Messenger.Default.Send(new ControlModifiedMessage() { Control = this, Modified = true });
                        };
                        _SqlEditor.ListIntellisenseItems += ScriptEditor_ListIntellisenseItems;

                        if (!string.IsNullOrWhiteSpace(model.FileName))
                        {
                            if (model.FileName != null && File.Exists(model.FileName))
                                _SqlEditor.LoadFromFile(model.FileName);
                        }
                    }
                    e.Control = _SqlEditor;
                    break;
                case "Results":
                    if (_ConsoleOutputControl == null)
                    {
                        var model = mvvmContext.GetViewModel<SqlScriptDocumentViewModel>();

                        _ConsoleOutputControl = new ConsoleOutputControl(model);
                        _ConsoleOutputControl.SetSqlMode();
                        _ConsoleOutputControl.RibbonUpdateRequest += DataGrid_RibbonUpdateRequest;
                        _ConsoleOutputControl.ConsoleLoaded       += ConsoleOutputControl_ConsoleLoaded; ;
                    }
                    e.Control = _ConsoleOutputControl;

                    ResetModifiedAll();
                    break;
                case "CommandLine":
                    if (_ConsoleInputControl == null)
                    {
                        _ConsoleInputControl = new ConsoleInputControl()
                        {
                            ParseErrorsVisible = false
                        };

                        _ConsoleInputControl.LoadSyntax("SqlScript");
                        _ConsoleInputControl.ExecuteCommand        += ExecuteCommandHandler;
                        _ConsoleInputControl.ListIntellisenseItems += ScriptEditor_ListIntellisenseItems;
                        _ConsoleInputControl.ShowParseError        += ScriptEditor_ShowParseError;
                    }
                    e.Control = _ConsoleInputControl;
                    break;
            }
        }

        private void DataGrid_RibbonUpdateRequest(object sender, RibbonUpdateRequestEventArgs e)
        {
            Ribbon.UnMergeRibbon();
            RibbonStatusBar.UnMergeStatusBar();

            if (e.RibbonHolder != null && !e.IsFloating)	//leave ribbon on document if it is floating
            {
                if (e.RibbonHolder.Ribbon != null)
                    Ribbon.MergeRibbon(e.RibbonHolder.Ribbon);
                if (e.RibbonHolder.RibbonStatusBar != null)
                    RibbonStatusBar.MergeStatusBar(e.RibbonHolder.RibbonStatusBar);
            }

            //Re-merge ribbons on main form
            var mainRibbonHolder = Parameters.MainForm as IRibbonHolder;
            if (mainRibbonHolder?.Ribbon?.MergedRibbon == Ribbon)
            {
                mainRibbonHolder.Ribbon.UnMergeRibbon();
                mainRibbonHolder.Ribbon.MergeRibbon(Ribbon);

                mainRibbonHolder.RibbonStatusBar.UnMergeStatusBar();
                mainRibbonHolder.RibbonStatusBar.MergeStatusBar(RibbonStatusBar);
            }
        }

        private void ScriptEditor_ListIntellisenseItems(object sender, ScriptEditorControl.ListIntellisenseItemsEventArgs e)
        {
            var fluent = mvvmContext.OfType<SqlScriptDocumentViewModel>();
            fluent.ViewModel.ListScriptIntellisenseItems(e.Text, e.Lines, e.CaretPosition, e.Intellisense);
        }

        private void ScriptEditor_ShowParseError(object sender, ConsoleInputControl.ShowParseErrorArgs e)
        {
            _SqlEditor?.ShowParseError(e.Error);
        }

        private void BarUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.Undo();
        }

        private void BarRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.Redo();
        }

        private void BarPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.Paste();
        }

        private void BarCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.Cut();
        }

        private void BarCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.Copy();
        }

        private void BarToggleBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.BookmarkToggle();
        }

        private void BarPrevBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.BookmarkPrev();
        }

        private void BarNextBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.BookmarkNext();
        }

        private void BarClearBookmarks_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.BookmarksClear();
        }

        private void BarFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.ShowFindDialog(false);
        }

        private void BarReplace_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.ShowFindDialog(true);
        }

        private void BarGotoLine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.ShowGotoLineDialog();
        }

        private void BarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.PrintScriptQuick();
        }

        private void BarPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.PrintScript();
        }

        private void BarNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.NewDocument();

            var fluent = mvvmContext.OfType<SqlScriptDocumentViewModel>();
            fluent.ViewModel.FileName = null;
            fluent.ViewModel.Modified = false;
        }

        private void BarOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.OpenDocument();

            var fluent = mvvmContext.OfType<SqlScriptDocumentViewModel>();
            fluent.ViewModel.FileName = _SqlEditor.FileName;
            fluent.ViewModel.Modified = false;
        }

        private void BarSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _SqlEditor?.SaveDocument();

            var fluent = mvvmContext.OfType<SqlScriptDocumentViewModel>();
            fluent.ViewModel.FileName = _SqlEditor.FileName;
            fluent.ViewModel.Modified = false;
        }

        private void ExecuteCommandHandler(object sender, ConsoleInputControl.CommandArgs args)
        {
            var fluent = mvvmContext.OfType<SqlScriptDocumentViewModel>();
            fluent.ViewModel.ExecuteCommandText(args.CommandText, true);
        }
    }
}
