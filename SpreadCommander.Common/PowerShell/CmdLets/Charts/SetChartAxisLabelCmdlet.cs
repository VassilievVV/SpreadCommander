using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommon.Set, "ChartAxisLabel")]
    public class SetChartAxisLabelCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Axis type - X or Y.")]
        public ChartAxisType AxisType { get; set; }

        [Parameter(Position = 1, HelpMessage = "Name of the axis.")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "Rotation angle of axis labels in degrees.")]
        public int? Angle { get; set; }

        [Parameter(HelpMessage = "Background color for an axis label.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Label's border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Label's border thickness, in pixels.")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Whether the label's border is visible.")]
        public bool? BorderVisible { get; set; }

        [Parameter(HelpMessage = "Label's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Parameter(HelpMessage = "Label's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Font used to display the label's text.")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "Number of lines to which a label's text is allowed to wrap.")]
        public int? MaxLineCount { get; set; }

        [Parameter(HelpMessage = "Maximum width allowed for axis labels.")]
        public int? MaxWidth { get; set; }

        [Parameter(HelpMessage = "Whether axis labels are positioned in staggered order.")]
        public bool? Staggered { get; set; }

        [Parameter(HelpMessage = "Alignment of the axis labels text.")]
        public StringAlignment? TextAlignment { get; set; }

        [Parameter(HelpMessage = "Pattern specifying the text to be displayed within the axis label that appears for a diagram X - axis or Y- axis.")]
        public string TextPattern { get; set; }

        [Parameter(HelpMessage = "Whether label is visible.")]
        public bool? Visible { get; set; }

        [Parameter(HelpMessage = "Do not allow auto-hide axis labels.")]
        public SwitchParameter DoNotAutoHide { get; set; }

        [Parameter(HelpMessage = "Do not allow auto-rotate axis labels.")]
        public SwitchParameter DoNotAutoRotate { get; set; }

        [Parameter(HelpMessage = "Do not allow auto-stagger axis labels.")]
        public SwitchParameter DoNotAutoStagger { get; set; }

        [Parameter(HelpMessage = "Minimum indent between adjacent axis labels, when an overlap resolution algorithm is applied to them.")]
        public int? MinIndent { get; set; }


        protected override void UpdateChart()
        {
            AxisBase axis;

            if (!string.IsNullOrWhiteSpace(Name))
            {
                axis = BaseAxisCmdlet.GetSecondaryAxis(ChartContext.Chart.Diagram, AxisType, Name);
                if (axis == null)
                    throw new Exception($"Cannot find axis '{Name}'.");
            }
            else 
            {
                axis = BaseAxisCmdlet.GetPrimaryAxis(ChartContext.Chart.Diagram, AxisType);
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
                            if (backColor2 != System.Drawing.Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = FillGradientMode.Value;
                        }
                        break;
                    case DevExpress.XtraCharts.FillMode.Hatch:
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
            if (DoNotAutoHide)
                label.ResolveOverlappingOptions.AllowHide = false;
            if (DoNotAutoRotate)
                label.ResolveOverlappingOptions.AllowRotate = false;
            if (DoNotAutoStagger)
                label.ResolveOverlappingOptions.AllowStagger = false;
            if (MinIndent.HasValue)
                label.ResolveOverlappingOptions.MinIndent = MinIndent.Value;
        }
    }
}
