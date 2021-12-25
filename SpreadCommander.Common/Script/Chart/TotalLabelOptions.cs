using DevExpress.Utils;
using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class TotalLabelOptions
    {
        [Description("Label's background color.")]
        public string BackColor { get; set; }

        [Description("Label's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Label's border color.")]
        public string BorderColor { get; set; }

        [Description("Border's thickness")]
        public int? BorderThickness { get; set; }

        [Description("Label's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Font used to display the legend's contents.")]
        public string Font { get; set; }

        [Description("Number of lines to which a label's text is allowed to wrap. Value in range 0 (no limit) to 20.")]
        public int? MaxLineCount { get; set; }

        [Description("Maximum width allowed for series labels.")]
        public int? MaxWidth { get; set; }

        [Description("Minimum indent between adjacent series labels, when an overlapping resolving algorithm is applied to them.")]
        public int? ResolveOverlappingMinIndent { get; set; }

        [Description("Mode to resolve overlapping of series labels.")]
        [DefaultValue(ResolveOverlappingMode.Default)]
        public ResolveOverlappingMode ResolveOverlappingMode { get; set; } = ResolveOverlappingMode.Default;

        [Description("Shadow's color.")]
        public string ShadowColor { get; set; }

        [Description("Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Description("Alignment of the series labels text.")]
        public System.Drawing.StringAlignment TextAlignment { get; set; }

        [Description("Pattern specifying the text to be displayed within series labels.")]
        public string TextPattern { get; set; }

        [Description("Dash style used to paint the connector line. For use with stacked bar total label.")]
        public DevExpress.XtraCharts.DashStyle? ConnectorDashStyle { get; set; }

        [Description("Join style for the ends of consecutive connector lines. For use with stacked bar total label.")]
        public LineJoin? ConnectorLineJoin { get; set; }

        [Description("Connector line's thickness. For use with stacked bar total label.")]
        public int? ConnectorThickness { get; set; }

        [Description("Whether total labels' connectors should be visible. For use with stacked bar total label.")]
        public bool ShowConnector { get; set; }

        [Description("Distance between a series group's top edge and the group's total label. For use with stacked bar total label.")]
        public int? Indent { get; set; }


        protected internal virtual void SetupXtraChartTotalLabel(SCChart chart, TotalLabel label)
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
                stackedBarLabel.ResolveOverlappingMode      = (DevExpress.XtraCharts.ResolveOverlappingMode)ResolveOverlappingMode;

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
                label.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)FillMode.Value;
                switch (FillMode.Value)
                {
                    case Chart.FillMode.Empty:
                        break;
                    case Chart.FillMode.Solid:
                        break;
                    case Chart.FillMode.Gradient:
                        if (label.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                        }
                        break;
                    case Chart.FillMode.Hatch:
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

    public partial class SCChart
    {
        public SCChart SetStackedBarTotalLabel(string paneName, TotalLabelOptions options = null)
        {
            if (Chart.Diagram is not XYDiagram diagramXY)
                throw new Exception("Total label can be set only for 2D XY diagrams.");

            XYDiagramPaneBase pane;
            if (!string.IsNullOrWhiteSpace(paneName))
                pane = diagramXY.Panes[paneName];
            else
                pane = diagramXY.DefaultPane;

            options ??= new TotalLabelOptions();
            options.SetupXtraChartTotalLabel(this, pane.StackedBarTotalLabel);

            return this;
        }

        public SCChart SetPieSeriesTotalLabel(string seriesName, TotalLabelOptions options = null)
        {
            DevExpress.XtraCharts.Series series;
            if (string.IsNullOrWhiteSpace(seriesName))
                series = CurrentSeries;
            else
                series = Chart.Series[seriesName];
            if (series == null)
                throw new Exception($"Cannot find series '{seriesName}'.");

            if (series.View is not PieSeriesView pieSeriesView)
                throw new Exception($"Series '{seriesName} is not Pie series.");

            options ??= new TotalLabelOptions();
            options.SetupXtraChartTotalLabel(this, pieSeriesView.TotalLabel);

            return this;
        }
    }
}
