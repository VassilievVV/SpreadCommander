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
	public class KeySeriesColorizerContext: BaseSeriesColorizerContext
	{
		[Parameter(Mandatory = true, HelpMessage = "Keys used to colorize series points using the key-color colorizer.")]
		public object[] Keys { get; set; }

		[Parameter(HelpMessage = "Pattern to format the text the legend shows for a color range. For example '{V1}-{V2}'.")]
		public string LegendItemPattern { get; set; }

		[Parameter(HelpMessage = "palette that provides colors for the colorizer.")]
		public ChartPaletteName? Palette { get; set; }

		[Parameter(HelpMessage = "Whether to show the colorizer items in the legend.")]
		public SwitchParameter ShowInLegend { get; set; }


		public override ChartColorizerBase CreateColorizer()
		{
			return new KeyColorColorizer();
		}

		public override void SetupXtraChartColorizer(ChartContext chartContext, ChartColorizerBase colorizer)
		{
			var keyColorizer = colorizer as KeyColorColorizer ?? throw new Exception("Colorizer must be Key color colorizer.");

			if (Keys != null)
			{
				foreach (var key in Keys)
				{
					object objKey = key is PSObject ? ((PSObject)key).BaseObject : key;
					keyColorizer.Keys.Add(objKey);
				}
			}

			if (!string.IsNullOrWhiteSpace(LegendItemPattern))
				keyColorizer.LegendItemPattern = LegendItemPattern;

			if (Palette.HasValue && Palette.Value != ChartPaletteName.None)
			{
				string paletteName = Regex.Replace(Enum.GetName(typeof(ChartPaletteName), Palette), "([A-Z])", " $1").Trim();
				keyColorizer.PaletteName = paletteName;
			}

			keyColorizer.ShowInLegend = ShowInLegend;
		}
	}
}
