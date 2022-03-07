using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class WriteOptions
    {
        [Description("Width of the image in document units (1/300 of inch). Default value is 2000. Has low effect in 3D charts.")]
        [DefaultValue(2000)]
        public int Width { get; set; } = 2000;

        [Description("Height of the image in document units (1/300 of inch). Default value is 1200.")]
        [DefaultValue(1200)]
        public int Height { get; set; } = 1200;

        [Description("DPI of the image. Default value is 300.")]
        public int? DPI { get; set; }

        [Description("Scaling factor of the image.")]
        [DefaultValue(1.0f)]
        public float Scale { get; set; } = 1;

        [Description("Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Description("Paragraph's text alignment")]
        public Book.ParagraphAlignment? Alignment { get; set; }

        [Description("Indent of the first line of a paragraph")]
        public float? FirstLineIdent { get; set; }

        [Description("Whether and how a paragraph's first line is indented")]
        public Book.ParagraphFirstLineIndent? FirstLineIndentType { get; set; }

        [Description("Paragraph's left indent")]
        public float? LeftIndent { get; set; }

        [Description("Paragraph's right indent")]
        public float? RightIndent { get; set; }

        [Description("Book to copy table to. If NULL and CopyToBook is set - copy to host's Book.")]
        public Book.SCBook TargetBook { get; set; }

        [Description("Comment for the text. If Stream is set, note is added at end")]
        public string Comment { get; set; }

        [Description("If set - Comment is treated as HTML")]
        public bool CommentHtml { get; set; }

        [Description("Bookmark in the document")]
        public string Bookmark { get; set; }

        [Description("Hyperlink to existing bookmark")]
        public string Hyperlink { get; set; }

        [Description("Text for the tooltip displayed when the mouse hovers over a hyperlink")]
        public string HyperlinkTooltip { get; set; }

        [Description("Target window or frame in which to display the web page content when the hyperlink is clicked")]
        public string HyperlinkTarget { get; set; }
    }

    public partial class SCChart
    {
        public void Write(WriteOptions options = null)
        {
            options ??= new WriteOptions();

            var book = options.TargetBook?.Document ?? Host?.Book?.Document;

            var chartBitmap = PaintChart(Chart, options.Width, options.Height, options.DPI);
            ExecuteSynchronized(() => DoWriteImage(book, chartBitmap, options));
        }

        protected internal virtual void DoWriteImage(Document book, Image chartBitmap, WriteOptions options)
        {
            if (chartBitmap == null)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { book.EndUpdate(); }))
            {
                var image = book.Images.Append(chartBitmap);

                if (options.Scale != 1.0f)
                    image.ScaleX = image.ScaleY = options.Scale;

                var range = image.Range;

                if (!string.IsNullOrWhiteSpace(options.ParagraphStyle) || 
                    options.FirstLineIdent.HasValue || options.FirstLineIndentType.HasValue ||
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

                var rangeNewLine = book.AppendText(Environment.NewLine);

                AddComments(book, range, options);

                book.CaretPosition = rangeNewLine.End;

                if (book.CaretPosition != null)
                    Script.Book.SCBook.ResetBookFormatting(book, book.CaretPosition);

                ScrollToCaret();
            }
        }

#pragma warning disable CA1822 // Mark members as static
        protected void AddComments(Document book, DocumentRange range, WriteOptions options)
#pragma warning restore CA1822 // Mark members as static
        {
            var comment = options.Comment;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var bookComment = book.Comments.Create(range, Environment.UserName);
                var docComment = bookComment.BeginUpdate();
                try
                {
                    if (options.CommentHtml)
                        docComment.AppendHtmlText(comment, InsertOptions.KeepSourceFormatting);
                    else
                        docComment.AppendText(comment);
                }
                finally
                {
                    bookComment.EndUpdate(docComment);
                }
            }

            var bookmark = options.Bookmark;
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                var oldBookmark = book.Bookmarks[bookmark];
                if (oldBookmark != null)
                    book.Bookmarks.Remove(oldBookmark);

                book.Bookmarks.Create(range, bookmark);
            }

            var hyperlink = options.Hyperlink;
            if (!string.IsNullOrWhiteSpace(hyperlink))
            {
                var hyperlinkBookmark = book.Bookmarks[hyperlink];
                var link = book.Hyperlinks.Create(range);
                if (hyperlinkBookmark != null)
                    link.Anchor = hyperlinkBookmark.Name;
                else
                    link.NavigateUri = hyperlink;

                if (!string.IsNullOrWhiteSpace(options.HyperlinkTooltip))
                    link.ToolTip = options.HyperlinkTooltip;

                if (!string.IsNullOrWhiteSpace(options.HyperlinkTarget))
                    link.Target = options.HyperlinkTarget;
            }
        }
    }
}
