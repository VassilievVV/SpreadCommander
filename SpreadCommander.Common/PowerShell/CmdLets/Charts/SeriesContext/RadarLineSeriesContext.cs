using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class RadarLineSeriesContext: RadarPointSeriesContext
    {
        [Parameter(HelpMessage = "Whether the last point in a series is joined with the first point to form a open/closed polygon.")]
        public SwitchParameter Open { get; set; }

        [Parameter(HelpMessage = "Color of data point markers.")]
        public string MarkerColor { get; set; }

        [Parameter(HelpMessage = "Whether the series point markers are visible.")]
        public bool? MarkerVisibility { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? LineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? LineThickness { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is RadarLineSeriesView view)
            {
                if (Open && !(view is RadarAreaSeriesView))
                    view.Closed = false;

                var markerColor = Utils.ColorFromString(MarkerColor);
                if (markerColor != Color.Empty)
                    view.LineMarkerOptions.Color = markerColor;

                if (MarkerVisibility.HasValue)
                    view.MarkerVisibility = MarkerVisibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

                if (LineDashStyle.HasValue)
                    view.LineStyle.DashStyle = LineDashStyle.Value;
                if (LineJoin.HasValue)
                    view.LineStyle.LineJoin  = LineJoin.Value;
                if (LineThickness.HasValue)
                    view.LineStyle.Thickness = LineThickness.Value;
            }
        }
    }
}
