using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;

namespace SpreadCommander.Documents.Dialogs
{
    public class SpreadRichDocumentVisitor : DocumentVisitorBase
    {
        private readonly RichTextString richTextString;
        private readonly int endPosition;

        public RichTextString RichText
        {
            get { return richTextString; }
        }

        public SpreadRichDocumentVisitor(int endPos)
        {
            richTextString = new RichTextString();
            endPosition    = endPos;
        }

        public override void Visit(DocumentText text)
        {
            base.Visit(text);
            RichTextRunFont runFont = CreateRichTextRun(text.TextProperties);
            richTextString.AddTextRun(text.Text, runFont);
        }

        public override void Visit(DocumentParagraphEnd paragraphEnd)
        {
            base.Visit(paragraphEnd);
            if (endPosition - 1 != paragraphEnd.Position)
            {
                RichTextRunFont runFont = CreateRichTextRun(paragraphEnd.TextProperties);
                richTextString.AddTextRun(paragraphEnd.Text, runFont);
            }
        }

        private static RichTextRunFont CreateRichTextRun(ReadOnlyTextProperties tp)
        {
            var runFont = new RichTextRunFont(tp.FontName, tp.DoubleFontSize / 2, tp.ForeColor)
            {
                Bold          = tp.FontBold,
                Italic        = tp.FontItalic,
                Strikethrough = tp.StrikeoutType == StrikeoutType.Single
            };

            runFont.Script = tp.Script switch
            {
                DevExpress.Office.CharacterFormattingScript.Subscript   => ScriptType.Subscript,
                DevExpress.Office.CharacterFormattingScript.Superscript => ScriptType.Superscript,
                _                                                       => ScriptType.None
            };
            runFont.UnderlineType = tp.UnderlineType switch
            {
                DevExpress.XtraRichEdit.API.Native.UnderlineType.Single => DevExpress.Spreadsheet.UnderlineType.Single,
                DevExpress.XtraRichEdit.API.Native.UnderlineType.Double => DevExpress.Spreadsheet.UnderlineType.Double,
                _                                                       => DevExpress.Spreadsheet.UnderlineType.None
            };
            return runFont;
        }
    }
}
