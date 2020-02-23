using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class XY2DSeriesBaseExContext: XY2DSeriesBaseContext
	{
		[Parameter(HelpMessage = "Series shadow's color.")]
		public string ShadowColor { get; set; }

		[Parameter(HelpMessage = "Series shadow's thickness.")]
		public int? ShadowSize { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is XYDiagramSeriesViewBase view)
			{
				var shadowColor = Utils.ColorFromString(ShadowColor);
				if (shadowColor != System.Drawing.Color.Empty)
				{
					view.Shadow.Color   = shadowColor;
					view.Shadow.Visible = true;
				}
				if (ShadowSize.HasValue)
				{
					view.Shadow.Size    = ShadowSize.Value;
					view.Shadow.Visible = true;
				}
			}
		}
	}
}
