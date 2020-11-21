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
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Services;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.Utils.Commands;
using DevExpress.XtraRichEdit.API.Native;
using System.IO;
using DevExpress.Office.Services;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using System.Reflection;
using DevExpress.Utils.Animation;
using SpreadCommander.Common.Book;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;
using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;
using SpreadCommander.Common;
using SpreadCommander.Documents.Messages;

namespace SpreadCommander.Documents.Views
{
    public partial class BookDocumentView : DevExpress.XtraBars.Ribbon.RibbonForm, BookDocumentViewModel.ICallback, IImageHolder
    {
        private SCBook SCBook;

        public BookDocumentView()
        {
            using var _ = new DocumentAddingProcessor(this);

            InitializeComponent();

            UIUtils.ConfigureRibbonBar(Ribbon);

            //Fix skin colors in comment control
            var commentEdit = richEditCommentControl1.Controls.OfType<InnerCommentControl>().FirstOrDefault();
            if (commentEdit != null)
            {
                commentEdit.Options.DocumentCapabilities.Macros     = DocumentCapability.Disabled;
                commentEdit.Options.DocumentCapabilities.OleObjects = DocumentCapability.Disabled;

                commentEdit.Views.DraftView.AdjustColorsToSkins       = commentEdit.RichEditControl.Views.DraftView.AdjustColorsToSkins;
                commentEdit.Views.SimpleView.AdjustColorsToSkins      = commentEdit.RichEditControl.Views.SimpleView.AdjustColorsToSkins;
                commentEdit.Views.PrintLayoutView.AdjustColorsToSkins = commentEdit.RichEditControl.Views.PrintLayoutView.AdjustColorsToSkins;
            }

            ribbonControl.SelectedPage = homeRibbonPage1;

            SCBook = new SCBook(Editor);
            BookFactoryHelper.SetCommandFactory(Editor, this);

            Disposed += BookDocumentView_Disposed;
        }

        private void BookDocumentView_Disposed(object sender, EventArgs e)
        {
            SCBook?.Dispose();
            SCBook = null;
        }

        private void BookDocumentView_Load(object sender, EventArgs e)
        {
            ActiveControl = Editor;
            Editor.Focus();
        }

        public SvgImage GetControlImage() =>
            svgFormIcon.Count > 0 ? svgFormIcon[0] : null;

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<BookDocumentViewModel>();
            fluent.ViewModel.InitializeBindings();
            fluent.ViewModel.Callback = this;

            fluent.BindCommand(barUpdateMailMergeFields, m => m.UpdateMailMergeFields());
            fluent.BindCommand(barClone, m => m.CloneBook());
        }

        public void LoadFromStream(Stream stream, DocumentFormat documentFormat)
        {
            Document.LoadDocument(stream, documentFormat);
        }

        public void SaveToStream(Stream stream, DocumentFormat documentFormat)
        {
            var modified = Editor.Modified;
            try
            {
                Document.SaveDocument(stream, documentFormat);
            }
            finally
            {
                //Restore Spreadsheet.Modified, do not treat this SaveToStream() as saving file.
                Editor.Modified = modified;
            }
        }

        public object InvokeFunction(Func<object> method)
        {
            object result = null;
            Invoke((MethodInvoker)(() => result = method()));
            return result;
        }

        public void DocumentModified() => DocumentModified(true);

        public void DocumentModified(bool value)
        {
            Editor.Modified = value;
        }
        
        public bool Modified
        {
            get => Editor.Modified;
            set => Editor.Modified = value;
        }

        public void UpdateMergeSource(MailMergeOptions mergeOptions)
        {
            Editor.Options.MailMerge.DataSource = mergeOptions.DataSource;
            Editor.Options.MailMerge.DataMember = mergeOptions.DataMember;
        }

        public void BeginWait()
        {
            transitionManager.StartWaitingIndicator(this, WaitingAnimatorType.Default);
        }

        public void EndWait()
        {
            transitionManager.StopWaitingIndicator();
        }

        public Document Document            => Editor.Document;
        public DocumentFormat CurrentFormat => Editor.Options.DocumentSaveOptions.CurrentFormat;

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

        private void Editor_DocumentLoaded(object sender, EventArgs e)
        {
            var fluent = mvvmContext.OfType<BookDocumentViewModel>();
            fluent.ViewModel.FileName = Editor.Options.DocumentSaveOptions.CurrentFileName;
            fluent.ViewModel.Modified = false;
        }

        internal void DocumentSaved()
        {
            var fluent = mvvmContext.OfType<BookDocumentViewModel>();
            fluent.ViewModel.FileName = Editor.Options.DocumentSaveOptions.CurrentFileName;
            fluent.ViewModel.Modified = false;
        }

        private void Editor_ModifiedChanged(object sender, EventArgs e)
        {
            var modified = Editor.Modified;
            
            var fluent = mvvmContext.OfType<BookDocumentViewModel>();
            fluent.ViewModel.Modified = modified;

            if (modified)
                Messenger.Default.Send(new ControlModifiedMessage() { Control = this, Modified = true });
        }

        private bool _ZoomChanging;
        private void BarZoom_EditValueChanged(object sender, EventArgs e)
        {
            if (_ZoomChanging)
                return;

            using (new UsingProcessor(() => _ZoomChanging = true, () => _ZoomChanging = false))
            {
                int value                    = Convert.ToInt32(barZoom.EditValue);
                Editor.ActiveView.ZoomFactor = value / 100f;
                barZoom.Caption              = $"{value}%";
            }
        }

        private void Editor_ZoomChanged(object sender, EventArgs e)
        {
            if (_ZoomChanging)
                return;

            using (new UsingProcessor(() => _ZoomChanging = true, () => _ZoomChanging = false))
            {
                int value         = Convert.ToInt32(Editor.ActiveView.ZoomFactor * 100);
                barZoom.EditValue = value;
                barZoom.Caption   = $"{value}%";
            }
        }

        private void Editor_ActiveViewChanged(object sender, EventArgs e)
        {
            if (_ZoomChanging)
                return;

            using (new UsingProcessor(() => _ZoomChanging = true, () => _ZoomChanging = false))
            {
                int value         = Convert.ToInt32(Editor.ActiveView.ZoomFactor * 100);
                barZoom.EditValue = value;
                barZoom.Caption   = $"{value}%";
            }

            switch (Editor.ActiveViewType)
            {
                case RichEditViewType.Simple:
                    barViewSimple.Down = true;
                    break;
                case RichEditViewType.Draft:
                    barViewDraft.Down = true;
                    break;
                case RichEditViewType.PrintLayout:
                    barViewPrintLayout.Down = true;
                    break;
            }
        }

        private void BarViewDraft_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Editor.ActiveViewType = RichEditViewType.Draft;
        }

        private void BarViewPrintLayout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Editor.ActiveViewType = RichEditViewType.PrintLayout;
        }

        private void BarViewSimple_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Editor.ActiveViewType = RichEditViewType.Simple;
        }

        private void Editor_UpdateUI(object sender, EventArgs e)
        {
            if (Editor.ActiveView is PageBasedRichEditView printView)
                barPages.Caption = $"Page {printView.CurrentPageIndex + 1} of {printView.PageCount}";
            else
                barPages.Caption = string.Empty;
        }

        private void BarLoadTemplate_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dir = Project.Current.MapPath("~\\Templates");
            if (Directory.Exists(dir))
                dlgOpen.InitialDirectory = dir;

            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return;

            Editor.LoadDocumentTemplate(dlgOpen.FileName);
        }

        private void BarSaveTemplate_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dir = Project.Current.MapPath("~\\Templates");
            if (Directory.Exists(dir))
                dlgSave.InitialDirectory = dir;

            if (dlgSave.ShowDialog(this) != DialogResult.OK)
                return;

            var oldFileName = Editor.Options.DocumentSaveOptions.CurrentFileName;
            var oldFormat   = Editor.Options.DocumentSaveOptions.CurrentFormat;
            var oldModified = Editor.Modified;
            try
            {
                Editor.Document.SaveDocument(dlgSave.FileName, DocumentFormat.OpenXml);
            }
            finally
            {
                Editor.Options.DocumentSaveOptions.CurrentFileName = oldFileName;
                Editor.Options.DocumentSaveOptions.CurrentFormat   = oldFormat;
                Editor.Modified                                    = oldModified;
            }
        }

        private void BarNewStyle_ItemClick(object sender, ItemClickEventArgs e)
        {
            var styleNames   = Editor.Document.ParagraphStyles.Select(s => s.Name).ToList();
            var newStyleName = Utils.AddUniqueString(styleNames, "NewStyle1", StringComparison.CurrentCultureIgnoreCase, false);

            var newStyle  = Editor.Document.ParagraphStyles.CreateNew();
            newStyle.Name = newStyleName;
            Editor.Document.ParagraphStyles.Add(newStyle);

            var cmd = Editor.CreateCommand(RichEditCommandId.ShowEditStyleForm);
            cmd.Execute();
        }

        private void PopupBookStyles_BeforePopup(object sender, CancelEventArgs e)
        {
            popupBookStyles.ClearLinks();

            var styleNames = Editor.Document.ParagraphStyles.Select(s => s.Name).ToList();
            styleNames.Sort(StringLogicalComparer.DefaultComparer);

            foreach (var styleName in styleNames)
            {
                var barItem = new BarButtonItem(popupBookStyles.Manager, styleName);
                barItem.ImageOptions.SvgImage = svgFormIcon["style"];
                barItem.ItemClick += (s, args) =>
                {
                    var name = args.Item.Caption;
                    if (XtraMessageBox.Show(this, $"Do you want to delete style '{name}'?", "Confirm delete", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;

                    var style = Editor.Document.ParagraphStyles[name];
                    if (style != null)
                        Editor.Document.ParagraphStyles.Delete(style);
                };
                popupBookStyles.AddItem(barItem);
            }
        }

        private void BarEditBookStyles_ItemClick(object sender, ItemClickEventArgs e)
        {
            var cmd = Editor.CreateCommand(RichEditCommandId.ShowEditStyleForm);
            cmd.Execute();
        }
    }

    #region Book Factory
    public static class BookFactoryHelper
    {
        public static void SetCommandFactory(RichEditControl control, BookDocumentView documentView)
        {
            var myCommandFactory = new BookCommandFactoryService(control, documentView, control.GetService<IRichEditCommandFactoryService>());
            control.ReplaceService<IRichEditCommandFactoryService>(myCommandFactory);
        }
    }

    public class BookCommandFactoryService : IRichEditCommandFactoryService
    {
        private readonly IRichEditCommandFactoryService service;
        private readonly RichEditControl control;
        private readonly BookDocumentView documentView;

        public BookCommandFactoryService(RichEditControl control, BookDocumentView documentView,
            IRichEditCommandFactoryService service)
        {
            this.control      = control;
            this.service      = service;
            this.documentView = documentView;
        }
        public RichEditCommand CreateCommand(RichEditCommandId id)
        {
            if (id == RichEditCommandId.FileOpen)
                return new BookOpenDocumentCommand(control);
            else if (id == RichEditCommandId.FileSave)
                return new BookSaveDocumentCommand(control, documentView);
            else if (id == RichEditCommandId.FileSaveAs)
                return new BookSaveAsDocumentCommand(control, documentView);

            return service.CreateCommand(id);
        }
    }

    public class BookOpenDocumentCommand : LoadDocumentCommand
    {
        public BookOpenDocumentCommand(IRichEditControl control)
            : base(control)
        {
        }

        protected override void ExecuteCore()
        {
            base.ExecuteCore();
        }
    }

    public class BookSaveDocumentCommand : SaveDocumentCommand
    {
        private readonly BookDocumentView _DocumentView;

        public BookSaveDocumentCommand(IRichEditControl control, BookDocumentView documentView)
            : base(control)
        {
            _DocumentView = documentView;
        }

        protected override void UpdateUIStateCore(ICommandUIState state)
        {
            base.UpdateUIStateCore(state);
        }

        protected override void ExecuteCore()
        {
            base.ExecuteCore();
            _DocumentView.DocumentSaved();
        }
    }

    public class BookSaveAsDocumentCommand : SaveDocumentAsCommand
    {
        private readonly BookDocumentView _DocumentView;

        public BookSaveAsDocumentCommand(IRichEditControl control, BookDocumentView documentView)
            : base(control)
        {
            _DocumentView = documentView;
        }

        protected override void UpdateUIStateCore(ICommandUIState state)
        {
            base.UpdateUIStateCore(state);
        }

        protected override void ExecuteCore()
        {
            base.ExecuteCore();
            _DocumentView.DocumentSaved();
        }
    }
    #endregion
}
