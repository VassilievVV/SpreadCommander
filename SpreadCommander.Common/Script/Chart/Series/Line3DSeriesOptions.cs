using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class Line3DSeriesOptions: XY3DSeriesBaseOptions
    {
        [Description("Thickness of the line.")]
        public int? LineThickness { get; set; }

        [Description("Width of a line (the extent of the line along the Z-axis) in the 3D Line Chart. Value, measured in fractions of X-axis units, where an axis unit is the distance between two major X-axis values.")]
        public double? LineWidth { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is Line3DSeriesView view)
            {
                if (LineThickness.HasValue)
                    view.LineThickness = LineThickness.Value;
                if (LineWidth.HasValue)
                    view.LineWidth = LineWidth.Value;
            }
        }
    }
}
