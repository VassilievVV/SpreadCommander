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
    public class SeriesLabelOptions
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

        [Description("Color of series labels' connection lines.")]
        public string LineColor { get; set; }

        [Description("Length of series labels' connection lines.")]
        public int? LineLength { get; set; }

        [Description("Whether to show connection lines for a series labels.")]
        public bool? LineVisibility { get; set; }

        [Description("Number of lines to which a label's text is allowed to wrap. Value in range 0 (no limit) to 20.")]
        public int? MaxLineCount { get; set; }

        [Description("Maximum width allowed for series labels.")]
        public int? MaxWidth { get; set; }

        [Description("Minimum indent between adjacent series labels, when an overlapping resolving algorithm is applied to them.")]
        public int? ResolveOverlappingMinIndent { get; set; }

        [Description("Mode to resolve overlapping of series labels.")]
        public ResolveOverlappingMode? ResolveOverlappingMode { get; set; }

        [Description("Shadow's color.")]
        public string ShadowColor { get; set; }

        [Description("Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Description("Alignment of the series labels text.")]
        public StringAlignment? TextAlignment { get; set; }

        [Description("Direction of text within the series labels.")]
        public TextOrientation? TextOrientation { get; set; }

        [Description("Pattern specifying the text to be displayed within series labels.")]
        public string TextPattern { get; set; }

        [Description("The indent between labels and the corresponding Bar side.")]
        public int? Indent { get; set; }

        [Description("Angle which controls the position of data point labels.")]
        public int? Angle { get; set; }

        [Description("Kind of labels in 2D Range Area and 3D Range Area series.")]
        public RangeAreaLabelKind? RangeAreaLabelKind {get; set;}

        [Description("Angle between the point line and the label, which shows the maximum value of a series point in a Range Area series view.")]
        public int? RangeAreaMaxValueAngle { get; set; }

        [Description("Angle between the point line and the label, which shows the minimum value of a series point in a Range Area series view.")]
        public int? RangeAreaMinValueAngle { get; set; }

        [Description("Kind of labels in Side-by-Side Range Bar, Overlapped Range Bar and Gantt series.")]
        public RangeBarLabelKind? RangeBarLabelKind { get; set; }

        [Description("Position of series labels relative to the corresponding range bars.")]
        public RangeBarLabelPosition? RangeBarLabelPosition { get; set; }

        [Description("Whether to show series labels.")]
        public bool? ShowLabels { get; set; }


        protected internal virtual void SetupXtraChartSeriesLabel(SCChart chart, DevExpress.XtraCharts.Series series)
        {
            var label = series.Label;

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
                label.ResolveOverlappingMode = (DevExpress.XtraCharts.ResolveOverlappingMode)ResolveOverlappingMode.Value;
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
                label.TextOrientation = (DevExpress.XtraCharts.TextOrientation)TextOrientation.Value;
            if (TextPattern != null)    //allow empty string pattern
                label.TextPattern = TextPattern;

            if (ShowLabels.HasValue)
                series.LabelsVisibility = ShowLabels.Value ? DefaultBoolean.True : DefaultBoolean.False;

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
                labelRangeArea.Kind = (DevExpress.XtraCharts.RangeAreaLabelKind)RangeAreaLabelKind.Value;
                if (RangeAreaMaxValueAngle.HasValue)
                    labelRangeArea.MaxValueAngle = RangeAreaMaxValueAngle.Value;
                if (RangeAreaMinValueAngle.HasValue)
                    labelRangeArea.MinValueAngle = RangeAreaMinValueAngle.Value;
            }

            if (label is RangeBarSeriesLabel labelRangeBar)
            {
                if (RangeBarLabelKind.HasValue)
                    labelRangeBar.Kind = (DevExpress.XtraCharts.RangeBarLabelKind)RangeBarLabelKind.Value;
                if (RangeBarLabelPosition.HasValue)
                    labelRangeBar.Position = (DevExpress.XtraCharts.RangeBarLabelPosition)RangeBarLabelPosition.Value;
            }
        }
    }

    public partial class SCChart
    {
        public SCChart SetSeriesLabel(string seriesName, SeriesLabelOptions options = null)
        {
            options ??= new SeriesLabelOptions();

            DevExpress.XtraCharts.Series series;
            if (string.IsNullOrWhiteSpace(seriesName))
                series = CurrentSeries;
            else
                series = Chart.Series[seriesName];
            if (series == null)
                throw new Exception($"Cannot find series '{seriesName}'.");

            options.SetupXtraChartSeriesLabel(this, series);

            return this;
        }
    }
}
