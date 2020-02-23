namespace SpreadCommander.Documents.Controls
{
	partial class HeapControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeapControl));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.Animation.Transition transition1 = new DevExpress.Utils.Animation.Transition();
            DevExpress.Utils.Animation.FadeTransition fadeTransition1 = new DevExpress.Utils.Animation.FadeTransition();
            this.gridFiles = new DevExpress.XtraGrid.GridControl();
            this.bindingFiles = new System.Windows.Forms.BindingSource(this.components);
            this.viewFiles = new DevExpress.XtraGrid.Views.WinExplorer.WinExplorerView();
            this.colFileName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExtension = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImageIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEnabled = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGroupName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageFilesMedium = new DevExpress.Utils.SvgImageCollection(this.components);
            this.imageFilesSmall = new DevExpress.Utils.SvgImageCollection(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.toolbar = new DevExpress.XtraBars.Bar();
            this.barFolder = new DevExpress.XtraBars.BarEditItem();
            this.repositoryFolder2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.barMask = new DevExpress.XtraBars.BarEditItem();
            this.repositoryMask = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.toolbarLeft = new DevExpress.XtraBars.Bar();
            this.barFiles = new DevExpress.XtraBars.BarButtonItem();
            this.barPreview = new DevExpress.XtraBars.BarButtonItem();
            this.barOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barFileFirst = new DevExpress.XtraBars.BarButtonItem();
            this.barFilePrevious = new DevExpress.XtraBars.BarButtonItem();
            this.barFileNext = new DevExpress.XtraBars.BarButtonItem();
            this.barFileLast = new DevExpress.XtraBars.BarButtonItem();
            this.barList = new DevExpress.XtraBars.BarButtonItem();
            this.barTiles = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryFolder = new DevExpress.XtraEditors.Repository.RepositoryItemBreadCrumbEdit();
            this.transitionManager = new DevExpress.Utils.Animation.TransitionManager(this.components);
            this.folderBrowser = new DevExpress.XtraEditors.XtraFolderBrowserDialog(this.components);
            this.watcher = new System.IO.FileSystemWatcher();
            this.panelHost = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilesMedium)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilesSmall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryFolder2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.watcher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelHost)).BeginInit();
            this.panelHost.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridFiles
            // 
            this.gridFiles.DataSource = this.bindingFiles;
            this.gridFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFiles.Location = new System.Drawing.Point(2, 2);
            this.gridFiles.MainView = this.viewFiles;
            this.gridFiles.MenuManager = this.barManager;
            this.gridFiles.Name = "gridFiles";
            this.gridFiles.Size = new System.Drawing.Size(941, 724);
            this.gridFiles.TabIndex = 4;
            this.gridFiles.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewFiles});
            // 
            // bindingFiles
            // 
            this.bindingFiles.DataSource = typeof(SpreadCommander.Documents.Controls.HeapControl.FileItem);
            this.bindingFiles.CurrentChanged += new System.EventHandler(this.BindingFiles_CurrentChanged);
            // 
            // viewFiles
            // 
            this.viewFiles.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFileName,
            this.colExtension,
            this.colDescription,
            this.colFileType,
            this.colImageIndex,
            this.colEnabled,
            this.colGroupName});
            this.viewFiles.ColumnSet.DescriptionColumn = this.colDescription;
            this.viewFiles.ColumnSet.EnabledColumn = this.colEnabled;
            this.viewFiles.ColumnSet.GroupColumn = this.colGroupName;
            this.viewFiles.ColumnSet.MediumImageIndexColumn = this.colImageIndex;
            this.viewFiles.ColumnSet.SmallImageIndexColumn = this.colImageIndex;
            this.viewFiles.ColumnSet.TextColumn = this.colFileName;
            this.viewFiles.GridControl = this.gridFiles;
            this.viewFiles.GroupCount = 1;
            this.viewFiles.MediumImages = this.imageFilesMedium;
            this.viewFiles.Name = "viewFiles";
            this.viewFiles.OptionsView.Style = DevExpress.XtraGrid.Views.WinExplorer.WinExplorerViewStyle.Tiles;
            this.viewFiles.SmallImages = this.imageFilesSmall;
            this.viewFiles.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colGroupName, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.viewFiles.ItemDoubleClick += new DevExpress.XtraGrid.Views.WinExplorer.WinExplorerViewItemDoubleClickEventHandler(this.ViewFiles_ItemDoubleClick);
            this.viewFiles.CustomColumnSort += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewFiles_CustomColumnSort);
            // 
            // colFileName
            // 
            this.colFileName.FieldName = "FileName";
            this.colFileName.Name = "colFileName";
            this.colFileName.OptionsColumn.ReadOnly = true;
            this.colFileName.Visible = true;
            this.colFileName.VisibleIndex = 0;
            // 
            // colExtension
            // 
            this.colExtension.FieldName = "Extension";
            this.colExtension.Name = "colExtension";
            this.colExtension.OptionsColumn.AllowEdit = false;
            this.colExtension.OptionsColumn.ReadOnly = true;
            this.colExtension.Visible = true;
            this.colExtension.VisibleIndex = 1;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.OptionsColumn.ReadOnly = true;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 1;
            // 
            // colFileType
            // 
            this.colFileType.FieldName = "FileType";
            this.colFileType.Name = "colFileType";
            this.colFileType.OptionsColumn.ReadOnly = true;
            this.colFileType.Visible = true;
            this.colFileType.VisibleIndex = 3;
            // 
            // colImageIndex
            // 
            this.colImageIndex.FieldName = "ImageIndex";
            this.colImageIndex.Name = "colImageIndex";
            this.colImageIndex.OptionsColumn.AllowEdit = false;
            this.colImageIndex.OptionsColumn.ReadOnly = true;
            this.colImageIndex.Visible = true;
            this.colImageIndex.VisibleIndex = 4;
            // 
            // colEnabled
            // 
            this.colEnabled.FieldName = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.OptionsColumn.AllowEdit = false;
            this.colEnabled.OptionsColumn.ReadOnly = true;
            this.colEnabled.Visible = true;
            this.colEnabled.VisibleIndex = 5;
            // 
            // colGroupName
            // 
            this.colGroupName.FieldName = "GroupName";
            this.colGroupName.Name = "colGroupName";
            this.colGroupName.OptionsColumn.AllowEdit = false;
            this.colGroupName.Visible = true;
            this.colGroupName.VisibleIndex = 2;
            // 
            // imageFilesMedium
            // 
            this.imageFilesMedium.ImageSize = new System.Drawing.Size(48, 48);
            this.imageFilesMedium.Add("folder", "image://svgimages/icon builder/actions_folderclose.svg");
            this.imageFilesMedium.Add("file", "image://svgimages/dashboards/new.svg");
            this.imageFilesMedium.Add("spreadsheet", "image://svgimages/richedit/inserttable.svg");
            this.imageFilesMedium.Add("csv", "image://svgimages/spreadsheet/freezetoprow.svg");
            this.imageFilesMedium.Add("sql", "image://svgimages/dashboards/filterquery.svg");
            this.imageFilesMedium.Add("script", "image://svgimages/icon builder/travel_map.svg");
            this.imageFilesMedium.Add("document", "image://svgimages/pdf viewer/singlepageview.svg");
            this.imageFilesMedium.Add("image", "image://svgimages/richedit/insertimage.svg");
            this.imageFilesMedium.Add("dashboard", "image://svgimages/dashboards/layout.svg");
            this.imageFilesMedium.Add("pdf", "image://svgimages/pdf viewer/documentpdf.svg");
            // 
            // imageFilesSmall
            // 
            this.imageFilesSmall.Add("folder", "image://svgimages/icon builder/actions_folderclose.svg");
            this.imageFilesSmall.Add("file", "image://svgimages/dashboards/new.svg");
            this.imageFilesSmall.Add("spreadsheet", "image://svgimages/richedit/inserttable.svg");
            this.imageFilesSmall.Add("csv", "image://svgimages/spreadsheet/freezetoprow.svg");
            this.imageFilesSmall.Add("sql", "image://svgimages/dashboards/filterquery.svg");
            this.imageFilesSmall.Add("script", "image://svgimages/icon builder/travel_map.svg");
            this.imageFilesSmall.Add("document", "image://svgimages/pdf viewer/singlepageview.svg");
            this.imageFilesSmall.Add("image", "image://svgimages/richedit/insertimage.svg");
            this.imageFilesSmall.Add("dashboard", "image://svgimages/dashboards/layout.svg");
            this.imageFilesSmall.Add("pdf", "image://svgimages/pdf viewer/documentpdf.svg");
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.toolbar,
            this.toolbarLeft});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barFiles,
            this.barList,
            this.barTiles,
            this.barFolder,
            this.barOpen,
            this.barMask,
            this.barPreview,
            this.barFileFirst,
            this.barFilePrevious,
            this.barFileNext,
            this.barFileLast});
            this.barManager.MaxItemId = 12;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryFolder,
            this.repositoryMask,
            this.repositoryFolder2});
            // 
            // toolbar
            // 
            this.toolbar.BarName = "Tools";
            this.toolbar.CanDockStyle = ((DevExpress.XtraBars.BarCanDockStyle)(((((DevExpress.XtraBars.BarCanDockStyle.Left | DevExpress.XtraBars.BarCanDockStyle.Top) 
            | DevExpress.XtraBars.BarCanDockStyle.Right) 
            | DevExpress.XtraBars.BarCanDockStyle.Bottom) 
            | DevExpress.XtraBars.BarCanDockStyle.Standalone)));
            this.toolbar.DockCol = 0;
            this.toolbar.DockRow = 0;
            this.toolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.toolbar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barFolder, "", true, true, true, 296),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.barMask, "", false, true, true, 162)});
            this.toolbar.OptionsBar.DisableClose = true;
            this.toolbar.OptionsBar.DisableCustomization = true;
            this.toolbar.OptionsBar.UseWholeRow = true;
            this.toolbar.Text = "Tools";
            // 
            // barFolder
            // 
            this.barFolder.AutoFillWidth = true;
            this.barFolder.Caption = "Folder";
            this.barFolder.Edit = this.repositoryFolder2;
            this.barFolder.Id = 3;
            this.barFolder.Name = "barFolder";
            this.barFolder.EditValueChanged += new System.EventHandler(this.BarFolder_EditValueChanged);
            // 
            // repositoryFolder2
            // 
            this.repositoryFolder2.AutoHeight = false;
            this.repositoryFolder2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryFolder2.Name = "repositoryFolder2";
            this.repositoryFolder2.ReadOnly = true;
            this.repositoryFolder2.UseReadOnlyAppearance = false;
            this.repositoryFolder2.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.RepositoryFolder2_ButtonClick);
            // 
            // barMask
            // 
            this.barMask.Caption = "Mask";
            this.barMask.Edit = this.repositoryMask;
            this.barMask.Id = 6;
            this.barMask.Name = "barMask";
            // 
            // repositoryMask
            // 
            this.repositoryMask.AutoHeight = false;
            this.repositoryMask.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryMask.Name = "repositoryMask";
            this.repositoryMask.SelectedValueChanged += new System.EventHandler(this.RepositoryMask_SelectedValueChanged);
            this.repositoryMask.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RepositoryMask_KeyDown);
            this.repositoryMask.Leave += new System.EventHandler(this.RepositoryMask_Leave);
            // 
            // toolbarLeft
            // 
            this.toolbarLeft.BarName = "Actions";
            this.toolbarLeft.CanDockStyle = ((DevExpress.XtraBars.BarCanDockStyle)(((((DevExpress.XtraBars.BarCanDockStyle.Left | DevExpress.XtraBars.BarCanDockStyle.Top) 
            | DevExpress.XtraBars.BarCanDockStyle.Right) 
            | DevExpress.XtraBars.BarCanDockStyle.Bottom) 
            | DevExpress.XtraBars.BarCanDockStyle.Standalone)));
            this.toolbarLeft.DockCol = 0;
            this.toolbarLeft.DockRow = 0;
            this.toolbarLeft.DockStyle = DevExpress.XtraBars.BarDockStyle.Left;
            this.toolbarLeft.FloatLocation = new System.Drawing.Point(1444, 474);
            this.toolbarLeft.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barFiles),
            new DevExpress.XtraBars.LinkPersistInfo(this.barPreview),
            new DevExpress.XtraBars.LinkPersistInfo(this.barOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.barFileFirst, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barFilePrevious),
            new DevExpress.XtraBars.LinkPersistInfo(this.barFileNext),
            new DevExpress.XtraBars.LinkPersistInfo(this.barFileLast),
            new DevExpress.XtraBars.LinkPersistInfo(this.barList, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barTiles)});
            this.toolbarLeft.OptionsBar.UseWholeRow = true;
            this.toolbarLeft.Text = "Actions";
            // 
            // barFiles
            // 
            this.barFiles.Caption = "Files";
            this.barFiles.Id = 0;
            this.barFiles.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barFiles.ImageOptions.SvgImage")));
            this.barFiles.Name = "barFiles";
            this.barFiles.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarFiles_ItemClick);
            // 
            // barPreview
            // 
            this.barPreview.Caption = "Preview";
            this.barPreview.Id = 7;
            this.barPreview.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barPreview.ImageOptions.SvgImage")));
            this.barPreview.Name = "barPreview";
            this.barPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarPreview_ItemClick);
            // 
            // barOpen
            // 
            this.barOpen.Caption = "Open";
            this.barOpen.Id = 5;
            this.barOpen.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barOpen.ImageOptions.SvgImage")));
            this.barOpen.Name = "barOpen";
            this.barOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarOpen_ItemClick);
            // 
            // barFileFirst
            // 
            this.barFileFirst.Caption = "First";
            this.barFileFirst.Id = 8;
            this.barFileFirst.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barFileFirst.ImageOptions.SvgImage")));
            this.barFileFirst.Name = "barFileFirst";
            this.barFileFirst.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarFileFirst_ItemClick);
            // 
            // barFilePrevious
            // 
            this.barFilePrevious.Caption = "Previous";
            this.barFilePrevious.Id = 9;
            this.barFilePrevious.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barFilePrevious.ImageOptions.SvgImage")));
            this.barFilePrevious.Name = "barFilePrevious";
            this.barFilePrevious.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarFilePrevious_ItemClick);
            // 
            // barFileNext
            // 
            this.barFileNext.Caption = "Next";
            this.barFileNext.Id = 10;
            this.barFileNext.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barFileNext.ImageOptions.SvgImage")));
            this.barFileNext.Name = "barFileNext";
            this.barFileNext.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarFileNext_ItemClick);
            // 
            // barFileLast
            // 
            this.barFileLast.Caption = "Last";
            this.barFileLast.Id = 11;
            this.barFileLast.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barFileLast.ImageOptions.SvgImage")));
            this.barFileLast.Name = "barFileLast";
            this.barFileLast.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarFileLast_ItemClick);
            // 
            // barList
            // 
            this.barList.Caption = "List";
            this.barList.Id = 1;
            this.barList.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barList.ImageOptions.SvgImage")));
            this.barList.Name = "barList";
            this.barList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarList_ItemClick);
            // 
            // barTiles
            // 
            this.barTiles.Caption = "Tiles";
            this.barTiles.Id = 2;
            this.barTiles.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barTiles.ImageOptions.SvgImage")));
            this.barTiles.Name = "barTiles";
            this.barTiles.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarTiles_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(972, 21);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 749);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(972, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 21);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(27, 728);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(972, 21);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 728);
            // 
            // repositoryFolder
            // 
            this.repositoryFolder.AutoHeight = false;
            this.repositoryFolder.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", "Browse", null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.repositoryFolder.Name = "repositoryFolder";
            this.repositoryFolder.PathChanged += new DevExpress.XtraEditors.BreadCrumbPathChangedEventHandler(this.RepositoryFolder_PathChanged);
            this.repositoryFolder.QueryChildNodes += new DevExpress.XtraEditors.BreadCrumbQueryChildNodesEventHandler(this.RepositoryFolder_QueryChildNodes);
            this.repositoryFolder.ValidatePath += new DevExpress.XtraEditors.BreadCrumbValidatePathEventHandler(this.RepositoryFolder_ValidatePath);
            this.repositoryFolder.PathRejected += new DevExpress.XtraEditors.BreadCrumbPathRejectedEventHandler(this.RepositoryFolder_PathRejected);
            this.repositoryFolder.NewNodeAdding += new DevExpress.XtraEditors.BreadCrumbNewNodeAddingEventHandler(this.RepositoryFolder_NewNodeAdding);
            this.repositoryFolder.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.RepositoryFolder_ButtonClick);
            // 
            // transitionManager
            // 
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.Control = this.gridFiles;
            transition1.LineWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.LineWaitingIndicatorProperties.Caption = "";
            transition1.LineWaitingIndicatorProperties.Description = "";
            transition1.RingWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.RingWaitingIndicatorProperties.Caption = "";
            transition1.RingWaitingIndicatorProperties.Description = "";
            transition1.TransitionType = fadeTransition1;
            transition1.WaitingIndicatorProperties.Caption = "";
            transition1.WaitingIndicatorProperties.Description = "";
            this.transitionManager.Transitions.Add(transition1);
            // 
            // folderBrowser
            // 
            this.folderBrowser.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            this.folderBrowser.Title = "Select folder";
            // 
            // watcher
            // 
            this.watcher.EnableRaisingEvents = true;
            this.watcher.SynchronizingObject = this;
            this.watcher.Changed += new System.IO.FileSystemEventHandler(this.Watcher_Changed);
            this.watcher.Created += new System.IO.FileSystemEventHandler(this.Watcher_Created);
            this.watcher.Deleted += new System.IO.FileSystemEventHandler(this.Watcher_Deleted);
            this.watcher.Renamed += new System.IO.RenamedEventHandler(this.Watcher_Renamed);
            // 
            // panelHost
            // 
            this.panelHost.Controls.Add(this.gridFiles);
            this.panelHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHost.Location = new System.Drawing.Point(27, 21);
            this.panelHost.Name = "panelHost";
            this.panelHost.Size = new System.Drawing.Size(945, 728);
            this.panelHost.TabIndex = 9;
            // 
            // HeapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelHost);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "HeapControl";
            this.Size = new System.Drawing.Size(972, 749);
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilesMedium)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFilesSmall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryFolder2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.watcher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelHost)).EndInit();
            this.panelHost.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.Bar toolbar;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraGrid.GridControl gridFiles;
		private DevExpress.XtraGrid.Views.WinExplorer.WinExplorerView viewFiles;
		private System.Windows.Forms.BindingSource bindingFiles;
		private DevExpress.Utils.Animation.TransitionManager transitionManager;
		private DevExpress.XtraBars.BarButtonItem barFiles;
		private DevExpress.XtraBars.BarButtonItem barList;
		private DevExpress.XtraBars.BarButtonItem barTiles;
		private DevExpress.XtraBars.BarEditItem barFolder;
		private DevExpress.XtraEditors.Repository.RepositoryItemBreadCrumbEdit repositoryFolder;
		private DevExpress.XtraBars.BarButtonItem barOpen;
		private DevExpress.XtraBars.BarEditItem barMask;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryMask;
		private DevExpress.Utils.SvgImageCollection imageFilesMedium;
		private DevExpress.Utils.SvgImageCollection imageFilesSmall;
		private DevExpress.XtraGrid.Columns.GridColumn colFileName;
		private DevExpress.XtraGrid.Columns.GridColumn colExtension;
		private DevExpress.XtraGrid.Columns.GridColumn colDescription;
		private DevExpress.XtraGrid.Columns.GridColumn colFileType;
		private DevExpress.XtraGrid.Columns.GridColumn colImageIndex;
		private DevExpress.XtraGrid.Columns.GridColumn colEnabled;
		private DevExpress.XtraGrid.Columns.GridColumn colGroupName;
		private DevExpress.XtraEditors.XtraFolderBrowserDialog folderBrowser;
		private System.IO.FileSystemWatcher watcher;
		private DevExpress.XtraEditors.PanelControl panelHost;
		private DevExpress.XtraBars.Bar toolbarLeft;
		private DevExpress.XtraBars.BarButtonItem barPreview;
		private DevExpress.XtraBars.BarButtonItem barFileFirst;
		private DevExpress.XtraBars.BarButtonItem barFilePrevious;
		private DevExpress.XtraBars.BarButtonItem barFileNext;
		private DevExpress.XtraBars.BarButtonItem barFileLast;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryFolder2;
    }
}
