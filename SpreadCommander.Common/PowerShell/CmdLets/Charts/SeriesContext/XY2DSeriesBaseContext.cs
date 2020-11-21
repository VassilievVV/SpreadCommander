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
	public class XY2DSeriesBaseContext: BaseSeriesContext
	{
		[Parameter(HelpMessage = "Aggregate function used for a series.")]
		[PSDefaultValue(Value = SCSeriesAggregateFunction.Default)]
		[DefaultValue(SCSeriesAggregateFunction.Default)]
		public SCSeriesAggregateFunction AggregateFunction { get; set; } = SCSeriesAggregateFunction.Default;

		[Parameter(HelpMessage = "Pane's name.")]
		public string Pane { get; set; }

		[Parameter(HelpMessage = "X axis name.")]
		public string AxisX { get; set; }

		[Parameter(HelpMessage = "Y axis name.")]
		public string AxisY { get; set; }

		[Parameter(HelpMessage = "Color of the series.")]
		public string Color { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is XYDiagram2DSeriesViewBase view)
			{
				if (view is SwiftPlotSeriesView swiftView)
					swiftView.Antialiasing = true;

				if (AggregateFunction != SCSeriesAggregateFunction.Default)
					view.AggregateFunction = (SeriesAggregateFunction)(int)AggregateFunction;

				var color = Utils.ColorFromString(Color);
				if (color != System.Drawing.Color.Empty)
					view.Color = color;

				if (chartContext.Chart.Diagram is XYDiagram2D diagram)
				{
					XYDiagramPane pane = null;
					Axis2D axisX = null, axisY = null;
					if (!string.IsNullOrWhiteSpace(Pane))
					{
						pane = diagram.Panes[Pane];
						if (pane == null)
							throw new Exception("Invalid pane name.");
					
					}
					if (!string.IsNullOrWhiteSpace(AxisX))
					{
						axisX = diagram.FindAxisXByName(AxisX);
						if (axisX == null)
							throw new Exception("Invalid axis X name.");
					}
					if (!string.IsNullOrWhiteSpace(AxisY))
					{
						axisY = diagram.FindAxisYByName(AxisY);
						if (axisY == null)
							throw new Exception("Invalid axis Y name.");
					}

					switch (view)
					{
						case XYDiagramSeriesViewBase viewBase:
							if (pane != null)
								viewBase.Pane = pane;
							if (axisX != null)
								viewBase.AxisX = axisX as AxisXBase;
							if (axisY != null)
								viewBase.AxisY = axisY as AxisYBase;
							break;
						case SwiftPlotSeriesViewBase viewSwift:
							if (pane != null)
								viewSwift.Pane = pane;
							if (axisX != null)
								viewSwift.AxisX = axisX as SwiftPlotDiagramAxisXBase;
							if (axisY != null)
								viewSwift.AxisY = axisY as SwiftPlotDiagramAxisYBase;
							break;
					}

				}
			}
		}
	}
}
