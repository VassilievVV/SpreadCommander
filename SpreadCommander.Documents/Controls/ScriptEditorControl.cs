using Alsing.Windows.Forms.Document;
using Alsing.Windows.Forms.Document.DocumentStructure.Structs;
using Alsing.Windows.Forms.Document.DocumentStructure.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using SpreadCommander.Common;
using System.IO;
using System.Reflection;
using SpreadCommander.Documents.Dialogs.SyntaxEditorDialogs;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common.Code;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using DevExpress.XtraBars;
using Alsing.Windows.Forms.Classes;
using Alsing.Windows.Forms.Document.DocumentStructure.Row;
using Alsing.Windows.Forms.Controls.EditView;

namespace SpreadCommander.Documents.Controls
{
    public partial class ScriptEditorControl : DevExpress.XtraEditors.XtraUserControl, IScriptEditorHost
    {
        #region ListIntellisenseItemsEventArgs
        public class ListIntellisenseItemsEventArgs: EventArgs
        {
            public string FileName                  { get; set; }

            public string Text						{ get; set; }

            public string[] Lines					{ get; set; }

            public Point CaretPosition				{ get; set; }

            public ScriptIntellisense Intellisense	{ get; } = new ScriptIntellisense();
        }
        #endregion

        private const int WM_SHOWPOPUP = WinAPI.WM_USER + 1;

        public event EventHandler<ListIntellisenseItemsEventArgs> ListIntellisenseItems;

        public EventHandler ModifiedChanged;

        public Guid EditorUniqueID { get; set; } = Guid.NewGuid();

        public ScriptEditorControl()
        {
            InitializeComponent();

            syntaxEditor.GutterIcons.Images[1] = Properties.Resources.Bookmark_16x16;
            syntaxEditor.GutterMarginColor = this.BackColor;

            InitSyntaxEditor();
        }

        public SyntaxDocument SyntaxDocument => syntaxDocument;
        public string FileName { get; set; }

        public event EventHandler ScriptChanged;

        ScriptEditorControl IScriptEditorHost.ScriptEditor => this;

        public string CommandText
        {
            get
            {
                var result = syntaxEditor.Selection.Text;
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                return syntaxDocument.Text;
            }
        }

        public string ScriptText
        {
            get => syntaxDocument.Text;
            set => syntaxDocument.Text = value;
        }

        public bool ShowGutterMargin
        {
            get => syntaxEditor.ShowGutterMargin;
            set => syntaxEditor.ShowGutterMargin = value;
        }

        public bool ShowLineNumbers
        {
            get => syntaxEditor.ShowLineNumbers;
            set => syntaxEditor.ShowLineNumbers = value;
        }

        [System.Diagnostics.DebuggerStepThrough()]
        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case WM_SHOWPOPUP:
                    ShowDropDown();
                    break;
            }

            base.WndProc(ref msg);
        }

        protected virtual ScriptIntellisense OnListIntellisenseItems()
        {
            var args = new ListIntellisenseItemsEventArgs()
            {
                Text          = syntaxDocument.Text,
                Lines         = syntaxDocument.Lines,
                CaretPosition = new Point(syntaxEditor.Caret.Position.X, syntaxEditor.Caret.Position.Y)
            };
            ListIntellisenseItems?.Invoke(this, args);
            return args.Intellisense;
        }

        private void InitSyntaxEditor()
        {
            KeyboardAction action;

            syntaxEditor.RemoveKeyboardAction(Keys.G, false, true, false);
            syntaxEditor.RemoveKeyboardAction(Keys.F, false, true, false);
            syntaxEditor.RemoveKeyboardAction(Keys.H, false, true, false);
            syntaxEditor.RemoveKeyboardAction(Keys.O, false, true, false);
            syntaxEditor.RemoveKeyboardAction(Keys.S, false, true, false);

            action = new KeyboardAction(Keys.Space, false, true, false, false, new ActionDelegate(ShowDropDown));
            syntaxEditor.KeyboardActions.Add(action);

            action = new KeyboardAction(Keys.G, false, true, false, true, new ActionDelegate(ShowGotoLineDialog));
            syntaxEditor.KeyboardActions.Add(action);

            action = new KeyboardAction(Keys.F, false, true, false, true, new ActionDelegate(ShowFindDialog));
            syntaxEditor.KeyboardActions.Add(action);

            action = new KeyboardAction(Keys.H, false, true, false, true, new ActionDelegate(ShowReplaceDialog));
            syntaxEditor.KeyboardActions.Add(action);

            action = new KeyboardAction(Keys.O, false, true, false, true, new ActionDelegate(OpenDocument));
            syntaxEditor.KeyboardActions.Add(action);

            action = new KeyboardAction(Keys.S, false, true, false, true, new ActionDelegate(SaveDocument));
            syntaxEditor.KeyboardActions.Add(action);
        }

        public void LoadSyntax(string syntaxName)
        {
            syntaxEditor.SetParserSyntax(syntaxName);
        }

        public bool ReadOnly
        {
            get => syntaxEditor.ReadOnly;
            set { syntaxEditor.ReadOnly = value; barStatus.Visible = !value; }
        }

        public string DefaultExt
        {
            get => dlgOpen.DefaultExt;
            set
            {
                dlgOpen.DefaultExt = value;
                dlgSave.DefaultExt = value;
            }
        }

        public string FileFilter
        {
            get => dlgOpen.Filter;
            set
            {
                dlgOpen.Filter = $"{value}|All files (*.*)|*.*";
                dlgSave.Filter = $"{value}|All files (*.*)|*.*";
            }
        }

        public void Undo()           => syntaxDocument.Undo();
        public void Redo()           => syntaxDocument.Redo();
        public void Paste()          => syntaxEditor.Paste();
        public void Copy()           => syntaxEditor.Copy();
        public void Cut()            => syntaxEditor.Cut();
        public void BookmarkToggle() => syntaxEditor.ToggleBookmark();
        public void BookmarkPrev()   => syntaxEditor.GotoPreviousBookmark();
        public void BookmarkNext()   => syntaxEditor.GotoNextBookmark();
        public void BookmarksClear()
        {
            foreach (Row row in syntaxDocument)
                row.Bookmarked = false;
            syntaxEditor.Invalidate();
        }

        public void RemoveKeyboardAction(Keys key, bool shift, bool control, bool alt) =>
            syntaxEditor.RemoveKeyboardAction(key, shift, control, alt);

        public void AddKeyboardAction(KeyboardAction action)                         => syntaxEditor.KeyboardActions.Add(action);
        private void ShowGotoLineDialog(object sender, KeyboardActionEventArgs args) => ShowGotoLineDialog();
        private void ShowFindDialog(object sender, KeyboardActionEventArgs args)     => ShowFindDialog(false);
        private void ShowReplaceDialog(object sender, KeyboardActionEventArgs args)  => ShowFindDialog(true);

        private void ShowDropDown(object sender, KeyboardActionEventArgs args)
        {
            args.Cancel = true;

            WinAPI.PostMessage(Handle, WM_SHOWPOPUP, IntPtr.Zero, IntPtr.Zero);
        }

        public void ShowDropDown()
        {
            var intellisense = OnListIntellisenseItems();
            ShowEditorDropDown(syntaxEditor, intellisense);
        }

        internal static void ShowEditorDropDown(SyntaxEditorControl Editor, ScriptIntellisense intellisense)
        {
            if ((intellisense?.Items?.Count ?? 0) <= 0)
                return;

            SyntaxDocument Document = Editor.Document;
            if (Document == null)
                return;

            TextPoint startPos = Editor.Caret.Position;
            if (startPos.Y < 0 || startPos.Y >= Document.Count)
                return;

            if (Editor.ActiveViewControl is not EditViewControl ctrlEdit || ctrlEdit.Painter == null)
                return;

            Point popupLocation = Editor.PointToScreen(ctrlEdit.Painter.GetCaretPixelPos());
            popupLocation.X += ctrlEdit.View.CharWidth;
            popupLocation.Y += ctrlEdit.View.RowHeight;

            var popup = new ScriptEditorPopupForm(Editor, intellisense)
            {
                Location = popupLocation
            };
            popup.Show(Editor);

            popup.FormClosed += (s, e) =>
            {
                if (!popup.IsDisposed && popup.DialogResult == DialogResult.OK && popup.SelectedItem != null)
                {
                    popup.Dispose();

                    startPos     = popup.StartSearchPosition;
                    var CaretPos = Editor.Caret.Position;

                    if (startPos.X > 0 && CaretPos.Y >= 0)
                    {
                        var rPrevChar = new TextRange()
                        {
                            FirstRow    = CaretPos.Y,
                            LastRow     = CaretPos.Y,
                            FirstColumn = startPos.X - 1,
                            LastColumn  = startPos.X
                        };
                        string prevChar = Document.GetRange(rPrevChar);
                        if (prevChar == "[")
                            startPos.X--;
                    }

                    var tr = new TextRange()
                    {
                        FirstRow    = CaretPos.Y,
                        LastRow     = CaretPos.Y,
                        FirstColumn = startPos.X,
                        LastColumn  = CaretPos.X
                    };

                    Document.DeleteRange(tr, true);
                    Editor.Caret.Position.X = startPos.X;
                    CaretPos = Editor.Caret.Position;

                    string caption = popup.SelectedItem.Value ?? popup.SelectedItem.Caption;

                    var NewCaretPos = Document.InsertText(caption,
                        CaretPos.X, CaretPos.Y, true);
                    ctrlEdit.Caret.SetPos(NewCaretPos);
                    ctrlEdit.Focus();

                    if (Editor.Parent is ContainerControl containerControl)
                        containerControl.ActiveControl = ctrlEdit;
                }

                popup.Dispose();
            };
        }

        public void ShowGotoLineDialog()
        {
            if (syntaxEditor.ActiveViewControl is not EditViewControl ctrlEdit)
                return;

            using var dlg = new EditorGotoLineForm(ctrlEdit);
            dlg.ShowDialog(this);
        }

        public void ShowFindDialog(bool replace)
        {
            if (syntaxEditor.ActiveViewControl == null)
                return;

            Form topForm = FindForm();
            if (topForm == null)
                return;

            string strFind = null;
            EditViewControl edit = syntaxEditor.ActiveViewControl as EditViewControl;
            if (edit != null)
            {
                if (edit.Selection != null)
                    strFind = edit.Selection.Text;
                if (string.IsNullOrEmpty(strFind) && edit.Caret != null && edit.Caret.CurrentWord != null)
                    strFind = edit.Caret.CurrentWord.Text;
            }

            var EditorTopLeft = syntaxEditor.PointToScreen(new Point(0, 0));
            var dlg           = new EditorFindReplaceForm(edit, strFind, replace);
            dlg.Left          = topForm.Left + EditorTopLeft.X + Math.Max((syntaxEditor.ClientWidth - dlg.Width) / 2, 0);
            dlg.Top           = topForm.Top + EditorTopLeft.Y + Math.Max((syntaxEditor.ClientHeight - dlg.Height) / 2, 0);

            dlg.FormClosed += (s, args) => dlg.Dispose();

            dlg.Show(this);
        }

        public void FindNext(string pattern, bool matchCase, bool wholeWords, bool useRegEx)
        {
            syntaxEditor.FindNext(pattern, matchCase, wholeWords, useRegEx);
        }

        public void PrintScript()
        {
            Printing.PrintRtfText(this, syntaxEditor.ExportToRTF(false));
        }

        public void PrintScriptQuick()
        {
            Printing.ExecutePrintCommandForRtf(syntaxEditor.ExportToRTF(false),
                PrintingSystemCommand.PrintDirect);
        }

        public void PrintScriptSelection()
        {
            Printing.PrintRtfText(this, syntaxEditor.ExportToRTF(true));
        }

        public void PrintScriptSelectionQuick()
        {
            Printing.ExecutePrintCommandForRtf(syntaxEditor.ExportToRTF(true),
                PrintingSystemCommand.PrintDirect);
        }

        public void NavigateToLine(int line)
        {
            try
            {
                syntaxEditor.GotoLine(line);
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        public bool IsModified
        {
            get => syntaxDocument.Modified;
            set => syntaxDocument.Modified = value;
        }

        public void NewDocument()
        {
            if (syntaxDocument.Modified && (dlgSave.ShowDialog(ParentForm) == DialogResult.OK))
                syntaxEditor.Save(dlgSave.FileName);

            syntaxDocument.Clear();
            syntaxDocument.Modified = false;
        }

        private void OpenDocument(object sender, KeyboardActionEventArgs args) => OpenDocument();
        public void OpenDocument()
        {
            if (string.IsNullOrWhiteSpace(dlgOpen.InitialDirectory))
                dlgOpen.InitialDirectory = Project.Current.ProjectPath;

            if (dlgOpen.ShowDialog(ParentForm) != DialogResult.OK)
                return;

            OpenDocument(dlgOpen.FileName);
        }

        public void OpenDocument(string fileName)
        {
            NewDocument();
            syntaxEditor.Open(fileName);
            syntaxDocument.Modified = false;
            FileName = fileName;
        }

        private void SaveDocument(object sender, KeyboardActionEventArgs args) => SaveDocument();
        public void SaveDocument()
        {
            var fileName = FileName;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                if (dlgSave.ShowDialog(ParentForm) != DialogResult.OK)
                    return;

                fileName = dlgSave.FileName;
            }

            SaveDocument(fileName);
        }

        public void SaveDocument(string fileName)
        {
            syntaxEditor.Save(fileName);
            syntaxDocument.Modified = false;
            FileName = fileName;
        }

        public void LoadFromFile(string fileName, bool chooseSyntax = true)
        {
            var ext = Path.GetExtension(fileName)?.ToLower();

            if (chooseSyntax)
            {
                string syntax = null;
                switch (ext)
                {
                    case ".ps1":
                    case ".psm1":
                    case ".psd1":
                    case ".psx":
                        syntax = "PowerShell";
                        break;
                    case ".csx":
                    case ".cs":
                        syntax = "C#";
                        break;
                    case ".fsx":
                    case ".fs":
                        syntax = "F#";
                        break;
                    case ".sql":
                        syntax = "SqlScript";
                        break;
                    case ".r":
                        syntax = "R";
                        break;
                    case ".py":
                        syntax = "Python";
                        break;
                }

                LoadSyntax(syntax);
            }

            syntaxEditor.Open(fileName);
            syntaxDocument.Modified = false;
            FileName = fileName;
        }

        public void SaveToFile(string fileName)
        {
            syntaxEditor.Save(fileName);
            syntaxDocument.Modified = false;
        }

        private void SyntaxDocument_Change(object sender, EventArgs e)
        {
            ScriptChanged?.Invoke(this, e);
        }

        private void SyntaxDocument_ModifiedChanged(object sender, EventArgs e)
        {
            ModifiedChanged?.Invoke(this, e);
        }

        private void SyntaxDocument_RowParsed(object sender, RowEventArgs e)
        {
            var rowIndex = e.Row.Index + 1;

            var rowErrors = _ParseErrors.ContainsKey(rowIndex) ? _ParseErrors[rowIndex] : null;
            if (rowErrors != null)
            {
                foreach (var word in e.Row.FormattedWords)
                {
                    var wordErrors = GetWordErrors(word);
                    if (!string.IsNullOrWhiteSpace(wordErrors))
                    {
                        word.HasError = true;
                        word.InfoTip  = wordErrors;
                    }
                }
            }


            string GetWordErrors(Word word)
            {
                var result = new StringBuilder();

                foreach (var error in rowErrors)
                {
                    if (((error.StartLineNumber < rowIndex) ||
                        (error.StartLineNumber == rowIndex && error.StartColumnNumber <= word.Index + (word.Text?.Length ?? 0) + 1)) &&
                        ((error.EndLineNumber > rowIndex) ||
                        (error.EndLineNumber == rowIndex && error.EndColumnNumber >= word.Index + 1)))
                    {
                        if (result.Length > 0) result.AppendLine();
                        result.Append(error.Message);
                    }
                }

                return result.Length > 0 ? result.ToString() : null;
            }
        }

        private readonly Dictionary<int, List<ScriptParseError>> _ParseErrors = new ();

        public void UpdateParseErrors(List<ScriptParseError> errors)
        {
            _ParseErrors.Clear();

            foreach (var error in errors)
            {
                for (int i = error.StartLineNumber; i <= Math.Max(error.StartLineNumber, error.EndLineNumber); i++)
                {
                    if (!_ParseErrors.ContainsKey(i))
                        _ParseErrors.Add(i, new List<ScriptParseError>());

                    var rowErrors = _ParseErrors[i];
                    rowErrors.Add(error);

                    //Highlight error only in first 100 lines. This should be enough for any error.
                    if (i > error.StartLineNumber + 100)
                        break;
                }
            }

            foreach (var row in syntaxDocument.VisibleRows)
                syntaxDocument.ParseRow(row, true);
            syntaxEditor.Refresh();
        }

        public void ShowParseError(ScriptParseError error)
        {
            if (error == null)
                return;

            syntaxEditor.ClearSelection();
            syntaxEditor.Selection.Bounds = new TextRange(
                Math.Max(error.StartColumnNumber - 1, 0), Math.Max(error.StartLineNumber - 1, 0),
                Math.Max(error.EndColumnNumber - 1, 0), Math.Max(error.EndLineNumber - 1, 0));

            syntaxEditor.Caret.Position = new TextPoint(
                Math.Max(error.StartColumnNumber - 1, 0), Math.Max(error.StartLineNumber - 1, 0));

            syntaxEditor.Focus();
        }

        private void PopupMenu_BeforePopup(object sender, CancelEventArgs e)
        {
            barCut.Enabled            = syntaxEditor.CanCopy;
            barCopy.Enabled           = syntaxEditor.CanCopy;
            barPaste.Enabled          = syntaxEditor.CanPaste;
            barToggleBookmark.Enabled = true;
            barUndo.Enabled           = syntaxEditor.CanUndo;
            barRedo.Enabled           = syntaxEditor.CanRedo;
        }

        private void BarCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (syntaxEditor.CanCopy)
                syntaxEditor.Cut();
        }

        private void BarCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (syntaxEditor.CanCopy)
                syntaxEditor.Copy();
        }

        private void BarPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (syntaxEditor.CanPaste)
                syntaxEditor.Paste();
        }

        private void BarToggleBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            syntaxEditor.ToggleBookmark();
        }

        private void BarUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (syntaxEditor.CanUndo)
                syntaxEditor.Undo();
        }

        private void BarRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (syntaxEditor.CanRedo)
                syntaxEditor.Redo();
        }

        private void SyntaxEditor_CaretChange(object sender, EventArgs e)
        {
            var cursor = syntaxEditor.Caret.Position;
            barCursor.Caption = $"{cursor.Y+1}:{cursor.X+1}";
        }

        private void SyntaxEditor_SelectionChange(object sender, EventArgs e)
        {
            var selection = syntaxEditor.Selection.Bounds;
            barSelection.Caption = $"{selection.FirstRow+1}:{selection.FirstColumn+1} - {selection.LastRow+1}:{selection.LastColumn+1}";
        }
    }
}
