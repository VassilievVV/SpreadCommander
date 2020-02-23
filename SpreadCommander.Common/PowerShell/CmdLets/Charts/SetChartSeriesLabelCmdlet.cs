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
    [Cmdlet(VerbsCommon.Set, "ChartSeriesLabel")]
    public class SetChartSeriesLabelCmdlet: BaseChartWithContextCmdlet
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

        [Parameter(HelpMessage = "Color of series labels' connection lines.")]
        public string LineColor { get; set; }

        [Parameter(HelpMessage = "Length of series labels' connection lines.")]
        public int? LineLength { get; set; }

        [Parameter(HelpMessage = "Whether to show connection lines for a series labels.")]
        public bool? LineVisibility { get; set; }

        [Parameter(HelpMessage = "Number of lines to which a label's text is allowed to wrap. Value in range 0 (no limit) to 20.")]
        public int? MaxLineCount { get; set; }

        [Parameter(HelpMessage = "Maximum width allowed for series labels.")]
        public int? MaxWidth { get; set; }

        [Parameter(HelpMessage = "Minimum indent between adjacent series labels, when an overlapping resolving algorithm is applied to them.")]
        public int? ResolveOverlappingMinIndent { get; set; }

        [Parameter(HelpMessage = "Mode to resolve overlapping of series labels.")]
        public ResolveOverlappingMode? ResolveOverlappingMode { get; set; }

        [Parameter(HelpMessage = "Shadow's color.")]
        public string ShadowColor { get; set; }

        [Parameter(HelpMessage = "Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Parameter(HelpMessage = "Alignment of the series labels text.")]
        public StringAlignment? TextAlignment { get; set; }

        [Parameter(HelpMessage = "Direction of text within the series labels.")]
        public TextOrientation? TextOrientation { get; set; }

        [Parameter(HelpMessage = "Pattern specifying the text to be displayed within series labels.")]
        public string TextPattern { get; set; }

        [Parameter(HelpMessage = "The indent between labels and the corresponding Bar side.")]
        public int? Indent { get; set; }

        [Parameter(HelpMessage = "Angle which controls the position of data point labels.")]
        public int? Angle { get; set; }

        [Parameter(HelpMessage = "Kind of labels in 2D Range Area and 3D Range Area series.")]
        public RangeAreaLabelKind? RangeAreaLabelKind {get; set;}

        [Parameter(HelpMessage = "Angle between the point line and the label, which shows the maximum value of a series point in a Range Area series view.")]
        public int? RangeAreaMaxValueAngle { get; set; }

        [Parameter(HelpMessage = "Angle between the point line and the label, which shows the minimum value of a series point in a Range Area series view.")]
        public int? RangeAreaMinValueAngle { get; set; }

        [Parameter(HelpMessage = "Kind of labels in Side-by-Side Range Bar, Overlapped Range Bar and Gantt series.")]
        public RangeBarLabelKind? RangeBarLabelKind { get; set; }

        [Parameter(HelpMessage = "Position of series labels relative to the corresponding range bars.")]
        public RangeBarLabelPosition? RangeBarLabelPosition { get; set; }

        [Parameter(HelpMessage = "Hide series labels.")]
        [Alias("NoLabels")]
        public SwitchParameter HideLabels { get; set; }

        [Parameter(HelpMessage = "Show series labels.")]
        public SwitchParameter ShowLabels { get; set; }


        protected override void UpdateChart()
        {
            if (ChartContext.CurrentSeries != null)
                SetupXtraChartLabel(ChartContext.CurrentSeries, ChartContext.CurrentSeries.Label);
            else if (ChartContext.Chart != null)
            {
                //Series were added in QuickMode, without special cmdlet, so update all series.
                foreach (Series series in ChartContext.Chart.Series)
                    SetupXtraChartLabel(series, series.Label);
            }				
        }

        public virtual void SetupXtraChartLabel(Series series, SeriesLabelBase label)
        {
            label.EnableAntialiasing = DefaultBoolean.True;

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
            var lineColor = Utils.ColorFromString(LineColor);
            if (lineColor != Color.Empty)
                label.LineColor = lineColor;
            if (LineLength.HasValue)
                label.LineLength = LineLength.Value;
            if (LineVisibility.HasValue)
                label.LineVisibility = LineVisibility.Value ? DefaultBoolean.True : DefaultBoolean.False;
            if (MaxLineCount.HasValue)
                label.MaxLineCount = MaxLineCount.Value;
            if (MaxWidth.HasValue)
                label.MaxWidth = MaxWidth.Value;
            if (ResolveOverlappingMinIndent.HasValue)
                label.ResolveOverlappingMinIndent = ResolveOverlappingMinIndent.Value;
            if (ResolveOverlappingMode.HasValue)
                label.ResolveOverlappingMode = ResolveOverlappingMode.Value;
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
            if (TextAlignment.HasValue)
                label.TextAlignment = TextAlignment.Value;
            if (TextOrientation.HasValue)
                label.TextOrientation = TextOrientation.Value;
            if (TextPattern != null)    //allow empty string pattern
                label.TextPattern = TextPattern;

            if (HideLabels)
                series.LabelsVisibility = DefaultBoolean.False;
            if (ShowLabels)
                series.LabelsVisibility = DefaultBoolean.True;

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


            if (label is BarSeriesLabel labelBar)
            {
                if (Indent.HasValue)
                    labelBar.Indent = Indent.Value;
            }

            if (label is PointSeriesLabel labelPoint)
            {
                if (Angle.HasValue)
                    labelPoint.Angle = Angle.Value;
            }

            if (label is RangeAreaSeriesLabel labelRangeArea)
            {
                if (RangeAreaLabelKind.HasValue)
                labelRangeArea.Kind = RangeAreaLabelKind.Value;
                if (RangeAreaMaxValueAngle.HasValue)
                    labelRangeArea.MaxValueAngle = RangeAreaMaxValueAngle.Value;
                if (RangeAreaMinValueAngle.HasValue)
                    labelRangeArea.MinValueAngle = RangeAreaMinValueAngle.Value;
            }

            if (label is RangeBarSeriesLabel labelRangeBar)
            {
                if (RangeBarLabelKind.HasValue)
                    labelRangeBar.Kind = RangeBarLabelKind.Value;
                if (RangeBarLabelPosition.HasValue)
                    labelRangeBar.Position = RangeBarLabelPosition.Value;
            }
        }
    }
}
