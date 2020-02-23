using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    public class BaseBookHeaderFooterCmdlet: BaseBookCmdlet
    {
        protected enum DocumentType { Header, Footer }

        [Parameter(Position = 0, HelpMessage = "Plain or HTML text of the header/footer")]
        public string Text { get; set; }

        [Parameter(Position = 1, HelpMessage = "Type of the header/footer")]
        [DefaultValue(HeaderFooterType.Primary)]
        [PSDefaultValue(Value = HeaderFooterType.Primary)]
        public HeaderFooterType Type { get; set; } = HeaderFooterType.Primary;

        [Parameter(HelpMessage = "1-based number of section to update. Negative numbers are allowed, -1 is the last section")]
        public int? SectionNum { get; set; }

        [Parameter(HelpMessage = "If set - Text represents HTML content")]
        public SwitchParameter Html { get; set; }

        [Parameter(HelpMessage = "When set - retains styles associated with the target document when style definition conflicts occur.")]
        [Alias("DestStyles", "ds")]
        public SwitchParameter UseDestinationStyles { get; set; }

        [Parameter(HelpMessage = "Retains character styles and direct formatting (font size, emphasis, etc.) applied to the inserted text. Incompatible with 'UseDestinationStyles'.")]
        [Alias("KeepFormatting", "ks")]
        public SwitchParameter KeepSourceFormatting { get; set; }

        [Parameter(HelpMessage = "Text character style")]
        public string CharacterStyle { get; set; }

        [Parameter(HelpMessage = "Paragraph style")]
        public string ParagraphStyle { get; set; }

        [Parameter(HelpMessage = "Convert '{...}' blocks into fields")]
        public SwitchParameter ExpandFields { get; set; }

        [Parameter(HelpMessage = "Establishes a link to the next section's header/footer so that they have the same content")]
        public SwitchParameter LinkToNext { get; set; }

        [Parameter(HelpMessage = "Establishes a link to the previous section's header/footer so that they have the same content")]
        public SwitchParameter LinkToPrevious { get; set; }


        protected virtual DocumentType HeaderFooter { get; }


        protected virtual void SetupHeaderFooter(Document book)
        {
            Section section;
            int sectionNum = SectionNum ?? -1;
            if (sectionNum > 0 && sectionNum <= book.Sections.Count)
                section = book.Sections[sectionNum - 1];
            else if (sectionNum < 0 && -sectionNum <= book.Sections.Count)
                section = book.Sections[book.Sections.Count - (-sectionNum)];
            else
                throw new Exception("Invalid SectionNum.");

            SubDocument document = null;
            DocumentRange range;

            var type = Type;

            try
            {
                switch (HeaderFooter)
                {
                    case DocumentType.Header:
                        if (LinkToNext)
                            section.LinkHeaderToNext(type);
                        if (LinkToPrevious)
                            section.LinkHeaderToPrevious(type);

                        document = section.BeginUpdateHeader(type);
                        break;
                    case DocumentType.Footer:
                        if (LinkToNext)
                            section.LinkFooterToNext(type);
                        if (LinkToPrevious)
                            section.LinkFooterToPrevious(type);

                        document = section.BeginUpdateFooter(type);
                        break;
                    default:
                        throw new Exception("Invalid DocumentType.");
                }

                document.Delete(document.Range);
                if (Html)
                {
                    //var insertOptions = string.IsNullOrWhiteSpace(ParagraphStyle) ? InsertOptions.KeepSourceFormatting : InsertOptions.UseDestinationStyles;
                    var insertOptions = InsertOptions.KeepSourceFormatting;
                    if (UseDestinationStyles) insertOptions = InsertOptions.UseDestinationStyles;
                    if (KeepSourceFormatting) insertOptions = InsertOptions.KeepSourceFormatting;
                    range = document.AppendHtmlText(Text, insertOptions);
                }
                else
                    range = document.AppendText(Text);

                if (!string.IsNullOrWhiteSpace(CharacterStyle))
                {
                    var style = book.CharacterStyles[CharacterStyle] ?? throw new Exception($"Character style '{CharacterStyle}' does not exist.");

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

                if (!string.IsNullOrWhiteSpace(ParagraphStyle))
                {
                    var style = book.ParagraphStyles[ParagraphStyle] ?? throw new Exception($"Paragraph style '{ParagraphStyle}' does not exist.");

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

                if (ExpandFields)
                    ExpandFieldsInBookRange(range);
            }
            finally
            { 
                if (document != null)
                {
                    switch (HeaderFooter)
                    {
                        case DocumentType.Header:
                            section.EndUpdateHeader(document);
                            break;
                        case DocumentType.Footer:
                            section.EndUpdateFooter(document);
                            break;
                    }
                }
            }
        }
    }
}
