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
	public class SideBySideStackedBarSeriesContext: BarSeriesContext
	{
		[Parameter(HelpMessage = "Variable distance value (as a fraction of axis units) between two bars of different series shown at the same argument point.")]
		public double? BarDistance { get; set; }

		[Parameter(HelpMessage = "Fixed distance value (in pixels) between two bars of different series shown at the same argument point.")]
		public int? BarDistanceFixed { get; set; }

		[Parameter(HelpMessage = "Whether all bars of the same series should always have the same width, or they may have different widths, if some points of other series are missing.")]
		public SwitchParameter EqualBarWidth { get; set; }

		[Parameter(HelpMessage = "Group for all similar series having the same stack group value, to be accumulated into the same stacked bars.")]
		public int? StackedGroup { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is SideBySideStackedBarSeriesView view)
			{
				if (BarDistance.HasValue)
					view.BarDistance = BarDistance.Value;
				if (BarDistanceFixed.HasValue)
					view.BarDistanceFixed = BarDistanceFixed.Value;
				if (EqualBarWidth)
					view.EqualBarWidth = true;
			}
		}

		public override void BoundDataChanged(ChartContext chartContext, Series series)
		{
			base.BoundDataChanged(chartContext, series);

			if (series.View is SideBySideStackedBarSeriesView view)
			{
				if (StackedGroup.HasValue)
					view.StackedGroup = StackedGroup.Value;
			}
		}
	}
}
