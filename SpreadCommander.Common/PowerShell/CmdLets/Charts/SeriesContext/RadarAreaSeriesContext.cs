using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class RadarAreaSeriesContext : BaseSeriesContext
    {
        [Parameter(HelpMessage = "Aggregate function used for a series.")]
        [PSDefaultValue(Value = SCSeriesAggregateFunction.Default)]
        [DefaultValue(SCSeriesAggregateFunction.Default)]
        public SCSeriesAggregateFunction AggregateFunction { get; set; } = SCSeriesAggregateFunction.Default;

        [Parameter(HelpMessage = "Border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Border thickness.")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Color of the series.")]
        public string Color { get; set; }

        [Parameter(HelpMessage = "2nd color of the series used in gradient and hatch filling styles.")]
        public string Color2 { get; set; }

        [Parameter(HelpMessage = "Whether each data point of a series is shown in a different color.")]
        public SwitchParameter ColorEach { get; set; }

        [Parameter(HelpMessage = "Series's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public PolygonGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

        [Parameter(HelpMessage = "Size of the marker.")]
        public int? MarkerSize { get; set; }

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

        [Parameter(HelpMessage = "Color of data markers.")]
        public string MarkerColor { get; set; }

        [Parameter(HelpMessage = "Whether the series markers are visible.")]
        public bool? MarkerVisibility { get; set; }

        [Parameter(HelpMessage = "Series shadow's color.")]
        public string ShadowColor { get; set; }

        [Parameter(HelpMessage = "Series shadow's thickness.")]
        public int? ShadowSize { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is RadarAreaSeriesView view)
            {
                if (AggregateFunction != SCSeriesAggregateFunction.Default)
                    view.AggregateFunction = (SeriesAggregateFunction)(int)AggregateFunction;

                var borderColor = Utils.ColorFromString(BorderColor);
                if (borderColor != System.Drawing.Color.Empty)
                {
                    view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    view.Border.Color      = borderColor;
                }
                if (BorderThickness.HasValue)
                {
                    view.Border.Thickness  = BorderThickness.Value;
                    view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                }

                var color = Utils.ColorFromString(Color);
                if (color != System.Drawing.Color.Empty)
                    view.Color = color;

                view.ColorEach = ColorEach;

                if (FillMode.HasValue)
                {
                    switch (FillMode)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (FillGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = FillGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(Color2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (view.FillStyle.Options is HatchFillOptions hatchFillOptions)
                            {
                                if (FillHatchStyle.HasValue)
                                    hatchFillOptions.HatchStyle = FillHatchStyle.Value;
                                var backColor2 = Utils.ColorFromString(Color2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    hatchFillOptions.Color2 = backColor2;
                            }
                            break;
                    }
                }

                if (MarkerKind.HasValue)
                    view.MarkerOptions.Kind = MarkerKind.Value;
                if (MarkerSize.HasValue)
                    view.MarkerOptions.Size = MarkerSize.Value;
                if (MarkerStarPointCount.HasValue)
                    view.MarkerOptions.StarPointCount = MarkerStarPointCount.Value;
                if (!string.IsNullOrWhiteSpace(MarkerBorderColor))
                {
                    var markerBorderColor = Utils.ColorFromString(MarkerBorderColor);
                    if (markerBorderColor != System.Drawing.Color.Empty)
                    {
                        view.MarkerOptions.BorderVisible = true;
                        view.MarkerOptions.BorderColor   = markerBorderColor;
                    }
                }

                var markerColor = Utils.ColorFromString(MarkerColor);
                if (markerColor != System.Drawing.Color.Empty)
                    view.MarkerOptions.Color = markerColor;

                if (MarkerVisibility.HasValue)
                    view.MarkerVisibility = MarkerVisibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

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

                if (!string.IsNullOrWhiteSpace(ShadowColor))
                {
                    var shadowColor = Utils.ColorFromString(ShadowColor);
                    if (shadowColor != System.Drawing.Color.Empty)
                    {
                        view.Shadow.Visible = true;
                        view.Shadow.Color   = shadowColor;
                    }
                }
                if (ShadowSize.HasValue)
                {
                    view.Shadow.Size    = ShadowSize.Value;
                    view.Shadow.Visible = true;
                }
            }
        }
    }
}
