using DevExpress.XtraEditors;
using SpreadCommander.Documents.Code;
using SpreadCommander.Documents.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
	public class SpreadsheetTableSelectorService: ISpreadsheetTableSelectorService
	{
		public IWin32Window _Owner;

		public SpreadsheetTableSelectorService()
		{
		}

		public SpreadsheetTableSelectorService(IWin32Window owner) : this()
		{
			_Owner = owner;
		}

		public SpreadDataSourceData SelectSpreadsheetTables(string fileName)
		{
			var control = new SpreadsheetTableSelectorControl(fileName);
			
			var dlgResult = XtraDialog.Show(_Owner, control, "Table selected", MessageBoxButtons.OK);
			if (dlgResult != DialogResult.OK)
				return null;

			var result = new SpreadDataSourceData()
			{
				FileName = fileName,
				Worksheet = control.SelectedTable?.SheetName,
				TableName = control.SelectedTable?.TableName,
				TableRange = control.SelectedTable?.Range
			};
			return result;
		}
	}
}
