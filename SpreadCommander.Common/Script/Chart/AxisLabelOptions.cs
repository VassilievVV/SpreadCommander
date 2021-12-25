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
    public class AxisLabelOptions
    {
        [Description("Rotation angle of axis labels in degrees.")]
        public int? Angle { get; set; }

        [Description("Background color for an axis label.")]
        public string BackColor { get; set; }

        [Description("Label's border color.")]
        public string BorderColor { get; set; }

        [Description("Label's border thickness, in pixels.")]
        public int? BorderThickness { get; set; }

        [Description("Whether the label's border is visible.")]
        public bool? BorderVisible { get; set; }

        [Description("Label's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Label's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Font used to display the label's text.")]
        public string Font { get; set; }

        [Description("Number of lines to which a label's text is allowed to wrap.")]
        public int? MaxLineCount { get; set; }

        [Description("Maximum width allowed for axis labels.")]
        public int? MaxWidth { get; set; }

        [Description("Whether axis labels are positioned in staggered order.")]
        public bool? Staggered { get; set; }

        [Description("Alignment of the axis labels text.")]
        public StringAlignment? TextAlignment { get; set; }

        [Description("Pattern specifying the text to be displayed within the axis label that appears for a diagram X - axis or Y- axis.")]
        public string TextPattern { get; set; }

        [Description("Whether label is visible.")]
        public bool? Visible { get; set; }

        [Description("Do not allow auto-hide axis labels.")]
        [DefaultValue(true)]
        public bool AutoHide { get; set; } = true;

        [Description("Do not allow auto-rotate axis labels.")]
        [DefaultValue(true)]
        public bool AutoRotate { get; set; } = true;

        [Description("Do not allow auto-stagger axis labels.")]
        [DefaultValue(true)]
        public bool AutoStagger { get; set; } = true;

        [Description("Minimum indent between adjacent axis labels, when an overlap resolution algorithm is applied to them.")]
        public int? MinIndent { get; set; }


        protected internal virtual void SetupXtraChartAxisLabel(SCChart chart, ChartAxisType axisType, string axisName)
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

            var label = axis.Label;

            label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

            if (Angle.HasValue)
                label.Angle = Angle.Value;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                label.BackColor = backColor;

            var borderColor = Utils.ColorFromString(BorderColor);
            if (borderColor != Color.Empty)
            {
                label.Border.Color = borderColor;
                label.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }
            if (BorderThickness.HasValue)
            {
                label.Border.Thickness  = BorderThickness.Value;
                label.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }
            if (BorderVisible.HasValue)
                label.Border.Visibility = BorderVisible.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.Default;

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
                            if (backColor2 != System.Drawing.Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                        }
                        break;
                    case Chart.FillMode.Hatch:
                        if (label.FillStyle.Options is HatchFillOptions hatchOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                hatchOptions.Color2 = backColor2;
                            if (FillHatchStyle.HasValue)
                                hatchOptions.HatchStyle = FillHatchStyle.Value;
                        }
                        break;
                }
            }

            var font = Utils.StringToFont(Font, out Color textColor);
            if (font != null)
                label.Font = font;
            if (textColor != Color.Empty)
                label.TextColor = textColor;

            if (MaxLineCount.HasValue)
                label.MaxLineCount = MaxLineCount.Value;
            if (MaxWidth.HasValue)
                label.MaxWidth = MaxWidth.Value;
            if (Staggered.HasValue)
                label.Staggered = Staggered.Value;
            if (TextAlignment.HasValue)
                label.TextAlignment = TextAlignment.Value;
            if (!string.IsNullOrWhiteSpace(TextPattern))
                label.TextPattern = TextPattern;
            if (Visible.HasValue)
                label.Visible = Visible.Value;
            label.ResolveOverlappingOptions.AllowHide = AutoHide;
            label.ResolveOverlappingOptions.AllowRotate = AutoRotate;
            label.ResolveOverlappingOptions.AllowStagger = AutoStagger;
            if (MinIndent.HasValue)
                label.ResolveOverlappingOptions.MinIndent = MinIndent.Value;
        }
    }

    public partial class SCChart
    {
        public SCChart SetAxisLabel(ChartAxisType axisType, string axisName, AxisLabelOptions options = null)
        {
            options ??= new AxisLabelOptions();
            options.SetupXtraChartAxisLabel(this, axisType, axisName);

            return this;
        }
    }
}
