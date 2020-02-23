using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Animation;
using DevExpress.Utils.MVVM.Services;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using SpreadCommander.Common;
using SpreadCommander.Common.SqlScript;
using SpreadCommander.Documents.Code;
using SpreadCommander.Documents.Services;
using SpreadCommander.Documents.Viewers;
using SpreadCommander.Documents.ViewModels;
using SpreadCommander.Documents.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Code = SpreadCommander.Documents.Code;
using SpreadCommander.Documents.Extensions;
using DevExpress.XtraTreeList.Nodes;
using System.Diagnostics;
using DevExpress.XtraBars.Navigation;
using SpreadCommander.Documents.Console;
using DevExpress.XtraBars.Ribbon;
using DevExpress.Skins;
using DevExpress.XtraBars.Ribbon.Gallery;
using DevExpress.XtraBars.Helpers;
using SpreadCommander.Common.ScriptEngines;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common.Grid.Functions;
using DevExpress.XtraBars.Docking2010.Views;
using SpreadCommander.Common.Messages;
using DevExpress.Utils.Svg;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.Utils.Design;
using System.Linq.Expressions;

namespace SpreadCommander
{
    public partial class MainView : DevExpress.XtraBars.Ribbon.RibbonForm, MainViewModel.ICallback, IRibbonHolder
    {
        #region AppMenuProjectItem
        public class AppMenuProjectItem
        {
            public int ImageIndex           { get; set; }
            public string ItemName          { get; set; }
            public string DocumentType      { get; set; }
            public string DocumentSubType   { get; set; }
        }
        #endregion

        public MainView()
        {
            //Load skins before InitializeComponent(), to allow skinned splash screen.
            LoadStartupSkin();

            InitializeComponent();

            //Repeat from Program.cs
            WindowsFormsSettings.SetDPIAware();

            //Do not force DirectX on Windows 7
            if (Environment.OSVersion.Version.Major >= 10)
                WindowsFormsSettings.ForceDirectXPaint();
            else
                WindowsFormsSettings.ForceGDIPlusPaint();

            BaseForm.ApplicationIcon = this.Icon;
            this.AdjustSizeForMonitor();

            navProject.State           = NavigationPaneState.Collapsed;

            UIUtils.ConfigureRibbonBar(Ribbon);
            Ribbon.CommandLayout = (CommandLayout)(int)ApplicationSettings.Default.RibbonCommandLayout;

            InitializeAppMenuItems();
            InitializeSkinGallery();

            var skinName = UserLookAndFeel.Default.ActiveSkinName;
            var isSkinVector = UIUtils.IsSkinVector(skinName);
            skinPaletteRibbonGalleryBarItem.Visibility = isSkinVector ? BarItemVisibility.Always : BarItemVisibility.Never;

            UserLookAndFeel.Default.StyleChanged += LookAndFeel_StyleChanged;
            ((DevExpress.LookAndFeel.Design.UserLookAndFeelDefault)UserLookAndFeel.Default).StyleChangeProgress += LookAndFeel_StyleChangeProgress;

            PowerShellScriptEngine.RefreshAvailableCommandLets();

            GridFunctionFactory.RegisterFunctions();
        }

        RibbonControl IRibbonHolder.Ribbon            => Ribbon;
        RibbonStatusBar IRibbonHolder.RibbonStatusBar => ribbonStatusBar;
        bool IRibbonHolder.IsRibbonVisible
        {
            get => Ribbon.Visible;
            set { }	//Do nothing, do not allow make ribbon invisible
        }

        private void InitializeBindings()
        {
            var fluent = mvvmContext.OfType<MainViewModel>();
            fluent.ViewModel.InitializeBindings();

            fluent.ViewModel.Callback = this;

            mvvmContext.RegisterService(MainViewModel.DocumentServiceKey, DocumentManagerService.Create(viewDocuments));
            mvvmContext.RegisterService(MainViewModel.FloatDocumentServiceKey, WindowedDocumentManagerService.Create(viewDocuments));
            mvvmContext.RegisterDefaultService(ViewLocator.DefaultLocator);
            mvvmContext.RegisterDefaultService(new AlertService(this));
            mvvmContext.RegisterDefaultService(new DbConnectionEditorService(this));
            mvvmContext.RegisterDefaultService(new ExportTablesService(this));
            mvvmContext.RegisterDefaultService(new SaveFilesService(this));
            mvvmContext.RegisterDefaultService(new ApplicationService());
            mvvmContext.RegisterDefaultService(new SpreadsheetTemplateService(this));
            mvvmContext.RegisterDefaultService(new BookTemplateService(this));
            mvvmContext.RegisterDefaultService(new ImportExportSpreadSheetsService(this));
            mvvmContext.RegisterDefaultService(new EditObjectService(this));
            mvvmContext.RegisterDefaultService(new SCDispatcherService());
            mvvmContext.RegisterDefaultService(new SelectProjectService(this));
            mvvmContext.RegisterDefaultService(new ViewRichTextService(this));

            var folderBrowserOptions = new FolderBrowserDialogServiceOptions()
            {
                Description                        = Parameters.ApplicationName,
                RestorePreviouslySelectedDirectory = DefaultBoolean.True,
                ShowNewFolderButton                = DefaultBoolean.True
            };
            mvvmContext.RegisterDefaultService(FolderBrowserDialogService.Create(this, folderBrowserOptions, DevExpress.Utils.CommonDialogs.FolderBrowserStyle.SkinnableWide));

            var openFileOptions = new OpenFileDialogServiceOptions()
            {
                Title = Parameters.ApplicationName
            };
            mvvmContext.RegisterDefaultService(OpenFileDialogService.Create(this, openFileOptions, DevExpress.Utils.CommonDialogs.FileBrowserStyle.Skinnable));

            var saveFileOptions = new SaveFileDialogServiceOptions()
            {
                Title           = Parameters.ApplicationName,
                OverwritePrompt = DefaultBoolean.True
            };
            mvvmContext.RegisterDefaultService(SaveFileDialogService.Create(this, saveFileOptions, DevExpress.Utils.CommonDialogs.FileBrowserStyle.Skinnable));

            fluent.ViewModel.InitializeServices();

            fluent.BindCommand(barNewProject, m => m.NewProject());
            fluent.BindCommand(barOpenProject, m => m.OpenProject());
            fluent.BindCommand(barSelectProject, m => m.SelectProject());
            fluent.BindCommand(barNewSpreadsheetDocument, m => m.AddNewSpreadsheet());
            fluent.BindCommand(barNewDashboardDocument, m => m.AddNewDashboard());
            fluent.BindCommand(barNewBookDocument, m => m.AddNewBook());
            fluent.BindCommand(barNewSqlScriptDocument, m => m.AddNewSqlScript());
            fluent.BindCommand(barNewRScriptDocument, m => m.AddNewRScript());
            fluent.BindCommand(barNewPyScriptDocument, m => m.AddNewPyScript());
            fluent.BindCommand(barNewPSScriptDocument, m => m.AddNewPSScript());
            //fluent.BindCommand(barNewCSharpScriptDocument, m => m.AddNewCSharpScript());
            //fluent.BindCommand(barNewFSharpScriptDocument, m => m.AddNewFSharpScript());
            fluent.BindCommand(barNewChartDocument, m => m.AddNewChart());
            fluent.BindCommand(barNewPivotDocument, m => m.AddNewPivot());
            fluent.BindCommand(barOpenFile, m => m.OpenFile());
            fluent.BindCommand(barOptions, m => m.EditApplicationSettings());
            fluent.BindCommand(barSaveAll, m => m.SaveAllDocuments());
            fluent.BindCommand(barAbout, m => m.ShowAbout());
        }

        private void InitializeAppMenuItems()
        {
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "Book", ImageIndex = 0, DocumentType = BookDocumentViewModel.ViewName });
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "Spread\r\nsheet", ImageIndex = 1, DocumentType =  SpreadsheetDocumentViewModel.ViewName});
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "SQL", ImageIndex = 2, DocumentType = SqlScriptDocumentViewModel.ViewName });
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "Power\r\nShell", ImageIndex = 3, DocumentType = ConsoleDocumentViewModel.ViewName, DocumentSubType = PowerShellScriptEngine.ScriptEngineName });
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "R", ImageIndex = 4, DocumentType = ConsoleDocumentViewModel.ViewName, DocumentSubType = RScriptEngine.ScriptEngineName });
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "Python", ImageIndex = 5, DocumentType = ConsoleDocumentViewModel.ViewName, DocumentSubType = PythonScriptEngine.ScriptEngineName });
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "Chart", ImageIndex = 6, DocumentType =  ChartDocumentViewModel.ViewName});
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "Pivot", ImageIndex = 7, DocumentType =  PivotDocumentViewModel.ViewName});
            bindingAppMenuProjectItems.Add(new AppMenuProjectItem() { ItemName = "Dash\r\nboard", ImageIndex = 8, DocumentType = DashboardDocumentViewModel.ViewName });
        }

        //Unlike other forms, initialize model in _Load even handler
        private void MainView_Load(object sender, EventArgs e)
        {
            if (!mvvmContext.IsDesignMode)
                InitializeBindings();

            ProjectChanged();
            LoadConnections();

            Task.Run(() =>
            {
                //Let main form to display
                Thread.Sleep(200);

                Invoke((MethodInvoker)(() =>
                {
                    Application.DoEvents();
                    SelectProject();
                }));
            });
        }

        //Model's OnClose is not called automatically
        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;	//Do not save documents if Windows is shutting down.

            var fluent = mvvmContext.OfType<MainViewModel>();

            var cancelArgs = new CancelEventArgs(e.Cancel);
            fluent.ViewModel.OnClose(cancelArgs);
            e.Cancel = cancelArgs.Cancel;

            CloseCurrentViewer();

            ApplicationVisualState.Default.SaveSettings();

            GridFunctionFactory.UnregisterFunctions();
        }

        //This even can be removed and program will continue to work. 
        //But this way "Please wait" dialog is shown automatically.
        private void ViewDocuments_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            e.Control = ViewLocator.DefaultLocator.Resolve(e.Document.ControlName) as Control;
            if (e.Control is IImageHolder imageHolder)
            {
                e.Document.ImageOptions.SvgImage     = imageHolder.GetControlImage();
                e.Document.ImageOptions.SvgImageSize = new Size(16, 16);
            }
        }

        private void InitializeSkinGallery()
        {
            var gallery = ((skinDropDownButtonItem.DropDownControl as SkinPopupControlContainer)?.Controls?.OfType<GalleryControl>()?.FirstOrDefault())?.Gallery;
            if (gallery != null)
            {
                foreach (GalleryItemGroup galleryGroup in gallery.Groups)
                {
                    foreach (GalleryItem galleryItem in galleryGroup.Items)
                        galleryItem.ItemClick += SkinGallery_ItemClick;
                }
            }
        }

        private void SkinGallery_ItemClick(object sender, GalleryItemClickEventArgs e)
        {
            Utils.StartProfile("Skins");

            if (!transitionManager.IsTransition)
                transitionManager.StartTransition(this);
        }

        private void SkinPaletteRibbonGalleryBarItem_GalleryItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            Utils.StartProfile("Skins");

            if (!transitionManager.IsTransition)
                transitionManager.StartTransition(this);
        }

        private void LookAndFeel_StyleChangeProgress(object sender, LookAndFeelProgressEventArgs e)
        {
            if (e.IsBegin && !transitionManager.IsTransition)
                transitionManager.StartTransition(this);
        }

        private void LookAndFeel_StyleChanged(object sender, EventArgs e)
        {
            var skinName     = UserLookAndFeel.Default.ActiveSkinName;
            var isSkinVector = UIUtils.IsSkinVector(skinName);
            skinPaletteRibbonGalleryBarItem.Visibility = isSkinVector ? BarItemVisibility.Always : BarItemVisibility.Never;

            if (isSkinVector && !string.IsNullOrWhiteSpace(UserLookAndFeel.Default.ActiveSvgPaletteName))
                skinName += '|' + UserLookAndFeel.Default.ActiveSvgPaletteName;

            try
            {
                ApplicationVisualState.Default.SkinName = skinName;
                ApplicationVisualState.Default.SaveSettings();
            }
            catch (Exception)
            {
                //Do nothing
            }

            if (transitionManager.IsTransition)
                transitionManager.EndTransition();
        }

        private static void LoadStartupSkin()
        {
            try
            {
                var skinName = ApplicationVisualState.Default.SkinName;
                if (!string.IsNullOrWhiteSpace(skinName))
                {
                    var parts = skinName.Split(new char[] { '|' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 1)
                        UserLookAndFeel.Default.SetSkinStyle(parts[0]);
                    else if (parts.Length == 2)
                        UserLookAndFeel.Default.SetSkinStyle(parts[0], parts[1]);
                }
                else
                    UserLookAndFeel.Default.SetSkinStyle(SkinSvgPalette.Bezier.Default);
            }
            catch (Exception)
            {
                UserLookAndFeel.Default.SetSkinStyle(SkinSvgPalette.Bezier.Default);
            }
        }

        public void ProjectChanged()
        {
            Text = $"SpreadCommander - {Project.Current.Name}";
            
            ReloadProjectFiles();
            UpdateRecentProjects();
        }

        public void ReloadProjectFiles()
        {
            treeProjectFiles.DataSource = null;

            var project = Project.Current;

            if (project == null)
                return;

            var data = new Code.DirectoryTreeNode(project.ProjectPath);

            treeProjectFiles.DataSource = data;
            treeProjectFiles.RefreshDataSource();
        }

        public void ConnectionListChanged()
        {
            LoadConnections();
        }

        public void PSCmdletListChanged()
        {
            LoadPSCmdlets();
        }

        private void TreeProjectFiles_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (!(treeProjectFiles.GetDataRecordByNode(e.Node) is BaseFileSystemTreeNode node))
                return;

            if (node is DirectoryTreeNode)
                e.Appearance.FontStyleDelta = FontStyle.Bold;
        }

        private void TreeProjectFiles_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            if (!(treeProjectFiles.GetDataRecordByNode(e.Node) is BaseFileSystemTreeNode node))
                return;

            if (node is DirectoryTreeNode)
                e.NodeImageIndex = 0;
            else if (node is FileTreeNode nodeFile)
            {
                var ext = Path.GetExtension(nodeFile.Text)?.ToLower();
#pragma warning disable CRRSP01 // A misspelled word has been found
#pragma warning disable CRRSP06 // A misspelled word has been found
                switch (ext)
                {
                    case ".xlsx":
                    case ".xls":
                        e.NodeImageIndex = 2;
                        break;
                    case ".csv":
                    case ".txt":
                        e.NodeImageIndex = 3;
                        break;
                    case ".sql":
                        e.NodeImageIndex = 4;
                        break;
                    case ".ps":
                    case ".ps1":
                    case ".csx":
                    case ".fsx":
                    case ".r":
                    case ".py":
                        e.NodeImageIndex = 5;
                        break;
                    case ".docx":
                    case ".doc":
                    case ".rtf":
                    case ".htm":
                    case ".html":
                    case ".mht":
                    case ".odt":
                    case ".epub":
                        e.NodeImageIndex = 6;
                        break;
                    case ".png":
                    case ".tif":
                    case ".tiff":
                    case ".jpg":
                    case ".jpeg":
                    case ".gif":
                    case ".bmp":
                        e.NodeImageIndex = 7;
                        break;
                    case ".scdash":
                        e.NodeImageIndex = 8;
                        break;
                    case ".pdf":
                        e.NodeImageIndex = 9;
                        break;
                    case ".scchart":
                        e.NodeImageIndex = 10;
                        break;
                    case ".scpivot":
                        e.NodeImageIndex = 11;
                        break;
                    default:
                        e.NodeImageIndex = 1;
                        break;
                }
#pragma warning restore CRRSP06 // A misspelled word has been found
#pragma warning restore CRRSP01 // A misspelled word has been found
            }

            /*
            0	folder
            1	file
            2	spreadsheet
            3	csv
            4	sql
            5	script
            6	document
            7	image
            8	dashboard
            9	pdf
            10	chart
            */
        }

        private void TreeProjectFiles_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.NodeValue1 is string && e.NodeValue2 is string)
                e.Result = StringLogicalComparer.Compare(Convert.ToString(e.NodeValue1), Convert.ToString(e.NodeValue2));
        }

        private void TreeProjectFiles_DoubleClick(object sender, EventArgs e)
        {
            var node = treeProjectFiles.FocusedNode;

            if (!(treeProjectFiles.GetDataRecordByNode(node) is FileTreeNode fileNode))
                return;

            if (!string.IsNullOrWhiteSpace(fileNode.FullPath) && File.Exists(fileNode.FullPath))
            {
                var fluent = mvvmContext.OfType<MainViewModel>();
                fluent.ViewModel.OpenDocumentFile(fileNode.FullPath);
            }
        }

        private TreeListHitInfo dragFiles_StartHitInfo;
        private void TreeProjectFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None)
                dragFiles_StartHitInfo = (sender as TreeList).CalcHitInfo(new Point(e.X, e.Y));
        }

        private void TreeProjectFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && dragFiles_StartHitInfo != null && dragFiles_StartHitInfo.HitInfoType == HitInfoType.Cell && dragFiles_StartHitInfo.Node != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(dragFiles_StartHitInfo.MousePoint.X - dragSize.Width / 2,
                    dragFiles_StartHitInfo.MousePoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    if (!(treeProjectFiles.GetDataRecordByNode(dragFiles_StartHitInfo.Node) is FileTreeNode node) || !node.CanDrag || IsFileOpen(node.FullPath))
                        return;

                    string text = node.DragText;

                    var dragData = new DataObject();
                    dragData.SetText(text, TextDataFormat.Text);
                    dragData.SetText(text, TextDataFormat.UnicodeText);
                    dragData.SetData(typeof(FileTreeNode), node);
                    dragData.SetData(typeof(TreeListNode), dragFiles_StartHitInfo.Node);

                    (sender as TreeList).DoDragDrop(dragData, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        private void TreeProjectFiles_DragOver(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetData(typeof(FileTreeNode)) is FileTreeNode))
                return;

            var hitInfo = treeProjectFiles.CalcHitInfo(treeProjectFiles.PointToClient(new Point(e.X, e.Y)));

            var treeNode = hitInfo?.Node;
            if (treeNode == null)
                return;

            if (!(treeProjectFiles.GetDataRecordByNode(treeNode) is DirectoryTreeNode))
                return;

            e.Effect = (e.KeyState & 8) == 8 ? DragDropEffects.Copy : DragDropEffects.Move;
        }

        private void TreeProjectFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (!(e.Data.GetData(typeof(FileTreeNode)) is FileTreeNode sourceNode))
                return;

            if (!(e.Data.GetData(typeof(TreeListNode)) is TreeListNode treeSourceNode))
                return;

            var hitInfo = treeProjectFiles.CalcHitInfo(treeProjectFiles.PointToClient(new Point(e.X, e.Y)));

            var treeNode = hitInfo?.Node;
            if (treeNode == null)
                return;

            if (!(treeProjectFiles.GetDataRecordByNode(treeNode) is DirectoryTreeNode dirNode))
                return;

            var srcFile = sourceNode.FullPath;
            var dstFile = Path.Combine(dirNode.FullPath, Path.GetFileName(srcFile));

            switch (e.Effect)
            {
                case DragDropEffects.Copy:
                    File.Copy(srcFile, dstFile);
                    dirNode.ChildNodes.Add(new FileTreeNode(dirNode, new FileInfo(dstFile)));
                    break;
                case DragDropEffects.Move:
                    File.Move(srcFile, dstFile);
                    dirNode.ChildNodes.Add(new FileTreeNode(dirNode, new FileInfo(dstFile)));
                    sourceNode.ParentNode.ChildNodes.Remove(sourceNode);
                    break;
            }

            treeProjectFiles.RefreshNode(treeNode);
            treeProjectFiles.RefreshNode(treeSourceNode.ParentNode);
        }

#pragma warning disable IDE0069 // Disposable fields should be disposed
        private BaseViewer _CurrentViewer;
#pragma warning restore IDE0069 // Disposable fields should be disposed
        private void CloseCurrentViewer()
        {
            if (_CurrentViewer != null)
            {
                _CurrentViewer.Parent = null;
                _CurrentViewer.Dispose();
                _CurrentViewer = null;
            }
        }

        private void TreeProjectFiles_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            CloseCurrentViewer();

            if (!(treeProjectFiles.GetDataRecordByNode(e.Node) is FileTreeNode node))
                return;

            using (new UsingProcessor(() => transitionManager.StartTransition(splitProjectFiles.Panel2),
                () => transitionManager.EndTransition()))
            { 
                if (node.Info.Length > 10 * 1024 * 1024)    //10MB
                    _CurrentViewer = new OtherViewer("Big file");

                if (!File.Exists(node.FullPath))
                    _CurrentViewer = new OtherViewer("File does not exist");

                try
                { 
                    if (_CurrentViewer == null)
                        _CurrentViewer = BaseViewer.CreateViewer(this, node.FullPath, null, null);
                }
                catch (Exception ex)
                {
                    _CurrentViewer = new OtherViewer("Cannot preview: " + ex.Message);
                }

                _CurrentViewer.Dock   = DockStyle.Fill;
                _CurrentViewer.Parent = splitProjectFiles.Panel2;
            }
        }

        private void RibbonControl_Merge(object sender, DevExpress.XtraBars.Ribbon.RibbonMergeEventArgs e)
        {
            if (ribbonControl.StatusBar != null && e.MergedChild.StatusBar != null)
                ribbonControl.StatusBar.MergeStatusBar(e.MergedChild.StatusBar);
        }

        private void RibbonControl_UnMerge(object sender, DevExpress.XtraBars.Ribbon.RibbonMergeEventArgs e)
        {
            if (ribbonControl.StatusBar != null && e.MergedChild.StatusBar != null)
                ribbonControl.StatusBar.UnMergeStatusBar();
        }

        private void LoadConnections()
        {
            var connections = DBConnections.LoadConnections();

            bindingConnections.Clear();

            int selectedIndex = 0;
            for (int i = 0; i < connections.Connections.Count; i++)
            {
                var connection = connections.Connections[i];

                bindingConnections.Add(connection);

                if (string.Compare(connection.Name, connections.SelectedConnection, true) == 0)
                    selectedIndex = i;
            }

            if (bindingConnections.Count > 0)
            {
                if (selectedIndex >= 0 && selectedIndex < bindingConnections.Count)
                    bindingConnections.Position = selectedIndex;
                else
                    bindingConnections.Position = 0;
            }
        }

        private void SelectProject()
        {
            var fluent = mvvmContext.OfType<MainViewModel>();
            fluent.ViewModel.SelectProject();
        }

        private bool _UpdatingCmdlets;
        private void LoadPSCmdlets()
        {
            bindingPSCmdlets.Clear();

            using (new UsingProcessor(() => { _UpdatingCmdlets = true; gridPSCmdlets.BeginUpdate(); }, 
                () => { _UpdatingCmdlets = false; gridPSCmdlets.EndUpdate(); }))
            {
                foreach (var cmdlet in PowerShellScriptEngine.PSCommands)
                    bindingPSCmdlets.Add(cmdlet);

                viewPSCmdlets.CollapseAllGroups();
            }
        }

        private void SearchConnections_EditValueChanged(object sender, EventArgs e)
        {
            var connection = searchConnections.EditValue as DBConnection;
            SelectDBConnection(connection);
        }

        public void SelectDBConnectionByName(string connectionName)
        {
            foreach (DBConnection connection in bindingConnections)
            {
                if (string.Compare(connection.Name, connectionName, true) == 0)
                {
                    SelectDBConnection(connection);
                    return;
                }
            }
        }

        private async void SelectDBConnection(DBConnection connection)
        {
            treeConnections.DataSource = null;

            if (connection == null)
                return;

            try
            {
                var connectionString = connection.ConnectionString;
                var conn = new Connection(connection.Provider, connectionString);
                await conn.OpenAsync(CancellationToken.None).ConfigureAwait(true);

                var dbConnection = conn.DbConnection;
                if (dbConnection == null)
                    return;

                UpdateDbConnection(dbConnection);
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        private string _DefaultDatabase;
        public void UpdateDbConnection(DbConnection connection)
        {
            treeConnections.DataSource = null;
            _DefaultDatabase           = connection?.Database;

            var data = new Code.DatabaseListSchemaNode()
            {
                ConnectionType   = connection?.GetType(),
                ConnectionString = connection?.ConnectionString
            };

            treeConnections.DataSource = data;
        }

        private void TreeConnections_NodesReloaded(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_DefaultDatabase))
            {
                if (treeConnections.Nodes == null)
                    return;

                var node = treeConnections.Nodes.FirstOrDefault(n => string.Compare(Convert.ToString(n[treeConnectionsText]), _DefaultDatabase, true) == 0);
                if (node != null)
                {
                    try
                    {
                        node.Selected = true;
                        node.Expand();

                        treeConnections.MakeNodeVisible(node);
                    }
                    catch (Exception)
                    {
                        //Do nothing. Calling this without delay throws exception
                    }
                }
            }
        }

        private void TreeConnections_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (!(treeConnections.GetDataRecordByNode(e.Node) is DbSchemaBaseNode node))
                return;

            if (node is DatabaseSchemaNode)
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            else if (node is TableListSchemaNode || node is StoredProceduresListSchemaNode || node is FunctionListSchemaNode ||
                node is IndexListSchemaNode || node is ColumnListSchemaNode)
                e.Appearance.FontStyleDelta = FontStyle.Italic;
        }

        private void TreeConnections_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            if (!(treeConnections.GetDataRecordByNode(e.Node) is DbSchemaBaseNode node))
                return;

            if (node is DatabaseSchemaNode)
                e.NodeImageIndex = 3;
            else if (node is TableListSchemaNode || node is StoredProceduresListSchemaNode || node is FunctionListSchemaNode)
                e.NodeImageIndex = 1;
            else if (node is ColumnListSchemaNode || node is IndexListSchemaNode)
                e.NodeImageIndex = 2;
            else if (node is TableSchemaNode)
                e.NodeImageIndex = 4;
            else if (node is ColumnSchemaNode || node is IndexColumnSchemaNode)
                e.NodeImageIndex = 5;
            else if (node is IndexSchemaNode)
                e.NodeImageIndex = 6;
            else if (node is StoredProcedureSchemaNode || node is FunctionSchemaNode)
                e.NodeImageIndex = 8;
            else if (node is StoredProcedureParameterSchemaNode || node is FunctionParameterSchemaNode)
                e.NodeImageIndex = 9;
            else
                e.NodeImageIndex = 0;

            /*
            0	objects
            1	folder
            2	files
            3	database
            4	table
            5	column
            6	key_primary
            7	foreign_key
            8	sql_query
            9	field
            10	attribute
            */
        }

        private TreeListHitInfo dragConnections_StartHitInfo;
        private void TreeConnections_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None)
                dragConnections_StartHitInfo = (sender as TreeList).CalcHitInfo(new Point(e.X, e.Y));
        }

        private void TreeConnections_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && dragConnections_StartHitInfo != null && dragConnections_StartHitInfo.HitInfoType == HitInfoType.Cell && dragConnections_StartHitInfo.Node != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(dragConnections_StartHitInfo.MousePoint.X - dragSize.Width / 2,
                    dragConnections_StartHitInfo.MousePoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    if (!(treeConnections.GetDataRecordByNode(dragConnections_StartHitInfo.Node) is DbSchemaBaseNode node) || !node.CanDrag)
                        return;

                    string text = node.DragText;

                    var dragData = new DataObject();
                    dragData.SetText(text, TextDataFormat.Text);
                    dragData.SetText(text, TextDataFormat.UnicodeText);

                    (sender as TreeList).DoDragDrop(dragData, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        public void StartAddingDocument()
        {
            //transitionManager.StartTransition(this);
            transitionManager.StartWaitingIndicator(this, WaitingAnimatorType.Default);
        }

        public void EndAddingDocument()
        {
            if (transitionManager.IsTransition)
                transitionManager.StopWaitingIndicator();
                //transitionManager.EndTransition();
        }

        private void ViewDocuments_TabMouseActivating(object sender, DevExpress.XtraBars.Docking2010.Views.DocumentCancelEventArgs e)
        {
            if (e.Document != viewDocuments.ActiveDocument)
                transitionManager.StartTransition(this);
        }

        private async void ViewDocuments_DocumentActivated(object sender, DevExpress.XtraBars.Docking2010.Views.DocumentEventArgs e)
        {
            if (transitionManager.IsTransition)
            {
                //Allow some time for transition
                await Task.Yield();
                transitionManager.EndTransition();
            }
        }

        private void ViewDocuments_DocumentDeactivated(object sender, DevExpress.XtraBars.Docking2010.Views.DocumentEventArgs e)
        {
            ResetDocumentModified(e.Document);
        }

        private void BarClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        private void PageProjectFiles_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch (Convert.ToString(e.Button.Properties.Tag))
            {
                case "Refresh":
                    ReloadProjectFiles();
                    break;
                case "NewFolder":
                    var focusedNode = treeProjectFiles.FocusedNode;
                    if (focusedNode == null || !(treeProjectFiles.GetDataRecordByNode(focusedNode) is DirectoryTreeNode dirNode))
                    {
                        XtraMessageBox.Show(this, "Please select parent directory node", "No parent directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }

                    var folderName = XtraInputBox.Show(this, "Folder name", "Folder", "New folder");
                    if (string.IsNullOrWhiteSpace(folderName))
                        return;

                    var fullFolderName = Path.Combine(dirNode.FullPath, folderName);
                    if (!Utils.IsPathNameValid(fullFolderName))
                    {
                        XtraMessageBox.Show(this, "Folder name is not valid", "Invalid folder name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                    if (Directory.Exists(fullFolderName))
                    {
                        XtraMessageBox.Show(this, "Folder already exists", "Invalid folder name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                    Directory.CreateDirectory(fullFolderName);

                    var newDirNode = new DirectoryTreeNode(dirNode, new DirectoryInfo(fullFolderName));
                    dirNode.ChildNodes.Add(newDirNode);
                    break;
                case "Delete":
                    DeleteFocusedNode();
                    break;
                case "Explorer":
                    Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"),
                        Utils.QuoteString(Project.Current.ProjectPath, "\""));
                    break;
            }
        }

        private bool HasOpenFilesInDirectory(string directory)
        {
            var fluent = mvvmContext.OfType<MainViewModel>();
            var result = fluent.ViewModel.HasOpenFilesInDirectory(directory);
            return result;
        }

        private bool IsFileOpen(string fileName)
        {
            var fluent = mvvmContext.OfType<MainViewModel>();
            var model  = fluent.ViewModel.GetFileDocument(fileName);
            return (model != null);
        }

        private void DeleteFocusedNode()
        {
            var focusedNode = treeProjectFiles.FocusedNode;
            if (focusedNode == null)
                return;

            if (treeProjectFiles.GetDataRecordByNode(focusedNode) is DirectoryTreeNode dirNode && dirNode.ParentNode != null)
            {
                if (HasOpenFilesInDirectory(dirNode.FullPath))
                {
                    XtraMessageBox.Show(this, "There are open files in selected directory", "Open files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (XtraMessageBox.Show(this, $"Do you want to delete directory '{dirNode.Text}' and all its content?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Directory.Delete(dirNode.FullPath, true);
                    dirNode.ParentNode.ChildNodes.Remove(dirNode);
                }
            }
            else if (treeProjectFiles.GetDataRecordByNode(focusedNode) is FileTreeNode fileNode && fileNode.ParentNode != null)
            {
                if (IsFileOpen(fileNode.FullPath))
                {
                    XtraMessageBox.Show(this, "File is open", "Open file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (XtraMessageBox.Show(this, $"Do you want to delete file '{fileNode.Text}'?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    File.Delete(fileNode.FullPath);
                    fileNode.ParentNode.ChildNodes.Remove(fileNode);
                }
            }
        }

        private void TreeProjectFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.None)
                DeleteFocusedNode();
        }

        private void UpdateRecentProjects()
        {
            var projectPath = Project.Current.ProjectPath;
            var storage = SavedProjectsStorage.Load();
            var recentProject = storage.FindRecentProject(projectPath);
            if (recentProject == null)
                recentProject = new SavedProject() { Directory = projectPath };
            recentProject.LastAccess = DateTime.Now;
            storage.Save();
        }

        private GridHitInfo dragPSCmdlet_StartHitInfo;
        private void ViewPSCmdlets_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is GridView view))
                return;

            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None)
                dragPSCmdlet_StartHitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void ViewPSCmdlets_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is GridView view))
                return;

            if (e.Button == MouseButtons.Left && dragPSCmdlet_StartHitInfo != null && dragPSCmdlet_StartHitInfo.InRow && 
                dragPSCmdlet_StartHitInfo.RowHandle != GridControl.InvalidRowHandle)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(dragPSCmdlet_StartHitInfo.HitPoint.X - dragSize.Width / 2,
                    dragPSCmdlet_StartHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    var command = viewPSCmdlets.GetRow(dragPSCmdlet_StartHitInfo.RowHandle) as PowerShellScriptEngine.PowerShellCommand;
                    string text = command.Name;

                    var dragData = new DataObject();
                    dragData.SetText(text, TextDataFormat.Text);
                    dragData.SetText(text, TextDataFormat.UnicodeText);

                    view.GridControl.DoDragDrop(dragData, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        private async void BindingPSCmdlets_CurrentChanged(object sender, EventArgs e)
        {
            if (_UpdatingCmdlets)
                return;

            editPSCmdletDescription.Text = string.Empty;

            if (!(bindingPSCmdlets.Current is PowerShellScriptEngine.PowerShellCommand cmdlet))
                return;

            var helper = new CmdletIntellisenseHelp(cmdlet.Name);
            string description = null;

            try
            {
                await Task.Run(() => description = helper.GetHelpHtmlContent(null)).ConfigureAwait(true);
            }
            catch (Exception)
            {
                //throw ex;
                description = null;
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                editPSCmdletDescription.HtmlText = description;
                editPSCmdletDescription.Document.DefaultCharacterProperties.FontName = "Lucida Console";
                editPSCmdletDescription.Document.DefaultCharacterProperties.FontSize = 8;
            }
        }

        private void LayoutControlGroupPSCmdletDescription_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            switch (Convert.ToString(e.Button?.Properties.Tag))
            {
                case "Help":
#pragma warning disable IDE0067 // Dispose objects before losing scope
                    var viewer = new RichTextViewer()
                    {
                        Location = editPSCmdletDescription.PointToScreen(new Point(editPSCmdletDescription.Right + 20, 
                            -editPSCmdletDescription.Top + gridPSCmdlets.Top)),
                        Width  = Math.Min((int)(editPSCmdletDescription.Width * 3), 1000),
                        Height = Math.Min(this.Height, 1000)
                    };
#pragma warning restore IDE0067 // Dispose objects before losing scope

                    viewer.FormClosed += (s, e) =>
                    {
                        viewer.Dispose();
                    };

                    viewer.LoadHtmlText(editPSCmdletDescription.HtmlText);
                    viewer.Show(this);
                    break;
                case "WebHelp":
                    if (bindingPSCmdlets.Current is PowerShellScriptEngine.PowerShellCommand cmdlet)
                    {
                        var helper = new CmdletIntellisenseHelp(cmdlet.Name);
                        helper.ShowOnlineHelp(null);
                    }
                    break;
            }
        }

        private void UpdateControlModified(Control control)
        {
            if (control == null)
                return;

            var document = viewDocuments.Documents.Where(doc => doc.Control == control).FirstOrDefault();
            if (document != null && document != viewDocuments.ActiveDocument &&
                document is DevExpress.XtraBars.Docking2010.Views.Tabbed.Document tabDocument)
            {
                var appearance = tabDocument.Appearance.Header;
                appearance.BackColor = SystemColors.Info;
                appearance.ForeColor = SystemColors.InfoText;
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

        private void ResetDocumentModified(BaseDocument document)
        {
            if (document is DevExpress.XtraBars.Docking2010.Views.Tabbed.Document tabDocument)
            {
                var appearance = tabDocument.Appearance.Header;
                appearance.Reset();
            }
        }

        public void ControlModified(ControlModifiedMessage message)
        {
            if (message.Modified)
                UpdateControlModified(message.Control);
            else
                ResetControlModified(message.Control);
        }

        private void ViewApplicationItems_ItemCustomize(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemCustomizeEventArgs e)
        {
            var view = sender as TileView;
            int val  = Convert.ToInt32(view?.GetRowCellValue(e.RowHandle, "ImageIndex"));
            if (val >= 0 && val < imagesApplicationItems.Count)
            {
                var el = e.Item.GetElementByName("Image");
                el.ImageIndex = val;
            }
        }

        private void MnuAppProjectNew_Click(object sender, EventArgs e)
        {
            popupControlContainerAppMenu.HidePopup();

            var model = mvvmContext.GetViewModel<MainViewModel>();
            model.NewProject();
        }

        private void MnuAppProjectOpen_Click(object sender, EventArgs e)
        {
            popupControlContainerAppMenu.HidePopup();

            var model = mvvmContext.GetViewModel<MainViewModel>();
            model.OpenProject();
        }

        private void MnuAppProjectSelect_Click(object sender, EventArgs e)
        {
            popupControlContainerAppMenu.HidePopup();

            var model = mvvmContext.GetViewModel<MainViewModel>();
            model.SelectProject();
        }

        private void MnuAppFileOpen_Click(object sender, EventArgs e)
        {
            popupControlContainerAppMenu.HidePopup();

            var model = mvvmContext.GetViewModel<MainViewModel>();
            model.OpenFile();
        }

        private void MnuAppFileSaveAll_Click(object sender, EventArgs e)
        {
            popupControlContainerAppMenu.HidePopup();

            var model = mvvmContext.GetViewModel<MainViewModel>();
            model.SaveAllDocuments();
        }

        private void MnuAppControlSettings_Click(object sender, EventArgs e)
        {
            popupControlContainerAppMenu.HidePopup();

            var model = mvvmContext.GetViewModel<MainViewModel>();
            model.EditApplicationSettings();
        }

        private void BtnAppMenuExit_Click(object sender, EventArgs e)
        {
            popupControlContainerAppMenu.HidePopup();
            Close();
        }

        private void ViewApplicationItems_ItemClick(object sender, TileViewItemClickEventArgs e)
        {
            if (!(e.Item.View.GetRow(e.Item.RowHandle) is AppMenuProjectItem row))
                return;

            popupControlContainerAppMenu.HidePopup();

            var model = mvvmContext.GetViewModel<MainViewModel>();
            model.AddNewDocument(row.DocumentType, row.DocumentSubType);
        }
    }
}
