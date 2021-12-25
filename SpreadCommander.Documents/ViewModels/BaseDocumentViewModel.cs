using DevExpress.Compression;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.SqlScript;
using SpreadCommander.Documents.Code;
using SpreadCommander.Documents.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpreadCommander.Common;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using Common = SpreadCommander.Common;

namespace SpreadCommander.Documents.ViewModels
{
    [POCOViewModel]
    public partial class BaseDocumentViewModel : ViewModelBase, IDocument, IDocumentContent, ISupportParentViewModel, IExportSource
    {
        public enum ParametersScriptType { Sql, PowerShell, FSharp }

#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static int DocumentCounter = 1;
#pragma warning restore CA2211 // Non-constant fields should not be visible
#pragma warning disable CRRSP06 // A misspelled word has been found
        public const string ParametersFileExtension = ".scparams";
#pragma warning restore CRRSP06 // A misspelled word has been found

        #region IDocumentParent
        public interface IDocumentParent
        {
            IDocument AddNewDocument(BaseDocumentViewModel viewModel, string modelName);
            
            void SelectDBConnectionByName(string connectionName);

            IEnumerable<BaseDocumentViewModel> OpenDocuments { get; }
            BaseDocumentViewModel ActiveDocument { get; }

            bool IsFileOpen(string fileName);

            BaseDocumentViewModel GetFileDocument(string fileName);

            void OpenFile();
            
            BaseDocumentViewModel OpenDocumentFile(string fileName);
        }
        #endregion

        #region ParametersData
        public class ParametersData
        {
            public DataTable Parameters					{ get; set; }
            public ParametersScriptType DataScriptType	{ get; set; }
            public string DataScript					{ get; set; }
            public ParametersScriptType DataParametersScriptType { get; set; }
            public string DataParametersScript			{ get; set; }
            public string OutputDirectory				{ get; set; }
        }
        #endregion

        public BaseDocumentViewModel()
        {
            _ID    = DocumentCounter++;
            _Title = $"Document {_ID}";
        }

        protected IDialogService DialogService                                     => this.GetService<IDialogService>();
        protected IMessageBoxService MessageService                                => this.GetService<IMessageBoxService>();
        //protected IDispatcherService DispatcherService                             => this.GetService<IDispatcherService>();
        protected ISCDispatcherService SCDispatcherService                         => this.GetService<ISCDispatcherService>();
        protected IOpenFileDialogService OpenFileService                           => this.GetService<IOpenFileDialogService>();
        protected ISaveFileDialogService SaveFileService                           => this.GetService<ISaveFileDialogService>();
        protected IFolderBrowserDialogService BrowseFolderService                  => this.GetService<IFolderBrowserDialogService>();
        protected ISpreadsheetEditTableService SpreadsheetEditTableService         => this.GetService<ISpreadsheetEditTableService>();
        protected ISpreadsheetTableSelectorService SpreadsheetTableSelectorService => this.GetService<ISpreadsheetTableSelectorService>();
        protected IAlertService AlertService                                       => this.GetService<IAlertService>();
        protected IExportTablesService TableExporterService                        => this.GetService<IExportTablesService>();
        protected ISpreadsheetEditTableService SpreadsheetTableService             => this.GetService<ISpreadsheetEditTableService>();
        protected ISpreadsheetTemplateService SpreadsheetTemplateService           => this.GetService<ISpreadsheetTemplateService>();
        protected IBookTemplateService BookTemplateService                         => this.GetService<IBookTemplateService>();
        protected IImportExportSpreadSheetsService ImportExportSpreadSheetsService => this.GetService<IImportExportSpreadSheetsService>();
        protected IEditObjectService EditObjectService                             => this.GetService<IEditObjectService>();

        public virtual IDocumentOwner DocumentOwner			{ get; set; }

        private string _Title;
        public virtual object Title => _Title;

        private int _ID;
        public object Id									{ get => _ID; set => _ID = Convert.ToInt32(value); }
        public virtual Guid DocumentUniqueID				{ get; set; } = Guid.NewGuid();
        public object LockObject							{ get; }      = new object();

        public object Content								=> this;

        object IDocument.Title								{ get => _Title; set => _Title = Convert.ToString(value); }

        public bool DestroyOnClose							{ get; set; } = true;

        public object ParentViewModel						{ get; set; }

        public IDocumentParent DocumentParent				{ get => ParentViewModel as IDocumentParent; set => ParentViewModel = value; }

        public virtual bool Modified						{ get; set; }

        public virtual string DefaultExt					{ get; }
        public virtual string FileFilter					{ get; }

        public virtual string DocumentType					{ get; }
        public virtual string DocumentSubType				{ get; }

        private string _FileName;
        public string FileName
        {
            get => _FileName;
            set
            {
                _FileName = value;
                UpdateTitle();
            }
        }

        public void UpdateTitle()
        {
            SetTitle(!string.IsNullOrWhiteSpace(_FileName) ? Path.GetFileNameWithoutExtension(_FileName) : $"Document {Id}");
        }

        private static IDocumentParent _MainDocumentParent;
        public static IDocumentParent MainDocumentParent
        {
            get => _MainDocumentParent;
            set
            {
                if (_MainDocumentParent != null)
                    throw new Exception("Only one main document parent is allowed");
                _MainDocumentParent = value;
            }
        }


[Command(false)]
        public virtual void InitializeBindings()
        {
            //Do nothing. This function is called to ensure that model is created.
        }

        public virtual void OnClose(CancelEventArgs e)
        {
            if (Modified)
            {
                switch (MessageService.ShowMessage("Do you want to save changes?", "Save changes", MessageButton.YesNoCancel, MessageIcon.Question, MessageResult.Yes))
                {
                    case MessageResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageResult.Yes:
                        Save();
                        break;
                    case MessageResult.No:
                        //Do nothing, continue closing
                        break;
                }
            }
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void Show()
        {
        }

        public virtual void SetTitle(string value)
        {
            _Title = value;
            RaisePropertyChanged(nameof(Title));
        }

        public virtual string AskForFileName()
        {
            SaveFileService.Title      = Common.Parameters.ApplicationName;
            SaveFileService.DefaultExt = DefaultExt;
            SaveFileService.Filter     = FileFilter;
            if (!SaveFileService.ShowDialog())
                return null;
            return SaveFileService.GetFullFileName();
        }

        public virtual void Save()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                FileName = AskForFileName();

            if (!string.IsNullOrWhiteSpace(FileName))
                SaveToFile(FileName);
        }

        public void Hide()
        {
        }

        public void Close(bool force = true)
        {
        }

        public void DocumentModified()
        {
            Modified = true;
        }

        public void AddNewDocument(BaseDocumentViewModel viewModel, string modelName) =>
            DocumentParent.AddNewDocument(viewModel, modelName);

        public BookDocumentViewModel AddNewBookModel()
        {
             var model = BookDocumentViewModel.Create();
            AddNewDocument(model, BookDocumentViewModel.ViewName);
            return model;           
        }
        
        public SpreadsheetDocumentViewModel AddNewSpreadsheetModel()
        {
            var model = SpreadsheetDocumentViewModel.Create();
            AddNewDocument(model, SpreadsheetDocumentViewModel.ViewName);
            return model;
        }
        
        public static void StaticAddNewDocument(BaseDocumentViewModel viewModel, string modelName) =>
            MainDocumentParent.AddNewDocument(viewModel, modelName);

        public static BookDocumentViewModel StaticAddNewBookModel()
        {
            var model = BookDocumentViewModel.Create();
            StaticAddNewDocument(model, BookDocumentViewModel.ViewName);
            return model;
        }

        public static SpreadsheetDocumentViewModel StaticAddNewSpreadsheetModel()
        {
            var model = SpreadsheetDocumentViewModel.Create();
            StaticAddNewDocument(model, SpreadsheetDocumentViewModel.ViewName);
            return model;
        }

        public virtual string[] GetTableNames()
        {
            throw new Exception("This document does not support exporting tables");
        }

        public virtual DbDataReader GetDataTable(string tableName)
        {
            throw new Exception("This document does not support exporting tables");
        }

        public virtual void LoadFromFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            FileName = fileName;

            SetTitle(Path.GetFileNameWithoutExtension(fileName));

            Modified = false;
        }

        public virtual void SaveToFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);
            FileName = fileName;

            SetTitle(Path.GetFileName(fileName));

            Modified = false;
        }
    }
}
