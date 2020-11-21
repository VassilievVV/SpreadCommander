using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Code
{
	public class DocumentLayout
	{
		public string DocumentType				{ get; set; }
		public string DocumentSubType			{ get; set; }

		public List<DocumentLayoutFile> Files	{ get; } = new List<DocumentLayoutFile>();
	}

	public class DocumentLayoutFile
	{
		public string FileName { get; set; }
	}

	public class RecentLayout
	{
		public List<DocumentLayout> DocumentLayouts { get; } = new List<DocumentLayout>();
	}
}
