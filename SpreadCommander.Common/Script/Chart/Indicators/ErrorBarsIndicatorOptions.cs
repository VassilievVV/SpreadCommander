using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class ErrorBarsIndicatorOptions: IndicatorOptions
    {
        [Description("Error bar direction.")]
        public ErrorBarDirection? Direction { get; set; }

        [Description("Style of the error bar end.")]
        public ErrorBarEndStyle? EndStyle { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, DevExpress.XtraCharts.Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is ErrorBars errorBars)
            {
                if (Direction.HasValue)
                    errorBars.Direction = (DevExpress.XtraCharts.ErrorBarDirection)Direction.Value;
                
                if (EndStyle.HasValue)
                    errorBars.EndStyle  = (DevExpress.XtraCharts.ErrorBarEndStyle)EndStyle.Value;
            }
        }
    }
}
