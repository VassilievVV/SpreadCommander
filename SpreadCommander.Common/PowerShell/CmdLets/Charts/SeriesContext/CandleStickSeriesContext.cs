using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class CandleStickSeriesContext : FinancialSeriesContext
	{
		[Parameter(HelpMessage = "Color of the price reduction.")]
		public string ReductionColor { get; set; }

		[Parameter(HelpMessage = "Mode used to color the financial series points.")]
		public ReductionColorMode? ReductionColorMode { get; set; }

		[Parameter(HelpMessage = "Value specifying how the Candle Stick Series View points will be filled.")]
		public CandleStickFillMode? ReductionFillMode { get; set; }

		[Parameter(HelpMessage = "Particular price value (open, close, high or low) which the analysis of the price action is performed by.")]
		public StockLevel? ReductionLevel { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is CandleStickSeriesView view)
			{
				var reductionColor = Utils.ColorFromString(ReductionColor);
				if (reductionColor != System.Drawing.Color.Empty)
				{
					view.ReductionOptions.Color   = reductionColor;
					view.ReductionOptions.Visible = true;
				}
				if (ReductionColorMode.HasValue)
				{
					view.ReductionOptions.ColorMode = ReductionColorMode.Value;
					view.ReductionOptions.Visible   = true;
				}
				if (ReductionFillMode.HasValue)
				{
					view.ReductionOptions.FillMode = ReductionFillMode.Value;
					view.ReductionOptions.Visible  = true;
				}
				if (ReductionLevel.HasValue)
				{
					view.ReductionOptions.Level   = ReductionLevel.Value;
					view.ReductionOptions.Visible = true;
				}
			}
		}
	}
}
