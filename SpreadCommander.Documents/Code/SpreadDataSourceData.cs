using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Code
{
	public class SpreadDataSourceData
	{
		public string FileName   { get; set; }
		public string Worksheet  { get; set; }
		public string TableName  { get; set; }
		public string TableRange { get; set; }

		public void Assign(SpreadDataSourceData source)
		{
			FileName   = source?.FileName;
			Worksheet  = source?.Worksheet;
			TableName  = source?.TableName;
			TableRange = source?.TableRange;
		}

		public void Clear() => Assign(null);
	}
}
