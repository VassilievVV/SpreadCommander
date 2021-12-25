using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class PercentageErrorBarsIndicatorOptions: ErrorBarsIndicatorOptions
    {
        [Description("Percentage of error values of series point values.")]
        [DefaultValue(5.0)]
        public double Percent { get; set; } = 5.0;


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is PercentageErrorBars percentageErrorBars)
            {
                percentageErrorBars.Percent = Percent;
            }
        }
    }
}
