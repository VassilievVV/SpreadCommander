using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    //No options, all parts are mandatory

    public partial class SCChart
    {
        public SCChart AddScaleBreak(ChartAxisType axisType, string name,
            object edge1, object edge2) =>
            AddScaleBreak(axisType, null, name, edge1, edge2);

        public SCChart AddScaleBreak(ChartAxisType axisType, string axisName, string name, 
            object edge1, object edge2)
        {
            AxisBase axis;

            if (!string.IsNullOrWhiteSpace(axisName))
            {
                axis = AxisOptions.GetSecondaryAxis(Chart.Diagram, axisType, axisName);
                if (axis == null)
                    throw new Exception($"Cannot find axis '{axisName}'.");
            }
            else
            {
                axis = AxisOptions.GetPrimaryAxis(Chart.Diagram, axisType);
                if (axis == null)
                    throw new Exception("Cannot find primary axis.");
            }

            if (axis is not Axis axis2D)
                throw new Exception("Only 2D axis except SwiftPlot support scale breaks.");

            var scaleBreak = new ScaleBreak();
            if (!string.IsNullOrWhiteSpace(name))
                scaleBreak.Name = name;

            scaleBreak.Edge1 = edge1;
            scaleBreak.Edge2 = edge2;

            scaleBreak.Visible = true;

            axis2D.ScaleBreaks.Add(scaleBreak);

            return this;
        }
    }
}
