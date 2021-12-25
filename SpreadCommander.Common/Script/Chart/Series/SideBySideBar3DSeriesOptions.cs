using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class SideBySideBar3DSeriesOptions: Bar3DSeriesOptions
    {
        [Description("Variable distance value (as a fraction of axis units) between two bars of different series shown at the same argument point.")]
        public double? BarDistance { get; set; }

        [Description("Fixed distance value (in pixels) between two bars of different series shown at the same argument point.")]
        public int? BarDistanceFixed { get; set; }

        [Description("Whether all bars of the same series should always have the same width, or they may have different widths, if some points of other series are missing.")]
        public bool EqualBarWidth { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is SideBySideBar3DSeriesView view)
            {
                if (BarDistance.HasValue)
                    view.BarDistance = BarDistance.Value;
                if (BarDistanceFixed.HasValue)
                    view.BarDistanceFixed = BarDistanceFixed.Value;
                if (EqualBarWidth)
                    view.EqualBarWidth = true;
            }
        }
    }
}
