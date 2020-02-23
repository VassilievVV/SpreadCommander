namespace SpreadCommander.Documents.Viewers
{
    partial class GridViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridViewer));
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barCopyWithHeaders = new DevExpress.XtraBars.BarButtonItem();
            this.barCopyWithoutHeaders = new DevExpress.XtraBars.BarButtonItem();
            this.barCopyPlainWithHeaders = new DevExpress.XtraBars.BarButtonItem();
            this.barCopyPlainWithoutHeaders = new DevExpress.XtraBars.BarButtonItem();
            this.barMenuFormatting = new DevExpress.XtraBars.BarButtonItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.gridTable = new DevExpress.XtraGrid.GridControl();
            this.viewTable = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemBlobEditor = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemStringEditor = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemDateEditor = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBlobEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemStringEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor.CalendarTimeProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyWithHeaders),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyWithoutHeaders),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyPlainWithHeaders, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopyPlainWithoutHeaders),
            new DevExpress.XtraBars.LinkPersistInfo(this.barMenuFormatting, true)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
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
            this.barCopyWithoutHeaders.Caption = "Copy without header";
            this.barCopyWithoutHeaders.Id = 1;
            this.barCopyWithoutHeaders.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCopyWithoutHeaders.ImageOptions.SvgImage")));
            this.barCopyWithoutHeaders.Name = "barCopyWithoutHeaders";
            this.barCopyWithoutHeaders.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopyWithoutHeaders_ItemClick);
            // 
            // barCopyPlainWithHeaders
            // 
            this.barCopyPlainWithHeaders.Caption = "Copy plain data with headers";
            this.barCopyPlainWithHeaders.Id = 2;
            this.barCopyPlainWithHeaders.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCopyPlainWithHeaders.ImageOptions.SvgImage")));
            this.barCopyPlainWithHeaders.Name = "barCopyPlainWithHeaders";
            this.barCopyPlainWithHeaders.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopyPlainWithHeaders_ItemClick);
            // 
            // barCopyPlainWithoutHeaders
            // 
            this.barCopyPlainWithoutHeaders.Caption = "Copy plain data without headers";
            this.barCopyPlainWithoutHeaders.Id = 3;
            this.barCopyPlainWithoutHeaders.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCopyPlainWithoutHeaders.ImageOptions.SvgImage")));
            this.barCopyPlainWithoutHeaders.Name = "barCopyPlainWithoutHeaders";
            this.barCopyPlainWithoutHeaders.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopyPlainWithoutHeaders_ItemClick);
            // 
            // barMenuFormatting
            // 
            this.barMenuFormatting.Caption = "Manage formatting rules ...";
            this.barMenuFormatting.Id = 4;
            this.barMenuFormatting.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barMenuFormatting.ImageOptions.SvgImage")));
            this.barMenuFormatting.Name = "barMenuFormatting";
            toolTipTitleItem2.Text = "Formatting";
            toolTipItem2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage")));
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Manage conditional formatting";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.barMenuFormatting.SuperTip = superToolTip2;
            this.barMenuFormatting.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarMenuFormatting_ItemClick);
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
            this.barCopyPlainWithHeaders,
            this.barCopyPlainWithoutHeaders,
            this.barMenuFormatting});
            this.barManager.MaxItemId = 5;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1039, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 746);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1039, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 746);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1039, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 746);
            // 
            // gridTable
            // 
            this.gridTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTable.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridTable.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridTable.Location = new System.Drawing.Point(0, 0);
            this.gridTable.MainView = this.viewTable;
            this.gridTable.Name = "gridTable";
            this.gridTable.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemBlobEditor,
            this.repositoryItemStringEditor,
            this.repositoryItemDateEditor});
            this.gridTable.Size = new System.Drawing.Size(1039, 746);
            this.gridTable.TabIndex = 5;
            this.gridTable.UseEmbeddedNavigator = true;
            this.gridTable.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewTable});
            this.gridTable.ViewRegistered += new DevExpress.XtraGrid.ViewOperationEventHandler(this.GridTable_ViewRegistered);
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
            this.viewTable.OptionsDetail.DetailMode = DevExpress.XtraGrid.Views.Grid.DetailMode.Embedded;
            this.viewTable.OptionsFilter.ColumnFilterPopupMode = DevExpress.XtraGrid.Columns.ColumnFilterPopupMode.Excel;
            this.viewTable.OptionsFilter.DefaultFilterEditorView = DevExpress.XtraEditors.FilterEditorViewMode.VisualAndText;
            this.viewTable.OptionsFilter.UseNewCustomFilterDialog = true;
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
            this.viewTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ViewTable_KeyDown);
            this.viewTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewTable_MouseDown);
            this.viewTable.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.ViewTable_MouseWheel);
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
            // GridViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridTable);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "GridViewer";
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBlobEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemStringEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.PopupMenu popupMenu;
        private DevExpress.XtraBars.BarButtonItem barCopyWithHeaders;
        private DevExpress.XtraBars.BarButtonItem barCopyWithoutHeaders;
        private DevExpress.XtraBars.BarButtonItem barCopyPlainWithHeaders;
        private DevExpress.XtraBars.BarButtonItem barCopyPlainWithoutHeaders;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barMenuFormatting;
        private DevExpress.XtraGrid.GridControl gridTable;
        private DevExpress.XtraGrid.Views.Grid.GridView viewTable;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemBlobEditor;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemStringEditor;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEditor;
    }
}
