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
    public class BollingerBandsIndicatorOptions: IndicatorOptions
    {
        [Description("Bands indicator's color.")]
        public string BandsColor { get; set; }

        [Description("Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? BandsDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? BandsJoin { get; set; }

        [Description("Line's thickness.")]
        public int? BandsThickness { get; set; }

        [Description("Number of data points used to calculate the indicator.")]
        [DefaultValue(20)]
        public int PointsCount { get; set; } = 20;

        [Description("Multiplier of the standard deviation used to plot this indicator.")]
        [DefaultValue(2.0)]
        public double StandardDeviationMultiplier { get; set; } = 2.0;

        [Description("Value specifying which series point value should be used to calculate the indicator.")]
        public ValueLevel? ValueLevel { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is BollingerBands bollingerBands)
            {
                var bandsColor = Utils.ColorFromString(BandsColor);
                if (bandsColor != System.Drawing.Color.Empty)
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
                    bollingerBands.ValueLevel = (DevExpress.XtraCharts.ValueLevel)ValueLevel.Value;
            }
        }
    }
}
