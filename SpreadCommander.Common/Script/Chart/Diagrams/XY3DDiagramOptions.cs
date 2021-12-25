using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Diagrams
{
    public class XY3DDiagramOptions : Base3DDiagramOptions
    {
        [Description("Depth (thickness) of coordinate planes. The units of measurement are specific diagram pixels.")]
        [DefaultValue(15)]
        public int PlaneDepthFixed { get; set; } = 15;

        [Description("Distance between Manhattan Bar series, as a fraction of axis units.")]
        public double SeriesDistance { get; set; }

        [Description("Distance between Manhattan Bar series, in pixels.")]
        public int SeriesDistanceFixed { get; set; }

        [Description("Distance between series and argument coordinate planes (front and back). The units of measurement are specific diagram pixels.")]
        [DefaultValue(10)]
        public int SeriesIndentFixed { get; set; } = 10;

        [Description("Diagram's background color.")]
        public string DiagramBackColor { get; set; }

        [Description("Diagram's 2nd background color, if FillMode is gradient or hatch.")]
        public string DiagramBackColor2 { get; set; }

        [Description("Diagram's filling mode for an element's surface.")]
        public FillMode3D? DiagramFillMode { get; set; }

        [Description("Diagram's direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? DiagramFillGradientMode { get; set; }


        internal override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (diagram is not XYDiagram3D diagram3D)
                return;

            diagram3D.PlaneDepthFixed = PlaneDepthFixed;
            if (SeriesDistance > 0)
                diagram3D.SeriesDistance = SeriesDistance;
            if (SeriesDistanceFixed > 0)
                diagram3D.SeriesDistanceFixed = SeriesDistanceFixed;
            diagram3D.SeriesIndentFixed = SeriesIndentFixed;

            if (!string.IsNullOrWhiteSpace(DiagramBackColor))
            {
                var backColor = Utils.ColorFromString(DiagramBackColor);
                if (backColor != Color.Empty)
                    diagram3D.BackColor = backColor;
            }

            if (DiagramFillMode.HasValue)
                diagram3D.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode3D)DiagramFillMode.Value;
            switch ((FillMode3D)diagram3D.FillStyle.FillMode)
            {
                case FillMode3D.Empty:
                    break;
                case FillMode3D.Solid:
                    var backColor = Utils.ColorFromString(DiagramBackColor);
                    if (backColor != Color.Empty)
                        diagram3D.BackColor = backColor;
                    break;
                case FillMode3D.Gradient:
                    if (diagram3D.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                    {
                        var backColor2 = Utils.ColorFromString(DiagramBackColor2);
                        if (backColor2 != Color.Empty)
                            gradientOptions.Color2 = backColor2;
                        if (DiagramFillGradientMode.HasValue)
                            gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)DiagramFillGradientMode.Value;
                    }
                    break;
            }
        }
    }
}
