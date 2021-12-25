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
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraBars.Ribbon;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Book;
using DevExpress.Utils.Animation;
using System.IO;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Common;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraBars;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleBookControl : ConsoleBaseControl, IRibbonHolder
    {
        private InternalBook SCBook;

        private int _SilentUpdateCounter;

        public ConsoleBookControl()
        {
            InitializeComponent();

            SetupEditor();

            SCBook = new InternalBook(Editor);
            Disposed += ConsoleBookControl_Disposed;

            //Fix skin colors in comment control
            var commentEdit = richEditCommentControl1.Controls.OfType<InnerCommentControl>().FirstOrDefault();
            if (commentEdit != null)
            {
                commentEdit.Views.DraftView.AdjustColorsToSkins       = commentEdit.RichEditControl.Views.DraftView.AdjustColorsToSkins;
                commentEdit.Views.SimpleView.AdjustColorsToSkins      = commentEdit.RichEditControl.Views.SimpleView.AdjustColorsToSkins;
                commentEdit.Views.PrintLayoutView.AdjustColorsToSkins = commentEdit.RichEditControl.Views.PrintLayoutView.AdjustColorsToSkins;
            }

            UIUtils.ConfigureRibbonBar(Ribbon);
            Ribbon.SelectedPage = homeRibbonPage1;

            ProjectUriStreamProvider.RegisterProvider(Editor);

            //Disable removing styles
            barRemoveStyle.Visibility = BarItemVisibility.Never;
        }

        public Document Document      => Editor?.Document;

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

        private void ConsoleBookControl_Disposed(object sender, EventArgs e)
        {
            SCBook?.Dispose();
            SCBook = null;
        }

        private void SetupEditor()
        {
            bool oldModified = Editor.Modified;
            _SilentUpdateCounter++;
            try
            {
                var charProperties      = Editor.Document.DefaultCharacterProperties;
                charProperties.FontName = "Lucida Console";
                charProperties.FontSize = 8.25F;
            }
            finally
            {
                Editor.Modified = oldModified;
                _SilentUpdateCounter--;
            }
        }

        private void Editor_DocumentLoaded(object sender, EventArgs e)
        {
            SetupEditor();
        }

        private void Editor_ContentChanged(object sender, EventArgs e)
        {
            if (_SilentUpdateCounter > 0)
                return;

            try
            {
                if (Editor.Modified)
                    FireModified(false);
            }
            catch (Exception)
            {
                //Do nothing
            }
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

        private void BarClone_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var book = Document ?? throw new NullReferenceException("Book is not loaded yet.");
            var modified = Editor.Modified;

            using (new UsingProcessor(
                () => transitionManager.StartWaitingIndicator(Editor, WaitingAnimatorType.Default),
                () => transitionManager.StopWaitingIndicator()))
            {
                using var stream = new MemoryStream(65536);
                book.SaveDocument(stream, DocumentFormat.OpenXml);
                stream.Seek(0, SeekOrigin.Begin);
                Editor.Modified = modified;

                var newBookModel = BaseDocumentViewModel.StaticAddNewBookModel();
                newBookModel.LoadFromStream(stream);
                newBookModel.Modified = true;
            }
        }

        private void BarLoadTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dir = Project.Current.MapPath("~#\\Templates");
            if (Directory.Exists(dir))
                dlgOpen.InitialDirectory = dir;

            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return;

            Editor.LoadDocumentTemplate(dlgOpen.FileName);
        }

        private void BarSaveTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var dir = Project.Current.MapPath("~#\\Templates");
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

        private void BarNewStyle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var styleNames   = Editor.Document.ParagraphStyles.Select(s => s.Name).ToList();
            var newStyleName = Utils.AddUniqueString(styleNames, "NewStyle1", StringComparison.CurrentCultureIgnoreCase, false);

            var newStyle  = Editor.Document.ParagraphStyles.CreateNew();
            newStyle.Name = newStyleName;
            Editor.Document.ParagraphStyles.Add(newStyle);

            var cmd = Editor.CreateCommand(RichEditCommandId.ShowEditStyleForm);
            cmd.Execute();
        }

        private void BarEditStyles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                    var style = Editor.Document.ParagraphStyles[name];

                    if (style == null)
                    {
                        XtraMessageBox.Show(this, "Style does not exist in document", "Cannot find style",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var paragraph = Editor.Document.Paragraphs.FirstOrDefault(p => p.Style == style);
                    if (paragraph != null)
                    {
                        XtraMessageBox.Show(this, "Cannot delete style that is using in document.", "Style is using",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (XtraMessageBox.Show(this, $"Do you want to delete style '{name}'?", "Confirm delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;

                    if (style != null)
                    {
                        try
                        {
                            Editor.Document.ParagraphStyles.Delete(style);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(this, ex.Message, "Cannot delete style",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };
                popupBookStyles.AddItem(barItem);
            }
        }

        private void BarUpdateMailMergeFields_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (new UsingProcessor(
                () => transitionManager.StartWaitingIndicator(Editor, WaitingAnimatorType.Default),
                () => transitionManager.StopWaitingIndicator()))
            {
                Document.UpdateAllFields();
            }
        }
    }
}
