using DevExpress.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet.Chart
{
    public class SpreadChartAxis
    {
        [Description("Title of axis.")]
        public string Title							{ get; set; }

        [Description("Format of axis labels.")]
        public string NumberFormat					{ get; set; }

        [Description("Axis font in form 'Tahoma, 8, Bold, Italic, Green'.")]
        public string Font							{ get; set; }

        [Description("Position of major tick marks on the axis.")]
        public AxisTickMarks MajorTickMarks			{ get; set; }

        [Description("Position of major tick marks on the axis.")]
        public AxisTickMarks MinorTickMarks			{ get; set; }

        [Description("Whether to draw major gridlines on the chart or no.")]
        public bool? ShowMajorGridLines				{ get; set; }

        [Description("Whether to draw minor gridlines on the chart or no.")]
        public bool? ShowMinorGridLines				{ get; set; }

        [Description("Position of axis on the chart.")]
        public AxisPosition? Position				{ get; set; }

        [Description("Base unit for the date axis.")]
        [DefaultValue(AxisTimeUnits.Auto)]
        public AxisTimeUnits BaseTimeUnit			{ get; set; } = AxisTimeUnits.Auto;

        [Description("Text alignment for the tick-mark labels on the category axis.")]
        public AxisLabelAlignment? LabelAlignment	{ get; set; }

        [Description("Logarithmic base for the logarithmic axis.")]
        public double LogScaleBase					{ get; set; } = Math.E;

        [Description("Whether the value axis should display its numerical values using a logarithmic scale.")]
        public bool LogScale						{ get; set; }

        [Description("Minimum value of the numerical or date axis.")]
        public double? Minimum						{ get; set; }

        [Description("Maximum value of the numerical or date axis.")]
        public double? Maximum						{ get; set; }

        [Description("Specifies that the axis must be reversed, so the axis starts at the maximum value and ends at the minimum value.")]
        public bool Reversed						{ get; set; }

        [Description("Whether tick labels should be hidden.")]
        public bool HideTickLabels					{ get; set; }

        [Description("Whether the axis should be displayed.")]	
        [DefaultValue(true)]
        public bool Visible							{ get; set; } = true;
    }
}
