using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class TrendLineIndicatorOptions : FinancialIndicatorOptions
    {
        [Description("Whether the Trend Line is extrapolated to infinity.")]
        public bool? ExtrapolateToInfinity { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is TrendLine trendLine)
            {
                if (ExtrapolateToInfinity.HasValue)
                    trendLine.ExtrapolateToInfinity = ExtrapolateToInfinity.Value;
            }
        }
    }
}
