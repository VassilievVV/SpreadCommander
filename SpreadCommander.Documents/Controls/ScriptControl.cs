using Alsing.Windows.Forms.Document;
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
using System.IO;
using DevExpress.XtraBars.Docking2010.Views;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Controls
{
    public partial class ScriptControl : DevExpress.XtraEditors.XtraUserControl, IScriptEditorHost
    {
        private bool _ReadOnly;
        private int _DocumentCounter;
        private string _SyntaxName;
        private string _DefaultExt;
        private string _FileFilter;

        public event EventHandler<ScriptEditorControl.ListIntellisenseItemsEventArgs> ListIntellisenseItems;
        public event EventHandler ScriptChanged;

        public ScriptControl()
        {
            InitializeComponent();
        }

        private void ScriptControl_Load(object sender, EventArgs e)
        {
            if (viewScripts.Documents.Count <= 0)
                NewDocument();
        }

        private void ViewScripts_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            var editor = new ScriptEditorControl();
            if (!string.IsNullOrWhiteSpace(_SyntaxName))
                editor.LoadSyntax(_SyntaxName);

            e.Control = editor;
        }

        public ScriptEditorControl ActiveEditor => viewScripts.ActiveDocument?.Control as ScriptEditorControl;
        
        ScriptEditorControl IScriptEditorHost.ScriptEditor => ActiveEditor;

        public SyntaxDocument SyntaxDocument => ActiveEditor?.SyntaxDocument;

        public string CommandText => ActiveEditor?.CommandText;

        public string ScriptText => ActiveEditor?.ScriptText;

        public string ActiveFileName
        {
            get => ActiveEditor?.FileName;
            set
            {
                var editor = ActiveEditor;
                if (editor != null)
                    editor.FileName = value;
            }
        }

        public void LoadSyntax(string syntaxName)
        {
            _SyntaxName = syntaxName;

            foreach (var doc in viewScripts.Documents)
                if (doc.Control is ScriptEditorControl editor)
                    editor.LoadSyntax(syntaxName);
        }

        public string DefaultExt
        {
            get => _DefaultExt;
            set
            {
                _DefaultExt = value;
                dlgOpen.DefaultExt = value;
                dlgSave.DefaultExt = value;

                foreach (var doc in viewScripts.Documents)
                    if (doc.Control is ScriptEditorControl editor)
                        editor.DefaultExt = _DefaultExt;
            }
        }

        public string FileFilter
        {
            get => _FileFilter;
            set
            {
                _FileFilter = value;
                dlgOpen.Filter = $"{value}|All files (*.*)|*.*";
                dlgSave.Filter = $"{value}|All files (*.*)|*.*";

                foreach (var doc in viewScripts.Documents)
                    if (doc.Control is ScriptEditorControl editor)
                        editor.FileFilter = _FileFilter;
            }
        }

        public bool ReadOnly
        {
            get => _ReadOnly;
            set
            {
                _ReadOnly = value;

                foreach (var doc in viewScripts.Documents)
                    if (doc.Control is ScriptEditorControl editor)
                        editor.ReadOnly = value;
            }
        }

        public void Undo()                       => ActiveEditor?.Undo();
        public void Redo()                       => ActiveEditor?.Redo();
        public void Paste()                      => ActiveEditor?.Paste();
        public void Copy()                       => ActiveEditor?.Copy();
        public void Cut()                        => ActiveEditor?.Cut();
        public void BookmarkToggle()             => ActiveEditor?.BookmarkToggle();
        public void BookmarkPrev()               => ActiveEditor?.BookmarkPrev();
        public void BookmarkNext()               => ActiveEditor?.BookmarkNext();
        public void BookmarksClear()             => ActiveEditor?.BookmarksClear();
        public void ShowFindDialog(bool replace) => ActiveEditor?.ShowFindDialog(replace);
        public void ShowGotoLineDialog()         => ActiveEditor?.ShowGotoLineDialog();

        public void PrintScript()                => ActiveEditor?.PrintScript();
        public void PrintScriptQuick()           => ActiveEditor?.PrintScriptQuick();
        public void PrintScriptSelection()       => ActiveEditor?.PrintScriptSelection();
        public void PrintScriptSelectionQuick()  => ActiveEditor?.PrintScriptSelectionQuick();

        public void NavigateToLine(int line)     => ActiveEditor?.NavigateToLine(line);

        public BaseDocument NewDocument()
        {
            var ctrl = new ScriptEditorControl()
            {
                DefaultExt = this.DefaultExt,
                FileFilter = this.FileFilter
            };
            ctrl.LoadSyntax(_SyntaxName);
            ctrl.ListIntellisenseItems += Editor_ListIntellisenseItems;
            ctrl.ScriptChanged         += Editor_ScriptChanged;

            var doc     = viewScripts.AddDocument(ctrl);
            doc.Caption = $"Document {++_DocumentCounter}";

            viewScripts.ActivateDocument(ctrl);

            return doc;
        }

        private void Editor_ListIntellisenseItems(object sender, ScriptEditorControl.ListIntellisenseItemsEventArgs e)
        {
            ListIntellisenseItems?.Invoke(this, e);
        }

        private void Editor_ScriptChanged(object sender, EventArgs e)
        {
            ScriptChanged?.Invoke(this, e);
        }

        public void OpenDocument()
        {
            if (dlgOpen.ShowDialog(ParentForm) != DialogResult.OK)
                return;

            var doc    = NewDocument();
            var editor = (ScriptEditorControl)doc.Control;
            editor.LoadFromFile(dlgOpen.FileName, true);
        }

        public void SaveDocument() => ActiveEditor?.SaveDocument();

        public void SaveAllDocuments()
        {
            foreach (var doc in viewScripts.Documents)
                if (doc.Control is ScriptEditorControl editor)
                    editor.SaveDocument();
        }

        private ScriptEditorControl FindEditor(Guid tag)
        {
            foreach (var document in viewScripts.Documents)
            {
                if (document.Control is ScriptEditorControl editor &&
                    editor.EditorUniqueID == tag)
                    return editor;
            }

            return null;
        }

        public void LoadFromFile(string fileName, bool chooseSyntax = true, bool newEditor = true)
        {
            var editor = (newEditor ? NewDocument().Control : ActiveEditor) as ScriptEditorControl;
            editor.LoadFromFile(fileName, chooseSyntax);
        }

        public void SaveToFile(string fileName, object tag)
        {
            if (!(tag is Guid gTag))
                throw new ArgumentException("Invalid document identifier", nameof(tag));

            var editor = FindEditor(gTag);
            if (editor == null)
                throw new Exception("Cannot find editor");

            editor.SaveToFile(fileName);
        }

        public IList<ModifiedFileItem> ListFiles()
        {
            var result = new List<ModifiedFileItem>();

            foreach (var document in viewScripts.Documents)
            {
                if (document.Control is ScriptEditorControl editor)
                    result.Add(new ModifiedFileItem() { FileName = editor.FileName, Title = document.Caption, Tag = editor.EditorUniqueID });
            }

            return result;
        }

        public IList<ModifiedFileItem> ListModifiedFiles()
        {
            var result = new List<ModifiedFileItem>();

            foreach (var document in viewScripts.Documents)
            {
                if (document.Control is ScriptEditorControl editor && editor.IsModified)
                    result.Add(new ModifiedFileItem() { FileName = editor.FileName, Title = document.Caption, Tag = editor.EditorUniqueID });
            }

            return result;
        }

        public void UpdateParseErrors(List<ScriptParseError> errors)
        {
            ActiveEditor?.UpdateParseErrors(errors);
        }

        public void ShowParseError(ScriptParseError error)
        {
            ActiveEditor?.ShowParseError(error);
        }
    }
}
