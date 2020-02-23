using DevExpress.XtraCharts;
using SpreadCommander.Common.PowerShell.CmdLets.Charts.SegmentColorizerContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartSegmentColorizer")]
    public class AddChartSegmentColorizerCmdlet: BaseChartWithContextCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Type of segment colorizer - Range, Trend or Point.")]
        public SegmentColorizerType ColorizerType { get; set; }

        [Parameter(HelpMessage = "Name of series to which indicator is adding.")]
        public string SeriesName { get; set; }


        private BaseSegmentColorizerContext _ColorizerContext;

        public object GetDynamicParameters()
        {
            return ColorizerType switch
            {
                SegmentColorizerType.Range => CreateColorizerContext(typeof(RangeSegmentColorizerContext)),
                SegmentColorizerType.Trend => CreateColorizerContext(typeof(TrendSegmentColorizerContext)),
                SegmentColorizerType.Point => CreateColorizerContext(typeof(PointBasedSegmentColorizerContext)),
                _                          => throw new Exception("Unrecognized segment colorizer type.")
            };
            BaseSegmentColorizerContext CreateColorizerContext(Type typeContext)
            {
                if (_ColorizerContext == null || !(typeContext.IsInstanceOfType(_ColorizerContext)))
                    _ColorizerContext = Activator.CreateInstance(typeContext) as BaseSegmentColorizerContext;
                return _ColorizerContext;
            }
        }

        protected override void UpdateChart()
        {
            var colorizer = _ColorizerContext?.CreateColorizer() ?? throw new Exception("Cannot configure indicator with specified type.");

            Series series;
            if (string.IsNullOrWhiteSpace(SeriesName))
                series = ChartContext.CurrentSeries;
            else
                series = ChartContext.Chart.Series[SeriesName];
            if (series == null)
                throw new Exception($"Cannot find series '{SeriesName}'.");

            var view   = series.View as LineSeriesView ?? throw new Exception("Colorizers are supported only for 2D line charts.");

            SetupXtraChartColorizer(colorizer);

            if (_ColorizerContext != null)
                _ColorizerContext.SetupXtraChartColorizer(ChartContext, colorizer);

            view.SegmentColorizer = colorizer;
        }

        protected virtual void SetupXtraChartColorizer(SegmentColorizerBase colorizer)
        {
        }
    }
}
