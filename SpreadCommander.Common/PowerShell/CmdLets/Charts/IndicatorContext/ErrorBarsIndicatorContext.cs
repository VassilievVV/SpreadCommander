using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext
{
    public class ErrorBarsIndicatorContext: BaseIndicatorContext
    {
        [Parameter(HelpMessage = "Error bar direction.")]
        public ErrorBarDirection? Direction { get; set; }

        [Parameter(HelpMessage = "Style of the error bar end.")]
        public ErrorBarEndStyle? EndStyle { get; set; }


        public override void SetupXtraChartIndicator(ChartContext chartContext, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chartContext, indicator);

            if (indicator is ErrorBars errorBars)
            {
                if (Direction.HasValue)
                    errorBars.Direction = Direction.Value;
                
                if (EndStyle.HasValue)
                    errorBars.EndStyle  = EndStyle.Value;
            }
        }
    }
}
