using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class DetrendedPriceOscillatorIndicatorOptions: SeparatePanelIndicatorOptions
    {
        [Description("Number of data points used to calculate the indicator.")]
        [DefaultValue(20)]
        public int PointsCount { get; set; } = 20;

        [Description("Value specifying which series point value should be used to calculate the indicator.")]
        public ValueLevel? ValueLevel { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is DetrendedPriceOscillator detrendedPriceOscillator)
            {
                detrendedPriceOscillator.PointsCount = PointsCount;
                if (ValueLevel.HasValue)
                    detrendedPriceOscillator.ValueLevel = (DevExpress.XtraCharts.ValueLevel)ValueLevel.Value;
            }
        }
    }
}
