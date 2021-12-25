using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class TextOptions: CommentOptions
    {
        [Description("Text foreground color")]
        public string ForegroundColor { get; set; }

        [Description("Text background color")]
        public string BackgroundColor { get; set; }

        [Description("Font for output text")]
        public string FontName { get; set; }

        [Description("Font size for output text")]
        public float? FontSize { get; set; }

        [Description("When set - output using bold font")]
        public bool? Bold { get; set; }

        [Description("When set - output with italic font")]
        public bool? Italic { get; set; }

        [Description("When set - output using underlined font")]
        public bool? Underline { get; set; }

        [Description("When set - output text as subscript")]
        public bool? Subscript { get; set; }

        [Description("When set - output text as superscript")]
        public bool? Superscript { get; set; }

        [Description("Write each string individually, without using cache")]
        public bool? Stream { get; set; }

        [Description("Type of underline line")]
        public UnderlineType? UnderlineType { get; set; }

        [Description("Color of underline line")]
        public string UnderlineColor { get; set; }

        [Description("Add line breaks after each line or no")]
        public bool? NoLineBreaks { get; set; }

        [Description("Text character style")]
        public string CharacterStyle { get; set; }

        [Description("Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Description("Convert '{...}' blocks into fields")]
        public bool ExpandFields { get; set; }

        [Description("Snippets contains Books, Spreadsheets, Images and are used with Fields.")]
        public Hashtable Snippets { get; set; }
    }

    public partial class SCBook
    {
        public SCBook WriteText(object obj, TextOptions options = null) =>
            WriteText(Convert.ToString(obj), options);

        public SCBook WriteText(string text, TextOptions options = null)
        {
            ExecuteSynchronized(options, () => DoWriteText(text, options));
            return this;
        }

        protected void DoWriteText(string text, TextOptions options)
        {
            options ??= new TextOptions();

            var book = options.Book?.Document ?? Document;

            book.BeginUpdate();
            try
            {
                var range = book.AppendText(text);

                if (options != null)
                {
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

                    var foregroundColor = ColorExtensions.FromHtmlColor(options.ForegroundColor, SystemColors.WindowText);
                    var backgroundColor = ColorExtensions.FromHtmlColor(options.BackgroundColor, SystemColors.Window);
                    var underlineColor  = ColorExtensions.FromHtmlColor(options.UnderlineColor, Color.Empty);

                    if (foregroundColor != SystemColors.WindowText || backgroundColor != SystemColors.Window || options.FontName != null || options.FontSize.HasValue ||
                        options.Bold.HasValue || options.Italic.HasValue || options.Underline.HasValue || options.Subscript.HasValue || options.Superscript.HasValue ||
                        options.UnderlineType.HasValue || underlineColor != Color.Empty ||
                        !string.IsNullOrWhiteSpace(options.CharacterStyle))
                    {
                        var cp = book.BeginUpdateCharacters(range);
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(options.CharacterStyle))
                            {
                                var style = book.CharacterStyles[options.CharacterStyle] ?? throw new Exception($"Character style '{options.CharacterStyle}' does not exist.");
                                cp.Style = style;
                            }

                            if (foregroundColor != SystemColors.WindowText)
                                cp.ForeColor = foregroundColor;
                            if (backgroundColor != SystemColors.Window)
                                cp.BackColor = backgroundColor;
                            if (!string.IsNullOrWhiteSpace(options.FontName))
                                cp.FontName = options.FontName;
                            if (options.FontSize.HasValue && options.FontSize.Value >= 4 && options.FontSize.Value <= 500)
                                cp.FontSize = options.FontSize.Value;
                            if (options.Bold.HasValue)
                                cp.Bold = options.Bold.Value;
                            if (options.Italic.HasValue)
                                cp.Italic = options.Italic;
                            if (options.Underline.HasValue)
                                cp.Underline = (DevExpress.XtraRichEdit.API.Native.UnderlineType)(options.Underline.Value ? UnderlineType.Single : UnderlineType.None);
                            if (options.UnderlineType.HasValue)
                                cp.Underline = (DevExpress.XtraRichEdit.API.Native.UnderlineType)options.UnderlineType;
                            if (underlineColor != Color.Empty)
                                cp.UnderlineColor = underlineColor;
                            if (options.Subscript.HasValue)
                                cp.Subscript = options.Subscript.Value;
                            if (options.Superscript.HasValue)
                                cp.Superscript = options.Superscript.Value;
                        }
                        finally
                        {
                            book.EndUpdateCharacters(cp);
                        }
                    }

                    if (options.ExpandFields && options.Snippets != null)
                        Script.Book.SCBook.ExpandFieldsInBookRange(range, Host?.Spreadsheet?.Workbook, options.Snippets);
                }

                Script.Book.SCBook.AddComments(book, range, options);

                if (range?.End != null)
                {
                    book.CaretPosition = range.End;
                    Script.Book.SCBook.ResetBookFormatting(book, range.End);
                    ScrollToCaret();
                }

                WriteTextToConsole(text);
            }
            finally
            {                
                book.EndUpdate();
            }
        }
    }
}
