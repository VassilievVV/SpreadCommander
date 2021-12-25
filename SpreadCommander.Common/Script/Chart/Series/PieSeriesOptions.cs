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
    public class PieSeriesOptions: SeriesOptions
    {
        [Description("Which series should be represented as slices offset from the pie.")]
        public PieExplodeMode? ExplodeMode { get; set; }

        [Description("Conditions for the points to be plotted as exploded pie slices.")]
        public object[] ExplodedPoints { get; set; }

        [Description("Series's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Legend's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Ratio of the pie chart's height to its width.")]
        public double? HeightToWidthRatio { get; set; }

        [Description("Angle by which pie slices are rotated.")]
        public int? Rotation { get; set; }

        [Description("Sweep direction (clockwise or counterclockwise) for a Pie series.")]
        public PieSweepDirection? SweepDirection { get; set; }

        [Description("Minimum allowed size of the pie chart inside its boundaries, as a percentage.")]
        public double? MinAllowedSizePercentage { get; set; }

        [Description("Thickness of the entire pie in 3D Pie Chart.")]
        public int? Depth { get; set; }


        protected internal override void SetupXtraChartSeries(SCChart chart, DevExpress.XtraCharts.Series series, string name, string argument, string[] values)
        {
            base.SetupXtraChartSeries(chart, series, name, argument, values);

            if (series.View is PieSeriesView view)
            {
                if (ExplodeMode.HasValue)
                    view.ExplodeMode = (DevExpress.XtraCharts.PieExplodeMode)ExplodeMode.Value;
                if (HeightToWidthRatio.HasValue)
                    view.HeightToWidthRatio = HeightToWidthRatio.Value;
                if (Rotation.HasValue)
                    view.Rotation = Rotation.Value;
                if (SweepDirection.HasValue)
                    view.SweepDirection = (DevExpress.XtraCharts.PieSweepDirection)SweepDirection.Value;
                if (MinAllowedSizePercentage.HasValue)
                    view.MinAllowedSizePercentage = MinAllowedSizePercentage.Value;

                if (ExplodedPoints != null && ExplodedPoints.Length > 0)
                {
                    view.ExplodeMode = (DevExpress.XtraCharts.PieExplodeMode)PieExplodeMode.UseFilters;
                    view.ExplodedPointsFilters.ConjunctionMode = ConjunctionTypes.Or;

                    foreach (var explodedPoint in ExplodedPoints)
                        view.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.Equal, explodedPoint));
                }

                if (FillMode.HasValue)
                {
                    view.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)FillMode.Value;
                    switch (FillMode.Value)
                    {
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            break;
                        case Chart.FillMode.Gradient:
                            if (view.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                            }
                            break;
                        case Chart.FillMode.Hatch:
                            if (view.FillStyle.Options is HatchFillOptions hatchOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != Color.Empty)
                                    hatchOptions.Color2 = backColor2;
                                if (FillHatchStyle.HasValue)
                                    hatchOptions.HatchStyle = FillHatchStyle.Value;
                            }
                            break;
                    }
                }
            }
            else if (series.View is Pie3DSeriesView view3D)
            {
                if (ExplodeMode.HasValue)
                    view3D.ExplodeMode = (DevExpress.XtraCharts.PieExplodeMode)ExplodeMode.Value;
                if (SweepDirection.HasValue)
                    view3D.SweepDirection = (DevExpress.XtraCharts.PieSweepDirection)SweepDirection.Value;
                if (MinAllowedSizePercentage.HasValue)
                    view3D.SizeAsPercentage = MinAllowedSizePercentage.Value;

                if (ExplodedPoints != null && ExplodedPoints.Length > 0)
                {
                    view3D.ExplodeMode = DevExpress.XtraCharts.PieExplodeMode.UseFilters;
                    view3D.ExplodedPointsFilters.ConjunctionMode = ConjunctionTypes.Or;

                    foreach (var explodedPoint in ExplodedPoints)
                        view3D.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.Equal, explodedPoint));
                }

                if (FillMode.HasValue)
                {
                    switch (FillMode.Value)
                    {
                        case Chart.FillMode.Empty:
                            break;
                        case Chart.FillMode.Solid:
                            view3D.PieFillStyle.FillMode = (DevExpress.XtraCharts.FillMode3D)FillMode3D.Solid;
                            break;
                        case Chart.FillMode.Gradient:
                            view3D.PieFillStyle.FillMode = (DevExpress.XtraCharts.FillMode3D)FillMode3D.Gradient;
                            if (view3D.PieFillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                            }
                            break;
                        case Chart.FillMode.Hatch:
                            //Ignore, cannot be applied to 3D series
                            break;
                    }
                }

                if (Depth.HasValue)
                    view3D.Depth = Depth.Value;
            }
        }
    }
}
