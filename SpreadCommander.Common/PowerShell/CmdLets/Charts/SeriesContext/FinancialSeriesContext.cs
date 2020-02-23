using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class FinancialSeriesContext: XY2DSeriesBaseExContext
	{
		[Parameter(HelpMessage = "Length (in axis units) of the level line in financial series.")]
		public double? LevelLineLength { get; set; }

		[Parameter(HelpMessage = "Thickness of the lines used to draw stocks or candle sticks within the financial view.")]
		public int? LineThickness { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is FinancialSeriesViewBase view)
			{
				if (LevelLineLength.HasValue && LevelLineLength.Value >= 0)
					view.LevelLineLength = LevelLineLength.Value;
				if (LineThickness.HasValue && LineThickness.Value >= 0)
					view.LineThickness = LineThickness.Value;
			}
		}
	}
}
