using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public enum HeaderFooterKind { Header, Footer }

    public class HeaderFooterOptions: BookOptions
    {
        [Description("Type of the header/footer")]
        [DefaultValue(HeaderFooterType.Primary)]
        public HeaderFooterType Type { get; set; } = HeaderFooterType.Primary;

        [Description("1-based number of section to update. Negative numbers are allowed, -1 is the last section")]
        public int? SectionNum { get; set; }

        [Description("If set - Text represents HTML content")]
        public bool Html { get; set; }

        [Description("When set - retains styles associated with the target document when style definition conflicts occur.")]
        public bool UseDestinationStyles { get; set; }

        [Description("Retains character styles and direct formatting (font size, emphasis, etc.) applied to the inserted text. Incompatible with 'UseDestinationStyles'.")]
        public bool KeepSourceFormatting { get; set; }

        [Description("Text character style")]
        public string CharacterStyle { get; set; }

        [Description("Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Description("Convert '{...}' blocks into fields")]
        public bool ExpandFields { get; set; }

        [Description("Establishes a link to the next section's header/footer so that they have the same content")]
        public bool LinkToNext { get; set; }

        [Description("Establishes a link to the previous section's header/footer so that they have the same content")]
        public bool LinkToPrevious { get; set; }
    }

    public class HeaderOptions: HeaderFooterOptions
    {
    }

    public class FooterOptions: HeaderFooterOptions
    {
    }

    public partial class SCBook
    {
        protected virtual void SetHeaderFooterOptions(Document book, HeaderFooterKind headerFooterKind, string text, HeaderFooterOptions options)
        {
            options ??= new HeaderFooterOptions();

            Section section;
            int sectionNum = options.SectionNum ?? -1;
            if (sectionNum > 0 && sectionNum <= book.Sections.Count)
                section = book.Sections[sectionNum - 1];
            else if (sectionNum < 0 && -sectionNum <= book.Sections.Count)
                section = book.Sections[book.Sections.Count - (-sectionNum)];
            else
                throw new Exception("Invalid SectionNum.");

            SubDocument document = null;
            DocumentRange range;

            var type = options.Type;

            try
            {
                switch (headerFooterKind)
                {
                    case HeaderFooterKind.Header:
                        if (options.LinkToNext)
                            section.LinkHeaderToNext((DevExpress.XtraRichEdit.API.Native.HeaderFooterType)type);
                        if (options.LinkToPrevious)
                            section.LinkHeaderToPrevious((DevExpress.XtraRichEdit.API.Native.HeaderFooterType)type);

                        document = section.BeginUpdateHeader((DevExpress.XtraRichEdit.API.Native.HeaderFooterType)type);
                        break;
                    case HeaderFooterKind.Footer:
                        if (options.LinkToNext)
                            section.LinkFooterToNext((DevExpress.XtraRichEdit.API.Native.HeaderFooterType)type);
                        if (options.LinkToPrevious)
                            section.LinkFooterToPrevious((DevExpress.XtraRichEdit.API.Native.HeaderFooterType)type);

                        document = section.BeginUpdateFooter((DevExpress.XtraRichEdit.API.Native.HeaderFooterType)type);
                        break;
                    default:
                        throw new Exception("Invalid DocumentType.");
                }

                document.Delete(document.Range);
                if (options.Html)
                {
                    //var insertOptions = string.IsNullOrWhiteSpace(ParagraphStyle) ? InsertOptions.KeepSourceFormatting : InsertOptions.UseDestinationStyles;
                    var insertOptions = InsertOptions.KeepSourceFormatting;
                    if (options.UseDestinationStyles) insertOptions = InsertOptions.UseDestinationStyles;
                    if (options.KeepSourceFormatting) insertOptions = InsertOptions.KeepSourceFormatting;
                    range = document.AppendHtmlText(text, insertOptions);
                }
                else
                    range = document.AppendText(text);

                if (!string.IsNullOrWhiteSpace(options.CharacterStyle))
                {
                    var style = book.CharacterStyles[options.CharacterStyle] ?? throw new Exception($"Character style '{options.CharacterStyle}' does not exist.");

                    var cp = document.BeginUpdateCharacters(range);
                    try
                    {
                        cp.Style = style;
                    }
                    finally
                    {
                        document.EndUpdateCharacters(cp);
                    }
                }

                if (!string.IsNullOrWhiteSpace(options.ParagraphStyle))
                {
                    var style = book.ParagraphStyles[options.ParagraphStyle] ?? throw new Exception($"Paragraph style '{options.ParagraphStyle}' does not exist.");

                    var pp = document.BeginUpdateParagraphs(range);
                    try
                    {
                        pp.Style = style;
                    }
                    finally
                    {
                        document.EndUpdateParagraphs(pp);
                    }
                }

                if (options.ExpandFields)
                    ExpandFieldsInBookRange(range, Host?.Spreadsheet?.Workbook);
            }
            finally
            {
                if (document != null)
                {
                    switch (headerFooterKind)
                    {
                        case HeaderFooterKind.Header:
                            section.EndUpdateHeader(document);
                            break;
                        case HeaderFooterKind.Footer:
                            section.EndUpdateFooter(document);
                            break;
                    }
                }
            }
        }
    }
}
