using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using System.Drawing;
using System.Globalization;

namespace SpreadCommander.Common.Script.Book
{
    public class CharacterStyleOptions: BookOptions
    {
        [Description("Whether all characters are capital letters")]
        public bool? AllCaps { get; set; }

        [Description("Background color of character(s)")]
        public string BackColor { get; set; }

        [Description("Whether characters are bold")]
        public bool? Bold { get; set; }

        [Description("Font")]
        public string Font { get; set; }

        [Description("Character(s) font name")]
        public string FontName { get; set; }

        [Description("Character(s) font size")]
        public float? FontSize { get; set; }

        [Description("Foreground color of characters")]
        public string ForeColor { get; set; }

        [Description("Whether a character(s) is hidden")]
        public bool? Hidden { get; set; }

        [Description("Text's highlight color")]
        public string HighlightColor { get; set; }

        [Description("Whether a character(s) is italicized")]
        public bool? Italic { get; set; }

        [Description("Spell check language, up to 3 cultures for Latin, BiDi and East Asia.")]
        public string Language { get; set; }

        [Description("Whether or not the text shall be proof read by the spell checker")]
        public bool? NoProof { get; set; }

        [Description("Whether characters are strikeout")]
        public StrikeoutType? Strikeout { get; set; }

        [Description("Whether character(s) are formatted as subscript")]
        public bool? Subscript { get; set; }

        [Description("Whether character(s) are formatted as superscript")]
        public bool? Superscript { get; set; }

        [Description("Type of underline applied to the character(s)")]
        public UnderlineType? Underline { get; set; }

        [Description("Underline color for the specified character(s)")]
        public string UnderlineColor { get; set; }

        [Description("Font's scaling percentage (from 1 to 600)")]
        public int? Scale { get; set; }

        [Description("Spacing (in twips) between characters (from -31680 to 31680)")]
        public int? Spacing { get; set; }

        [Description("Position of characters (in points) relative to the base line (from -3168 to 3168)")]
        public float? Position { get; set; }

        [Description("Whether to snap East-Asian characters to a grid when the grid is defined")]
        public bool? SnapToGrid { get; set; }

        [Description("Minimum font size for which the kerning is adjusted automatically")]
        public float? KerningThreshold { get; set; }

        [Description("Whether all characters are small capital letters")]
        public bool? SmallCaps { get; set; }

        [Description("Theme font for East Asian characters")]
        public ThemeFont? ThemeFontEastAsia { get; set; }

        [Description("Theme font for the Complex Script characters (right-to-left languages)")]
        public ThemeFont? ThemeFontComplexScript { get; set; }

        [Description("Theme font for High ANSI characters")]
        public ThemeFont? ThemeFontHighAnsi { get; set; }

        [Description("Theme font for Unicode (U+0000–U+007F) characters")]
        public ThemeFont? ThemeFontAscii { get; set; }

        [Description("Font name for East Asian characters")]
        public string FontNameEastAsia { get; set; }

        [Description("Font name for the Complex Script characters (right-to-left languages)")]
        public string FontNameComplexScript { get; set; }

        [Description("Font name for High ANSI characters")]
        public string FontNameHighAnsi { get; set; }

        [Description("Font name for Unicode (U+0000–U+007F) characters")]
        public string FontNameAscii { get; set; }
    }

    public partial class SCBook
    {
        protected virtual void SetCharacterOptions(CharacterPropertiesBase style, CharacterStyleOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            if (!string.IsNullOrWhiteSpace(options.Font))
                Utils.StringToCharacterPropertiesFont(options.Font, style);

            if (options.AllCaps.HasValue) style.AllCaps = options.AllCaps.Value;

            var backColor = Utils.ColorFromString(options.BackColor);
            if (backColor != Color.Empty)
                style.BackColor = backColor;

            if (options.Bold.HasValue) style.Bold = options.Bold.Value;

            if (!string.IsNullOrWhiteSpace(options.FontName))
                style.FontName = options.FontName;

            if (options.FontSize.HasValue)
                style.FontSize = options.FontSize;

            var foreColor = Utils.ColorFromString(options.ForeColor);
            if (foreColor != Color.Empty)
                style.ForeColor = foreColor;

            if (options.Hidden.HasValue) style.Hidden = options.Hidden.Value;

            var highlightColor = Utils.ColorFromString(options.HighlightColor);
            if (highlightColor != Color.Empty)
                style.HighlightColor = highlightColor;

            if (options.Italic.HasValue) style.Italic = options.Italic;

            if (!string.IsNullOrWhiteSpace(options.Language))
            {
                var langParts       = options.Language.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var latinCulture    = langParts.Length > 0 ? new CultureInfo(langParts[0]) : CultureInfo.InvariantCulture;
                var bidiCulture     = langParts.Length > 1 ? new CultureInfo(langParts[1]) : CultureInfo.InvariantCulture;
                var eastAsiaCulture = langParts.Length > 2 ? new CultureInfo(langParts[2]) : CultureInfo.InvariantCulture;

                var langInfo = new DevExpress.XtraRichEdit.Model.LangInfo(latinCulture, bidiCulture, eastAsiaCulture);
                style.Language = langInfo;
            }

            if (options.NoProof.HasValue) style.NoProof = options.NoProof.Value;

            if (options.Strikeout.HasValue)
                style.Strikeout = (DevExpress.XtraRichEdit.API.Native.StrikeoutType)options.Strikeout.Value;

            if (options.Subscript.HasValue) style.Subscript = options.Subscript;

            if (options.Superscript.HasValue) style.Superscript = options.Superscript;

            if (options.Underline.HasValue)
                style.Underline = (DevExpress.XtraRichEdit.API.Native.UnderlineType)options.Underline.Value;

            var underlineColor = Utils.ColorFromString(options.UnderlineColor);
            if (underlineColor != Color.Empty)
                style.UnderlineColor = underlineColor;

            if (options.Scale.HasValue)
                style.Scale = options.Scale.Value;

            if (options.Spacing.HasValue)
                style.Spacing = options.Spacing.Value;

            if (options.Position.HasValue)
                style.Position = options.Position.Value;

            if (options.SnapToGrid.HasValue) style.SnapToGrid = options.SnapToGrid.Value;

            if (options.KerningThreshold.HasValue)
                style.KerningThreshold = options.KerningThreshold.Value;

            if (options.SmallCaps.HasValue) style.SmallCaps = options.SmallCaps.Value;

            if (options.ThemeFontEastAsia.HasValue)
                style.ThemeFontEastAsia = (DevExpress.XtraRichEdit.API.Native.ThemeFont)options.ThemeFontEastAsia.Value;
            if (options.ThemeFontComplexScript.HasValue)
                style.ThemeFontComplexScript = (DevExpress.XtraRichEdit.API.Native.ThemeFont)options.ThemeFontComplexScript.Value;
            if (options.ThemeFontHighAnsi.HasValue)
                style.ThemeFontHighAnsi = (DevExpress.XtraRichEdit.API.Native.ThemeFont)options.ThemeFontHighAnsi.Value;
            if (options.ThemeFontAscii.HasValue)
                style.ThemeFontAscii = (DevExpress.XtraRichEdit.API.Native.ThemeFont)options.ThemeFontAscii.Value;

            if (!string.IsNullOrWhiteSpace(options.FontNameEastAsia))
                style.FontNameEastAsia = options.FontNameEastAsia;
            if (!string.IsNullOrWhiteSpace(options.FontNameComplexScript))
                style.FontNameComplexScript = options.FontNameComplexScript;
            if (!string.IsNullOrWhiteSpace(options.FontNameHighAnsi))
                style.FontNameHighAnsi = options.FontNameHighAnsi;
            if (!string.IsNullOrWhiteSpace(options.FontNameAscii))
                style.FontNameAscii = options.FontNameAscii;
        }
    }
}
