using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class ColorEachSeriesOptions: XY2DSeriesBaseExSeriesOptions
    {
        [Description("Whether each data point of a series is shown in a different color.")]
        public bool ColorEach { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is SeriesViewColorEachSupportBase view)
            {
                view.ColorEach = ColorEach;
            }
        }
    }
}
