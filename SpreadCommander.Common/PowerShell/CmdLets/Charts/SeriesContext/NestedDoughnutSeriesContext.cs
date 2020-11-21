using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class NestedDoughnutSeriesContext: DoughnutSeriesContext
	{
		[Parameter(HelpMessage = "Specifies a group for all series having the same nested group value.")]
		public int? Group { get; set; }

		[Parameter(HelpMessage = "Inner indent between the outer and inner edges of nested doughnuts.")]
		public double? InnerIndent { get; set; }

		[Parameter(HelpMessage = "Nested doughnut's size, in respect to the sizes of other nested doughnuts.")]
		public double? Weight { get; set; }
		

		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is not NestedDoughnutSeriesView viewDoughnut)
				return;

			if (InnerIndent.HasValue)
				viewDoughnut.InnerIndent = InnerIndent.Value;
			if (Weight.HasValue)
				viewDoughnut.Weight = Weight.Value;
		}

		public override void BoundDataChanged(ChartContext chartContext, Series series)
		{
			base.BoundDataChanged(chartContext, series);

			if (series.View is NestedDoughnutSeriesView view)
			{
				if (Group.HasValue)
					view.Group = Group.Value;
			}
		}
	}
}
