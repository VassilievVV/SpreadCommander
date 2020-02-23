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

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
    public class FibonacciIndicatorContext: FinancialIndicatorContext
    {
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Kind of the Fibonacci Indicator.")]
        public FibonacciIndicatorKind Kind { get; set; }

        [Parameter(HelpMessage = "Color of the lines representing the Fibonacci Indicator's base levels (0% or 100% depending on the indicator kind).")]
        public string BaseLevelColor { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? BaseLevelLineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? BaseLevelLineJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? BaseLevelLineThickness { get; set; }

        [Parameter(HelpMessage = "Text color of the Fibonacci Indicator's base levels labels (0% or 100% depending on the indicator kind).")]
        public string BaseLevelTextColor { get; set; }

        [Parameter(HelpMessage = "Font used to display the label's text.")]
        public string LabelFont { get; set; }

        [Parameter(HelpMessage = "Whether label is visible.")]
        public bool? LabelVisible { get; set; }

        [Parameter(HelpMessage = "Whether to display the additional levels for the Retracement type of Fibonacci Indicators.")]
        public SwitchParameter HideAdditionalLevels { get; set; }

        [Parameter(HelpMessage = "Whether to display the 0% level for the Fans type of Fibonacci Indicators.")]
        public SwitchParameter HideLevel0 { get; set; }

        [Parameter(HelpMessage = "Whether to display the 100% level for the Arcs type ofFibonacci Indicators.")]
        public SwitchParameter HideLevel100 { get; set; }

        [Parameter(HelpMessage = "Whether to display the 23.6% level for the Fibonacci Indicator of the required kind.")]
        public SwitchParameter HideLevel23_6 { get; set; }

        [Parameter(HelpMessage = "Whether to display the 76.4% level for the Fibonacci Indicator of the required kind.")]
        public SwitchParameter HideLevel76_4 { get; set; }


        public override Indicator CreateIndicator()
        {
            return new FibonacciIndicator();
        }

        public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chartContext, indicator);

            if (indicator is FibonacciIndicator fibonacciIndicator)
            {
                fibonacciIndicator.Kind = Kind;

                var baseLevelColor = Utils.ColorFromString(BaseLevelColor);
                if (baseLevelColor != Color.Empty)
                    fibonacciIndicator.BaseLevelColor = baseLevelColor;

                if (BaseLevelLineDashStyle.HasValue)
                    fibonacciIndicator.LineStyle.DashStyle = BaseLevelLineDashStyle.Value;
                if (BaseLevelLineJoin.HasValue)
                    fibonacciIndicator.LineStyle.LineJoin  = BaseLevelLineJoin.Value;
                if (BaseLevelLineThickness.HasValue)
                    fibonacciIndicator.LineStyle.Thickness = BaseLevelLineThickness.Value;

                fibonacciIndicator.Label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

                var baseLevelTextColor = Utils.ColorFromString(BaseLevelTextColor);
                if (baseLevelColor != Color.Empty)
                    fibonacciIndicator.Label.BaseLevelTextColor = baseLevelTextColor;

                var labelFont = Utils.StringToFont(LabelFont, out Color labelTextColor);
                if (labelFont != null)
                    fibonacciIndicator.Label.Font = labelFont;
                if (labelTextColor != Color.Empty)
                    fibonacciIndicator.Label.TextColor = labelTextColor;

                if (LabelVisible.HasValue)
                    fibonacciIndicator.Label.Visible = LabelVisible.Value;

                fibonacciIndicator.ShowAdditionalLevels = !HideAdditionalLevels;
                fibonacciIndicator.ShowLevel0           = !HideLevel0;
                fibonacciIndicator.ShowLevel100         = !HideLevel100;
                fibonacciIndicator.ShowLevel23_6        = !HideLevel23_6;
                fibonacciIndicator.ShowLevel76_4        = !HideLevel76_4;
            }
        }
    }
}
