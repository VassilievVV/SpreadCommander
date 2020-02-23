using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.SqlScript
{
	public class ScriptRunParameters
	{
		public List<ScriptRunParameter> CommandParameters { get; } = new List<ScriptRunParameter>();
		public int CommandTimeout { get; set; } = 0;

		public ScriptRunParameter FindDbParameter(string name)
		{
			var result =
				from parameter in CommandParameters
				where string.Compare(parameter.Name, name, true) == 0
				select parameter;

			return result.FirstOrDefault();
		}
	}
}
