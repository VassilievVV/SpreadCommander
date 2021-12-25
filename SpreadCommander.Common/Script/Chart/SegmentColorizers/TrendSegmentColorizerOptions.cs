using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.SegmentColorizers
{
    public class TrendSegmentColorizerOptions: SegmentColorizerOptions
    {
        [Description("Color used to draw the falling value segments.")]
        public string FallingColor { get; set; }

        [Description("Text the legend uses to identify the falling trend segments.")]
        public string FallingText { get; set; }

        [Description("Color used to draw the rising value segments.")]
        public string RisingColor { get; set; }

        [Description("Text the legend uses to identify the rising trend segments.")]
        public string RisingText { get; set; }

        [Description("Whether the colorizer should provide items for the legend.")]
        public bool ShowInLegend { get; set; }


        protected internal override void SetupXtraChartColorizer(SCChart chart, SegmentColorizerBase colorizer)
        {
            base.SetupXtraChartColorizer(chart, colorizer);

            if (colorizer is TrendSegmentColorizer trendColorizer)
            { 
                var fallingColor = Utils.ColorFromString(FallingColor);
                if (fallingColor != System.Drawing.Color.Empty)
                    trendColorizer.FallingTrendColor = fallingColor;
                if (!string.IsNullOrWhiteSpace(FallingText))
                    trendColorizer.FallingTrendLegendText = FallingText;

                var risingColor = Utils.ColorFromString(RisingColor);
                if (risingColor != System.Drawing.Color.Empty)
                    trendColorizer.RisingTrendColor = risingColor;
                if (!string.IsNullOrWhiteSpace(RisingText))
                    trendColorizer.RisingTrendLegendText = RisingText;

                trendColorizer.ShowInLegend = ShowInLegend;
            }
        }
    }
}
