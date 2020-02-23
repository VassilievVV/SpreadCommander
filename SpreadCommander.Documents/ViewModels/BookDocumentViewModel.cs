using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.XtraRichEdit.API.Native;
using System.IO;
using System.Data;
using System.Threading;
using DevExpress.XtraRichEdit;
using SpreadCommander.Common.Code;
using DevExpress.Office.Services;
using SpreadCommander.Common;
using System.Collections.Generic;
using SpreadCommander.Documents.Code;
using System.ComponentModel;
using SpreadCommander.Common.Extensions;
using SpreadCommander.Common.Book;

namespace SpreadCommander.Documents.ViewModels
{
    [POCOViewModel]
    public class BookDocumentViewModel: BaseDocumentViewModel
    {
        public const string ViewName = "BookDocument";

        #region ICallback
        public interface ICallback
        {
            void LoadFromStream(Stream stream, DocumentFormat documentFormat);
            void SaveToStream(Stream stream, DocumentFormat documentFormat);
            object InvokeFunction(Func<object> method);
            void DocumentModified();
            void UpdateMergeSource(MailMergeOptions mergeOptions);

            void BeginWait();
            void EndWait();

            Document Document				{ get; }
            DocumentFormat CurrentFormat	{ get; }
            bool Modified                   { get; set; }
        }
        #endregion

        #region BookParametersData
        public class BookParametersData : BaseDocumentViewModel.ParametersData
        {
            public Stream BookStream { get; set; }
        }
        #endregion

        public BookDocumentViewModel()
        {
        }

        public static BookDocumentViewModel Create() =>
            ViewModelSource.Create<BookDocumentViewModel>(() => new BookDocumentViewModel());

        public ICallback Callback { get; set; }

        public Document Document => Callback?.Document;

        public override string DefaultExt => "docx";
        public override string FileFilter => "Microsoft Word 2007+ Document (*.docx)|*.docx|Microsoft Word Document (*.doc)|*.doc|Rich Text Format (*.rtf)|*.rtf|OpenDocument Text Document (*.odt)|*.odt|Electronic Publication (*.epub)|*.epub";

        public override string DocumentType => ViewName;

        public override bool Modified
        {
            get => base.Modified;
            set
            {
                if (base.Modified != value)
                {
                    base.Modified = value;
                    if (value)
                        Callback?.DocumentModified();
                }
            }
        }

        public override void LoadFromFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            Document.LoadDocument(fileName);
            base.LoadFromFile(fileName);
        }
        
        public void LoadFromStream(Stream stream)
        {
            Document.LoadDocument(stream);
        }

        public override void SaveToFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            Document.SaveDocument(fileName, Callback.CurrentFormat);
            base.SaveToFile(fileName);
        }

        public void UpdateMailMergeFields()
        {
            using (new UsingProcessor(() => Callback.BeginWait(), () => Callback.EndWait()))
            {
                Document.UpdateAllFields();
            }
        }

        public void CloneBook()
        {
            var book = Document ?? throw new NullReferenceException("Book is not loaded yet.");
            var modified = Callback.Modified;

            using (new UsingProcessor(() => Callback.BeginWait(), () => Callback.EndWait()))
            {
                using var stream = new MemoryStream(65536);
                book.SaveDocument(stream, DocumentFormat.OpenXml);
                stream.Seek(0, SeekOrigin.Begin);
                Callback.Modified = modified;

                var newBookModel = AddNewBookModel();
                newBookModel.LoadFromStream(stream);
                newBookModel.Modified = true;
            }
        }
    }
}