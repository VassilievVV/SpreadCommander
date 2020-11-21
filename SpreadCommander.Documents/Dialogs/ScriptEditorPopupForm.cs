using Alsing.Windows.Forms.Document.DocumentStructure.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using SpreadCommander.Common;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using SpreadCommander.Common.Code;
using System.Threading.Tasks;
using Alsing.Windows.Forms.Controls.SyntaxBox;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class ScriptEditorPopupForm : BaseForm
    {
        private const int WM_UPDATESELECTION = WinAPI.WM_USER + 1;
    
        private SyntaxBoxControl _Editor;
        private readonly ScriptIntellisense _Intellisense;

        private readonly TextPoint _StartSearchPosition;
        private readonly int _LastPartStart;

        public ScriptEditorPopupForm(SyntaxBoxControl editor, ScriptIntellisense intellisense)
        {
            InitializeComponent();

            _Editor       = editor;
            _Intellisense = intellisense;

            if (_Intellisense.Help != null && (_Intellisense.Help.SupportsHelp || _Intellisense.Help.SupportsOnlineHelp))
            {
                if (_Intellisense.Help.SupportsHelp && _Intellisense.Help.SupportsOnlineHelp)
                    lblComments.Text = "Press<b>F1</b> for more help, <b>F2</b> for online help";
                else if (_Intellisense.Help.SupportsHelp)
                    lblComments.Text = "Press<b>F1</b> for more help";
                else if (_Intellisense.Help.SupportsOnlineHelp)
                    lblComments.Text = "Press <b>F2</b> for online help";
            }
            else
                lblComments.Visible = false;


            _StartSearchPosition = new TextPoint()
            {
                X = _Editor.Caret.Position.X, 
                Y = _Editor.Caret.Position.Y
            };

            _StartSearchPosition.X = FindStartSymbol(_Editor.Caret.CurrentRow.Text, Math.Max(_StartSearchPosition.X - 1, 0), 
                intellisense.UsePeriodInIntellisense, out _LastPartStart);

            _Intellisense.Items.Sort((x, y) =>
            {
                if (x.DisplayOrder != y.DisplayOrder)
                    return x.DisplayOrder.CompareTo(y.DisplayOrder);
                if (x.IsMandatory.CompareTo(y.IsMandatory) != 0)
                    return -x.IsMandatory.CompareTo(y.IsMandatory);
                if (x.Position.HasValue && !y.Position.HasValue)
                    return -1;
                if (!x.Position.HasValue && y.Position.HasValue)
                    return 1;
                if (x.Position.HasValue && y.Position.HasValue && x.Position.Value.CompareTo(y.Position.Value) != 0)
                    return x.Position.Value.CompareTo(y.Position.Value);
                return StringLogicalComparer.Compare(x.Caption, y.Caption);
            });
            Grid.DataSource = _Intellisense.Items;

            lblDescription.DataBindings.Add(nameof(lblDescription.Text), _Intellisense.Items, nameof(ScriptIntellisenseItem.Description));

            _Editor.CaretChange += new EventHandler(Editor_CaretChange);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

        private void FormDispose()
        {
            if (_Editor != null)
                _Editor.CaretChange -= new EventHandler(Editor_CaretChange);
            _Editor = null;
        }

        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case WM_UPDATESELECTION:
                    UpdateSelection();
                    break;
            }

            base.WndProc(ref msg);
        }
        
        private void PopupForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                case Keys.Tab:
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
                case Keys.F1:
                    if (!(_Intellisense.Help?.SupportsHelp ?? false))
                        return;

                    var item = SelectedItem;
                    if (item == null)
                        return;

                    string htmlContent = null;
                    var taskContent = Task.Run(() => htmlContent = _Intellisense.Help.GetHelpHtmlContent(item));

                    var commentForm = new RichTextViewer()
                    {
                        Location = _Editor.PointToScreen(new Point(_Editor.Right + 20, _Editor.Top)),
                        Height   = Math.Max(Convert.ToInt32(_Editor.Height), 500)
                    };

                    var screen = Screen.FromControl(_Editor);
                    if (commentForm.Bottom > screen.WorkingArea.Bottom)
                        commentForm.Top = Math.Max(screen.WorkingArea.Bottom - commentForm.Height, 10);
                    if (commentForm.Right > screen.WorkingArea.Right)
                        commentForm.Left = Math.Max(screen.WorkingArea.Right - commentForm.Width, 10);

                    commentForm.FormClosed += (s, e) =>
                    {
                        commentForm.Dispose();
                    };

                    commentForm.Show(_Editor);

                    taskContent.Wait();
                    commentForm.LoadHtmlText(htmlContent);

                    Close();

                    commentForm.Activate();
                    break;
                case Keys.F2:
                    if (!(_Intellisense.Help?.SupportsOnlineHelp ?? false))
                        return;

                    var selItem = SelectedItem;
                    if (selItem == null)
                        return;

                    _Intellisense.Help.ShowOnlineHelp(selItem);
                    break;
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    break;
                default:
                    if (_Editor != null && _Editor.ActiveViewControl != null)
                    {
                        typeof(Control).InvokeMember(nameof(OnKeyDown),
                            BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                            BindingFlags.Instance,
                            null, _Editor.ActiveViewControl, new object[] {e});
                    }
                    break;
            }
        }

        private void PopupForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_Editor != null && _Editor.ActiveViewControl != null)
            {
                typeof(Control).InvokeMember(nameof(OnKeyPress), 
                    BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                    BindingFlags.Instance,
                    null, _Editor.ActiveViewControl, new object[] {e});
            }
        }

        private void PopupForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                case Keys.Escape:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.F1:
                case Keys.F2:
                    break;
                default:
                    if (_Editor != null && _Editor.ActiveViewControl != null)
                        typeof(Control).InvokeMember(nameof(OnKeyUp), 
                            BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                            BindingFlags.Instance,
                            null, _Editor.ActiveViewControl, new object[] {e});
                    break;
            }
        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            ActiveControl = Grid;
            if (GridView.RowCount > 0)
                GridView.FocusedRowHandle = GridView.GetRowHandle(0);
            
            WinAPI.PostMessage(Handle, WM_UPDATESELECTION, IntPtr.Zero, IntPtr.Zero);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.Style |= WinAPI.WS_BORDER | WinAPI.WS_POPUP | WinAPI.WS_SYSMENU | WinAPI.WS_SIZEBOX;
                return param;
            }
        }

        private void PopupForm_Deactivate(object sender, EventArgs e)
        {
            if (!IsDisposed)
                Close();
        }

        public ScriptIntellisenseItem SelectedItem
        {
            get 
            {
                int handle = GridView.FocusedRowHandle;
                if (handle == GridControl.InvalidRowHandle)
                    return null;

                if (Grid.DataSource is List<ScriptIntellisenseItem> items && handle >= 0 && handle < items.Count)
                    return items[handle];

                return null;
            }
        }

        public static int FindStartSymbol(string str, int startPos, bool usePeriodInIntellisense,
            out int lastPosStart)
        {
            lastPosStart	= 0;
            if (string.IsNullOrEmpty(str))
                return 0;

            int p = Math.Min(startPos, str.Length - 1);
            while (p >= 0)
            {
                char c = str[p];
                if (char.IsLetterOrDigit(c) || c == '$' || c == '_' || c == '-' || (usePeriodInIntellisense && c == '.'))
                    p--;
                else 
                    break;
            }

            int quotedPos = p;

            while (quotedPos >= 0)
            {
                char c = str[quotedPos];
                bool stop = false;

                switch (c)
                {
                    case '[':
                    case '"':
                    case '`':
                        stop = true;
                        break;
                    case '\'':
                    case ']':
                        stop = true;
                        quotedPos = -1;
                        break;
                }

                if (stop)
                    break;

                quotedPos--;
            }

            if (quotedPos >= 0)
                p = quotedPos;

            if (lastPosStart <= 0)
                lastPosStart = p + 1;
            
            return Math.Max(p + 1, 0);
        }

        public TextPoint StartSearchPosition
        {
            get {return _StartSearchPosition;}
        }
        
        private void UpdateSelection()
        {
            TextPoint currentPos = _Editor.Caret.Position;
            if (currentPos.Y != _StartSearchPosition.Y)
                return;
                
            if (currentPos.X < _LastPartStart)
                return;

            string str = _Editor.Caret.CurrentRow.Text[_LastPartStart..currentPos.X];
            if (string.IsNullOrEmpty(str))
                return;

            if (str.StartsWith("-"))	//'-' is prefix of property in command
                str = str[1..];

            if (Grid.DataSource is List<ScriptIntellisenseItem>)
                GridView.ApplyFindFilter(str);
        }
        
        private void Editor_CaretChange(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
        }

        private void GridView_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}