using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Diagrams
{
    public class Pie3DDiagramOptions: Base3DDiagramOptions
    {
        [Description("Number of 3D Pie Charts that can be displayed in one line (row or column).")]
        public int? Dimension { get; set; }

        [Description("Manner in which multiple 3D Pie Charts are positioned within a chart control's diagram.")]
        public LayoutDirection? LayoutDirection { get; set; }


        internal override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (diagram is not SimpleDiagram3D diagram3D)
                return;

            if (Dimension.HasValue)
                diagram3D.Dimension = Dimension.Value;

            if (LayoutDirection.HasValue)
                diagram3D.LayoutDirection = (DevExpress.XtraCharts.LayoutDirection)LayoutDirection.Value;
        }
    }
}
