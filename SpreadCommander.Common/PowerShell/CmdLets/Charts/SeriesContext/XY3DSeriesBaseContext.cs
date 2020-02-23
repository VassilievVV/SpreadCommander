using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class XY3DSeriesBaseContext : BaseSeriesContext
	{
		[Parameter(HelpMessage = "Aggregate function used for a series.")]
		[PSDefaultValue(Value = SCSeriesAggregateFunction.Default)]
		[DefaultValue(SCSeriesAggregateFunction.Default)]
		public SCSeriesAggregateFunction AggregateFunction { get; set; } = SCSeriesAggregateFunction.Default;

		[Parameter(HelpMessage = "Color of the series.")]
		public string Color { get; set; }

		[Parameter(HelpMessage = "Transparency (0-255) to use for displaying XY 3D charts.")]
		public byte? Transparency { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is XYDiagram3DSeriesViewBase view)
			{
				view.AggregateFunction = (SeriesAggregateFunction)(int)AggregateFunction;
				var color = Utils.ColorFromString(Color);
				if (color != System.Drawing.Color.Empty)
					view.Color = color;
				if (Transparency.HasValue)
					view.Transparency = Transparency.Value;
			}
		}
	}
}
