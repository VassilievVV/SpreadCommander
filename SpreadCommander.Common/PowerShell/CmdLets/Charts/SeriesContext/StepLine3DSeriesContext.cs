using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class StepLine3DSeriesContext: Line3DSeriesContext
    {
        [Parameter(HelpMessage = "The manner in which a step area connects data point markers.")]
        public SwitchParameter InvertedStep { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is StepLine3DSeriesView view)
            {
                view.InvertedStep = !InvertedStep;
            }
        }
    }
}
