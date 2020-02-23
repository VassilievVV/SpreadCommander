using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class AreaSeriesContext: ColorEachSeriesContext
    {
        [Parameter(HelpMessage = "Border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Border thickness.")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Second background color, if FillMode is gradient or hatch.")]
        public string Color2 { get; set; }

        [Parameter(HelpMessage = "Legend's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Transparency (0-255) to use for displaying the filled color areas.")]
        public byte? Transparency { get; set; }

        [Parameter(HelpMessage = "Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

        [Parameter(HelpMessage = "Size of the marker.")]
        public int? MarkerSize { get; set; }

        [Parameter(HelpMessage = "Color of data point markers.")]
        public string MarkerColor { get; set; }

        [Parameter(HelpMessage = "Number of points that star-shaped marker have.")]
        public int? MarkerStarPointCount { get; set; }

        [Parameter(HelpMessage = "Border color of the marker")]
        public string MarkerBorderColor { get; set; }

        [Parameter(HelpMessage = "Whether border of the marker is visible.")]
        public SwitchParameter HideMarkerBorder { get; set; }

        [Parameter(HelpMessage = "Filling mode of the marker.")]
        public DevExpress.XtraCharts.FillMode? MarkerFillMode { get; set; }

        [Parameter(HelpMessage = "Second color for gradient and hatch fill of the marker.")]
        public string MarkerBackColor2 { get; set; }

        [Parameter(HelpMessage = "Background gradient's direction of the marker.")]
        public PolygonGradientMode? MarkerGradientMode { get; set; }

        [Parameter(HelpMessage = "Marker's hatch style")]
        public HatchStyle? MarkerHatchStyle { get; set; }

        [Parameter(HelpMessage = "Whether the series markers are visible.")]
        public bool? MarkerVisibility { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is AreaSeriesViewBase view)
            {
                view.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

                var borderColor = Utils.ColorFromString(BorderColor);
                if (borderColor != System.Drawing.Color.Empty)
                {
                    view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    view.Border.Color = borderColor;
                }
                if (BorderThickness.HasValue)
                {
                    view.Border.Thickness  = BorderThickness.Value;
                    view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                }

                if (FillMode.HasValue)
                {
                    view.FillStyle.FillMode = FillMode.Value;
                    switch (FillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var color2 = Utils.ColorFromString(Color2);
                                if (color2 != System.Drawing.Color.Empty)
                                    gradientOptions.Color2 = color2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = FillGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (view.FillStyle.Options is HatchFillOptions hatchOptions)
                            {
                                var color2 = Utils.ColorFromString(Color2);
                                if (color2 != System.Drawing.Color.Empty)
                                    hatchOptions.Color2 = color2;
                                if (FillHatchStyle.HasValue)
                                    hatchOptions.HatchStyle = FillHatchStyle.Value;
                            }
                            break;
                    }
                }

                if (Transparency.HasValue)
                    view.Transparency = Transparency.Value;


                if (MarkerKind.HasValue)
                    view.MarkerOptions.Kind = MarkerKind.Value;
                var markerColor = Utils.ColorFromString(MarkerColor);
                if (markerColor != System.Drawing.Color.Empty)
                    view.MarkerOptions.Color = markerColor;
                if (MarkerSize.HasValue)
                    view.MarkerOptions.Size = MarkerSize.Value;
                if (MarkerStarPointCount.HasValue)
                    view.MarkerOptions.StarPointCount = MarkerStarPointCount.Value;
                var markerBorderColor = Utils.ColorFromString(MarkerBorderColor);
                if (markerBorderColor != System.Drawing.Color.Empty)
                    view.MarkerOptions.BorderColor = markerBorderColor;
                if (HideMarkerBorder)
                    view.MarkerOptions.BorderVisible = false;

                if (MarkerFillMode.HasValue)
                {
                    switch (MarkerFillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.MarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = MarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (view.MarkerOptions.FillStyle.Options is HatchFillOptions hatchFillOptions)
                            {
                                if (MarkerHatchStyle.HasValue)
                                    hatchFillOptions.HatchStyle = MarkerHatchStyle.Value;
                                var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    hatchFillOptions.Color2 = backColor2;
                            }
                            break;
                    }
                }

                if (MarkerVisibility.HasValue)
                    view.MarkerVisibility = MarkerVisibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            }
        }
    }
}
