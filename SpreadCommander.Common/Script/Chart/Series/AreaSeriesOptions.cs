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
    public class AreaSeriesOptions: ColorEachSeriesOptions
    {
        [Description("Border color.")]
        public string BorderColor { get; set; }

        [Description("Border thickness.")]
        public int? BorderThickness { get; set; }

        [Description("Second background color, if FillMode is gradient or hatch.")]
        public string Color2 { get; set; }

        [Description("Legend's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Transparency (0-255) to use for displaying the filled color areas.")]
        public byte? Transparency { get; set; }

        [Description("Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

        [Description("Size of the marker.")]
        public int? MarkerSize { get; set; }

        [Description("Color of data point markers.")]
        public string MarkerColor { get; set; }

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


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

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
                                var color2 = Utils.ColorFromString(Color2);
                                if (color2 != System.Drawing.Color.Empty)
                                    gradientOptions.Color2 = color2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                            }
                            break;
                        case Chart.FillMode.Hatch:
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
                    view.MarkerOptions.Kind = (DevExpress.XtraCharts.MarkerKind)MarkerKind.Value;
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
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.MarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (MarkerGradientMode.HasValue)
                                    gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)MarkerGradientMode.Value;
                                var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case Chart.FillMode.Hatch:
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
