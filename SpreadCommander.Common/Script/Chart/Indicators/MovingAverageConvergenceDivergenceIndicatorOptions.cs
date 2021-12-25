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
    public class MovingAverageConvergenceDivergenceIndicatorOptions: SeparatePanelIndicatorOptions
    {
        [Description("Long period value required to calculate the indicator.")]
        [DefaultValue(26)]
        public int LongPeriod { get; set; } = 26;

        [Description("Short period value required to calculate the indicator.")]
        [DefaultValue(12)]
        public int ShortPeriod { get; set; } = 12;

        [Description("Color of the signal line of the indicator.")]
        public string SignalLineColor { get; set; }

        [Description("Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? SignalLineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? SignalLineJoin { get; set; }

        [Description("Line's thickness.")]
        public int? SignalLineThickness { get; set; }

        [Description("Smoothing period value required to calculate the indicator.")]
        [DefaultValue(9)]
        public int SignalSmoothingPeriod { get; set; } = 9;

        [Description("Value specifying which series point value should be used to calculate the indicator.")]
        public ValueLevel? ValueLevel { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is MovingAverageConvergenceDivergence macd)
            {
                macd.LongPeriod            = LongPeriod;
                macd.ShortPeriod           = ShortPeriod;
                macd.SignalSmoothingPeriod = SignalSmoothingPeriod;
                if (ValueLevel.HasValue)
                    macd.ValueLevel = (DevExpress.XtraCharts.ValueLevel)ValueLevel.Value;

                var signalLineColor = Utils.ColorFromString(SignalLineColor);
                if (signalLineColor != System.Drawing.Color.Empty)
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
