using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesColorizerContext
{
	public class RangeSeriesColorizerContext: BaseSeriesColorizerContext
	{
		[Parameter(HelpMessage = "Pattern to format the text the legend shows for a color range. For example '{V1}-{V2}'.")]
		public string LegendItemPattern { get; set; }

		[Parameter(HelpMessage = "palette that provides colors for the colorizer.")]
		public ChartPaletteName? Palette { get; set; }

		[Parameter(Mandatory = true, HelpMessage = "Double values that form ranges to determine the color a line segment should have.")]
		public double[] RangeStops { get; set; }

		[Parameter(HelpMessage = "Whether to show the colorizer items in the legend.")]
		public SwitchParameter ShowInLegend { get; set; }


		public override ChartColorizerBase CreateColorizer()
		{
			return new RangeColorizer();
		}

		public override void SetupXtraChartColorizer(ChartContext chartContext, ChartColorizerBase colorizer)
		{
			var rangeColorizer = colorizer as RangeColorizer ?? throw new Exception("Colorizer must be Range colorizer.");

			if (!string.IsNullOrWhiteSpace(LegendItemPattern))
				rangeColorizer.LegendItemPattern = LegendItemPattern;

			if (Palette.HasValue && Palette.Value != ChartPaletteName.None)
			{
				string paletteName = Regex.Replace(Enum.GetName(typeof(ChartPaletteName), Palette), "([A-Z])", " $1").Trim();
				rangeColorizer.PaletteName = paletteName;
			}

			if (RangeStops != null)
				rangeColorizer.RangeStops.AddRange(RangeStops);

			rangeColorizer.ShowInLegend = ShowInLegend;
		}
	}
}
