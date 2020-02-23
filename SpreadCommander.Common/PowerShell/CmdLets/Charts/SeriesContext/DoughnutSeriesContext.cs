using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class DoughnutSeriesContext: PieSeriesContext
	{
		[Parameter(HelpMessage = "Radius of the inner circle in the Doughnut Chart.")]
		[PSDefaultValue(Value = 60)]
		[DefaultValue(60)]
		[ValidateRange(0, 100)]
		public int HoleRadiusPercent { get; set; } = 60;


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

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
