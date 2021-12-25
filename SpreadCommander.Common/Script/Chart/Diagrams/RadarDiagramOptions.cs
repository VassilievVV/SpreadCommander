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

namespace SpreadCommander.Common.Script.Chart.Diagrams
{
    public class RadarDiagramOptions : DiagramOptions
    {
        [Description("Diagram's background color.")]
        public string DiagramBackColor { get; set; }

        [Description("Diagram's 2nd background color, if FillMode is gradient or hatch.")]
        public string DiagramBackColor2 { get; set; }

        [Description("Diagram's border color.")]
        public string DiagramBorderColor { get; set; }

        [Description("Diagram's filling mode for an element's surface.")]
        public DevExpress.XtraCharts.FillMode? DiagramFillMode { get; set; }

        [Description("Diagram's direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? DiagramFillGradientMode { get; set; }

        [Description("Diagram's hatch style used for background filling.")]
        public HatchStyle? DiagramFillHatchStyle { get; set; }

        [Description("Indent between the diagram's edge and other chart elements (e.g. legend and chart titles), in pixels.")]
        public int[] Margins { get; set; }

        [Description("Direction in which the RadarAxisX is drawn.")]
        public RadarDiagramRotationDirection? RotationDirection { get; set; }

        [Description("Diagram's shadow color.")]
        public string DiagramShadowColor { get; set; }

        [Description("Diagram shadow size.")]
        public int? DiagramShadowSize { get; set; }

        [Description("Angle for the radial axis to start drawing.")]
        public double? StartAngleInDegrees { get; set; }


        internal override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (diagram is not RadarDiagram diagramRadar)
                return;

            if (!string.IsNullOrWhiteSpace(DiagramBackColor))
            {
                var backColor = Utils.ColorFromString(DiagramBackColor);
                if (backColor != Color.Empty)
                    diagramRadar.BackColor = backColor;
            }

            if (!string.IsNullOrWhiteSpace(DiagramBorderColor))
            {
                var borderColor = Utils.ColorFromString(DiagramBorderColor);
                if (borderColor != Color.Empty)
                {
                    diagramRadar.BorderVisible = true;
                    diagramRadar.BorderColor = borderColor;
                }
            }

            if (DiagramFillMode.HasValue)
            {
                diagramRadar.FillStyle.FillMode = DiagramFillMode.Value;
                switch (DiagramFillMode.Value)
                {
                    case DevExpress.XtraCharts.FillMode.Empty:
                        break;
                    case DevExpress.XtraCharts.FillMode.Solid:
                        var backColor = Utils.ColorFromString(DiagramBackColor);
                        if (backColor != Color.Empty)
                            diagramRadar.BackColor = backColor;
                        break;
                    case DevExpress.XtraCharts.FillMode.Gradient:
                        if (diagramRadar.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(DiagramBackColor2);
                            if (backColor2 != Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (DiagramFillGradientMode.HasValue)
                                gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)DiagramFillGradientMode.Value;
                        }
                        break;
                    case DevExpress.XtraCharts.FillMode.Hatch:
                        if (diagramRadar.FillStyle.Options is HatchFillOptions hatchOptions)
                        {
                            var backColor2 = Utils.ColorFromString(DiagramBackColor2);
                            if (backColor2 != Color.Empty)
                                hatchOptions.Color2 = backColor2;
                            if (DiagramFillHatchStyle.HasValue)
                                hatchOptions.HatchStyle = DiagramFillHatchStyle.Value;
                        }
                        break;
                }
            }

            if (Margins != null && Margins.Length == 1)
                diagramRadar.Margins.All = Margins[0];
            else if (Margins != null && Margins.Length == 4)
            {
                diagramRadar.Margins.Left = Margins[0];
                diagramRadar.Margins.Top = Margins[1];
                diagramRadar.Margins.Right = Margins[2];
                diagramRadar.Margins.Bottom = Margins[3];
            }

            if (RotationDirection.HasValue)
                diagramRadar.RotationDirection = (DevExpress.XtraCharts.RadarDiagramRotationDirection)RotationDirection.Value;

            if (StartAngleInDegrees.HasValue)
                diagramRadar.StartAngleInDegrees = StartAngleInDegrees.Value;

            if (!string.IsNullOrWhiteSpace(DiagramShadowColor))
            {
                var shadowColor = Utils.ColorFromString(DiagramShadowColor);
                if (shadowColor != Color.Empty)
                {
                    diagramRadar.Shadow.Visible = true;
                    diagramRadar.Shadow.Color = shadowColor;
                }
            }
            if (DiagramShadowSize.HasValue)
            {
                diagramRadar.Shadow.Size = DiagramShadowSize.Value;
                diagramRadar.Shadow.Visible = true;
            }
        }
    }
}
