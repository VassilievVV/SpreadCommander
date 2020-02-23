using DevExpress.XtraCharts;
using SpreadCommander.Common.PowerShell.CmdLets.Charts.IndicatorContext;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartIndicator")]
    public class AddChartIndicatorCmdlet: BaseChartWithContextCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Indicator type.")]
        public ChartIndicatorType IndicatorType { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Name of series to which indicator is adding.")]
        public string SeriesName { get; set; }

        [Parameter(HelpMessage = "Indicator name.")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "Indicator's color.")]
        public string Color { get; set; }

        [Parameter(HelpMessage = "Legend displaying an indicator legend item.")]
        public string LegendName { get; set; }

        [Parameter(HelpMessage = "Text that identifies an indicator within the chart legend.")]
        public string LegendText { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the line.")]
        public DevExpress.XtraCharts.DashStyle? LineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive lines.")]
        public LineJoin? LineJoin { get; set; }

        [Parameter(HelpMessage = "Line's thickness.")]
        public int? LineThickness { get; set; }

        [Parameter(HelpMessage = "Whether the constant line is represented in the chart's legend.")]
        public SwitchParameter ShowInLegend { get; set; }


        private BaseIndicatorContext _IndicatorContext;

        public object GetDynamicParameters()
        {
            switch (IndicatorType)
            {
                case ChartIndicatorType.RegressionLine:
                    return CreateIndicatorContext(typeof(RegressionLineIndicatorContext));
                case ChartIndicatorType.TrendLine:
                    return CreateIndicatorContext(typeof(TrendLineIndicatorContext));

                case ChartIndicatorType.MedianPrice:
                    return CreateIndicatorContext(typeof(MedianPriceIndicatorContext));
                case ChartIndicatorType.TypicalPrice:
                    return CreateIndicatorContext(typeof(TypicalPriceIndicatorContext));
                case ChartIndicatorType.WeightedClose:
                    return CreateIndicatorContext(typeof(WeightedCloseIndicatorContext));

                case ChartIndicatorType.Fibonacci:
                    return CreateIndicatorContext(typeof(FibonacciIndicatorContext));

                case ChartIndicatorType.EMA:
                case ChartIndicatorType.ExponentialMovingAverage:
                    return CreateIndicatorContext(typeof(ExponentialMovingAverageIndicatorContext));
                case ChartIndicatorType.SMA:
                case ChartIndicatorType.SimpleMovingAverage:
                    return CreateIndicatorContext(typeof(SimpleMovingAverageIndicatorContext));
                case ChartIndicatorType.TMA:
                case ChartIndicatorType.TriangularMovingAverage:
                    return CreateIndicatorContext(typeof(TriangularMovingAverageIndicatorContext));
                case ChartIndicatorType.TEMA:
                case ChartIndicatorType.TripleExponentialMovingAverageTema:
                    return CreateIndicatorContext(typeof(TripleExponentialMovingAverageTemaIndicatorContext));
                case ChartIndicatorType.WMA:
                case ChartIndicatorType.WeightedMovingAverage:
                    return CreateIndicatorContext(typeof(WeightedMovingAverageIndicatorContext));

                case ChartIndicatorType.BollingerBands:
                    return CreateIndicatorContext(typeof(BollingerBandsIndicatorContext));
                case ChartIndicatorType.MassIndex:
                    return CreateIndicatorContext(typeof(MassIndexIndicatorContext));
                case ChartIndicatorType.StandardDeviation:
                    return CreateIndicatorContext(typeof(StandardDeviationIndexContext));

                case ChartIndicatorType.ATR:
                case ChartIndicatorType.AverageTrueRange:
                    return CreateIndicatorContext(typeof(AverageTrueRangeIndicatorContext));
                case ChartIndicatorType.CHV:
                case ChartIndicatorType.ChaikinsVolatility:
                    return CreateIndicatorContext(typeof(ChaikinsVolatilityIndicatorContext));
                case ChartIndicatorType.CCI:
                case ChartIndicatorType.CommodityChannelIndex:
                    return CreateIndicatorContext(typeof(CommodityChannelIndexIndicatorContext));
                case ChartIndicatorType.DPO:
                case ChartIndicatorType.DetrendedPriceOscillator:
                    return CreateIndicatorContext(typeof(DetrendedPriceOscillatorIndicatorContext));
                case ChartIndicatorType.MACD:
                case ChartIndicatorType.MovingAverageConvergenceDivergence:
                    return CreateIndicatorContext(typeof(MovingAverageConvergenceDivergenceIndicatorContext));
                case ChartIndicatorType.ROC:
                case ChartIndicatorType.RateOfChange:
                    return CreateIndicatorContext(typeof(RateOfChangeIndicatorContext));
                case ChartIndicatorType.RSI:
                case ChartIndicatorType.RelativeStrengthIndex:
                    return CreateIndicatorContext(typeof(RelativeStrengthIndexIndicatorContext));
                case ChartIndicatorType.TRIX:
                case ChartIndicatorType.TripleExponentialMovingAverageTrix:
                    return CreateIndicatorContext(typeof(TripleExponentialMovingAverageTrixIndicatorContext));
                case ChartIndicatorType.WilliamsR:
                    return CreateIndicatorContext(typeof(WilliamsRIndicatorContext));

                case ChartIndicatorType.DataSourceBasedErrorBars:
                    return CreateIndicatorContext(typeof(DataSourceBasedErrorBarsIndicatorContext));
                case ChartIndicatorType.FixedValueErrorBars:
                    return CreateIndicatorContext(typeof(FixedValueErrorBarsIndicatorContext));
                case ChartIndicatorType.PercentageErrorBars:
                    return CreateIndicatorContext(typeof(PercentageErrorBarsIndicatorContext));
                case ChartIndicatorType.StandardDeviationErrorBars:
                    return CreateIndicatorContext(typeof(StandardDeviationErrorBarsIndicatorContext));
                case ChartIndicatorType.StandardErrorBars:
                    return CreateIndicatorContext(typeof(StandardErrorBarsIndicatorContext));
            }

            _IndicatorContext = null;
            return null;


            BaseIndicatorContext CreateIndicatorContext(Type typeContext)
            {
                if (_IndicatorContext == null || !(typeContext.IsInstanceOfType(_IndicatorContext)))
                    _IndicatorContext = Activator.CreateInstance(typeContext) as BaseIndicatorContext;
                return _IndicatorContext;
            }
        }

        protected override void UpdateChart()
        {
            var indicator = _IndicatorContext?.CreateIndicator() ?? throw new Exception("Cannot configure indicator with specified type.");

            Series series;
            if (string.IsNullOrWhiteSpace(SeriesName))
                series = ChartContext.CurrentSeries;
            else
                series = ChartContext.Chart.Series[SeriesName]; 
            if (series == null)
                throw new Exception($"Cannot find series '{SeriesName}'.");

            var view   = series.View as XYDiagram2DSeriesViewBase ?? throw new Exception("Indicators are supported only for 2D charts.");

            view.Indicators.Add(indicator);

            SetupXtraChartIndicator(indicator);

            if (_IndicatorContext != null)
                _IndicatorContext.SetupXtraChartIndicator(ChartContext, indicator);
        }

        protected virtual void SetupXtraChartIndicator(Indicator indicator)
        {
            if (!string.IsNullOrWhiteSpace(Name))
                indicator.Name = Name;

            var color = Utils.ColorFromString(Color);
            if (color != System.Drawing.Color.Empty)
                indicator.Color = color;

            if (!string.IsNullOrWhiteSpace(LegendName))
            {
                var legend       = ChartContext.Chart.Legends[LegendName] ?? throw new Exception($"Invalid legend name: '{LegendName}'");
                indicator.Legend = legend;
            }

            if (!string.IsNullOrWhiteSpace(LegendText))
                indicator.LegendText = LegendText;

            if (LineDashStyle.HasValue)
                indicator.LineStyle.DashStyle = LineDashStyle.Value;
            if (LineJoin.HasValue)
                indicator.LineStyle.LineJoin  = LineJoin.Value;
            if (LineThickness.HasValue)
                indicator.LineStyle.Thickness = LineThickness.Value;

            indicator.ShowInLegend = ShowInLegend;

            indicator.Visible = true;
        }
    }
}
