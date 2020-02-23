using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class LineSeriesContext: ColorEachSeriesContext
    {
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

        [Parameter(HelpMessage = "Whether the series markers are visible.")]
        public bool? MarkerVisibility { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? LineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? LineThickness { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is LineSeriesView view)
            {
                view.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

                if (MarkerKind.HasValue)
                    view.LineMarkerOptions.Kind = MarkerKind.Value;
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
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.LineMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = MarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
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
                    view.LineStyle.DashStyle = LineDashStyle.Value;
                if (LineJoin.HasValue)
                    view.LineStyle.LineJoin = LineJoin.Value;
                if (LineThickness.HasValue)
                    view.LineStyle.Thickness = LineThickness.Value;
            }
        }
    }
}
