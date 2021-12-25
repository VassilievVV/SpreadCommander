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
    //public enum ChartAnnotationType { Chart, Pane, Series }

    [Cmdlet(VerbsCommon.Add, "ChartAnnotation", DefaultParameterSetName = "Chart")]
    public class AddChartAnnotationCmdlet : BaseChartWithContextCmdlet
    {
        [Parameter(HelpMessage = "Name of the annotation.")]
        public string Name { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Annotation's text. HTML formatting is supported.")]
        public string Text { get; set; }


        //--------------------------------
        [Parameter(ParameterSetName = "Chart", Mandatory = true, Position = 1, HelpMessage = "X-coordinate for the annotation's anchor point.")]
        public int X { get; set; }

        [Parameter (ParameterSetName = "Chart", Mandatory = true, Position = 2, HelpMessage = "Y-coordinate for the annotation's anchor point.")]
        public int Y { get; set; }

        [Parameter(ParameterSetName = "Pane", Mandatory = true, Position = 1, HelpMessage = "Pane to which the annotation is anchored. If $null - anchor to Chart.")]
        public string Pane { get; set; }

        [Parameter(ParameterSetName = "Pane", HelpMessage = "X-axis, to which the X-coordinate of the annotation's anchor point corresponds.")]
        public string AxisX { get; set; }

        [Parameter(ParameterSetName = "Pane", Mandatory = true, Position = 2, HelpMessage = "X-axis value for the annotation anchored to a pane.")]
        public object ValueX { get; set; }

        [Parameter(ParameterSetName = "Pane", HelpMessage = "Y-axis, to which the Y-coordinate of the annotation's anchor point corresponds.")]
        public string AxisY { get; set; }

        [Parameter(ParameterSetName = "Pane", Mandatory = true, Position = 3, HelpMessage = "Y-axis value for the annotation anchored to a pane.")]
        public object ValueY { get; set; }

        [Parameter(ParameterSetName = "Series", Mandatory = true, Position = 1, HelpMessage = "Series to which annotation is anchored.")]
        public string SeriesName { get; set; }

        [Parameter(ParameterSetName = "Series", Mandatory = true, Position = 2, HelpMessage = "Argument value of series to which annotation is anchored.")]
        public object Argument { get; set; }
        //--------------------------------

        [Parameter(HelpMessage = "Angle by which the annotation's shape is rotated.")]
        public int? Angle { get; set; }

        [Parameter(HelpMessage = "Annotation's background color.")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Annotation's border color.")]
        public string BorderColor { get; set; }

        [Parameter(HelpMessage = "Annotation's border thickness, in pixels.")]
        public int? BorderThickness { get; set; }

        [Parameter(HelpMessage = "Whether the Annotation's border is visible.")]
        public bool? BorderVisible { get; set; }

        [Parameter(HelpMessage = "Connector style of an annotation.")]
        public AnnotationConnectorStyle? ConnectorStyle { get; set; }

        [Parameter(HelpMessage = "Annotation's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Parameter(HelpMessage = "Annotation's filling mode.")]
        public DevExpress.XtraCharts.FillMode? FillMode { get; set; }

        [Parameter(HelpMessage = "Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Parameter(HelpMessage = "Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Parameter(HelpMessage = "Font used to display the Annotation's text.")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "Width (in pixels) of the annotation.")]
        public int? Width { get; set; }

        [Parameter(HelpMessage = "Height (in pixels) of the annotation.")]
        public int? Height { get; set; }

        [Parameter(HelpMessage = "Whether the annotation should be cut off if it doesn't fit into the diagram's dimensions.")]
        public SwitchParameter LabelMode { get; set; }

        [Parameter(HelpMessage = "Internal space between the annotation's content and its edge, in pixels.")]
        public int[] Padding { get; set; }

        [Parameter(HelpMessage = "Shadow's color.")]
        public string ShadowColor { get; set; }

        [Parameter(HelpMessage = "Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Parameter(HelpMessage = "Fillet when the annotation's shape is RoundedRectangle.")]
        public int? ShapeFillet { get; set; }

        [Parameter(HelpMessage = "Shape kind of an annotation.")]
        public ShapeKind? ShapeKind { get; set; }

        [Parameter(HelpMessage = "Angle by which the annotation is rotated around its anchor point.")]
        public double? AnchorAngle { get; set; }

        [Parameter(HelpMessage = "Length of the line connecting the annotation with its anchor point.")]
        public double? AnchorConnectorLength { get; set; }

        [Parameter(HelpMessage = "Corner of the annotation's parent element, to which the annotation is anchored.")]
        public DockCorner? AnchorDockCorner { get; set; }

        [Parameter(HelpMessage = "Pane to which the annotation's shape is docked. If null and DockCorner is specified - dock to chart.")]
        public string AnchorDockPane { get; set; }

        [Parameter(HelpMessage = "Inner indents between the edges of the annotation and its container element.")]
        public int[] DockInnerIndents { get; set; }

        [Parameter(HelpMessage = "Outer indents between the edges of the annotation and its container element.")]
        public int[] DockOuterIndents { get; set; }

        [Parameter(HelpMessage = "Alignment of the annotation's text.")]
        public StringAlignment? TextAlignment { get; set; }

        [Parameter(HelpMessage = "Z-Order of the annotation.")]
        public int? ZOrder { get; set; }


        protected override void UpdateChart()
        {
            var annotation = new TextAnnotation();
            SetupXtraChartAnnotation(annotation);
            ChartContext.Chart.AnnotationRepository.Add(annotation);
        }

        protected virtual void SetupXtraChartAnnotation(TextAnnotation annotation)
        {
            AnchorDockPane = Pane ?? AnchorDockPane;

            //Need this to link annotation to series point in BoundDataChanged
            annotation.Tag = this;

            if (!string.IsNullOrWhiteSpace(Name))
                annotation.Name = Name;

            annotation.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;

            if (Angle.HasValue)
                annotation.Angle = Angle.Value;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                annotation.BackColor = backColor;

            var borderColor = Utils.ColorFromString(BorderColor);
            if (borderColor != Color.Empty)
            {
                annotation.Border.Color = borderColor;
                annotation.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }
            if (BorderThickness.HasValue)
            {
                annotation.Border.Thickness  = BorderThickness.Value;
                annotation.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }
            if (BorderVisible.HasValue)
                annotation.Border.Visibility = BorderVisible.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.Default;

            if (FillMode.HasValue)
            {
                annotation.FillStyle.FillMode = FillMode.Value;
                switch (FillMode.Value)
                {
                    case DevExpress.XtraCharts.FillMode.Empty:
                        break;
                    case DevExpress.XtraCharts.FillMode.Solid:
                        break;
                    case DevExpress.XtraCharts.FillMode.Gradient:
                        if (annotation.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = FillGradientMode.Value;
                        }
                        break;
                    case DevExpress.XtraCharts.FillMode.Hatch:
                        if (annotation.FillStyle.Options is HatchFillOptions hatchOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                hatchOptions.Color2 = backColor2;
                            if (FillHatchStyle.HasValue)
                                hatchOptions.HatchStyle = FillHatchStyle.Value;
                        }
                        break;
                }
            }

            var font = Utils.StringToFont(Font, out Color textColor);
            if (font != null)
                annotation.Font = font;
            if (textColor != Color.Empty)
                annotation.TextColor = textColor;

            if (ConnectorStyle.HasValue)
                annotation.ConnectorStyle = ConnectorStyle.Value;

            if (Width.HasValue)
            {
                annotation.Width    = Width.Value;
                annotation.AutoSize = false;
            }
            if (Height.HasValue)
            {
                annotation.Height   = Height.Value;
                annotation.AutoSize = false;
            }

            if (Padding != null && Padding.Length == 1)
                annotation.Padding.All = Padding[0];
            else if (Padding != null && Padding.Length == 4)
            {
                annotation.Padding.Left   = Padding[0];
                annotation.Padding.Top    = Padding[1];
                annotation.Padding.Right  = Padding[2];
                annotation.Padding.Bottom = Padding[3];
            }
            else if (Padding != null)
                throw new Exception("Invalid padding. Padding shall be an array with 1 or 4 integer values.");

            var shadowColor = Utils.ColorFromString(ShadowColor);
            if (shadowColor != Color.Empty)
            {
                annotation.Shadow.Color   = shadowColor;
                annotation.Shadow.Visible = true;
            }
            if (ShadowSize.HasValue)
            {
                annotation.Shadow.Size    = ShadowSize.Value;
                annotation.Shadow.Visible = true;
            }

            if (ShapeFillet.HasValue)
                annotation.ShapeFillet = ShapeFillet.Value;
            if (ShapeKind.HasValue)
                annotation.ShapeKind = ShapeKind.Value;

            annotation.LabelMode = LabelMode;

            if (AnchorAngle.HasValue || AnchorConnectorLength.HasValue)
                annotation.ShapePosition = new RelativePosition(AnchorAngle ?? 0.0, AnchorConnectorLength ?? 0.0);
            else if (AnchorDockCorner.HasValue)
            {
                XYDiagramPane pane = null;
                if (!string.IsNullOrWhiteSpace(AnchorDockPane))
                {
                    if (ChartContext.Chart.Diagram is not XYDiagram2D diagramXY)
                        throw new Exception("Panes are available only in 2D XY charts.");

                    pane = diagramXY.Panes[AnchorDockPane];
                    if (pane == null)
                        throw new Exception($"Cannot find pane '{AnchorDockPane}'.");
                }

                var freePosition = new FreePosition();
                if (pane != null)
                    freePosition.DockTarget = pane;
                freePosition.DockCorner = AnchorDockCorner.Value;

                if (DockInnerIndents != null && DockInnerIndents.Length == 1)
                    freePosition.InnerIndents.All = DockInnerIndents[0];
                else if (DockInnerIndents != null && DockInnerIndents.Length == 4)
                {
                    freePosition.InnerIndents.Left   = DockInnerIndents[0];
                    freePosition.InnerIndents.Top    = DockInnerIndents[1];
                    freePosition.InnerIndents.Right  = DockInnerIndents[2];
                    freePosition.InnerIndents.Bottom = DockInnerIndents[3];
                }

                if (DockOuterIndents != null && DockOuterIndents.Length == 1)
                    freePosition.OuterIndents.All = DockOuterIndents[0];
                else if (DockOuterIndents != null && DockOuterIndents.Length == 4)
                {
                    freePosition.OuterIndents.Left   = DockOuterIndents[0];
                    freePosition.OuterIndents.Top    = DockOuterIndents[1];
                    freePosition.OuterIndents.Right  = DockOuterIndents[2];
                    freePosition.OuterIndents.Bottom = DockOuterIndents[3];
                }

                annotation.ShapePosition = freePosition;
            }

            annotation.Text          = Text;
            annotation.Visible       = true;
            if (TextAlignment.HasValue)
                annotation.TextAlignment = TextAlignment.Value;
            if (ZOrder.HasValue)
                annotation.ZOrder = ZOrder.Value;

            switch (ParameterSetName)
            {
                case "Chart":
                    annotation.AnchorPoint = new ChartAnchorPoint(X, Y);
                    break;
                case "Pane":
                    if (ChartContext.Chart.Diagram is not XYDiagram diagramXY)
                        throw new Exception("Panes are available only in 2D XY charts.");

                    var anchor = new PaneAnchorPoint();

                    if (!string.IsNullOrWhiteSpace(AnchorDockPane))
                    {
                        var pane = diagramXY.Panes[AnchorDockPane];
                        anchor.Pane = pane ?? throw new Exception($"Cannot find pane '{AnchorDockPane}'.");
                    }

                    var axisX = !string.IsNullOrWhiteSpace(AxisX) ? diagramXY.FindAxisXByName(AxisX) : diagramXY.AxisX;
                    var axisY = !string.IsNullOrWhiteSpace(AxisY) ? diagramXY.FindAxisYByName(AxisY) : diagramXY.AxisY;

                    anchor.AxisXCoordinate.Axis = axisX;
                    anchor.AxisXCoordinate.AxisValue = ValueX;

                    anchor.AxisYCoordinate.Axis = axisY;
                    anchor.AxisYCoordinate.AxisValue = ValueY;
                    break;
            }
        }

        public virtual void BoundDataChanged(ChartContext chartContext, Annotation annotation)
        {
            switch (ParameterSetName)
            {
                case "Series":
                    var series = chartContext.Chart.Series[SeriesName] ?? throw new Exception($"Cannot find series '{SeriesName}'.");

                    SeriesPoint annotationPoint = null;
                    switch (series.ActualArgumentScaleType)
                    {
                        case ScaleType.Qualitative:
                            var strArgument = Convert.ToString(Argument);
                            foreach (SeriesPoint point in series.Points)
                            {
                                if (point.Argument == strArgument)
                                {
                                    annotationPoint = point;
                                    break;
                                }
                            }
                            break;
                        case ScaleType.Numerical:
                            var numArgument = Convert.ToDouble(Argument);
                            foreach (SeriesPoint point in series.Points)
                            {
                                if (point.NumericalArgument == numArgument)
                                {
                                    annotationPoint = point;
                                    break;
                                }
                            }
                            break;
                        case ScaleType.DateTime:
                            var dateArgument = Convert.ToDateTime(Argument);
                            foreach (SeriesPoint point in series.Points)
                            {
                                if (point.DateTimeArgument == dateArgument)
                                {
                                    annotationPoint = point;
                                    break;
                                }
                            }
                            break;
                        case ScaleType.Auto:
                            throw new Exception("Cannot determine series scale type to add annotation.");
                    }

                    if (annotationPoint == null)
                        throw new Exception("Cannot find anchor point for annotation.");

                    annotation.AnchorPoint = new SeriesPointAnchorPoint(annotationPoint);
                    break;
            }
        }
    }
}
