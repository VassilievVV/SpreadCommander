using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class IndicatorOptions
    {
        [Description("Indicator name.")]
        public string Name { get; set; }

        [Description("Indicator's color.")]
        public string Color { get; set; }

        [Description("Legend displaying an indicator legend item.")]
        public string LegendName { get; set; }

        [Description("Text that identifies an indicator within the chart legend.")]
        public string LegendText { get; set; }

        [Description("Dash style used to paint the line.")]
        public DashStyle? LineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Description("Line's thickness.")]
        public int? LineThickness { get; set; }

        [Description("Whether the constant line is represented in the chart's legend.")]
        public bool ShowInLegend { get; set; }


        protected internal virtual void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            if (!string.IsNullOrWhiteSpace(Name))
                indicator.Name = Name;

            var color = Utils.ColorFromString(Color);
            if (color != System.Drawing.Color.Empty)
                indicator.Color = color;

            if (!string.IsNullOrWhiteSpace(LegendName))
            {
                var legend       = chart.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'");
                indicator.Legend = legend;
            }

            if (!string.IsNullOrWhiteSpace(LegendText))
                indicator.LegendText = LegendText;

            if (LineDashStyle.HasValue)
                indicator.LineStyle.DashStyle = (DevExpress.XtraCharts.DashStyle)LineDashStyle.Value;
            if (LineJoin.HasValue)
                indicator.LineStyle.LineJoin  = LineJoin.Value;
            if (LineThickness.HasValue)
                indicator.LineStyle.Thickness = LineThickness.Value;

            indicator.ShowInLegend = ShowInLegend;

            indicator.Visible = true;
        }
    }
}

namespace SpreadCommander.Common.Script.Chart
{
    public partial class SCChart
    {
        public SCChart AddIndicator(ChartIndicatorType indicatorType, string seriesName = null, Indicators.IndicatorOptions options = null)
        {
            options ??= new Indicators.IndicatorOptions();

            var indicator = CreateIndicator(indicatorType) ?? throw new Exception("Cannot configure indicator with specified type.");

            DevExpress.XtraCharts.Series series;
            if (string.IsNullOrWhiteSpace(seriesName))
                series = CurrentSeries;
            else
                series = Chart.Series[seriesName]; 
            if (series == null)
                throw new Exception($"Cannot find series '{seriesName}'.");

            var view   = series.View as XYDiagram2DSeriesViewBase ?? throw new Exception("Indicators are supported only for 2D charts.");

            view.Indicators.Add(indicator);

            options.SetupXtraChartIndicator(this, indicator);

            return this;
        }

        protected virtual Indicator CreateIndicator(ChartIndicatorType indicatorType)
        {
            return indicatorType switch
            {
                ChartIndicatorType.RegressionLine                                                => new RegressionLine(),
                ChartIndicatorType.TrendLine                                                     => new TrendLine(),
                ChartIndicatorType.MedianPrice                                                   => new MedianPrice(),
                ChartIndicatorType.TypicalPrice                                                  => new TypicalPrice(),
                ChartIndicatorType.WeightedClose                                                 => new WeightedClose(),
                ChartIndicatorType.Fibonacci                                                     => new FibonacciIndicator(),
                ChartIndicatorType.EMA or ChartIndicatorType.ExponentialMovingAverage            => new ExponentialMovingAverage(),
                ChartIndicatorType.SMA or ChartIndicatorType.SimpleMovingAverage                 => new SimpleMovingAverage(),
                ChartIndicatorType.TMA or ChartIndicatorType.TriangularMovingAverage             => new TriangularMovingAverage(),
                ChartIndicatorType.TEMA or ChartIndicatorType.TripleExponentialMovingAverageTema => new TripleExponentialMovingAverageTema(),
                ChartIndicatorType.WMA or ChartIndicatorType.WeightedMovingAverage               => new WeightedMovingAverage(),
                ChartIndicatorType.BollingerBands                                                => new BollingerBands(),
                ChartIndicatorType.MassIndex                                                     => new MassIndex(),
                ChartIndicatorType.StandardDeviation                                             => new StandardDeviation(),
                ChartIndicatorType.ATR or ChartIndicatorType.AverageTrueRange                    => new AverageTrueRange(),
                ChartIndicatorType.CHV or ChartIndicatorType.ChaikinsVolatility                  => new ChaikinsVolatility(),
                ChartIndicatorType.CCI or ChartIndicatorType.CommodityChannelIndex               => new CommodityChannelIndex(),
                ChartIndicatorType.DPO or ChartIndicatorType.DetrendedPriceOscillator            => new DetrendedPriceOscillator(),
                ChartIndicatorType.MACD or ChartIndicatorType.MovingAverageConvergenceDivergence => new MovingAverageConvergenceDivergence(),
                ChartIndicatorType.ROC or ChartIndicatorType.RateOfChange                        => new RateOfChange(),
                ChartIndicatorType.RSI or ChartIndicatorType.RelativeStrengthIndex               => new RelativeStrengthIndex(),
                ChartIndicatorType.TRIX or ChartIndicatorType.TripleExponentialMovingAverageTrix => new TripleExponentialMovingAverageTrix(),
                ChartIndicatorType.WilliamsR                                                     => new WilliamsR(),
                ChartIndicatorType.DataSourceBasedErrorBars                                      => new DataSourceBasedErrorBars(),
                ChartIndicatorType.FixedValueErrorBars                                           => new FixedValueErrorBars(),
                ChartIndicatorType.PercentageErrorBars                                           => new PercentageErrorBars(),
                ChartIndicatorType.StandardDeviationErrorBars                                    => new StandardDeviationErrorBars(),
                ChartIndicatorType.StandardErrorBars                                             => new StandardErrorBars(),
                _                                                                                => null,
            };
        }
    }
}
