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
    public class RangeAreaSeriesOptions: ColorEachSeriesOptions
    {
        [Description("Border color.")]
        public string Border1Color { get; set; }

        [Description("Border thickness.")]
        public int? Border1Thickness { get; set; }

        [Description("Border color.")]
        public string Border2Color { get; set; }

        [Description("Border thickness.")]
        public int? Border2Thickness { get; set; }

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
        public MarkerKind? Marker1Kind { get; set; }

        [Description("Size of the marker.")]
        public int? Marker1Size { get; set; }

        [Description("Color of data point markers.")]
        public string Marker1Color { get; set; }

        [Description("Number of points that star-shaped marker have.")]
        public int? Marker1StarPointCount { get; set; }

        [Description("Border color of the marker")]
        public string Marker1BorderColor { get; set; }

        [Description("Whether border of the marker is visible.")]
        public bool HideMarker1Border { get; set; }

        [Description("Filling mode of the marker.")]
        public FillMode? Marker1FillMode { get; set; }

        [Description("Second color for gradient and hatch fill of the marker.")]
        public string Marker1BackColor2 { get; set; }

        [Description("Background gradient's direction of the marker.")]
        public PolygonGradientMode? Marker1GradientMode { get; set; }

        [Description("Marker's hatch style")]
        public HatchStyle? Marker1HatchStyle { get; set; }

        [Description("Whether the series markers are visible.")]
        public bool? Marker1Visibility { get; set; }

        [Description("Shape of the marker.")]
        public MarkerKind? Marker2Kind { get; set; }

        [Description("Size of the marker.")]
        public int? Marker2Size { get; set; }

        [Description("Color of data point markers.")]
        public string Marker2Color { get; set; }

        [Description("Number of points that star-shaped marker have.")]
        public int? Marker2StarPointCount { get; set; }

        [Description("Border color of the marker")]
        public string Marker2BorderColor { get; set; }

        [Description("Whether border of the marker is visible.")]
        public bool HideMarker2Border { get; set; }

        [Description("Filling mode of the marker.")]
        public FillMode? Marker2FillMode { get; set; }

        [Description("Second color for gradient and hatch fill of the marker.")]
        public string Marker2BackColor2 { get; set; }

        [Description("Background gradient's direction of the marker.")]
        public PolygonGradientMode? Marker2GradientMode { get; set; }

        [Description("Marker's hatch style")]
        public HatchStyle? Marker2HatchStyle { get; set; }

        [Description("Whether the series markers are visible.")]
        public bool? Marker2Visibility { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is RangeAreaSeriesView view)
            {
                view.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

                var border1Color = Utils.ColorFromString(Border1Color);
                if (border1Color != System.Drawing.Color.Empty)
                {
                    view.Border1.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    view.Border1.Color = border1Color;
                }
                if (Border1Thickness.HasValue)
                {
                    view.Border1.Thickness  = Border1Thickness.Value;
                    view.Border1.Visibility = DevExpress.Utils.DefaultBoolean.True;
                }

                var border2Color = Utils.ColorFromString(Border2Color);
                if (border2Color != System.Drawing.Color.Empty)
                {
                    view.Border2.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    view.Border2.Color = border2Color;
                }
                if (Border2Thickness.HasValue)
                {
                    view.Border2.Thickness  = Border2Thickness.Value;
                    view.Border2.Visibility = DevExpress.Utils.DefaultBoolean.True;
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


                if (Marker1Kind.HasValue)
                    view.Marker1.Kind = (DevExpress.XtraCharts.MarkerKind)Marker1Kind.Value;
                var marker1Color = Utils.ColorFromString(Marker1Color);
                if (marker1Color != System.Drawing.Color.Empty)
                    view.Marker1.Color = marker1Color;
                if (Marker1Size.HasValue)
                    view.Marker1.Size = Marker1Size.Value;
                if (Marker1StarPointCount.HasValue)
                    view.Marker1.StarPointCount = Marker1StarPointCount.Value;
                var marker1BorderColor = Utils.ColorFromString(Marker1BorderColor);
                if (marker1BorderColor != System.Drawing.Color.Empty)
                    view.Marker1.BorderColor = marker1BorderColor;
                if (HideMarker1Border)
                    view.Marker1.BorderVisible = false;

                if (Marker1FillMode.HasValue)
                {
                    switch (Marker1FillMode.Value)
                    {
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.Marker1.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (Marker1GradientMode.HasValue)
                                    gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)Marker1GradientMode.Value;
                                var backColor2 = Utils.ColorFromString(Marker1BackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case Chart.FillMode.Hatch:
                            if (view.Marker1.FillStyle.Options is HatchFillOptions hatchFillOptions)
                            {
                                if (Marker1HatchStyle.HasValue)
                                    hatchFillOptions.HatchStyle = Marker1HatchStyle.Value;
                                var backColor2 = Utils.ColorFromString(Marker1BackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    hatchFillOptions.Color2 = backColor2;
                            }
                            break;
                    }
                }

                if (Marker1Visibility.HasValue)
                    view.Marker1Visibility = Marker1Visibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;


                if (Marker2Kind.HasValue)
                    view.Marker2.Kind = (DevExpress.XtraCharts.MarkerKind)Marker2Kind.Value;
                var marker2Color = Utils.ColorFromString(Marker2Color);
                if (marker2Color != System.Drawing.Color.Empty)
                    view.Marker2.Color = marker2Color;
                if (Marker2Size.HasValue)
                    view.Marker2.Size = Marker2Size.Value;
                if (Marker2StarPointCount.HasValue)
                    view.Marker2.StarPointCount = Marker2StarPointCount.Value;
                var marker2BorderColor = Utils.ColorFromString(Marker2BorderColor);
                if (marker2BorderColor != System.Drawing.Color.Empty)
                    view.Marker2.BorderColor = marker2BorderColor;
                if (HideMarker2Border)
                    view.Marker2.BorderVisible = false;

                if (Marker2FillMode.HasValue)
                {
                    switch (Marker2FillMode.Value)
                    {
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.Marker2.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                            {
                                if (Marker2GradientMode.HasValue)
                                    gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)Marker2GradientMode.Value;
                                var backColor2 = Utils.ColorFromString(Marker2BackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    gradientFillOptions.Color2 = backColor2;
                            }
                            break;
                        case Chart.FillMode.Hatch:
                            if (view.Marker2.FillStyle.Options is HatchFillOptions hatchFillOptions)
                            {
                                if (Marker2HatchStyle.HasValue)
                                    hatchFillOptions.HatchStyle = Marker2HatchStyle.Value;
                                var backColor2 = Utils.ColorFromString(Marker2BackColor2);
                                if (backColor2 != System.Drawing.Color.Empty)
                                    hatchFillOptions.Color2 = backColor2;
                            }
                            break;
                    }
                }

                if (Marker2Visibility.HasValue)
                    view.Marker2Visibility = Marker2Visibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            }
        }
    }
}
