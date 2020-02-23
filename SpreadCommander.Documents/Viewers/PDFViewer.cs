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
using DevExpress.XtraPdfViewer;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;

namespace SpreadCommander.Documents.Viewers
{
	public partial class PDFViewer : BaseViewer
	{
		public PDFViewer()
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
			Editor.ZoomMode = PdfZoomMode.Custom;
			Editor.ZoomFactor *= 1.1f;
		}

		public override void ZoomOut()
		{
			Editor.ZoomMode = PdfZoomMode.Custom;
			Editor.ZoomFactor *= 0.9f;
		}

		public override void Zoom100()
		{
			Editor.ZoomMode = PdfZoomMode.PageLevel;

		}

		public override void Find(string value)
		{
			Editor.FindText(value);
		}

		public override void ClearFind()
		{
		}

		public override void Print()
		{
			Editor.Print();
		}
	}
}
