using DevExpress.XtraCharts;
using SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesColorizerContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartSeriesColorizer")]
    public class AddChartSeriesColorizerCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Type of Series colorizer - Range, Trend or Point.")]
        public SeriesColorizerType ColorizerType { get; set; }

        [Parameter(HelpMessage = "Name of series to which indicator is adding.")]
        public string SeriesName { get; set; }


        private BaseSeriesColorizerContext _ColorizerContext;

        public object GetDynamicParameters()
        {
            return ColorizerType switch
            {
                SeriesColorizerType.Object => CreateColorizerContext(typeof(ObjectSeriesColorizerContext)),
                SeriesColorizerType.Key    => CreateColorizerContext(typeof(KeySeriesColorizerContext)),
                SeriesColorizerType.Range  => CreateColorizerContext(typeof(RangeSeriesColorizerContext)),
                _                          => throw new Exception("Unrecognized series colorizer type.")
            };
            BaseSeriesColorizerContext CreateColorizerContext(Type typeContext)
            {
                if (_ColorizerContext == null || !(typeContext.IsInstanceOfType(_ColorizerContext)))
                    _ColorizerContext = Activator.CreateInstance(typeContext) as BaseSeriesColorizerContext;
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

            SetupXtraChartColorizer(colorizer);

            if (_ColorizerContext != null)
                _ColorizerContext.SetupXtraChartColorizer(ChartContext, colorizer);

            series.View.Colorizer = colorizer;
        }

        protected virtual void SetupXtraChartColorizer(ChartColorizerBase colorizer)
        {
        }
    }
}
