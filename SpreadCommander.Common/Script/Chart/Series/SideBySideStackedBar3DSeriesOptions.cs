using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class SideBySideStackedBar3DSeriesOptions: SideBySideBar3DSeriesOptions
    {
        [Description("Group for all similar series having the same stack group value, to be accumulated into the same stacked bars.")]
        public int? StackedGroup { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is SideBySideFullStackedBar3DSeriesView viewFull)
            {
                if (StackedGroup.HasValue)
                    viewFull.StackedGroup = StackedGroup.Value;
            }
            else if (series.View is SideBySideStackedBar3DSeriesView view)
            {
                if (StackedGroup.HasValue)
                    view.StackedGroup = StackedGroup.Value;
            }
        }
    }
}
