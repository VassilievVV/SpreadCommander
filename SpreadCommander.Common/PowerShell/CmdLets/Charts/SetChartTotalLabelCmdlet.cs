using DevExpress.Charts.Model;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Set, "ChartTotalLabel")]
    public class SetChartTotalLabelCmdlet : BaseChartWithContextCmdlet
    {
        [Parameter(HelpMessage = "Label's background color.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Label's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Parameter(HelpMessage = "Label's border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Border's thickness")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Label's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Font used to display the legend's contents.")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "Number of lines to which a label's text is allowed to wrap. Value in range 0 (no limit) to 20.")]
        public int? MaxLineCount { get; set; }

        [Parameter(HelpMessage = "Maximum width allowed for series labels.")]
        public int? MaxWidth { get; set; }

        [Parameter(HelpMessage = "Minimum indent between adjacent series labels, when an overlapping resolving algorithm is applied to them.")]
        public int? ResolveOverlappingMinIndent { get; set; }

        [Parameter(HelpMessage = "Mode to resolve overlapping of series labels.")]
        [PSDefaultValue(Value = ResolveOverlappingMode.Default)]
        [DefaultValue(ResolveOverlappingMode.Default)]
        public ResolveOverlappingMode ResolveOverlappingMode { get; set; } = ResolveOverlappingMode.Default;

        [Parameter(HelpMessage = "Shadow's color.")]
        public string ShadowColor { get; set; }

        [Parameter(HelpMessage = "Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Parameter(HelpMessage = "Alignment of the series labels text.")]
        public StringAlignment TextAlignment { get; set; }

        [Parameter(HelpMessage = "Pattern specifying the text to be displayed within series labels.")]
        public string TextPattern { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the connector line. For use with stacked bar total label.")]
        public DevExpress.XtraCharts.DashStyle? ConnectorDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive connector lines. For use with stacked bar total label.")]
        public LineJoin? ConnectorLineJoin { get; set; }

        [Parameter(HelpMessage = "Connector line's thickness. For use with stacked bar total label.")]
        public int? ConnectorThickness { get; set; }

        [Parameter(HelpMessage = "Whether total labels' connectors should be visible. For use with stacked bar total label.")]
        public SwitchParameter ShowConnector { get; set; }

        [Parameter(HelpMessage = "Distance between a series group's top edge and the group's total label. For use with stacked bar total label.")]
        public int? Indent { get; set; }


        protected override void UpdateChart()
        {
            bool processed = true;
            if (ChartContext.CurrentPane != null)
                SetupXtraChartLabel(ChartContext.CurrentPane.StackedBarTotalLabel);
            else if (ChartContext.Chart?.Diagram is XYDiagram diagramXY)
                SetupXtraChartLabel(diagramXY.DefaultPane.StackedBarTotalLabel);
            else if (ChartContext.CurrentSeries?.View is PieSeriesView pieView)
                SetupXtraChartLabel(pieView.TotalLabel);
            else if (ChartContext.Chart != null)
            {
                processed = false;
                //Series were added in QuickMode, without special cmdlet, so update all series.
                foreach (Series series in ChartContext.Chart.Series)
                    if (series.View is PieSeriesView pieSeriesView)
                    {
                        SetupXtraChartLabel(pieSeriesView.TotalLabel);
                        processed = true;
                    }
            }
            else
                processed = false;

            if (!processed)
                throw new Exception("Cannot find target for total label. Set-ChartTotalLabel cmdlet can be used for chart's pane (only for 2D XY charts) or 2D pie series.");
        }

        public virtual void SetupXtraChartLabel(TotalLabel label)
        {
            label.EnableAntialiasing = DefaultBoolean.True;
            label.Visible = true;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                label.BackColor = backColor;
            var borderColor = Utils.ColorFromString(BorderColor);
            if (borderColor != Color.Empty)
            {
                label.Border.Color = borderColor;
                label.Border.Visibility = DefaultBoolean.True;
            }
            if (BorderThickness.HasValue)
            {
                label.Border.Thickness  = BorderThickness.Value;
                label.Border.Visibility = DefaultBoolean.True;
            }
            var font = Utils.StringToFont(Font, out Color fontColor);
            if (font != null)
                label.Font = font;
            if (fontColor != Color.Empty)
                label.TextColor = fontColor;
            if (MaxLineCount.HasValue)
                label.MaxLineCount = MaxLineCount.Value;
            if (MaxWidth.HasValue)
                label.MaxWidth = MaxWidth.Value;
            var shadowColor = Utils.ColorFromString(ShadowColor);
            if (shadowColor != Color.Empty)
            {
                label.Shadow.Color   = shadowColor;
                label.Shadow.Visible = true;
            }
            if (ShadowSize.HasValue)
            {
                label.Shadow.Size    = ShadowSize.Value;
                label.Shadow.Visible = true;
            }
            label.TextAlignment = TextAlignment;
            if (!string.IsNullOrWhiteSpace(TextPattern))
                label.TextPattern = TextPattern;

            if (label is StackedBarTotalLabel stackedBarLabel)
            {
                if (ResolveOverlappingMinIndent.HasValue)
                stackedBarLabel.ResolveOverlappingMinIndent = ResolveOverlappingMinIndent.Value;
                stackedBarLabel.ResolveOverlappingMode = ResolveOverlappingMode;

                if (ConnectorDashStyle.HasValue)
                {
                    stackedBarLabel.ConnectorLineStyle.DashStyle = ConnectorDashStyle.Value;
                    stackedBarLabel.ShowConnector = true;
                }
                if (ConnectorLineJoin.HasValue)
                {
                    stackedBarLabel.ConnectorLineStyle.LineJoin = ConnectorLineJoin.Value;
                    stackedBarLabel.ShowConnector = true;
                }
                if (ConnectorThickness.HasValue)
                {
                    stackedBarLabel.ConnectorLineStyle.Thickness = ConnectorThickness.Value;
                    stackedBarLabel.ShowConnector = true;
                }
                if (ShowConnector)
                    stackedBarLabel.ShowConnector = true;
                if (Indent.HasValue)
                    stackedBarLabel.Indent = Indent.Value;
            }

            if (FillMode.HasValue)
            {
                label.FillStyle.FillMode = FillMode.Value;
                switch (FillMode.Value)
                {
                    case DevExpress.XtraCharts.FillMode.Empty:
                        break;
                    case DevExpress.XtraCharts.FillMode.Solid:
                        break;
                    case DevExpress.XtraCharts.FillMode.Gradient:
                        if (label.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = FillGradientMode.Value;
                        }
                        break;
                    case DevExpress.XtraCharts.FillMode.Hatch:
                        if (label.FillStyle.Options is HatchFillOptions hatchOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != Color.Empty)
                                hatchOptions.Color2 = backColor2;
                            if (FillHatchStyle.HasValue)
                                hatchOptions.HatchStyle = FillHatchStyle.Value;
                        }
                        break;
                }
            }
        }
    }
}
