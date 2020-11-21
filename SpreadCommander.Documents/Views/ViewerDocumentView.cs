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
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Documents.Viewers;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Messages;

namespace SpreadCommander.Documents.Views
{
    public partial class ViewerDocumentView : DevExpress.XtraBars.Ribbon.RibbonForm,
        ViewerDocumentViewModel.ICallback, IImageHolder
    {
        private BaseViewer _Viewer;

        public ViewerDocumentView()
        {
            using var _ = new DocumentAddingProcessor(this);

            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);
        }

        public SvgImage GetControlImage() =>
            svgFormIcon.Count > 0 ? svgFormIcon[0] : null;

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<ViewerDocumentViewModel>();
            fluent.ViewModel.InitializeBindings();
            fluent.ViewModel.Callback = this;
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

        public void LoadFile(string fileName, StringNoCaseDictionary<string> parameters, List<BaseCommand> commands)
        {
            if (_Viewer != null)
            {
                _Viewer.Parent = null;
                _Viewer.Dispose();
                _Viewer = null;

                ribbonPageGroupPrint.Visible = false;
                ribbonPageGroupFind.Visible  = false;
                ribbonPageGroupZoom.Visible  = false;
            }

            _Viewer = BaseViewer.CreateViewer(this, fileName, parameters, commands);
            if (_Viewer != null)
            {
                _Viewer.Dock = DockStyle.Fill;
                _Viewer.Parent = this;
                _Viewer.BringToFront();

                ribbonPageGroupPrint.Visible = _Viewer.SupportPrint;
                ribbonPageGroupFind.Visible  = _Viewer.SupportFind;
                ribbonPageGroupZoom.Visible  = _Viewer.SupportZoom;
            }
        }

        private void BarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _Viewer?.Print();
        }

        private void RepositoryItemSearch_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            _Viewer?.Find(Convert.ToString(barSearch.EditValue));
        }

        private void BarZoomIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _Viewer?.ZoomIn();
        }

        private void BarZoomOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _Viewer?.ZoomOut();
        }

        private void BarZoom100_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            _Viewer.Zoom100();
        }
    }
}
