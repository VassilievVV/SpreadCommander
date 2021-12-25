using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class RadarLineSeriesOptions: RadarPointSeriesOptions
    {
        [Description("Whether the last point in a series is joined with the first point to form a open/closed polygon.")]
        public bool Open { get; set; }

        [Description("Color of data point markers.")]
        public string MarkerColor { get; set; }

        [Description("Whether the series point markers are visible.")]
        public bool? MarkerVisibility { get; set; }

        [Description("Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? LineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Description("Line's thickness.")]
        public int? LineThickness { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is RadarLineSeriesView view)
            {
                if (Open && view is not RadarAreaSeriesView)
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
