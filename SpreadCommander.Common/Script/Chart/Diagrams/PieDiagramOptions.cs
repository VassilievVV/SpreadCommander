using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Diagrams
{
    public class PieDiagramOptions: DiagramOptions
    {
        [Description("Number of pie charts that can be displayed in one line (row or column). 0 - use auto layout.")]
        public int? Dimension { get; set; }

        [Description("Manner in which multiple pie charts are positioned within a chart control's diagram.")]
        public LayoutDirection? LayoutDirection { get; set; }

        [Description("Whether pies in a chart are equal in size.")]
        public bool EqualPieSize { get; set; }

        [Description("Indent between the diagram's edge and other chart elements (e.g. legend and chart titles), in pixels.")]
        public int[] Margins { get; set; }


        internal override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (diagram is not SimpleDiagram simpleDiagram)
                return;

            if (LayoutDirection.HasValue)
                simpleDiagram.LayoutDirection = (DevExpress.XtraCharts.LayoutDirection)LayoutDirection.Value;

            simpleDiagram.EqualPieSize = EqualPieSize;

            if (Dimension.HasValue)
                simpleDiagram.Dimension = Dimension.Value;

            if (Margins != null && Margins.Length == 1)
                simpleDiagram.Margins.All = Margins[0];
            else if (Margins != null && Margins.Length == 4)
            {
                simpleDiagram.Margins.Left   = Margins[0];
                simpleDiagram.Margins.Top    = Margins[1];
                simpleDiagram.Margins.Right  = Margins[2];
                simpleDiagram.Margins.Bottom = Margins[3];
            }
        }
    }
}
