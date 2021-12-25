using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class SwiftPlotSeriesOptions: XY2DSeriesBaseSeriesOptions
    {
        [Description("Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? LineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is SwiftPlotSeriesView view)
            {
                if (LineDashStyle.HasValue)
                    view.LineStyle.DashStyle = LineDashStyle.Value;
                if (LineJoin.HasValue)
                    view.LineStyle.LineJoin = LineJoin.Value;
            }
        }
    }
}
