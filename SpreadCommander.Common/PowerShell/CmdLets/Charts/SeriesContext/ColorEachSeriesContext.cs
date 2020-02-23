using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class ColorEachSeriesContext: XY2DSeriesBaseExContext
	{
		[Parameter(HelpMessage = "Whether each data point of a series is shown in a different color.")]
		public SwitchParameter ColorEach { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is SeriesViewColorEachSupportBase view)
			{
				view.ColorEach = ColorEach;
			}
		}
	}
}
