using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class RateOfChangeIndicatorOptions: SeparatePanelIndicatorOptions
    {
        [Description("Number of data points used to calculate the indicator.")]
        [DefaultValue(14)]
        public int PointsCount { get; set; } = 14;

        [Description("Value specifying which series point value should be used to calculate the indicator.")]
        public ValueLevel? ValueLevel { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is RateOfChange rateOfChange)
            {
                rateOfChange.PointsCount = PointsCount;
                if (ValueLevel.HasValue)
                    rateOfChange.ValueLevel = (DevExpress.XtraCharts.ValueLevel)ValueLevel.Value;
            }
        }
    }
}
