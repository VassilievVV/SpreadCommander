using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class AxisCustomLabelOptions
    {
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

        [Description("Whether to show a grid line for the custom axis label.")]
        public bool ShowGridLine { get; set; }


        protected internal virtual void SetupXtraChartCustomLabel(SCChart chart,
            ChartAxisType axisType, string axisName, string name, object value)
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

            if (axis is not Axis2D axis2D)
                throw new Exception("Only 2D axis support custom labels.");

            var label = new CustomAxisLabel();

            if (!string.IsNullOrWhiteSpace(name))
                label.Name = name;

            label.AxisValue = value;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != System.Drawing.Color.Empty)
                label.BackColor = backColor;

            var borderColor = Utils.ColorFromString(BorderColor);
            if (borderColor != System.Drawing.Color.Empty)
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

            var font = Utils.StringToFont(Font, out System.Drawing.Color textColor);
            if (font != null)
                label.Font = font;
            if (textColor != System.Drawing.Color.Empty)
                label.TextColor = textColor;

            if (ShowGridLine)
                label.GridLineVisible = true;

            label.Visible = true;


            axis2D.CustomLabels.Add(label);
        }
    }

    public partial class SCChart
    {
        public SCChart AddAxisCustomLabel(ChartAxisType axisType, string name, object value,
            AxisCustomLabelOptions options = null) =>
            AddAxisCustomLabel(axisType, null, name, value, options);

        public SCChart AddAxisCustomLabel(ChartAxisType axisType, string axisName, string name, object value,
            AxisCustomLabelOptions options = null)
        {
            options ??= new AxisCustomLabelOptions();
            options.SetupXtraChartCustomLabel(this, axisType, axisName, name, value);

            return this;
        }
    }
}
