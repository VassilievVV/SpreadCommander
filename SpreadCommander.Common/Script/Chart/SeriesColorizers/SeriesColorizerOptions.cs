using DevExpress.XtraCharts;
using SpreadCommander.Common.Script.Chart.SeriesColorizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.SeriesColorizers
{
    public class SeriesColorizerOptions
    {
        protected internal virtual void SetupXtraChartColorizer(SCChart chart,
            ChartColorizerBase colorizer)
        {
        }
    }
}

namespace SpreadCommander.Common.Script.Chart
{ 
    public partial class SCChart
    {
        public SCChart AddSeriesColorizer(SeriesColorizerType colorizerType, string seriesName, SeriesColorizerOptions options = null)
        {
            ChartColorizerBase colorizer;
            switch (colorizerType)
            {
                case SeriesColorizerType.Key:
                    colorizer = new KeyColorColorizer();
                    options ??= new KeySeriesColorizerOptions();
                    break;
                case SeriesColorizerType.Object:
                    colorizer = new ColorObjectColorizer();
                    options ??= new ObjectSeriesColorizerOptions();
                    break;
                case SeriesColorizerType.Range:
                    colorizer = new RangeColorizer();
                    options ??= new RangeSeriesColorizerOptions();
                    break;
                default:
                    colorizer = new KeyColorColorizer();
                    options ??= new KeySeriesColorizerOptions();
                    break;
            }

            DevExpress.XtraCharts.Series series;
            if (string.IsNullOrWhiteSpace(seriesName))
                series = CurrentSeries;
            else
                series = Chart.Series[seriesName];
            if (series == null)
                throw new Exception($"Cannot find series '{seriesName}'.");

            options.SetupXtraChartColorizer(this, colorizer);

            series.View.Colorizer = colorizer;

            return this;
        }
    }
}
