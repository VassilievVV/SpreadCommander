using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class FixedValueErrorBarsIndicatorOptions: ErrorBarsIndicatorOptions
    {
        [Description("Fixed positive error value.")]
        [DefaultValue(1.0)]
        public double PositiveError { get; set; } = 1.0;

        [Description("Fixed negative error value.")]
        [DefaultValue(1.0)]
        public double NegativeError { get; set; } = 1.0;


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is FixedValueErrorBars fixedValueErrorBars)
            {
                fixedValueErrorBars.PositiveError = PositiveError;
                fixedValueErrorBars.NegativeError = NegativeError;
            }
        }
    }
}
