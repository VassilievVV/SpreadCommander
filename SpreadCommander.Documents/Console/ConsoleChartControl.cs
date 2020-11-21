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
using SpreadCommander.Documents.Code;
using DevExpress.XtraBars.Ribbon;
using System.Data.Common;
using SpreadCommander.Common.Code;
using DevExpress.XtraCharts.Wizard;
using SpreadCommander.Documents.ViewModels;
using DevExpress.XtraCharts.Native;
using DevExpress.Compression;
using System.IO;
using Svg;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Xml;
using SpreadCommander.Documents.Reports;
using DevExpress.XtraReports.UI;
using SpreadCommander.Common.PowerShell.CmdLets.Book;
using System.Threading;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleChartControl : ConsoleCustomControl, IRibbonHolder, ChartDocumentViewModel.IChartCallback
    {
        public enum ChartDocumentMode { Standard, FixedDataSource }

        public ConsoleChartControl()
        {
            InitializeComponent();
            UIUtils.ConfigureRibbonBar(Ribbon);

            barChartDpi.EditValue = BaseBookCmdlet.DefaultDPI;
        }

        public override string Caption => "Chart";
        public override DevExpress.Utils.Svg.SvgImage CaptionSvgImage => images["chart"];

        private ChartDocumentMode _DocumentMode;
        public ChartDocumentMode DocumentMode
        {
            get => _DocumentMode;
            protected set
            {
                _DocumentMode = value;
                ribbonPageGroupDataSource.Visible = (_DocumentMode != ChartDocumentMode.FixedDataSource);
            }
        }

        private void ConsoleChartControl_Load(object sender, EventArgs e)
        {
        }

        private void GalleryDataSources_Gallery_ItemClick(object sender, GalleryItemClickEventArgs e)
        {
            if (DocumentMode == ChartDocumentMode.FixedDataSource)
                return;

            if (e.Item.Tag is not DataTable table)
                return;

            ActiveTable = table;
            TableName   = table.TableName;

            if (ActiveTable != null && Chart.Series.Count <= 0)
                runDesignerChartItem1.PerformClick();

            FireModified(true);
        }

        protected override void ViewModelChanged()
        {
            var model = ViewModel;

            if (model is ChartDocumentViewModel chartDocumentViewModel)
                chartDocumentViewModel.ChartCallback = this;
            else if (model is PivotDocumentViewModel pivotDocumentViewModel)
                pivotDocumentViewModel.ChartCallback = this;
        }

        protected override void DataSetChanged(DataSet oldDataSet, DataSet newDataSet)
        {
            base.DataSetChanged(oldDataSet, newDataSet);

            if (oldDataSet != null)
                oldDataSet.Tables.CollectionChanged -= DataSetTables_CollectionChanged;

            if (newDataSet != null)
                newDataSet.Tables.CollectionChanged += DataSetTables_CollectionChanged;

            DataSetTables_CollectionChanged(newDataSet?.Tables, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
        }

        RibbonControl IRibbonHolder.Ribbon            => Ribbon;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => RibbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => Ribbon.Visible;
            set
            {
                Ribbon.Visible = value;
                RibbonStatusBar.Visible = value;
            }
        }

        Chart ChartDocumentViewModel.IChartCallback.Chart => ((IChartContainer)Chart).Chart;
        public ChartDocumentViewModel.IModelWithChart ModelWithChart => (ChartDocumentViewModel.IModelWithChart)ViewModel;
        
        public void ClearChart()
        {
            Chart.Series.Clear();
            Chart.Titles.Clear();
            Chart.Legends.Clear();
            Chart.SeriesDataMember                = string.Empty;
            Chart.SeriesTemplate.SeriesDataMember = string.Empty;

            FireModified(true);
        }

        public string TableName { get; set; }
        
        public DataTable ActiveTable
        {
            get => Chart.DataSource as DataTable;
            set
            {
                if (DocumentMode == ChartDocumentMode.FixedDataSource)
                    return;

                if (Chart.DataSource == value)
                    return;

                Chart.DataSource = value;
                FireModified(true);
            }
        }

        public object DataSource
        {
            get => Chart.DataSource;
            set { Chart.DataSource = value; }
        }

        private void DataSetTables_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if (DocumentMode == ChartDocumentMode.FixedDataSource)
                return;
            
            var items = galleryDataSources.Gallery.Groups[0].Items;
            var table = (DataTable)e.Element;

            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (table == null)
                        return;

                    AddGalleryItem(table);

                    if (ActiveTable == null && string.Compare(table.TableName, TableName, true) == 0)
                    {
                        ActiveTable = table;
                        TableName   = table.TableName;
                    }
                    break;
                case CollectionChangeAction.Remove:
                    int i = items.Count - 1;
                    while (i >= 0)
                    {
                        if (items[i].Tag == table)
                            items.RemoveAt(i);

                        i--;
                    }

                    if (ActiveTable == table)
                    {
                        ActiveTable = null;
                        TableName   = null;
                    }
                    break;
                case CollectionChangeAction.Refresh:
                    ActiveTable = null;                    
                    items.Clear();

                    var dataSet = DataSet;
                    if (dataSet != null)
                    {
                        var tableName = TableName;

                        foreach (DataTable tbl in dataSet.Tables)
                            AddGalleryItem(tbl);

                        if (!string.IsNullOrWhiteSpace(tableName))
                        {
                            var dataTable = dataSet.Tables[tableName];
                            {
                                ActiveTable = dataTable;
                                TableName   = tableName;
                            }
                        }
                    }
                    break;
            }

            FireModified(true);


            void AddGalleryItem(DataTable tbl)
            {
                if (tbl == null)
                    return;
                
                var galleryItem = new GalleryItem()
                {
                    Caption = tbl.TableName,
                    Tag     = tbl
                };
                galleryItem.ImageOptions.SvgImage = images32["table"];
                items.Add(galleryItem);

                if (ActiveTable == null && TableName == null)
                {
                    ActiveTable = tbl;
                    TableName   = tbl.TableName;
                }
            }
        }

        private void BarLoadTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return;

            ModelWithChart.LoadChart(dlgOpen.FileName);
        }

        private void BarSaveTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgSave.ShowDialog(this) != DialogResult.OK)
                return;

            ModelWithChart.SaveChart(dlgSave.FileName);
        }

        private XmlDocument SaveChartAsSvg(int width, int height)
        {
            int w = Convert.ToInt32(width * 96.0 / 300.0);
            int h = Convert.ToInt32(height * 96.0 / 300.0);

            var chart = ((IChartContainer)Chart).Chart;

            var svg = chart.CreateSvg(new Size((int)w, (int)h));
            return svg;
        }

        private async Task<Bitmap> SaveChartAsBitmap(int width, int height, int dpi, CancellationToken cancellationToken)
        {
            float scale = dpi / 96f;

            int w = Convert.ToInt32(width * 96.0 / 300.0);
            int h = Convert.ToInt32(height * 96.0 / 300.0);
            
            var svg = SaveChartAsSvg(width, height);

            Bitmap bitmap = null;

            await Task.Run(() =>
            {
                var doc            = SvgDocument.Open(svg);
                doc.ShapeRendering = SvgShapeRendering.GeometricPrecision;

                bitmap = new Bitmap((int)Math.Ceiling(w * scale), (int)Math.Ceiling(h * scale));
                bitmap.SetResolution(dpi, dpi);

                if (cancellationToken.IsCancellationRequested)
                    return;

                using var graphics = Graphics.FromImage(bitmap);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode   = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode     = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                graphics.Clear(Color.White);

                var renderer = SvgRenderer.FromGraphics(graphics);
                renderer.SmoothingMode = SmoothingMode.HighQuality;
                if (scale != 1f)
                    renderer.ScaleTransform(scale, scale);

                doc.Draw(renderer);
            }, cancellationToken).ConfigureAwait(true);

            return bitmap;
        }

        private (int width, int height, int dpi) GetImageSize()
        {
            int width  = Utils.ValueInRange(Convert.ToInt32(barChartWidth.EditValue),  300, 20000);
            int height = Utils.ValueInRange(Convert.ToInt32(barChartHeigth.EditValue), 200, 20000);
            int dpi    = Utils.ValueInRange(Convert.ToInt32(barChartDpi.EditValue),    48,  4800);

            return (width, height, dpi);
        }

        private async void PrintChart(bool preview)
        {
            (int width, int height, int dpi) = GetImageSize();

            try
            {
                using var png = await SaveChartAsBitmap(width, height, dpi, CancellationToken.None).ConfigureAwait(true);
                var report = new ImageReport(png)
                {
                    Landscape = (width > height)
                };
                using var printTool = new ReportPrintTool(report);
                if (preview)
                    printTool.ShowRibbonPreviewDialog();
                else
                    printTool.Print();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BarPrintPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintChart(true);
        }

        private void BarPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PrintChart(false);
        }

        private async void BarSaveAsSVG_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgSaveSvg.ShowDialog(this) != DialogResult.OK)
                return;

            var fileName = dlgSaveSvg.FileName;
            (int width, int height, int _) = GetImageSize();

            var svg = SaveChartAsSvg(width, height);

            try
            {
                await Task.Run(() => { svg.Save(fileName); }).ConfigureAwait(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void BarSaveAsPNG_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgSavePng.ShowDialog(this) != DialogResult.OK)
                return;

            var fileName = dlgSavePng.FileName;
            (int width, int height, int dpi) = GetImageSize();

            try
            {
                using var png = await SaveChartAsBitmap(width, height, dpi, CancellationToken.None).ConfigureAwait(true);
                await Task.Run(() => { png.Save(fileName); }).ConfigureAwait(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RunDesignerChartItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FireModified(true);
        }
    }
}
