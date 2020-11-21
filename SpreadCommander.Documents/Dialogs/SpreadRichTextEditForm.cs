using System;
using System.Drawing;
using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraSpreadsheet;
using DevExpress.XtraSpreadsheet.Menu;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class SpreadRichTextEditForm : BaseRibbonForm
    {
        private readonly Cell cell;

        public SpreadRichTextEditForm(Cell cell)
        {
            InitializeComponent();
            this.cell = cell;
            InitRichEditControl();
        }

        private void InitRichEditControl()
        {
            using (new UsingProcessor(() => Editor.BeginUpdate(), () => Editor.EndUpdate()))
            {
                if (cell.HasRichText)
                {
                    RichTextString richText = cell.GetRichText();
                    Document document = Editor.Document;

                    foreach (RichTextRun run in richText.Runs)
                    {
                        DocumentRange range = document.InsertText(document.Range.End, run.Text);
                        CharacterProperties cp = document.BeginUpdateCharacters(range);
                        try
                        { 
                            cp.Bold      = run.Font.Bold;
                            cp.ForeColor = run.Font.Color;
                            cp.Italic    = run.Font.Italic;
                            cp.FontName  = run.Font.Name;
                            cp.FontSize  = (float)run.Font.Size;
                            cp.Strikeout = run.Font.Strikethrough ? StrikeoutType.Single : StrikeoutType.None;

                            switch (run.Font.Script)
                            {
                                case ScriptType.Subscript:
                                    cp.Subscript = true;
                                    break;
                                case ScriptType.Superscript:
                                    cp.Superscript = true;
                                    break;
                                default:
                                    cp.Subscript   = false;
                                    cp.Superscript = false;
                                    break;
                            }
                            cp.Underline = run.Font.UnderlineType switch
                            {
                                DevExpress.Spreadsheet.UnderlineType.Single => DevExpress.XtraRichEdit.API.Native.UnderlineType.Single,
                                DevExpress.Spreadsheet.UnderlineType.Double => DevExpress.XtraRichEdit.API.Native.UnderlineType.Double,
                                _                                           => DevExpress.XtraRichEdit.API.Native.UnderlineType.None
                            };
                        }
                        finally
                        { 
                            document.EndUpdateCharacters(cp);
                        }
                    }
                }
                else
                    Editor.Text = cell.DisplayText;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            var visitor = new SpreadRichDocumentVisitor(Editor.Document.Range.End.ToInt());
            using (DocumentIterator iterator = new DocumentIterator(Editor.Document, true))
            {
                while (iterator.MoveNext())
                {
                    iterator.Current.Accept(visitor);
                }
            }
            RichTextString richText = visitor.RichText;
            cell.SetRichText(richText);
            if (Editor.Document.Paragraphs.Count > 1)
                cell.Alignment.WrapText = true;
        }

        public static void SpreadsheetControl_CellBeginEdit(object sender, DevExpress.XtraSpreadsheet.SpreadsheetCellCancelEventArgs e)
        {
            if (e.Cell.HasRichText)
            {
                e.Cancel = true;
                using var richEditForm = new SpreadRichTextEditForm(e.Cell);
                richEditForm.ShowDialog();
            }
        }

        public static void SpreadsheetControl_PopupMenuShowing(object sender, DevExpress.XtraSpreadsheet.PopupMenuShowingEventArgs e)
        {
            if (sender is not SpreadsheetControl spreadsheetControl)
                return;

            if (e.MenuType == DevExpress.XtraSpreadsheet.SpreadsheetMenuType.Cell)
            {
                Cell activeCell = spreadsheetControl.ActiveCell;
                if (activeCell.Value.IsEmpty || (!activeCell.HasFormula && activeCell.Value.IsText))
                {
                    var setRichTextItem = new SpreadsheetMenuItem("Set rich text ...",
                        (s, args) =>
                        {
                            if (spreadsheetControl?.IsDisposed ?? true)
                                return;

                            using var richEditForm = new SpreadRichTextEditForm(spreadsheetControl.ActiveCell);
                            richEditForm.ShowDialog();
                        })
                    {
                        BeginGroup = true
                    };
                    setRichTextItem.ImageOptions.SvgImage     = (DevExpress.Utils.Svg.SvgImage)Properties.Resources.List;
                    setRichTextItem.ImageOptions.SvgImageSize = new Size(16, 16);

                    e.Menu.Items.Add(setRichTextItem);
                }
            }
        }
    }
}