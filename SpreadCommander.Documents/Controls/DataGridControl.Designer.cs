namespace SpreadCommander.Documents.Controls
{
	partial class DataGridControl
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
            DevExpress.XtraEditors.TableLayout.ItemTemplateBase itemTemplateBase1 = new DevExpress.XtraEditors.TableLayout.ItemTemplateBase();
            DevExpress.XtraEditors.TableLayout.TableColumnDefinition tableColumnDefinition1 = new DevExpress.XtraEditors.TableLayout.TableColumnDefinition();
            DevExpress.XtraEditors.TableLayout.TableColumnDefinition tableColumnDefinition2 = new DevExpress.XtraEditors.TableLayout.TableColumnDefinition();
            DevExpress.XtraEditors.TableLayout.TemplatedItemElement templatedItemElement1 = new DevExpress.XtraEditors.TableLayout.TemplatedItemElement();
            DevExpress.XtraEditors.TableLayout.TemplatedItemElement templatedItemElement2 = new DevExpress.XtraEditors.TableLayout.TemplatedItemElement();
            DevExpress.XtraEditors.TableLayout.TemplatedItemElement templatedItemElement3 = new DevExpress.XtraEditors.TableLayout.TemplatedItemElement();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition1 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableRowDefinition tableRowDefinition2 = new DevExpress.XtraEditors.TableLayout.TableRowDefinition();
            DevExpress.XtraEditors.TableLayout.TableSpan tableSpan1 = new DevExpress.XtraEditors.TableLayout.TableSpan();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataGridControl));
            DevExpress.Utils.Animation.Transition transition1 = new DevExpress.Utils.Animation.Transition();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.gridTable = new DevExpress.XtraGrid.GridControl();
            this.viewTable = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemBlobEditor = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemStringEditor = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemDateEditor = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.navLeft = new DevExpress.XtraBars.Navigation.NavigationPane();
            this.pageTables = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.listTables = new DevExpress.XtraEditors.ImageListBoxControl();
            this.bindingTables = new System.Windows.Forms.BindingSource(this.components);
            this.searchTables = new DevExpress.XtraEditors.SearchControl();
            this.popupGrid = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barCopyWithHeaders = new DevExpress.XtraBars.BarButtonItem();
            this.barCopyWithoutHeaders = new DevExpress.XtraBars.BarButtonItem();
            this.barCopyPlainDataWithHeaders = new DevExpress.XtraBars.BarButtonItem();
            this.barCopyPlainDataWithoutheaders = new DevExpress.XtraBars.BarButtonItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.splitterGrid = new DevExpress.XtraEditors.SplitterControl();
            this.transitionManager = new DevExpress.Utils.Animation.TransitionManager(this.components);
            this.barMenuFormatting = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.gridTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBlobEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemStringEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navLeft)).BeginInit();
            this.navLeft.SuspendLayout();
            this.pageTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listTables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchTables.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.SuspendLayout();
            // 
            // gridTable
            // 
            this.gridTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTable.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridTable.Location = new System.Drawing.Point(310, 0);
            this.gridTable.MainView = this.viewTable;
            this.gridTable.Name = "gridTable";
            this.gridTable.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemBlobEditor,
            this.repositoryItemStringEditor,
            this.repositoryItemDateEditor});
            this.gridTable.Size = new System.Drawing.Size(846, 751);
            this.gridTable.TabIndex = 2;
            this.gridTable.UseEmbeddedNavigator = true;
            this.gridTable.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewTable});
            this.gridTable.ViewRegistered += new DevExpress.XtraGrid.ViewOperationEventHandler(this.GridTable_ViewRegistered);
            this.gridTable.ViewRemoved += new DevExpress.XtraGrid.ViewOperationEventHandler(this.GridTable_ViewRemoved);
            // 
            // viewTable
            // 
            this.viewTable.GridControl = this.gridTable;
            this.viewTable.Name = "viewTable";
            this.viewTable.OptionsBehavior.AllowIncrementalSearch = true;
            this.viewTable.OptionsBehavior.AllowSortAnimation = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.AllowCopy = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.AllowCsvFormat = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.AllowExcelFormat = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.AllowHtmlFormat = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.AllowRtfFormat = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.AllowTxtFormat = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.ClipboardMode = DevExpress.Export.ClipboardMode.Formatted;
            this.viewTable.OptionsClipboard.CopyCollapsedData = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.CopyColumnHeaders = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsClipboard.PasteMode = DevExpress.Export.PasteMode.None;
            this.viewTable.OptionsDetail.AllowExpandEmptyDetails = true;
            this.viewTable.OptionsDetail.AllowOnlyOneMasterRowExpanded = true;
            this.viewTable.OptionsFilter.ColumnFilterPopupMode = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode.Excel;
            this.viewTable.OptionsFind.AlwaysVisible = true;
            this.viewTable.OptionsFind.Behavior = DevExpress.XtraEditors.FindPanelBehavior.Search;
            this.viewTable.OptionsFind.SearchInPreview = true;
            this.viewTable.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsMenu.ShowConditionalFormattingItem = true;
            this.viewTable.OptionsMenu.ShowFooterItem = true;
            this.viewTable.OptionsMenu.ShowGroupSummaryEditorItem = true;
            this.viewTable.OptionsScrollAnnotations.ShowErrors = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsScrollAnnotations.ShowFocusedRow = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsScrollAnnotations.ShowSelectedRows = DevExpress.Utils.DefaultBoolean.True;
            this.viewTable.OptionsSelection.MultiSelect = true;
            this.viewTable.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.viewTable.OptionsView.BestFitMaxRowCount = 100;
            this.viewTable.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Fast;
            this.viewTable.OptionsView.ColumnAutoWidth = false;
            this.viewTable.OptionsView.EnableAppearanceEvenRow = true;
            this.viewTable.OptionsView.ShowAutoFilterRow = true;
            this.viewTable.OptionsView.ShowGroupPanelColumnsAsSingleRow = true;
            this.viewTable.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.ViewTable_CustomDrawCell);
            this.viewTable.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.ViewTable_CustomRowCellEdit);
            this.viewTable.CustomColumnGroup += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewTable_CustomColumnGroup);
            this.viewTable.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.ViewTable_ShowingEditor);
            this.viewTable.CustomColumnSort += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewTable_CustomColumnSort);
            this.viewTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewTable_MouseDown);
            // 
            // repositoryItemBlobEditor
            // 
            this.repositoryItemBlobEditor.AutoHeight = false;
            this.repositoryItemBlobEditor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemBlobEditor.Name = "repositoryItemBlobEditor";
            this.repositoryItemBlobEditor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.RepositoryItemBlobEditor_ButtonClick);
            this.repositoryItemBlobEditor.FormatEditValue += new DevExpress.XtraEditors.Controls.ConvertEditValueEventHandler(this.RepositoryItemBlobEditor_FormatEditValue);
            this.repositoryItemBlobEditor.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.RepositoryItemBlobEditor_CustomDisplayText);
            // 
            // repositoryItemStringEditor
            // 
            this.repositoryItemStringEditor.AutoHeight = false;
            this.repositoryItemStringEditor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemStringEditor.Name = "repositoryItemStringEditor";
            this.repositoryItemStringEditor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.RepositoryItemStringEditor_ButtonClick);
            this.repositoryItemStringEditor.FormatEditValue += new DevExpress.XtraEditors.Controls.ConvertEditValueEventHandler(this.RepositoryItemStringEditor_FormatEditValue);
            this.repositoryItemStringEditor.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.RepositoryItemStringEditor_CustomDisplayText);
            // 
            // repositoryItemDateEditor
            // 
            this.repositoryItemDateEditor.AutoHeight = false;
            this.repositoryItemDateEditor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEditor.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEditor.DisplayFormat.FormatString = "g";
            this.repositoryItemDateEditor.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEditor.EditFormat.FormatString = "g";
            this.repositoryItemDateEditor.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEditor.Name = "repositoryItemDateEditor";
            // 
            // navLeft
            // 
            this.navLeft.Controls.Add(this.pageTables);
            this.navLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.navLeft.ItemOrientation = System.Windows.Forms.Orientation.Vertical;
            this.navLeft.Location = new System.Drawing.Point(0, 0);
            this.navLeft.Name = "navLeft";
            this.navLeft.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.pageTables});
            this.navLeft.RegularSize = new System.Drawing.Size(300, 751);
            this.navLeft.SelectedPage = this.pageTables;
            this.navLeft.Size = new System.Drawing.Size(300, 751);
            this.navLeft.TabIndex = 0;
            // 
            // pageTables
            // 
            this.pageTables.Caption = "Tables";
            this.pageTables.Controls.Add(this.listTables);
            this.pageTables.Controls.Add(this.searchTables);
            this.pageTables.Name = "pageTables";
            this.pageTables.Size = new System.Drawing.Size(240, 678);
            // 
            // listTables
            // 
            this.listTables.DataSource = this.bindingTables;
            this.listTables.DisplayMember = "TableName";
            this.listTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTables.ItemHeight = 50;
            this.listTables.Location = new System.Drawing.Point(0, 20);
            this.listTables.Name = "listTables";
            this.listTables.Size = new System.Drawing.Size(240, 658);
            this.listTables.TabIndex = 1;
            tableColumnDefinition1.Length.Value = 53D;
            tableColumnDefinition2.Length.Value = 200D;
            itemTemplateBase1.Columns.Add(tableColumnDefinition1);
            itemTemplateBase1.Columns.Add(tableColumnDefinition2);
            templatedItemElement1.Appearance.Normal.FontStyleDelta = System.Drawing.FontStyle.Bold;
            templatedItemElement1.Appearance.Normal.Options.UseFont = true;
            templatedItemElement1.ColumnIndex = 1;
            templatedItemElement1.FieldName = "TableName";
            templatedItemElement1.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            templatedItemElement1.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomInside;
            templatedItemElement1.Text = "TableName";
            templatedItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            templatedItemElement2.Appearance.Normal.FontStyleDelta = System.Drawing.FontStyle.Italic;
            templatedItemElement2.Appearance.Normal.Options.UseFont = true;
            templatedItemElement2.ColumnIndex = 1;
            templatedItemElement2.FieldName = "Description";
            templatedItemElement2.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            templatedItemElement2.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomInside;
            templatedItemElement2.RowIndex = 1;
            templatedItemElement2.Text = "Description";
            templatedItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            templatedItemElement3.FieldName = "";
            templatedItemElement3.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            templatedItemElement3.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomInside;
            templatedItemElement3.ImageOptions.ImageSize = new System.Drawing.Size(32, 32);
            templatedItemElement3.ImageOptions.SvgImage = global::SpreadCommander.Documents.Properties.Resources.Table;
            templatedItemElement3.ImageOptions.SvgImageSize = new System.Drawing.Size(32, 32);
            templatedItemElement3.Text = "";
            templatedItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            itemTemplateBase1.Elements.Add(templatedItemElement1);
            itemTemplateBase1.Elements.Add(templatedItemElement2);
            itemTemplateBase1.Elements.Add(templatedItemElement3);
            itemTemplateBase1.Name = "templateTable1";
            tableRowDefinition1.Length.Value = 15D;
            tableRowDefinition2.Length.Value = 15D;
            itemTemplateBase1.Rows.Add(tableRowDefinition1);
            itemTemplateBase1.Rows.Add(tableRowDefinition2);
            tableSpan1.RowSpan = 2;
            itemTemplateBase1.Spans.Add(tableSpan1);
            this.listTables.Templates.Add(itemTemplateBase1);
            this.listTables.ValueMember = "DataTable";
            // 
            // bindingTables
            // 
            this.bindingTables.DataSource = typeof(SpreadCommander.Documents.Controls.DataGridControl.TableData);
            this.bindingTables.CurrentChanged += new System.EventHandler(this.BindingTables_CurrentChanged);
            // 
            // searchTables
            // 
            this.searchTables.Client = this.listTables;
            this.searchTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchTables.Location = new System.Drawing.Point(0, 0);
            this.searchTables.Name = "searchTables";
            this.searchTables.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.searchTables.Properties.Client = this.listTables;
            this.searchTables.Size = new System.Drawing.Size(240, 20);
            this.searchTables.TabIndex = 0;
            // 
            // popupGrid
            // 
            this.popupGrid.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyWithHeaders),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyWithoutHeaders),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyPlainDataWithHeaders, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyPlainDataWithoutheaders),
            new DevExpress.XtraBars.LinkPersistInfo(this.barMenuFormatting, true)});
            this.popupGrid.Manager = this.barManager;
            this.popupGrid.Name = "popupGrid";
            // 
            // barCopyWithHeaders
            // 
            this.barCopyWithHeaders.Caption = "Copy with headers";
            this.barCopyWithHeaders.Id = 0;
            this.barCopyWithHeaders.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCopyWithHeaders.ImageOptions.SvgImage")));
            this.barCopyWithHeaders.Name = "barCopyWithHeaders";
            this.barCopyWithHeaders.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopyWithHeaders_ItemClick);
            // 
            // barCopyWithoutHeaders
            // 
            this.barCopyWithoutHeaders.Caption = "Copy without headers";
            this.barCopyWithoutHeaders.Id = 1;
            this.barCopyWithoutHeaders.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCopyWithoutHeaders.ImageOptions.SvgImage")));
            this.barCopyWithoutHeaders.Name = "barCopyWithoutHeaders";
            this.barCopyWithoutHeaders.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopyWithoutHeaders_ItemClick);
            // 
            // barCopyPlainDataWithHeaders
            // 
            this.barCopyPlainDataWithHeaders.Caption = "Copy plain data with headers";
            this.barCopyPlainDataWithHeaders.Id = 2;
            this.barCopyPlainDataWithHeaders.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCopyPlainDataWithHeaders.ImageOptions.SvgImage")));
            this.barCopyPlainDataWithHeaders.Name = "barCopyPlainDataWithHeaders";
            this.barCopyPlainDataWithHeaders.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopyPlainDataWithHeaders_ItemClick);
            // 
            // barCopyPlainDataWithoutheaders
            // 
            this.barCopyPlainDataWithoutheaders.Caption = "Copy plain data without headers";
            this.barCopyPlainDataWithoutheaders.Id = 3;
            this.barCopyPlainDataWithoutheaders.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCopyPlainDataWithoutheaders.ImageOptions.SvgImage")));
            this.barCopyPlainDataWithoutheaders.Name = "barCopyPlainDataWithoutheaders";
            this.barCopyPlainDataWithoutheaders.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopyPlainDataWithoutheaders_ItemClick);
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barCopyWithHeaders,
            this.barCopyWithoutHeaders,
            this.barCopyPlainDataWithHeaders,
            this.barCopyPlainDataWithoutheaders,
            this.barMenuFormatting});
            this.barManager.MaxItemId = 5;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1156, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 751);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1156, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 751);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1156, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 751);
            // 
            // splitterGrid
            // 
            this.splitterGrid.Location = new System.Drawing.Point(300, 0);
            this.splitterGrid.Name = "splitterGrid";
            this.splitterGrid.Size = new System.Drawing.Size(10, 751);
            this.splitterGrid.TabIndex = 8;
            this.splitterGrid.TabStop = false;
            // 
            // transitionManager
            // 
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.Control = this.gridTable;
            transition1.LineWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.LineWaitingIndicatorProperties.Caption = "";
            transition1.LineWaitingIndicatorProperties.Description = "";
            transition1.RingWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.RingWaitingIndicatorProperties.Caption = "";
            transition1.RingWaitingIndicatorProperties.Description = "";
            transition1.WaitingIndicatorProperties.Caption = "";
            transition1.WaitingIndicatorProperties.Description = "";
            this.transitionManager.Transitions.Add(transition1);
            // 
            // barMenuFormatting
            // 
            this.barMenuFormatting.Caption = "Manage formatting rules ...";
            this.barMenuFormatting.Id = 4;
            this.barMenuFormatting.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barMenuFormatting.ImageOptions.SvgImage")));
            this.barMenuFormatting.Name = "barMenuFormatting";
            toolTipTitleItem1.Text = "Formatting";
            toolTipItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage")));
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Manage conditional formatting";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.barMenuFormatting.SuperTip = superToolTip1;
            this.barMenuFormatting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarMenuFormatting_ItemClick);
            // 
            // DataGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridTable);
            this.Controls.Add(this.splitterGrid);
            this.Controls.Add(this.navLeft);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "DataGridControl";
            this.Size = new System.Drawing.Size(1156, 751);
            ((System.ComponentModel.ISupportInitialize)(this.gridTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBlobEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemStringEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navLeft)).EndInit();
            this.navLeft.ResumeLayout(false);
            this.pageTables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listTables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchTables.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.Navigation.NavigationPane navLeft;
		private DevExpress.XtraBars.Navigation.NavigationPage pageTables;
		private DevExpress.XtraGrid.GridControl gridTable;
		private DevExpress.XtraGrid.Views.Grid.GridView viewTable;
		private DevExpress.XtraEditors.ImageListBoxControl listTables;
		private DevExpress.XtraEditors.SearchControl searchTables;
		private System.Windows.Forms.BindingSource bindingTables;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemBlobEditor;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemStringEditor;
		private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEditor;
		private DevExpress.XtraBars.PopupMenu popupGrid;
		private DevExpress.XtraBars.BarButtonItem barCopyWithHeaders;
		private DevExpress.XtraBars.BarButtonItem barCopyWithoutHeaders;
		private DevExpress.XtraBars.BarButtonItem barCopyPlainDataWithHeaders;
		private DevExpress.XtraBars.BarButtonItem barCopyPlainDataWithoutheaders;
		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraEditors.SplitterControl splitterGrid;
		private DevExpress.Utils.Animation.TransitionManager transitionManager;
        private DevExpress.XtraBars.BarButtonItem barMenuFormatting;
    }
}
