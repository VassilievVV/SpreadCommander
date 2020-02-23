using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Code
{
	public interface IExportSource
	{
		string[] GetTableNames();
		DbDataReader GetDataTable(string tableName);
	}
}
