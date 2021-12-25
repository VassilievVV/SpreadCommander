using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using System.ComponentModel;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "Image")]
    public class WriteImageCmdlet : BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Filenames of images to write into book")]
        public string[] FileNames { get; set; }

        [Parameter(HelpMessage = "Write System.Bitmap.Image object.")]
        public Image Image { get; set; }

        [Parameter(HelpMessage = "Write each file individually, without using cache")]
        public SwitchParameter Stream { get; set; }

        [Parameter(HelpMessage = "Add line breaks after each line or no.")]
        public SwitchParameter NoLineBreaks { get; set; }

        [Parameter(HelpMessage = "X-scaling factor of the image.")]
        [ValidateRange(0.001, 1000)]
        [PSDefaultValue(Value = 1.0f)]
        [DefaultValue(1.0f)]
        public float ScaleX { get; set; } = 1.0f;

        [Parameter(HelpMessage = "Y-scaling factor of the image.")]
        [ValidateRange(0.001, 1000)]
        [PSDefaultValue(Value = 1.0f)]
        [DefaultValue(1.0f)]
        public float ScaleY { get; set; } = 1.0f;

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


        private readonly List<object> _Output = new ();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            if (Image != null)
                _Output.Add(Image);

            var text = FileNames;
            if (text == null)
                text = Array.Empty<string>();

            foreach (var line in text)
            {
                if (line != null)
                    _Output.Add(line);

                if (Stream)
                    WriteBuffer(false);
            }
        }

        protected override void EndProcessing()
        {
            WriteBuffer(true);
        }

        protected void WriteBuffer(bool lastBlock)
        {
            WriteImage(GetCmdletBook(), _Output, lastBlock);
            _Output.Clear();
        }

        protected void WriteImage(Document book, List<object> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteImage(book, buffer, lastBlock));
        }

        protected virtual void DoWriteImage(Document book, List<object> buffer, bool lastBlock)
        {
            if (buffer.Count <= 0 && !lastBlock)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { ResetBookFormatting(book); book.EndUpdate(); }))
            {
                DocumentPosition rangeStart = null, rangeEnd = null;

                foreach (var line in buffer)
                {
                    DocumentImage image;
                    if (line is Image img)
                        image = book.Images.Append(img);
                    else if (line is string strLine)
                    {
                        var fileName = Project.Current.MapPath(strLine);

                        var source = DocumentImageSource.FromFile(fileName);
                        image = book.Images.Append(source);
                    }
                    else
                        throw new Exception("WriteImage - unknown input object.");

                    image.ScaleX = ScaleX;
                    image.ScaleY = ScaleY;

                    var range  = image.Range;
                    if (rangeStart == null)
                        rangeStart = range.Start;

                    if (!NoLineBreaks)
                        range = book.AppendText(Environment.NewLine);

                    rangeEnd = range.End;
                }

                if (lastBlock && rangeStart != null && rangeEnd != null)
                {
                    var range = book.CreateRange(rangeStart, rangeEnd.ToInt() - rangeStart.ToInt());

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

                    AddComments(book, range);
                }

                if (rangeEnd != null)
                {
                    book.CaretPosition = rangeEnd;
                    ScrollToCaret();
                }
            }
        }
    }
}
