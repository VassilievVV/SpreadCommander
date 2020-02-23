using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class SwiftPlotSeriesContext: XY2DSeriesBaseContext
    {
        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? LineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

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
