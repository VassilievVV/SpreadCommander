using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class ParagraphStyleOptions: CharacterStyleOptions
    {
        [Description("Paragraph's text alignment")]
        public ParagraphAlignment? Alignment { get; set; }

        [Description("Whether to suppress addition of additional space (contextual spacing) between paragraphs of the same style")]
        public bool? ContextualSpacing { get; set; }

        [Description("Indent of the first line of a paragraph")]
        public float? FirstLineIdent { get; set; }

        [Description("Whether and how a paragraph's first line is indented")]
        public ParagraphFirstLineIndent? FirstLineIndentType { get; set; }

        [Description("Whether to prevent all page breaks that interrupt a paragraph")]
        public bool? KeepLinesTogether { get; set; }

        [Description("Whether to put a break between the current paragraph and the next paragraph")]
        public bool? KeepWithNext { get; set; }

        [Description("Paragraph's left indent")]
        public float? LeftIndent { get; set; }

        [Description("Line spacing value")]
        public float? LineSpacing { get; set; }

        [Description("Multiplier which is used to calculate the line spacing value")]
        public float? LineSpacingMultiplier { get; set; }

        [Description("Spacing between paragraph's lines")]
        public ParagraphLineSpacing? LineSpacingType { get; set; }

        [Description("Index of a list applied to the paragraph style")]
        public int? NumberingListIndex { get; set; }

        [Description("Paragraph's outline level")]
        public int? OutlineLevel { get; set; }

        [Description("Whether to insert a page break before specified paragraph(s)")]
        public bool? PageBreakBefore { get; set; }

        [Description("Paragraph's right indent")]
        public float? RightIndent { get; set; }

        [Description("Whether to change the paragraph's text direction to right-to-left")]
        public bool? RightToLeft { get; set; }

        [Description("Spacing after the current paragraph")]
        public float? SpacingAfter { get; set; }

        [Description("Spacing before the current paragraph")]
        public float? SpacingBefore { get; set; }

        [Description("Whether to hyphenate a paragraph")]
        public bool? SuppressHyphenation { get; set; }

        [Description("Whether to display line numbers for the paragraphs")]
        public bool? SuppressLineNumbers { get; set; }

        [Description("Whether to apply control over the widow and orphan lines")]
        public bool? WidowOrphanControl { get; set; }
    }

    public partial class SCBook
    {
        protected virtual void SetParagraphOptions(ParagraphPropertiesBase style, ParagraphStyleOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            if (style is CharacterPropertiesBase characterStyle)
                this.SetCharacterOptions(characterStyle, options);

            if (options.Alignment.HasValue)
                style.Alignment = (DevExpress.XtraRichEdit.API.Native.ParagraphAlignment)options.Alignment.Value;

            if (options.ContextualSpacing.HasValue) style.ContextualSpacing = options.ContextualSpacing.Value;

            if (options.FirstLineIdent.HasValue)
                style.FirstLineIndent = options.FirstLineIdent.Value;
            if (options.FirstLineIndentType.HasValue)
                style.FirstLineIndentType = (DevExpress.XtraRichEdit.API.Native.ParagraphFirstLineIndent)options.FirstLineIndentType.Value;

            if (options.KeepLinesTogether.HasValue) style.KeepLinesTogether = options.KeepLinesTogether.Value;

            if (options.KeepWithNext.HasValue) style.KeepWithNext = options.KeepWithNext.Value;

            if (options.LeftIndent.HasValue)
                style.LeftIndent = options.LeftIndent.Value;

            if (options.LineSpacing.HasValue)
                style.LineSpacing = options.LineSpacing.Value;

            if (options.LineSpacingMultiplier.HasValue)
                style.LineSpacingMultiplier = options.LineSpacingMultiplier.Value;

            if (options.LineSpacingType.HasValue)
                style.LineSpacingType = (DevExpress.XtraRichEdit.API.Native.ParagraphLineSpacing)options.LineSpacingType.Value;

            if (options.NumberingListIndex.HasValue && style is ParagraphStyle paragraphStyle)
                paragraphStyle.NumberingListIndex = options.NumberingListIndex.Value;

            if (options.OutlineLevel.HasValue)
                style.OutlineLevel = options.OutlineLevel;

            if (options.PageBreakBefore.HasValue) style.PageBreakBefore = options.PageBreakBefore.Value;

            if (options.RightIndent.HasValue)
                style.RightIndent = options.RightIndent.Value;

            if (options.RightToLeft.HasValue) style.RightToLeft = options.RightToLeft.Value;

            if (options.SpacingAfter.HasValue)
                style.SpacingAfter = options.SpacingAfter.Value;
            if (options.SpacingBefore.HasValue)
                style.SpacingBefore = options.SpacingBefore.Value;

            if (options.SuppressHyphenation.HasValue) style.SuppressHyphenation = options.SuppressHyphenation.Value;

            if (options.SuppressLineNumbers.HasValue) style.SuppressLineNumbers = options.SuppressLineNumbers.Value;

            if (options.WidowOrphanControl.HasValue) style.WidowOrphanControl = options.WidowOrphanControl.Value;
        }
    }
}
