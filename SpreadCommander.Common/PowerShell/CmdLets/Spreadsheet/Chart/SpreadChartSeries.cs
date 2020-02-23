using DevExpress.Spreadsheet.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Spreadsheet.Chart
{
    public class SpreadChartSeries
    {
        [Description("Name of chart series. If skipped - column names will be using as series names.")]
        public string Name			{ get; set; }

        [Description("Data (column name) to plot as series arguments.")]
        public string Arguments		{ get; set; }

        [Description("Data (column names) to plot as series values. Multiple values will be using for individual series. " +
            "Bubble chart requires pairs of adjoined columns. Stock chart requires set of adjoined columns.")]
        public string Values		{ get; set; }

        [Description("Chart types for series, when chart contain series with different types. " +
            "ChartType has to be set to the type compatible with types of all series.")]
        public ChartType Type		{ get; set; }

        [Description("Shape of markers which can be painted at each data point in the series on the line, scatter or radar chart and within the chart legend.")]
        public MarkerStyle Markers	{ get; set; }

        [Description("Axis types for series. Default is primary axis. First axis group must be Primary.")]
        public AxisGroup AxisGroup	{ get; set; }

        [Description("Explosion value for all slices in a pie or doughnut chart series.")]
        public int Explosion { get; set; }

        [Description("Shape used to display data points in the 3D bar or column chart.")]
        public BarShape Shape { get; set; }

        [Description("Whether the curve smoothing is turned on for the line or scatter chart.")]
        public bool Smooth { get; set; }

        [Description("Start index for series. Index is 0-base. Negative values are allowed: -1 is last value, -2 is value before last etc.")]
        public int? FromIndex { get; set; }

        [Description("End index for series. Index is 0-base. Negative values are allowed: -1 is last value, -2 is value before last etc.")]
        public int? ToIndex { get; set; }

        [Description("Size of the marker in points. Values from 2 to 72 are allowed, default is 7.")]
        [DefaultValue(7)]
        public int MarkerSize { get; set; } = 7;

        //Trendlines are not rendered in SpreadsheetControl. Code can be used to display trendlines in MS Excel.
        //[Description("Trendlines for the series.")]
        //public SpreadChartSeriesTrendline[] Trendlines { get; set; }
    }
}
