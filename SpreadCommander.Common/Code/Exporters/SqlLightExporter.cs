using SpreadCommander.Common.SqlScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code.Exporters
{
	public class SqlLightExporter: SqlExporter
	{
		public override string Name => "MS SQL Server Exporter (light)";

		public override object CreateConnectionStringBuilder()
		{
			return new SqlConnectionStringBuilderLight();
		}
	}
}
