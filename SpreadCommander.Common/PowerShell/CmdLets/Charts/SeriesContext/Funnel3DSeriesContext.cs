using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class Funnel3DSeriesContext : BaseSeriesContext
    {
        [Parameter(HelpMessage = "Height - to - width ratio of a Funnel series. 0 - auto.")]
        public double? HeightToWidthRatio { get; set; }

        [Parameter(HelpMessage = "Distance between points of a Funnel series.")]
        public int? PointDistance { get; set; }

        [Parameter(HelpMessage = "Radius of the inner circle in the Funnel 3D series.")]
        [ValidateRange(0, 100)]
        public int? HoleRadiusPercent { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

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
