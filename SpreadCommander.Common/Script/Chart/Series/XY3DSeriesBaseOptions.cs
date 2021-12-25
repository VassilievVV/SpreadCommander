using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class XY3DSeriesBaseOptions: SeriesOptions
    {
        [Description("Aggregate function used for a series.")]
        [DefaultValue(SeriesAggregateFunction.Default)]
        public SeriesAggregateFunction AggregateFunction { get; set; } = SeriesAggregateFunction.Default;

        [Description("Color of the series.")]
        public string Color { get; set; }

        [Description("Transparency (0-255) to use for displaying XY 3D charts.")]
        public byte? Transparency { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is XYDiagram3DSeriesViewBase view)
            {
                view.AggregateFunction = (DevExpress.XtraCharts.SeriesAggregateFunction)AggregateFunction;
                var color = Utils.ColorFromString(Color);
                if (color != System.Drawing.Color.Empty)
                    view.Color = color;
                if (Transparency.HasValue)
                    view.Transparency = Transparency.Value;
            }
        }
    }
}
