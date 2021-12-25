using DevExpress.Utils;
using SpreadCommander.Common.Code;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class SeriesTitleOptions
    {
        [Description("Title's alignment.")]
        [DefaultValue(StringAlignment.Center)]
        public StringAlignment Alignment { get; set; } = StringAlignment.Center;

        [Description("Parent control edges, to which a title is docked.")]
        public ChartTitleDockStyle? Dock { get; set; }

        [Description("Font used to display the title's text.")]
        public string Font { get; set; }

        [Description("How much the chart title is indented from the client region of its parent control. Default is 5.")]
        public int? Indent { get; set; } = 5;

        [Description("Whether a title's text should wrap when it's too lengthy.")]
        public bool WordWrap { get; set; }


        protected internal virtual void SetupChartSeriesTitle(SCChart chart, string text, SeriesTitle title)
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
        public SCChart AddSeriesTitle(string seriesName, string text, SeriesTitleOptions options = null)
        {
            options ??= new SeriesTitleOptions();

            DevExpress.XtraCharts.Series series;
            if (string.IsNullOrWhiteSpace(seriesName))
                series = CurrentSeries;
            else
                series = Chart.Series[seriesName];
            if (series == null)
                throw new Exception($"Cannot find series '{seriesName}'.");

            if (series.View is not SimpleDiagramSeriesViewBase seriesView)
                throw new Exception("Series title is supported only on 2D Pie, Doughnut and Funnel series.");

            var seriesTitle = new SeriesTitle();
            options.SetupChartSeriesTitle(this, text, seriesTitle);
            seriesView.Titles.Add(seriesTitle);

            return this;
        }
    }
}
