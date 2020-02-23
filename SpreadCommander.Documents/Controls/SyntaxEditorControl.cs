#pragma warning disable CRR0047

using Alsing.Windows.Forms.Document.DocumentStructure.Word;
using Alsing.Windows.Forms.Document.SyntaxDefinition;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
using SpreadCommander.Common.Extensions;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using Alsing.Windows.Forms.Controls.EditView;
using Alsing.Windows.Forms.Controls.SyntaxBox;
using Alsing.Windows.Forms.Controls.SplitView;
using Alsing.Windows.Forms.Classes;

namespace SpreadCommander.Documents.Controls
{
    class EditorHScrollBar : DevExpress.XtraEditors.HScrollBar, Alsing.Windows.Forms.Controls.SplitView.IScrollBar
    {
    }

    class EditorVScrollBar : DevExpress.XtraEditors.VScrollBar, Alsing.Windows.Forms.Controls.SplitView.IScrollBar
    {
    }

    public class ViewControl : EditViewControl
    {
        public ViewControl(SyntaxBoxControl parent) : base(parent)
        {
        }

        protected override Alsing.Windows.Forms.Controls.SplitView.IScrollBar CreateHScrollBar() => new EditorHScrollBar();
        protected override Alsing.Windows.Forms.Controls.SplitView.IScrollBar CreateVScrollBar() => new EditorVScrollBar();
    }

    public class SyntaxEditorControl : SyntaxBoxControl
    {
        private string _SyntaxName;
        private SyntaxDefinition _InitialSyntaxDefinition;

        public SyntaxEditorControl(): base()
        {
            UserLookAndFeel.Default.StyleChanged += LookAndFeel_StyleChanged;
            StoreInitialColors();
        }

        private void LookAndFeel_StyleChanged(object sender, EventArgs e)
        {
            ApplyStyle();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyStyle();
        }

        protected override SplitViewChildControl GetNewView() => new ViewControl(this);

        public void RemoveKeyboardAction(Keys key, bool shift, bool control, bool alt)
        {
            int i = KeyboardActions.Count - 1;
            while (i >= 0)
            {
                KeyboardAction ka = KeyboardActions[i];

                if (ka.Key == key &&
                    ka.Alt == alt &&
                    ka.Shift == shift &&
                    ka.Control == control)
                    KeyboardActions.Remove(ka);

                i--;
            }
        }

        private Color _BorderColor;
        private Color _BracketBackColor;
        private Color _BracketBorderColor;
        private Color _BracketForeColor;
        private Color _BreakPointBackColor;
        private Color _BreakPointForeColor;
        private Color _ChildBorderColor;
        private Color _EOLMarkerColor;
        private Color _GutterMarginBorderColor;
        private Color _GutterMarginColor;
        private Color _HighLightedLineColor;
        private Color _InactiveSelectionBackColor;
        //private Color _InactiveSelectionForeColor;
        private Color _LineNumberBackColor;
        private Color _LineNumberBorderColor;
        private Color _LineNumberForeColor;
        private Color _OutlineColor;
        private Color _ScopeBackColor;
        private Color _ScopeIndicatorColor;
        private Color _SelectionBackColor;
        private Color _SelectionForeColor;
        private Color _SeparatorColor;
        private Color _TabGuideColor;
        private Color _WhitespaceColor;

        protected void StoreInitialColors()
        {
            _BorderColor                = this.BorderColor;
            _BracketBackColor           = this.BracketBackColor;
            _BracketBorderColor         = this.BracketBorderColor;
            _BracketForeColor           = this.BracketForeColor;
            _BreakPointBackColor        = this.BreakPointBackColor;
            _BreakPointForeColor        = this.BreakPointForeColor;
            _ChildBorderColor           = this.ChildBorderColor;
            _EOLMarkerColor             = this.EOLMarkerColor;
            _GutterMarginBorderColor    = this.GutterMarginBorderColor;
            _GutterMarginColor          = this.GutterMarginColor;
            _HighLightedLineColor       = this.HighLightedLineColor;
            _InactiveSelectionBackColor = this.InactiveSelectionBackColor;
            //_InactiveSelectionForeColor = this.InactiveSelectionForeColor;
            _LineNumberBackColor        = this.LineNumberBackColor;
            _LineNumberBorderColor      = this.LineNumberBorderColor;
            _LineNumberForeColor        = this.LineNumberForeColor;
            _OutlineColor               = this.OutlineColor;
            _ScopeBackColor             = this.ScopeBackColor;
            _ScopeIndicatorColor        = this.ScopeIndicatorColor;
            _SelectionBackColor         = this.SelectionBackColor;
            _SelectionForeColor         = this.SelectionForeColor;
            _SeparatorColor             = this.SeparatorColor;
            _TabGuideColor              = this.TabGuideColor;
            _WhitespaceColor            = this.WhitespaceColor;
        }

        protected SyntaxDefinition LoadSyntax()
        {
            if (string.IsNullOrWhiteSpace(_SyntaxName))
                return null;

            using Stream streamSyntax = Utils.GetEmbeddedResource(Assembly.GetAssembly(typeof(SpreadCommanderCommon)), $"SyntaxFiles.{_SyntaxName}.syn");
            streamSyntax.Seek(0, SeekOrigin.Begin);
            using StreamReader reader = new StreamReader(streamSyntax);
            var syntax = SyntaxDefinition.FromSyntaxXml(reader.ReadToEnd());
            return syntax;
        }

        protected void ApplyStyle()
        {
            _InitialSyntaxDefinition = LoadSyntax();
            var syntaxDefinition     = LoadSyntax();

            var skin = CommonSkins.GetSkin(UserLookAndFeel.Default);
            var backColor = skin.TranslateColor(SystemColors.Window);
            var foreColor = skin.TranslateColor(SystemColors.WindowText);

            BackColor = backColor;
            ForeColor = foreColor;

            bool isSkinDark = backColor.IsDark();

            if (syntaxDefinition != null)
            {
                foreach (var textStyle in syntaxDefinition.Styles)
                {
                    if (textStyle.BackColor == Color.Transparent || textStyle.BackColor.ToArgb() == Color.Empty.ToArgb())
                        textStyle.BackColor = SystemColors.Window;
                    textStyle.BackColor = skin.TranslateColor(textStyle.BackColor);
                    //textStyle.BackColor = textStyle.BackColor != SystemColors.Window ? isSkinDark ? textStyle.BackColor.Invert() : textStyle.BackColor : backColor;

                    if (textStyle.ForeColor == Color.Transparent || textStyle.ForeColor.ToArgb() == Color.Empty.ToArgb())
                        textStyle.ForeColor = SystemColors.WindowText;
                    textStyle.ForeColor = skin.TranslateColor(textStyle.ForeColor);
                    //textStyle.ForeColor = textStyle.ForeColor != SystemColors.WindowText ? isSkinDark ? textStyle.ForeColor.Invert() : textStyle.ForeColor : foreColor;
                }
            }

            BorderColor                = skin.TranslateColor(_BorderColor);
            BracketBackColor           = skin.TranslateColor(_BracketBackColor);
            BracketBorderColor         = skin.TranslateColor(_BracketBorderColor);
            BracketForeColor           = skin.TranslateColor(_BracketForeColor);
            BreakPointBackColor        = skin.TranslateColor(_BreakPointBackColor);
            BreakPointForeColor        = skin.TranslateColor(_BreakPointForeColor);
            ChildBorderColor           = skin.TranslateColor(_ChildBorderColor);
            EOLMarkerColor             = skin.TranslateColor(_EOLMarkerColor);
            GutterMarginBorderColor    = skin.TranslateColor(_GutterMarginBorderColor);
            GutterMarginColor          = skin.TranslateColor(_GutterMarginColor);
            HighLightedLineColor       = skin.TranslateColor(_HighLightedLineColor);
            InactiveSelectionBackColor = skin.TranslateColor(_InactiveSelectionBackColor);
            InactiveSelectionForeColor = //skin.TranslateColor(Color.FromArgb(_InactiveSelectionForeColor.ToArgb()));
                                            isSkinDark ? Color.White : Color.Black;
            LineNumberBackColor        = skin.TranslateColor(_LineNumberBackColor);
            LineNumberBorderColor      = skin.TranslateColor(_LineNumberBorderColor);
            LineNumberForeColor        = skin.TranslateColor(_LineNumberForeColor);
            OutlineColor               = skin.TranslateColor(_OutlineColor);
            ScopeBackColor             = skin.TranslateColor(_ScopeBackColor);
            ScopeIndicatorColor        = skin.TranslateColor(_ScopeIndicatorColor);
            SelectionBackColor         = skin.TranslateColor(_SelectionBackColor);
            SelectionForeColor         = skin.TranslateColor(_SelectionForeColor);
            SeparatorColor             = skin.TranslateColor(_SeparatorColor);
            TabGuideColor              = skin.TranslateColor(_TabGuideColor);
            WhitespaceColor            = skin.TranslateColor(_WhitespaceColor);

            Word.DefaultErrorColor     = skin.TranslateColor(Color.Red);

            foreach (var view in Views)
            {
                view.TopThumb.BackColor  = BorderColor;
                view.LeftThumb.BackColor = BorderColor;
                view.FillerBackColor     = backColor;
            }
            SplitViewBackColor = skin.TranslateColor(Color.FromArgb((isSkinDark ? SystemColors.ControlDarkDark : SystemColors.ControlLightLight).ToArgb()));

            if (syntaxDefinition != null)
                Document.Parser.Init(syntaxDefinition);
            else
                Document.Parser.SyntaxDefinition = null;
            Document.ReParse();
        }

        public void SetParserSyntax(string syntaxName)
        {
            _SyntaxName = syntaxName;
            ApplyStyle();
        }

        public override void Copy()
        {
            var syntaxDefinition = Document.Parser.SyntaxDefinition;
            try
            {
                if (_InitialSyntaxDefinition != null)
                    Document.Parser.Init(_InitialSyntaxDefinition);

                base.Copy();
            }
            finally
            {
                if (_InitialSyntaxDefinition != null)
                    Document.Parser.Init(syntaxDefinition);
            }
        }

        public override void Cut()
        {
            var syntaxDefinition = Document.Parser.SyntaxDefinition;
            try
            {
                if (_InitialSyntaxDefinition != null)
                    Document.Parser.Init(_InitialSyntaxDefinition);

                base.Cut();
            }
            finally
            {
                if (_InitialSyntaxDefinition != null)
                    Document.Parser.Init(syntaxDefinition);
            }
        }

        public string ExportToRTF(bool selection)
        {
            var syntaxDefinition = Document.Parser.SyntaxDefinition;
            try
            {
                if (_InitialSyntaxDefinition != null)
                    Document.Parser.Init(_InitialSyntaxDefinition);

                var result = (ActiveViewControl as EditViewControl)?.ExportToRTF(selection);
                return result;
            }
            finally
            {
                if (_InitialSyntaxDefinition != null)
                    Document.Parser.Init(syntaxDefinition);
            }
        }
    }
}