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
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System.IO;

namespace SpreadCommander.Documents.Viewers
{
	public partial class BookViewer : BaseViewer
	{
		public BookViewer()
		{
			InitializeComponent();
		}

		public override bool SupportDataSource => false;
		public override bool SupportZoom       => true;
		public override bool SupportFind       => true;
		public override bool SupportPrint      => true;

		public override void LoadFile(string fileName, Dictionary<string, string> parameters, List<BaseCommand> commands)
		{
			Editor.LoadDocument(fileName);
		}

		public override void AttachDataSource(object dataSource)
		{
		}

		public override void ZoomIn()
		{
			Editor.ActiveView.ZoomFactor *= 1.1f;
		}

		public override void ZoomOut()
		{
			Editor.ActiveView.ZoomFactor *= 0.9f;
		}

		public override void Zoom100()
		{
			Editor.ActiveView.ZoomFactor *= 1.0f;
		}

		public override void Find(string value)
		{
			ClearFind();

			var options = DevExpress.XtraRichEdit.API.Native.SearchOptions.None;
			var ranges  = Editor.Document.FindAll(value, options);
			if (ranges != null && ranges.Length > 0)
			{
				foreach (var range in ranges)
				{
					Editor.Document.CustomMarks.Create(range.Start, range);
					Editor.Document.CustomMarks.Create(range.End, range);
				}
			}
		}

		public override void ClearFind()
		{
			for (int i = Editor.Document.CustomMarks.Count - 1; i >= 0; i--)
			{
				var mark = Editor.Document.CustomMarks[i];
				Editor.Document.CustomMarks.Remove(mark);
			}
		}

		public override void Print()
		{
			Editor.ShowPrintPreview();
		}
	}
}
