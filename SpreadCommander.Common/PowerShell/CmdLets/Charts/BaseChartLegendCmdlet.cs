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
    public class BaseChartLegendCmdlet : BaseChartWithContextCmdlet
    {
        [Parameter(HelpMessage = "Legend's horizontal alignment within the chart control.")]
        [PSDefaultValue(Value = LegendAlignmentHorizontal.RightOutside)]
        [DefaultValue(LegendAlignmentHorizontal.RightOutside)]
        public LegendAlignmentHorizontal AlignmentHorizontal { get; set; } = LegendAlignmentHorizontal.RightOutside;

        [Parameter(HelpMessage = "Legend's vertical alignment within the chart control.")]
        [PSDefaultValue(Value = LegendAlignmentVertical.Top)]
        [DefaultValue(LegendAlignmentVertical.Top)]
        public LegendAlignmentVertical AlignmentVertical { get; set; } = LegendAlignmentVertical.Top;

        [Parameter(HelpMessage = "Legend's background color.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Legend's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Parameter(HelpMessage = "Legend's border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Border's thickness")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Direction in which the names of the series are displayed within the legend.")]
        public LegendDirection? Direction { get; set; }

        [Parameter(HelpMessage = "Legend's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Font used to display the legend's contents.")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "Indent between the legend's edge and other chart elements (e.g. diagram and chart titles), in pixels.")]
        public int[] Margins { get; set; }

        [Parameter(HelpMessage = "Whether markers in legend should be visible.")]
        public SwitchParameter HideMarkers { get; set; }

        [Parameter(HelpMessage = "Offset between a marker and check box, when both are displayed in the legend.")]
        [PSDefaultValue(Value = 2)]
        [DefaultValue(2)]
        public int MarkerOffset { get; set; } = 2;

        [Parameter(HelpMessage = "Marker's height and width.")]
        public Size? MarkerSize { get; set; }

        [Parameter(HelpMessage = "Whether the legend markers are visible.")]
        public bool MarkerVisible { get; set; } = true;

        [Parameter(HelpMessage = "Legend's maximum possible width (in percents).")]
        [PSDefaultValue(Value = 100)]
        [DefaultValue(100)]
        public double MaxHorizontalPercentage { get; set; } = 100;

        [Parameter(HelpMessage = "Legend's maximum possible height (in percents).")]
        [PSDefaultValue(Value = 100)]
        [DefaultValue(100)]
        public double MaxVerticalPercentage { get; set; } = 100;

        [Parameter(HelpMessage = "Internal space between the legend's content and its edge, in pixels.")]
        public int[] Padding { get; set; }

        [Parameter(HelpMessage = "Shadow's color.")]
        public string ShadowColor { get; set; }

        [Parameter(HelpMessage = "Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Parameter(HelpMessage = "Indent between legend markers and their text.")]
        [PSDefaultValue(Value = 2)]
        [DefaultValue(2)]
        public int? TextOffset { get; set; } = 2;

        [Parameter(HelpMessage = "Title's alignment")]
        public StringAlignment? TitleAlignment { get; set; }

        [Parameter(HelpMessage = "Title's font")]
        public string TitleFont { get; set; }

        [Parameter(HelpMessage = "Title's margins")]
        public int[] TitleMargins { get; set; }

        [Parameter(HelpMessage = "Title. Simple HTML formatting is supported.")]
        public string TitleText { get; set; }

        [Parameter(HelpMessage = "Whether a title's text should wrap when it's too lengthy.")]
        public SwitchParameter TitleWordWrap { get; set; }

        [Parameter(HelpMessage = "Horizontal indent between legend items.")]
        public int? HorizontalIndent { get; set; }

        [Parameter(HelpMessage = "Vertical indent between legend items.")]
        public int? VerticalIndent { get; set; }

        [Parameter(HelpMessage = "Whether to show the legend on a chart.")]
        public bool? Visibility { get; set; }


        public void SetupXtraChartLegend(Legend legend)
        {
            legend.EnableAntialiasing = DefaultBoolean.True;
            legend.EquallySpacedItems = true;

            legend.AlignmentHorizontal = AlignmentHorizontal;
            legend.AlignmentVertical = AlignmentVertical;
            legend.MarkerMode = HideMarkers ? LegendMarkerMode.None : LegendMarkerMode.Marker;
            legend.MarkerOffset = MarkerOffset;
            legend.MaxHorizontalPercentage = MaxHorizontalPercentage;
            legend.MaxVerticalPercentage = MaxVerticalPercentage;

            if (Direction.HasValue)
                legend.Direction = Direction.Value;
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
                legend.Border.Color = borderColor;
                legend.Border.Visibility = DefaultBoolean.True;
            }
            if (BorderThickness.HasValue)
            {
                legend.Border.Thickness = BorderThickness.Value;
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
                legend.FillStyle.FillMode = FillMode.Value;
                switch (FillMode.Value)
                {
                    case DevExpress.XtraCharts.FillMode.Empty:
                        break;
                    case DevExpress.XtraCharts.FillMode.Solid:
                        break;
                    case DevExpress.XtraCharts.FillMode.Gradient:
                        if (legend.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = FillGradientMode.Value;
                        }
                        break;
                    case DevExpress.XtraCharts.FillMode.Hatch:
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
}
