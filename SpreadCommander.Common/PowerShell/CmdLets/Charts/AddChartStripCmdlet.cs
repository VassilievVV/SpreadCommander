using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartStrip")]
    public class AddChartStripCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(HelpMessage = "Name of the strip.")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Axis type. X or Y.")]
        public ChartAxisType AxisType { get; set; }

        [Parameter(Position = 1, HelpMessage = "Axis name. $null for primary axis.")]
        public string AxisName { get; set; }

        [Parameter(HelpMessage = "Text for an axis label that identifies the strip within its axis.")]
        public string AxisLabelText { get; set; }

        [Parameter(HelpMessage = "Background color for an axis label.")]
        public string Color { get; set; }

        [Parameter(HelpMessage = "Label's 2nd background color, if FillMode is gradient or hatch.")]
        public string Color2 { get; set; }

        [Parameter(HelpMessage = "Label's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Legend displaying a strip legend item.")]
        public string LegendName { get; set; }

        [Parameter(HelpMessage = "Minimum limiting value of the strip along an axis.")]
        public object MinLimit { get; set; }

        [Parameter(HelpMessage = "Maximum limiting value of the strip along an axis.")]
        public object MaxLimit { get; set; }

        [Parameter(HelpMessage = "Whether the strip is labeled within its axis.")]
        public SwitchParameter ShowAxisLabel { get; set; }

        [Parameter(HelpMessage = "Whether the strip is represented in the chart control's legend.")]
        public SwitchParameter ShowInLegend { get; set; }


        protected override void UpdateChart()
        {
            AxisBase axis;

            if (!string.IsNullOrWhiteSpace(AxisName))
            {
                axis = BaseAxisCmdlet.GetSecondaryAxis(ChartContext.Chart.Diagram, AxisType, AxisName);
                if (axis == null)
                    throw new Exception($"Cannot find axis '{AxisName}'.");
            }
            else
            {
                axis = BaseAxisCmdlet.GetPrimaryAxis(ChartContext.Chart.Diagram, AxisType);
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
                strip.FillStyle.FillMode = FillMode.Value;
                switch (FillMode.Value)
                {
                    case DevExpress.XtraCharts.FillMode.Empty:
                        break;
                    case DevExpress.XtraCharts.FillMode.Solid:
                        break;
                    case DevExpress.XtraCharts.FillMode.Gradient:
                        if (strip.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(Color2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = FillGradientMode.Value;
                        }
                        break;
                    case DevExpress.XtraCharts.FillMode.Hatch:
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
                var legend   = ChartContext.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'");
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
}
