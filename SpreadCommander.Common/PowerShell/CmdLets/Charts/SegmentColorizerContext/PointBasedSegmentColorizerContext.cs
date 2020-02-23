using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SegmentColorizerContext
{
    public class PointBasedSegmentColorizerContext: BaseSegmentColorizerContext
    {
        [Parameter(HelpMessage = "Direction that is used to distribute the point marker color.")]
        public ColorDistributionDirection? Direction { get; set; }


        public override SegmentColorizerBase CreateColorizer()
        {
            return new PointBasedSegmentColorizer();
        }

        public override void SetupXtraChartColorizer(ChartContext chartContext, SegmentColorizerBase colorizer)
        {
            var pointColorizer = colorizer as PointBasedSegmentColorizer ?? throw new Exception("Colorizer must be Point-based segment colorizer.");

            if (Direction.HasValue)
                pointColorizer.Direction = Direction.Value;
        }
    }
}
