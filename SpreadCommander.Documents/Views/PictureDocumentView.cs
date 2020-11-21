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
using DevExpress.XtraEditors.Controls;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;
using SpreadCommander.Documents.Messages;

namespace SpreadCommander.Documents.Views
{
    public partial class PictureDocumentView : DevExpress.XtraBars.Ribbon.RibbonForm, PictureDocumentViewModel.ICallback, IImageHolder
    {
        public PictureDocumentView()
        {
            using var _ = new DocumentAddingProcessor(this);

            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);
        }

        private void PictureDocumentView_Load(object sender, EventArgs e)
        {
            ActiveControl = Picture;
            Picture.Focus();
        }

        public SvgImage GetControlImage() =>
            svgFormIcon.Count > 0 ? svgFormIcon[0] : null;

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<PictureDocumentViewModel>();
            fluent.ViewModel.InitializeBindings();

            fluent.ViewModel.Callback = this;
        }

        public void LoadFromFile(string fileName)
        {
            Picture.LoadAsync(fileName);
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

        private void BarOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Picture.LoadImage();

            var fluent = mvvmContext.OfType<PictureDocumentViewModel>();
            fluent.ViewModel.FileName = Picture.GetLoadedImageLocation();
            fluent.ViewModel.Modified = false;
        }

        private void BarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Printing.PrintControl(this, Picture);
        }

        private void BarZoomOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Picture.Properties.SizeMode = PictureSizeMode.Clip;
            Picture.Properties.ZoomPercent *= 0.9;
        }

        private void BarZoomIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Picture.Properties.SizeMode = PictureSizeMode.Clip;
            Picture.Properties.ZoomPercent *= 1.1;
        }

        private void BarZoom100_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Picture.Properties.SizeMode = PictureSizeMode.Clip;
            Picture.Properties.ZoomPercent = 100;
        }

        private void BarZoomPage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Picture.Properties.SizeMode = PictureSizeMode.Squeeze;
        }

        private void RepositoryItemZoomTrackBar_EditValueChanged(object sender, EventArgs e)
        {
            var value = Convert.ToDouble(barZoom.EditValue);
            if (value >= 10 && value <= 190)
            {
                var percent = Math.Pow(value / 100.0, 3.3) * 100;

                Picture.Properties.SizeMode = PictureSizeMode.Clip;
                Picture.Properties.ZoomPercent = percent;
            }
        }

        private void Picture_ZoomPercentChanged(object sender, EventArgs e)
        {
            var zoom = Convert.ToInt32(Math.Pow(Picture.Properties.ZoomPercent / 100.0, 1.0 / 3.3) * 100);

            if (zoom >= repositoryItemZoomTrackBar.Minimum && zoom <= repositoryItemZoomTrackBar.Maximum)
                barZoom.EditValue = zoom;
        }
    }
}
