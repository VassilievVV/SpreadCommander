using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class StepAreaSeriesContext: AreaSeriesContext
    {
        [Parameter(HelpMessage = "The manner in which a step area connects data point markers.")]
        public SwitchParameter InvertedStep { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is StepAreaSeriesView view)
            {
                view.InvertedStep = InvertedStep;
            }
        }
    }
}
