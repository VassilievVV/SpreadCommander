using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alsing.Windows.Forms.Controls.EditView;
using DevExpress.XtraEditors;

namespace SpreadCommander.Documents.Dialogs.SyntaxEditorDialogs
{
	public partial class EditorGotoLineForm: XtraForm
	{
		private readonly EditViewControl _Owner;

		public EditorGotoLineForm(EditViewControl owner)
		{
			InitializeComponent();

			_Owner = owner;

			int rowCount	= _Owner.Document.Count;
			spinLineNumber.Properties.MaxValue = Math.Max(rowCount, 1);
			lblCaption.Text = $"Line number (1 - {rowCount})";
			ActiveControl	= spinLineNumber;
		}

		private void EditorGotoLineForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult != DialogResult.OK)
				return;

			int row = Convert.ToInt32(spinLineNumber.Value);
			_Owner.GotoLine(Math.Max(row - 1, 1));
		}
	}
}