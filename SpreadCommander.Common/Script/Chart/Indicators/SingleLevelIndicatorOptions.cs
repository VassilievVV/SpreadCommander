using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class SingleLevelIndicatorOptions: IndicatorOptions
    {
        [Description("Value level of the indicator - Close, High, Low, Open etc.")]
        public ValueLevel? ValueLevel { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is SingleLevelIndicator singleLevelIndicator)
            {
                if (ValueLevel.HasValue)
                    singleLevelIndicator.ValueLevel = (DevExpress.XtraCharts.ValueLevel)ValueLevel.Value;
            }
        }
    }
}
