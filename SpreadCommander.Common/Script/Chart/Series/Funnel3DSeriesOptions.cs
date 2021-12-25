using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class Funnel3DSeriesOptions: SeriesOptions
    {
        [Description("Height - to - width ratio of a Funnel series. 0 - auto.")]
        public double? HeightToWidthRatio { get; set; }

        [Description("Distance between points of a Funnel series.")]
        public int? PointDistance { get; set; }

        [Description("Radius of the inner circle in the Funnel 3D series.")]
        public int? HoleRadiusPercent { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is Funnel3DSeriesView funnelView)
            {
                if (HeightToWidthRatio.HasValue)
                    funnelView.HeightToWidthRatio = HeightToWidthRatio.Value;

                if (HoleRadiusPercent.HasValue)
                    funnelView.HoleRadiusPercent = HoleRadiusPercent.Value;

                if (PointDistance.HasValue)
                    funnelView.PointDistance = PointDistance.Value;
            }
        }
    }
}
