using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class RadarPointSeriesContext: BaseSeriesContext
    {
        [Parameter(HelpMessage = "Aggregate function used for a series.")]
        [PSDefaultValue(Value = SCSeriesAggregateFunction.Default)]
        [DefaultValue(SCSeriesAggregateFunction.Default)]
        public SCSeriesAggregateFunction AggregateFunction { get; set; } = SCSeriesAggregateFunction.Default;

        [Parameter(HelpMessage = "Whether each data point of a series is shown in a different color.")]
        public SwitchParameter ColorEach { get; set; }

        [Parameter(HelpMessage = "Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

        [Parameter(HelpMessage = "Size of the marker.")]
        public int? MarkerSize { get; set; }

        [Parameter(HelpMessage = "Number of points that star-shaped marker have.")]
        public int? MarkerStarPointCount { get; set; }

        [Parameter(HelpMessage = "Border color of the marker")]
        public string MarkerBorderColor { get; set; }

        [Parameter(HelpMessage = "Whether border of the marker is visible.")]
        public SwitchParameter HideMarkerBorder { get; set; }

        [Parameter(HelpMessage = "Filling mode of the marker.")]
        public DevExpress.XtraCharts.FillMode? MarkerFillMode { get; set; }

        [Parameter(HelpMessage = "Second color for gradient and hatch fill of the marker.")]
        public string MarkerBackColor2 { get; set; }

        [Parameter(HelpMessage = "Background gradient's direction of the marker.")]
        public PolygonGradientMode? MarkerGradientMode { get; set; }

        [Parameter(HelpMessage = "Marker's hatch style")]
        public HatchStyle? MarkerHatchStyle { get; set; }

        [Parameter(HelpMessage = "Series shadow's color.")]
        public string ShadowColor { get; set; }

        [Parameter(HelpMessage = "Series shadow's thickness.")]
        public int? ShadowSize { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is RadarPointSeriesView view)
            {
                if (AggregateFunction != SCSeriesAggregateFunction.Default)
                    view.AggregateFunction = (SeriesAggregateFunction)(int)AggregateFunction;
                view.ColorEach = ColorEach;

                if (view is RadarLineSeriesView viewLine)
                {
                    if (MarkerKind.HasValue)
                        viewLine.LineMarkerOptions.Kind = MarkerKind.Value;
                    if (MarkerSize.HasValue)
                        viewLine.LineMarkerOptions.Size = MarkerSize.Value;
                    if (MarkerStarPointCount.HasValue)
                        viewLine.LineMarkerOptions.StarPointCount = MarkerStarPointCount.Value;
                    if (!string.IsNullOrWhiteSpace(MarkerBorderColor))
                    {
                        var borderColor = Utils.ColorFromString(MarkerBorderColor);
                        if (borderColor != Color.Empty)
                        {
                            viewLine.LineMarkerOptions.BorderVisible = true;
                            viewLine.LineMarkerOptions.BorderColor = borderColor;
                        }
                    }

                    if (HideMarkerBorder)
                        viewLine.LineMarkerOptions.BorderVisible = false;
                    if (MarkerFillMode.HasValue)
                    {
                        switch (MarkerFillMode.Value)
                        {
                            case DevExpress.XtraCharts.FillMode.Empty:
                                break;
                            case DevExpress.XtraCharts.FillMode.Solid:
                                break;
                            case DevExpress.XtraCharts.FillMode.Gradient:
                                if (viewLine.LineMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                                {
                                    if (MarkerGradientMode.HasValue)
                                        gradientFillOptions.GradientMode = MarkerGradientMode.Value;
                                    var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                    if (backColor2 != Color.Empty)
                                        gradientFillOptions.Color2 = backColor2;
                                }
                                break;
                            case DevExpress.XtraCharts.FillMode.Hatch:
                                if (viewLine.LineMarkerOptions.FillStyle.Options is HatchFillOptions hatchFillOptions)
                                {
                                    if (MarkerHatchStyle.HasValue)
                                        hatchFillOptions.HatchStyle = MarkerHatchStyle.Value;
                                    var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                    if (backColor2 != Color.Empty)
                                        hatchFillOptions.Color2 = backColor2;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    if (MarkerKind.HasValue)
                        view.PointMarkerOptions.Kind = MarkerKind.Value;
                    if (MarkerSize.HasValue)
                        view.PointMarkerOptions.Size = MarkerSize.Value;
                    if (MarkerStarPointCount.HasValue)
                        view.PointMarkerOptions.StarPointCount = MarkerStarPointCount.Value;
                    if (!string.IsNullOrWhiteSpace(MarkerBorderColor))
                    {
                        var borderColor = Utils.ColorFromString(MarkerBorderColor);
                        if (borderColor != Color.Empty)
                        {
                            view.PointMarkerOptions.BorderVisible = true;
                            view.PointMarkerOptions.BorderColor = borderColor;
                        }
                    }

                    if (HideMarkerBorder)
                        view.PointMarkerOptions.BorderVisible = false;
                    if (MarkerFillMode.HasValue)
                    {
                        switch (MarkerFillMode.Value)
                        {
                            case DevExpress.XtraCharts.FillMode.Empty:
                                break;
                            case DevExpress.XtraCharts.FillMode.Solid:
                                break;
                            case DevExpress.XtraCharts.FillMode.Gradient:
                                if (view.PointMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                                {
                                    if (MarkerGradientMode.HasValue)
                                        gradientFillOptions.GradientMode = MarkerGradientMode.Value;
                                    var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                    if (backColor2 != Color.Empty)
                                        gradientFillOptions.Color2 = backColor2;
                                }
                                break;
                            case DevExpress.XtraCharts.FillMode.Hatch:
                                if (view.PointMarkerOptions.FillStyle.Options is HatchFillOptions hatchFillOptions)
                                {
                                    if (MarkerHatchStyle.HasValue)
                                        hatchFillOptions.HatchStyle = MarkerHatchStyle.Value;
                                    var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                    if (backColor2 != Color.Empty)
                                        hatchFillOptions.Color2 = backColor2;
                                }
                                break;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ShadowColor))
                {
                    var shadowColor = Utils.ColorFromString(ShadowColor);
                    if (shadowColor != Color.Empty)
                    {
                        view.Shadow.Visible = true;
                        view.Shadow.Color   = shadowColor;
                    }
                }
                if (ShadowSize.HasValue)
                {
                    view.Shadow.Size    = ShadowSize.Value;
                    view.Shadow.Visible = true;
                }
            }
        }
    }
}
