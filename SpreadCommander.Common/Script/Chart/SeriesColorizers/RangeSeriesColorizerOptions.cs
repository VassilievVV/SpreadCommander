using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.SeriesColorizers
{
    public class RangeSeriesColorizerOptions: SeriesColorizerOptions
    {
        [Description("Double values that form ranges to determine the color a line segment should have.")]
        public double[] RangeStops { get; set; }

        [Description("Pattern to format the text the legend shows for a color range. For example '{V1}-{V2}'.")]
        public string LegendItemPattern { get; set; }

        [Description("palette that provides colors for the colorizer.")]
        public ChartPaletteName? Palette { get; set; }

        [Description("Whether to show the colorizer items in the legend.")]
        public bool ShowInLegend { get; set; }


        protected internal override void SetupXtraChartColorizer(SCChart chart,
            ChartColorizerBase colorizer)
        {
            base.SetupXtraChartColorizer(chart, colorizer);

            if (colorizer is RangeColorizer rangeColorizer)
            {
                if (!string.IsNullOrWhiteSpace(LegendItemPattern))
                    rangeColorizer.LegendItemPattern = LegendItemPattern;

                if (Palette.HasValue && Palette.Value != ChartPaletteName.None)
                {
                    string paletteName = Regex.Replace(Enum.GetName(typeof(ChartPaletteName), Palette), "([A-Z])", " $1").Trim();
                    rangeColorizer.PaletteName = paletteName;
                }

                if (RangeStops != null)
                    rangeColorizer.RangeStops.AddRange(RangeStops);

                rangeColorizer.ShowInLegend = ShowInLegend;
            }
        }
    }
}
