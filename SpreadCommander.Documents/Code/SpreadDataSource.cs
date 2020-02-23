using DevExpress.DataAccess.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Code
{
	public class SpreadDataSource: ExcelDataSource
	{
		//This string is displayed in some UI
		public override string ToString() => "Data Source";
	}
}
