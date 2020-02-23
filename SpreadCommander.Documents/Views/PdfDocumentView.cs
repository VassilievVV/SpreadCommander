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
using SpreadCommander.Documents.Code;
using DevExpress.Utils.Svg;

namespace SpreadCommander.Documents.Views
{
    public partial class PdfDocumentView : DevExpress.XtraBars.Ribbon.RibbonForm, PdfDocumentViewModel.ICallback, IImageHolder
    {
        public PdfDocumentView()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);
        }

        private void PdfDocumentView_Load(object sender, EventArgs e)
        {
            ActiveControl = pdfViewer;
            pdfViewer.Focus();
        }

        public SvgImage GetControlImage() =>
            svgFormIcon.Count > 0 ? svgFormIcon[0] : null;

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<PdfDocumentViewModel>();
            fluent.ViewModel.InitializeBindings();
            fluent.ViewModel.Callback = this;
        }

        public void LoadFromFile(string fileName)
        {
            pdfViewer.LoadDocument(fileName);
        }

        public void SaveToFile(string fileName)
        {
            pdfViewer.SaveDocument(fileName);
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
    }
}
