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
using SpreadCommander.Documents.Controls;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Common.ScriptEngines;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using SpreadCommander.Documents.Console;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using System.IO;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;
using SpreadCommander.Documents.Messages;

namespace SpreadCommander.Documents.Views
{
    public partial class ConsoleDocumentView : DevExpress.XtraBars.Ribbon.RibbonForm, ConsoleDocumentViewModel.ICallback, IImageHolder
    {
        private const int ParseScriptTimerID  = 1;
        private const int ParseScriptInterval = 1000;   //1 sec.

        private ScriptEditorControl _ScriptEditor;
        private ConsoleOutputControl _ConsoleOutputControl;
        private ConsoleInputControl _ConsoleInputControl;

        private ulong _LastParseScriptTicks = 0;

        public ConsoleDocumentView()
        {
            using var _ = new DocumentAddingProcessor(this);

            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);

            Disposed += ConsoleDocumentView_Disposed;
        }

        private void ConsoleDocumentView_Disposed(object sender, EventArgs e)
        {
            if (_ScriptEditor != null)
            {
                _ScriptEditor.ListIntellisenseItems -= ScriptEditor_ListIntellisenseItems;
                _ScriptEditor.ScriptChanged         -= ScriptEditor_ScriptChanged;

                _ScriptEditor.Dispose();
                _ScriptEditor = null;
            }

            if (_ConsoleInputControl != null)
            {
                _ConsoleInputControl.ExecuteCommand        -= ExecuteCommandHandler;
                _ConsoleInputControl.ListIntellisenseItems -= ScriptEditor_ListIntellisenseItems;
                _ConsoleInputControl.ShowParseError        -= ScriptEditor_ShowParseError;

                _ConsoleInputControl.Dispose();
                _ConsoleInputControl = null;
            }

            if (_ConsoleOutputControl != null)
            {
                _ConsoleOutputControl.RibbonUpdateRequest -= ConsoleOutputControl_RibbonUpdateRequest;
                _ConsoleOutputControl.ConsoleLoaded       -= ConsoleOutputControl_ConsoleLoaded;

                _ConsoleOutputControl.Dispose();
                _ConsoleOutputControl = null;
            }
        }

        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case WinAPI.WM_TIMER:
                    var ticks = WinAPI.GetTickCount64();
                    if (ticks - _LastParseScriptTicks > ParseScriptInterval)
                    {
                        _LastParseScriptTicks = 0;
                        WinAPI.KillTimer(this, ParseScriptTimerID);

                        ParseScrtiptErrors();
                    }
                    break;
            }

            base.WndProc(ref msg);
        }

        public SvgImage GetControlImage() =>
            svgFormIcon.Count > 0 ? svgFormIcon[0] : null;

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<ConsoleDocumentViewModel>();
            fluent.ViewModel.InitializeBindings();
            fluent.ViewModel.Callback = this;

            mvvmContext.RegisterService(new SpreadCommander.Documents.Services.SaveFilesService(this));

            //fluent.BindCommand(barExecute, m => m.Execute(CommandText));
            fluent.BindCommand(barCancel, m => m.Cancel());
            fluent.BindCommand(barRecreateEngine, m => m.RecreateEngine());

            fluent.BindCommand(barClearAll, m => m.ClearAllOutput());
            fluent.BindCommand(barClearBook, m => m.ClearBook());
            fluent.BindCommand(barClearSpreadsheet, m => m.ClearSpreadsheet());
            fluent.BindCommand(barClearGrid, m => m.ClearGrid());

            if (_ConsoleOutputControl != null)
                _ConsoleOutputControl.ViewModel = fluent.ViewModel;

            if (fluent.ViewModel.HasCustomControls)
            {
                barNew.Visibility  = DevExpress.XtraBars.BarItemVisibility.Never;
                barOpen.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        public void SetConsoleTitle(string title)
        {
            if (docConsole != null)
                docConsole.Caption = title;
        }

        public void NeedScrollToCaret()
        {
            _ConsoleOutputControl?.ScrollToCaret();
        }

        public void ProgressChanged(BaseScriptEngine.ProgressKind progressKind, int value, int max, string status)
        {
            _ConsoleOutputControl?.ProgressChanged(progressKind, value, max, status);
        }

        public string RequestLine()
        {
            string result = null;
            Invoke((MethodInvoker)(() =>
            {
                result = XtraInputBox.Show(this, "Response: ", "User response", string.Empty);
            }));

            return result;
        }

        private void ConsoleDocumentView_Load(object sender, EventArgs e)
        {
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

        public IList<ModifiedFileItem> ListFiles()
        {
            var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();

            var result = new List<ModifiedFileItem>
            {
                new ModifiedFileItem()
                {
                    FileName = _ScriptEditor.FileName,
                    Title    = Convert.ToString(model.Title),
                    Tag      = _ScriptEditor.EditorUniqueID
                }
            };

            return result;
        }

        public IList<ModifiedFileItem> ListModifiedFiles()
        {
            var result = new List<ModifiedFileItem>
            {
                new ModifiedFileItem() 
                { 
                    FileName = _ScriptEditor.FileName, 
                    //Title    = Convert.ToString(model.Title), 
                    Tag      = _ScriptEditor.EditorUniqueID 
                }
            };

            return result;
        }

        public void LoadFromFile(string fileName)
        {
            _ScriptEditor?.LoadFromFile(fileName);

            var fluent = mvvmContext.OfType<ConsoleDocumentViewModel>();
            fluent.ViewModel.FileLoaded(fileName);
            Messenger.Default.Send(new ControlModifiedMessage() { Control = this, Modified = false });
        }

        public void SaveToFile(string fileName)
        {
            _ScriptEditor.SaveToFile(fileName);

            var fluent = mvvmContext.OfType<ConsoleDocumentViewModel>();
            fluent.ViewModel.FileSaved(fileName);
            Messenger.Default.Send(new ControlModifiedMessage() { Control = this, Modified = false });
        }

        public void ResetModifiedAll()         => _ConsoleOutputControl?.ResetModifiedAll();
        public void ResetModifiedBook()        => _ConsoleOutputControl?.ResetModifiedBook();
        public void ResetModifiedSpreadsheet() => _ConsoleOutputControl?.ResetModifiedSpreadsheet();
        public void ResetModifiedGrid()        => _ConsoleOutputControl?.ResetModifiedGrid();

        private void ViewEditors_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            switch (e.Document.ControlName)
            {
                case "docScript":
                    if (_ScriptEditor == null)
                    {
                        _ScriptEditor = new ScriptEditorControl();

                        var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();
                        _ScriptEditor.LoadSyntax(model.Engine.SyntaxFile);
                        _ScriptEditor.DefaultExt = model.Engine.DefaultExt;
                        _ScriptEditor.FileFilter = model.Engine.FileFilter;
                        _ScriptEditor.ModifiedChanged       += ScriptEditor_ModifiedChanged;
                        _ScriptEditor.ListIntellisenseItems += ScriptEditor_ListIntellisenseItems;
                        _ScriptEditor.ScriptChanged         += ScriptEditor_ScriptChanged;

                        if (!string.IsNullOrWhiteSpace(model.FileName))
                        {
                            var scriptFileName = model.GetScriptFileName(model.FileName);
                            if (scriptFileName != null && File.Exists(scriptFileName))
                                _ScriptEditor.LoadFromFile(scriptFileName);
                        }
                    }
                    e.Control = _ScriptEditor;
                    break;
                case "docConsole":
                    if (_ConsoleOutputControl == null)
                    {
                        var model                       = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();
                        _ConsoleOutputControl           = new ConsoleOutputControl(model);
                        model.SynchronizeInvoke         = _ConsoleOutputControl;

                        _ConsoleOutputControl.RibbonUpdateRequest += ConsoleOutputControl_RibbonUpdateRequest;
                        _ConsoleOutputControl.ConsoleLoaded       += ConsoleOutputControl_ConsoleLoaded;
                    }
                    e.Control = _ConsoleOutputControl;
                    break;
                case "docCommandLine":
                    if (_ConsoleInputControl == null)
                    {
                        _ConsoleInputControl = new ConsoleInputControl()
                        {
                            MessagesVisible = false
                        };

                        var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();

                        _ConsoleInputControl.LoadSyntax(model.Engine?.SyntaxFile);
                        _ConsoleInputControl.ExecuteCommand        += ExecuteCommandHandler;
                        _ConsoleInputControl.ListIntellisenseItems += ScriptEditor_ListIntellisenseItems;
                        _ConsoleInputControl.ShowParseError        += ScriptEditor_ShowParseError;
                    }
                    e.Control = _ConsoleInputControl;
                    break;
            }
        }

        private void ScriptEditor_ModifiedChanged(object sender, EventArgs args)
        {
            var fluent = mvvmContext.OfType<ConsoleDocumentViewModel>();
            fluent.ViewModel.Modified = true;
            Messenger.Default.Send(new ControlModifiedMessage() { Control = this, Modified = true });
        }

        private void ScriptEditor_ListIntellisenseItems(object sender, ScriptEditorControl.ListIntellisenseItemsEventArgs e)
        {
            var fluent = mvvmContext.OfType<ConsoleDocumentViewModel>();
            fluent.ViewModel.ListScriptIntellisenseItems(e.Text, e.Lines, e.CaretPosition, e.Intellisense);
        }

        private void ScriptEditor_ShowParseError(object sender, ConsoleInputControl.ShowParseErrorArgs e)
        {
            _ScriptEditor?.ShowParseError(e.Error);
        }

        private void ScriptEditor_ScriptChanged(object sender, EventArgs e)
        {
            if (_LastParseScriptTicks == 0)
            {
                WinAPI.SetTimer(this, ParseScriptTimerID, ParseScriptInterval);
                _LastParseScriptTicks = WinAPI.GetTickCount64();
            }
        }

        private void ParseScrtiptErrors()
        {
            var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();

            var errors = new List<ScriptParseError>();
            model.ParseScriptErrors(_ScriptEditor?.ScriptText, errors);

            _ScriptEditor?.UpdateParseErrors(errors);
            _ConsoleInputControl?.UpdateParseErrors(errors);
        }

        private void UpdateModelDocuments()
        {
            var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();

            if (model.BookServer == null)
                model.BookServer = _ConsoleOutputControl.Editor;
            if (model.Workbook == null)
                model.Workbook = _ConsoleOutputControl.Workbook;
            if (model.GridDataSet == null)
                model.GridDataSet = _ConsoleOutputControl.DataSet;
            if (model.FileViewer == null)
                model.FileViewer = _ConsoleOutputControl.FileViewer;
        }

        private void ConsoleOutputControl_ConsoleLoaded(object sender, EventArgs e)
        {
            UpdateModelDocuments();

            try
            {
                _ConsoleOutputControl.ConsoleDocumentLoaded();
            }
            catch (Exception ex)
            {
                Dialogs.ExceptionHandler.HandleException(ex);
            }

            var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();
            model.Modified = false;

            //Start engine only when console is loaded - script engine may start output immediately
            model.StartEngine();
        }

        private void ConsoleOutputControl_RibbonUpdateRequest(object sender, RibbonUpdateRequestEventArgs e)
        {
            Ribbon.UnMergeRibbon();
            ribbonStatusBar.UnMergeStatusBar();

            if (e.RibbonHolder != null && !e.IsFloating)	//leave ribbon on document if it is floating
            {
                if (e.RibbonHolder.Ribbon != null)
                    Ribbon.MergeRibbon(e.RibbonHolder.Ribbon);
                if (e.RibbonHolder.RibbonStatusBar != null)
                    ribbonStatusBar.MergeStatusBar(e.RibbonHolder.RibbonStatusBar);
            }

            //Re-merge ribbons on main form
            var mainRibbonHolder = Parameters.MainForm as IRibbonHolder;
            if (mainRibbonHolder?.Ribbon?.MergedRibbon == Ribbon)
            {
                mainRibbonHolder.Ribbon.UnMergeRibbon();
                mainRibbonHolder.Ribbon.MergeRibbon(Ribbon);

                mainRibbonHolder.RibbonStatusBar.UnMergeStatusBar();
                mainRibbonHolder.RibbonStatusBar.MergeStatusBar(ribbonStatusBar);
            }
        }

        private void ViewEditors_ControlLoaded(object sender, DevExpress.XtraBars.Docking2010.Views.DeferredControlLoadEventArgs e)
        {
        }

        private void BarUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.Undo();
        }

        private void BarRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.Redo();
        }

        private void BarPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.Paste();
        }

        private void BarCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.Cut();
        }

        private void BarCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.Copy();
        }

        private void BarToggleBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.BookmarkToggle();
        }

        private void BarPrevBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.BookmarkPrev();
        }

        private void BarNextBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.BookmarkNext();
        }

        private void BarClearBookmarks_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.BookmarksClear();
        }

        private void BarFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.ShowFindDialog(false);
        }

        private void BarReplace_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.ShowFindDialog(true);
        }

        private void BarGotoLine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.ShowGotoLineDialog();
        }

        private void BarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.PrintScriptQuick();
        }

        private void BarPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.PrintScript();
        }

        private void BarNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.NewDocument();
        }

        private void BarOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ScriptEditor?.OpenDocument();
        }

        private void BarSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();
            model.Save();
        }

        public string CommandText => _ScriptEditor?.CommandText;

        private void ExecuteCommand(string command)
        {
            UpdateModelDocuments();

            _ConsoleInputControl.AddHistoryItem(command);

            var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();
            model.Execute(command);
        }

        private void BarExecute_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExecuteCommand(CommandText);
        }

        private void ExecuteCommandHandler(object sender, ConsoleInputControl.CommandArgs args)
        {
            ExecuteCommand(args.CommandText);
        }

        private void BarExecuteScript_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _ConsoleOutputControl.ResetModifiedAll();

            UpdateModelDocuments();

            var model = mvvmContext.GetViewModel<ConsoleDocumentViewModel>();
            model.ExecuteScript(_ScriptEditor.ScriptText);
        }
    }
}
