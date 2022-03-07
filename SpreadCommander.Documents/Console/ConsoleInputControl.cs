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
using SpreadCommander.Common.ScriptEngines;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.API.Native;
using System.IO;
using System.Reflection;
using SpreadCommander.Common;
using DevExpress.LookAndFeel;
using DevExpress.XtraRichEdit;
using Alsing.Windows.Forms.SyntaxBox;
using DevExpress.XtraLayout.Utils;
using SpreadCommander.Documents.Controls;
using SpreadCommander.Common.Code;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using SpreadCommander.Common.SqlScript;
using SpreadCommander.Documents.Code;
using Alsing.Windows.Forms.Document;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleInputControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region CommandArgs
        public class CommandArgs: EventArgs
        {
            public string CommandText { get; set; }
        }
        #endregion

        #region HistoryItem
        public class HistoryItem
        {
            public string Text { get; set; }
            public string RtfText { get; set; }
            public DateTime TimeStamp { get; set; } = DateTime.Now;
        }
        #endregion

        #region ShowParseErrorArgs
        public class ShowParseErrorArgs: EventArgs
        {
            public ScriptParseError Error { get; set; }
        }
        #endregion

        public event EventHandler<CommandArgs> ExecuteCommand;
        public event EventHandler<ScriptEditorControl.ListIntellisenseItemsEventArgs> ListIntellisenseItems;
        public event EventHandler<ShowParseErrorArgs> ShowParseError;

        public ConsoleInputControl()
        {
            InitializeComponent();
            Disposed += ConsoleInputControl_Disposed;
        }

        public bool MessagesVisible
        {
            get => layoutControlGroupMessages.Visible;
            set
            {
                layoutControlGroupMessages.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;
                if (value)
                    layoutControlGroupMessages.Selected = true;
                else if (ParseErrorsVisible)
                    layoutControlGroupErrors.Selected = true;
                else 
                    layoutControlGroupCommand.Selected = true;
            }
        }

        public bool ParseErrorsVisible
        {
            get => layoutControlGroupErrors.Visible;
            set
            {
                layoutControlGroupErrors.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;
                if (!value)
                {
                    if (MessagesVisible)
                        layoutControlGroupMessages.Selected = true;
                    else
                        layoutControlGroupCommand.Selected = true;
                }
            }
        }

        private void ConsoleInputControl_Disposed(object sender, EventArgs e)
        {
            if (!popupCommandHistory.IsDisposed && popupCommandHistory.Parent == null)
                popupCommandHistory.Dispose();
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            var command = syntaxDocument.Text;
            ExecuteCommand?.Invoke(this, new CommandArgs() { CommandText = command });
        }

        public void LoadSyntax(string syntaxName)
        {
            if (string.IsNullOrWhiteSpace(syntaxName))
                return;

            editCommand.SetParserSyntax(syntaxName);
            editHistory.SetParserSyntax(syntaxName);
        }

        public void AddHistoryItem(string history)
        {
            syntaxDocumentHistory.Text = history;
            var rtf = syntaxDocumentHistory.ExportToRTF(null, editHistory.FontName);

            var itemHistory = new HistoryItem()
            {
                Text    = history,
                RtfText = rtf
            };
            var index = bindingHistory.Add(itemHistory);
            bindingHistory.Position = index;
        }

        private void BindingHistory_CurrentChanged(object sender, EventArgs e)
        {
            syntaxDocumentHistory.Text = string.Empty;

            if (bindingHistory.Current is HistoryItem historyItem)
                syntaxDocumentHistory.Text = historyItem.Text;
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            popupCommandHistory.Width  = this.Width;
            popupCommandHistory.Height = this.Height * 2;

            if (bindingHistory.Count > 0)
                bindingHistory.Position = bindingHistory.Count - 1;

            var p = new Point()
            {
                X = Math.Max(btnHistory.Right - popupCommandHistory.Width, 0),
                Y = Math.Max(btnHistory.Top - popupCommandHistory.Height, 0)
            };
            popupCommandHistory.ShowPopup(this.PointToScreen(p));
            viewHistory.Focus();
        }

        private void ViewHistory_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (viewHistory.GetRow(e.RowHandle) is HistoryItem historyItem)
                syntaxDocument.Text = historyItem.Text;
            popupCommandHistory.HidePopup();
        }

        private void ViewHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Alt && !e.Control && !e.Shift)
            {
                if (viewHistory.GetFocusedRow() is HistoryItem historyItem)
                    syntaxDocument.Text = historyItem.Text;

                popupCommandHistory.HidePopup();
            }
        }

        protected virtual ScriptIntellisense OnListIntellisenseItems()
        {
            var args = new ScriptEditorControl.ListIntellisenseItemsEventArgs()
            {
                Text           = syntaxDocument.Text,
                Lines          = syntaxDocument.Lines,
                CaretPosition = new Point(editCommand.Caret.Position.X, editCommand.Caret.Position.Y)
            };
            ListIntellisenseItems?.Invoke(this, args);
            return args.Intellisense;
        }

        public void ShowDropDown()
        {
            var items = OnListIntellisenseItems();
            ScriptEditorControl.ShowEditorDropDown(editCommand, items);
        }

        public void UpdateParseErrors(List<ScriptParseError> errors)
        {
            using (new UsingProcessor(() => gridErrors.BeginUpdate(), () => gridErrors.EndUpdate()))
            {
                bindingErrors.Clear();

                foreach (var error in errors)
                    bindingErrors.Add(error);
            }
        }

        public void ClearSqlMessages()
        {
            bindingSqlMessages.Clear();
        }

        public void AddSqlMessage(SqlMessage message)
        {
            bindingSqlMessages.Add(message);
        }

        private void ViewErrors_DoubleClick(object sender, EventArgs e)
        {
            if (e is not DXMouseEventArgs ea || sender is not GridView view)
                return;

            var info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                if (view.GetRow(info.RowHandle) is ScriptParseError error)
                    ShowParseError?.Invoke(this, new ShowParseErrorArgs() { Error = error });
            }
        }

        private void PopupMenu_BeforePopup(object sender, CancelEventArgs e)
        {
            barCut.Enabled            = editCommand.CanCopy;
            barCopy.Enabled           = editCommand.CanCopy;
            barPaste.Enabled          = editCommand.CanPaste;
            barToggleBookmark.Enabled = true;
            barUndo.Enabled           = editCommand.CanUndo;
            barRedo.Enabled           = editCommand.CanRedo;
        }

        private void BarCut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (editCommand.CanCopy)
                editCommand.Cut();
        }

        private void BarCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (editCommand.CanCopy)
                editCommand.Copy();
        }

        private void BarPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (editCommand.CanPaste)
                editCommand.Paste();
        }

        private void BarToggleBookmark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editCommand.ToggleBookmark();
        }

        private void BarUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (editCommand.CanUndo)
                editCommand.Undo();
        }

        private void BarRedo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (editCommand.CanRedo)
                editCommand.Redo();
        }

        private void TabbedControlGroupControls_SelectedPageChanging(object sender, DevExpress.XtraLayout.LayoutTabPageChangingEventArgs e)
        {
            transitionManager.StartTransition(this);
        }

        private void TabbedControlGroupControls_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            transitionManager.EndTransition();
        }

        private static void CustomColumnSort(DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            if (e.Handled)
                return;

            if (e.Value1 is string && e.Value2 is string)
            {
                e.Result  = StringLogicalComparer.Compare(Convert.ToString(e.Value1), Convert.ToString(e.Value2));
                e.Handled = true;
            }
        }

        private void ViewMessages_CustomColumnGroup(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomColumnSort(e);
        }

        private void ViewMessages_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomColumnSort(e);
        }

        private void ViewErrors_CustomColumnGroup(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomColumnSort(e);
        }

        private void ViewErrors_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomColumnSort(e);
        }

        private void ViewHistory_CustomColumnGroup(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomColumnSort(e);
        }

        private void ViewHistory_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            CustomColumnSort(e);
        }
    }
}
