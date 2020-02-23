using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using System.Collections;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommunications.Write, "Text")]
    public class WriteTextCmdlet: BaseBookWithCommentsCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Text to write into book")]
        public string[] Text { get; set; }

        [Parameter(HelpMessage = "Text foreground color")]
        [Alias("fore", "fg")]
        public string ForegroundColor { get; set; }

        [Parameter(HelpMessage = "Text background color")]
        [Alias("back", "bg")]
        public string BackgroundColor { get; set; }

        [Parameter(HelpMessage = "Font for output text")]
        [Alias("font")]
        public string FontName { get; set; }

        [Parameter(HelpMessage = "Font size for output text")]
        [Alias("size")]
        [ValidateRange(4, 500)]
        public float? FontSize { get; set; }

        [Parameter(HelpMessage = "When set - output using bold font")]
        [Alias("b")]
        public SwitchParameter Bold { get; set; }

        [Parameter(HelpMessage = "When set - output with italic font")]
        [Alias("i")]
        public SwitchParameter Italic { get; set; }

        [Parameter(HelpMessage = "When set - output using underlined font")]
        [Alias("u")]
        public SwitchParameter Underline { get; set; }

        [Parameter(HelpMessage = "When set - output text as subscript")]
        [Alias("down")]
        public SwitchParameter Subscript { get; set; }

        [Parameter(HelpMessage = "When set - output text as superscript")]
        [Alias("up")]
        public SwitchParameter Superscript { get; set; }

        [Parameter(HelpMessage = "Write each string individually, without using cache")]
        public SwitchParameter Stream { get; set; }

        [Parameter(HelpMessage = "Type of underline line")]
        [Alias("ut")]
        public UnderlineType? UnderlineType { get; set; }

        [Parameter(HelpMessage = "Color of underline line")]
        [Alias("uc")]
        public string UnderlineColor { get; set; }

        [Parameter(HelpMessage = "Add line breaks after each line or no")]
        [Alias("NoBreaks")]
        public SwitchParameter NoLineBreaks { get; set; }
        
        [Parameter(HelpMessage = "Text character style")]
        public string CharacterStyle { get; set; }
        
        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }
        
        [Parameter(HelpMessage = "Convert '{...}' blocks into fields")]
        public SwitchParameter ExpandFields { get; set; }

        [Parameter(HelpMessage = "Snippets contains Books, Spreadsheets, Images and are used with Fields.")]
        public Hashtable Snippets { get; set; }


        private readonly StringBuilder _Output = new StringBuilder();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var text = Text;
            if (text == null || text.Length <= 0)
                text = new string[] { string.Empty };

            foreach (var line in text)
            {
                if (line != null)
                    _Output.Append(line);
                if (!NoLineBreaks)
                    _Output.AppendLine();

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
            WriteText(GetCmdletBook(), _Output, lastBlock);
            _Output.Clear();
        }

        protected void WriteText(Document book, StringBuilder buffer, bool lastBlock)
        {
            if (buffer.Length <= 0 && !lastBlock)
                return;

            ExecuteSynchronized(() => DoWriteText(book, buffer, lastBlock));
        }

        protected virtual void DoWriteText(Document book, StringBuilder buffer, bool lastBlock)
        {
            if (buffer.Length <= 0 && !lastBlock)
                return;

            book.BeginUpdate();
            try
            {
                var text = buffer.ToString();

                var range = book.AppendText(text);

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

                var foregroundColor = ColorExtensions.FromHtmlColor(ForegroundColor, SystemColors.WindowText);
                var backgroundColor = ColorExtensions.FromHtmlColor(BackgroundColor, SystemColors.Window);
                var underlineColor  = ColorExtensions.FromHtmlColor(UnderlineColor, Color.Empty);

                if (foregroundColor != SystemColors.WindowText || backgroundColor != SystemColors.Window || FontName != null || FontSize.HasValue ||
                    Bold || Italic || Underline || Subscript || Superscript || UnderlineType.HasValue || underlineColor != Color.Empty ||
                    !string.IsNullOrWhiteSpace(CharacterStyle))
                {
                    var cp = book.BeginUpdateCharacters(range);
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(CharacterStyle))
                        {
                            var style = book.CharacterStyles[CharacterStyle] ?? throw new Exception($"Character style '{CharacterStyle}' does not exist.");
                            cp.Style = style;
                        }

                        if (foregroundColor != SystemColors.WindowText)
                            cp.ForeColor = foregroundColor;
                        if (backgroundColor != SystemColors.Window)
                            cp.BackColor = backgroundColor;
                        if (!string.IsNullOrWhiteSpace(FontName))
                            cp.FontName = FontName;
                        if (FontSize.HasValue && FontSize.Value >= 4 && FontSize.Value <= 500)
                            cp.FontSize = FontSize.Value;
                        if (Bold)
                            cp.Bold = true;
                        if (Italic)
                            cp.Italic = true;
                        if (Underline)
                            cp.Underline = DevExpress.XtraRichEdit.API.Native.UnderlineType.Single;
                        if (UnderlineType.HasValue)
                            cp.Underline = UnderlineType;
                        if (underlineColor != Color.Empty)
                            cp.UnderlineColor = underlineColor;
                        if (Subscript)
                            cp.Subscript = true;
                        if (Superscript)
                            cp.Superscript = true;
                    }
                    finally
                    {
                        book.EndUpdateCharacters(cp);
                    }
                }

                if (ExpandFields)
                    ExpandFieldsInBookRange(range, Snippets);

                if (lastBlock)
                    AddComments(book, range);

                if (range?.End != null)
                {
                    book.CaretPosition = range.End;
                    ScrollToCaret();
                }
            }
            finally
            {
                ResetBookFormatting(book);
                book.EndUpdate();
            }
        }
    }
}
