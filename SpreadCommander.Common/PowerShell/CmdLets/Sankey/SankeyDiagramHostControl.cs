using DevExpress.Utils.DPI;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraCharts.Sankey;
using DevExpress.XtraCharts.Sankey.Native;
using DevExpress.XtraCharts.Sankey.Printing.Native;
using DevExpress.XtraPrinting;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SpreadCommander.Common.PowerShell.CmdLets.Sankey
{
    public class SankeyDiagramHostControl : IDisposable, ISankeyContainer, ISankeyRenderProvider, IPrintable
    {
        #region SankeyErrorMessageEventArgs
        public class SankeyErrorMessageEventArgs
        {
            public string Title     { get; set; }
            public string Message   { get; set; }
        }
        #endregion

        protected readonly SankeyDiagram innerControl;
        protected readonly SankeySelectionController selectionController;
        protected readonly SankeyPrinter printer;
        protected bool isDisposed = false;
        protected static ScaleHelper DpiScaleHelper   => ScaleHelper.NoScale;
        protected SankeyDataController DataController => innerControl.DataController;
        protected static bool IsPrintingAvailable     => ComponentPrinterBase.IsPrintingAvailable(false);

        public event EventHandler<SankeyErrorMessageEventArgs> ErrorMessage;

        public Color BackColor
        {
            get { return innerControl.BackColor; }
            set { innerControl.BackColor = value; }
        }

        public object DataSource
        {
            get { return DataController.DataSource; }
            set { DataController.DataSource = value; }
        }

        public string SourceDataMember
        {
            get { return DataController.SourceDataMember; }
            set { DataController.SourceDataMember = value; }
        }

        public string TargetDataMember
        {
            get { return DataController.TargetDataMember; }
            set { DataController.TargetDataMember = value; }
        }

        public string WeightDataMember
        {
            get { return DataController.WeightDataMember; }
            set { DataController.WeightDataMember = value; }
        }

        public SankeyTitleCollection Titles
        {
            get { return innerControl.Titles; }
        }

        public ISankeyColorizer Colorizer
        {
            get { return innerControl.Colorizer; }
            set { innerControl.Colorizer = value; }
        }

        public SankeyNodeLabel NodeLabel         => innerControl.NodeLabel;
        public SankeyBorderOptions BorderOptions => innerControl.BorderOptions;
        public SankeyViewOptions ViewOptions     => innerControl.ViewOptions;
        public EmptySankeyText EmptySankeyText   => innerControl.EmptySankeyText;
        public SmallSankeyText SmallSankeyText   => innerControl.SmallSankeyText;
        public SankeyPadding Padding             => innerControl.Padding;

        public int Width  { get; set; } = 2000;
        public int Height { get; set; } = 1200;

        public IList SelectedItems => selectionController.SelectedItems;

        public SankeyDiagramHostControl()
        {
            innerControl = new SankeyDiagram(this);

            var printerPropertyInfo = typeof(SankeyDiagram).GetProperty("Printer", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            printer                 = printerPropertyInfo.GetValue(innerControl) as SankeyPrinter;

            var selectionPropertyInfo = typeof(SankeyDiagram).GetProperty("SelectionController", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            selectionController       = selectionPropertyInfo.GetValue(innerControl) as SankeySelectionController;
            selectionController.SelectionMode = SankeySelectionMode.Multiple;
        }
        #region ISankeyContainer

        private SizeF dpiScaleFactor = new (1f, 1f);
        SizeF ISankeyContainer.DpiScaleFactor                 => dpiScaleFactor; 
        bool ISankeyContainer.DesignMode                      => false;
        ISankeyRenderProvider ISankeyContainer.RenderProvider => this;
        IServiceProvider ISankeyContainer.ServiceProvider     => null;
        ISite ISankeyContainer.Site                           => null;
        SankeyDiagram ISankeyContainer.InnerControl           => innerControl;

        void ISankeyContainer.OnCustomizeNode(CustomizeSankeyNodeEventArgs e)
        {
        }
        void ISankeyContainer.OnCustomizeNodeToolTip(CustomizeSankeyNodeToolTipEventArgs e)
        {
        }
        void ISankeyContainer.OnCustomizeLinkToolTip(CustomizeSankeyLinkToolTipEventArgs e)
        {
        }
        void ISankeyContainer.OnHighlightedItemsChanged(SankeyHighlightedItemsChangedEventArgs e)
        {
        }
        void ISankeyContainer.OnSelectedItemsChanged(SankeySelectedItemsChangedEventArgs e)
        {
        }
        void ISankeyContainer.OnSelectedItemsChanging(SankeySelectedItemsChangingEventArgs e)
        {
        }
        void ISankeyContainer.ShowErrorMessage(string message, string title)
        {
            ErrorMessage?.Invoke(this, new SankeyErrorMessageEventArgs() { Title = title, Message = message });
        }
        #endregion

        #region ISankeyRenderProvider 
        ISankeyStyle ISankeyRenderProvider.Style      => null;
        IBasePrintable IChartRenderProvider.Printable => null;
        object IChartRenderProvider.LookAndFeel       => null;
        void IChartRenderProvider.Invalidate()
        {
        }
        void IChartRenderProvider.InvokeInvalidate()
        {
        }
        Bitmap IChartRenderProvider.LoadBitmap(string url)
        {
            return null;
        }
        ComponentExporter IChartRenderProvider.CreateComponentPrinter(IBasePrintable iPrintable)
        {
            return new ComponentPrinter(iPrintable as IPrintable);
        }
        System.Drawing.Rectangle IChartRenderProvider.DisplayBounds => new (0, 0, Width, Height);
        bool IChartRenderProvider.IsPrintingAvailable               => IsPrintingAvailable;
        void ISankeyRenderProvider.Changed()
        {
        }
        #endregion

        #region IBasePrintable
        void IBasePrintable.CreateArea(string areaName, IBrickGraphics graph)
        {
            printer.CreateArea(areaName, graph);
        }
        void IBasePrintable.Finalize(IPrintingSystem printingSystem, ILink link)
        {
            printer.Release();
        }
        void IBasePrintable.Initialize(IPrintingSystem printingSystem, ILink link)
        {
            printer.Initialize(printingSystem, link);
        }
        bool IBasePrintable.CreatesIntersectedBricks => true;
        #endregion

        #region IPrintable 
        UserControl IPrintable.PropertyEditorControl => null;
        bool IPrintable.HasPropertyEditor()
        {
            return false;
        }
        bool IPrintable.SupportsHelp()
        {
            return false;
        }
        void IPrintable.ShowHelp()
        {
        }
        void IPrintable.AcceptChanges()
        {
        }
        void IPrintable.RejectChanges()
        {
        }
        #endregion
        
        public void Dispose()
        {
            if (isDisposed)
                return;
            isDisposed = true;

            innerControl?.Dispose();
        }

        public void SaveToStream(Stream stream)
        {
            innerControl.SaveLayout(stream);
        }
        public void SaveToFile(string path)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            SaveToStream(fs);
        }
        public void LoadFromStream(Stream stream)
        {
            stream.Seek(0L, SeekOrigin.Begin);
            if (!XtraSerializingHelper.IsValidSankeyXml(stream))
                throw new LayoutStreamException();
            stream.Seek(0L, SeekOrigin.Begin);
            innerControl.LoadLayout(stream);
        }
        public void LoadFromFile(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            LoadFromStream(fs);
        }

        #region Printing 
        public void Print()
        {
            innerControl.Print();
        }
        public void ShowPrintDialog()
        {
            innerControl.ShowPrintDialog();
        }
        public void ShowPrintPreview()
        {
            innerControl.ShowPrintPreview();
        }
        public void ShowRibbonPrintPreview()
        {
            innerControl.ShowRibbonPrintPreview();
        }
        #endregion

        #region ExportTo
        public void ExportToSvg(string filePath)                             => innerControl.ExportToSvg(filePath);
        public void ExportToSvg(Stream stream)                               => innerControl.ExportToSvg(stream);
        public void ExportToHtml(string filePath)                            => innerControl.Export(ExportTarget.Html, filePath);
        public void ExportToHtml(string filePath, HtmlExportOptions options) => innerControl.Export(ExportTarget.Html, filePath, options);
        public void ExportToHtml(Stream stream)                              => innerControl.Export(ExportTarget.Html, stream);
        public void ExportToHtml(Stream stream, HtmlExportOptions options)   => innerControl.Export(ExportTarget.Html, stream, options);
        public void ExportToMht(string filePath)                             => innerControl.Export(ExportTarget.Mht, filePath);
        public void ExportToMht(string filePath, MhtExportOptions options)   => innerControl.Export(ExportTarget.Mht, filePath, options);
        public void ExportToMht(Stream stream)                               => innerControl.Export(ExportTarget.Mht, stream);
        public void ExportToMht(Stream stream, MhtExportOptions options)     => innerControl.Export(ExportTarget.Mht, stream, options);
        public void ExportToPdf(string filePath)                             => innerControl.Export(ExportTarget.Pdf, filePath);
        public void ExportToPdf(string filePath, PdfExportOptions options)   => innerControl.Export(ExportTarget.Pdf, filePath, options);
        public void ExportToPdf(Stream stream)                               => innerControl.Export(ExportTarget.Pdf, stream);
        public void ExportToPdf(Stream stream, PdfExportOptions options)     => innerControl.Export(ExportTarget.Pdf, stream, options);
        public void ExportToRtf(string filePath)                             => innerControl.Export(ExportTarget.Rtf, filePath);
        public void ExportToRtf(Stream stream)                               => innerControl.Export(ExportTarget.Rtf, stream);
        public void ExportToXls(string filePath)                             => innerControl.Export(ExportTarget.Xls, filePath);
        public void ExportToXls(string filePath, XlsExportOptions options)   => innerControl.Export(ExportTarget.Xls, filePath, options);
        public void ExportToXls(Stream stream)                               => innerControl.Export(ExportTarget.Xls, stream);
        public void ExportToXls(Stream stream, XlsExportOptions options)     => innerControl.Export(ExportTarget.Xls, stream, options);
        public void ExportToXlsx(string filePath)                            => innerControl.Export(ExportTarget.Xlsx, filePath);
        public void ExportToXlsx(string filePath, XlsxExportOptions options) => innerControl.Export(ExportTarget.Xlsx, filePath, options);
        public void ExportToXlsx(Stream stream)                              => innerControl.Export(ExportTarget.Xlsx, stream);
        public void ExportToXlsx(Stream stream, XlsxExportOptions options)   => innerControl.Export(ExportTarget.Xlsx, stream, options);
        public void ExportToImage(Stream stream, ImageFormat format)         => innerControl.ExportToImage(stream, format);
        public void ExportToImage(string filePath, ImageFormat format)       => innerControl.ExportToImage(filePath, format);
        public void ExportToDocx(string filePath)                            => innerControl.Export(ExportTarget.Docx, filePath);
        public void ExportToDocx(Stream stream)                              => innerControl.Export(ExportTarget.Docx, stream);
        #endregion
    }
}
