using DevExpress.Utils;
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
    public class TitleOptions
    {
        [Description("Title's alignment.")]
        [DefaultValue(StringAlignment.Center)]
        public StringAlignment Alignment { get; set; } = StringAlignment.Center;

        [Description("Edge of the chart which a title is docked to.")]
        public ChartTitleDockStyle? Dock { get; set; }

        [Description("Font used to display the title's text.")]
        public string Font { get; set; }

        [Description("How much the chart title is indented from the client region of its parent control. Default is 5.")]
        public int? Indent { get; set; } = 5;

        [Description("Whether a title's text should wrap when it's too lengthy.")]
        public bool WordWrap { get; set; }


        protected internal virtual void SetupXtraChartTitle(SCChart chart, string text, DevExpress.XtraCharts.ChartTitle title)
        {
            title.Alignment          = Alignment;
            title.EnableAntialiasing = DefaultBoolean.True;
            title.Text               = text;
            title.Visibility         = DefaultBoolean.True;
            title.WordWrap           = WordWrap;

            if (Dock.HasValue)
                title.Dock = (DevExpress.XtraCharts.ChartTitleDockStyle)Dock.Value;
            if (Indent.HasValue)
                title.Indent = Indent.Value;

            var font = Utils.StringToFont(Font, out Color fontColor);
            if (font != null)
                title.Font = font;
            if (fontColor != Color.Empty)
                title.TextColor = fontColor;
        }
    }

    public partial class SCChart
    {
        public SCChart AddTitle(string seriesName, string text, TitleOptions options = null)
        {
            options ??= new TitleOptions();

            var chartTitle = new DevExpress.XtraCharts.ChartTitle();
            options.SetupXtraChartTitle(this, text, chartTitle);
            Chart.Titles.Add(chartTitle);

            return this;
        }
    }
}
