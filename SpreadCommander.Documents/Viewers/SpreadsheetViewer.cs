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
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Extensions;

namespace SpreadCommander.Documents.Viewers
{
	public partial class SpreadsheetViewer : BaseViewer
	{
		public SpreadsheetViewer()
		{
			InitializeComponent();

			Editor.InitializeSpreadsheet();
		}

		public override bool SupportDataSource => true;
		public override bool SupportZoom       => true;
		public override bool SupportFind       => true;
		public override bool SupportPrint      => true;

		public override void LoadFile(string fileName, Dictionary<string, string> paramters, List<BaseCommand> commands)
		{
			Editor.LoadDocument(fileName);
		}

		public override void AttachDataSource(object dataSource)
		{
			if (!Editor.Document.CreateNewDocument())
				return;

			Editor.ActiveWorksheet.DataBindings.BindTableToDataSource(dataSource);
		}

		public override void ZoomIn()
		{
			Editor.ActiveViewZoom = Convert.ToInt32(Editor.ActiveViewZoom * 1.1);
		}

		public override void ZoomOut()
		{
			Editor.ActiveViewZoom = Convert.ToInt32(Editor.ActiveViewZoom * 0.9);
		}

		public override void Zoom100()
		{
			Editor.ActiveViewZoom = 100;
		}

		public override void Find(string value)
		{
			var cells = Editor.ActiveWorksheet.Search(value);
			var selectedCell = cells.FirstOrDefault();
			if (selectedCell != null)
				Editor.ActiveWorksheet.SelectedCell = selectedCell;
		}

		public override void ClearFind()
		{
		}

		public override void Print()
		{
			Editor.ShowRibbonPrintPreview();
		}

		private void Editor_EncryptedFilePasswordRequest(object sender, DevExpress.Spreadsheet.EncryptedFilePasswordRequestEventArgs e)
		{
			e.Cancel  = true;
			e.Handled = true;
		}
	}
}
