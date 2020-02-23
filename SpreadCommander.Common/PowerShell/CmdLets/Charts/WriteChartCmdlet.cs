using DevExpress.XtraCharts.Native;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Charts
{
    [Cmdlet(VerbsCommunications.Write, "Chart")]
    public class WriteChartCmdlet: BaseChartWithContextCmdlet
    {
        [Parameter(HelpMessage = "Width of the image in document units (1/300 of inch). Default value is 2000. Has low effect in 3D charts.")]
        [ValidateRange(300, 20000)] 
        [PSDefaultValue(Value = 2000)]
        [DefaultValue(2000)]
        public int Width { get; set; } = 2000;

        [Parameter(HelpMessage = "Height of the image in document units (1/300 of inch). Default value is 1200.")]
        [ValidateRange(200, 20000)]
        [PSDefaultValue(Value = 1200)]
        [DefaultValue(1200)]
        public int Height { get; set; } = 1200;

        [Parameter(HelpMessage = "DPI of the image. Default value is 300.")]
        [ValidateRange(48, 4800)]
        public int? DPI { get; set; }

        [Parameter(HelpMessage = "Scaling factor of the image.")]
        [ValidateRange(0.01, 100)]
        [PSDefaultValue(Value = 1)]
        [DefaultValue(1.0f)]
        public float Scale { get; set; } = 1;

        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Parameter(HelpMessage = "Paragraph's text alignment")]
        public ParagraphAlignment? Alignment { get; set; }

        [Parameter(HelpMessage = "Indent of the first line of a paragraph")]
        public float? FirstLineIdent { get; set; }

        [Parameter(HelpMessage = "Whether and how a paragraph's first line is indented")]
        public ParagraphFirstLineIndent? FirstLineIndentType { get; set; }

        [Parameter(HelpMessage = "Paragraph's left indent")]
        public float? LeftIndent { get; set; }

        [Parameter(HelpMessage = "Paragraph's right indent")]
        public float? RightIndent { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Comment for the text. If Stream is set, note is added at end")]
        public string Comment { get; set; }

        [Parameter(HelpMessage = "If set - Comment is treated as HTML")]
        public SwitchParameter CommentHtml { get; set; }

        [Parameter(HelpMessage = "Bookmark in the document")]
        public string Bookmark { get; set; }

        [Parameter(HelpMessage = "Hyperlink to existing bookmark")]
        public string Hyperlink { get; set; }

        [Parameter(HelpMessage = "Text for the tooltip displayed when the mouse hovers over a hyperlink")]
        public string HyperlinkTooltip { get; set; }

        [Parameter(HelpMessage = "Target window or frame in which to display the web page content when the hyperlink is clicked")]
        public string HyperlinkTarget { get; set; }


        protected override bool PassThruChartContext => PassThru;

        protected override void UpdateChart()
        {
            WriteChart();
        }

        protected void WriteChart()
        {
            WriteImage(GetCmdletBook());
        }

        protected void WriteImage(Document book)
        {
            var chart = ChartContext?.Chart;
            if (chart == null)
                throw new Exception("Chart is not provided. Please use one of New-Chart cmdlets to create a chart.");

            var chartBitmap = PaintChart(chart, Width, Height, DPI);

            ExecuteSynchronized(() => DoWriteImage(book, chartBitmap));
        }

        protected override void DoWriteImage(Document book, Image chartBitmap)
        {
            if (chartBitmap == null)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { ResetBookFormatting(book); book.EndUpdate(); }))
            {
                var image = book.Images.Append(chartBitmap);

                if (Scale != 1.0f)
                    image.ScaleX = image.ScaleY = Scale;

                var range = image.Range;

                if (!string.IsNullOrWhiteSpace(ParagraphStyle) || FirstLineIdent.HasValue || FirstLineIndentType.HasValue ||
                    LeftIndent.HasValue || RightIndent.HasValue)
                {
                    var pp = book.BeginUpdateParagraphs(range);
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(ParagraphStyle))
                        {
                            var style = book.ParagraphStyles[ParagraphStyle] ?? throw new Exception($"Paragraph style '{ParagraphStyle}' does not exist.");
                            pp.Style = style;
                        }

                        if (FirstLineIdent.HasValue)
                            pp.FirstLineIndent = FirstLineIdent;
                        if (FirstLineIndentType.HasValue)
                            pp.FirstLineIndentType = FirstLineIndentType;

                        if (LeftIndent.HasValue)
                            pp.LeftIndent = LeftIndent;
                        if (RightIndent.HasValue)
                            pp.RightIndent = RightIndent;
                    }
                    finally
                    {
                        book.EndUpdateParagraphs(pp);
                    }
                }

                var rangeNewLine = book.AppendText(Environment.NewLine);

                AddComments(book, range);

                book.CaretPosition = rangeNewLine.End;
                ScrollToCaret();
            }
        }

        protected void AddComments(Document book, DocumentRange range)
        {
            var comment = Comment;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var bookComment = book.Comments.Create(range, Environment.UserName);
                var docComment = bookComment.BeginUpdate();
                try
                {
                    if (CommentHtml)
                        docComment.AppendHtmlText(comment, InsertOptions.KeepSourceFormatting);
                    else
                        docComment.AppendText(comment);
                }
                finally
                {
                    bookComment.EndUpdate(docComment);
                }
            }

            var bookmark = Bookmark;
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                var oldBookmark = book.Bookmarks[bookmark];
                if (oldBookmark != null)
                    book.Bookmarks.Remove(oldBookmark);

                book.Bookmarks.Create(range, bookmark);
            }

            var hyperlink = Hyperlink;
            if (!string.IsNullOrWhiteSpace(hyperlink))
            {
                var hyperlinkBookmark = book.Bookmarks[hyperlink];
                var link = book.Hyperlinks.Create(range);
                if (hyperlinkBookmark != null)
                    link.Anchor = hyperlinkBookmark.Name;
                else
                    link.NavigateUri = hyperlink;

                if (!string.IsNullOrWhiteSpace(HyperlinkTooltip))
                    link.ToolTip = HyperlinkTooltip;

                if (!string.IsNullOrWhiteSpace(HyperlinkTarget))
                    link.Target = HyperlinkTarget;
            }
        }
    }
}
