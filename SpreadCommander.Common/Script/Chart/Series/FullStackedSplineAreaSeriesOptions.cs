using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    internal class FullStackedSplineAreaSeriesOptions: StackedAreaSeriesOptions
    {
        [Description("Line tension (\"smoothness\" of a spline curve) to be used when drawing splines of the SplineAreaSeriesView, in percents.")]
        public int? LineTensionPercent { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is FullStackedSplineAreaSeriesView view)
            {
                if (LineTensionPercent.HasValue)
                    view.LineTensionPercent = LineTensionPercent.Value;
            }
        }
    }
}
