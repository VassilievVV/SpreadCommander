using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Diagrams
{
    public class Base3DDiagramOptions: DiagramOptions
    {
        [Description("Amount by which to scroll a diagram horizontally.")]
        public double? HorizontalScrollPercent { get; set; }

        [Description("Amount by which to scroll a diagram vertically.")]
        public double? VerticalScrollPercent { get; set; }

        [Description("Perspective angle for a 3D diagram in a perspective projection")]
        public int? PerspectiveAngle { get; set; }

        [Description("Value (in degrees) at which the diagram should be rotated around the X-axis.")]
        public int? RotationAngleX { get; set; }

        [Description("Value (in degrees) at which the diagram should be rotated around the Y-axis.")]
        public int? RotationAngleY { get; set; }

        [Description("Value (in degrees) at which the diagram should be rotated around the Z-axis.")]
        public int? RotationAngleZ { get; set; }

        [Description("Order of rotation around the X, Y and Z axes.")]
        public RotationOrder? RotationOrder { get; set; }


        internal override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (diagram is not Diagram3D diagram3D)
                return;

            if (HorizontalScrollPercent.HasValue)
                diagram3D.HorizontalScrollPercent = HorizontalScrollPercent.Value;
            if (VerticalScrollPercent.HasValue)
                diagram3D.VerticalScrollPercent = VerticalScrollPercent.Value;

            if (PerspectiveAngle != 0)
            {
                diagram3D.PerspectiveEnabled = true;
                if (PerspectiveAngle.HasValue)
                    diagram3D.PerspectiveAngle = PerspectiveAngle.Value;
            }

            diagram3D.RotationType = RotationType.UseAngles;
            if (RotationAngleX.HasValue) diagram3D.RotationAngleX = RotationAngleX.Value;
            if (RotationAngleY.HasValue) diagram3D.RotationAngleY = RotationAngleY.Value;
            if (RotationAngleZ.HasValue) diagram3D.RotationAngleZ = RotationAngleZ.Value;
            if (RotationOrder.HasValue)  diagram3D.RotationOrder  = (DevExpress.XtraCharts.RotationOrder)RotationOrder.Value;
        }
    }
}
