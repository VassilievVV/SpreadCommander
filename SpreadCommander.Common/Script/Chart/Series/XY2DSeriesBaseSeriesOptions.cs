using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class XY2DSeriesBaseSeriesOptions: SeriesOptions
    {
        [DefaultValue(SeriesAggregateFunction.Default)]
        public SeriesAggregateFunction AggregateFunction { get; set; } = SeriesAggregateFunction.Default;

        [Description("Pane's name.")]
        public string Pane { get; set; }

        [Description("X axis name.")]
        public string AxisX { get; set; }

        [Description("Y axis name.")]
        public string AxisY { get; set; }

        [Description("Color of the series.")]
        public string Color { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is XYDiagram2DSeriesViewBase view)
            {
                if (view is SwiftPlotSeriesView swiftView)
                    swiftView.Antialiasing = true;

                if (AggregateFunction != SeriesAggregateFunction.Default)
                    view.AggregateFunction = (DevExpress.XtraCharts.SeriesAggregateFunction)(int)AggregateFunction;

                var color = Utils.ColorFromString(Color);
                if (color != System.Drawing.Color.Empty)
                    view.Color = color;

                if (chart.Chart.Diagram is XYDiagram2D diagram)
                {
                    XYDiagramPane pane = null;
                    Axis2D axisX = null, axisY = null;
                    if (!string.IsNullOrWhiteSpace(Pane))
                    {
                        pane = diagram.Panes[Pane];
                        if (pane == null)
                            throw new Exception("Invalid pane name.");

                    }
                    if (!string.IsNullOrWhiteSpace(AxisX))
                    {
                        axisX = diagram.FindAxisXByName(AxisX);
                        if (axisX == null)
                            throw new Exception("Invalid axis X name.");
                    }
                    if (!string.IsNullOrWhiteSpace(AxisY))
                    {
                        axisY = diagram.FindAxisYByName(AxisY);
                        if (axisY == null)
                            throw new Exception("Invalid axis Y name.");
                    }

                    switch (view)
                    {
                        case XYDiagramSeriesViewBase viewBase:
                            if (pane != null)
                                viewBase.Pane = pane;
                            if (axisX != null)
                                viewBase.AxisX = axisX as AxisXBase;
                            if (axisY != null)
                                viewBase.AxisY = axisY as AxisYBase;
                            break;
                        case SwiftPlotSeriesViewBase viewSwift:
                            if (pane != null)
                                viewSwift.Pane = pane;
                            if (axisX != null)
                                viewSwift.AxisX = axisX as SwiftPlotDiagramAxisXBase;
                            if (axisY != null)
                                viewSwift.AxisY = axisY as SwiftPlotDiagramAxisYBase;
                            break;
                    }

                }
            }
        }
    }
}
