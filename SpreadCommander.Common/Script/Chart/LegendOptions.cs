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
    public class LegendOptions
    {
        [Description("Legend's horizontal alignment within the chart control.")]
        [DefaultValue(LegendAlignmentHorizontal.RightOutside)]
        public LegendAlignmentHorizontal AlignmentHorizontal { get; set; } = LegendAlignmentHorizontal.RightOutside;

        [Description("Legend's vertical alignment within the chart control.")]
        [DefaultValue(LegendAlignmentVertical.Top)]
        public LegendAlignmentVertical AlignmentVertical { get; set; } = LegendAlignmentVertical.Top;

        [Description("Legend's background color.")]
        public string BackColor { get; set; }

        [Description("Legend's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Legend's border color.")]
        public string BorderColor { get; set; }

        [Description("Border's thickness")]
        public int? BorderThickness { get; set; }

        [Description("Direction in which the names of the series are displayed within the legend.")]
        public LegendDirection? Direction { get; set; }

        [Description("Legend's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Font used to display the legend's contents.")]
        public string Font { get; set; }

        [Description("Indent between the legend's edge and other chart elements (e.g. diagram and chart titles), in pixels.")]
        public int[] Margins { get; set; }

        [Description("Whether markers in legend should be visible.")]
        public bool HideMarkers { get; set; }

        [Description("Offset between a marker and check box, when both are displayed in the legend.")]
        [DefaultValue(2)]
        public int MarkerOffset { get; set; } = 2;

        [Description("Marker's height and width.")]
        public Size? MarkerSize { get; set; }

        [Description("Whether the legend markers are visible.")]
        public bool MarkerVisible { get; set; } = true;

        [Description("Legend's maximum possible width (in percents).")]
        [DefaultValue(100)]
        public double MaxHorizontalPercentage { get; set; } = 100;

        [Description("Legend's maximum possible height (in percents).")]
        [DefaultValue(100)]
        public double MaxVerticalPercentage { get; set; } = 100;

        [Description("Internal space between the legend's content and its edge, in pixels.")]
        public int[] Padding { get; set; }

        [Description("Shadow's color.")]
        public string ShadowColor { get; set; }

        [Description("Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Description("Indent between legend markers and their text.")]
        [DefaultValue(2)]
        public int? TextOffset { get; set; } = 2;

        [Description("Title's alignment")]
        public StringAlignment? TitleAlignment { get; set; }

        [Description("Title's font")]
        public string TitleFont { get; set; }

        [Description("Title's margins")]
        public int[] TitleMargins { get; set; }

        [Description("Title. Simple HTML formatting is supported.")]
        public string TitleText { get; set; }

        [Description("Whether a title's text should wrap when it's too lengthy.")]
        public bool TitleWordWrap { get; set; }

        [Description("Horizontal indent between legend items.")]
        public int? HorizontalIndent { get; set; }

        [Description("Vertical indent between legend items.")]
        public int? VerticalIndent { get; set; }

        [Description("Whether to show the legend on a chart.")]
        public bool? Visibility { get; set; }


        protected internal virtual void SetupXtraChartLegend(SCChart chart, Legend legend)
        {
            legend.EnableAntialiasing = DefaultBoolean.True;

            legend.AlignmentHorizontal     = (DevExpress.XtraCharts.LegendAlignmentHorizontal)AlignmentHorizontal;
            legend.AlignmentVertical       = (DevExpress.XtraCharts.LegendAlignmentVertical)AlignmentVertical;
            legend.MarkerMode              = HideMarkers ? LegendMarkerMode.None : LegendMarkerMode.Marker;
            legend.MarkerOffset            = MarkerOffset;
            legend.MaxHorizontalPercentage = MaxHorizontalPercentage;
            legend.MaxVerticalPercentage   = MaxVerticalPercentage;

            if (Direction.HasValue)
                legend.Direction = (DevExpress.XtraCharts.LegendDirection)Direction.Value;
            if (legend.Direction == DevExpress.XtraCharts.LegendDirection.RightToLeft || legend.Direction == DevExpress.XtraCharts.LegendDirection.LeftToRight)
                legend.EquallySpacedItems = true;

            if (HorizontalIndent.HasValue)
                legend.HorizontalIndent = HorizontalIndent.Value;
            if (MarkerSize.HasValue)
                legend.MarkerSize = MarkerSize.Value;
            if (TextOffset.HasValue)
                legend.TextOffset = TextOffset.Value;
            if (VerticalIndent.HasValue)
                legend.VerticalIndent = VerticalIndent.Value;

            if (Visibility.HasValue)
                legend.Visibility = Visibility.Value ? DefaultBoolean.True : DefaultBoolean.False;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                legend.BackColor = backColor;
            var borderColor = Utils.ColorFromString(BorderColor);
            if (borderColor != Color.Empty)
            {
                legend.Border.Color      = borderColor;
                legend.Border.Visibility = DefaultBoolean.True;
            }
            if (BorderThickness.HasValue)
            {
                legend.Border.Thickness  = BorderThickness.Value;
                legend.Border.Visibility = DefaultBoolean.True;
            }
            var font = Utils.StringToFont(Font, out Color fontColor);
            if (font != null)
                legend.Font = font;
            if (fontColor != Color.Empty)
                legend.TextColor = fontColor;
            var shadowColor = Utils.ColorFromString(ShadowColor);
            if (shadowColor != Color.Empty)
            {
                legend.Shadow.Color = shadowColor;
                legend.Shadow.Visible = true;
            }
            if (ShadowSize.HasValue)
            {
                legend.Shadow.Size = ShadowSize.Value;
                legend.Shadow.Visible = true;
            }

            if (FillMode.HasValue)
            {
                legend.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)FillMode.Value;
                switch (FillMode.Value)
                {
                    case Chart.FillMode.Empty:
                        break;
                    case Chart.FillMode.Solid:
                        break;
                    case Chart.FillMode.Gradient:
                        if (legend.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                        }
                        break;
                    case Chart.FillMode.Hatch:
                        if (legend.FillStyle.Options is HatchFillOptions hatchOptions)
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

            if (TitleAlignment.HasValue)
                legend.Title.Alignment = TitleAlignment.Value;
            var titleFont = Utils.StringToFont(TitleFont, out Color titleColor);
            if (titleFont != null)
                legend.Title.Font = titleFont;
            if (titleColor != Color.Empty)
                legend.Title.TextColor = titleColor;
            legend.Title.WordWrap = TitleWordWrap;
            if (TitleMargins != null && TitleMargins.Length == 1)
                legend.Title.Margins.All = TitleMargins[0];
            else if (TitleMargins != null && TitleMargins.Length == 4)
            {
                legend.Title.Margins.Left = TitleMargins[0];
                legend.Title.Margins.Top = TitleMargins[1];
                legend.Title.Margins.Right = TitleMargins[2];
                legend.Title.Margins.Bottom = TitleMargins[3];
            }
            if (!string.IsNullOrEmpty(TitleText))
            {
                legend.Title.Text = TitleText;
                legend.Title.Visible = true;
            }

            if (Margins != null && Margins.Length == 1)
                legend.Margins.All = Margins[0];
            else if (Margins != null && Margins.Length == 4)
            {
                legend.Margins.Left = Margins[0];
                legend.Margins.Top = Margins[1];
                legend.Margins.Right = Margins[2];
                legend.Margins.Bottom = Margins[3];
            }
            if (Padding != null && Padding.Length == 1)
                legend.Padding.All = Padding[0];
            else if (Padding != null && Padding.Length == 4)
            {
                legend.Padding.Left = Padding[0];
                legend.Padding.Top = Padding[1];
                legend.Padding.Right = Padding[2];
                legend.Padding.Bottom = Padding[3];
            }
            else if (Padding != null)
                throw new Exception("Invalid padding. Padding shall be an array with 1 or 4 integer values.");
        }
    }

    public partial class SCChart
    {
        public SCChart AddLegend(string name, LegendOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Legend name cannot be empty.");

            options ??= new LegendOptions();

            var legend = new Legend(name);
            Chart.Legends.Add(legend);

            options.SetupXtraChartLegend(this, legend);

            return this;
        }

        public SCChart SetLegend(string name = null, LegendOptions options = null)
        {
            options ??= new LegendOptions();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var legend = Chart.Legends[name];
                if (legend == null)
                    throw new Exception($"Cannot find legend '{name}'.");

                options.SetupXtraChartLegend(this, legend);
            }
            else
                options.SetupXtraChartLegend(this, Chart.Legend);

            return this;
        }
    }
}
