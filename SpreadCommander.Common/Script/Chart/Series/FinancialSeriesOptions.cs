using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class FinancialSeriesOptions: XY2DSeriesBaseExSeriesOptions
    {
        [Description("Length (in axis units) of the level line in financial series.")]
        public double? LevelLineLength { get; set; }

        [Description("Thickness of the lines used to draw stocks or candle sticks within the financial view.")]
        public int? LineThickness { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is FinancialSeriesViewBase view)
            {
                if (LevelLineLength.HasValue && LevelLineLength.Value >= 0)
                    view.LevelLineLength = LevelLineLength.Value;
                if (LineThickness.HasValue && LineThickness.Value >= 0)
                    view.LineThickness = LineThickness.Value;
            }
        }
    }
}
