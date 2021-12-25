using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class Area3DSeriesOptions: XY3DSeriesBaseOptions
    {
        [Description("Depth of a slice that represents the 3D area series (the extent of the area series along the Z-axis).")]
        public double? AreaWidth { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is Area3DSeriesView view)
            {
                if (AreaWidth.HasValue)
                    view.AreaWidth = AreaWidth.Value;
            }
        }
    }
}
