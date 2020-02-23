using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class SideBySideBar3DSeriesContext: Bar3DSeriesContext
	{
		[Parameter(HelpMessage = "Variable distance value (as a fraction of axis units) between two bars of different series shown at the same argument point.")]
		public double? BarDistance { get; set; }

		[Parameter(HelpMessage = "Fixed distance value (in pixels) between two bars of different series shown at the same argument point.")]
		public int? BarDistanceFixed { get; set; }

		[Parameter(HelpMessage = "Whether all bars of the same series should always have the same width, or they may have different widths, if some points of other series are missing.")]
		public SwitchParameter EqualBarWidth { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is SideBySideBar3DSeriesView view)
			{
				if (BarDistance.HasValue)
					view.BarDistance = BarDistance.Value;
				if (BarDistanceFixed.HasValue)
					view.BarDistanceFixed = BarDistanceFixed.Value;
				if (EqualBarWidth)
					view.EqualBarWidth = true;
			}
		}
	}
}
