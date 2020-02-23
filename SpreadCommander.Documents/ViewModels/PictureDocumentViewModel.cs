using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Collections.Generic;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common;

namespace SpreadCommander.Documents.ViewModels
{
	[POCOViewModel]
	public class PictureDocumentViewModel: BaseDocumentViewModel
	{
		public const string ViewName = "PictureDocument";

		#region ICallback
		public interface ICallback
		{
			void LoadFromFile(string fileName);
		}
		#endregion

		public PictureDocumentViewModel()
		{
		}

		public static PictureDocumentViewModel Create() =>
			ViewModelSource.Create<PictureDocumentViewModel>(() => new PictureDocumentViewModel());

		public override string DefaultExt   => "png";
		public override string FileFilter   => "Picture (*.png;*.jpg;*.gif;*.tif;*.bmp)|*.png;*.jpg;*.gif;*.tif;*.bmp";
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
			//No saving in this view
			base.SaveToFile(fileName);
		}
	}
}