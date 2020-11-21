using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.DiagramContext
{
    public class PieDiagram3DContext : BaseDiagram3DContext
    {
        [Parameter(HelpMessage = "Number of 3D Pie Charts that can be displayed in one line (row or column).")]
        [ValidateRange(0, 100)]
        public int? Dimension { get; set; }

        [Parameter(HelpMessage = "Manner in which multiple 3D Pie Charts are positioned within a chart control's diagram.")]
        public LayoutDirection? LayoutDirection { get; set; }


        public override void SetupDiagram(Diagram diagram)
        {
            base.SetupDiagram(diagram);

            if (diagram is not SimpleDiagram3D diagram3D)
                return;

            if (Dimension.HasValue)
                diagram3D.Dimension = Dimension.Value;

            if (LayoutDirection.HasValue)
                diagram3D.LayoutDirection = LayoutDirection.Value;
        }
    }
}
