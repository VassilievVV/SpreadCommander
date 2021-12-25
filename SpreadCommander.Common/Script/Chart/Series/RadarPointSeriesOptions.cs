using DevExpress.XtraCharts;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart.Series
{
    public class RadarPointSeriesOptions: SeriesOptions
    {
        [Description("Aggregate function used for a series.")]
        [DefaultValue(SeriesAggregateFunction.Default)]
        public SeriesAggregateFunction AggregateFunction { get; set; } = SeriesAggregateFunction.Default;

        [Description("Whether each data point of a series is shown in a different color.")]
        public bool ColorEach { get; set; }

        [Description("Shape of the marker.")]
        public MarkerKind? MarkerKind { get; set; }

        [Description("Size of the marker.")]
        public int? MarkerSize { get; set; }

        [Description("Number of points that star-shaped marker have.")]
        public int? MarkerStarPointCount { get; set; }

        [Description("Border color of the marker")]
        public string MarkerBorderColor { get; set; }

        [Description("Whether border of the marker is visible.")]
        public bool HideMarkerBorder { get; set; }

        [Description("Filling mode of the marker.")]
        public FillMode? MarkerFillMode { get; set; }

        [Description("Second color for gradient and hatch fill of the marker.")]
        public string MarkerBackColor2 { get; set; }

        [Description("Background gradient's direction of the marker.")]
        public PolygonGradientMode? MarkerGradientMode { get; set; }

        [Description("Marker's hatch style")]
        public HatchStyle? MarkerHatchStyle { get; set; }

        [Description("Series shadow's color.")]
        public string ShadowColor { get; set; }

        [Description("Series shadow's thickness.")]
        public int? ShadowSize { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is RadarPointSeriesView view)
            {
                if (AggregateFunction != SeriesAggregateFunction.Default)
                    view.AggregateFunction = (DevExpress.XtraCharts.SeriesAggregateFunction)AggregateFunction;
                view.ColorEach = ColorEach;

                if (view is RadarLineSeriesView viewLine)
                {
                    if (MarkerKind.HasValue)
                        viewLine.LineMarkerOptions.Kind = (DevExpress.XtraCharts.MarkerKind)MarkerKind.Value;
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
                            case FillMode.Empty:
                                break;
                            case FillMode.Solid:
                                break;
                            case FillMode.Gradient:
                                if (viewLine.LineMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                                {
                                    if (MarkerGradientMode.HasValue)
                                        gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)MarkerGradientMode.Value;
                                    var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                    if (backColor2 != Color.Empty)
                                        gradientFillOptions.Color2 = backColor2;
                                }
                                break;
                            case FillMode.Hatch:
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
                        view.PointMarkerOptions.Kind = (DevExpress.XtraCharts.MarkerKind)MarkerKind.Value;
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
                            case FillMode.Empty:
                                break;
                            case FillMode.Solid:
                                break;
                            case FillMode.Gradient:
                                if (view.PointMarkerOptions.FillStyle.Options is PolygonGradientFillOptions gradientFillOptions)
                                {
                                    if (MarkerGradientMode.HasValue)
                                        gradientFillOptions.GradientMode = (DevExpress.XtraCharts.PolygonGradientMode)MarkerGradientMode.Value;
                                    var backColor2 = Utils.ColorFromString(MarkerBackColor2);
                                    if (backColor2 != Color.Empty)
                                        gradientFillOptions.Color2 = backColor2;
                                }
                                break;
                            case FillMode.Hatch:
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
