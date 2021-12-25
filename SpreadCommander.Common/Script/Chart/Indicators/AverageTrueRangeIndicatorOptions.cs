using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class AverageTrueRangeIndicatorOptions: SeparatePanelIndicatorOptions
    {
        [Description("Number of data points used to calculate the indicator.")]
        [DefaultValue(14)]
        public int PointsCount { get; set; } = 14;


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is AverageTrueRange averageTrueRange)
                averageTrueRange.PointsCount = PointsCount;
        }
    }
}
