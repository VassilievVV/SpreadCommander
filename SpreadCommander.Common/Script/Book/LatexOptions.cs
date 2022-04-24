using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using Svg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfMath;
using WpfMath.Converters;

namespace SpreadCommander.Common.Script.Book
{
    public class LaTeXOptions : CommentOptions
    {
        [Description("DPI of the image. Default value is 300.")]
        public int? DPI { get; set; }

        [Description("Scaling factor of the image.")]
        [DefaultValue(1.0f)]
        public float Scale { get; set; } = 1;

        [Description("Font size. Default is 20.")]
        [DefaultValue(20.0f)]
        public float FontSize { get; set; } = 20;

        [Description("Add line breaks after each line or no.")]
        public bool NoLineBreaks { get; set; }

        [Description("Paragraph style")]
        public string ParagraphStyle { get; set; }
    }

    public partial class SCBook
    {
        public void WriteLaTeX(string text, LaTeXOptions options = null) =>
            ExecuteSynchronized(options, () => DoWriteLatex(text, options));

        protected void DoWriteLatex(string text, LaTeXOptions options)
        {
            options ??= new LaTeXOptions();

            var book = options.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                var parser  = new TexFormulaParser();
                var formula = parser.Parse(text);

                var renderer = formula.GetRenderer(TexStyle.Text, options.FontSize, "Tahoma");

                var geometry    = renderer.RenderToGeometry(0, 0);
                var converter   = new SVGConverter();
                var svgPathText = converter.ConvertGeometry(geometry);
                var svgText     = AddSVGHeader(svgPathText);

                var imageLatex = PaintSVG(svgText, options.DPI);
                AddUserCommentsToImage(imageLatex, text);
                var image = book.Images.Append(imageLatex);

                image.ScaleX = options.Scale;
                image.ScaleY = options.Scale;

                var rangeImage = image.Range;
                if (rangeStart == null)
                    rangeStart = rangeImage.Start;

                if (!options.NoLineBreaks)
                    rangeImage = book.AppendText(Environment.NewLine);

                rangeEnd = rangeImage.End;

                if (rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());

                    if (!string.IsNullOrWhiteSpace(options.ParagraphStyle))
                    {
                        var style = book.ParagraphStyles[options.ParagraphStyle] ?? throw new Exception($"Paragraph style '{options.ParagraphStyle}' does not exist.");
                        var pp = book.BeginUpdateParagraphs(range);
                        try
                        {
                            pp.Style = style;
                        }
                        finally
                        {
                            book.EndUpdateParagraphs(pp);
                        }
                    }

                    Script.Book.SCBook.AddComments(book, range, options);

                    WriteRangeToConsole(book, range);
                }

                if (rangeEnd != null)
                {
                    book.CaretPosition = rangeEnd;
                    Script.Book.SCBook.ResetBookFormatting(book, rangeEnd);
                    ScrollToCaret();
                }
            }


            static string AddSVGHeader(string svgText)
            {
                var builder = new StringBuilder();
                builder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
                    .AppendLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">")
                    .AppendLine(svgText)
                    .AppendLine("</svg>");

                return builder.ToString();
            }
        }

        protected virtual Bitmap PaintSVG(string svg, int? dpi = null)
        {
            if (dpi == null || dpi == 0)
                dpi = BookOptions.DefaultDPI;

            float scale = dpi.Value / 96f;

            var docSVG = new XmlDocument();
            docSVG.LoadXml(svg);

            var doc            = SvgDocument.Open(docSVG);
            doc.ShapeRendering = SvgShapeRendering.GeometricPrecision;

            var bitmap = new Bitmap((int)Math.Ceiling(doc.Bounds.Width * scale), (int)Math.Ceiling(doc.Bounds.Height * scale));
            bitmap.SetResolution(dpi.Value, dpi.Value);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode   = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode     = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                graphics.Clear(Color.White);

                var renderer = SvgRenderer.FromGraphics(graphics);
                renderer.SmoothingMode = SmoothingMode.HighQuality;
                if (scale != 1f)
                    renderer.ScaleTransform(scale, scale);

                doc.Draw(renderer);
            }

            return bitmap;
        }

        //https://csharp.hotexamples.com/examples/System.Drawing/Image/SetPropertyItem/php-image-setpropertyitem-method-examples.html
        public static void AddDescriptionToImage(Image image, string description) =>
            AddPropertyToImage(image, 0x010E, description);
        public static void AddUserCommentsToImage(Image image, string description) =>
            AddPropertyToImage(image, 0x9286, description);

        public static void AddPropertyToImage(Image image, int propID, string value)
        {
            var newItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            newItem.Id    = propID;
            newItem.Value = Encoding.UTF8.GetBytes($"{value}\0");
            newItem.Len   = newItem.Value.Length;
            newItem.Type  = 2; //ASCII array

            image.SetPropertyItem(newItem);
        }
    }
}
