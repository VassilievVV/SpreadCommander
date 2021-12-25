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
    public class ConstantLineOptions
    {
        [Description("Color of a constant line.")]
        public string Color { get; set; }

        [Description("Legend displaying a constant line legend item.")]
        public string LegendName { get; set; }

        [Description("Text that identifies the constant line within the legend.")]
        public string LegendText { get; set; }

        [Description("Dash style used to paint the line.")]
        public DashStyle? LineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Description("Line's thickness.")]
        public int? LineThickness { get; set; }

        [Description("Whether the constant line is displayed behind the other chart elements on the diagram.")]
        public bool ShowBehind { get; set; }

        [Description("Whether the constant line is represented in the chart's legend.")]
        public bool ShowInLegend { get; set; }

        [Description("Title's text, with HTML formatting.")]
        public string Title { get; set; }

        [Description("Value that defines how to align the pane title.")]
        public ConstantLineTitleAlignment? TitleAlignment { get; set; }

        [Description("Font used to display the title's text.")]
        public string TitleFont { get; set; }

        [Description("Whether a constant line's title is displayed below or above the line.")]
        public bool TitleBelowLine { get; set; }


        protected internal virtual void SetupXtraChartConstantLine(SCChart chart, ChartAxisType axisType,
            string axisName, string name, object value)
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

            if (axis is not Axis2D axis2D)
                throw new Exception("Only 2D axis support constant lines.");

            var line = new ConstantLine();

            if (!string.IsNullOrWhiteSpace(name))
                line.Name = name;

            var color = Utils.ColorFromString(Color);
            if (color != System.Drawing.Color.Empty)
                line.Color = color;

            if (!string.IsNullOrWhiteSpace(LegendName))
            {
                var legend  = chart.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'");
                line.Legend = legend;
            }

            if (!string.IsNullOrWhiteSpace(LegendText))
                line.LegendText = LegendText;

            if (LineDashStyle.HasValue)
                line.LineStyle.DashStyle = (DevExpress.XtraCharts.DashStyle)LineDashStyle.Value;
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
                line.Title.Alignment = (DevExpress.XtraCharts.ConstantLineTitleAlignment)TitleAlignment.Value;
            var titleFont = Utils.StringToFont(TitleFont, out System.Drawing.Color titleColor);
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

    public partial class SCChart
    {
        public SCChart AddConstantLine(ChartAxisType axisType,
            string name, object value, ConstantLineOptions options = null) =>
            AddConstantLine(axisType, null, name, value, options);

        public SCChart AddConstantLine(ChartAxisType axisType,
            string axisName, string name, object value, ConstantLineOptions options = null)
        {
            options ??= new ConstantLineOptions();
            options.SetupXtraChartConstantLine(this, axisType, axisName, name, value);

            return this;
        }
    }
}
