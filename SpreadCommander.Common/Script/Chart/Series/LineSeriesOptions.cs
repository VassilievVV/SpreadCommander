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
    public class LineSeriesOptions: ColorEachSeriesOptions
    {
        [Description("Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

        [Description("Size of the marker.")]
        public int? MarkerSize { get; set; }

        [Description("Number of points that star-shaped marker have.")]
        public int? MarkerStarPointCount { get; set; }

        [Description("Border color of the marker")]
        public string MarkerBorderColor { get; set; }

        [Description("Whether border of the marker is visible.")]
        public bool HideMarkerBorder { get; set; }

        [Description("Filling mode of the marker.")]
        public FillMode? MarkerFillMode { get; set; }

        [Description("Second color for gradient and hatch fill of the marker.")]
        public string MarkerBackColor2 { get; set; }

        [Description("Background gradient's direction of the marker.")]
        public PolygonGradientMode? MarkerGradientMode { get; set; }

        [Description("Marker's hatch style")]
        public HatchStyle? MarkerHatchStyle { get; set; }

        [Description("Whether the series markers are visible.")]
        public bool? MarkerVisibility { get; set; }

        [Description("Dash style used to paint the line.")]
        public DashStyle? LineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Description("Line's thickness.")]
        public int? LineThickness { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is LineSeriesView view)
            {
                view.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

                if (MarkerKind.HasValue)
                    view.LineMarkerOptions.Kind = (DevExpress.XtraCharts.MarkerKind)MarkerKind.Value;
                if (MarkerSize.HasValue)
                    view.LineMarkerOptions.Size = MarkerSize.Value;
                if (MarkerStarPointCount.HasValue)
                    view.LineMarkerOptions.StarPointCount = MarkerStarPointCount.Value;
                var markerBorderColor = Utils.ColorFromString(MarkerBorderColor);
                if (markerBorderColor != System.Drawing.Color.Empty)
                    view.LineMarkerOptions.BorderColor = markerBorderColor;
                if (HideMarkerBorder)
                    view.LineMarkerOptions.BorderVisible = false;

                if (MarkerFillMode.HasValue)
                {
                    switch (MarkerFillMode.Value)
                    {
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.LineMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)MarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case Chart.FillMode.Hatch:
                            if (view.LineMarkerOptions.FillStyle.Options is HatchFillOptions hatchFillOptions)
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

                if (LineDashStyle.HasValue)
                    view.LineStyle.DashStyle = (DevExpress.XtraCharts.DashStyle)LineDashStyle.Value;
                if (LineJoin.HasValue)
                    view.LineStyle.LineJoin = LineJoin.Value;
                if (LineThickness.HasValue)
                    view.LineStyle.Thickness = LineThickness.Value;
            }
        }
    }
}
