using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Sankey;
using SpreadCommander.Common.Code;
using Svg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Sankey
{
    public partial class SCSankey : ScriptHostObject, IDisposable
    {
        private readonly object _DataSource;
        private readonly string _SourceColumn;
        private readonly string _TargetColumn;
        private readonly SankeyOptions _Options;
        
        protected SankeyDiagramHostControl _HostControl;

        public SCSankey(object dataSource, string sourceColumn, string targetColumn,
            SankeyOptions options = null)
        {
            options ??= new SankeyOptions();

            _DataSource   = dataSource;
            _SourceColumn = sourceColumn;
            _TargetColumn = targetColumn;
            _Options      = options;
        }

        public void Dispose()
        {
            _HostControl?.Dispose();
        }

        protected virtual void InitializeSankey()
        {
            var dataSource = GetDataSource(_DataSource,
                new DataSourceParameters() { IgnoreErrors = _Options.IgnoreErrors, Columns = _Options.SelectColumns, SkipColumns = _Options.SkipColumns });

            var sankey = new SankeyDiagramHostControl()
            {
                Width            = _Options.Width,
                Height           = _Options.Height,
                DataSource       = dataSource,
                SourceDataMember = _SourceColumn,
                TargetDataMember = _TargetColumn,
                WeightDataMember = _Options.Weight
            };
            _HostControl = sankey;

            sankey.ErrorMessage += (s, e) =>
            {
                throw new Exception($@"{e.Title}: {e.Message}");
            };

            if (!string.IsNullOrWhiteSpace(_Options.BackColor))
            {
                var backColor = Utils.ColorFromString(_Options.BackColor);
                if (backColor != Color.Empty)
                    sankey.BackColor = backColor;
            }

            if (!string.IsNullOrWhiteSpace(_Options.NodeFont))
            {
                var font = Utils.StringToFont(_Options.NodeFont, out _);
                if (font != null)
                    sankey.NodeLabel.Font = font;
            }

            if (_Options.NodeMaxWidth.HasValue)
                sankey.NodeLabel.MaxWidth = _Options.NodeMaxWidth.Value;
            if (_Options.NodeMaxLineCount.HasValue)
                sankey.NodeLabel.MaxLineCount = _Options.NodeMaxLineCount.Value;
            if (_Options.NodeTextAlignment.HasValue)
                sankey.NodeLabel.TextAlignment = _Options.NodeTextAlignment.Value;
            if (_Options.NodeTextOrientation.HasValue)
                sankey.NodeLabel.TextOrientation = (DevExpress.XtraCharts.TextOrientation)_Options.NodeTextOrientation.Value;

            if (!string.IsNullOrWhiteSpace(_Options.BorderColor))
            {
                var color = Utils.ColorFromString(_Options.BorderColor);
                if (color != Color.Empty)
                    sankey.BorderOptions.Color = color;
            }
            if (_Options.BorderThickness.HasValue)
                sankey.BorderOptions.Thickness = _Options.BorderThickness.Value;

            if (_Options.NodeWidth.HasValue)
                sankey.ViewOptions.NodeWidth = _Options.NodeWidth.Value;
            if (_Options.VerticalNodeAlignment.HasValue)
                sankey.ViewOptions.VerticalNodeIndent = _Options.VerticalNodeAlignment.Value;
            if (_Options.LinkTransparency.HasValue)
                sankey.ViewOptions.LinkTransparency = _Options.LinkTransparency.Value;

            if (_Options.Padding != null && _Options.Padding.Length == 1)
                sankey.Padding.All = _Options.Padding[0];
            else if (_Options.Padding != null && _Options.Padding.Length == 4)
            {
                sankey.Padding.Left   = _Options.Padding[0];
                sankey.Padding.Top    = _Options.Padding[1];
                sankey.Padding.Right  = _Options.Padding[2];
                sankey.Padding.Bottom = _Options.Padding[3];
            }
            else if (_Options.Padding != null)
                throw new Exception("Invalid padding. Padding shall be an array with 1 or 4 integer values.");

            if (!string.IsNullOrWhiteSpace(_Options.EmptyText))
                sankey.EmptySankeyText.Text = _Options.EmptyText;
            if (!string.IsNullOrWhiteSpace(_Options.EmptyTextFont))
            {
                var font = Utils.StringToFont(_Options.EmptyTextFont, out Color color);
                if (font != null)
                    sankey.EmptySankeyText.Font = font;
                if (color != Color.Empty)
                    sankey.EmptySankeyText.TextColor = color;
            }
            if (_Options.EmptyTextAlignment.HasValue)
                sankey.EmptySankeyText.TextAlignment = _Options.EmptyTextAlignment.Value;

            if (!string.IsNullOrWhiteSpace(_Options.SmallText))
                sankey.SmallSankeyText.Text = _Options.SmallText;
            if (!string.IsNullOrWhiteSpace(_Options.SmallTextFont))
            {
                var font = Utils.StringToFont(_Options.SmallTextFont, out Color color);
                if (font != null)
                    sankey.SmallSankeyText.Font = font;
                if (color != Color.Empty)
                    sankey.SmallSankeyText.TextColor = color;
            }
            if (_Options.SmallTextAlignment.HasValue)
                sankey.SmallSankeyText.TextAlignment = _Options.SmallTextAlignment.Value;

            var sankeyPalette = GetSankeyPalette(_Options.Palette);
            sankey.Colorizer = new SankeyPaletteColorizer() { Palette = sankeyPalette };

            if (!string.IsNullOrWhiteSpace(_Options.TitleText))
            {
                var title = new SankeyTitle() { Text = _Options.TitleText };
                if (!string.IsNullOrWhiteSpace(_Options.TitleFont))
                {
                    var font = Utils.StringToFont(_Options.TitleFont, out Color color);
                    if (font != null)
                        title.Font = font;
                    if (color != Color.Empty)
                        title.TextColor = color;
                    if (_Options.TitleAlignment.HasValue)
                        title.TextAlignment = _Options.TitleAlignment.Value;
                    if (_Options.TitleDock.HasValue)
                        title.Dock = (DevExpress.XtraCharts.Sankey.SankeyTitleDockStyle)_Options.TitleDock.Value;
                    if (_Options.TitleIndent.HasValue)
                        title.Indent = _Options.TitleIndent.Value;
                }
                sankey.Titles.Add(title);
            }
            if (!string.IsNullOrWhiteSpace(_Options.Title2Text))
            {
                var title2 = new SankeyTitle() { Text = _Options.Title2Text };
                if (!string.IsNullOrWhiteSpace(_Options.Title2Font))
                {
                    var font = Utils.StringToFont(_Options.Title2Font, out Color color);
                    if (font != null)
                        title2.Font = font;
                    if (color != Color.Empty)
                        title2.TextColor = color;
                    if (_Options.Title2Alignment.HasValue)
                        title2.TextAlignment = _Options.Title2Alignment.Value;
                    if (_Options.Title2Dock.HasValue)
                        title2.Dock = (DevExpress.XtraCharts.Sankey.SankeyTitleDockStyle)_Options.Title2Dock.Value;
                    if (_Options.Title2Indent.HasValue)
                        title2.Indent = _Options.Title2Indent.Value;
                }
                sankey.Titles.Add(title2);
            }
            if (!string.IsNullOrWhiteSpace(_Options.Title3Text))
            {
                var title3 = new SankeyTitle() { Text = _Options.Title3Text };
                if (!string.IsNullOrWhiteSpace(_Options.Title3Font))
                {
                    var font = Utils.StringToFont(_Options.Title3Font, out Color color);
                    if (font != null)
                        title3.Font = font;
                    if (color != Color.Empty)
                        title3.TextColor = color;
                    if (_Options.Title3Alignment.HasValue)
                        title3.TextAlignment = _Options.Title3Alignment.Value;
                    if (_Options.Title3Dock.HasValue)
                        title3.Dock = (DevExpress.XtraCharts.Sankey.SankeyTitleDockStyle)_Options.Title3Dock.Value;
                    if (_Options.Title3Indent.HasValue)
                        title3.Indent = _Options.Title3Indent.Value;
                }
                sankey.Titles.Add(title3);
            }

            if (_Options.SelectedNodes != null && _Options.SelectedNodes.Length > 0)
            {
                foreach (var selectedNode in _Options.SelectedNodes)
                    sankey.SelectedItems.Add(selectedNode);
            }

            if (!string.IsNullOrWhiteSpace(_Options.Selected))
            {
                var colSelected = dataSource.Columns[_Options.Selected];
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
        }

        protected virtual Bitmap PaintSankey(SankeyDiagramHostControl sankey, int? dpi = null)
        {
            if (dpi == null || dpi == 0)
                dpi = DefaultDPI;

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

        protected virtual Palette GetSankeyPalette(SankeyColorizerPalette palette)
        {
            return palette switch
            {
                SankeyColorizerPalette.Default        => Palettes.Office,
                SankeyColorizerPalette.Office         => Palettes.Office,
                SankeyColorizerPalette.Office2013     => Palettes.Office2013,
                SankeyColorizerPalette.Opulent        => Palettes.Opulent,
                SankeyColorizerPalette.Orange         => Palettes.Orange,
                SankeyColorizerPalette.OrangeRed      => Palettes.OrangeRed,
                SankeyColorizerPalette.Oriel          => Palettes.Oriel,
                SankeyColorizerPalette.Origin         => Palettes.Origin,
                SankeyColorizerPalette.Paper          => Palettes.Paper,
                SankeyColorizerPalette.PastelKie      => Palettes.PastelKit,
                SankeyColorizerPalette.Red            => Palettes.Red,
                SankeyColorizerPalette.NorthernLights => Palettes.NorthernLights,
                SankeyColorizerPalette.RedOrange      => Palettes.RedOrange,
                SankeyColorizerPalette.Slipstream     => Palettes.Slipstream,
                SankeyColorizerPalette.Solstice       => Palettes.Solstice,
                SankeyColorizerPalette.Technic        => Palettes.Technic,
                SankeyColorizerPalette.TerracottaPie  => Palettes.TerracottaPie,
                SankeyColorizerPalette.TheTrees       => Palettes.TheTrees,
                SankeyColorizerPalette.Trek           => Palettes.Trek,
                SankeyColorizerPalette.Urban          => Palettes.Urban,
                SankeyColorizerPalette.Verve          => Palettes.Verve,
                SankeyColorizerPalette.Violet         => Palettes.Violet,
                SankeyColorizerPalette.VioletII       => Palettes.VioletII,
                SankeyColorizerPalette.RedViolet      => Palettes.RedViolet,
                SankeyColorizerPalette.Yellow         => Palettes.Yellow,
                SankeyColorizerPalette.NatureColors   => Palettes.NatureColors,
                SankeyColorizerPalette.Mixed          => Palettes.Mixed,
                SankeyColorizerPalette.Apex           => Palettes.Apex,
                SankeyColorizerPalette.Aspect         => Palettes.Aspect,
                SankeyColorizerPalette.BlackAndWhite  => Palettes.BlackAndWhite,
                SankeyColorizerPalette.Blue           => Palettes.Blue,
                SankeyColorizerPalette.BlueGreen      => Palettes.BlueGreen,
                SankeyColorizerPalette.BlueWarn       => Palettes.BlueWarm,
                SankeyColorizerPalette.Chameleon      => Palettes.Chameleon,
                SankeyColorizerPalette.Module         => Palettes.Module,
                SankeyColorizerPalette.Concourse      => Palettes.Concourse,
                SankeyColorizerPalette.Civic          => Palettes.Civic,
                SankeyColorizerPalette.Flow           => Palettes.Flow,
                SankeyColorizerPalette.Foundry        => Palettes.Foundry,
                SankeyColorizerPalette.Grayscale      => Palettes.Grayscale,
                SankeyColorizerPalette.Green          => Palettes.Green,
                SankeyColorizerPalette.GreenYellow    => Palettes.GreenYellow,
                SankeyColorizerPalette.InAFog         => Palettes.InAFog,
                SankeyColorizerPalette.Marquee        => Palettes.Marquee,
                SankeyColorizerPalette.Median         => Palettes.Median,
                SankeyColorizerPalette.Metro          => Palettes.Metro,
                SankeyColorizerPalette.Equity         => Palettes.Equity,
                SankeyColorizerPalette.YellowOrange   => Palettes.YellowOrange,
                _                                     => Palettes.Office,
            };
        }

        protected void ScrollToCaret()
        {
            Host?.Engine?.ScrollToCaret();
        }
    }
}
