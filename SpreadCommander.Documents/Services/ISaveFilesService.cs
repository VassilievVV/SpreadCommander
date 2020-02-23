#pragma warning disable CRR0047

using SpreadCommander.Documents.Code;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Documents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Services
{
	#region SaveFileData
	public class SaveFileData
	{
		public bool Selected					{ get; set; }
		public string Title						{ get; set; }
		public string FileName					{ get; set; }

		public BaseDocumentViewModel ViewModel	{ get; set; }
	}
	#endregion

	public interface ISaveFilesService
	{
		DialogResult SaveFiles(IList<SaveFileData> filesData);
		DialogResult SaveFiles(BaseDocumentViewModel viewModel);
	}
}
