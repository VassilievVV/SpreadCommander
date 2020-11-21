using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SpreadCommander.Common.ScriptEngines;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.API.Native;
using Alsing.Windows.Forms.Document;
using System.IO;
using System.Reflection;
using SpreadCommander.Common;
using DevExpress.LookAndFeel;
using DevExpress.XtraRichEdit;
using Alsing.Windows.Forms.SyntaxBox;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using Tabbed = DevExpress.XtraBars.Docking2010.Views.Tabbed;
using SpreadCommander.Documents.ViewModels;
using DevExpress.Utils;
using DevExpress.Mvvm;
using SpreadCommander.Common.Messages;
using SpreadCommander.Documents.Messages;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleOutputControl : DevExpress.XtraEditors.XtraUserControl
    {
        private ConsoleBookControl Book					{ get; set; }
        private ConsoleSpreadsheetControl Spreadsheet	{ get; set; }
        private ConsoleGridControl Grid					{ get; set; }
        private ConsoleHeapControl Heap					{ get; set; }
        private ConsoleCustomControl[] CustomControls   { get; set; }

        private bool _StartWithGrid;

        public event EventHandler<RibbonUpdateRequestEventArgs> RibbonUpdateRequest;

        private Type[] CustomControlTypes { get; }

        public event EventHandler ConsoleLoaded;

        private const string CustomControlName = "CustomControl";

        public ConsoleOutputControl()
        {
            InitializeComponent();

            _DataSet = new DataSet("Data");
        }

        public ConsoleOutputControl(BaseDocumentViewModel model): this()
        {
            ViewModel = model;
            
            if (model is ConsoleDocumentViewModel consoleViewModel)
                CustomControlTypes = consoleViewModel.CustomControlTypes;
        }

        private void ConsoleOutputControl_Load(object sender, EventArgs e)
        {
            CreateCustomControls();
            
            //Force loading of all documents
            var documents = new List<BaseDocument>(viewDocuments.Documents);
            var activeDocument = viewDocuments.ActiveDocument;

            using (new UsingProcessor(() => viewDocuments.BeginUpdate(), () => viewDocuments.EndUpdate()))
            {
                Utils.StartProfile("ConsoleOutput");

                foreach (var document in documents)
                {
                    if (document.Control == null)
                        viewDocuments.Controller.Activate(document);
                }

                if (activeDocument == null)
                    activeDocument = documentManager.GetDocument(_StartWithGrid ? (Control)Grid : Book);

                if (activeDocument != null)
                    viewDocuments.Controller.Activate(activeDocument);
            }

            if (CustomControls != null && CustomControls.Length > 0 && CustomControls[0] != null)
                viewDocuments.ActivateDocument(CustomControls[0]);
            
            ResetModifiedAll();

            ConsoleLoaded?.Invoke(this, new EventArgs());
        }

        public void ConsoleDocumentLoaded()
        {
            if (ViewModel is ConsoleDocumentViewModel consoleViewModel)
                consoleViewModel?.InitializeCustomControls();
        }
        
        private void CreateCustomControls()
        {
            if (CustomControlTypes == null)
                return;

            var controller = viewDocuments.Controller;
            int counter = 0;

            for (int i = 0; i < CustomControlTypes.Length; i++)
            {
                var docCustom                   = viewDocuments.AddDocument(CustomControlName, $"{CustomControlName}{counter}") as Tabbed.Document;
                docCustom.Properties.AllowClose = DefaultBoolean.False;
                controller.Dock(docCustom, documentGroupMain, counter++);
            }

            controller.Dock(docGrid, documentGroupMain, counter);
            CustomControls = new ConsoleCustomControl[CustomControlTypes.Length];

            ResetModifiedAll();
        }
        
        public void SetSqlMode()
        {
            var controller = viewDocuments.Controller;
            controller.Dock(docGrid, documentGroupMain, 0);
            controller.Dock(docSpreadsheet, documentGroupMain, 1);
            controller.Activate(docGrid);
            _StartWithGrid = true;
        }

        public RichEditControl Editor => Book?.Editor;
        public Document Document      => Book?.Document;
        public IWorkbook Workbook     => Spreadsheet?.Document;
        public IFileViewer FileViewer => Heap;

        private DataSet _DataSet;
        public DataSet DataSet
        {
            get => _DataSet;
            set
            {
                _DataSet = value;

                if (Grid != null)
                    Grid.DataSet = value;

                if (CustomControls != null)
                {
                    foreach (var customControl in CustomControls)
                        if (customControl != null)
                            customControl.DataSet = value;
                }
            }
        }

        private BaseDocumentViewModel _ViewModel;
        public BaseDocumentViewModel ViewModel
        {
            get => _ViewModel;
            set
            {
                if (_ViewModel == value)
                    return;

                _ViewModel = value;

                if (Book != null)
                    Book.ViewModel        = _ViewModel;
                if (Spreadsheet != null)
                    Spreadsheet.ViewModel = _ViewModel;
                if (Grid != null)
                    Grid.ViewModel        = _ViewModel;
                if (Heap != null)
                    Heap.ViewModel        = _ViewModel;

                if (CustomControls != null)
                {
                    foreach (var control in CustomControls)
                    {
                        if (control != null)
                            control.ViewModel = _ViewModel;
                    }
                }
            }
        }

        public void AddOutput(BaseScriptEngine.ScriptOutputMessage args)
        {
            Document?.AppendText(args.Text);
            Document?.AppendText("\n");
        }

        public void ScrollToCaret()
        {
            Editor?.ScrollToCaret();
        }

        public void ProgressChanged(BaseScriptEngine.ProgressKind progressKind, int value, int max, string status)
        {
            if (value < 0)
                value = 0;
            if (max < value)
                max = value;

            switch (progressKind)
            {
                case BaseScriptEngine.ProgressKind.None:
                    layoutControlProgressMarquee.Visibility = LayoutVisibility.Always;  //Show marquee progress bar when there is no progressing
                    layoutControlProgressBar.Visibility     = LayoutVisibility.Never;
                    progressMarqueeBar.Properties.Stopped   = true;
                    progressMarqueeBar.ToolTip              = string.Empty;
                    progressBar.ToolTip                     = string.Empty;
                    break;
                case BaseScriptEngine.ProgressKind.Undetermined:
                    layoutControlProgressBar.Visibility     = LayoutVisibility.Never;
                    layoutControlProgressMarquee.Visibility = LayoutVisibility.Always;
                    progressMarqueeBar.Properties.Stopped   = false;
                    progressMarqueeBar.ToolTip              = status;
                    progressBar.ToolTip                     = string.Empty;
                    break;
                case BaseScriptEngine.ProgressKind.Value:
                    layoutControlProgressMarquee.Visibility = LayoutVisibility.Never;
                    layoutControlProgressBar.Visibility     = LayoutVisibility.Always;
                    progressMarqueeBar.Properties.Stopped   = true;
                    progressBar.Properties.Maximum          = value;
                    progressMarqueeBar.ToolTip              = string.Empty;
                    progressBar.ToolTip                     = status;

                    progressBar.Properties.Maximum          = max;
                    progressBar.EditValue                   = value;
                    break;
            }
        }

        public void ResetModifiedAll()
        {
            ResetModifiedBook();
            ResetModifiedSpreadsheet();
            ResetModifiedGrid();
            ResetModifiedHeap();
            ResetModifiedCustomControls();
        }

        public void ResetModifiedBook()          => ResetControlModified(Book);
        public void ResetModifiedSpreadsheet()   => ResetControlModified(Spreadsheet);
        public void ResetModifiedGrid()          => ResetControlModified(Grid);
        public void ResetModifiedHeap()          => ResetControlModified(Heap);

        public void ResetModifiedCustomControls()
        {
            if (CustomControls != null)
                    foreach (var customControl in CustomControls)
                        ResetControlModified(customControl);
        }

        private Control FindParentDocument()
        {
            Control result = Parent;
            while (result != null && !(result is Form))
                result = result.Parent;
            return result;
        }
        
        private void NotifyDocumentModified(bool modified)
        {
            var parentDocument = FindParentDocument();
            if (parentDocument != null)
                Messenger.Default.Send(new ControlModifiedMessage() { Control = parentDocument, Modified = modified });
        }
        
        private void UpdateControlModified(Control control)
        {
            if (control == null)
                return;
            
            var document = viewDocuments.Documents.Where(doc => doc.Control == control).FirstOrDefault();
            if (document != null && document != viewDocuments.ActiveDocument &&
                document is DevExpress.XtraBars.Docking2010.Views.Tabbed.Document tabDocument)
            { 
                var appearance       = tabDocument.Appearance.Header;
                appearance.BackColor = SystemColors.Info;
                appearance.ForeColor = SystemColors.InfoText;

                NotifyDocumentModified(true);
            }
        }

        private void ResetControlModified(Control control)
        {
            if (control == null)
                return;
            
            var document = viewDocuments.Documents.Where(doc => doc.Control == control).FirstOrDefault();
            if (document != null)
                ResetDocumentModified(document);
        }

        private static void ResetDocumentModified(BaseDocument document)
        {
            if (document is DevExpress.XtraBars.Docking2010.Views.Tabbed.Document tabDocument)
            { 
                var appearance = tabDocument.Appearance.Header;
                appearance.Reset();
            }
        }

        protected virtual ConsoleCustomControl CreateControlCustomControl(Type customControlType) =>
            Activator.CreateInstance(customControlType) as ConsoleCustomControl;

        private void ViewDocuments_QueryControl(object sender, QueryControlEventArgs e)
        {
            var controlName = e.Document?.ControlName;

            if (string.IsNullOrWhiteSpace(controlName))
                return;

            var viewModel = ViewModel;

            switch (controlName)
            {
                case "Book":
                    Book = new ConsoleBookControl()
                    {
                        ViewModel = viewModel
                    };
                    e.Control = Book;

                    RibbonUpdateRequest?.Invoke(this, new RibbonUpdateRequestEventArgs() { RibbonHolder = Book as IRibbonHolder });

                    Book.Modified += Book_Modified;
                    break;
                case "Spreadsheet":
                    Spreadsheet = new ConsoleSpreadsheetControl()
                    {
                        ViewModel = viewModel
                    };
                    e.Control = Spreadsheet;

                    Spreadsheet.Modified += Spreadsheet_Modified;
                    break;
                case "Grid":
                    Grid = new ConsoleGridControl()
                    {
                        DataSet   = this.DataSet,
                        ViewModel = viewModel
                    };
                    e.Control = Grid;

                    Grid.Modified += Grid_Modified;
                    Grid.RibbonUpdateRequest += Grid_RibbonUpdateRequest;
                    break;
                case "Heap":
                    Heap = new ConsoleHeapControl()
                    {
                        ViewModel = viewModel
                    };
                    e.Control = Heap;

                    Heap.Modified += Heap_ModifiedChanged;
                    break;
            }

            if (controlName.StartsWith(CustomControlName))
            {
                var index = int.Parse(controlName[CustomControlName.Length..]);

                var customControlType = CustomControlTypes[index];
                var customControl = CreateControlCustomControl(customControlType) ??
                    throw new Exception("Invalid custom control type in console output.");
                customControl.DataSet              = this.DataSet;
                customControl.ViewModel            = ViewModel;
                customControl.Modified            += CustomControl_Modified;
                customControl.RibbonUpdateRequest += CustomControl_RibbonUpdateRequest;

                e.Control = customControl;

                e.Document.Caption                   = customControl.Caption;
                e.Document.ImageOptions.SvgImage     = customControl.CaptionSvgImage;
                e.Document.ImageOptions.SvgImageSize = new Size(16, 16);
                e.Document.Properties.AllowClose     = DefaultBoolean.False;

                CustomControls[index] = customControl;
            }
        }

        private void Grid_RibbonUpdateRequest(object sender, RibbonUpdateRequestEventArgs e)
        {
            if (viewDocuments.ActiveDocument == docGrid)
                RequestRibbonUpdate(docGrid);
        }

        private void CustomControl_RibbonUpdateRequest(object sender, RibbonUpdateRequestEventArgs e)
        {
            var document = viewDocuments.Documents.Where(doc => doc.Control == sender).FirstOrDefault();
            if (document != null)
                RequestRibbonUpdate(document);
        }

        private void Book_Modified(object sender, EventArgs e)
        {
            UpdateControlModified(Book);
        }

        private void Spreadsheet_Modified(object sender, EventArgs e)
        {
            UpdateControlModified(Spreadsheet);
        }

        private void Grid_Modified(object sender, EventArgs e)
        {
            UpdateControlModified(Grid);
        }

        private void Heap_ModifiedChanged(object sender, EventArgs e)
        {
            UpdateControlModified(Heap);
        }

        private void CustomControl_Modified(object sender, EventArgs e)
        {
            UpdateControlModified(sender as Control);
        }

        private void ViewDocuments_TabMouseActivating(object sender, DocumentCancelEventArgs e)
        {
            if (e.Document != viewDocuments.ActiveDocument)
                transitionManager.StartTransition(panelConsole);
        }

        private async void ViewDocuments_DocumentActivated(object sender, DocumentEventArgs e)
        {
            if (transitionManager.IsTransition)
            {
                //Allow some time for transition
                await Task.Yield();
                transitionManager.EndTransition();
            }

            ResetDocumentModified(e.Document);
        }

        private void ViewDocuments_DocumentDeactivated(object sender, DocumentEventArgs e)
        {
            RequestRibbonUpdate(null);

            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = e.Document?.IsFloating ?? false;
        }

        private void RequestRibbonUpdate(BaseDocument document)
        {
            RibbonUpdateRequest?.Invoke(this, new RibbonUpdateRequestEventArgs() { RibbonHolder = document?.Control as IRibbonHolder, IsFloating = document?.IsFloating ?? false });
        }

        private void DocumentManager_DocumentActivate(object sender, DocumentEventArgs e)
        {
            RequestRibbonUpdate(e.Document);

            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = e.Document?.IsFloating ?? false;
        }

        private void ViewDocuments_Floating(object sender, DocumentEventArgs e)
        {
            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = true;
        }

        private void ViewDocuments_EndFloating(object sender, DocumentEventArgs e)
        {
            if (e.Document?.Control is IRibbonHolder ribbonHolder)
                ribbonHolder.IsRibbonVisible = false;
        }

        private void ViewDocuments_EndDocking(object sender, DocumentEventArgs e)
        {
            if (e.Document?.Control is IRibbonHolder ribbonHolder)
            {
                ribbonHolder.IsRibbonVisible = false;
                RequestRibbonUpdate(e.Document);
            }
        }
    }
}
