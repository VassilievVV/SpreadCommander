using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
	public class SelectedDbConnection
	{
		public string ConnectionName		{ get; set; }

		public Connection Connection		{ get; set; }

		public ConnectionFactory Factory	{ get; set; }

		public string ConnectionString		{ get; set; }
	}
}
