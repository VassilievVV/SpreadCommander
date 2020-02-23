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
    public class BaseBookCharacterPropertiesCmdlet : BaseBookCmdlet
    {
        [Parameter(HelpMessage = "Whether all characters are capital letters")]
        [Alias("caps")]
        public SwitchParameter AllCaps { get; set; }

        [Parameter(HelpMessage = "Reset AllCaps from parent style")]
        public SwitchParameter ResetAllCaps { get; set; }

        [Parameter(HelpMessage = "Background color of character(s)")]
        [Alias("back")]
        public string BackColor { get; set; }

        [Parameter(HelpMessage = "Whether characters are bold")]
        [Alias("b")]
        public SwitchParameter Bold { get; set; }

        [Parameter(HelpMessage = "Reset Bold from parent style")]
        public SwitchParameter ResetBold { get; set; }

        [Parameter(HelpMessage = "Font")]
        [Alias("f")]
        public string Font { get; set; }

        [Parameter(HelpMessage = "Character(s) font name")]
        [Alias("fn")]
        public string FontName { get; set; }

        [Parameter(HelpMessage = "Character(s) font size")]
        [Alias("fs,size")]
        public float? FontSize { get; set; }

        [Parameter(HelpMessage = "Foreground color of characters")]
        [Alias("fore")]
        public string ForeColor { get; set; }

        [Parameter(HelpMessage = "Whether a character(s) is hidden")]
        public SwitchParameter Hidden { get; set; }

        [Parameter(HelpMessage = "Reset Hidden from parent style")]
        public SwitchParameter ResetHidden { get; set; }

        [Parameter(HelpMessage = "Text's highlight color")]
        [Alias("highlight")]
        public string HighlightColor { get; set; }

        [Parameter(HelpMessage = "Whether a character(s) is italicized")]
        [Alias("i")]
        public SwitchParameter Italic { get; set; }

        [Parameter(HelpMessage = "Reset Italic from parent style")]
        public SwitchParameter ResetItalic { get; set; }

        [Parameter(HelpMessage = "Spell check language, up to 3 cultures for Latin, BiDi and East Asia.")]
        [Alias("lang")]
        public string Language { get; set; }

        [Parameter(HelpMessage = "Whether or not the text shall be proof read by the spell checker")]
        public SwitchParameter NoProof { get; set; }

        [Parameter(HelpMessage = "Reset NoProof from parent style")]
        public SwitchParameter ResetNoProof { get; set; }

        [Parameter(HelpMessage = "Whether characters are strikeout")]
        [Alias("strike")]
        public StrikeoutType? Strikeout { get; set; }

        [Parameter(HelpMessage = "Whether character(s) are formatted as subscript")]
        [Alias("sub")]
        public SwitchParameter Subscript { get; set; }

        [Parameter(HelpMessage = "Reset Subscript from parent style")]
        public SwitchParameter ResetSubscript { get; set; }

        [Parameter(HelpMessage = "Whether character(s) are formatted as superscript")]
        [Alias("super")]
        public SwitchParameter Superscript { get; set; }

        [Parameter(HelpMessage = "Reset Superscript from parent style")]
        public SwitchParameter ResetSuperscript { get; set; }

        [Parameter(HelpMessage = "Type of underline applied to the character(s)")]
        public UnderlineType? Underline { get; set; }

        [Parameter(HelpMessage = "Underline color for the specified character(s)")]
        public string UnderlineColor { get; set; }


        protected override void ProcessRecord()
        {
            ExecuteSynchronized(() => DoProcessRecord(GetCmdletBook()));
        }

        protected virtual void DoProcessRecord(Document book)
        {
        }

        protected virtual void SetCharacterProperties(CharacterPropertiesBase style)
        { 
            if (!string.IsNullOrWhiteSpace(Font))
                Utils.StringToCharacterPropertiesFont(Font, style);

            if (AllCaps) style.AllCaps      = true;
            if (ResetAllCaps) style.AllCaps = false;

            var backColor = Utils.ColorFromString(BackColor);
            if (backColor != Color.Empty)
                style.BackColor = backColor;

            if (Bold) style.Bold      = true;
            if (ResetBold) style.Bold = false;

            if (!string.IsNullOrWhiteSpace(FontName))
                style.FontName = FontName;

            if (FontSize.HasValue)
                style.FontSize = FontSize;

            var foreColor = Utils.ColorFromString(ForeColor);
            if (foreColor != Color.Empty)
                style.ForeColor = foreColor;

            if (Hidden) style.Hidden = true;
            if (ResetHidden) style.Hidden = false;

            var highlightColor = Utils.ColorFromString(HighlightColor);
            if (highlightColor != Color.Empty)
                style.HighlightColor = highlightColor;

            if (Italic) style.Italic = true;
            if (ResetItalic) style.Italic = false;

            if (!string.IsNullOrWhiteSpace(Language))
            {
                var langParts       = Language.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var latinCulture    = langParts.Length > 0 ? new CultureInfo(langParts[0]) : CultureInfo.InvariantCulture;
                var bidiCulture     = langParts.Length > 1 ? new CultureInfo(langParts[1]) : CultureInfo.InvariantCulture;
                var eastAsiaCulture = langParts.Length > 2 ? new CultureInfo(langParts[2]) : CultureInfo.InvariantCulture;

                var langInfo   = new DevExpress.XtraRichEdit.Model.LangInfo(latinCulture, bidiCulture, eastAsiaCulture);
                style.Language = langInfo;
            }

            if (NoProof) style.NoProof      = true;
            if (ResetNoProof) style.NoProof = false;

            if (Strikeout.HasValue)
                style.Strikeout = Strikeout;

            if (Subscript) style.Subscript      = true;
            if (ResetSubscript) style.Subscript = false;

            if (Superscript) style.Superscript    = true;
            if (ResetSubscript) style.Superscript = false;

            if (Underline.HasValue)
                style.Underline = Underline;

            var underlineColor = Utils.ColorFromString(UnderlineColor);
            if (underlineColor != Color.Empty)
                style.UnderlineColor = underlineColor;
        }
    }
}
