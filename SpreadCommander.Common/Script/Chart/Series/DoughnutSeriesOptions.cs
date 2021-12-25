using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class DoughnutSeriesOptions: PieSeriesOptions
    {
        [Description("Radius of the inner circle in the Doughnut Chart.")]
        [DefaultValue(60)]
        public int HoleRadiusPercent { get; set; } = 60;


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is DoughnutSeriesView viewDoughnut)
            {
                viewDoughnut.HoleRadiusPercent = HoleRadiusPercent;
            }
            else if (series.View is Doughnut3DSeriesView viewDoughnut3D)
            {
                viewDoughnut3D.HoleRadiusPercent = HoleRadiusPercent;
            }
        }
    }
}
