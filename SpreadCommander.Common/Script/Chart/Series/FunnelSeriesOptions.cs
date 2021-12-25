using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class FunnelSeriesOptions : SeriesOptions
    {
        [Description("Whether a Funnel series should be center-aligned within a diagram.")]
        public bool AlignToCenter { get; set; }

        [Description("Height - to - width ratio of a Funnel series. 0 - auto.")]
        public double? HeightToWidthRatio { get; set; }

        [Description("Distance between points of a Funnel series.")]
        public int? PointDistance { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is FunnelSeriesView funnelView)
            {
                funnelView.AlignToCenter = AlignToCenter;

                if (HeightToWidthRatio.HasValue)
                {
                    funnelView.HeightToWidthRatioAuto = false;
                    funnelView.HeightToWidthRatio     = HeightToWidthRatio.Value;
                }

                if (PointDistance.HasValue)
                    funnelView.PointDistance = PointDistance.Value;
            }
        }
    }
}
