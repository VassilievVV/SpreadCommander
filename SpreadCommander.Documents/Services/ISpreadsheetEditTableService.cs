using SpreadCommander.Documents.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Services
{
	#region ScriptParameters
	public class ScriptParameters
	{
		public DataTable Parameters										{ get; set; }
		public BaseDocumentViewModel.ParametersScriptType ScriptType	{ get; set; }
		public string Script											{ get; set; }
	}
	#endregion

	public interface ISpreadsheetEditTableService
	{
		bool EditTable(ScriptParameters parameters);
	}
}
