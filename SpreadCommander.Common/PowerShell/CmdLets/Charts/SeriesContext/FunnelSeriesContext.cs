using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class FunnelSeriesContext: BaseSeriesContext
    {
        [Parameter(HelpMessage = "Whether a Funnel series should be center-aligned within a diagram.")]
        public SwitchParameter AlignToCenter { get; set; }

        [Parameter(HelpMessage = "Height - to - width ratio of a Funnel series. 0 - auto.")]
        public double? HeightToWidthRatio { get; set; }

        [Parameter(HelpMessage = "Distance between points of a Funnel series.")]
        public int? PointDistance { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

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
