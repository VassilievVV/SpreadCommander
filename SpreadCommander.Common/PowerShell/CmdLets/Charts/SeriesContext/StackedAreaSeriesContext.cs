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
    public class StackedAreaSeriesContext: XY2DSeriesBaseExContext
    {
        [Parameter(HelpMessage = "Border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Border thickness.")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Second background color, if FillMode is gradient or hatch.")]
        public string Color2 { get; set; }

        [Parameter(HelpMessage = "Legend's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Transparency (0-255) to use for displaying the filled color areas.")]
        public byte? Transparency { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

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
                                var color2 = Utils.ColorFromString(Color2);
                                if (color2 != System.Drawing.Color.Empty)
                                    gradientOptions.Color2 = color2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = FillGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
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
