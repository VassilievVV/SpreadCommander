using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
	public class GanttSeriesContext: RangeBarSeriesContext
	{
		[Parameter(HelpMessage = "Arrowhead length in pixels.")]
		public int? TaskLinkArrowHeight { get; set; }

		[Parameter(HelpMessage = "Arrowhead width in pixels.")]
		public int? TaskLinkArrowWidth { get; set; }

		[Parameter(HelpMessage = "Color which is used to draw the task link if the TaskLinkOptions.ColorSource property value is TaskLinkColorSource.OwnColor.")]
		public string TaskLinkColor { get; set; }

		[Parameter(HelpMessage = "Value indicating which color should be used when painting a task link.")]
		public TaskLinkColorSource? TaskLinkColorSource { get; set; }

		[Parameter(HelpMessage = "Whether task links should be in front of a diagram.")]
		public SwitchParameter TaskLinkInFront { get; set; }

		[Parameter(HelpMessage = "Minimum distance between a line connecting relative bars, and bar bodies (in pixels).")]
		public int? TaskLinkMinIndent { get; set; }

		[Parameter(HelpMessage = " Thickness (in pixels) of the line which is used to draw the task link.")]
		public int? TaskLinkThickness { get; set; }


		public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
		{
			base.SetupXtraChartSeries(chartContext, series);

			if (series.View is GanttSeriesView view)
			{
				if (TaskLinkArrowHeight.HasValue)
					view.LinkOptions.ArrowHeight = TaskLinkArrowHeight.Value;
				if (TaskLinkArrowWidth.HasValue)
					view.LinkOptions.ArrowWidth = TaskLinkArrowWidth.Value;

				var color = Utils.ColorFromString(TaskLinkColor);
				if (color != System.Drawing.Color.Empty)
				{
					view.LinkOptions.Color = color;
					view.LinkOptions.ColorSource = DevExpress.XtraCharts.TaskLinkColorSource.OwnColor;
				}
				else if (TaskLinkColorSource.HasValue)
					view.LinkOptions.ColorSource = TaskLinkColorSource.Value;
				if (TaskLinkInFront)
					view.LinkOptions.InFront = true;
				if (TaskLinkMinIndent.HasValue)
					view.LinkOptions.MinIndent = TaskLinkMinIndent.Value;
				if (TaskLinkThickness.HasValue)
					view.LinkOptions.Thickness = TaskLinkThickness.Value;

				view.LinkOptions.Visible |= TaskLinkArrowHeight.HasValue | TaskLinkArrowWidth.HasValue | !string.IsNullOrWhiteSpace(TaskLinkColor) |
					TaskLinkColorSource.HasValue | TaskLinkInFront | TaskLinkMinIndent.HasValue | TaskLinkThickness.HasValue;
			}
		}
	}
}
