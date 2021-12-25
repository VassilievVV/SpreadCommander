using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class DataSourceBasedErrorBarsIndicatorOptions: ErrorBarsIndicatorOptions
    {
        [Description("Name of the data field which contains positive error values of an indicator for generated series points.")]
        public string PositiveErrorDataMember { get; set; }

        [Description("Name of the data field which contains negative error values of an indicator for generated series points.")]
        public string NegativeErrorDataMember { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is DataSourceBasedErrorBars dataSourceBasedErrorBars)
            {
                dataSourceBasedErrorBars.PositiveErrorDataMember = PositiveErrorDataMember;
                dataSourceBasedErrorBars.NegativeErrorDataMember = NegativeErrorDataMember;
            }
        }
    }
}
