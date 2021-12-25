using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class SubsetBasedIndicatorOptions: IndicatorOptions
    {
        [Description("Number of data points used to calculate of the subset-based indicator.")]
        [DefaultValue(10)]
        public int PointsCount { get; set; } = 10;


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is SubsetBasedIndicator subsetIndicator)
                subsetIndicator.PointsCount = PointsCount;
        }
    }
}
