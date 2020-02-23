using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class Spline3DSeriesContext: Line3DSeriesContext
	{
		[Parameter(HelpMessage = "Line tension (\"smoothness\" of a spline curve) to be used when drawing splines of the Spline3DSeriesView, in percents.")]
		public int? LineTensionPercent { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is Spline3DSeriesView view)
			{
				if (LineTensionPercent.HasValue)
					view.LineTensionPercent = LineTensionPercent.Value;
			}
		}
	}
}
