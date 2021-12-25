using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class GanttSeriesOptions: RangeBarSeriesOptions
    {
        [Description("Arrowhead length in pixels.")]
        public int? TaskLinkArrowHeight { get; set; }

        [Description("Arrowhead width in pixels.")]
        public int? TaskLinkArrowWidth { get; set; }

        [Description("Color which is used to draw the task link if the TaskLinkOptions.ColorSource property value is TaskLinkColorSource.OwnColor.")]
        public string TaskLinkColor { get; set; }

        [Description("Value indicating which color should be used when painting a task link.")]
        public TaskLinkColorSource? TaskLinkColorSource { get; set; }

        [Description("Whether task links should be in front of a diagram.")]
        public bool TaskLinkInFront { get; set; }

        [Description("Minimum distance between a line connecting relative bars, and bar bodies (in pixels).")]
        public int? TaskLinkMinIndent { get; set; }

        [Description(" Thickness (in pixels) of the line which is used to draw the task link.")]
        public int? TaskLinkThickness { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

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
                    view.LinkOptions.ColorSource = (DevExpress.XtraCharts.TaskLinkColorSource)TaskLinkColorSource.Value;
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
