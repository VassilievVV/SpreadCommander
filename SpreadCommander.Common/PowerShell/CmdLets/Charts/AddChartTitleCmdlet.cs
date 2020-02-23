using DevExpress.Utils;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Add, "ChartTitle")]
    public class AddChartTitleCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Title's text. Supports basic HTML formatting.")]
        public string Text { get; set; }

        [Parameter(HelpMessage = "Title's alignment.")]
        [PSDefaultValue(Value = StringAlignment.Center)]
        [DefaultValue(StringAlignment.Center)]
        public StringAlignment Alignment { get; set; } = StringAlignment.Center;

        [Parameter(HelpMessage = "Edge of the chart which a title is docked to.")]
        public ChartTitleDockStyle? Dock { get; set; }

        [Parameter(HelpMessage = "Font used to display the title's text.")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "How much the chart title is indented from the client region of its parent control. Default is 5.")]
        public int? Indent { get; set; } = 5;

        [Parameter(HelpMessage = "Whether a title's text should wrap when it's too lengthy.")]
        public SwitchParameter WordWrap { get; set; }


        protected override void UpdateChart()
        {
            var chartTitle = new DevExpress.XtraCharts.ChartTitle();
            SetupXtraChartTitle(chartTitle);
            ChartContext.Chart.Titles.Add(chartTitle);
        }

        public void SetupXtraChartTitle(DevExpress.XtraCharts.ChartTitle title)
        {
            title.Alignment          = Alignment;
            title.EnableAntialiasing = DefaultBoolean.True;
            title.Text               = Text;
            title.Visibility         = DefaultBoolean.True;
            title.WordWrap           = WordWrap;

            if (Dock.HasValue)
                title.Dock = Dock.Value;
            if (Indent.HasValue)
                title.Indent = Indent.Value;

            var font = Utils.StringToFont(Font, out Color fontColor);
            if (font != null)
                title.Font = font;
            if (fontColor != Color.Empty)
                title.TextColor = fontColor;
        }
    }
}
