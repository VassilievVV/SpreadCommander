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

namespace SpreadCommander.Common.Script.Chart
{
    public class AnnotationOptions
    {
        [Description("Angle by which the annotation's shape is rotated.")]
        public int? Angle { get; set; }

        [Description("Annotation's background color.")]
        public string BackColor { get; set; }

        [Description("Annotation's border color.")]
        public string BorderColor { get; set; }

        [Description("Annotation's border thickness, in pixels.")]
        public int? BorderThickness { get; set; }

        [Description("Whether the Annotation's border is visible.")]
        public bool? BorderVisible { get; set; }

        [Description("Connector style of an annotation.")]
        public AnnotationConnectorStyle? ConnectorStyle { get; set; }

        [Description("Annotation's 2nd background color, if FillMode is gradient or hatch.")]
        public string BackColor2 { get; set; }

        [Description("Annotation's filling mode.")]
        public FillMode? FillMode { get; set; }

        [Description("Direction of a linear gradient, if FillMode is gradient.")]
        public RectangleGradientMode? FillGradientMode { get; set; }

        [Description("Hatch style used for background filling.")]
        public HatchStyle? FillHatchStyle { get; set; }

        [Description("Font used to display the Annotation's text.")]
        public string Font { get; set; }

        [Description("Width (in pixels) of the annotation.")]
        public int? Width { get; set; }

        [Description("Height (in pixels) of the annotation.")]
        public int? Height { get; set; }

        [Description("Whether the annotation should be cut off if it doesn't fit into the diagram's dimensions.")]
        public bool LabelMode { get; set; }

        [Description("Internal space between the annotation's content and its edge, in pixels.")]
        public int[] Padding { get; set; }

        [Description("Shadow's color.")]
        public string ShadowColor { get; set; }

        [Description("Shadow's size.")]
        public int? ShadowSize { get; set; }

        [Description("Fillet when the annotation's shape is RoundedRectangle.")]
        public int? ShapeFillet { get; set; }

        [Description("Shape kind of an annotation.")]
        public ShapeKind? ShapeKind { get; set; }

        [Description("Angle by which the annotation is rotated around its anchor point.")]
        public double? AnchorAngle { get; set; }

        [Description("Length of the line connecting the annotation with its anchor point.")]
        public double? AnchorConnectorLength { get; set; }

        [Description("Corner of the annotation's parent element, to which the annotation is anchored.")]
        public DockCorner? AnchorDockCorner { get; set; }

        [Description("Pane to which the annotation's shape is docked. If null and DockCorner is specified - dock to chart.")]
        public string AnchorDockPane { get; set; }

        [Description("Inner indents between the edges of the annotation and its container element.")]
        public int[] DockInnerIndents { get; set; }

        [Description("Outer indents between the edges of the annotation and its container element.")]
        public int[] DockOuterIndents { get; set; }

        [Description("Alignment of the annotation's text.")]
        public StringAlignment? TextAlignment { get; set; }

        [Description("Z-Order of the annotation.")]
        public int? ZOrder { get; set; }


        //For internal use
        protected internal string SeriesName { get; set; }
        protected internal object Argument { get; set; }



        protected internal virtual void SetupXtraChartAnnotation(SCChart chart, string name, string text, TextAnnotation annotation)
        {
            //Need this to link annotation to series point in BoundDataChanged
            annotation.Tag = this;

            if (!string.IsNullOrWhiteSpace(name))
                annotation.Name = name;

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
                annotation.Border.Thickness = BorderThickness.Value;
                annotation.Border.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }
            if (BorderVisible.HasValue)
                annotation.Border.Visibility = BorderVisible.Value ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.Default;

            if (FillMode.HasValue)
            {
                annotation.FillStyle.FillMode = (DevExpress.XtraCharts.FillMode)FillMode.Value;
                switch (FillMode.Value)
                {
                    case Chart.FillMode.Empty:
                        break;
                    case Chart.FillMode.Solid:
                        break;
                    case Chart.FillMode.Gradient:
                        if (annotation.FillStyle.Options is RectangleGradientFillOptions gradientOptions)
                        {
                            var backColor2 = Utils.ColorFromString(BackColor2);
                            if (backColor2 != System.Drawing.Color.Empty)
                                gradientOptions.Color2 = backColor2;
                            if (FillGradientMode.HasValue)
                                gradientOptions.GradientMode = (DevExpress.XtraCharts.RectangleGradientMode)FillGradientMode.Value;
                        }
                        break;
                    case Chart.FillMode.Hatch:
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
                    if (chart.Chart.Diagram is not XYDiagram2D diagramXY)
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

            annotation.Text = text;
            annotation.Visible = true;
            if (TextAlignment.HasValue)
                annotation.TextAlignment = TextAlignment.Value;
            if (ZOrder.HasValue)
                annotation.ZOrder = ZOrder.Value;
         }

        protected internal virtual void BoundDataChanged(SCChart chart, Annotation annotation)
        {
            if (string.IsNullOrWhiteSpace(SeriesName) || Argument == null)
                return;

            var series = chart.Chart.Series[SeriesName] ?? throw new Exception($"Cannot find series '{SeriesName}'.");

            SeriesPoint annotationPoint = null;
            switch (series.ActualArgumentScaleType)
            {
                case DevExpress.XtraCharts.ScaleType.Qualitative:
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
                case DevExpress.XtraCharts.ScaleType.Numerical:
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
                case DevExpress.XtraCharts.ScaleType.DateTime:
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
                case DevExpress.XtraCharts.ScaleType.Auto:
                    throw new Exception("Cannot determine series scale type to add annotation.");
            }

            if (annotationPoint == null)
                throw new Exception("Cannot find anchor point for annotation.");

            annotation.AnchorPoint = new SeriesPointAnchorPoint(annotationPoint);
        }
    }

    public partial class SCChart
    {
        public SCChart AddAnnotation(string name, string text, int x, int y,
            AnnotationOptions options = null)
        {
            options ??= new AnnotationOptions();
            var annotation = new TextAnnotation();
            options.SetupXtraChartAnnotation(this, name, text, annotation);
            annotation.AnchorPoint = new ChartAnchorPoint(x, y);

            return this;
        }

        public SCChart AddPaneAnnotation(string name, string text, string pane, string valueX, string valueY,
            string axisX = null, string axisY = null,
            AnnotationOptions options = null)
        {
            options ??= new AnnotationOptions();
            options.AnchorDockPane = pane;
            var annotation = new TextAnnotation();
            options.SetupXtraChartAnnotation(this, name, text, annotation);

            if (Chart.Diagram is not XYDiagram diagramXY)
                throw new Exception("Panes are available only in 2D XY charts.");

            var anchor = new PaneAnchorPoint();

            if (!string.IsNullOrWhiteSpace(pane))
            {
                var chartPane = diagramXY.Panes[pane];
                anchor.Pane = chartPane ?? throw new Exception($"Cannot find pane '{pane}'.");
            }

            var axX = !string.IsNullOrWhiteSpace(axisX) ? diagramXY.FindAxisXByName(axisX) : diagramXY.AxisX;
            var axY = !string.IsNullOrWhiteSpace(axisY) ? diagramXY.FindAxisYByName(axisY) : diagramXY.AxisY;

            anchor.AxisXCoordinate.Axis      = axX;
            anchor.AxisXCoordinate.AxisValue = valueX;

            anchor.AxisYCoordinate.Axis      = axY;
            anchor.AxisYCoordinate.AxisValue = valueY;

            return this;
        }

        public SCChart AddSeriesAnnotation(string name, string text, string seriesName, string argument,
            AnnotationOptions options = null)
        {
            options ??= new AnnotationOptions();
            options.SeriesName = seriesName;
            options.Argument   = argument;
            var annotation     = new TextAnnotation();
            options.SetupXtraChartAnnotation(this, name, text, annotation);

            return this;
        }
    }
}
