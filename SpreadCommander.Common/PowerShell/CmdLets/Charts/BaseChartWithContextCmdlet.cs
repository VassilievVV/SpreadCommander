using DevExpress.XtraCharts.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
	public class BaseChartWithContextCmdlet: BaseChartCmdlet
	{
		[Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Chart context generated in other cmdlets such as New-Chart.")]
		public ChartContext ChartContext { get; set; }

		protected virtual bool PassThruChartContext => true;

		protected override void EndProcessing()
		{
			if (ChartContext?.Chart == null)
				throw new ArgumentNullException("Chart is not provided. Please provide ChartContext from pipeline, using New-Chart or another chart cmdlet.",
					nameof(ChartContext));

			UpdateChart();

			if (PassThruChartContext)
				WriteObject(ChartContext);
			else 
				ChartContext.Clear();
		}

		protected void FlushChart()
		{
			base.FlushChart(ChartContext.Chart);
		}

		protected virtual void UpdateChart()
		{
		}

		protected void BeginLoadChart()
		{
			(ChartContext?.Chart?.Container as ChartContainer)?.BeginLoad();
		}

		protected void EndLoadChart()
		{
			(ChartContext?.Chart?.Container as ChartContainer)?.EndLoad();
		}

		protected override Bitmap PaintChart(Chart chart, int width = 2000, int height = 1200, int? dpi = null)
		{
			if (ChartContext?.DiagramContext != null && chart.Diagram != null)
				ChartContext.DiagramContext.SetupDiagram(chart.Diagram);

			return base.PaintChart(chart, width, height, dpi);
		}
	}
}
