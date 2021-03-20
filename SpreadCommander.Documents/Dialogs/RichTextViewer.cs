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
using SpreadCommander.Common;
using System.Threading;
using SpreadCommander.Common.Code;
using DevExpress.XtraRichEdit;
using SpreadCommander.Documents.Code;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraEditors;
using System.IO;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class RichTextViewer : BaseRibbonForm
    {
        public RichTextViewer()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);

            //Disable removing styles
            barRemoveStyle.Visibility = BarItemVisibility.Never;
        }

        private void IntellisenseCommentForm_Load(object sender, EventArgs e)
        {
        }

        private void IntellisenseCommentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (editHelp.Modified)
                editHelp.SaveDocument(this);
        }

        private void IntellisenseCommentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        public void LoadHtmlText(string htmlText)
        {
            editHelp.HtmlText = htmlText;
            editHelp.Modified = false;
        }

        public void LoadDocument(Stream stream)
        {
            editHelp.LoadDocument(stream);
            editHelp.Modified = false;
        }

        private bool _ZoomChanging;
        private void BarZoom_EditValueChanged(object sender, EventArgs e)
        {
            if (_ZoomChanging)
                return;

            using (new UsingProcessor(() => _ZoomChanging = true, () => _ZoomChanging = false))
            {
                int value                      = Convert.ToInt32(barZoom.EditValue);
                editHelp.ActiveView.ZoomFactor = value / 100f;
                barZoom.Caption                = $"{value}%";
            }
        }

        private void EditHelp_ZoomChanged(object sender, EventArgs e)
        {
            if (_ZoomChanging)
                return;

            using (new UsingProcessor(() => _ZoomChanging = true, () => _ZoomChanging = false))
            {
                int value         = Convert.ToInt32(editHelp.ActiveView.ZoomFactor * 100);
                barZoom.EditValue = value;
                barZoom.Caption   = $"{value}%";
            }
        }

        private void EditHelp_ActiveViewChanged(object sender, EventArgs e)
        {
            if (_ZoomChanging)
                return;

            using (new UsingProcessor(() => _ZoomChanging = true, () => _ZoomChanging = false))
            {
                int value         = Convert.ToInt32(editHelp.ActiveView.ZoomFactor * 100);
                barZoom.EditValue = value;
                barZoom.Caption   = $"{value}%";
            }

            switch (editHelp.ActiveViewType)
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
            editHelp.ActiveViewType = RichEditViewType.Draft;
        }

        private void BarViewPrintLayout_ItemClick(object sender, ItemClickEventArgs e)
        {
            editHelp.ActiveViewType = RichEditViewType.PrintLayout;
        }

        private void BarViewSimple_ItemClick(object sender, ItemClickEventArgs e)
        {
            editHelp.ActiveViewType = RichEditViewType.Simple;
        }

        private void EditHelp_UpdateUI(object sender, EventArgs e)
        {
            if (editHelp.ActiveView is PageBasedRichEditView printView)
                barPages.Caption = $"Page {printView.CurrentPageIndex + 1} of {printView.PageCount}";
            else
                barPages.Caption = string.Empty;
        }

        private void BarNewStyle_ItemClick(object sender, ItemClickEventArgs e)
        {
            var styleNames   = editHelp.Document.ParagraphStyles.Select(s => s.Name).ToList();
            var newStyleName = Utils.AddUniqueString(styleNames, "NewStyle1", StringComparison.CurrentCultureIgnoreCase, false);

            var newStyle  = editHelp.Document.ParagraphStyles.CreateNew();
            newStyle.Name = newStyleName;
            editHelp.Document.ParagraphStyles.Add(newStyle);

            var cmd = editHelp.CreateCommand(RichEditCommandId.ShowEditStyleForm);
            cmd.Execute();
        }

        private void BarEditStyles_ItemClick(object sender, ItemClickEventArgs e)
        {
            var cmd = editHelp.CreateCommand(RichEditCommandId.ShowEditStyleForm);
            cmd.Execute();
        }

        private void PopupBookStyles_BeforePopup(object sender, CancelEventArgs e)
        {
            popupBookStyles.ClearLinks();

            var styleNames = editHelp.Document.ParagraphStyles.Select(s => s.Name).ToList();
            styleNames.Sort(StringLogicalComparer.DefaultComparer);

            foreach (var styleName in styleNames)
            {
                var barItem = new BarButtonItem(popupBookStyles.Manager, styleName);
                barItem.ImageOptions.SvgImage = svgImages["style"];
                barItem.ItemClick += (s, args) =>
                {
                    var name = args.Item.Caption;
                    var style = editHelp.Document.ParagraphStyles[name];

                    if (style == null)
                    {
                        XtraMessageBox.Show(this, "Style does not exist in document", "Cannot find style",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var paragraph = editHelp.Document.Paragraphs.FirstOrDefault(p => p.Style == style);
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
                            editHelp.Document.ParagraphStyles.Delete(style);
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
    }
}