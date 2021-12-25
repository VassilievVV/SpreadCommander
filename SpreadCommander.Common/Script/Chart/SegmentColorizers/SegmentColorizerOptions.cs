using DevExpress.XtraCharts;
using SpreadCommander.Common.Script.Chart.SegmentColorizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.SegmentColorizers
{
    public class SegmentColorizerOptions
    {
        protected internal virtual void SetupXtraChartColorizer(SCChart chart, SegmentColorizerBase colorizer)
        {
        }
    }
}

namespace SpreadCommander.Common.Script.Chart
{ 
    public partial class SCChart
    {
        public SCChart AddPointBasedSegmentColorizer(SegmentColorizerType colorizerType, string seriesName, SegmentColorizerOptions options = null)
        {
            SegmentColorizerBase colorizer;

            switch (colorizerType)
            {
                case SegmentColorizerType.PointBased:
                    colorizer = new PointBasedSegmentColorizer();
                    options ??= new PointBasedSegmentColorizerOptions();
                    break;
                case SegmentColorizerType.Range:
                    colorizer = new RangeSegmentColorizer();
                    options ??= new RangeSegmentColorizerOptions();
                    break;
                case SegmentColorizerType.Trend:
                    colorizer = new TrendSegmentColorizer();
                    options = new TrendSegmentColorizerOptions();
                    break;
                default:
                    colorizer = new PointBasedSegmentColorizer();
                    options ??= new PointBasedSegmentColorizerOptions();
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

            var view = series.View as LineSeriesView ?? throw new Exception("Colorizers are supported only for 2D line charts.");
            view.SegmentColorizer = colorizer;

            return this;
        }
    }
}
