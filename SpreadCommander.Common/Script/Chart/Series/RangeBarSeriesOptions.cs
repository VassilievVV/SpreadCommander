using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class RangeBarSeriesOptions: BarSeriesOptions
    {
        [Description("Shape of the max value marker.")]
        public MarkerKind? MaxMarkerKind { get; set; }

        [Description("Size of the max value marker.")]
        public int? MaxMarkerSize { get; set; }

        [Description("Number of points that star-shaped max value marker have.")]
        public int? MaxMarkerStarPointCount { get; set; }

        [Description("Border color of the max value marker")]
        public string MaxMarkerBorderColor { get; set; }

        [Description("Whether border of the max value marker is visible.")]
        public bool MaxHideMarkerBorder { get; set; }

        [Description("Filling mode of the max value marker.")]
        public FillMode? MaxMarkerFillMode { get; set; }

        [Description("Second color for gradient and hatch fill of the max value marker.")]
        public string MaxMarkerBackColor2 { get; set; }

        [Description("Background gradient's direction of the max value marker.")]
        public PolygonGradientMode? MaxMarkerGradientMode { get; set; }

        [Description("max value marker's hatch style")]
        public HatchStyle? MaxMarkerHatchStyle { get; set; }

        [Description("Color of max value markers.")]
        public string MaxMarkerColor { get; set; }

        [Description("Whether the series max value markers are visible.")]
        public bool? MaxMarkerVisibility { get; set; }


        [Description("Shape of the min value marker.")]
        public MarkerKind? MinMarkerKind { get; set; }

        [Description("Size of the min value marker.")]
        public int? MinMarkerSize { get; set; }

        [Description("Number of points that star-shaped min value marker have.")]
        public int? MinMarkerStarPointCount { get; set; }

        [Description("Border color of the min value marker")]
        public string MinMarkerBorderColor { get; set; }

        [Description("Whether border of the min value marker is visible.")]
        public bool MinHideMarkerBorder { get; set; }

        [Description("Filling mode of the min value marker.")]
        public FillMode? MinMarkerFillMode { get; set; }

        [Description("Second color for gradient and hatch fill of the min value marker.")]
        public string MinMarkerBackColor2 { get; set; }

        [Description("Background gradient's direction of the min value marker.")]
        public PolygonGradientMode? MinMarkerGradientMode { get; set; }

        [Description("min value marker's hatch style")]
        public HatchStyle? MinMarkerHatchStyle { get; set; }

        [Description("Color of min value  markers.")]
        public string MinMarkerColor { get; set; }

        [Description("Whether the series min value markers are visible.")]
        public bool? MinMarkerVisibility { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is RangeBarSeriesView view)
            {
                if (MaxMarkerKind.HasValue)
                    view.MaxValueMarker.Kind = (DevExpress.XtraCharts.MarkerKind)MaxMarkerKind.Value;
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
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.MaxValueMarker.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MaxMarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)MaxMarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MaxMarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case Chart.FillMode.Hatch:
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
                    view.MinValueMarker.Kind = (DevExpress.XtraCharts.MarkerKind)MinMarkerKind.Value;
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
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.MinValueMarker.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MinMarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)MinMarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MinMarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case Chart.FillMode.Hatch:
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
