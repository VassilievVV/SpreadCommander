using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class NestedDoughnutSeriesOptions : DoughnutSeriesOptions
    {
        [Description("Specifies a group for all series having the same nested group value.")]
        public int? Group { get; set; }

        [Description("Inner indent between the outer and inner edges of nested doughnuts.")]
        public double? InnerIndent { get; set; }

        [Description("Nested doughnut's size, in respect to the sizes of other nested doughnuts.")]
        public double? Weight { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is not NestedDoughnutSeriesView viewDoughnut)
                return;

            if (InnerIndent.HasValue)
                viewDoughnut.InnerIndent = InnerIndent.Value;
            if (Weight.HasValue)
                viewDoughnut.Weight = Weight.Value;
        }

        protected internal override void BoundDataChanged(SCChart chart, DevExpress.XtraCharts.Series series)
        {
            base.BoundDataChanged(chart, series);

            if (series.View is NestedDoughnutSeriesView view)
            {
                if (Group.HasValue)
                    view.Group = Group.Value;
            }
        }
    }
}
