using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
	public class ScriptIntellisense
	{
		public ScriptIntellisenseHelp Help { get; set; }

		public bool UsePeriodInIntellisense { get; set; }

		public List<ScriptIntellisenseItem> Items { get; } = new List<ScriptIntellisenseItem>();
	}
}
