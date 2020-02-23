using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class SplineSeriesContext: LineSeriesContext
	{
		[Parameter(HelpMessage = "Line tension (\"smoothness\" of a spline curve) to be used when drawing splines of the SplineAreaSeriesView, in percents.")]
		[ValidateRange(0, 100)]
		public int? LineTensionPercent { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is SplineSeriesView view)
			{
				if (LineTensionPercent.HasValue)
					view.LineTensionPercent = LineTensionPercent.Value;
			}
		}
	}
}
