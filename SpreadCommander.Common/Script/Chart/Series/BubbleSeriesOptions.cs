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
    public class BubbleSeriesOptions: ColorEachSeriesOptions
    {
        [Description("Minimum size of the marker of the Bubble series view.")]
        public double? MinSize { get; set; }

        [Description("Maximum size of the marker of the Bubble series view.")]
        public double? MaxSize { get; set; }

        [Description("Size unit of Bubble series view.")]
        public BubbleSizeUnit? SizeUnit { get; set; }

        [Description("Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

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


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is BubbleSeriesView view)
            {
                if (SizeUnit.HasValue)
                    view.SizeUnit = (DevExpress.XtraCharts.BubbleSizeUnit)SizeUnit.Value;
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
                    view.BubbleMarkerOptions.Kind = (DevExpress.XtraCharts.MarkerKind)MarkerKind.Value;
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
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.BubbleMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)MarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case Chart.FillMode.Hatch:
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
