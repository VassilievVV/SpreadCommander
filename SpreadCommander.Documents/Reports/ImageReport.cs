using System;
using System.Drawing;
using DevExpress.Utils.Svg;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Drawing;

namespace SpreadCommander.Documents.Reports
{
    public partial class ImageReport : DevExpress.XtraReports.UI.XtraReport
    {
        public ImageReport()
        {
            InitializeComponent();
            PrintingSystem.AfterMarginsChange  += PrintingSystem_AfterMarginsChange;
            PrintingSystem.PageSettingsChanged += PrintingSystem_PageSettingsChanged;
        }

        public ImageReport(Bitmap image): this()
        {
            PictureBox.ImageSource = new ImageSource(image);
        }

        public ImageReport(SvgImage image): this()
        {
            PictureBox.ImageSource = new ImageSource(image);
        }

        private void PrintingSystem_AfterMarginsChange(object sender, DevExpress.XtraPrinting.MarginsChangeEventArgs e)
        {
            Convert.ToInt32(Math.Round(e.Value));
            switch (e.Side) {
                case MarginSide.Left:
                    Margins = new System.Drawing.Printing.Margins((int)e.Value, Margins.Right, Margins.Top, Margins.Bottom);
                    CreateDocument();
                    break;
                case MarginSide.Right:
                    Margins = new System.Drawing.Printing.Margins(Margins.Left, (int)e.Value, Margins.Top, Margins.Bottom);
                    CreateDocument();
                    break;
                case MarginSide.Top:
                    Margins = new System.Drawing.Printing.Margins(Margins.Left, Margins.Right, (int)e.Value, Margins.Bottom);
                    CreateDocument();
                    break;
                case MarginSide.Bottom:
                    Margins = new System.Drawing.Printing.Margins(Margins.Left, Margins.Right, Margins.Top, (int)e.Value);
                    CreateDocument();
                    break;
                case MarginSide.All:
                    Margins = (sender as DevExpress.XtraPrinting.PrintingSystemBase).PageSettings.Margins;
                    CreateDocument();
                    break;
                default:
                    break;
            }
        }

        private void PrintingSystem_PageSettingsChanged(object sender, EventArgs e)
        {
            XtraPageSettingsBase pageSettings = ((PrintingSystemBase)sender).PageSettings;
            PaperKind = pageSettings.PaperKind;
            Landscape = pageSettings.Landscape;
            Margins = new System.Drawing.Printing.Margins(pageSettings.LeftMargin, pageSettings.RightMargin, pageSettings.TopMargin, pageSettings.BottomMargin);
            CreateDocument();
        }

        private void AdjustControls()
        {
            float newWidth      = PageWidth - Margins.Left - Margins.Right;
            float newHeight     = PageHeight - Margins.Top - Margins.Bottom;
            PictureBox.SizeF = new SizeF(newWidth, newHeight);
        }

        private void XtraReport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            AdjustControls();
        }
    }
}
