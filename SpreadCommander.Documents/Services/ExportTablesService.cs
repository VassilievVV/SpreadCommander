using SpreadCommander.Documents.Code;
using SpreadCommander.Documents.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
	public class ExportTablesService: IExportTablesService
	{
		private readonly IWin32Window _Owner;

		public ExportTablesService(IWin32Window owner)
		{
			_Owner = owner;
		}

		public void ExportDataTables(IExportSource source) =>
			ExportDataTables(_Owner, source);

		public static void ExportDataTables(IWin32Window owner, IExportSource source)
		{
            using var frm = new ExportTablesForm();
            frm.SetExportSource(source);
            frm.ShowDialog(owner);
        }
	}
}
