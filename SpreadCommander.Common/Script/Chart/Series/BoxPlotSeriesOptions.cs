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
    public class BoxPlotSeriesOptions: ColorEachSeriesOptions
    {
        [Description("Border color.")]
        public string BorderColor { get; set; }

        [Description("Border thickness.")]
        public int? BorderThickness { get; set; }

        [Description("Variable distance value (as a fraction of axis units) between two points of different Box Plot series shown at the same argument point.")]
        public double? BoxDistance { get; set; }

        [Description("Fixed distance value (in pixels) between two points of different Box Plot series shown at the same argument point.")]
        public int? BoxDistanceFixed { get; set; }

        [Description("Width of bars.")]
        public double? BoxWidth { get; set; }

        [Description("Cap width of a Box Plot series point as percentage.")]
        public double? CapWidthPercentage { get; set; }

        [Description("Value that indicates whether all Box Plot points of the same series should always have the same width, or they may have different widths, if some points of other series are missing.")]
        public bool EqualBoxWidth { get; set; }

        [Description("Series 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Series filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Thickness of the line used to draw the box plot whiskers, median line and caps.")]
        public int? LineThickness { get; set; }

        [Description("Color of the Mean symbol and Median line.")]
        public string MeanAndMedianColor { get; set; }

        [Description("Mean value's marker kind.")]
        public MarkerKind? MeanMarkerKind { get; set; }

        [Description("Mean value's marker size in pixels.")]
        public int? MeanMarkerSize { get; set; }

        [Description("Marker kind of outliers.")]
        public MarkerKind? OutlierMarkerKind { get; set; }

        [Description("Marker size of outliers.")]
        public int? OutlierMarkerSize { get; set; }

        [Description("Transparency (0-255) to use for displaying the filled color areas.")]
        public byte? Transparency { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

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
                    view.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)FillMode.Value;
                    switch (FillMode.Value)
                    {
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                            }
                            break;
                        case Chart.FillMode.Hatch:
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
                    view.MeanMarkerKind = (DevExpress.XtraCharts.MarkerKind)MeanMarkerKind.Value;

                if (MeanMarkerSize.HasValue)
                    view.MeanMarkerSize = MeanMarkerSize.Value;

                if (OutlierMarkerKind.HasValue)
                    view.OutlierMarkerKind = (DevExpress.XtraCharts.MarkerKind)OutlierMarkerKind.Value;

                if (OutlierMarkerSize.HasValue)
                    view.OutlierMarkerSize = OutlierMarkerSize.Value;

                if (Transparency.HasValue)
                    view.Transparency = Transparency.Value;
            }
        }
    }
}
