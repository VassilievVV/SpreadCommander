using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.SqlScript
{
	public class LoadAdapter : DataAdapter
	{
		public LoadAdapter()
		{
		}

		public new void FillSchema(DataTable dataTable, SchemaType schemaType, IDataReader dataReader)
		{
			base.FillSchema(dataTable, schemaType, dataReader);
		}

		public new int Fill(DataTable dataTable, IDataReader dataReader)
		{
			return base.Fill(dataTable, dataReader);
		}
	}
}
