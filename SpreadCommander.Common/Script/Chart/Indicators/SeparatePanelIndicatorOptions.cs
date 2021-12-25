using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Indicators
{
    public class SeparatePanelIndicatorOptions: IndicatorOptions
    {
        [Description("Y-axis that is used to plot the current indicator.")]
        public string AxisY { get; set; }

        [Description("Pane, used to plot the separate pane indicator")]
        public string Pane { get; set; }


        protected internal override void SetupXtraChartIndicator(SCChart chart, Indicator indicator)
        {
            base.SetupXtraChartIndicator(chart, indicator);

            if (indicator is SeparatePaneIndicator separateIndicator)
            {
                if (!string.IsNullOrWhiteSpace(AxisY))
                {
                    var axisY = (chart.Chart.Diagram as XYDiagram)?.FindAxisYByName(AxisY) ??
                        throw new Exception($"Cannot find axis '{AxisY}' or indicator is not supported on this chart.");

                    separateIndicator.AxisY = axisY;
                }

                if (!string.IsNullOrWhiteSpace(Pane))
                {
                    var pane = (chart.Chart.Diagram as XYDiagram)?.Panes[Pane] ??
                        throw new Exception($"Cannot find pane '{Pane}' or indicator is not supported on this chart.");

                    separateIndicator.Pane = pane;
                }
            }
        }
    }
}
