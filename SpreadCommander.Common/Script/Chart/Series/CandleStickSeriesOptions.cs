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
    public class CandleStickSeriesOptions: FinancialSeriesOptions
    {
        [Description("Color of the price reduction.")]
        public string ReductionColor { get; set; }

        [Description("Mode used to color the financial series points.")]
        public ReductionColorMode? ReductionColorMode { get; set; }

        [Description("Value specifying how the Candle Stick Series View points will be filled.")]
        public CandleStickFillMode? ReductionFillMode { get; set; }

        [Description("Particular price value (open, close, high or low) which the analysis of the price action is performed by.")]
        public StockLevel? ReductionLevel { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is CandleStickSeriesView view)
            {
                var reductionColor = Utils.ColorFromString(ReductionColor);
                if (reductionColor != System.Drawing.Color.Empty)
                {
                    view.ReductionOptions.Color   = reductionColor;
                    view.ReductionOptions.Visible = true;
                }
                if (ReductionColorMode.HasValue)
                {
                    view.ReductionOptions.ColorMode = (DevExpress.XtraCharts.ReductionColorMode)ReductionColorMode.Value;
                    view.ReductionOptions.Visible   = true;
                }
                if (ReductionFillMode.HasValue)
                {
                    view.ReductionOptions.FillMode = (DevExpress.XtraCharts.CandleStickFillMode)ReductionFillMode.Value;
                    view.ReductionOptions.Visible  = true;
                }
                if (ReductionLevel.HasValue)
                {
                    view.ReductionOptions.Level   = (DevExpress.XtraCharts.StockLevel)ReductionLevel.Value;
                    view.ReductionOptions.Visible = true;
                }
            }
        }
    }
}
