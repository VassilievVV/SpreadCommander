namespace SpreadCommander.Documents.Console
{
    partial class ConsolePivotControl
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
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup1 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsolePivotControl));
            DevExpress.Utils.Animation.Transition transition1 = new DevExpress.Utils.Animation.Transition();
            DevExpress.Utils.Animation.FadeTransition fadeTransition1 = new DevExpress.Utils.Animation.FadeTransition();
            this.PivotGrid = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.Ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.galleryDataSources = new DevExpress.XtraBars.RibbonGalleryBarItem();
            this.barLoadTemplate = new DevExpress.XtraBars.BarButtonItem();
            this.barSaveTemplate = new DevExpress.XtraBars.BarButtonItem();
            this.barPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barPrintPreview = new DevExpress.XtraBars.BarButtonItem();
            this.barFormatConditions = new DevExpress.XtraBars.BarButtonItem();
            this.barCustomization = new DevExpress.XtraBars.BarButtonItem();
            this.barExportSummary = new DevExpress.XtraBars.BarButtonItem();
            this.barExportDrillDown = new DevExpress.XtraBars.BarButtonItem();
            this.barRetrieveFields = new DevExpress.XtraBars.BarButtonItem();
            this.barLayout = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPagePivot = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupDataSource = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupTemplates = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupPrint = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupFormat = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupExport = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.RibbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.images = new DevExpress.Utils.SvgImageCollection(this.components);
            this.dlgOpen = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.dlgSave = new DevExpress.XtraEditors.XtraSaveFileDialog(this.components);
            this.images32 = new DevExpress.Utils.SvgImageCollection(this.components);
            this.transitionManager = new DevExpress.Utils.Animation.TransitionManager(this.components);
            this.navPivot = new DevExpress.XtraBars.Navigation.NavigationPane();
            this.navPageCustomization = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.splitterNavigation = new DevExpress.XtraEditors.SplitterControl();
            ((System.ComponentModel.ISupportInitialize)(this.PivotGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.images)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.images32)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navPivot)).BeginInit();
            this.navPivot.SuspendLayout();
            this.SuspendLayout();
            // 
            // PivotGrid
            // 
            this.PivotGrid.DefaultFilterEditorView = DevExpress.XtraEditors.FilterEditorViewMode.VisualAndText;
            this.PivotGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PivotGrid.Location = new System.Drawing.Point(254, 146);
            this.PivotGrid.MenuManager = this.Ribbon;
            this.PivotGrid.Name = "PivotGrid";
            this.PivotGrid.OptionsChartDataSource.AutoTransposeChart = true;
            this.PivotGrid.OptionsCustomization.AllowFilterInCustomizationForm = true;
            this.PivotGrid.OptionsCustomization.AllowSortInCustomizationForm = true;
            this.PivotGrid.OptionsCustomization.CustomizationFormLayout = DevExpress.XtraPivotGrid.Customization.CustomizationFormLayout.BottomPanelOnly1by4;
            this.PivotGrid.OptionsCustomization.CustomizationFormSearchBoxVisible = true;
            this.PivotGrid.OptionsCustomization.CustomizationFormStyle = DevExpress.XtraPivotGrid.Customization.CustomizationFormStyle.Excel2007;
            this.PivotGrid.OptionsFilterPopup.FieldFilterPopupMode = DevExpress.XtraPivotGrid.FieldFilterPopupMode.Excel;
            this.PivotGrid.OptionsFilterPopup.ToolbarButtons = ((DevExpress.XtraPivotGrid.FilterPopupToolbarButtons)((((((DevExpress.XtraPivotGrid.FilterPopupToolbarButtons.ShowOnlyAvailableItems | DevExpress.XtraPivotGrid.FilterPopupToolbarButtons.ShowNewValues) 
            | DevExpress.XtraPivotGrid.FilterPopupToolbarButtons.IncrementalSearch) 
            | DevExpress.XtraPivotGrid.FilterPopupToolbarButtons.MultiSelection) 
            | DevExpress.XtraPivotGrid.FilterPopupToolbarButtons.RadioMode) 
            | DevExpress.XtraPivotGrid.FilterPopupToolbarButtons.InvertFilter)));
            this.PivotGrid.OptionsLayout.StoreAllOptions = true;
            this.PivotGrid.OptionsLayout.StoreFormatRules = true;
            this.PivotGrid.OptionsLayout.StoreLayoutOptions = true;
            this.PivotGrid.OptionsMenu.EnableFormatRulesMenu = true;
            this.PivotGrid.OptionsView.RowTotalsLocation = DevExpress.XtraPivotGrid.PivotRowTotalsLocation.Tree;
            this.PivotGrid.OptionsView.ShowColumnHeaders = false;
            this.PivotGrid.OptionsView.ShowDataHeaders = false;
            this.PivotGrid.OptionsView.ShowFilterHeaders = false;
            this.PivotGrid.OptionsView.ShowRowHeaders = false;
            this.PivotGrid.OptionsView.ShowTotalsForSingleValues = true;
            this.PivotGrid.Prefilter.ShowOperandTypeIcon = true;
            this.PivotGrid.Size = new System.Drawing.Size(750, 568);
            this.PivotGrid.TabIndex = 2;
            this.PivotGrid.GridLayout += new System.EventHandler(this.PivotGrid_GridLayout);
            this.PivotGrid.CustomFieldSort += new DevExpress.XtraPivotGrid.PivotGridCustomFieldSortEventHandler(this.PivotGrid_CustomFieldSort);
            // 
            // Ribbon
            // 
            this.Ribbon.ExpandCollapseItem.Id = 0;
            this.Ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Ribbon.ExpandCollapseItem,
            this.Ribbon.SearchEditItem,
            this.galleryDataSources,
            this.barLoadTemplate,
            this.barSaveTemplate,
            this.barPrint,
            this.barPrintPreview,
            this.barFormatConditions,
            this.barCustomization,
            this.barExportSummary,
            this.barExportDrillDown,
            this.barRetrieveFields,
            this.barLayout});
            this.Ribbon.Location = new System.Drawing.Point(0, 0);
            this.Ribbon.MaxItemId = 12;
            this.Ribbon.Name = "Ribbon";
            this.Ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPagePivot});
            this.Ribbon.Size = new System.Drawing.Size(1004, 146);
            this.Ribbon.StatusBar = this.RibbonStatusBar;
            // 
            // galleryDataSources
            // 
            this.galleryDataSources.Caption = "Data Sources";
            // 
            // 
            // 
            galleryItemGroup1.Caption = "Tables";
            this.galleryDataSources.Gallery.Groups.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            galleryItemGroup1});
            this.galleryDataSources.Gallery.ImageSize = new System.Drawing.Size(32, 32);
            this.galleryDataSources.Gallery.ShowItemText = true;
            this.galleryDataSources.Gallery.ItemClick += new DevExpress.XtraBars.Ribbon.GalleryItemClickEventHandler(this.GalleryDataSources_ItemClick);
            this.galleryDataSources.Id = 1;
            this.galleryDataSources.Name = "galleryDataSources";
            // 
            // barLoadTemplate
            // 
            this.barLoadTemplate.Caption = "Load template";
            this.barLoadTemplate.Id = 2;
            this.barLoadTemplate.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barLoadTemplate.ImageOptions.SvgImage")));
            this.barLoadTemplate.Name = "barLoadTemplate";
            this.barLoadTemplate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarLoadTemplate_ItemClick);
            // 
            // barSaveTemplate
            // 
            this.barSaveTemplate.Caption = "Save template";
            this.barSaveTemplate.Id = 3;
            this.barSaveTemplate.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barSaveTemplate.ImageOptions.SvgImage")));
            this.barSaveTemplate.Name = "barSaveTemplate";
            this.barSaveTemplate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarSaveTemplate_ItemClick);
            // 
            // barPrint
            // 
            this.barPrint.Caption = "Print";
            this.barPrint.Id = 4;
            this.barPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barPrint.ImageOptions.SvgImage")));
            this.barPrint.Name = "barPrint";
            this.barPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarPrint_ItemClick);
            // 
            // barPrintPreview
            // 
            this.barPrintPreview.Caption = "Preview";
            this.barPrintPreview.Id = 5;
            this.barPrintPreview.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barPrintPreview.ImageOptions.SvgImage")));
            this.barPrintPreview.Name = "barPrintPreview";
            this.barPrintPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarPrintPreview_ItemClick);
            // 
            // barFormatConditions
            // 
            this.barFormatConditions.Caption = "Format conditions";
            this.barFormatConditions.Id = 6;
            this.barFormatConditions.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barFormatConditions.ImageOptions.SvgImage")));
            this.barFormatConditions.Name = "barFormatConditions";
            this.barFormatConditions.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarFormatConditions_ItemClick);
            // 
            // barCustomization
            // 
            this.barCustomization.Caption = "Customization";
            this.barCustomization.Id = 7;
            this.barCustomization.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barCustomization.ImageOptions.SvgImage")));
            this.barCustomization.Name = "barCustomization";
            this.barCustomization.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCustomization_ItemClick);
            this.barCustomization.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barExportSummary
            // 
            this.barExportSummary.Caption = "Summary";
            this.barExportSummary.Id = 8;
            this.barExportSummary.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barExportSummary.ImageOptions.SvgImage")));
            this.barExportSummary.Name = "barExportSummary";
            this.barExportSummary.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarExportSummary_ItemClick);
            // 
            // barExportDrillDown
            // 
            this.barExportDrillDown.Caption = "DrillDown";
            this.barExportDrillDown.Id = 9;
            this.barExportDrillDown.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barExportDrillDown.ImageOptions.SvgImage")));
            this.barExportDrillDown.Name = "barExportDrillDown";
            this.barExportDrillDown.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarExportDrillDown_ItemClick);
            // 
            // barRetrieveFields
            // 
            this.barRetrieveFields.Caption = "Retrieve fields";
            this.barRetrieveFields.Id = 10;
            this.barRetrieveFields.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barRetrieveFields.ImageOptions.SvgImage")));
            this.barRetrieveFields.Name = "barRetrieveFields";
            this.barRetrieveFields.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarRetrieveFields_ItemClick);
            // 
            // barLayout
            // 
            this.barLayout.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.barLayout.Caption = "Compact";
            this.barLayout.Id = 11;
            this.barLayout.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barLayout.ImageOptions.SvgImage")));
            this.barLayout.Name = "barLayout";
            this.barLayout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarLayout_ItemClick);
            // 
            // ribbonPagePivot
            // 
            this.ribbonPagePivot.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupDataSource,
            this.ribbonPageGroupTemplates,
            this.ribbonPageGroupPrint,
            this.ribbonPageGroupFormat,
            this.ribbonPageGroupExport});
            this.ribbonPagePivot.Name = "ribbonPagePivot";
            this.ribbonPagePivot.Text = "Pivot";
            // 
            // ribbonPageGroupDataSource
            // 
            this.ribbonPageGroupDataSource.ItemLinks.Add(this.galleryDataSources);
            this.ribbonPageGroupDataSource.Name = "ribbonPageGroupDataSource";
            this.ribbonPageGroupDataSource.ShowCaptionButton = false;
            this.ribbonPageGroupDataSource.Text = "Data Source";
            // 
            // ribbonPageGroupTemplates
            // 
            this.ribbonPageGroupTemplates.ItemLinks.Add(this.barLoadTemplate);
            this.ribbonPageGroupTemplates.ItemLinks.Add(this.barSaveTemplate);
            this.ribbonPageGroupTemplates.Name = "ribbonPageGroupTemplates";
            this.ribbonPageGroupTemplates.ShowCaptionButton = false;
            this.ribbonPageGroupTemplates.Text = "Templates";
            // 
            // ribbonPageGroupPrint
            // 
            this.ribbonPageGroupPrint.ItemLinks.Add(this.barPrint);
            this.ribbonPageGroupPrint.ItemLinks.Add(this.barPrintPreview);
            this.ribbonPageGroupPrint.Name = "ribbonPageGroupPrint";
            this.ribbonPageGroupPrint.ShowCaptionButton = false;
            this.ribbonPageGroupPrint.Text = "Print";
            // 
            // ribbonPageGroupFormat
            // 
            this.ribbonPageGroupFormat.ItemLinks.Add(this.barLayout);
            this.ribbonPageGroupFormat.ItemLinks.Add(this.barFormatConditions);
            this.ribbonPageGroupFormat.ItemLinks.Add(this.barCustomization);
            this.ribbonPageGroupFormat.Name = "ribbonPageGroupFormat";
            this.ribbonPageGroupFormat.ShowCaptionButton = false;
            this.ribbonPageGroupFormat.Text = "Format";
            // 
            // ribbonPageGroupExport
            // 
            this.ribbonPageGroupExport.ItemLinks.Add(this.barExportSummary);
            this.ribbonPageGroupExport.ItemLinks.Add(this.barExportDrillDown);
            this.ribbonPageGroupExport.Name = "ribbonPageGroupExport";
            this.ribbonPageGroupExport.ShowCaptionButton = false;
            this.ribbonPageGroupExport.Text = "Export";
            // 
            // RibbonStatusBar
            // 
            this.RibbonStatusBar.Location = new System.Drawing.Point(0, 714);
            this.RibbonStatusBar.Name = "RibbonStatusBar";
            this.RibbonStatusBar.Ribbon = this.Ribbon;
            this.RibbonStatusBar.Size = new System.Drawing.Size(1004, 22);
            // 
            // images
            // 
            this.images.Add("table", "image://svgimages/richedit/inserttable.svg");
            this.images.Add("pivot", "image://svgimages/spreadsheet/pivottable.svg");
            // 
            // dlgOpen
            // 
            this.dlgOpen.DefaultExt = "scpivot";
            this.dlgOpen.Filter = "Pivot files (*.scpivot)|*.scpivot|All files (*.*)|All files (*.*)";
            this.dlgOpen.RestoreDirectory = true;
            this.dlgOpen.Title = "Spread Commander";
            // 
            // dlgSave
            // 
            this.dlgSave.DefaultExt = "scpivot";
            this.dlgSave.Filter = "Pivot files (*.scpivot)|*.scpivot|All files (*.*)|All files (*.*)";
            this.dlgSave.RestoreDirectory = true;
            this.dlgSave.Title = "Spread Commander";
            // 
            // images32
            // 
            this.images32.ImageSize = new System.Drawing.Size(32, 32);
            this.images32.Add("table", "image://svgimages/richedit/inserttable.svg");
            this.images32.Add("pivot", "image://svgimages/spreadsheet/pivottable.svg");
            // 
            // transitionManager
            // 
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.Control = this.PivotGrid;
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
            // navPivot
            // 
            this.navPivot.Controls.Add(this.navPageCustomization);
            this.navPivot.Dock = System.Windows.Forms.DockStyle.Left;
            this.navPivot.ItemOrientation = System.Windows.Forms.Orientation.Vertical;
            this.navPivot.Location = new System.Drawing.Point(0, 146);
            this.navPivot.Name = "navPivot";
            this.navPivot.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.navPageCustomization});
            this.navPivot.RegularSize = new System.Drawing.Size(244, 568);
            this.navPivot.SelectedPage = this.navPageCustomization;
            this.navPivot.Size = new System.Drawing.Size(244, 568);
            this.navPivot.TabIndex = 8;
            this.navPivot.Text = "Pivot";
            // 
            // navPageCustomization
            // 
            this.navPageCustomization.Caption = "Customization";
            this.navPageCustomization.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("navPageCustomization.ImageOptions.SvgImage")));
            this.navPageCustomization.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.navPageCustomization.Name = "navPageCustomization";
            this.navPageCustomization.PageText = "";
            this.navPageCustomization.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.navPageCustomization.Size = new System.Drawing.Size(181, 495);
            // 
            // splitterNavigation
            // 
            this.splitterNavigation.Location = new System.Drawing.Point(244, 146);
            this.splitterNavigation.Name = "splitterNavigation";
            this.splitterNavigation.Size = new System.Drawing.Size(10, 568);
            this.splitterNavigation.TabIndex = 9;
            this.splitterNavigation.TabStop = false;
            // 
            // ConsolePivotControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PivotGrid);
            this.Controls.Add(this.splitterNavigation);
            this.Controls.Add(this.navPivot);
            this.Controls.Add(this.RibbonStatusBar);
            this.Controls.Add(this.Ribbon);
            this.Name = "ConsolePivotControl";
            this.Size = new System.Drawing.Size(1004, 736);
            this.Load += new System.EventHandler(this.ConsolePivotControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PivotGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.images)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.images32)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navPivot)).EndInit();
            this.navPivot.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl Ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPagePivot;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupDataSource;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar RibbonStatusBar;
        private DevExpress.XtraPivotGrid.PivotGridControl PivotGrid;
        private DevExpress.Utils.SvgImageCollection images;
        private DevExpress.XtraEditors.XtraOpenFileDialog dlgOpen;
        private DevExpress.XtraEditors.XtraSaveFileDialog dlgSave;
        private DevExpress.XtraBars.RibbonGalleryBarItem galleryDataSources;
        private DevExpress.Utils.SvgImageCollection images32;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupTemplates;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupPrint;
        private DevExpress.XtraBars.BarButtonItem barLoadTemplate;
        private DevExpress.XtraBars.BarButtonItem barSaveTemplate;
        private DevExpress.XtraBars.BarButtonItem barPrint;
        private DevExpress.XtraBars.BarButtonItem barPrintPreview;
        private DevExpress.XtraBars.BarButtonItem barFormatConditions;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupFormat;
        private DevExpress.XtraBars.BarButtonItem barCustomization;
        private DevExpress.XtraBars.BarButtonItem barExportSummary;
        private DevExpress.XtraBars.BarButtonItem barExportDrillDown;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupExport;
        private DevExpress.Utils.Animation.TransitionManager transitionManager;
        private DevExpress.XtraBars.BarButtonItem barRetrieveFields;
        private DevExpress.XtraBars.Navigation.NavigationPane navPivot;
        private DevExpress.XtraBars.Navigation.NavigationPage navPageCustomization;
        private DevExpress.XtraEditors.SplitterControl splitterNavigation;
        private DevExpress.XtraBars.BarButtonItem barLayout;
    }
}
