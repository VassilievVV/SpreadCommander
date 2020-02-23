namespace SpreadCommander.Documents.Controls
{
    partial class DataSetViewControl
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
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.imagesDocuments = new DevExpress.Utils.SvgImageCollection(this.components);
            this.viewTableViews = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barSearch = new DevExpress.XtraBars.BarEditItem();
            this.ribbonPageData = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTableViews)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager
            // 
            this.documentManager.ContainerControl = this;
            this.documentManager.Images = this.imagesDocuments;
            this.documentManager.SnapMode = ((DevExpress.Utils.Controls.SnapMode)((((DevExpress.Utils.Controls.SnapMode.OwnerControl | DevExpress.Utils.Controls.SnapMode.OwnerForm) 
            | DevExpress.Utils.Controls.SnapMode.Screens) 
            | DevExpress.Utils.Controls.SnapMode.SnapForms)));
            this.documentManager.View = this.viewTableViews;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.viewTableViews});
            this.documentManager.DocumentActivate += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.DocumentManager_DocumentActivate);
            // 
            // imagesDocuments
            // 
            this.imagesDocuments.ImageSize = new System.Drawing.Size(16, 16);
            this.imagesDocuments.Add("inserttable", "image://svgimages/richedit/inserttable.svg");
            // 
            // viewTableViews
            // 
            this.viewTableViews.AllowHotkeyNavigation = DevExpress.Utils.DefaultBoolean.True;
            this.viewTableViews.AllowResetLayout = DevExpress.Utils.DefaultBoolean.True;
            this.viewTableViews.DocumentGroupProperties.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Left;
            this.viewTableViews.DocumentProperties.AllowClose = false;
            this.viewTableViews.DocumentProperties.AllowPin = true;
            this.viewTableViews.EnableFreeLayoutMode = DevExpress.Utils.DefaultBoolean.True;
            this.viewTableViews.EnableStickySplitters = DevExpress.Utils.DefaultBoolean.True;
            this.viewTableViews.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.viewTableViews.RootContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.viewTableViews.ShowDockGuidesOnPressingShift = DevExpress.Utils.DefaultBoolean.False;
            this.viewTableViews.ShowDocumentSelectorMenuOnCtrlAltDownArrow = DevExpress.Utils.DefaultBoolean.True;
            this.viewTableViews.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.True;
            this.viewTableViews.UseLoadingIndicator = DevExpress.Utils.DefaultBoolean.False;
            this.viewTableViews.UseSnappingEmulation = DevExpress.Utils.DefaultBoolean.True;
            this.viewTableViews.TabMouseActivating += new DevExpress.XtraBars.Docking2010.Views.DocumentCancelEventHandler(this.ViewTableViews_TabMouseActivating);
            this.viewTableViews.DocumentActivated += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewTableViews_DocumentActivated);
            this.viewTableViews.DocumentDeactivated += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewTableViews_DocumentDeactivated);
            this.viewTableViews.Floating += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewTableViews_Floating);
            this.viewTableViews.EndFloating += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewTableViews_EndFloating);
            // 
            // ribbonControl
            // 
            this.ribbonControl.AutoHideEmptyItems = true;
            this.ribbonControl.AutoSizeItems = true;
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.ribbonControl.SearchEditItem,
            this.barSearch});
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.MaxItemId = 15;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageData});
            this.ribbonControl.PopupShowMode = DevExpress.XtraBars.PopupShowMode.Classic;
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.ribbonControl.ShowSearchItem = true;
            this.ribbonControl.Size = new System.Drawing.Size(1039, 146);
            this.ribbonControl.StatusBar = this.ribbonStatusBar;
            this.ribbonControl.Visible = false;
            // 
            // barSearch
            // 
            this.barSearch.Edit = null;
            this.barSearch.EditWidth = 200;
            this.barSearch.Id = 9;
            this.barSearch.Name = "barSearch";
            // 
            // ribbonPageData
            // 
            this.ribbonPageData.Name = "ribbonPageData";
            this.ribbonPageData.Text = "Data";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 746);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonControl;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1039, 22);
            this.ribbonStatusBar.Visible = false;
            // 
            // DataSetViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbonControl);
            this.Name = "DataSetViewControl";
            this.Size = new System.Drawing.Size(1039, 768);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTableViews)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView viewTableViews;
        private DevExpress.Utils.SvgImageCollection imagesDocuments;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
        private DevExpress.XtraBars.BarEditItem barSearch;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageData;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
    }
}
