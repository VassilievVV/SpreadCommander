using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using SpreadCommander.Documents.Code;
using System.Collections.Generic;
using SpreadCommander.Common;

namespace SpreadCommander.Documents.ViewModels
{
	[POCOViewModel]
	public class PdfDocumentViewModel : BaseDocumentViewModel
	{
		public const string ViewName = "PdfDocument";

		#region ICallback
		public interface ICallback
		{
			void LoadFromFile(string fileName);
			void SaveToFile(string fileName);
		}
		#endregion

		public PdfDocumentViewModel()
		{
		}

		public static PdfDocumentViewModel Create() =>
			ViewModelSource.Create<PdfDocumentViewModel>(() => new PdfDocumentViewModel());

		public override string DefaultExt   => "pdf";
		public override string FileFilter   => "Abode PDF document (*.pdf)|*.pdf";
		public override string DocumentType => ViewName;

		public override bool Modified { get => false; set { } }

		public ICallback Callback { get; set; }

		public override void LoadFromFile(string fileName)
		{
			fileName = Project.Current.MapPath(fileName);
			Callback.LoadFromFile(fileName);
			base.LoadFromFile(fileName);
		}

		public override void SaveToFile(string fileName)
		{
			fileName = Project.Current.MapPath(fileName);
			Callback.SaveToFile(fileName);
			base.SaveToFile(fileName);
		}
	}
}