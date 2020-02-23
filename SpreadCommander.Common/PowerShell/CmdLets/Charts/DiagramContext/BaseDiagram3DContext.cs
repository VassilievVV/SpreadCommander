using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.DiagramContext
{
    public class BaseDiagram3DContext : BaseDiagramContext
    {
        [Parameter(HelpMessage = "Amount by which to scroll a diagram horizontally.")]
        [ValidateRange(-100.0, 100.0)]
        public double? HorizontalScrollPercent { get; set; }

        [Parameter(HelpMessage = "Amount by which to scroll a diagram vertically.")]
        [ValidateRange(-100.0, 100.0)]
        public double? VerticalScrollPercent { get; set; }

        [Parameter(HelpMessage = "Perspective angle for a 3D diagram in a perspective projection")]
        [ValidateRange(0, 180)]
        [Alias("Perspective")]
        public int? PerspectiveAngle { get; set; }

        [Parameter(HelpMessage = "Value (in degrees) at which the diagram should be rotated around the X-axis.")]
        [ValidateRange(-360, 360)]
        [Alias("RotX")]
        public int? RotationAngleX { get; set; }

        [Parameter(HelpMessage = "Value (in degrees) at which the diagram should be rotated around the Y-axis.")]
        [ValidateRange(-360, 360)]
        [Alias("RotY")]
        public int? RotationAngleY { get; set; }

        [Parameter(HelpMessage = "Value (in degrees) at which the diagram should be rotated around the Z-axis.")]
        [ValidateRange(-360, 360)]
        [Alias("RotZ")]
        public int? RotationAngleZ { get; set; }

        [Parameter(HelpMessage = "Order of rotation around the X, Y and Z axes.")]
        [Alias("RotOrder")]
        public RotationOrder? RotationOrder { get; set; }



        public override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (!(diagram is Diagram3D diagram3D))
                return;

            if (HorizontalScrollPercent.HasValue)
                diagram3D.HorizontalScrollPercent = HorizontalScrollPercent.Value;
            if (VerticalScrollPercent.HasValue)
                diagram3D.VerticalScrollPercent   = VerticalScrollPercent.Value;

            if (PerspectiveAngle != 0)
            {
                diagram3D.PerspectiveEnabled = true;
                if (PerspectiveAngle.HasValue)
                    diagram3D.PerspectiveAngle = PerspectiveAngle.Value;
            }

            diagram3D.RotationType   = RotationType.UseAngles;
            if (RotationAngleX.HasValue) diagram3D.RotationAngleX = RotationAngleX.Value;
            if (RotationAngleY.HasValue) diagram3D.RotationAngleY = RotationAngleY.Value;
            if (RotationAngleZ.HasValue) diagram3D.RotationAngleZ = RotationAngleZ.Value;
            if (RotationOrder.HasValue)  diagram3D.RotationOrder  = RotationOrder.Value;
        }
    }
}