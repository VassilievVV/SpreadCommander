using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class ImageOptions : CommentOptions
    {
        [Description("Add line breaks after each line or no.")]
        public bool NoLineBreaks { get; set; }

        [Description("X-scaling factor of the image.")]
        [DefaultValue(1.0f)]
        public float ScaleX { get; set; } = 1.0f;

        [Description("Y-scaling factor of the image.")]
        [DefaultValue(1.0f)]
        public float ScaleY { get; set; } = 1.0f;

        [Description("Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Description("Paragraph's text alignment")]
        public ParagraphAlignment? Alignment { get; set; }

        [Description("Indent of the first line of a paragraph")]
        public float? FirstLineIdent { get; set; }

        [Description("Whether and how a paragraph's first line is indented")]
        public ParagraphFirstLineIndent? FirstLineIndentType { get; set; }

        [Description("Paragraph's left indent")]
        public float? LeftIndent { get; set; }

        [Description("Paragraph's right indent")]
        public float? RightIndent { get; set; }
    }

    public partial class SCBook
    {
        public void WriteImage(string fileName, ImageOptions options = null)
        {
            fileName  = Project.Current.MapPath(fileName);
            var image = Image.FromFile(fileName);

            ExecuteSynchronized(options, () => DoWriteImage(image, options));
        }

        public void WriteImage(Image image, ImageOptions options = null) =>
            ExecuteSynchronized(options, () => DoWriteImage(image, options));

        protected void DoWriteImage(Image bmp, ImageOptions options)
        {
            if (bmp == null)
                throw new ArgumentNullException(nameof(bmp));

            bmp = bmp.Clone() as Image;

            options ??= new ImageOptions();

            var book = options.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                DocumentImage image = book.Images.Append(bmp);

                image.ScaleX = options.ScaleX;
                image.ScaleY = options.ScaleY;

                var imageRange = image.Range;
                if (rangeStart == null)
                    rangeStart = imageRange.Start;

                if (!options.NoLineBreaks)
                    imageRange = book.AppendText(Environment.NewLine);

                rangeEnd = imageRange.End;

                if (rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());

                    if (!string.IsNullOrWhiteSpace(options.ParagraphStyle) || options.FirstLineIdent.HasValue || options.FirstLineIndentType.HasValue ||
                        options.LeftIndent.HasValue || options.RightIndent.HasValue)
                    {
                        var pp = book.BeginUpdateParagraphs(range);
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(options.ParagraphStyle))
                            {
                                var style = book.ParagraphStyles[options.ParagraphStyle] ?? throw new Exception($"Paragraph style '{options.ParagraphStyle}' does not exist.");
                                pp.Style = style;
                            }

                            if (options.FirstLineIdent.HasValue)
                                pp.FirstLineIndent = options.FirstLineIdent;
                            if (options.FirstLineIndentType.HasValue)
                                pp.FirstLineIndentType = (DevExpress.XtraRichEdit.API.Native.ParagraphFirstLineIndent)options.FirstLineIndentType;

                            if (options.LeftIndent.HasValue)
                                pp.LeftIndent = options.LeftIndent;
                            if (options.RightIndent.HasValue)
                                pp.RightIndent = options.RightIndent;
                        }
                        finally
                        {
                            book.EndUpdateParagraphs(pp);
                        }
                    }

                    Script.Book.SCBook.AddComments(book, range, options);
                }

                if (rangeEnd != null)
                {
                    book.CaretPosition = rangeEnd;
                    ResetBookFormatting(book, rangeEnd);
                    ScrollToCaret();
                }
            }
        }
    }
}
