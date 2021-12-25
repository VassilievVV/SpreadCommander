using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.SegmentColorizers
{
    public class PointBasedSegmentColorizerOptions: SegmentColorizerOptions
    {
        [Description("Direction that is used to distribute the point marker color.")]
        public ColorDistributionDirection? Direction { get; set; }


        protected internal override void SetupXtraChartColorizer(SCChart chart, SegmentColorizerBase colorizer)
        {
            base.SetupXtraChartColorizer(chart, colorizer);

            if (colorizer is PointBasedSegmentColorizer pointColorizer)
            {
                if (Direction.HasValue)
                    pointColorizer.Direction = (DevExpress.XtraCharts.ColorDistributionDirection)Direction.Value;
            }
        }
    }
}
