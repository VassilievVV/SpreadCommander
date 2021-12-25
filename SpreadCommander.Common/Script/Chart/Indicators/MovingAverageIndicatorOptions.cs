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
    public class MovingAverageIndicatorOptions: SubsetBasedIndicatorOptions
    {
        [Description("Color for the Envelope kind of the Moving Average and Envelope technical indicator.")]
        public string EnvelopeColor { get; set; }

        [Description("Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? EnvelopeDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? EnvelopeJoin { get; set; }

        [Description("Line's thickness.")]
        public int? EnvelopeThickness { get; set; }

        [Description("Whether to display a Moving Average, Envelope, or both.")]
        public MovingAverageKind? Kind { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is MovingAverage movingAverage)
            {
                var envelopeColor = Utils.ColorFromString(EnvelopeColor);
                if (envelopeColor != System.Drawing.Color.Empty)
                    movingAverage.EnvelopeColor = envelopeColor;

                if (EnvelopeDashStyle.HasValue)
                    movingAverage.EnvelopeLineStyle.DashStyle = EnvelopeDashStyle.Value;
                if (EnvelopeJoin.HasValue)
                    movingAverage.EnvelopeLineStyle.LineJoin  = EnvelopeJoin.Value;
                if (EnvelopeThickness.HasValue)
                    movingAverage.EnvelopeLineStyle.Thickness = EnvelopeThickness.Value;

                if (Kind.HasValue)
                    movingAverage.Kind = (DevExpress.XtraCharts.MovingAverageKind)Kind.Value;
            }
        }
    }
}
