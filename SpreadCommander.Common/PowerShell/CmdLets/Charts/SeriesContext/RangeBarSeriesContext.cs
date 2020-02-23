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
    public class RangeBarSeriesContext: BarSeriesContext
    {
        [Parameter(HelpMessage = "Shape of the max value marker.")]
        public MarkerKind? MaxMarkerKind { get; set; }

        [Parameter(HelpMessage = "Size of the max value marker.")]
        public int? MaxMarkerSize { get; set; }

        [Parameter(HelpMessage = "Number of points that star-shaped max value marker have.")]
        public int? MaxMarkerStarPointCount { get; set; }

        [Parameter(HelpMessage = "Border color of the max value marker")]
        public string MaxMarkerBorderColor { get; set; }

        [Parameter(HelpMessage = "Whether border of the max value marker is visible.")]
        public SwitchParameter MaxHideMarkerBorder { get; set; }

        [Parameter(HelpMessage = "Filling mode of the max value marker.")]
        public DevExpress.XtraCharts.FillMode? MaxMarkerFillMode { get; set; }

        [Parameter(HelpMessage = "Second color for gradient and hatch fill of the max value marker.")]
        public string MaxMarkerBackColor2 { get; set; }

        [Parameter(HelpMessage = "Background gradient's direction of the max value marker.")]
        public PolygonGradientMode? MaxMarkerGradientMode { get; set; }

        [Parameter(HelpMessage = "max value marker's hatch style")]
        public HatchStyle? MaxMarkerHatchStyle { get; set; }

        [Parameter(HelpMessage = "Color of max value markers.")]
        public string MaxMarkerColor { get; set; }

        [Parameter(HelpMessage = "Whether the series max value markers are visible.")]
        public bool? MaxMarkerVisibility { get; set; }


        [Parameter(HelpMessage = "Shape of the min value marker.")]
        public MarkerKind? MinMarkerKind { get; set; }

        [Parameter(HelpMessage = "Size of the min value marker.")]
        public int? MinMarkerSize { get; set; }

        [Parameter(HelpMessage = "Number of points that star-shaped min value marker have.")]
        public int? MinMarkerStarPointCount { get; set; }

        [Parameter(HelpMessage = "Border color of the min value marker")]
        public string MinMarkerBorderColor { get; set; }

        [Parameter(HelpMessage = "Whether border of the min value marker is visible.")]
        public SwitchParameter MinHideMarkerBorder { get; set; }

        [Parameter(HelpMessage = "Filling mode of the min value marker.")]
        public DevExpress.XtraCharts.FillMode? MinMarkerFillMode { get; set; }

        [Parameter(HelpMessage = "Second color for gradient and hatch fill of the min value marker.")]
        public string MinMarkerBackColor2 { get; set; }

        [Parameter(HelpMessage = "Background gradient's direction of the min value marker.")]
        public PolygonGradientMode? MinMarkerGradientMode { get; set; }

        [Parameter(HelpMessage = "min value marker's hatch style")]
        public HatchStyle? MinMarkerHatchStyle { get; set; }

        [Parameter(HelpMessage = "Color of min value  markers.")]
        public string MinMarkerColor { get; set; }

        [Parameter(HelpMessage = "Whether the series min value markers are visible.")]
        public bool? MinMarkerVisibility { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is RangeBarSeriesView view)
            {
                if (MaxMarkerKind.HasValue)
                    view.MaxValueMarker.Kind = MaxMarkerKind.Value;
                if (MaxMarkerSize.HasValue)
                    view.MaxValueMarker.Size = MaxMarkerSize.Value;
                if (MaxMarkerStarPointCount.HasValue)
                    view.MaxValueMarker.StarPointCount = MaxMarkerStarPointCount.Value;
                if (!string.IsNullOrWhiteSpace(MaxMarkerBorderColor))
                {
                    var markerBorderColor = Utils.ColorFromString(MaxMarkerBorderColor);
                    if (markerBorderColor != System.Drawing.Color.Empty)
                    {
                        view.MaxValueMarker.BorderVisible = true;
                        view.MaxValueMarker.BorderColor = markerBorderColor;
                    }
                }

                var maxMarkerColor = Utils.ColorFromString(MaxMarkerColor);
                if (maxMarkerColor != System.Drawing.Color.Empty)
                    view.MaxValueMarker.Color = maxMarkerColor;

                if (MaxMarkerVisibility.HasValue)
                    view.MaxValueMarkerVisibility = MaxMarkerVisibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

                if (MaxHideMarkerBorder)
                    view.MaxValueMarker.BorderVisible = false;
                if (MaxMarkerFillMode.HasValue)
                {
                    switch (MaxMarkerFillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.MaxValueMarker.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MaxMarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = MaxMarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MaxMarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (view.MaxValueMarker.FillStyle.Options is HatchFillOptions hatchFillOptions)
                            {
                                if (MaxMarkerHatchStyle.HasValue)
                                    hatchFillOptions.HatchStyle = MaxMarkerHatchStyle.Value;
                                var backColor2 = Utils.ColorFromString(MaxMarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    hatchFillOptions.Color2 = backColor2;
                            }
                            break;
                    }
                }


                if (MinMarkerKind.HasValue)
                    view.MinValueMarker.Kind = MinMarkerKind.Value;
                if (MinMarkerSize.HasValue)
                    view.MinValueMarker.Size = MinMarkerSize.Value;
                if (MinMarkerStarPointCount.HasValue)
                    view.MinValueMarker.StarPointCount = MinMarkerStarPointCount.Value;
                if (!string.IsNullOrWhiteSpace(MinMarkerBorderColor))
                {
                    var markerBorderColor = Utils.ColorFromString(MinMarkerBorderColor);
                    if (markerBorderColor != System.Drawing.Color.Empty)
                    {
                        view.MinValueMarker.BorderVisible = true;
                        view.MinValueMarker.BorderColor = markerBorderColor;
                    }
                }

                var minMarkerColor = Utils.ColorFromString(MinMarkerColor);
                if (minMarkerColor != System.Drawing.Color.Empty)
                    view.MinValueMarker.Color = minMarkerColor;

                if (MinMarkerVisibility.HasValue)
                    view.MinValueMarkerVisibility = MinMarkerVisibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

                if (MinHideMarkerBorder)
                    view.MinValueMarker.BorderVisible = false;
                if (MinMarkerFillMode.HasValue)
                {
                    switch (MinMarkerFillMode)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.MinValueMarker.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MinMarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = MinMarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MinMarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (view.MinValueMarker.FillStyle.Options is HatchFillOptions hatchFillOptions)
                            {
                                if (MinMarkerHatchStyle.HasValue)
                                    hatchFillOptions.HatchStyle = MinMarkerHatchStyle.Value;
                                var backColor2 = Utils.ColorFromString(MinMarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    hatchFillOptions.Color2 = backColor2;
                            }
                            break;
                    }
                }
            }
        }
    }
}
