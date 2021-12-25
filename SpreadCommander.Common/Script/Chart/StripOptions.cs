using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class StripOptions
    {
        [Description("Name of the strip.")]
        public string Name { get; set; }

        [Description("Text for an axis label that identifies the strip within its axis.")]
        public string AxisLabelText { get; set; }

        [Description("Background color for an axis label.")]
        public string Color { get; set; }

        [Description("Label's 2nd background color, if FillMode is gradient or hatch.")]
        public string Color2 { get; set; }

        [Description("Label's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Legend displaying a strip legend item.")]
        public string LegendName { get; set; }

        [Description("Minimum limiting value of the strip along an axis.")]
        public object MinLimit { get; set; }

        [Description("Maximum limiting value of the strip along an axis.")]
        public object MaxLimit { get; set; }

        [Description("Whether the strip is labeled within its axis.")]
        public bool ShowAxisLabel { get; set; }

        [Description("Whether the strip is represented in the chart control's legend.")]
        public bool ShowInLegend { get; set; }



        protected internal virtual void SetupXtraChartStrip(SCChart chart, ChartAxisType axisType, string axisName)
        {
            AxisBase axis;

            if (!string.IsNullOrWhiteSpace(axisName))
            {
                axis = AxisOptions.GetSecondaryAxis(chart.Chart.Diagram, axisType, axisName);
                if (axis == null)
                    throw new Exception($"Cannot find axis '{axisName}'.");
            }
            else
            {
                axis = AxisOptions.GetPrimaryAxis(chart.Chart.Diagram, axisType);
                if (axis == null)
                    throw new Exception("Cannot find primary axis.");
            }


            var axis2D = axis as Axis2D ?? throw new Exception("Strips are supported only in 2D charts.");

            var strip = new Strip();

            if (!string.IsNullOrWhiteSpace(Name))
                strip.Name = Name;

            strip.AxisLabelText = AxisLabelText;

            var backColor = Utils.ColorFromString(Color);
            if (backColor != System.Drawing.Color.Empty)
                strip.Color = backColor;

            if (FillMode.HasValue)
            {
                strip.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)FillMode.Value;
                switch (FillMode.Value)
                {
                    case Chart.FillMode.Empty:
                        break;
                    case Chart.FillMode.Solid:
                        break;
                    case Chart.FillMode.Gradient:
                        if (strip.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(Color2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                        }
                        break;
                    case Chart.FillMode.Hatch:
                        if (strip.FillStyle.Options is HatchFillOptions hatchOptions)
                        {
                            var backColor2 = Utils.ColorFromString(Color2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                hatchOptions.Color2 = backColor2;
                            if (FillHatchStyle.HasValue)
                                hatchOptions.HatchStyle = FillHatchStyle.Value;
                        }
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(LegendName))
            {
                var legend   = chart.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'");
                strip.Legend = legend;
            }

            if (MinLimit != null)
            {
                strip.MinLimit.AxisValue = MinLimit;
                strip.MinLimit.Enabled   = true;
            }
            if (MaxLimit != null)
            {
                strip.MaxLimit.AxisValue = MaxLimit;
                strip.MaxLimit.Enabled   = true;
            }

            strip.ShowAxisLabel = ShowAxisLabel;
            strip.ShowInLegend  = ShowInLegend;

            axis2D.Strips.Add(strip);
        }
    }

    public partial class SCChart
    {
        public SCChart AddStrip(ChartAxisType axisType, string axisName, StripOptions options = null)
        {
            options ??= new StripOptions();
            options.SetupXtraChartStrip(this, axisType, axisName);

            return this;
        }
    }
}
