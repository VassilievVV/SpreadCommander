using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class MassIndexIndicatorOptions: SeparatePanelIndicatorOptions
    {
        [Description("Count of points used to calculate the exponential moving average (EMA).")]
        [DefaultValue(9)]
        public int MovingAveragePointsCount { get; set; } = 9;

        [Description("Count of summable values.")]
        [DefaultValue(25)]
        public int SumPointsCount { get; set; } = 25;


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is MassIndex massIndex)
            {
                massIndex.MovingAveragePointsCount = MovingAveragePointsCount;
                massIndex.SumPointsCount           = SumPointsCount;
            }
        }
    }
}
