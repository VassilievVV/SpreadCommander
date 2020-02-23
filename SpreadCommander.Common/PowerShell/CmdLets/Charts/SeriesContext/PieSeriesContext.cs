using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts.SeriesContext
{
    public class PieSeriesContext: BaseSeriesContext
    {
        #region SCPieExplodeMode
        public enum SCPieExplodeMode
        {
            None     = 0,
            All      = 1,
            MinValue = 2,
            MaxValue = 3,
            Others   = 6
        }
        #endregion

        [Parameter(HelpMessage = "Which series should be represented as slices offset from the pie.")]
        public SCPieExplodeMode? ExplodeMode { get; set; }

        [Parameter(HelpMessage = "Conditions for the points to be plotted as exploded pie slices.")]
        public object[] ExplodedPoints { get; set; }

        [Parameter(HelpMessage = "Series's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Parameter(HelpMessage = "Legend's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Ratio of the pie chart's height to its width.")]
        public double? HeightToWidthRatio { get; set; }

        [Parameter(HelpMessage = "Angle by which pie slices are rotated.")]
        public int? Rotation { get; set; }

        [Parameter(HelpMessage = "Sweep direction (clockwise or counterclockwise) for a Pie series.")]
        public PieSweepDirection? SweepDirection { get; set; }

        [Parameter(HelpMessage = "Minimum allowed size of the pie chart inside its boundaries, as a percentage.")]
        public double? MinAllowedSizePercentage { get; set; }

        [Parameter(HelpMessage = "Thickness of the entire pie in 3D Pie Chart.")]
        public int? Depth { get; set; }


        public override void SetupXtraChartSeries(ChartContext chartContext, Series series)
        {
            base.SetupXtraChartSeries(chartContext, series);

            if (series.View is PieSeriesView view)
            { 
                if (ExplodeMode.HasValue)
                    view.ExplodeMode = (PieExplodeMode)ExplodeMode.Value;
                if (HeightToWidthRatio.HasValue)
                    view.HeightToWidthRatio = HeightToWidthRatio.Value;
                if (Rotation.HasValue)
                    view.Rotation = Rotation.Value;
                if (SweepDirection.HasValue)
                    view.SweepDirection = SweepDirection.Value;
                if (MinAllowedSizePercentage.HasValue)
                    view.MinAllowedSizePercentage = MinAllowedSizePercentage.Value;

                if (ExplodedPoints != null && ExplodedPoints.Length > 0)
                {
                    view.ExplodeMode = PieExplodeMode.UseFilters;
                    view.ExplodedPointsFilters.ConjunctionMode = ConjunctionTypes.Or;

                    foreach (var explodedPoint in ExplodedPoints)
                        view.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.Equal, explodedPoint));
                }

                if (FillMode.HasValue)
                {
                    view.FillStyle.FillMode = FillMode.Value;
                    switch (FillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (view.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = FillGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
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
                    view3D.ExplodeMode = (PieExplodeMode)ExplodeMode.Value;
                if (SweepDirection.HasValue)
                    view3D.SweepDirection = SweepDirection.Value;
                if (MinAllowedSizePercentage.HasValue)
                    view3D.SizeAsPercentage = MinAllowedSizePercentage.Value;

                if (ExplodedPoints != null && ExplodedPoints.Length > 0)
                {
                    view3D.ExplodeMode = PieExplodeMode.UseFilters;
                    view3D.ExplodedPointsFilters.ConjunctionMode = ConjunctionTypes.Or;

                    foreach (var explodedPoint in ExplodedPoints)
                        view3D.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.Equal, explodedPoint));
                }

                if (FillMode.HasValue)
                {
                    switch (FillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            view3D.PieFillStyle.FillMode = FillMode3D.Solid;
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            view3D.PieFillStyle.FillMode = FillMode3D.Gradient;
                            if (view3D.PieFillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var backColor2 = Utils.ColorFromString(BackColor2);
                                if (backColor2 != Color.Empty)
                                    gradientOptions.Color2 = backColor2;
                                if (FillGradientMode.HasValue)
                                    gradientOptions.GradientMode = FillGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
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
