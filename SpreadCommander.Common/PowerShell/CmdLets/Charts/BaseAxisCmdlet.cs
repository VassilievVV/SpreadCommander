using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    public enum ChartAxisType { X, Y }

    public class BaseAxisCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Axis type - X or Y.")]
        public ChartAxisType AxisType { get; set; }

        [Parameter(HelpMessage = "Aggregate function that should be used to relieve data.")]
        public AggregateFunction? AggregateFunction { get; set; }

        [Parameter(HelpMessage = "Whether the alignment, spacing and offset of grid lines and major tickmarks should be calculated automatically.")]
        public SwitchParameter NoAutoGrid { get; set; }

        [Parameter(HelpMessage = "Offset of grid lines and major tickmarks.")]
        public double? GridOffset { get; set; }

        [Parameter(HelpMessage = "Public double GridSpacing { get; set; }")]
        public double? GridSpacing { get; set; }

        [Parameter(HelpMessage = "Factor on the measurement unit multiplies to form a custom measurement unit.")]
        public int? MeasureUnitMultiplier { get; set; }

        [Parameter(HelpMessage = "Minimum allowed distance between two neighbor major tick marks and grid lines that affect an automatically calculated grid.")]
        public int? MinGridSpacingLength { get; set; }

        [Parameter(HelpMessage = "Action the chart control should perform with the missing points.")]
        public ProcessMissingPointsMode? ProcessMissingPoints { get; set; }

        [Parameter(HelpMessage = "Scale mode for an axis.")]
        public ScaleMode? ScaleMode { get; set; }

        //DateTimeScaleOptions
        [Parameter(HelpMessage = "Date-time measure unit to which the beginning of an axis' gridlines and labels should be aligned.")]
        public DateTimeGridAlignment? DateTimeGridAlignment { get; set; }

        [Parameter(HelpMessage = "Detail level for date-time values.")]
        public DateTimeMeasureUnit? DateTimeMeasureUnit { get; set; }    //set ScaleMode to Manual


        //NumericScaleOptions
        [Parameter(HelpMessage = "Custom numeric unit to which axis gridlines should be aligned.")]
        public double? NumericGridAlignment { get; set; }

        [Parameter(HelpMessage = "Custom measure unit level for numeric values.")]
        public double? NumericMeasureUnit { get; set; }

        //Grid lines
        [Parameter(HelpMessage = "Color of major grid lines.")]
        public string MajorLineColor { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint the major line.")]
        public DevExpress.XtraCharts.DashStyle? MajorLineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive major lines.")]
        public LineJoin? MajorLineJoin { get; set; }

        [Parameter(HelpMessage = "Major line's thickness.")]
        public int? MajorLineThickness { get; set; }

        [Parameter(HelpMessage = "Color of minor grid lines.")]
        public string MinorLineColor { get; set; }

        [Parameter(HelpMessage = "Dash style used to paint minor line.")]
        public DevExpress.XtraCharts.DashStyle? MinorLineDashStyle { get; set; }

        [Parameter(HelpMessage = "Join style for the ends of consecutive minor lines.")]
        public LineJoin? MinorLineJoin { get; set; }

        [Parameter(HelpMessage = "Minor line's thickness.")]
        public int? MinorLineThickness { get; set; }

        [Parameter(HelpMessage = "Whether the grid lines are visible.")]
        public SwitchParameter ShowLines { get; set; }

        [Parameter(HelpMessage = "Whether the minor grid lines are displayed by an axis.")]
        public SwitchParameter ShowMinorLines { get; set; }


        [Parameter(HelpMessage = "Whether interlacing is applied to the axis.")]
        public SwitchParameter Interlaced { get; set; }

        [Parameter(HelpMessage = "Interlaced color.")]
        public string InterlacedColor { get; set; }

        [Parameter(HelpMessage = "Whether the axis should display its numerical values using a logarithmic scale.")]
        [Alias("Log")]
        public SwitchParameter Logarithmic { get; set; }

        [Parameter(HelpMessage = "Logarithmic base ")]
        [Alias("LogBase")]
        public double? LogarithmicBase { get; set; }

        [Parameter(HelpMessage = "Number of minor tickmarks and grid lines.")]
        public int? MinorCount { get; set; }

        //Whole range - set WholeRange.Auto
        [Parameter(HelpMessage = "Whether or not an axis zero value should be displayed.")]
        public SwitchParameter AlwaysShowZeroLevel { get; set; }

        [Parameter(HelpMessage = "Minimum value to display on an Axis. This may not be the same as the minimum value of the series associated with this axis.")]
        [Alias("Min")]
        public object MinValue { get; set; }

        [Parameter(HelpMessage = "Maximum value to display on an Axis. This may not be the same as the maximum value of the series associated with this axis.")]
        [Alias("Max")]
        public object MaxValue { get; set; }

        [Parameter(HelpMessage = "Space between the outermost series point and the diagram's edge.")]
        public double? SideMarginsValue { get; set; }

        //2D Axis
        [Parameter(HelpMessage = "Position of an axis relative to another primary axis. For use with 2D charts.")]
        public AxisAlignment? Alignment { get; set; }

        [Parameter(HelpMessage = "Axis color. For use with 2D charts.")]
        public string Color { get; set; }

        [Parameter(HelpMessage = "Interlaced 2nd background color, if FillMode is gradient or hatch.")]
        public string InterlacedColor2 { get; set; }

        [Parameter(HelpMessage = "Pane's filling mode.")]
        public DevExpress.XtraCharts.FillMode? InterlacedFillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? InterlacedGradientMode { get; set; }

        [Parameter(HelpMessage = "Background gradient's direction of the marker.")]
        public PolygonGradientMode? InterlacedRadarGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? InterlacedHatchStyle { get; set; }

        [Parameter(HelpMessage = "Visibility mode of the axis labels. For use with 2D charts.")]
        public AxisLabelVisibilityMode? LabelVisibilityMode { get; set; }

        [Parameter(HelpMessage = "Thickness (in pixels) of the axis. For use with 2D charts.")]
        public int? Thickness { get; set; }

        //Tickmarks
        [Parameter(HelpMessage = "Whether tickmarks should be shown across an axis. For use with 2D charts.")]
        public SwitchParameter TickmarkCrossAxis { get; set; }

        [Parameter(HelpMessage = "Length of major tickmarks. For use with 2D charts.")]
        public int? MajorTickmarkLength { get; set; }

        [Parameter(HelpMessage = "Thickness of the major tickmarks. For use with 2D charts.")]
        public int? MajorTickmarkThickness { get; set; }

        [Parameter(HelpMessage = "Length of minor tickmarks. For use with 2D charts.")]
        public int? MinorTickmarkLength { get; set; }

        [Parameter(HelpMessage = "Thickness of the minor tickmarks. For use with 2D charts.")]
        public int? MinorTickmarkThickness { get; set; }

        [Parameter(HelpMessage = "Whether major tickmarks are displayed on an axis. For use with 2D charts.")]
        public SwitchParameter HideMajorTickmarks { get; set; }

        [Parameter(HelpMessage = "Whether minor tickmarks are displayed on an axis. For use with 2D charts.")]
        public SwitchParameter HideMinorTickmarks { get; set; }

        [Parameter(HelpMessage = "Whether to show X and Y-axes on a diagram. For use with 2D charts.")]
        public bool? Visibility { get; set; }

        [Parameter(HelpMessage = "List of panes in which to display the current axis. For use with 2D charts.")]
        public string[] VisibleInPanes { get; set; }

        [Parameter(HelpMessage = "List of panes in which to hide the current axis. For use with 2D charts.")]
        public string[] HiddenInPanes { get; set; }

        //Axis
        [Parameter(HelpMessage = "Whether to enable automatic scale breaks. For use with 2D charts.")]
        [Alias("Breaks")]
        public SwitchParameter AutoScaleBreaks { get; set; }

        [Parameter(HelpMessage = "Maximum number of automatic scale breaks. For use with 2D charts.")]
        [Alias("MaxBreaks")]
        public int? AutoScaleBreaksMaxCount { get; set; }

        [Parameter(HelpMessage = "Color of the axis' scale breaks. For use with 2D charts.")]
        public string ScaleBreakColor { get; set; }

        [Parameter(HelpMessage = "Size of scale breaks. For use with 2D charts.")]
        public int? ScaleBreakSize { get; set; }

        [Parameter(HelpMessage = "Style in which scale breaks' edges are drawn. For use with 2D charts.")]
        public ScaleBreakStyle? ScaleBreakStyle { get; set; }

        [Parameter(HelpMessage = "Whether the axis is reversed. For use with 2D charts.")]
        public SwitchParameter Reverse { get; set; }



        protected virtual AxisBase GetAxis()
        {
            return null;
        }

        protected override void UpdateChart()
        {
            var axis = GetAxis();
            if (axis == null)
                throw new Exception("Cannot determine chart's axis to apply settings.");
            SetupXtraChartsAxis(axis);

            ChartContext.CurrentAxis = axis;
        }

        internal static AxisBase GetPrimaryAxis(Diagram diagram, ChartAxisType axisType)
        {
            switch (diagram)
            {
                case XYDiagram xyDiagram:
                    switch (axisType)
                    {
                        case ChartAxisType.X:
                            return xyDiagram.AxisX;
                        case ChartAxisType.Y:
                            return xyDiagram.AxisY;
                    }
                    break;
                case SwiftPlotDiagram swiftPlotDiagram:
                    switch (axisType)
                    {
                        case ChartAxisType.X:
                            return swiftPlotDiagram.AxisX;
                        case ChartAxisType.Y:
                            return swiftPlotDiagram.AxisY;
                    }
                    break;
                case XYDiagram3D xyDiagram3D:
                    switch (axisType)
                    {
                        case ChartAxisType.X:
                            return xyDiagram3D.AxisX;
                        case ChartAxisType.Y:
                            return xyDiagram3D.AxisY;
                    }
                    break;
                case RadarDiagram radarDiagram:
                    switch (axisType)
                    {
                        case ChartAxisType.X:
                            return radarDiagram.AxisX;
                        case ChartAxisType.Y:
                            return radarDiagram.AxisY;
                    }
                    break;
                case SimpleDiagram _:
                    throw new Exception("Pie chart does not support axes.");
                case SimpleDiagram3D _:
                    throw new Exception("Pie chart does not support axes.");
            }

            throw new Exception("Current chart does not support axes.");
        }

        internal static AxisBase GetSecondaryAxis(Diagram diagram, ChartAxisType axisType, string name)
        {
            switch (diagram)
            {
                case XYDiagram xyDiagram:
                    switch (axisType)
                    {
                        case ChartAxisType.X:
                            var resultX = new SecondaryAxisX(name);
                            xyDiagram.SecondaryAxesX.Add(resultX);
                            return resultX;
                        case ChartAxisType.Y:
                            var resultY = new SecondaryAxisY(name);
                            xyDiagram.SecondaryAxesY.Add(resultY);
                            return resultY;
                    }
                    break;
                case SwiftPlotDiagram swiftPlotDiagram:
                    switch (axisType)
                    {
                        case ChartAxisType.X:
                            var resultX = new SwiftPlotDiagramSecondaryAxisX(name);
                            swiftPlotDiagram.SecondaryAxesX.Add(resultX);
                            return resultX;
                        case ChartAxisType.Y:
                            var resultY = new SwiftPlotDiagramSecondaryAxisY(name);
                            swiftPlotDiagram.SecondaryAxesY.Add(resultY);
                            return resultY;
                    }
                    break;
                case XYDiagram3D _:
                    throw new Exception("3D charts do not support secondary axes.");
                case RadarDiagram _:
                    throw new Exception("Radar chart does not support secondary axes.");
                case SimpleDiagram _:
                    throw new Exception("Pie chart does not support axes.");
                case SimpleDiagram3D _:
                    throw new Exception("Pie chart does not support axes.");
            }

            throw new Exception("Current chart does not support axes.");
        }

        protected virtual void SetupXtraChartsAxis(AxisBase axis)
        {
            var axisX      = axis as AxisXBase;
            var axisX3D    = axis as AxisX3D;
            var axisXRadar = axis as RadarAxisX;

            //Y axis does not use QualitativeScaleOptions
            /*
            var axisY      = axis as AxisYBase;
            var axisY3D    = axis as AxisY3D;
            var axisYRadar = axis as RadarAxisY;
            */

            if (AggregateFunction.HasValue)
            {
                axis.DateTimeScaleOptions.AggregateFunction = AggregateFunction.Value;
                axis.NumericScaleOptions.AggregateFunction  = AggregateFunction.Value;
                if (axisX != null)
                    axisX.QualitativeScaleOptions.AggregateFunction = AggregateFunction.Value;
            }

            axis.DateTimeScaleOptions.AutoGrid = !NoAutoGrid;
            axis.NumericScaleOptions.AutoGrid  = !NoAutoGrid;
            if (axisX != null)
                axisX.QualitativeScaleOptions.AutoGrid = !NoAutoGrid;
            if (axisX3D != null)
                axisX3D.QualitativeScaleOptions.AutoGrid = !NoAutoGrid;
            if (axisXRadar != null)
                axisXRadar.QualitativeScaleOptions.AutoGrid = !NoAutoGrid;

            if (ScaleMode.HasValue)
            {
                axis.DateTimeScaleOptions.ScaleMode = ScaleMode.Value;
                axis.NumericScaleOptions.ScaleMode  = ScaleMode.Value;
            }

            if (GridOffset.HasValue)
            {
                axis.DateTimeScaleOptions.GridOffset = GridOffset.Value;
                axis.NumericScaleOptions.GridOffset  = GridOffset.Value;
                if (axisX != null)
                    axisX.QualitativeScaleOptions.GridOffset = GridOffset.Value;
                if (axisX3D != null)
                    axisX3D.QualitativeScaleOptions.GridOffset = GridOffset.Value;
                if (axisXRadar != null)
                    axisXRadar.QualitativeScaleOptions.GridOffset = GridOffset.Value;
            }

            if (GridSpacing.HasValue)
            {
                axis.DateTimeScaleOptions.GridSpacing = GridSpacing.Value;
                axis.NumericScaleOptions.GridSpacing  = GridSpacing.Value;
                if (axisX != null)
                    axisX.QualitativeScaleOptions.GridSpacing = GridSpacing.Value;
                if (axisX3D != null)
                    axisX3D.QualitativeScaleOptions.GridSpacing = GridSpacing.Value;
                if (axisXRadar != null)
                    axisXRadar.QualitativeScaleOptions.GridSpacing = GridSpacing.Value;
            }

            if (MeasureUnitMultiplier.HasValue)
            {
                axis.DateTimeScaleOptions.MeasureUnitMultiplier = MeasureUnitMultiplier.Value;
            }

            if (MinGridSpacingLength.HasValue)
            {
                axis.DateTimeScaleOptions.MinGridSpacingLength = MinGridSpacingLength.Value;
                axis.NumericScaleOptions.MinGridSpacingLength  = MinGridSpacingLength.Value;
                if (axisX != null)
                    axisX.QualitativeScaleOptions.MinGridSpacingLength = MinGridSpacingLength.Value;
                if (axisX3D != null)
                    axisX3D.QualitativeScaleOptions.MinGridSpacingLength = MinGridSpacingLength.Value;
                if (axisXRadar != null)
                    axisXRadar.QualitativeScaleOptions.MinGridSpacingLength = MinGridSpacingLength.Value;
            }

            if (ProcessMissingPoints.HasValue)
            { 
                axis.DateTimeScaleOptions.ProcessMissingPoints = ProcessMissingPoints.Value;
                axis.NumericScaleOptions.ProcessMissingPoints  = ProcessMissingPoints.Value;
            }

            if (DateTimeGridAlignment.HasValue)
                axis.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Value;

            if (DateTimeMeasureUnit.HasValue)
                axis.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Value;

            if (NumericGridAlignment.HasValue)
                axis.NumericScaleOptions.CustomGridAlignment = NumericGridAlignment.Value;

            if (NumericMeasureUnit.HasValue)
                axis.NumericScaleOptions.CustomMeasureUnit = NumericMeasureUnit.Value;

            var majorLineColor = Utils.ColorFromString(MajorLineColor);
            if (majorLineColor != System.Drawing.Color.Empty)
            {
                axis.GridLines.Color   = majorLineColor;
                axis.GridLines.Visible = true;
            }
            if (MajorLineDashStyle.HasValue)
            {
                axis.GridLines.LineStyle.DashStyle = MajorLineDashStyle.Value;
                axis.GridLines.Visible             = true;
            }
            if (MajorLineJoin.HasValue)
            {
                axis.GridLines.LineStyle.LineJoin = MajorLineJoin.Value;
                axis.GridLines.Visible            = true;
            }
            if (MajorLineThickness.HasValue)
            {
                axis.GridLines.LineStyle.Thickness = MajorLineThickness.Value;
                axis.GridLines.Visible             = true;
            }

            var minorLineColor = Utils.ColorFromString(MinorLineColor);
            if (minorLineColor != System.Drawing.Color.Empty)
            {
                axis.GridLines.MinorColor   = minorLineColor;
                axis.GridLines.MinorVisible = true;
            }
            if (MinorLineDashStyle.HasValue)
            {
                axis.GridLines.MinorLineStyle.DashStyle = MinorLineDashStyle.Value;
                axis.GridLines.MinorVisible             = true;
            }
            if (MinorLineJoin.HasValue)
            {
                axis.GridLines.MinorLineStyle.LineJoin = MinorLineJoin.Value;
                axis.GridLines.MinorVisible            = true;
            }
            if (MinorLineThickness.HasValue)
            {
                axis.GridLines.MinorLineStyle.Thickness = MinorLineThickness.Value;
                axis.GridLines.MinorVisible             = true;
            }
            if (ShowLines)
                axis.GridLines.Visible = true;
            if (ShowMinorLines)
                axis.GridLines.MinorVisible = true;

            if (Interlaced)
                axis.Interlaced = true;

            var interlacedColor = Utils.ColorFromString(InterlacedColor);
            if (interlacedColor != System.Drawing.Color.Empty)
                axis.InterlacedColor = interlacedColor;

            if (Logarithmic)
                axis.Logarithmic = true;
            if (LogarithmicBase.HasValue)
                axis.LogarithmicBase = LogarithmicBase.Value;

            if (MinorCount.HasValue)
                axis.MinorCount = MinorCount.Value;

            if (AlwaysShowZeroLevel)
                axis.WholeRange.AlwaysShowZeroLevel = true;

            if (MinValue != null)
                axis.WholeRange.MinValue = MinValue;
            if (MaxValue != null)
                axis.WholeRange.MaxValue = MaxValue;

            if (SideMarginsValue.HasValue)
            {
                axis.WholeRange.SideMarginsValue = SideMarginsValue.Value;
                axis.WholeRange.AutoSideMargins  = false;
            }


            if (axis is Axis2D axis2D)
            {
                if (Alignment.HasValue)
                    axis2D.Alignment = Alignment.Value;

                var color = Utils.ColorFromString(Color);
                if (color != System.Drawing.Color.Empty)
                    axis2D.Color = color;

                if (InterlacedFillMode.HasValue)
                {
                    axis2D.InterlacedFillStyle.FillMode = InterlacedFillMode.Value;
                    switch (InterlacedFillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (axis2D.InterlacedFillStyle.Options is RectangleGradientFillOptions gradientOptions)
                            {
                                var color2 = Utils.ColorFromString(InterlacedColor2);
                                if (color2 != System.Drawing.Color.Empty)
                                    gradientOptions.Color2 = color2;
                                if (InterlacedGradientMode.HasValue)
                                    gradientOptions.GradientMode = InterlacedGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (axis2D.InterlacedFillStyle.Options is HatchFillOptions hatchOptions)
                            {
                                var color2 = Utils.ColorFromString(InterlacedColor2);
                                if (color2 != System.Drawing.Color.Empty)
                                    hatchOptions.Color2 = color2;
                                if (InterlacedHatchStyle.HasValue)
                                hatchOptions.HatchStyle = InterlacedHatchStyle.Value;
                            }
                            break;
                    }
                }

                if (LabelVisibilityMode.HasValue)
                    axis2D.LabelVisibilityMode = LabelVisibilityMode.Value;

                if (Thickness.HasValue)
                    axis2D.Thickness = Thickness.Value;

                if (TickmarkCrossAxis)
                    axis2D.Tickmarks.CrossAxis = true;

                if (MajorTickmarkLength.HasValue)
                    axis2D.Tickmarks.Length = MajorTickmarkLength.Value;
                if (MajorTickmarkThickness.HasValue)
                    axis2D.Tickmarks.Thickness = MajorTickmarkThickness.Value;
                if (MinorTickmarkLength.HasValue)
                    axis2D.Tickmarks.Length = MinorTickmarkLength.Value;
                if (MinorTickmarkThickness.HasValue)
                    axis2D.Tickmarks.Thickness = MinorTickmarkThickness.Value;
                if (HideMajorTickmarks)
                    axis2D.Tickmarks.Visible = false;
                if (HideMinorTickmarks)
                    axis2D.Tickmarks.MinorVisible = false;

                if (Visibility.HasValue)
                    axis2D.Visibility = Visibility.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
                if (VisibleInPanes != null && VisibleInPanes.Length > 0)
                {
                    if (ChartContext.Chart.Diagram is not XYDiagram chartDiagram)
                        throw new Exception("Property VisibleInPanes can be set only in 2D XY charts.");

                    foreach (var paneName in VisibleInPanes)
                    {
                        var pane = chartDiagram.Panes[paneName];
                        if (pane == null)
                            throw new Exception($"Cannot find pane '{paneName}' to setup VisibleInPanes.");

                        axis2D.SetVisibilityInPane(true, pane);
                    }
                }
                if (HiddenInPanes != null && HiddenInPanes.Length > 0)
                {
                    if (ChartContext.Chart.Diagram is not XYDiagram chartDiagram)
                        throw new Exception("Property HiddenInPanes can be set only in 2D XY charts.");

                    foreach (var paneName in HiddenInPanes)
                    {
                        var pane = chartDiagram.Panes[paneName];
                        if (pane == null)
                            throw new Exception($"Cannot find pane '{paneName}' to setup HiddenInPanes.");

                        axis2D.SetVisibilityInPane(false, pane);
                    }
                }
            }

            if (axis is Axis axisEx)
            {
                if (AutoScaleBreaks)
                    axisEx.AutoScaleBreaks.Enabled = true;
                if (AutoScaleBreaksMaxCount.HasValue)
                {
                    axisEx.AutoScaleBreaks.MaxCount = AutoScaleBreaksMaxCount.Value;
                    axisEx.AutoScaleBreaks.Enabled  = true;
                }

                var scaleBreakColor = Utils.ColorFromString(ScaleBreakColor);
                if (scaleBreakColor != System.Drawing.Color.Empty)
                    axisEx.ScaleBreakOptions.Color = scaleBreakColor;

                if (ScaleBreakSize.HasValue)
                    axisEx.ScaleBreakOptions.SizeInPixels = ScaleBreakSize.Value;

                if (ScaleBreakStyle.HasValue)
                    axisEx.ScaleBreakOptions.Style = ScaleBreakStyle.Value;

                if (Reverse)
                    axisEx.Reverse = Reverse;
            }

            if (axis is RadarAxis axisRadar)
            {
                if (InterlacedFillMode.HasValue)
                {
                    axisRadar.InterlacedFillStyle.FillMode = InterlacedFillMode.Value;
                    switch (InterlacedFillMode.Value)
                    {
                        case DevExpress.XtraCharts.FillMode.Empty:
                            break;
                        case DevExpress.XtraCharts.FillMode.Solid:
                            break;
                        case DevExpress.XtraCharts.FillMode.Gradient:
                            if (axisRadar.InterlacedFillStyle.Options is PolygonGradientFillOptions polygonOptions)
                            {
                                var color2 = Utils.ColorFromString(InterlacedColor2);
                                if (color2 != System.Drawing.Color.Empty)
                                    polygonOptions.Color2 = color2;
                                if (InterlacedGradientMode.HasValue)
                                    polygonOptions.GradientMode = InterlacedRadarGradientMode.Value;
                            }
                            break;
                        case DevExpress.XtraCharts.FillMode.Hatch:
                            if (axisRadar.InterlacedFillStyle.Options is HatchFillOptions hatchOptions)
                            {
                                var color2 = Utils.ColorFromString(InterlacedColor2);
                                if (color2 != System.Drawing.Color.Empty)
                                    hatchOptions.Color2 = color2;
                                if (InterlacedHatchStyle.HasValue)
                                    hatchOptions.HatchStyle = InterlacedHatchStyle.Value;
                            }
                            break;
                    }
                }
            }
        }
    }
}
