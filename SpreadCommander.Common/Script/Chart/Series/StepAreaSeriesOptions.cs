using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class StepAreaSeriesOptions: AreaSeriesOptions
    {
        [Description("The manner in which a step area connects data point markers.")]
        public bool InvertedStep { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is StepAreaSeriesView view)
            {
                view.InvertedStep = InvertedStep;
            }
        }
    }
}
