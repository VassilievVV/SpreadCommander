using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class SideBySideStackedBar3DSeriesContext: SideBySideBar3DSeriesContext
	{
		[Parameter(HelpMessage = "Group for all similar series having the same stack group value, to be accumulated into the same stacked bars.")]
		public int? StackedGroup { get; set; }


		public override void BoundDataChanged(ChartContext chartContext, Series series)
		{
			base.BoundDataChanged(chartContext, series);

			if (series.View is SideBySideFullStackedBar3DSeriesView viewFull)
			{
				if (StackedGroup.HasValue)
					viewFull.StackedGroup = StackedGroup.Value;
			}
			else if (series.View is SideBySideStackedBar3DSeriesView view)
			{
				if (StackedGroup.HasValue)
					view.StackedGroup = StackedGroup.Value;
			}
		}
	}
}
