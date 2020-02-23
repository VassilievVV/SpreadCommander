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
using DevExpress.XtraRichEdit;
using DevExpress.Snap.Core.API;
using DevExpress.Services.Internal;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.Snap.Core.Commands;
using DevExpress.Snap;
using DevExpress.XtraReports.Design.Commands;
using System.ComponentModel.Design;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Book;
using DevExpress.XtraEditors;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class BookTemplateEditor : BaseRibbonForm
    {
#pragma warning disable IDE0069 // Disposable fields should be disposed
        private SCBook SCBook;
#pragma warning restore IDE0069 // Disposable fields should be disposed

        public BookTemplateEditor()
        {
            InitializeComponent();

            SCBook = new SCBook(Editor);
            Disposed += BookTemplateEditor_Disposed;

            UIUtils.ConfigureRibbonBar(Ribbon);

            Editor.AddService(typeof(ICommandUIStateManagerService), new BookTemplateICommandUIStateManagerService());

            var menuCommandHandler = new BookTemplateMenuCommandHandler(Editor);
            menuCommandHandler.RegisterMenuCommands();
            Editor.RemoveService(typeof(DevExpress.Snap.MenuCommandHandler));
            Editor.AddService(typeof(DevExpress.Snap.MenuCommandHandler), menuCommandHandler);
        }

        private void BookTemplateEditor_Disposed(object sender, EventArgs e)
        {
            SCBook?.Dispose();
            SCBook = null;
        }

        public SnapDocument Document => Editor.Document;

        public void SetupMergeMail(object dataSource, string dataMember = null)
        {
            //Document.DataSource = dataSource;
            var mergeOptions = Editor.Options.SnapMailMergeVisualOptions;
            mergeOptions.ResetMailMerge();
            Editor.DataSources.Clear();
            Editor.DataSources.Add("Data", dataSource);
            mergeOptions.DataSource     = dataSource;
            mergeOptions.DataMember     = dataMember;
            mergeOptions.DataSourceName = "Data";
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

        private void BarViewDraft_ItemClick(object sender, ItemClickEventArgs e)
        {
            Editor.ActiveViewType = RichEditViewType.Draft;
        }

        private void BarViewPrintLayout_ItemClick(object sender, ItemClickEventArgs e)
        {
            Editor.ActiveViewType = RichEditViewType.PrintLayout;
        }

        private void BarViewSimple_ItemClick(object sender, ItemClickEventArgs e)
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

        private void BarEditStyle_ItemClick(object sender, ItemClickEventArgs e)
        {
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
                barItem.ImageOptions.SvgImage = svgImages["style"];
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
    }

    #region BookTemplateICommandUIStateManagerService
    public class BookTemplateICommandUIStateManagerService : ICommandUIStateManagerService
    {
        public void UpdateCommandUIState(DevExpress.Utils.Commands.Command command, DevExpress.Utils.Commands.ICommandUIState state)
        {
            if (!(command is RichEditCommand richEditCommand))
                return; 

            if (richEditCommand.Id == SnapCommandId.NewDataSource)
            { 
                state.Enabled = false;
                state.Visible = false;
            }
            else if (richEditCommand.Id == SnapCommandId.MailMergeDataSource)
            {
                state.Enabled = false;
                state.Visible = false;
            }
        }
    }
    #endregion

    #region BookTemplateMenuCommandHandler
    public class BookTemplateMenuCommandHandler : DevExpress.Snap.MenuCommandHandler
    {
        public BookTemplateMenuCommandHandler(SnapControl snapControl) : base(snapControl)
        {
        }

        public override void UpdateCommandStatus()
        {
            base.UpdateCommandStatus();

            foreach (CommandSetItem command in commands)
            {
                if (command.CommandID.Equals(DataExplorerCommands.AddDataSource))
                {
                    // Remove Add Datasource Command in Data Explorer, because this will call the default function.
                    command.Supported = false;
                }

                // Set empty Executor to prevent secondary call of the same method which will cause exceptions.
                // I don't understand why this is called after the default call.
                execHT[command.CommandID] = new EmptyCommandExecutor();
            }
        }
    }
    #endregion

    #region EmptyCommandExecutor
    public class EmptyCommandExecutor : DevExpress.XtraReports.Design.Commands.ICommandExecutor
    {
        public void ExecCommand(CommandID cmdID, object[] parameters)
        {
        }

        public void Dispose()
        {
        }
    }
    #endregion
}