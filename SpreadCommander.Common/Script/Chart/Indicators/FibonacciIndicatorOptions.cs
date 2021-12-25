using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class FibonacciIndicatorOptions: FinancialIndicatorOptions
    {
        [Description("Kind of the Fibonacci Indicator.")]
        public FibonacciIndicatorKind Kind { get; set; }

        [Description("Color of the lines representing the Fibonacci Indicator's base levels (0% or 100% depending on the indicator kind).")]
        public string BaseLevelColor { get; set; }

        [Description("Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? BaseLevelLineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? BaseLevelLineJoin { get; set; }

        [Description("Line's thickness.")]
        public int? BaseLevelLineThickness { get; set; }

        [Description("Text color of the Fibonacci Indicator's base levels labels (0% or 100% depending on the indicator kind).")]
        public string BaseLevelTextColor { get; set; }

        [Description("Font used to display the label's text.")]
        public string LabelFont { get; set; }

        [Description("Whether label is visible.")]
        public bool? LabelVisible { get; set; }

        [Description("Whether to display the additional levels for the Retracement type of Fibonacci Indicators.")]
        public bool HideAdditionalLevels { get; set; }

        [Description("Whether to display the 0% level for the Fans type of Fibonacci Indicators.")]
        public bool HideLevel0 { get; set; }

        [Description("Whether to display the 100% level for the Arcs type ofFibonacci Indicators.")]
        public bool HideLevel100 { get; set; }

        [Description("Whether to display the 23.6% level for the Fibonacci Indicator of the required kind.")]
        public bool HideLevel23_6 { get; set; }

        [Description("Whether to display the 76.4% level for the Fibonacci Indicator of the required kind.")]
        public bool HideLevel76_4 { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is FibonacciIndicator fibonacciIndicator)
            {
                fibonacciIndicator.Kind = (DevExpress.XtraCharts.FibonacciIndicatorKind)Kind;

                var baseLevelColor = Utils.ColorFromString(BaseLevelColor);
                if (baseLevelColor != System.Drawing.Color.Empty)
                    fibonacciIndicator.BaseLevelColor = baseLevelColor;

                if (BaseLevelLineDashStyle.HasValue)
                    fibonacciIndicator.LineStyle.DashStyle = BaseLevelLineDashStyle.Value;
                if (BaseLevelLineJoin.HasValue)
                    fibonacciIndicator.LineStyle.LineJoin  = BaseLevelLineJoin.Value;
                if (BaseLevelLineThickness.HasValue)
                    fibonacciIndicator.LineStyle.Thickness = BaseLevelLineThickness.Value;

                fibonacciIndicator.Label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

                var baseLevelTextColor = Utils.ColorFromString(BaseLevelTextColor);
                if (baseLevelColor != System.Drawing.Color.Empty)
                    fibonacciIndicator.Label.BaseLevelTextColor = baseLevelTextColor;

                var labelFont = Utils.StringToFont(LabelFont, out System.Drawing.Color labelTextColor);
                if (labelFont != null)
                    fibonacciIndicator.Label.Font = labelFont;
                if (labelTextColor != System.Drawing.Color.Empty)
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
