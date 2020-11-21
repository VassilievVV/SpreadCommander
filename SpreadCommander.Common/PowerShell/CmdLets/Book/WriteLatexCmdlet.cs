using DevExpress.XtraRichEdit.API.Native;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WpfMath;
using WpfMath.Converters;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "Latex")]
    public class WriteLatexCmdlet: BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "LaTeX strings to write into book.")]
        public string[] Latex { get; set; }

        [Parameter(HelpMessage = "DPI of the image. Default value is 300.")]
        [ValidateRange(48, 4800)]
        public int? DPI { get; set; }

        [Parameter(HelpMessage = "Scaling factor of the image.")]
        [Alias("s")]
        [ValidateRange(0.01, 100)]
        [PSDefaultValue(Value = 1)]
        [DefaultValue(1.0f)]
        public float Scale { get; set; } = 1;

        [Parameter(HelpMessage = "Font size. Default is 20.")]
        [Alias("fs")]
        [ValidateRange(6, 100)]
        [PSDefaultValue(Value = 20.0f)]
        [DefaultValue(20.0f)]
        public float FontSize { get; set; } = 20;

        [Parameter(HelpMessage = "Write each file individually, without using cache")]
        public SwitchParameter Stream { get; set; }

        [Parameter(HelpMessage = "Add line breaks after each line or no.")]
        public SwitchParameter NoLineBreaks { get; set; }

        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }


        private readonly List<string> _Output = new List<string>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            if ((Latex?.Length ?? 0) <= 0)
                return;

            foreach (var line in Latex)
                if (line != null)
                    _Output.Add(line);

            if (Stream)
                WriteBuffer(false);
        }

        protected override void EndProcessing()
        {
            WriteBuffer(true);
        }

        protected void WriteBuffer(bool lastBlock)
        {
            WriteText(GetCmdletBook(), _Output, lastBlock);
            _Output.Clear();
        }

        protected void WriteText(Document book, List<string> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteText(book, buffer, lastBlock));
        }

        protected virtual void DoWriteText(Document book, List<string> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { ResetBookFormatting(book); book.EndUpdate(); }))
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                foreach (var line in buffer)
                {
                    var parser  = new TexFormulaParser();
                    var formula = parser.Parse(line);

                    var renderer = formula.GetRenderer(TexStyle.Text, FontSize, "Tahoma");

                    var geometry    = renderer.RenderToGeometry(0, 0);
                    var converter   = new SVGConverter();
                    var svgPathText = converter.ConvertGeometry(geometry);
                    var svgText     = AddSVGHeader(svgPathText);

                    var imageLatex = PaintSVG(svgText, DPI);
                    AddUserCommentsToImage(imageLatex, line);
                    var image = book.Images.Append(imageLatex);

                    image.ScaleX = Scale;
                    image.ScaleY = Scale;

                    var range = image.Range;
                    if (rangeStart == null)
                        rangeStart = range.Start;

                    if (!NoLineBreaks)
                        range = book.AppendText(Environment.NewLine);

                    rangeEnd = range.End;
                }

                if (lastBlock && rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());

                    if (!string.IsNullOrWhiteSpace(ParagraphStyle))
                    {
                        var style = book.ParagraphStyles[ParagraphStyle] ?? throw new Exception($"Paragraph style '{ParagraphStyle}' does not exist.");
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

                    AddComments(book, range);

                    WriteRangeToConsole(book, range);
                }

                if (rangeEnd != null)
                {
                    book.CaretPosition = rangeEnd;
                    ScrollToCaret();
                }
                ResetBookFormatting(book);
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
                dpi = ExternalHost?.DefaultDPI ?? DefaultDPI;

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
            //pi = (PropertyItem)typeof(PropertyItem).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { }, null).Invoke(null);

            var newItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            newItem.Id    = propID;
            newItem.Value = Encoding.UTF8.GetBytes($"{value}\0");
            newItem.Len   = newItem.Value.Length;
            newItem.Type  = 2; //ASCII array

            image.SetPropertyItem(newItem);
        }
    }
}
