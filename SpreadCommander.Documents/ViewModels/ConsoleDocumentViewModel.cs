using System;
using System.Linq;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using SpreadCommander.Common.ScriptEngines;
using SpreadCommander.Documents.Services;
using System.Threading.Tasks;
using System.ComponentModel;
using DevExpress.XtraRichEdit.API.Native;
using System.Drawing;
using SpreadCommander.Common;
using System.Collections.Generic;
using SpreadCommander.Documents.Code;
using DevExpress.Spreadsheet;
using System.Data;
using SpreadCommander.Common.Code;
using DevExpress.XtraRichEdit;
using SpreadCommander.Documents.Console;
using SpreadCommander.Common.Spreadsheet;
using System.IO;

namespace SpreadCommander.Documents.ViewModels
{
    [POCOViewModel]
    public class ConsoleDocumentViewModel: BaseDocumentViewModel, ISpreadsheetHolder
    {
        #region ICallback
        public interface ICallback
        {
            void SetConsoleTitle(string title);
            void NeedScrollToCaret();
            void ProgressChanged(BaseScriptEngine.ProgressKind progressKind, int value, int max, string status);
            //Requires synchronization
            string RequestLine();

            IList<ModifiedFileItem> ListFiles();
            IList<ModifiedFileItem> ListModifiedFiles();
            void LoadFromFile(string fileName);
            void SaveToFile(string fileName);

            void ResetModifiedAll();
            void ResetModifiedBook();
            void ResetModifiedSpreadsheet();
            void ResetModifiedGrid();
        }
        #endregion

        public const string ViewName = "ConsoleDocument";

        public static readonly Guid CustomControlID = new ("{29E5AF72-C46B-4C5D-80A0-9CDB90F9B732}");

        public BaseScriptEngine Engine { get; private set; }

        public ConsoleDocumentViewModel(BaseScriptEngine engine)
        {
            Engine = engine;
            if (engine != null)
                InitializeEngine(Engine, true);

            //Call later from ConsoleDocument, when it is loaded
            //StartEngine();
        }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public override void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            Engine?.Dispose();
            Engine = null;

            base.Dispose();
        }

        public void SetupEngine(BaseScriptEngine engine)
        {
            if (Engine != null)
                throw new Exception("Engine is already initialized.");

            Engine = engine;
            InitializeEngine(Engine, true);
        }

        public static ConsoleDocumentViewModel Create(BaseScriptEngine engine) =>
            ViewModelSource.Create<ConsoleDocumentViewModel>(() => new ConsoleDocumentViewModel(engine));

        public override string DefaultExt        => Engine?.DefaultExt;
        public override string FileFilter        => Engine?.FileFilter;
        public override string DocumentType      => ViewName;
        public override string DocumentSubType   => Engine?.EngineName;

        public virtual Type[] CustomControlTypes { get; }

        public bool HasCustomControls => (CustomControlTypes?.Length ?? 0) > 0;

        protected ISaveFilesService SaveFilesService           => this.GetService<ISaveFilesService>();
        protected IApplicationService ApplicationService       => this.GetService<IApplicationService>();
        protected IOpenFileDialogService OpenFileDialogService => this.GetService<IOpenFileDialogService>();
        protected ISaveFileDialogService SaveFileDialogService => this.GetService<ISaveFileDialogService>();

        public ICallback Callback { get; set; }

        IWorkbook ISpreadsheetHolder.Workbook => Workbook;
        
        public IRichEditDocumentServer BookServer
        {
            get => Engine?.BookServer;
            set => Engine.BookServer = value;
        }

        public Document Document => BookServer?.Document;

        public IWorkbook Workbook
        {
            get => Engine?.Workbook;
            set => Engine.Workbook = value;
        }

        public DataSet GridDataSet
        {
            get => Engine?.GridDataSet;
            set => Engine.GridDataSet = value;
        }
        
        public IFileViewer FileViewer
        {
            get => Engine?.FileViewer;
            set => Engine.FileViewer = value;
        }

        public ISynchronizeInvoke SynchronizeInvoke
        {
            get => Engine?.SynchronizeInvoke;
            set => Engine.SynchronizeInvoke = value;
        }

        public override void Save()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                FileName = AskForFileName();

            if (string.IsNullOrWhiteSpace(FileName))
                return;

            SaveToFile(FileName);
        }

        protected void UninitializeEngine(BaseScriptEngine engine, bool mainScriptEngine)
        {
            if (mainScriptEngine)
            {
                engine.TitleChanged      -= Engine_TitleChanged;
                engine.NeedScrollToCaret -= Engine_NeedScrollToCaret;
                engine.ProgressChanged   -= Engine_ProgressChanged;
            }
            engine.RequestLine       -= Engine_RequestLine;
            engine.ViewFileRequest   -= Engine_ViewFileRequest;
        }

        protected void InitializeEngine(BaseScriptEngine engine, bool mainScriptEngine)
        {
            if (mainScriptEngine)
            {
                engine.TitleChanged      += Engine_TitleChanged;
                engine.NeedScrollToCaret += Engine_NeedScrollToCaret;
                engine.ProgressChanged   += Engine_ProgressChanged;
            }
            engine.RequestLine       += Engine_RequestLine;
            engine.ViewFileRequest   += Engine_ViewFileRequest;
        }

        public void RecreateEngine()
        {
            var oldEngine = Engine;
            oldEngine.Stop();
            UninitializeEngine(oldEngine, true);
            Engine = null;
            
            //Clone script engine. Execute script in "clear" script engine and do not affect existing script.
            var scriptEngine = Activator.CreateInstance(oldEngine.GetType()) as BaseScriptEngine;
            scriptEngine.SynchronizeInvoke = oldEngine.SynchronizeInvoke;
            InitializeEngine(scriptEngine, true);

            scriptEngine.BookServer  = oldEngine.BookServer;
            scriptEngine.Workbook    = oldEngine.Workbook;
            scriptEngine.GridDataSet = oldEngine.GridDataSet;

            oldEngine.Dispose();

            scriptEngine.Start();

            Engine = scriptEngine;
        }

        public void StartEngine()
        {
            Task.Run((() => 
            {
                try
                {
                    Engine.Start();

                    SCDispatcherService.BeginInvoke(() =>
                    {
                        Callback?.ResetModifiedAll();
                    });
                }
                catch (Exception ex)
                {
                    Engine.ScriptOutputAvailable(BaseScriptEngine.ScriptOutputType.Error, 
                        $"Cannot start script engine: {ex.Message}{Environment.NewLine}", 
                        Color.Red, SystemColors.Window);
                }
            }));
        }

        private void Engine_ViewFileRequest(object sender, BaseScriptEngine.ViewFileRequestArgs e)
        {
            Utils.StartProfile("Viewer");

            SCDispatcherService.BeginInvoke(() =>
            {
                var viewModel = ViewerDocumentViewModel.Create();
                AddNewDocument(viewModel, ViewerDocumentViewModel.ViewName);
                viewModel.LoadFile(e.FileName, e.Parameters, e.Commands);
            });
        }

        public override void OnClose(CancelEventArgs e)
        {
            base.OnClose(e);

            var engine = Engine;

            if (!e.Cancel && engine != null)
            {
                engine.Stop();
                engine.ViewFileRequest -= Engine_ViewFileRequest;

                Engine = null;
            }
        }

        protected virtual void CheckScriptEngine()
        {
            if (Engine == null)
                throw new InvalidOperationException("Script engine is not initialized.");
        }

        public async void Execute(string command)
        {
            CheckScriptEngine();

            await Task.Run(() =>
            {
                Engine.SendCommand(command);
            });
        }

        public async void Cancel()
        {
            await Task.Run(() =>
            {
                Engine.Stop();
            });
            StartEngine();
        }

        public async void ExecuteScript(string script)
        {
            CheckScriptEngine();

            ClearAllOutput();

            //Clone script engine. Execute script in "clear" script engine and do not affect existing script.
            var scriptEngine = Activator.CreateInstance(Engine.GetType()) as BaseScriptEngine;
            scriptEngine.SynchronizeInvoke = Engine.SynchronizeInvoke;
            InitializeEngine(scriptEngine, false);

            scriptEngine.BookServer  = Engine.BookServer;
            scriptEngine.Workbook    = Engine.Workbook;
            scriptEngine.GridDataSet = Engine.GridDataSet;

            scriptEngine.ExecutionType = BaseScriptEngine.ScriptExecutionType.Script;

            scriptEngine.ExecutionFinished += callbackFinished;

            await Task.Run(() =>
            {
                scriptEngine.Start();
                scriptEngine.SendCommand(script);
            });


            void callbackFinished(object s, EventArgs e)
            {
                scriptEngine.ExecutionFinished -= callbackFinished;
                scriptEngine.Dispose();

                GC.Collect();
            }
        }

        private void Engine_TitleChanged(object sender, EventArgs e)
        {
            SCDispatcherService.BeginInvoke(() =>
            {
                Callback?.SetConsoleTitle(Engine?.Title ?? "Console");
            });
        }

        private void Engine_NeedScrollToCaret(object sender, EventArgs e)
        {
            SCDispatcherService.BeginInvoke(() =>
            {
                Callback?.NeedScrollToCaret();
            });
        }

        private void Engine_ProgressChanged(object sender, EventArgs e)
        {
            SCDispatcherService.BeginInvoke(() =>
            {
                var engine = Engine;
                Callback?.ProgressChanged(engine.ProgressType, engine.ProgressValue, engine.ProgressMax, engine.ProgressStatus);
            });			
        }

        private void Engine_RequestLine(object sender, BaseScriptEngine.ReadLineArgs e)
        {
            e.Line = Callback?.RequestLine();
        }
        
        protected virtual void AddCustomControlModifiedFile(IList<ModifiedFileItem> files)
        {
            if (files.Where(f => (string.Compare(f.FileName, FileName, true) == 0)).FirstOrDefault() == null)
            {
                var file = new ModifiedFileItem()
                {
                    FileName = this.FileName,
                    Title    = Path.GetFileName(this.FileName),
                    Tag      = CustomControlID
                };
                files.Insert(0, file);
            }           
        }

        public virtual string GetScriptFileName(string fileName)
        {
            return fileName;
        }

        protected virtual void LoadScriptFile(string fileName)
        {
            var engine = Engine;
            if (engine == null && !string.IsNullOrWhiteSpace(fileName))
            {
                var ext = Path.GetExtension(fileName)?.ToLower();
                switch (ext)
                {
                    case ".ps1":
                    case ".ps":
                    case ".psm1":
                    case ".psd1":
                        SetupEngine(new PowerShellScriptEngine());
                        break;
                    case ".fsx":
                    case ".fs":
                        SetupEngine(new FSharpScriptEngine());
                        break;
                }
            }

            Callback?.LoadFromFile(fileName);
        }

        //If fileName == null - clear custom controls
        public virtual void LoadCustomControlsFromFile(string fileName)
        {
            //Do nothing
        }

        public override void LoadFromFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);

            if (Engine == null)
            {
                string testFileName = $"{fileName}.ps1";
                if (File.Exists(testFileName))
                    Engine = new PowerShellScriptEngine();
                else
                {
                    testFileName = $"{fileName}.fsx";
                    if (File.Exists(testFileName))
                        Engine = new FSharpScriptEngine();
                }

                if (Engine != null)
                    InitializeEngine(Engine, true);
            }

            var scriptFileName = GetScriptFileName(fileName);
            if (scriptFileName != null)
                LoadScriptFile(scriptFileName);

            if (string.Compare(scriptFileName, fileName, true) != 0)
                LoadCustomControlsFromFile(fileName);
            else
                LoadCustomControlsFromFile(null);

            base.LoadFromFile(fileName);
        }

        public void FileLoaded(string fileName)
        {
            base.LoadFromFile(fileName);
        }

        public virtual bool SaveCustomControlsToFile()
        {
            return false;
        }

        public virtual void SaveCustomControlsToFile(string fileName)
        {
            //Do nothing
        }

        public override void SaveToFile(string fileName)
        {
            fileName = Project.Current.MapPath(fileName);

            var scriptFileName = GetScriptFileName(fileName);
            if (scriptFileName != null)
                Callback.SaveToFile(scriptFileName);

            if (string.Compare(scriptFileName, fileName, true) != 0)
                SaveCustomControlsToFile(fileName);

            base.SaveToFile(fileName);
        }

        public void FileSaved(string fileName)
        {
            base.SaveToFile(fileName);
        }

        public virtual void ListScriptIntellisenseItems(string text, string[] lines, Point caretPosition, ScriptIntellisense intellisense)
        {
            Engine?.ListScriptIntellisenseItems(FileName, text, lines, caretPosition, intellisense);
        }

        public virtual void ParseScriptErrors(string text, List<ScriptParseError> errors)
        {
            Engine?.ListScriptParseErrors(text, errors);
        }

        public virtual void ClearBook()
        {
            var doc = Document;
            if (doc == null)
                return;

            doc.Text = string.Empty;

            Callback?.ResetModifiedBook();
        }

        public virtual void ClearSpreadsheet()
        {
            var workbook = Workbook;
            if (workbook == null)
                return;

            workbook.Modified = false;
            workbook.CreateNewDocument();

            Callback?.ResetModifiedSpreadsheet();
        }

        public virtual void ClearGrid()
        {
            var dataSet = GridDataSet;
            if (dataSet == null)
                return;

            dataSet.Clear();
            dataSet.Reset();

            Callback?.ResetModifiedGrid();
        }

        public virtual void ClearAllOutput()
        {
            ClearBook();
            ClearSpreadsheet();
            ClearGrid();

            Callback?.ResetModifiedAll();
        }

        public virtual void InitializeCustomControls()
        {
            //Do nothing
        }

        public void ExportToSpreadsheet(object dataSource, GridData gridData = null)
        {
            var workbook = Workbook;
            if (workbook == null)
            {
                MessageService.ShowMessage("Cannot determine target workbook.", "No target workbook", MessageButton.OK, MessageIcon.Error);
                return;
            }
            
            var table = SpreadsheetUtils.AppendDataSource(workbook, dataSource);
            if (gridData != null)
                SpreadsheetUtils.ApplyGridFormatting(table, gridData);
            else
                SpreadsheetUtils.FinishTableFormatting(table);
        }

        public void MergeSpreadsheet(IWorkbook mergeWorkbook)
        {
            var workbook = Workbook;
            if (workbook == null)
            {
                MessageService.ShowMessage("Cannot determine target workbook.", "No target workbook", MessageButton.OK, MessageIcon.Error);
                return;
            }

            workbook.Append(mergeWorkbook);
        }
    }
}