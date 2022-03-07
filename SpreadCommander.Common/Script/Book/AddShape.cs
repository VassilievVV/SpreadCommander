using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class ShapeOptions: CommentOptions
    {
        [Description("Name of a shape")]
        public string Name { get; set; }

        [Description("Text for the shape")]
        public string Text { get; set; }

        [Description("If set - Text is treated as HTML")]
        public bool Html { get; set; }

        [Description("When set - retains styles associated with the target document when style definition conflicts occur.")]
        public bool UseDestinationStyles { get; set; }

        [Description("Fill color")]
        public string FillColor { get; set; }

        [Description("How the shape is positioned horizontally")]
        public ShapeHorizontalAlignment? HorizontalAlignment { get; set; }

        [Description("How the shape is positioned vertically")]
        public ShapeVerticalAlignment? VerticalAlignment { get; set; }

        [Description("Color of the shape's border")]
        public string LineColor { get; set; }

        [Description("Thickness of the specified line in points")]
        public float? LineThickness { get; set; }

        [Description("Whether you can change the height and width of a shape independently when you resize it")]
        public bool LockAspectRatio { get; set; }

        [Description("Margins of the shape. 1 or 4 elements")]
        public float[] Margins { get; set; }

        [Description("Shape position relative to a certain element of the document layout. 2 elements - X and Y")]
        public float[] Offset { get; set; }

        [Description("Image's filename")]
        public string ImageFile { get; set; }

        [Description("Item to what the horizontal position of a shape is relative")]
        public ShapeRelativeHorizontalPosition? RelativeHorizontalPosition { get; set; }

        [Description("Item to what the vertical position of a shape is relative")]
        public ShapeRelativeVerticalPosition? RelativeVerticalPosition { get; set; }

        [Description("Number of degrees the shape is rotated around the z-axis")]
        public float? RotationAngle { get; set; }

        [Description("X-axis scale factor")]
        public float? ScaleX { get; set; }

        [Description("Y-axis scale factor")]
        public float? ScaleY { get; set; }

        [Description("Size of a shape")]
        public float[] Size { get; set; }

        [Description("How the shape is surrounded by the text")]
        public TextWrappingType? TextWrapping { get; set; }

        [Description("How text should wrap around the shape's sides")]
        public TextWrappingSide? TextWrappingSide { get; set; }

        [Description("Position of the shape in the z-order")]
        public int? ZOrder { get; set; }
    }

    public class AddShapeOptions: ShapeOptions
    {
    }

    public partial class SCBook
    {
        public void AddShape(ShapeOptions options) =>
            ExecuteSynchronized(options, () => DoAddShape(options));

        protected void DoAddShape(ShapeOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            var book = options.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                Shape shape;

                if (!string.IsNullOrWhiteSpace(options.ImageFile))
                {
                    var imageFile = Project.Current.MapPath(options.ImageFile);
                    var image     = Image.FromFile(imageFile);
                    shape         = book.Shapes.InsertPicture(book.Range.End, image);
                }
                else
                    shape = book.Shapes.InsertTextBox(book.Range.End);

                SetupShape(book, shape, options);
            }
        }

        protected virtual void SetupShape(Document book, Shape shape, ShapeOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            if (options.Text != null)
            {
                if (options.Html)
                {
                    var range = shape.ShapeFormat.TextBox.Document.AppendHtmlText(options.Text,
                        options.UseDestinationStyles ? InsertOptions.UseDestinationStyles : InsertOptions.KeepSourceFormatting);
                    WriteRangeToConsole(book, range);
                }
                else
                {
                    shape.ShapeFormat.TextBox.Document.AppendText(options.Text);
                    WriteTextToConsole(options.Text);
                }
            }

            if (!string.IsNullOrWhiteSpace(options.Name))
                shape.Name = options.Name;

            var fillColor = Utils.ColorFromString(options.FillColor);
            if (fillColor != Color.Empty)
                shape.Fill.Color = fillColor;

            if (options.HorizontalAlignment.HasValue)
                shape.HorizontalAlignment = (DevExpress.XtraRichEdit.API.Native.ShapeHorizontalAlignment)options.HorizontalAlignment.Value;
            if (options.VerticalAlignment.HasValue)
                shape.VerticalAlignment = (DevExpress.XtraRichEdit.API.Native.ShapeVerticalAlignment)options.VerticalAlignment.Value;

            var lineColor = Utils.ColorFromString(options.LineColor);
            if (lineColor != Color.Empty)
                shape.Line.Color = lineColor;
            if (options.LineThickness.HasValue)
                shape.Line.Thickness = options.LineThickness.Value;
            shape.LockAspectRatio = options.LockAspectRatio;

            switch (options.Margins?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    shape.MarginLeft = shape.MarginTop = shape.MarginRight = shape.MarginBottom = options.Margins[0];
                    break;
                case 4:
                    shape.MarginLeft   = options.Margins[0];
                    shape.MarginTop    = options.Margins[1];
                    shape.MarginRight  = options.Margins[2];
                    shape.MarginBottom = options.Margins[3];
                    break;
                default:
                    throw new Exception("Invalid count of values in Margins.");
            }

            switch (options.Offset?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    shape.Offset = new PointF(options.Offset[0], options.Offset[0]);
                    break;
                case 2:
                    shape.Offset = new PointF(options.Offset[0], options.Offset[1]);
                    break;
                default:
                    throw new Exception("Invalid count of values in Offset.");
            }

            if (options.RelativeHorizontalPosition.HasValue)
                shape.RelativeHorizontalPosition = (DevExpress.XtraRichEdit.API.Native.ShapeRelativeHorizontalPosition)options.RelativeHorizontalPosition.Value;
            if (options.RelativeVerticalPosition.HasValue)
                shape.RelativeVerticalPosition = (DevExpress.XtraRichEdit.API.Native.ShapeRelativeVerticalPosition)options.RelativeVerticalPosition.Value;
            if (options.RotationAngle.HasValue)
                shape.RotationAngle = options.RotationAngle.Value;
            if (options.ScaleX.HasValue)
                shape.ScaleX = options.ScaleX.Value;
            if (options.ScaleY.HasValue)
                shape.ScaleY = options.ScaleY.Value;

            switch (options.Size?.Length ?? 0)
            {
                case 0:
                    break;
                case 1:
                    shape.Size = new SizeF(options.Size[0], options.Size[0]);
                    break;
                case 2:
                    shape.Size = new SizeF(options.Size[0], options.Size[1]);
                    break;
                default:
                    throw new Exception("Invalid count of values in Size.");
            }

            if (options.TextWrapping.HasValue)
                shape.TextWrapping = (DevExpress.XtraRichEdit.API.Native.TextWrappingType)options.TextWrapping.Value;
            if (options.TextWrappingSide.HasValue)
                shape.TextWrappingSide = (DevExpress.XtraRichEdit.API.Native.TextWrappingSide)options.TextWrappingSide.Value;
            if (options.ZOrder.HasValue)
                shape.ZOrder = options.ZOrder.Value;


            AddComments(book, shape.Range, options);
        }
    }
}
