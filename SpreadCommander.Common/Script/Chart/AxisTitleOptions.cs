using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class AxisTitleOptions
    {
        [Description("Alignment of the axis title.")]
        public StringAlignment? Alignment { get; set; }

        [Description("Font used to display the title's text.")]
        public string Font { get; set; }

        [Description("Number of lines to which a title text is allowed to wrap.")]
        public int? MaxLineCount { get; set; }

        [Description("Value which specifies whether to show an axis title on a diagram.")]
        public bool? Visible { get; set; }

        [Description("Whether a title's text should wrap when it's too lengthy.")]
        public bool WordWrap { get; set; }

        protected internal virtual void SetupXtraChartAxisTitle(SCChart chart, ChartAxisType axisType, string axisName,
            string text)
        {
            AxisBase axis;

            if (!string.IsNullOrWhiteSpace(axisName))
            {
                axis = AxisOptions.GetSecondaryAxis(chart.Chart.Diagram, axisType, axisName);
                if (axis == null)
                    throw new Exception($"Cannot find axis '{axisName}'.");
            }
            else
            {
                axis = AxisOptions.GetPrimaryAxis(chart.Chart.Diagram, axisType);
                if (axis == null)
                    throw new Exception("Cannot find primary axis.");
            }

            if (axis is not Axis2D axis2D)
                throw new Exception("Only 2D axis support title.");

            var title = axis2D.Title;

            title.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

            if (Alignment.HasValue)
                title.Alignment = Alignment.Value;

            var font = Utils.StringToFont(Font, out Color textColor);
            if (font != null)
                title.Font = font;
            if (textColor != Color.Empty)
                title.TextColor = textColor;

            if (MaxLineCount.HasValue)
                title.MaxLineCount = MaxLineCount.Value;

            if (!string.IsNullOrWhiteSpace(text))
            {
                title.Text       = text;
                title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }

            if (Visible.HasValue)
                title.Visibility = Visible.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;

            title.WordWrap = WordWrap;
        }
    }

    public partial class SCChart
    {
        public SCChart SetAxisTitle(ChartAxisType axisType, string axisName, string text, AxisTitleOptions options = null)
        {
            options ??= new AxisTitleOptions();
            options.SetupXtraChartAxisTitle(this, axisType, axisName, text);

            return this;
        }
    }
}
