using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
    public class MovingAverageConvergenceDivergenceIndicatorContext: SeparatePanelIndicatorContext
    {
        [Parameter(HelpMessage = "Long period value required to calculate the indicator.")]
        [PSDefaultValue(Value = 26)]
        [DefaultValue(26)]
        public int LongPeriod { get; set; } = 26;

        [Parameter(HelpMessage = "Short period value required to calculate the indicator.")]
        [PSDefaultValue(Value = 12)]
        [DefaultValue(12)]
        public int ShortPeriod { get; set; } = 12;

        [Parameter(HelpMessage = "Color of the signal line of the indicator.")]
        public string SignalLineColor { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? SignalLineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? SignalLineJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? SignalLineThickness { get; set; }

        [Parameter(HelpMessage = "Smoothing period value required to calculate the indicator.")]
        [PSDefaultValue(Value = 9)]
        [DefaultValue(9)]
        public int SignalSmoothingPeriod { get; set; } = 9;

        [Parameter(HelpMessage = "Value specifying which series point value should be used to calculate the indicator.")]
        public ValueLevel? ValueLevel { get; set; }


        public override Indicator CreateIndicator()
        {
            return new MovingAverageConvergenceDivergence();
        }

        public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chartContext, indicator);

            if (indicator is MovingAverageConvergenceDivergence macd)
            {
                macd.LongPeriod            = LongPeriod;
                macd.ShortPeriod           = ShortPeriod;
                macd.SignalSmoothingPeriod = SignalSmoothingPeriod;
                if (ValueLevel.HasValue)
                    macd.ValueLevel = ValueLevel.Value;

                var signalLineColor = Utils.ColorFromString(SignalLineColor);
                if (signalLineColor != Color.Empty)
                    macd.SignalLineColor = signalLineColor;

                if (SignalLineDashStyle.HasValue)
                    macd.SignalLineStyle.DashStyle = SignalLineDashStyle.Value;
                if (SignalLineJoin.HasValue)
                    macd.SignalLineStyle.LineJoin  = SignalLineJoin.Value;
                if (SignalLineThickness.HasValue)
                    macd.SignalLineStyle.Thickness = SignalLineThickness.Value;
            }
        }
    }
}
