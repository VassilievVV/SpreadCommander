using SpreadCommander.Common.PowerShell.CmdLets.Book;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Sankey;
using System.Data;

namespace SpreadCommander.Common.PowerShell.CmdLets.Sankey
{
    public enum SCSankeyColorizerPalette { Default, Office, Office2013, Opulent, Orange, OrangeRed, Oriel, Origin, Paper, PastelKie, Red, NorthernLights,
        RedOrange, Slipstream, Solstice, Technic, TerracottaPie, TheTrees, Trek, Urban, Verve, Violet, VioletII, RedViolet, Yellow, NatureColors, Mixed,
        Apex, Aspect, BlackAndWhite, Blue, BlueGreen, BlueWarn, Chameleon, Module, Concourse, Civic, Flow, Foundry, Grayscale, Green, GreenYellow, InAFog, 
        Marquee, Median, Metro, Equity, YellowOrange }

    public class BaseSankeyDiagramCmdlet: BaseBookCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Name of a data member that contains source node labels.")]
        public string Source { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Name of a data member that contains target node labels.")]
        public string Target { get; set; }

        [Parameter(HelpMessage = "Name of a data member that contains link weights.")]
        public string Weight { get; set; }

        [Parameter(HelpMessage = "Name of a data member that specifies whether item is selected or no.")]
        public string Selected { get; set; }

        [Parameter(HelpMessage = "Name of nodes to display as selected.")]
        public string[] SelectedNodes { get; set; }

        [Parameter(HelpMessage = "Pallete using to color nodes.")]
        [DefaultValue(SCSankeyColorizerPalette.Default)]
        [PSDefaultValue(Value = SCSankeyColorizerPalette.Default)]
        public SCSankeyColorizerPalette Palette { get; set; } = SCSankeyColorizerPalette.Default;

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Width of the image in document units (1/300 of inch). Default value is 2000.")]
        [ValidateRange(300, 20000)]
        [PSDefaultValue(Value = 2000)]
        [DefaultValue(2000)]
        public int Width { get; set; } = 2000;

        [Parameter(HelpMessage = "Height of the image in document units (1/300 of inch). Default value is 1200.")]
        [ValidateRange(200, 20000)]
        [PSDefaultValue(Value = 1200)]
        [DefaultValue(1200)]
        public int Height { get; set; } = 1200;

        [Parameter(HelpMessage = "DPI of the image. Default value is 300.")]
        [ValidateRange(48, 4800)]
        public int? DPI { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Sankey diagram background color.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Font that is used to display the node label text.")]
        public string NodeFont { get; set; }

        [Parameter(HelpMessage = "Maximum width allowed for node labels in pixels.")]
        public int? NodeMaxWidth { get; set; }

        [Parameter(HelpMessage = "Number of lines that label text can wrap.")]
        public int? NodeMaxLineCount { get; set; }

        [Parameter(HelpMessage = "Node label alignment.")]
        public StringAlignment? NodeTextAlignment { get; set; }

        [Parameter(HelpMessage = "Text orientation for node labels.")]
        public TextOrientation? NodeTextOrientation { get; set; }

        [Parameter(HelpMessage = "Diagram border's color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Diagram border's thickness in pixels.")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Node width in pixels.")]
        public int? NodeWidth { get; set; }

        [Parameter(HelpMessage = "Indent between nodes in pixels.")]
        public int? VerticalNodeAlignment { get; set; }

        [Parameter(HelpMessage = "Link transparency.")]
        public byte? LinkTransparency { get; set; }

        [Parameter(HelpMessage = "Diagram's padding")]
        public int[] Padding { get; set; }

        [Parameter(HelpMessage = "Text that is displayed at runtime when a diagram has no data to display.")]
        public string EmptyText { get; set; }

        [Parameter(HelpMessage = "Font of empty text.")]
        public string EmptyTextFont { get; set; }

        [Parameter(HelpMessage = "Alignment of empty text.")]
        public StringAlignment? EmptyTextAlignment { get; set; }

        [Parameter(HelpMessage = "Text that is displayed in the diagram when it is too small.")]
        public string SmallText { get; set; }

        [Parameter(HelpMessage = "Font of small text.")]
        public string SmallTextFont { get; set; }

        [Parameter(HelpMessage = "Alignment of small text.")]
        public StringAlignment? SmallTextAlignment { get; set; }

        [Parameter(HelpMessage = "Title's text.")]
        public string TitleText { get; set; }

        [Parameter(HelpMessage = "Title's font.")]
        public string TitleFont { get; set; }

        [Parameter(HelpMessage = "Title's alignment.")]
        public StringAlignment? TitleAlignment { get; set; }

        [Parameter(HelpMessage = "Title's dock style.")]
        public SankeyTitleDockStyle? TitleDock { get; set; }

        [Parameter(HelpMessage = "Title's indent.")]
        public int? TitleIndent { get; set; }

        [Parameter(HelpMessage = "2nd title's text.")]
        public string Title2Text { get; set; }

        [Parameter(HelpMessage = "2nd title's font.")]
        public string Title2Font { get; set; }

        [Parameter(HelpMessage = "2nd title's alignment.")]
        public StringAlignment? Title2Alignment { get; set; }

        [Parameter(HelpMessage = "2nd title's dock style.")]
        public SankeyTitleDockStyle? Title2Dock { get; set; }

        [Parameter(HelpMessage = "2nd title's indent.")]
        public int? Title2Indent { get; set; }

        [Parameter(HelpMessage = "3rd title's text.")]
        public string Title3Text { get; set; }

        [Parameter(HelpMessage = "3rd title's font.")]
        public string Title3Font { get; set; }

        [Parameter(HelpMessage = "3rd title's alignment.")]
        public StringAlignment? Title3Alignment { get; set; }

        [Parameter(HelpMessage = "3rd title's dock style.")]
        public SankeyTitleDockStyle? Title3Dock { get; set; }

        [Parameter(HelpMessage = "3rd title's indent.")]
        public int? Title3Indent { get; set; }


        private readonly List<PSObject> _Output = new();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Output.Add(obj);
        }

        protected override void EndProcessing()
        {
            var dataSource = GetDataSource(_Output, DataSource,
                new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns });

            using var sankey = new SankeyDiagramHostControl()
            {
                Width            = this.Width,
                Height           = this.Height,
                DataSource       = dataSource,
                SourceDataMember = this.Source,
                TargetDataMember = this.Target,
                WeightDataMember = this.Weight
            };

            sankey.ErrorMessage += (s, e) =>
            {
                throw new Exception($@"{e.Title}: {e.Message}");
            };

            if (!string.IsNullOrWhiteSpace(BackColor))
            {
                var backColor = Utils.ColorFromString(BackColor);
                if (backColor != Color.Empty)
                    sankey.BackColor = backColor;
            }

            if (!string.IsNullOrWhiteSpace(NodeFont))
            {
                var font = Utils.StringToFont(NodeFont, out _);
                if (font != null)
                    sankey.NodeLabel.Font = font;
            }

            if (NodeMaxWidth.HasValue)
                sankey.NodeLabel.MaxWidth        = NodeMaxWidth.Value;
            if (NodeMaxLineCount.HasValue)
                sankey.NodeLabel.MaxLineCount    = NodeMaxLineCount.Value;
            if (NodeTextAlignment.HasValue)
                sankey.NodeLabel.TextAlignment   = NodeTextAlignment.Value;
            if (NodeTextOrientation.HasValue)
                sankey.NodeLabel.TextOrientation = NodeTextOrientation.Value;

            if (!string.IsNullOrWhiteSpace(BorderColor))
            {
                var color = Utils.ColorFromString(BorderColor);
                if (color != Color.Empty)
                    sankey.BorderOptions.Color = color;
            }
            if (BorderThickness.HasValue)
                sankey.BorderOptions.Thickness = BorderThickness.Value;

            if (NodeWidth.HasValue)
                sankey.ViewOptions.NodeWidth = NodeWidth.Value;
            if (VerticalNodeAlignment.HasValue)
                sankey.ViewOptions.VerticalNodeIndent = VerticalNodeAlignment.Value;
            if (LinkTransparency.HasValue)
                sankey.ViewOptions.LinkTransparency = LinkTransparency.Value;

            if (Padding != null && Padding.Length == 1)
                sankey.Padding.All = Padding[0];
            else if (Padding != null && Padding.Length == 4)
            {
                sankey.Padding.Left   = Padding[0];
                sankey.Padding.Top    = Padding[1];
                sankey.Padding.Right  = Padding[2];
                sankey.Padding.Bottom = Padding[3];
            }
            else if (Padding != null)
                throw new Exception("Invalid padding. Padding shall be an array with 1 or 4 integer values.");

            if (!string.IsNullOrWhiteSpace(EmptyText))
                sankey.EmptySankeyText.Text = EmptyText;
            if (!string.IsNullOrWhiteSpace(EmptyTextFont))
            {
                var font = Utils.StringToFont(EmptyTextFont, out Color color);
                if (font != null)
                    sankey.EmptySankeyText.Font = font;
                if (color != Color.Empty)
                    sankey.EmptySankeyText.TextColor = color;
            }
            if (EmptyTextAlignment.HasValue)
                sankey.EmptySankeyText.TextAlignment = EmptyTextAlignment.Value;

            if (!string.IsNullOrWhiteSpace(SmallText))
                sankey.SmallSankeyText.Text = SmallText;
            if (!string.IsNullOrWhiteSpace(SmallTextFont))
            {
                var font = Utils.StringToFont(SmallTextFont, out Color color);
                if (font != null)
                    sankey.SmallSankeyText.Font = font;
                if (color != Color.Empty)
                    sankey.SmallSankeyText.TextColor = color;
            }
            if (SmallTextAlignment.HasValue)
                sankey.SmallSankeyText.TextAlignment = SmallTextAlignment.Value;

            var sankeyPalette = GetSankeyPalette(Palette);
            sankey.Colorizer  = new SankeyPaletteColorizer() { Palette = sankeyPalette };

            if (!string.IsNullOrWhiteSpace(TitleText))
            {
                var title = new SankeyTitle() { Text = TitleText };
                if (!string.IsNullOrWhiteSpace(TitleFont))
                {
                    var font = Utils.StringToFont(TitleFont, out Color color);
                    if (font != null)
                        title.Font = font;
                    if (color != Color.Empty)
                        title.TextColor = color;
                    if (TitleAlignment.HasValue)
                        title.TextAlignment = TitleAlignment.Value;
                    if (TitleDock.HasValue)
                        title.Dock = TitleDock.Value;
                    if (TitleIndent.HasValue)
                        title.Indent = TitleIndent.Value;
                }
                sankey.Titles.Add(title);
            }
            if (!string.IsNullOrWhiteSpace(Title2Text))
            {
                var title2 = new SankeyTitle() { Text = Title2Text };
                if (!string.IsNullOrWhiteSpace(Title2Font))
                {
                    var font = Utils.StringToFont(Title2Font, out Color color);
                    if (font != null)
                        title2.Font = font;
                    if (color != Color.Empty)
                        title2.TextColor = color;
                    if (Title2Alignment.HasValue)
                        title2.TextAlignment = Title2Alignment.Value;
                    if (Title2Dock.HasValue)
                        title2.Dock = Title2Dock.Value;
                    if (Title2Indent.HasValue)
                        title2.Indent = Title2Indent.Value;
                }
                sankey.Titles.Add(title2);
            }
            if (!string.IsNullOrWhiteSpace(Title3Text))
            {
                var title3 = new SankeyTitle() { Text = Title3Text };
                if (!string.IsNullOrWhiteSpace(Title3Font))
                {
                    var font = Utils.StringToFont(Title3Font, out Color color);
                    if (font != null)
                        title3.Font = font;
                    if (color != Color.Empty)
                        title3.TextColor = color;
                    if (Title3Alignment.HasValue)
                        title3.TextAlignment = Title3Alignment.Value;
                    if (Title3Dock.HasValue)
                        title3.Dock = Title3Dock.Value;
                    if (Title3Indent.HasValue)
                        title3.Indent = Title3Indent.Value;
                }
                sankey.Titles.Add(title3);
            }

            if (SelectedNodes != null && SelectedNodes.Length > 0)
            {
                foreach (var selectedNode in SelectedNodes)
                    sankey.SelectedItems.Add(selectedNode);
            }

            if (!string.IsNullOrWhiteSpace(Selected))
            {
                var colSelected = dataSource.Columns[Selected];
                if (colSelected != null)
                {
                    var viewDataSource = dataSource.DefaultView;

                    if (colSelected.DataType == typeof(bool))
                    {
                        foreach (DataRowView row in viewDataSource)
                        {
                            var bSelected = Utils.ChangeType<bool>(row[colSelected.ColumnName], false);
                            if (bSelected)
                                sankey.SelectedItems.Add(row);
                        }
                    }
                    else if (Utils.IsTypeNumeric(colSelected.DataType))
                    {
                        foreach (DataRowView row in viewDataSource)
                        {
                            var dSelected = Utils.ChangeType<double>(row[colSelected.ColumnName], 0.0);
                            if (dSelected != 0.0)
                                sankey.SelectedItems.Add(row);
                        }
                    }
                    else if (colSelected.DataType == typeof(string) || colSelected.DataType == typeof(char))
                    {
                        foreach (DataRowView row in viewDataSource)
                        {
                            var cSelected = Utils.ChangeType<string>(row[colSelected.ColumnName], null);
                            if (string.Compare(cSelected, "true", true) == 0 || string.Compare(cSelected, "yes", true) == 0 ||
                                string.Compare(cSelected, "y", true) == 0)
                                sankey.SelectedItems.Add(row);
                        }
                    }
                }
            }


            var bmp  = PaintSankey(sankey, DPI);
            WriteSankey(bmp);

            if (PassThru)
                WriteObject(_Output, true);
        }

        protected virtual void WriteSankey(Bitmap bmp)
        {
            //Do nothing
        }

        protected virtual Bitmap PaintSankey(SankeyDiagramHostControl sankey, int? dpi = null)
        {
            if (dpi == null || dpi == 0)
                dpi = ExternalHost?.DefaultDPI ?? DefaultDPI;

            float scaleBitmap = dpi.Value / 300f;
            float scaleSVG    = 1f;

            using var streamSvg = new MemoryStream();
            sankey.ExportToSvg(streamSvg);
            streamSvg.Seek(0, SeekOrigin.Begin);

            var doc = SvgDocument.Open<SvgDocument>(streamSvg);
            doc.ShapeRendering = SvgShapeRendering.GeometricPrecision;

            streamSvg.Close();

            var bitmap = new Bitmap((int)Math.Ceiling(sankey.Width * scaleBitmap), (int)Math.Ceiling(sankey.Height * scaleBitmap));
            bitmap.SetResolution(dpi.Value, dpi.Value);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode   = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode     = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                graphics.Clear(Color.White);

                var renderer = SvgRenderer.FromGraphics(graphics);
                renderer.SmoothingMode = SmoothingMode.HighQuality;
                if (scaleSVG != 1f && scaleSVG > 0f)
                    renderer.ScaleTransform(1 / scaleSVG, 1 / scaleSVG);

                doc.Draw(renderer);
            }

            return bitmap;
        }

        protected virtual Palette GetSankeyPalette(SCSankeyColorizerPalette palette)
        {
            return palette switch
            {
                SCSankeyColorizerPalette.Default        => Palettes.Office,
                SCSankeyColorizerPalette.Office         => Palettes.Office,
                SCSankeyColorizerPalette.Office2013     => Palettes.Office2013,
                SCSankeyColorizerPalette.Opulent        => Palettes.Opulent,
                SCSankeyColorizerPalette.Orange         => Palettes.Orange,
                SCSankeyColorizerPalette.OrangeRed      => Palettes.OrangeRed,
                SCSankeyColorizerPalette.Oriel          => Palettes.Oriel,
                SCSankeyColorizerPalette.Origin         => Palettes.Origin,
                SCSankeyColorizerPalette.Paper          => Palettes.Paper,
                SCSankeyColorizerPalette.PastelKie      => Palettes.PastelKit,
                SCSankeyColorizerPalette.Red            => Palettes.Red,
                SCSankeyColorizerPalette.NorthernLights => Palettes.NorthernLights,
                SCSankeyColorizerPalette.RedOrange      => Palettes.RedOrange,
                SCSankeyColorizerPalette.Slipstream     => Palettes.Slipstream,
                SCSankeyColorizerPalette.Solstice       => Palettes.Solstice,
                SCSankeyColorizerPalette.Technic        => Palettes.Technic,
                SCSankeyColorizerPalette.TerracottaPie  => Palettes.TerracottaPie,
                SCSankeyColorizerPalette.TheTrees       => Palettes.TheTrees,
                SCSankeyColorizerPalette.Trek           => Palettes.Trek,
                SCSankeyColorizerPalette.Urban          => Palettes.Urban,
                SCSankeyColorizerPalette.Verve          => Palettes.Verve,
                SCSankeyColorizerPalette.Violet         => Palettes.Violet,
                SCSankeyColorizerPalette.VioletII       => Palettes.VioletII,
                SCSankeyColorizerPalette.RedViolet      => Palettes.RedViolet,
                SCSankeyColorizerPalette.Yellow         => Palettes.Yellow,
                SCSankeyColorizerPalette.NatureColors   => Palettes.NatureColors,
                SCSankeyColorizerPalette.Mixed          => Palettes.Mixed,
                SCSankeyColorizerPalette.Apex           => Palettes.Apex,
                SCSankeyColorizerPalette.Aspect         => Palettes.Aspect,
                SCSankeyColorizerPalette.BlackAndWhite  => Palettes.BlackAndWhite,
                SCSankeyColorizerPalette.Blue           => Palettes.Blue,
                SCSankeyColorizerPalette.BlueGreen      => Palettes.BlueGreen,
                SCSankeyColorizerPalette.BlueWarn       => Palettes.BlueWarm,
                SCSankeyColorizerPalette.Chameleon      => Palettes.Chameleon,
                SCSankeyColorizerPalette.Module         => Palettes.Module,
                SCSankeyColorizerPalette.Concourse      => Palettes.Concourse,
                SCSankeyColorizerPalette.Civic          => Palettes.Civic,
                SCSankeyColorizerPalette.Flow           => Palettes.Flow,
                SCSankeyColorizerPalette.Foundry        => Palettes.Foundry,
                SCSankeyColorizerPalette.Grayscale      => Palettes.Grayscale,
                SCSankeyColorizerPalette.Green          => Palettes.Green,
                SCSankeyColorizerPalette.GreenYellow    => Palettes.GreenYellow,
                SCSankeyColorizerPalette.InAFog         => Palettes.InAFog,
                SCSankeyColorizerPalette.Marquee        => Palettes.Marquee,
                SCSankeyColorizerPalette.Median         => Palettes.Median,
                SCSankeyColorizerPalette.Metro          => Palettes.Metro,
                SCSankeyColorizerPalette.Equity         => Palettes.Equity,
                SCSankeyColorizerPalette.YellowOrange   => Palettes.YellowOrange,
                _                                       => Palettes.Office,
            };
        }
    }
}
