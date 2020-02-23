using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class Area3DSeriesContext: XY3DSeriesBaseContext
	{
		[Parameter(HelpMessage = "Depth of a slice that represents the 3D area series (the extent of the area series along the Z-axis).")]
		public double? AreaWidth { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is Area3DSeriesView view)
			{
				if (AreaWidth.HasValue)
					view.AreaWidth = AreaWidth.Value;
			}
		}
	}
}
