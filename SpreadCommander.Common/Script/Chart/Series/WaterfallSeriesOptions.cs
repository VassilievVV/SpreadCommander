using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class WaterfallSeriesOptions: BarSeriesOptions
    {
        #region Subtotal
        public class Subtotal
        {
            public int PointIndex { get; set; }

            public string Label { get; set; }
        }
        #endregion

        [Description("Connector color.")]
        public string ConnectorColor { get; set; }

        [Description("Dash style used to paint the connector line.")]
        public DevExpress.XtraCharts.DashStyle? ConnectorLineDashStyle { get; set; }

        [Description("Join style for the ends of consecutive connector lines.")]
        public LineJoin? ConnectorLineJoin { get; set; }

        [Description("Connector line's thickness.")]
        public int? ConnectorThickness { get; set; }

        [Description("Falling bar color.")]
        public string FallingBarColor { get; set; }

        [Description("Rising bar color.")]
        public string RisingBarColor { get; set; }

        [Description("Start bar color.")]
        public string StartBarColor { get; set; }

        [Description("Subtotal bar color.")]
        public string SubtotalBarColor { get; set; }

        [Description("Total bar color.")]
        public string TotalBarColor { get; set; }

        [Description("Whether waterfall chart displays absolute or relative data values.")]
        public bool RelativeValues { get; set; }

        [Description("Whether to show the total bar.")]
        public bool ShowTotal { get; set; }

        [Description("Start bar label.")]
        public string StartBarLabel { get; set; }

        [Description("Start bar value.")]
        public double? StartBarValue { get; set; }

        [Description("Waterfall total bar's string label that is used in the axis label and crosshair.")]
        public string TotalLabel { get; set; }

        [Description("Waterfall chart subtotals.")]
        public Subtotal[] Subtotals { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is WaterfallSeriesView view)
            {
                var connectorColor = Utils.ColorFromString(ConnectorColor);
                if (connectorColor != System.Drawing.Color.Empty)
                    view.ConnectorColor = connectorColor;
                
                if (ConnectorLineDashStyle.HasValue)
                    view.ConnectorStyle.DashStyle = ConnectorLineDashStyle.Value;
                
                if (ConnectorLineJoin.HasValue)
                    view.ConnectorStyle.LineJoin = ConnectorLineJoin.Value;
                
                if (ConnectorThickness.HasValue)
                    view.ConnectorStyle.Thickness = ConnectorThickness.Value;
                
                var fallingBarColor = Utils.ColorFromString(FallingBarColor);
                if (fallingBarColor != System.Drawing.Color.Empty)
                    view.FallingBarColor = fallingBarColor;
                
                var risingBarColor = Utils.ColorFromString(RisingBarColor);
                if (risingBarColor != System.Drawing.Color.Empty)
                    view.RisingBarColor = risingBarColor;
                
                var startBarColor = Utils.ColorFromString(StartBarColor);
                if (startBarColor != System.Drawing.Color.Empty)
                    view.StartBarColor = startBarColor;

                var subtotalBarColor = Utils.ColorFromString(SubtotalBarColor);
                if (subtotalBarColor != System.Drawing.Color.Empty)
                    view.SubtotalBarColor = subtotalBarColor;

                var totalBarColor = Utils.ColorFromString(TotalBarColor);
                if (totalBarColor != System.Drawing.Color.Empty)
                    view.TotalBarColor = totalBarColor;

                if (RelativeValues)
                {
                    var valueOptions = new WaterfallRelativeValueOptions() { ShowTotal = this.ShowTotal };
                    
                    if (!string.IsNullOrWhiteSpace(StartBarLabel))
                        valueOptions.StartBarLabel = StartBarLabel;

                    if (StartBarValue.HasValue)
                        valueOptions.StartBarValue = StartBarValue.Value;

                    if (!string.IsNullOrWhiteSpace(TotalLabel))
                        valueOptions.TotalLabel = TotalLabel;

                    if (Subtotals != null && Subtotals.Length > 0)
                    {
                        foreach (var subtotal in Subtotals)
                            valueOptions.Subtotals.Add(new DevExpress.XtraCharts.Subtotal(subtotal.PointIndex, subtotal.Label));
                    }

                    view.ValueOptions = valueOptions;
                }
                else
                {
                    var valueOptions = new WaterfallAbsoluteValueOptions() { ShowTotal = this.ShowTotal };
                    
                    if (!string.IsNullOrWhiteSpace(TotalLabel))
                        valueOptions.TotalLabel = TotalLabel;

                    if (Subtotals != null && Subtotals.Length > 0)
                    {
                        foreach (var subtotal in Subtotals)
                            valueOptions.Subtotals.Add(new DevExpress.XtraCharts.Subtotal(subtotal.PointIndex, subtotal.Label));
                    }

                    view.ValueOptions = valueOptions;
                }
            }
        }
    }
}
