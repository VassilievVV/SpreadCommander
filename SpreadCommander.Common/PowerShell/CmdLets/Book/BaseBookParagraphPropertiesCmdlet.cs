using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using System.Drawing;
using System.Globalization;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    public class BaseBookParagraphPropertiesCmdlet : BaseBookCharacterPropertiesCmdlet
    {
        [Parameter(HelpMessage = "Paragraph's text alignment")]
        public ParagraphAlignment? Alignment { get; set; }

        [Parameter(HelpMessage = "Whether to suppress addition of additional space (contextual spacing) between paragraphs of the same style")]
        public SwitchParameter ContextualSpacing { get; set; }

        [Parameter(HelpMessage = "Reset ContextualSpacing from parent style")]
        public SwitchParameter ResetContextualSpacing { get; set; }

        [Parameter(HelpMessage = "Indent of the first line of a paragraph")]
        public float? FirstLineIdent { get; set; }

        [Parameter(HelpMessage = "Whether and how a paragraph's first line is indented")]
        public ParagraphFirstLineIndent? FirstLineIndentType { get; set; }

        [Parameter(HelpMessage = "Whether to prevent all page breaks that interrupt a paragraph")]
        public SwitchParameter KeepLinesTogether { get; set; }

        [Parameter(HelpMessage = "Reset KeepLinesTogether from parent style")]
        public SwitchParameter ResetKeepLinesTogether { get; set; }

        [Parameter(HelpMessage = "Whether to put a break between the current paragraph and the next paragraph")]
        public SwitchParameter KeepWithNext { get; set; }

        [Parameter(HelpMessage = "Reset KeepWithNext from parent style")]
        public SwitchParameter ResetKeepWithNext { get; set; }

        [Parameter(HelpMessage = "Paragraph's left indent")]
        public float? LeftIndent { get; set; }

        [Parameter(HelpMessage = "Line spacing value")]
        public float? LineSpacing { get; set; }

        [Parameter(HelpMessage = "Multiplier which is used to calculate the line spacing value")]
        public float? LineSpacingMultiplier { get; set; }

        [Parameter(HelpMessage = "Spacing between paragraph's lines")]
        public ParagraphLineSpacing? LineSpacingType { get; set; }

        [Parameter(HelpMessage = "Index of a list applied to the paragraph style")]
        public int? NumberingListIndex { get; set; }

        [Parameter(HelpMessage = "Paragraph's outline level")]
        public int? OutlineLevel { get; set; }

        [Parameter(HelpMessage = "Whether to insert a page break before specified paragraph(s)")]
        public SwitchParameter PageBreakBefore { get; set; }

        [Parameter(HelpMessage = "Reset PageBreakBefore from parent style")]
        public SwitchParameter ResetPageBreakBefore { get; set; }

        [Parameter(HelpMessage = "Paragraph's right indent")]
        public float? RightIndent { get; set; }

        [Parameter(HelpMessage = "Whether to change the paragraph's text direction to right-to-left")]
        public SwitchParameter RightToLeft { get; set; }

        [Parameter(HelpMessage = "Reset RightToLeft from parent style")]
        public SwitchParameter ResetRightToLeft { get; set; }

        [Parameter(HelpMessage = "Spacing after the current paragraph")]
        public float? SpacingAfter { get; set; }

        [Parameter(HelpMessage = "Spacing before the current paragraph")]
        public float? SpacingBefore { get; set; }

        [Parameter(HelpMessage = "Whether to hyphenate a paragraph")]
        public SwitchParameter SuppressHyphenation { get; set; }

        [Parameter(HelpMessage = "Reset SuppressHyphenation from parent style")]
        public SwitchParameter ResetSuppressHyphenation { get; set; }

        [Parameter(HelpMessage = "Whether to display line numbers for the paragraphs")]
        public SwitchParameter SuppressLineNumbers { get; set; }

        [Parameter(HelpMessage = "Reset SuppressLineNumbers from parent style")]
        public SwitchParameter ResetSuppressLineNumbers { get; set; }

        [Parameter(HelpMessage = "Whether to apply control over the widow and orphan lines")]
        public SwitchParameter WidowOrphanControl { get; set; }

        [Parameter(HelpMessage = "Reset WidowOrphanControl from parent style")]
        public SwitchParameter ResetWidowOrphanControl { get; set; }


        protected virtual void SetParagraphProperties(ParagraphPropertiesBase style)
        {
            if (style is CharacterPropertiesBase characterStyle)
                base.SetCharacterProperties(characterStyle);

            if (Alignment.HasValue)
                style.Alignment = Alignment;

            if (ContextualSpacing) style.ContextualSpacing = true;
            if (ResetContextualSpacing) style.ContextualSpacing = false;

            if (FirstLineIdent.HasValue)
                style.FirstLineIndent = FirstLineIdent;
            if (FirstLineIndentType.HasValue)
                style.FirstLineIndentType = FirstLineIndentType;

            if (KeepLinesTogether) style.KeepLinesTogether = true;
            if (ResetKeepLinesTogether) style.KeepLinesTogether = false;

            if (KeepWithNext) style.KeepWithNext = true;
            if (ResetKeepWithNext) style.KeepWithNext = false;

            if (LeftIndent.HasValue)
                style.LeftIndent = LeftIndent;

            if (LineSpacing.HasValue)
                style.LineSpacing = LineSpacing;

            if (LineSpacingMultiplier.HasValue)
                style.LineSpacingMultiplier = LineSpacingMultiplier;

            if (LineSpacingType.HasValue)
                style.LineSpacingType = LineSpacingType;

            if (NumberingListIndex.HasValue && style is ParagraphStyle paragraphStyle)
                paragraphStyle.NumberingListIndex = NumberingListIndex.Value;

            if (OutlineLevel.HasValue)
                style.OutlineLevel = OutlineLevel;

            if (PageBreakBefore) style.PageBreakBefore = true;
            if (ResetPageBreakBefore) style.PageBreakBefore = false;

            if (RightIndent.HasValue)
                style.RightIndent = RightIndent;

            if (RightToLeft) style.RightToLeft = true;
            if (ResetRightToLeft) style.RightToLeft = false;

            if (SpacingAfter.HasValue)
                style.SpacingAfter = SpacingAfter;
            if (SpacingBefore.HasValue)
                style.SpacingBefore = SpacingBefore;

            if (SuppressHyphenation) style.SuppressHyphenation = true;
            if (ResetSuppressHyphenation) style.SuppressHyphenation = false;

            if (SuppressLineNumbers) style.SuppressLineNumbers = true;
            if (ResetSuppressLineNumbers) style.SuppressLineNumbers = false;

            if (WidowOrphanControl) style.WidowOrphanControl = true;
            if (ResetWidowOrphanControl) style.WidowOrphanControl = false;
        }
    }
}
