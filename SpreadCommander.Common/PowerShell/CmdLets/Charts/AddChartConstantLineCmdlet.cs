using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartConstantLine")]
    public class AddChartConstantLineCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Axis type - X or Y.")]
        public ChartAxisType AxisType { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Name of the axis.")]
        public string AxisName { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Name of custom axis label.")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, Position = 3, HelpMessage = "Custom label's position along an axis.")]
        public object Value { get; set; }

        [Parameter(HelpMessage = "Color of a constant line.")]
        public string Color { get; set; }

        [Parameter(HelpMessage = "Legend displaying a constant line legend item.")]
        public string LegendName { get; set; }

        [Parameter(HelpMessage = "Text that identifies the constant line within the legend.")]
        public string LegendText { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? LineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? LineThickness { get; set; }

        [Parameter(HelpMessage = "Whether the constant line is displayed behind the other chart elements on the diagram.")]
        public SwitchParameter ShowBehind { get; set; }

        [Parameter(HelpMessage = "Whether the constant line is represented in the chart's legend.")]
        public SwitchParameter ShowInLegend { get; set; }

        [Parameter(HelpMessage = "Title's text, with HTML formatting.")]
        public string Title { get; set; }

        [Parameter(HelpMessage = "Value that defines how to align the pane title.")]
        public ConstantLineTitleAlignment? TitleAlignment { get; set; }

        [Parameter(HelpMessage = "Font used to display the title's text.")]
        public string TitleFont { get; set; }

        [Parameter(HelpMessage = "Whether a constant line's title is displayed below or above the line.")]
        public SwitchParameter TitleBelowLine { get; set; }


        protected override void UpdateChart()
        {
            AxisBase axis;

            if (!string.IsNullOrWhiteSpace(Name))
            {
                axis = BaseAxisCmdlet.GetSecondaryAxis(ChartContext.Chart.Diagram, AxisType, Name);
                if (axis == null)
                    throw new Exception($"Cannot find axis '{Name}'.");
            }
            else
            {
                axis = BaseAxisCmdlet.GetPrimaryAxis(ChartContext.Chart.Diagram, AxisType);
                if (axis == null)
                    throw new Exception("Cannot find primary axis.");
            }

            if (axis is not Axis2D axis2D)
                throw new Exception("Only 2D axis support constant lines.");

            var line = new ConstantLine();

            if (!string.IsNullOrWhiteSpace(Name))
                line.Name = Name;

            var color = Utils.ColorFromString(Color);
            if (color != System.Drawing.Color.Empty)
                line.Color = color;

            if (!string.IsNullOrWhiteSpace(LegendName))
            {
                var legend  = ChartContext.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'");
                line.Legend = legend;
            }

            if (!string.IsNullOrWhiteSpace(LegendText))
                line.LegendText = LegendText;

            if (LineDashStyle.HasValue)
                line.LineStyle.DashStyle = LineDashStyle.Value;
            if (LineJoin.HasValue)
                line.LineStyle.LineJoin = LineJoin.Value;
            if (LineThickness.HasValue)
                line.LineStyle.Thickness = LineThickness.Value;

            line.ShowBehind   = ShowBehind;
            line.ShowInLegend = ShowInLegend;

            if (!string.IsNullOrWhiteSpace(Title))
            {
                line.Title.Text    = Title;
                line.Title.Visible = true;
            }
            if (TitleAlignment.HasValue)
                line.Title.Alignment = TitleAlignment.Value;
            var titleFont = Utils.StringToFont(TitleFont, out Color titleColor);
            if (titleFont != null)
                line.Title.Font = titleFont;
            if (titleColor != System.Drawing.Color.Empty)
                line.Title.TextColor = titleColor;
            line.Title.ShowBelowLine = TitleBelowLine;
            line.Title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

            line.Visible = true;

            axis2D.ConstantLines.Add(line);
        }
    }
}