using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using System.Drawing;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Add, "BookShape")]
    public class AddBookShapeCmdlet: BaseBookWithCommentsCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of a shape")]
        public string Name { get; set; }

        [Parameter(Position = 1, HelpMessage = "Text for the shape")]
        public string Text { get; set; }

        [Parameter(HelpMessage = "If set - Text is treated as HTML")]
        public SwitchParameter Html { get; set; }

        [Parameter(HelpMessage = "When set - retains styles associated with the target document when style definition conflicts occur.")]
        [Alias("DestStyles", "ds")]
        public SwitchParameter UseDestinationStyles { get; set; }

        [Parameter(HelpMessage = "Fill color")]
        public string FillColor { get; set; }

        [Parameter(HelpMessage = "How the shape is positioned horizontally")]
        public ShapeHorizontalAlignment? HorizontalAlignment { get; set; }

        [Parameter(HelpMessage = "How the shape is positioned vertically")]
        public ShapeVerticalAlignment? VerticalAlignment { get; set; }

        [Parameter(HelpMessage = "Color of the shape's border")]
        public string LineColor { get; set; }

        [Parameter(HelpMessage = "Thickness of the specified line in points")]
        public float? LineThickness { get; set; }

        [Parameter(HelpMessage = "Whether you can change the height and width of a shape independently when you resize it")]
        public SwitchParameter LockAspectRatio { get; set; }

        [Parameter(HelpMessage = "Margins of the shape. 1 or 4 elements")]
        public float[] Margins { get; set; }

        [Parameter(HelpMessage = "Shape position relative to a certain element of the document layout. 2 elements - X and Y")]
        public float[] Offset { get; set; }

        [Parameter(HelpMessage = "Image's filename")]
        public string ImageFile { get; set; }

        [Parameter(HelpMessage = "Item to what the horizontal position of a shape is relative")]
        public ShapeRelativeHorizontalPosition? RelativeHorizontalPosition { get; set; }

        [Parameter(HelpMessage = "Item to what the vertical position of a shape is relative")]
        public ShapeRelativeVerticalPosition? RelativeVerticalPosition { get; set; }

        [Parameter(HelpMessage = "Number of degrees the shape is rotated around the z-axis")]
        public float? RotationAngle { get; set; }

        [Parameter(HelpMessage = "X-axis scale factor")]
        public float? ScaleX { get; set; }

        [Parameter(HelpMessage = "Y-axis scale factor")]
        public float? ScaleY { get; set; }

        [Parameter(HelpMessage = "Size of a shape")]
        public float[] Size { get; set; }

        [Parameter(HelpMessage = "How the shape is surrounded by the text")]
        public TextWrappingType? TextWrapping { get; set; }

        [Parameter(HelpMessage = "How text should wrap around the shape's sides")]
        public TextWrappingSide? TextWrappingSide { get; set; }

        [Parameter(HelpMessage = "Position of the shape in the z-order")]
        public int? ZOrder { get; set; }


        protected override void ProcessRecord()
        {
            var book = GetCmdletBook();
            ExecuteSynchronized(() =>
            {
                using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
                {
                    Shape shape;

                    if (!string.IsNullOrWhiteSpace(ImageFile))
                    {
                        var imageFile = Project.Current.MapPath(ImageFile);
                        var image     = Image.FromFile(imageFile);
                        shape         = book.Shapes.InsertPicture(book.Range.End, image);
                    }
                    else
                        shape = book.Shapes.InsertTextBox(book.Range.End);

                    SetupShape(book, shape);
                }
            });
        }

        protected virtual void SetupShape(Document book, Shape shape)
        {
            if (Text != null)
            {
                if (Html)
                    shape.TextBox.Document.AppendHtmlText(Text,
                        UseDestinationStyles ? InsertOptions.UseDestinationStyles : InsertOptions.KeepSourceFormatting);
                else
                    shape.TextBox.Document.AppendText(Text);
            }

            if (!string.IsNullOrWhiteSpace(Name))
                shape.Name = Name;

            var fillColor = Utils.ColorFromString(FillColor);
            if (fillColor != Color.Empty)
                shape.Fill.Color = fillColor;

            if (HorizontalAlignment.HasValue)
                shape.HorizontalAlignment = HorizontalAlignment.Value;
            if (VerticalAlignment.HasValue)
                shape.VerticalAlignment = VerticalAlignment.Value;

            var lineColor = Utils.ColorFromString(LineColor);
            if (lineColor != Color.Empty)
                shape.Line.Color = lineColor;
            if (LineThickness.HasValue)
                shape.Line.Thickness = LineThickness.Value;
            shape.LockAspectRatio = LockAspectRatio;

            switch (Margins?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    shape.MarginLeft = shape.MarginTop = shape.MarginRight = shape.MarginBottom = Margins[0];
                    break;
                case 4:
                    shape.MarginLeft   = Margins[0];
                    shape.MarginTop    = Margins[1];
                    shape.MarginRight  = Margins[2];
                    shape.MarginBottom = Margins[3];
                    break;
                default:
                    throw new Exception("Invalid count of values in Margins.");
            }

            switch (Offset?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    shape.Offset = new PointF(Offset[0], Offset[0]);
                    break;
                case 2:
                    shape.Offset = new PointF(Offset[0], Offset[1]);
                    break;
                default:
                    throw new Exception("Invalid count of values in Offset.");
            }

            if (RelativeHorizontalPosition.HasValue)
                shape.RelativeHorizontalPosition = RelativeHorizontalPosition.Value;
            if (RelativeVerticalPosition.HasValue)
                shape.RelativeVerticalPosition = RelativeVerticalPosition.Value;
            if (RotationAngle.HasValue)
                shape.RotationAngle = RotationAngle.Value;
            if (ScaleX.HasValue)
                shape.ScaleX = ScaleX.Value;
            if (ScaleY.HasValue)
                shape.ScaleY = ScaleY.Value;

            switch (Size?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    shape.Size = new SizeF(Size[0], Size[0]);
                    break;
                case 2:
                    shape.Size = new SizeF(Size[0], Size[1]);
                    break;
                default:
                    throw new Exception("Invalid count of values in Size.");
            }

            if (TextWrapping.HasValue)
                shape.TextWrapping = TextWrapping.Value;
            if (TextWrappingSide.HasValue)
                shape.TextWrappingSide = TextWrappingSide.Value;
            if (ZOrder.HasValue)
                shape.ZOrder = ZOrder.Value;


            AddComments(book, shape.Range);
        }
    }
}
