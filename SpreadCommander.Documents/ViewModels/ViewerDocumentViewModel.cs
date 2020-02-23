using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Collections.Generic;
using SpreadCommander.Common;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.ViewModels
{
	[POCOViewModel]
	public class ViewerDocumentViewModel: BaseDocumentViewModel
	{
		#region ICallback
		public interface ICallback
		{
			void LoadFile(string fileName, StringNoCaseDictionary<string> parameters, List<BaseCommand> commands);
		}
		#endregion

		public const string ViewName = "ViewerDocument";

		public ViewerDocumentViewModel()
		{
		}

		public static ViewerDocumentViewModel Create() =>
			ViewModelSource.Create<ViewerDocumentViewModel>(() => new ViewerDocumentViewModel());

		public override string DocumentType => ViewName;

		public ICallback Callback { get; set; }

		public override bool Modified { get => false; set { } }

		public void LoadFile(string fileName, StringNoCaseDictionary<string> parameters, List<BaseCommand> commands)
		{
			Callback?.LoadFile(fileName, parameters, commands);
		}

		public override void LoadFromFile(string fileName)
		{
			fileName = Project.Current.MapPath(fileName);
			Callback.LoadFile(fileName, null, null);
			base.LoadFromFile(fileName);
		}

		public override void SaveToFile(string fileName)
		{
			fileName = Project.Current.MapPath(fileName);
			//This view does not allow saving
			base.SaveToFile(fileName);
		}
	}
}