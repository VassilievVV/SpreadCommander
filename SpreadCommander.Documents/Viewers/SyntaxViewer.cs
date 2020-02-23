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

namespace SpreadCommander.Documents.Viewers
{
	public partial class SyntaxViewer : BaseViewer
	{
		public SyntaxViewer()
		{
			InitializeComponent();
		}

		public override bool SupportDataSource => false;
		public override bool SupportZoom       => false;
		public override bool SupportFind       => true;
		public override bool SupportPrint      => true;

		public override void LoadFile(string fileName, Dictionary<string, string> parameters, List<BaseCommand> commands)
		{
			Editor.LoadFromFile(fileName);
		}

		public override void Find(string value)
		{
			ClearFind();
			Editor.FindNext(value, false, false, false);
		}

		public override void ClearFind()
		{
		}

		public override void Print()
		{
			Editor.PrintScript();
		}
	}
}
