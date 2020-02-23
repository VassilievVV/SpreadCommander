using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Management.Automation;
using System.Text;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class BoxPlotSeriesContext: ColorEachSeriesContext
    {
        [Parameter(HelpMessage = "Border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Border thickness.")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Variable distance value (as a fraction of axis units) between two points of different Box Plot series shown at the same argument point.")]
        public double? BoxDistance { get; set; }

        [Parameter(HelpMessage = "Fixed distance value (in pixels) between two points of different Box Plot series shown at the same argument point.")]
        public int? BoxDistanceFixed { get; set; }

        [Parameter(HelpMessage = "Width of bars.")]
        public double? BoxWidth { get; set; }

        [Parameter(HelpMessage = "Cap width of a Box Plot series point as percentage.")]
        public double? CapWidthPercentage { get; set; }

        [Parameter(HelpMessage = "Value that indicates whether all Box Plot points of the same series should always have the same width, or they may have different widths, if some points of other series are missing.")]
        public SwitchParameter EqualBoxWidth { get; set; }

        [Parameter(HelpMessage = "Series 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Parameter(HelpMessage = "Series filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Thickness of the line used to draw the box plot whiskers, median line and caps.")]
        public int? LineThickness { get; set; }

        [Parameter(HelpMessage = "Color of the Mean symbol and Median line.")]
        public string MeanAndMedianColor { get; set; }

        [Parameter(HelpMessage = "Mean value's marker kind.")]
        public MarkerKind? MeanMarkerKind { get; set; }

        [Parameter(HelpMessage = "Mean value's marker size in pixels.")]
        public int? MeanMarkerSize { get; set; }

        [Parameter(HelpMessage = "Marker kind of outliers.")]
        public MarkerKind? OutlierMarkerKind { get; set; }

        [Parameter(HelpMessage = "Marker size of outliers.")]
        public int? OutlierMarkerSize { get; set; }

        [Parameter(HelpMessage = "Transparency (0-255) to use for displaying the filled color areas.")]
        public byte? Transparency { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is BoxPlotSeriesView view)
            {
                var borderColor = Utils.ColorFromString(BorderColor);
                if (borderColor != System.Drawing.Color.Empty)
                {
                    view.Border.Color      = borderColor;
                    view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                }

                if (BorderThickness.HasValue)
                {
                    view.Border.Thickness  = BorderThickness.Value;
                    view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                }

                if (BoxDistance.HasValue)
                    view.BoxDistance = BoxDistance.Value;

                if (BoxDistanceFixed.HasValue)
                    view.BoxDistanceFixed = BoxDistanceFixed.Value;

                if (BoxWidth.HasValue)
                    view.BoxWidth = BoxWidth.Value;

                if (CapWidthPercentage.HasValue)
                    view.CapWidthPercentage = CapWidthPercentage.Value;

                view.EqualBoxWidth = EqualBoxWidth;

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
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = FillGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (view.FillStyle.Options is HatchFillOptions hatchOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    hatchOptions.Color2 = backColor2;
                                if (FillHatchStyle.HasValue)
                                    hatchOptions.HatchStyle = FillHatchStyle.Value;
                            }
                            break;
                    }
                }

                if (LineThickness.HasValue)
                    view.LineThickness = LineThickness.Value;

                var meanAndMedianColor = Utils.ColorFromString(MeanAndMedianColor);
                if (meanAndMedianColor != System.Drawing.Color.Empty)
                    view.MeanAndMedianColor = meanAndMedianColor;

                if (MeanMarkerKind.HasValue)
                    view.MeanMarkerKind = MeanMarkerKind.Value;

                if (MeanMarkerSize.HasValue)
                    view.MeanMarkerSize = MeanMarkerSize.Value;

                if (OutlierMarkerKind.HasValue)
                    view.OutlierMarkerKind = OutlierMarkerKind.Value;

                if (OutlierMarkerSize.HasValue)
                    view.OutlierMarkerSize = OutlierMarkerSize.Value;

                if (Transparency.HasValue)
                    view.Transparency = Transparency.Value;
            }
        }
    }
}
