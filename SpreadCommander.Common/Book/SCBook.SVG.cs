using DevExpress.XtraRichEdit;
using SpreadCommander.Common.Code;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WpfMath;
using WpfMath.Converters;

namespace SpreadCommander.Common.Book
{
    public partial class SCBook
    {
        protected IRichEditDocumentServer AddSvg(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE SVG' requires filename as first argument.");

            var fileName = Project.Current.MapPath(arguments[0].Value);
            if (!File.Exists(fileName))
                throw new Exception($"File '{fileName}' does not exist.");

            using (new UsingProcessor(() => CheckNestedFile(fileName), () => RemoveNestedFile(fileName)))
            {
                float? scale = null, scaleX = null, scaleY = null;
                float dpi = 300;
                Size size = Size.Empty;

                if (arguments.Count > 1)
                {
                    var properties = Utils.SplitNameValueString(arguments[1].Value, ';');

                    foreach (var prop in properties)
                    {
                        if (string.IsNullOrWhiteSpace(prop.Key))
                            continue;

                        switch (prop.Key.ToLower())
                        {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                            case "dpi":
                                dpi = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                break;
                            case "scale":
                                scale = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                break;
                            case "scalex":
                                scaleX = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                break;
                            case "scaley":
                                scaleY = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                break;
                            case "size":
                                size = ParseSize(prop.Value);
                                break;
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
                        }
                    }
                }

                var docSVG = new XmlDocument();
                docSVG.Load(fileName);

                var image = PaintSVG(docSVG, size, dpi);

                var server = new RichEditDocumentServer();
                var document = server.Document;
                var docImage = document.Images.Append(image);

                if (scale.HasValue)
                    docImage.ScaleX = docImage.ScaleY = scale.Value;
                else
                {
                    if (scaleX.HasValue)
                        docImage.ScaleX = scaleX.Value;
                    if (scaleY.HasValue)
                        docImage.ScaleY = scaleY.Value;
                }

                return server;
            }
        }

        protected IRichEditDocumentServer AddLatex(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE LATEX' requires LaTeX as first argument.");

            var latex = arguments[0].Value;

            float? scale = null, scaleX = null, scaleY = null;
            float dpi = 300, fontSize = 20;

            if (arguments.Count > 1)
            {
                var properties = Utils.SplitNameValueString(arguments[1].Value, ';');

                foreach (var prop in properties)
                {
                    if (string.IsNullOrWhiteSpace(prop.Key))
                        continue;

                    switch (prop.Key.ToLower())
                    {
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                        case "dpi":
                            dpi = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
                        case "scale":
                            scale = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
                        case "scalex":
                            scaleX = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
                        case "scaley":
                            scaleY = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
                        case "fontsize":
                            fontSize = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                            break;
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
                    }
                }
            }

            var parser  = new TexFormulaParser();
            var formula = parser.Parse(latex);

            var renderer = formula.GetRenderer(TexStyle.Text, fontSize, "Tahoma");

            var geometry    = renderer.RenderToGeometry(0, 0);
            var converter   = new SVGConverter();
            var svgPathText = converter.ConvertGeometry(geometry);
            var svgText     = AddSVGHeader(svgPathText);

            var docSVG = new XmlDocument();
            docSVG.LoadXml(svgText);

            var image = PaintSVG(docSVG, Size.Empty, dpi);

            var server = new RichEditDocumentServer();
            var document = server.Document;
            var docImage = document.Images.Append(image);

            if (scale.HasValue)
                docImage.ScaleX = docImage.ScaleY = scale.Value;
            else
            {
                if (scaleX.HasValue)
                    docImage.ScaleX = scaleX.Value;
                if (scaleY.HasValue)
                    docImage.ScaleY = scaleY.Value;
            }

            return server;


            static string AddSVGHeader(string svgTxt)
            {
                var builder = new StringBuilder();
                builder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
                    .AppendLine("<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\">")
                    .AppendLine(svgTxt)
                    .AppendLine("</svg>");

                return builder.ToString();
            }
        }

        protected virtual Bitmap PaintSVG(XmlDocument docSVG, Size size, float dpi)
        {
            float scale = dpi / 96f;

            size.Width  = Convert.ToInt32(Math.Ceiling(size.Width * scale));
            size.Height = Convert.ToInt32(Math.Ceiling(size.Height * scale));

            var doc = SvgDocument.Open(docSVG);
            doc.ShapeRendering = SvgShapeRendering.GeometricPrecision;

            if (size.IsEmpty)
                size = new Size((int)Math.Ceiling(doc.Bounds.Width * scale), (int)Math.Ceiling(doc.Bounds.Height * scale));

            var bitmap = new Bitmap(size.Width, size.Height);
            bitmap.SetResolution(dpi, dpi);

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
    }
}
