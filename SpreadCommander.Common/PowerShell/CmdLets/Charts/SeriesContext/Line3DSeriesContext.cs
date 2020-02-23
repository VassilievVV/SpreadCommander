using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class Line3DSeriesContext: XY3DSeriesBaseContext
	{
		[Parameter(HelpMessage = "Thickness of the line.")]
		public int? LineThickness { get; set; }

		[Parameter(HelpMessage = "Width of a line (the extent of the line along the Z-axis) in the 3D Line Chart. Value, measured in fractions of X-axis units, where an axis unit is the distance between two major X-axis values.")]
		public double? LineWidth { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is Line3DSeriesView view)
			{
				if (LineThickness.HasValue)
					view.LineThickness = LineThickness.Value;
				if (LineWidth.HasValue)
					view.LineWidth = LineWidth.Value;
			}
		}
	}
}
