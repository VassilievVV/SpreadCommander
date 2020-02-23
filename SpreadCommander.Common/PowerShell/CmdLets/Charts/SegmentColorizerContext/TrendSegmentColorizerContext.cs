using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SegmentColorizerContext
{
	public class TrendSegmentColorizerContext: BaseSegmentColorizerContext
	{
		[Parameter(HelpMessage = "Color used to draw the falling value segments.")]
		public string FallingColor { get; set; }

		[Parameter(HelpMessage = "Text the legend uses to identify the falling trend segments.")]
		public string FallingText { get; set; }

		[Parameter(HelpMessage = "Color used to draw the rising value segments.")]
		public string RisingColor { get; set; }

		[Parameter(HelpMessage = "Text the legend uses to identify the rising trend segments.")]
		public string RisingText { get; set; }

		[Parameter(HelpMessage = "Whether the colorizer should provide items for the legend.")]
		public SwitchParameter ShowInLegend { get; set; }


		public override SegmentColorizerBase CreateColorizer()
		{
			return new TrendSegmentColorizer();
		}
		public override void SetupXtraChartColorizer(ChartContext chartContext, SegmentColorizerBase colorizer)
		{
			var trendColorizer = colorizer as TrendSegmentColorizer ?? throw new Exception("Colorizer must be Trend segment colorizer.");

			var fallingColor = Utils.ColorFromString(FallingColor);
			if (fallingColor != Color.Empty)
				trendColorizer.FallingTrendColor = fallingColor;
			if (!string.IsNullOrWhiteSpace(FallingText))
				trendColorizer.FallingTrendLegendText = FallingText;

			var risingColor = Utils.ColorFromString(RisingColor);
			if (risingColor != Color.Empty)
				trendColorizer.RisingTrendColor = risingColor;
			if (!string.IsNullOrWhiteSpace(RisingText))
				trendColorizer.RisingTrendLegendText = RisingText;

			trendColorizer.ShowInLegend = ShowInLegend;
		}
	}
}
