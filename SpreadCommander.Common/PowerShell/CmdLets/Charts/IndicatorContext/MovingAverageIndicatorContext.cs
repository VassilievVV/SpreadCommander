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
    public class MovingAverageIndicatorContext: SubsetBasedIndicatorContext
    {
        [Parameter(HelpMessage = "Color for the Envelope kind of the Moving Average and Envelope technical indicator.")]
        public string EnvelopeColor { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? EnvelopeDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? EnvelopeJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? EnvelopeThickness { get; set; }

        [Parameter(HelpMessage = "Whether to display a Moving Average, Envelope, or both.")]
        public MovingAverageKind? Kind { get; set; }


        public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chartContext, indicator);

            if (indicator is MovingAverage movingAverage)
            {
                var envelopeColor = Utils.ColorFromString(EnvelopeColor);
                if (envelopeColor != Color.Empty)
                    movingAverage.EnvelopeColor = envelopeColor;

                if (EnvelopeDashStyle.HasValue)
                    movingAverage.EnvelopeLineStyle.DashStyle = EnvelopeDashStyle.Value;
                if (EnvelopeJoin.HasValue)
                    movingAverage.EnvelopeLineStyle.LineJoin  = EnvelopeJoin.Value;
                if (EnvelopeThickness.HasValue)
                    movingAverage.EnvelopeLineStyle.Thickness = EnvelopeThickness.Value;

                if (Kind.HasValue)
                    movingAverage.Kind = Kind.Value;
            }
        }
    }
}
