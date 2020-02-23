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
    public class BollingerBandsIndicatorContext: BaseIndicatorContext
    {
        [Parameter(HelpMessage = "Bands indicator's color.")]
        public string BandsColor { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? BandsDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? BandsJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? BandsThickness { get; set; }

        [Parameter(HelpMessage = "Number of data points used to calculate the indicator.")]
        [PSDefaultValue(Value = 20)]
        [DefaultValue(20)]
        public int PointsCount { get; set; } = 20;

        [Parameter(HelpMessage = "Multiplier of the standard deviation used to plot this indicator.")]
        [PSDefaultValue(Value = 2.0)]
        [DefaultValue(2.0)]
        public double StandardDeviationMultiplier { get; set; } = 2.0;

        [Parameter(HelpMessage = "Value specifying which series point value should be used to calculate the indicator.")]
        public ValueLevel? ValueLevel { get; set; }


        public override Indicator CreateIndicator()
        {
            return new BollingerBands();
        }

        public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chartContext, indicator);

            if (indicator is BollingerBands bollingerBands)
            {
                var bandsColor = Utils.ColorFromString(BandsColor);
                if (bandsColor != Color.Empty)
                    bollingerBands.BandsColor = bandsColor;

                if (BandsDashStyle.HasValue)
                    bollingerBands.BandsLineStyle.DashStyle = BandsDashStyle.Value;
                if (BandsJoin.HasValue)
                    bollingerBands.BandsLineStyle.LineJoin  = BandsJoin.Value;
                if (BandsThickness.HasValue)
                    bollingerBands.BandsLineStyle.Thickness = BandsThickness.Value;

                bollingerBands.PointsCount                 = PointsCount;
                bollingerBands.StandardDeviationMultiplier = StandardDeviationMultiplier;

                if (ValueLevel.HasValue)
                    bollingerBands.ValueLevel = ValueLevel.Value;
            }
        }
    }
}
