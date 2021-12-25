using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class StandardDeviationErrorBarsIndicatorOptions: ErrorBarsIndicatorOptions
    {
        [Description("Multiplier on which the standard deviation value is multiplied before display.")]
        [DefaultValue(1.0)]
        public double Multiplier { get; set; } = 1.0;


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is StandardDeviationErrorBars standardDeviationErrorBars)
            {
                standardDeviationErrorBars.Multiplier = Multiplier;
            }
        }
    }
}
