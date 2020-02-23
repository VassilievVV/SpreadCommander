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
using DevExpress.XtraEditors.Controls;
using SpreadCommander.Common;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;

namespace SpreadCommander.Documents.Viewers
{
	public partial class ImageViewer : BaseViewer
	{
		public ImageViewer()
		{
			InitializeComponent();
		}

		public override bool SupportDataSource => false;
		public override bool SupportZoom       => true;
		public override bool SupportFind       => false;
		public override bool SupportPrint      => true;

		public override void LoadFile(string fileName, Dictionary<string, string> parameters, List<BaseCommand> commands)
		{
			var image = new Bitmap(fileName);
			Editor.Image = image;
		}

		public override void AttachDataSource(object dataSource)
		{
		}

		public override void ZoomIn()
		{
			Editor.Properties.SizeMode = PictureSizeMode.Zoom;
			Editor.Properties.ZoomPercent *= 1.1;
		}

		public override void ZoomOut()
		{
			Editor.Properties.SizeMode = PictureSizeMode.Zoom;
			Editor.Properties.ZoomPercent *= 0.9;
		}

		public override void Zoom100()
		{
			Editor.Properties.SizeMode = PictureSizeMode.Stretch;
		}

		public override void Find(string value)
		{
		}

		public override void ClearFind()
		{
		}

		public override void Print()
		{
			Printing.PrintControl(this, Editor);
		}
	}
}
