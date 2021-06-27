using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraEditors.Controls;
using SpreadCommander.Common;
using System.Text.RegularExpressions;
using SpreadCommander.Common.Code;
using Alsing.Windows.Forms.Controls.EditView;

namespace SpreadCommander.Documents.Dialogs.SyntaxEditorDialogs
{
	public partial class EditorFindReplaceForm: XtraForm
	{
		private static readonly List<string> _FindItems		= new ();
		private static readonly List<string> _ReplaceItems	= new ();

		private readonly EditViewControl	_Owner;
		private string						_Last;

		public EditorFindReplaceForm(EditViewControl owner, string strFind, bool replace)
		{
			InitializeComponent();

			_Owner = owner;
			ActiveControl = comboFindWhat;

			if (replace)
			{
				Text = "Replace";
			}
			else
			{
				Text                                    = "Find";
				layoutReplaceWith.Visibility		    = LayoutVisibility.Never;
				layoutButtonReplace.Visibility		    = LayoutVisibility.Never;
				layoutButtonReplaceAll.Visibility	    = LayoutVisibility.Never;
				layoutButtonReplaceSelection.Visibility = LayoutVisibility.Never;
			}

			comboFindWhat.Properties.Items.AddRange(_FindItems);
			comboFindWhat.Text = Utils.NonNullString(strFind);
			comboReplaceWith.Properties.Items.AddRange(_ReplaceItems);
		}

		private void EditorFindReplaceForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			FillStorage(comboFindWhat, _FindItems);
			FillStorage(comboReplaceWith, _ReplaceItems);
		}

		private static void AddItemToCombobox(ComboBoxEdit combo, string item)
		{
			if (string.IsNullOrEmpty(item))
				return;

			ComboBoxItemCollection items = combo.Properties.Items;

			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] as string == item)
				{
					items.RemoveAt(i);
					items.Insert(0, item);
					return;
				}
			}

			items.Add(item);
		}

		private static void FillStorage(ComboBoxEdit combo, List<string> list)
		{
			ComboBoxItemCollection items = combo.Properties.Items;

			list.Clear();
			for (int i = 0; i < items.Count; i++)
			{
				string value = items[i] as string;
				if (!string.IsNullOrEmpty(value))
					list.Add(value);
			}
		}

		private bool FindNext(bool silent)
		{
			string text = Utils.TrimLineBreaks(comboFindWhat.Text);

			if (!silent && !string.IsNullOrEmpty(_Last) && _Last != text)
			{
				_Owner.Caret.Position.X = 0;
				_Owner.Caret.Position.Y = 0;
				_Owner.ScrollIntoView();
			}

			_Last = text;

			if (string.IsNullOrEmpty(text))
				return false;

			if (!silent)
				AddItemToCombobox(comboFindWhat, text);

			bool result = _Owner.SelectNext(comboFindWhat.Text, listOptions.Items[0].CheckState == CheckState.Checked,
				listOptions.Items[1].CheckState == CheckState.Checked, 
				//listOptions.Items[2].CheckState == CheckState.Checked
				false);
			return result;
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnFindNext_Click(object sender, EventArgs e)
		{
			FindNext(false);
			comboFindWhat.Focus();
		}

		private void BtnReplace_Click(object sender, EventArgs e)
		{
			string replace = comboReplaceWith.Text;

			if (FindNext(false))
				_Owner.ReplaceSelection(replace);
		}

		private void BtnReplaceAll_Click(object sender, EventArgs e)
		{
			string replace = Utils.TrimLineBreaks(comboReplaceWith.Text);

			AddItemToCombobox(comboFindWhat, comboFindWhat.Text);
			AddItemToCombobox(comboReplaceWith, replace);

			while (FindNext(true))
				_Owner.ReplaceSelection(replace);

			_Owner.Selection.ClearSelection();
			comboFindWhat.Focus();
		}

		private void BtnMarkAll_Click(object sender, EventArgs e)
		{
			AddItemToCombobox(comboFindWhat, Utils.TrimLineBreaks(comboFindWhat.Text));

			while (FindNext(true))
			{
				if (_Owner.Caret != null && _Owner.Caret.CurrentRow != null)
					_Owner.Caret.CurrentRow.Bookmarked = true;
			}

			_Owner.Selection.ClearSelection();
			comboFindWhat.Focus();
		}

		private void BtnReplaceSelection_Click(object sender, EventArgs e)
		{
			string findWhat = Utils.TrimLineBreaks(comboFindWhat.Text);
			string replace  = Utils.TrimLineBreaks(comboReplaceWith.Text);

			AddItemToCombobox(comboFindWhat, comboFindWhat.Text);
			AddItemToCombobox(comboReplaceWith, replace);

			string selection = _Owner.Selection.Text;
			if (string.IsNullOrWhiteSpace(selection))
				return;

			bool matchCase = listOptions.Items[0].CheckState == CheckState.Checked;
			bool wholeWord = listOptions.Items[1].CheckState == CheckState.Checked;

			string pattern = Regex.Escape(findWhat);
			if (wholeWord)
				pattern = $"\b{pattern}\b";

			var re = new Regex(pattern, matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
			string result = re.Replace(selection, replace);

			_Owner.ReplaceSelection(result);
		}
	}
}