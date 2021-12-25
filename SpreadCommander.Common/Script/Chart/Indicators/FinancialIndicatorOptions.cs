using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class FinancialIndicatorOptions : IndicatorOptions
    {
        [Description("Individual point for the indicators, that are drawn through two data points (such as Fibonacci Indicator or a Trend Line).")]
        public object Point1Argument { get; set; }

        [Description("Value indicating how to obtain the value of a financial indicator's point. Close, High, Lot, Open etc.")]
        public ValueLevel? Point1ValueLevel { get; set; }

        [Description("Individual point for the indicators, that are drawn through two data points (such as Fibonacci Indicator or a Trend Line).")]
        public object Point2Argument { get; set; }

        [Description("Value indicating how to obtain the value of a financial indicator's point. Close, High, Lot, Open etc.")]
        public ValueLevel? Point2ValueLevel { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is FinancialIndicator financialIndicator)
            {
                financialIndicator.Point1.Argument = Point1Argument;
                if (Point1ValueLevel.HasValue)
                    financialIndicator.Point1.ValueLevel = (DevExpress.XtraCharts.ValueLevel)Point1ValueLevel.Value;
                financialIndicator.Point2.Argument = Point2Argument;
                if (Point2ValueLevel.HasValue)
                    financialIndicator.Point2.ValueLevel = (DevExpress.XtraCharts.ValueLevel)Point2ValueLevel.Value;
            }
        }
    }
}
