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

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.DiagramContext
{
    public class XYDiagram3DContext : BaseDiagram3DContext
    {
        [Parameter(HelpMessage = "Depth (thickness) of coordinate planes. The units of measurement are specific diagram pixels.")]
        [PSDefaultValue(Value = 15)]
        [DefaultValue(15)]
        [ValidateRange(0, 100)]
        public int PlaneDepthFixed { get; set; } = 15;

        [Parameter(HelpMessage = "Distance between Manhattan Bar series, as a fraction of axis units.")]
        [ValidateRange(0, 100)]
        public double SeriesDistance { get; set; }

        [Parameter(HelpMessage = "Distance between Manhattan Bar series, in pixels.")]
        [ValidateRange(0, 100)]
        public int SeriesDistanceFixed { get; set; }

        [Parameter(HelpMessage = "Distance between series and argument coordinate planes (front and back). The units of measurement are specific diagram pixels.")]
        [PSDefaultValue(Value = 10)]
        [DefaultValue(10)]
        [ValidateRange(0, 100)]
        public int SeriesIndentFixed { get; set; } = 10;

        [Parameter(HelpMessage = "Diagram's background color.")]
        public string DiagramBackColor { get; set; }

        [Parameter(HelpMessage = "Diagram's 2nd background color, if FillMode is gradient or hatch.")]
        public string DiagramBackColor2 { get; set; }

        [Parameter(HelpMessage = "Diagram's filling mode for an element's surface.")]
        public FillMode3D? DiagramFillMode { get; set; }

        [Parameter(HelpMessage = "Diagram's direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? DiagramFillGradientMode { get; set; }


        public override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (!(diagram is XYDiagram3D diagram3D))
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
                diagram3D.FillStyle.FillMode = DiagramFillMode.Value;
            switch (diagram3D.FillStyle.FillMode)
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
                            gradientOptions.Color2   = backColor2;
                        if (DiagramFillGradientMode.HasValue)
                            gradientOptions.GradientMode = DiagramFillGradientMode.Value;
                    }
                    break;
            }
        }
    }
}
