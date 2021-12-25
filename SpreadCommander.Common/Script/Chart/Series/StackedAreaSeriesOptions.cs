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
    public class StackedAreaSeriesOptions: XY2DSeriesBaseExSeriesOptions
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


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is StackedAreaSeriesView view)
            {
                view.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

                if (view.Border != null)
                {
                    var borderColor = Utils.ColorFromString(BorderColor);
                    if (borderColor != System.Drawing.Color.Empty)
                    {
                        view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                        view.Border.Color      = borderColor;
                    }
                    if (BorderThickness.HasValue)
                    {
                        view.Border.Thickness  = BorderThickness.Value;
                        view.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    }
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
                                if (FillGradientMode.HasValue)
                                    hatchOptions.HatchStyle = FillHatchStyle.Value;
                            }
                            break;
                    }
                }

                if (Transparency.HasValue)
                    view.Transparency = Transparency.Value;
            }
        }
    }
}
