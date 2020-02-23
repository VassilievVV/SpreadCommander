using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
	public class ScriptIntellisenseHelp
	{
		public virtual bool SupportsHelp       => false;
		public virtual bool SupportsOnlineHelp => false;

		public virtual string GetHelpHtmlContent(ScriptIntellisenseItem item) => null;

		public virtual void ShowOnlineHelp(ScriptIntellisenseItem item) { }
	}
}
