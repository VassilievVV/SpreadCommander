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
using System.IO;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Viewers
{
	public partial class BaseViewer : DevExpress.XtraEditors.XtraUserControl
	{
		public BaseViewer()
		{
			InitializeComponent();
		}

		public virtual bool SupportDataSource => false;
		public virtual bool SupportZoom       => false;
		public virtual bool SupportFind       => false;
		public virtual bool SupportPrint      => false;

		public virtual void LoadFile(string fileName, Dictionary<string, string> parameters, List<BaseCommand> commands)
		{
		}

		public virtual void AttachDataSource(object dataSource)
		{
		}

		public virtual void ZoomIn()
		{
		}

		public virtual void ZoomOut()
		{
		}

		public virtual void Zoom100()
		{
		}

		public virtual void Find(string value)
		{
		}

		public virtual void ClearFind()
		{
		}

		public virtual void Print()
		{
		}

#pragma warning disable IDE0060 // Remove unused parameter
		public static BaseViewer CreateViewer(IWin32Window owner,
			string fileName, StringNoCaseDictionary<string> parameters, List<BaseCommand> commands)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (string.IsNullOrWhiteSpace(fileName))
				return new OtherViewer();

			var ext = Path.GetExtension(fileName)?.ToLower();
			BaseViewer result = null;
			switch (ext)
			{
				case ".docx":
				case ".doc":
				case ".rtf":
				case ".htm":
				case ".html":
				case ".mht":
				case ".odt":
				case ".epub":
					result = new BookViewer();
					break;
				case ".png":
				case ".jpg":
				case ".gif":
				case ".tif":
				case ".tiff":
				case ".bmp":
					result = new ImageViewer();
					break;
				case ".csv":
				case ".txt":
					result = new GridViewer();
					break;
				case ".xlsx":
				case ".xls":
					result = new SpreadsheetViewer();
					break;
				case ".ps1":
				case ".psm1":
				case ".psd1":
				case ".ps":
				case ".csx":
				case ".cs":
				case ".fsx":
				case ".fs":
				case ".sql":
				case ".r":
				case ".py":
					result = new SyntaxViewer();
					break;
			}

			if (result == null)
				result = new OtherViewer();

			try
			{
				if (parameters == null)
					parameters = new StringNoCaseDictionary<string>();
				if (commands == null)
					commands = new List<BaseCommand>();

				result.LoadFile(fileName, parameters, commands);
			}
			catch (Exception ex)
			{
				result = new OtherViewer($"Cannot load file: {ex.Message}");
			}

			return result;
		}
	}
}
