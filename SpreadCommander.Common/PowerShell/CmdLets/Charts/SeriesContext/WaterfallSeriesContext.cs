using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Management.Automation;
using System.Text;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class WaterfallSeriesContext: BarSeriesContext
    {
        #region Subtotal
        public class Subtotal
        {
            public int PointIndex { get; set; }

            public string Label { get; set; }
        }
        #endregion

        [Parameter(HelpMessage = "Connector color.")]
        public string ConnectorColor { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the connector line.")]
        public DevExpress.XtraCharts.DashStyle? ConnectorLineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive connector lines.")]
        public LineJoin? ConnectorLineJoin { get; set; }

        [Parameter(HelpMessage = "Connector line's thickness.")]
        public int? ConnectorThickness { get; set; }

        [Parameter(HelpMessage = "Falling bar color.")]
        public string FallingBarColor { get; set; }

        [Parameter(HelpMessage = "Rising bar color.")]
        public string RisingBarColor { get; set; }

        [Parameter(HelpMessage = "Start bar color.")]
        public string StartBarColor { get; set; }

        [Parameter(HelpMessage = "Subtotal bar color.")]
        public string SubtotalBarColor { get; set; }

        [Parameter(HelpMessage = "Total bar color.")]
        public string TotalBarColor { get; set; }

        [Parameter(HelpMessage = "Whether waterfall chart displays absolute or relative data values.")]
        public SwitchParameter RelativeValues { get; set; }

        [Parameter(HelpMessage = "Whether to show the total bar.")]
        public SwitchParameter ShowTotal { get; set; }

        [Parameter(HelpMessage = "Start bar label.")]
        public string StartBarLabel { get; set; }

        [Parameter(HelpMessage = "Start bar value.")]
        public double? StartBarValue { get; set; }

        [Parameter(HelpMessage = "Waterfall total bar's string label that is used in the axis label and crosshair.")]
        public string TotalLabel { get; set; }

        [Parameter(HelpMessage = "Waterfall chart subtotals.")]
        public Subtotal[] Subtotals { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

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
