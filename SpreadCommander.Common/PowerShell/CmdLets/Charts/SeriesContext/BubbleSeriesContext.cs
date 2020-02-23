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
    public class BubbleSeriesContext: ColorEachSeriesContext
    {
        [Parameter(HelpMessage = "Minimum size of the marker of the Bubble series view.")]
        public double? MinSize { get; set; }

        [Parameter(HelpMessage = "Maximum size of the marker of the Bubble series view.")]
        public double? MaxSize { get; set; }

        [Parameter(HelpMessage = "Size unit of Bubble series view.")]
        public BubbleSizeUnit? SizeUnit { get; set; }

        [Parameter(HelpMessage = "Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

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


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is BubbleSeriesView view)
            {
                if (SizeUnit.HasValue)
                    view.SizeUnit = SizeUnit.Value;
                if (MinSize.HasValue)
                {
                    view.MinSize  = MinSize.Value;
                    view.AutoSize = false;
                }
                if (MaxSize.HasValue)
                {
                    view.MaxSize  = MaxSize.Value;
                    view.AutoSize = false;
                }

                if (MarkerKind.HasValue)
                    view.BubbleMarkerOptions.Kind = MarkerKind.Value;
                if (MarkerStarPointCount.HasValue)
                    view.BubbleMarkerOptions.StarPointCount = MarkerStarPointCount.Value;
                var markerBorderColor = Utils.ColorFromString(MarkerBorderColor);
                if (markerBorderColor != System.Drawing.Color.Empty)
                    view.BubbleMarkerOptions.BorderColor = markerBorderColor;
                if (HideMarkerBorder)
                    view.BubbleMarkerOptions.BorderVisible = false;

                if (MarkerFillMode.HasValue)
                {
                    switch (MarkerFillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.BubbleMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = MarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (view.BubbleMarkerOptions.FillStyle.Options is HatchFillOptions hatchFillOptions)
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
            }
        }
    }
}
