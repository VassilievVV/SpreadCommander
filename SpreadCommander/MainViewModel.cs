using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using SpreadCommander.Documents;
using SpreadCommander.Documents.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common;
using SpreadCommander.Common.ScriptEngines;
using SpreadCommander.Common.Messages;
using System.ComponentModel;
using System.Data;
using SpreadCommander.Documents.Services;
using SpreadCommander.Common.SqlScript;
using System.Threading;
using SpreadCommander.Documents.Code;
using SpreadCommander.Common.Code;
using ConsoleCommands = SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System.Data.Common;
using System.IO;
using System.Reflection;
using SpreadCommander.Documents.Messages;

namespace SpreadCommander
{
    [POCOViewModel()]
    public class MainViewModel : ViewModelBase, BaseDocumentViewModel.IDocumentParent, IDocumentContent
    {
        #region ICallback
        public interface ICallback
        {
            void ProjectChanged();
            void ConnectionListChanged();
            void PSCmdletListChanged();
            void StartAddingDocument();
            void EndAddingDocument();
            void SelectDBConnectionByName(string connectionName);
            void ControlModified(ControlModifiedMessage message);
        }
        #endregion

        public const string DocumentServiceKey = "Documents";
        public const string FloatDocumentServiceKey = "FloatDocuments";

        public MainViewModel()
        {
            BaseDocumentViewModel.MainDocumentParent = this;

            Project.ProjectChanged += ProjectChanged;

            Messenger.Default.Register<ConnectionListChangedMessage>(this, OnConnectionListChangedMessage);
            Messenger.Default.Register<PSCmdletListChangedMessage>(this, OnPSCmdletListChangedMessage);
            Messenger.Default.Register<ControlModifiedMessage>(this, OnControlModifiedMessage);
            Messenger.Default.Register<DocumentStartAddingMessage>(this, OnDocumentStartAdding);
            Messenger.Default.Register<DocumentEndAddingMessage>(this, OnDocumentEndAdding);
        }

        public static MainViewModel Create() =>
            ViewModelSource.Create<MainViewModel>();

        public void OnClose(CancelEventArgs e)
        {
            if (!CloseProject(true))
                e.Cancel = true;
        }

        public void OnDestroy()
        {
            Messenger.Default.Unregister<ConnectionListChangedMessage>(this);
            Messenger.Default.Unregister<PSCmdletListChangedMessage>(this);
            Messenger.Default.Unregister<ControlModifiedMessage>(this);
        }

#pragma warning disable CA1822 // Mark members as static
        [Command(false)]
        public void InitializeBindings()
        {
            //Do nothing. This function is called to ensure that model is created.
        }
#pragma warning restore CA1822 // Mark members as static

        [Command(false)]
        public void InitializeServices()
        {
            DocumentService.ActiveDocumentChanged += DocumentService_ActiveDocumentChanged;
        }

        protected IDocumentManagerService DocumentService                  => this.GetService<IDocumentManagerService>(DocumentServiceKey);
        protected IDocumentManagerService FloatDocumentService             => this.GetService<IDocumentManagerService>(FloatDocumentServiceKey);
        protected IDialogService DialogService                             => this.GetService<IDialogService>();
        protected IMessageBoxService MessageService                        => this.GetService<IMessageBoxService>();
        //protected IDispatcherService DispatcherService                     => this.GetService<IDispatcherService>();
        protected ISCDispatcherService SCDispatcherService                 => this.GetService<ISCDispatcherService>();
        protected IOpenFileDialogService OpenFileService                   => this.GetService<IOpenFileDialogService>();
        protected ISaveFileDialogService SaveFileService                   => this.GetService<ISaveFileDialogService>();
        protected IFolderBrowserDialogService BrowseFolderService          => this.GetService<IFolderBrowserDialogService>();
        protected ISpreadsheetEditTableService SpreadsheetEditTableService => this.GetService<ISpreadsheetEditTableService>();
        protected IAlertService AlertService                               => this.GetService<IAlertService>();
        protected IBatchProcessDocumentsService BatchProcessDocuments             => this.GetService<IBatchProcessDocumentsService>();
        protected ISaveFilesService SaveFilesService                       => this.GetService<ISaveFilesService>();
        protected IEditObjectService EditObjectService                     => this.GetService<IEditObjectService>();
        protected ISelectProjectService SelectProjectService               => this.GetService<ISelectProjectService>();
        protected IViewRichTextService ViewRichTextService                 => this.GetService<IViewRichTextService>();


        public virtual ICallback Callback { get; set; }
        public IDocumentOwner DocumentOwner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object Title => throw new NotImplementedException();

        public BaseDocumentViewModel ActiveDocument { get; private set; }

        protected void ProjectChanged(object sender, EventArgs e)
        {
            UpdateCommands();
            Callback?.ProjectChanged();
        }

        public void OnConnectionListChangedMessage(ConnectionListChangedMessage message)
        {
            SCDispatcherService.BeginInvoke(() => Callback?.ConnectionListChanged());
        }

        public void OnPSCmdletListChangedMessage(PSCmdletListChangedMessage message)
        {
            SCDispatcherService.BeginInvoke(() => Callback?.PSCmdletListChanged());
        }
        
        public void OnControlModifiedMessage(ControlModifiedMessage message)
        {
            Callback?.ControlModified(message);
        }

        private void DocumentService_ActiveDocumentChanged(object sender, ActiveDocumentChangedEventArgs e)
        {
            ActiveDocument = e.NewDocument?.Content as BaseDocumentViewModel;
            UpdateCommands();
        }

#pragma warning disable CA1822 // Mark members as static
        protected void UpdateCommands()
        {
        }
#pragma warning restore CA1822 // Mark members as static

        public IEnumerable<BaseDocumentViewModel> OpenDocuments
        {
            get
            {
                var result =
                    from doc in DocumentService.Documents
                    where  (doc as IDocument)?.Content is BaseDocumentViewModel
                    select (doc as IDocument)?.Content as BaseDocumentViewModel;
                return result;
            }
        }

        public void NewProject()
        {
            if (!CloseProject())
                return;

            if (!BrowseFolderService.ShowDialog())
                return;

            var projectPath = BrowseFolderService.ResultPath;
            Project.CreateNewProject(projectPath, true);
        }

        public void OpenProject()
        {
            var currentProject = Project.Current.ProjectPath;

            if (!CloseProject())
                return;

            string projectPath;
            if (BrowseFolderService.ShowDialog())
                projectPath = BrowseFolderService.ResultPath;
            else
                projectPath = currentProject;

            Project.LoadProjectFromDirectory(projectPath);
        }

        public void OpenExistingProject(string directory)
        {
            if (!CloseProject())
                return;

            Project.LoadProjectFromDirectory(directory);
        }

        public void SelectProject()
        {
            var currentProject = Project.Current.ProjectPath;

            if (!CloseProject())
                return;

            var projectPath = SelectProjectService.SelectProject() ?? currentProject;
            Project.LoadProjectFromDirectory(projectPath);
        }

        //Must execute from UI thread
        public bool CloseProject(bool closingApplication = false)
        {
            if (!SaveAllDocuments())
                return false;
            if (!CloseAllDocuments(closingApplication))
                return false;

            return true;
        }

        //Need this function to bind to UI button, 
        //SaveAllDocuments() has return type "bool" and does not work.
        public void SaveAllFiles() => SaveAllDocuments();

        //Must execute from UI thread
        public bool SaveAllDocuments()
        {
            bool hasDocuments = (DocumentService.Documents.FirstOrDefault() != null);
            if (!hasDocuments)
                return true;

            var filesData = new List<SaveFileData>();
            foreach (var document in OpenDocuments)
            {
                if (!document.Modified)
                    continue;

                var fileData = new SaveFileData()
                {
                    ViewModel = document,
                    FileName  = document.FileName,
                    Title     = Convert.ToString(document.Title)
                };
                filesData.Add(fileData);
            }

            if (filesData.Count <= 0)
                return true;	//No files to save - return "true" as if all needed files were saved.

            var result = SaveFilesService.SaveFiles(filesData);
            return result != System.Windows.Forms.DialogResult.Cancel;
        }

        //Must execute from UI thread
        public bool CloseAllDocuments(bool silent)
        {
            bool hasDocuments = (DocumentService.Documents.FirstOrDefault() != null);
            if (!hasDocuments)
                return true;

            if (!silent && MessageService.ShowMessage("Do you want to close all documents?", 
                "Confirm closing documents",
                MessageButton.YesNo, MessageIcon.Question) != MessageResult.Yes)
                return false;

            //SaveRecentLayout();

            foreach (var document in DocumentService.Documents.ToList())
                document.Close();

            return true;
        }

        public BaseDocumentViewModel GetFileDocument(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            foreach (var document in OpenDocuments)
            {
                if (string.IsNullOrWhiteSpace(document.FileName))
                    continue;

                if (string.Compare(fileName, document.FileName, true) == 0)
                    return document;
            }

            return null;
        }

        public bool HasOpenFilesInDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                return false;

            foreach (var document in OpenDocuments)
            {
                if (string.IsNullOrWhiteSpace(document.FileName))
                    continue;

                var dir = Path.GetDirectoryName(document.FileName);

                if (string.Compare(directory, dir, true) == 0)
                    return true;
            }

            return false;
        }

        public void OpenFile()
        {
            var filter = new StringBuilder();
            filter
                .Append("SpreadCommander files|*.docx;*.doc;*.rtf;*.htm;*.html;*.mht;*.odt;*.epub;*.xlsx;*.xls;*.csv;*.scchart;*.scpivot;*.scdash;*.ps1;*.sql|")
                .Append("Book files (*.docx;*.doc;*.rtf;*.htm;*.html;*.mht;*.odt;*.epub)|*.docx;*.doc;*.rtf;*.htm;*.html;*.mht;*.odt;*.epub|")
                .Append("Spreadsheet files (*.xlsx;*.xls;*.csv)|*.xlsx;*.xls;*.csv|")
                .Append("Chart files (*.scchart)|*.scchart|")
                .Append("Pivot files (*.scpivot)|*.scpivot|")
                .Append("Dashboard files (*.scdash)|*.scdash|")
                .Append("PowerShell script files (*.ps1)|*.ps1|")
                .Append("SQL script files (*.sql)|*.sql|")
                .Append("R script files (*.r)|*.r|")
                .Append("Python script files (*.py)|*.py|")
                //.Append("C# script files (*.csx)|*.csx|")
                //.Append("F# script files (*.fsx)|*.fsx|")
                .Append("Picture (*.png;*.jpg;*.gif;*.tif;*.bmp)|*.png;*.jpg;*.gif;*.tif;*.bmp");

            OpenFileService.Filter = filter.ToString();
            if (OpenFileService.ShowDialog())
                OpenDocumentFile(OpenFileService.File.GetFullName());
        }

        public BaseDocumentViewModel OpenDocumentFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            var viewModel = GetFileDocument(fileName);
            if (viewModel != null)
            {
                DocumentService.ActiveDocument = viewModel;
                return viewModel;
            }

            var ext = Path.GetExtension(fileName)?.ToLower();
            switch (ext)
            {
                case ".xlsx":
                case ".xls":
                case ".csv":
                    viewModel = AddNewSpreadsheetDocument();
                    break;
                case ".sql":
                    viewModel = AddNewSqlScriptDocument();
                    break;
                case ".ps":
                case ".ps1":
                    viewModel = AddNewPSScriptDocument();
                    break;
                //case ".csx":
                //	viewModel = AddNewCSharpScriptDocument();
                //	break;
                //case ".fsx":
                //	viewModel = AddNewFSharpScriptDocument();
                //	break;
                case ".r":
                    viewModel = AddNewRScriptDocument();
                    break;
                case ".py":
                    viewModel = AddNewPyScriptDocument();
                    break;
                case ".docx":
                case ".doc":
                case ".rtf":
                case ".htm":
                case ".html":
                case ".mht":
                case ".odt":
                case ".epub":
                case ".txt":
                    viewModel = AddNewBookDocument();
                    break;
                case ".png":
                case ".tif":
                case ".tiff":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".bmp":
                    viewModel = AddNewPictureDocument();
                    break;
                case ".scdash":
                    viewModel = AddNewDashboardDocument();
                    break;
                case ".pdf":
                    viewModel = AddNewPdfDocument();
                    break;
                case ".scchart":
                    viewModel = AddNewChartDocument();
                    break;
                case ".scpivot":
                    viewModel = AddNewPivotDocument();
                    break;
            }

            if (viewModel == null)
                return null;

            viewModel.LoadFromFile(fileName);

            return viewModel;
        }

        public void EditApplicationSettings()
        {
            EditObjectService.EditObject(ApplicationSettings.Default);
            ApplicationSettings.Default.SaveSettings();
        }

        public void ShowAbout()
        {
            using var stream = Utils.GetEmbeddedResource(Assembly.GetAssembly(typeof(MainViewModel)), "Files.License.docx");
            ViewRichTextService.ViewDocument(stream);
        }

        public void SelectDBConnectionByName(string connectionName)
        {
            Callback?.SelectDBConnectionByName(connectionName);
        }

        public void DocumentModified()
        {
            var activeDocument = ActiveDocument;
            if (activeDocument != null)
                activeDocument.Modified = true;
        }

        public void StartAddingDocument()
        {
            Callback?.StartAddingDocument();
        }

        public void EndAddingDocument()
        {
            Callback?.EndAddingDocument();
        }

#pragma warning disable CA1822 // Mark members as static
        public void OnDocumentStartAdding(DocumentStartAddingMessage message)
        {
            //Callback?.StartAddingDocument();
        }

        public void OnDocumentEndAdding(DocumentEndAddingMessage message)
        {
            //Callback?.EndAddingDocument();
        }
#pragma warning restore CA1822 // Mark members as static


        public IDocument AddNewDocument(BaseDocumentViewModel viewModel, string modelName)
        {
            viewModel.SetParentViewModel(this);

            var document = DocumentService.CreateDocument(modelName, viewModel);
            document.Show();
            ActiveDocument = viewModel;

            UpdateCommands();

            return document;
        }

        public BaseDocumentViewModel AddNewDocument(string documentType, string documentSubType)
        {
            return documentType switch
            {
                SpreadsheetDocumentViewModel.ViewName => AddNewSpreadsheetDocument(),
                DashboardDocumentViewModel.ViewName   => AddNewDashboardDocument(),
                BookDocumentViewModel.ViewName        => AddNewBookDocument(),
                SqlScriptDocumentViewModel.ViewName   => AddNewSqlScriptDocument(),
                ConsoleDocumentViewModel.ViewName     => documentSubType switch
                {
                    RScriptEngine.ScriptEngineName => AddNewRScriptDocument(),
                    PythonScriptEngine.ScriptEngineName => AddNewPyScriptDocument(),
                    PowerShellScriptEngine.ScriptEngineName => AddNewPSScriptDocument(),
                    //CSharpScriptEngine.ScriptEngineName     => AddNewCSharpScriptDocument(),
                    //FSharpScriptEngine.ScriptEngineName     => AddNewFSharpScriptDocument(),
                    _ => null,
                },
                ChartDocumentViewModel.ViewName   => AddNewChartDocument(),
                PictureDocumentViewModel.ViewName => AddNewPictureDocument(),
                PdfDocumentViewModel.ViewName     => AddNewPdfDocument(),
                _                                 => null
            };
        }

        public void AddNewSpreadsheet() => AddNewSpreadsheetDocument();
        public SpreadsheetDocumentViewModel AddNewSpreadsheetDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("Spreadsheet");

                var viewModel = SpreadsheetDocumentViewModel.Create();
                AddNewDocument(viewModel, SpreadsheetDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewDashboard() => AddNewDashboardDocument();
        public DashboardDocumentViewModel AddNewDashboardDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("Dashboard");

                var viewModel = DashboardDocumentViewModel.Create();
                AddNewDocument(viewModel, DashboardDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewBook() => AddNewBookDocument();
        public BookDocumentViewModel AddNewBookDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("Book");

                var viewModel = BookDocumentViewModel.Create();
                AddNewDocument(viewModel, BookDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewSqlScript() => AddNewSqlScriptDocument();
        public SqlScriptDocumentViewModel AddNewSqlScriptDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("SqlScript");

                var viewModel = SqlScriptDocumentViewModel.Create();
                AddNewDocument(viewModel, SqlScriptDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewRScript() => AddNewRScriptDocument();
        public ConsoleDocumentViewModel AddNewRScriptDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("RScript");

                var viewModel = ConsoleDocumentViewModel.Create(new RScriptEngine());
                AddNewDocument(viewModel, ConsoleDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewPyScript() => AddNewPyScriptDocument();
        public ConsoleDocumentViewModel AddNewPyScriptDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("PythonScript");

                var viewModel = ConsoleDocumentViewModel.Create(new PythonScriptEngine());
                AddNewDocument(viewModel, ConsoleDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewPSScript() => AddNewPSScriptDocument();
        public ConsoleDocumentViewModel AddNewPSScriptDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("PowerShellScript");

                var viewModel = ConsoleDocumentViewModel.Create(new PowerShellScriptEngine());
                AddNewDocument(viewModel, ConsoleDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        /*
        public void AddNewCSharpScript() => AddNewCSharpScriptDocument();
        public ConsoleDocumentViewModel AddNewCSharpScriptDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("CSharpScript");

                var viewModel = ConsoleDocumentViewModel.Create(new CSharpScriptEngine());
                AddNewDocument(viewModel, ConsoleDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewFSharpScript() => AddNewFSharpScriptDocument();
        public ConsoleDocumentViewModel AddNewFSharpScriptDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("FSharpScript");

                var viewModel = ConsoleDocumentViewModel.Create(new FSharpScriptEngine());
                AddNewDocument(viewModel, ConsoleDocumentViewModel.ViewName);

                return viewModel;
            }
        }
        */

        public void AddNewChart() => AddNewChartDocument();
        public ChartDocumentViewModel AddNewChartDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("Chart");

                var viewModel = ChartDocumentViewModel.Create();
                AddNewDocument(viewModel, ChartDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewPivot() => AddNewPivotDocument();
        public PivotDocumentViewModel AddNewPivotDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("Pivot");

                var viewModel = PivotDocumentViewModel.Create();
                AddNewDocument(viewModel, PivotDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewPicture() => AddNewPictureDocument();
        public PictureDocumentViewModel AddNewPictureDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("Picture");

                var viewModel = PictureDocumentViewModel.Create();
                AddNewDocument(viewModel, PictureDocumentViewModel.ViewName);

                return viewModel;
            }
        }

        public void AddNewPdf() => AddNewPdfDocument();
        public PdfDocumentViewModel AddNewPdfDocument()
        {
            using (new UsingProcessor(() => StartAddingDocument(), () => EndAddingDocument()))
            {
                Utils.StartProfile("PDF");

                var viewModel = PdfDocumentViewModel.Create();
                AddNewDocument(viewModel, PdfDocumentViewModel.ViewName);

                return viewModel;
            }
        }
    }
}
