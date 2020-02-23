namespace SpreadCommander.Documents.Views
{
	partial class PictureDocumentView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PictureDocumentView));
            this.mvvmContext = new DevExpress.Utils.MVVM.MVVMContext(this.components);
            this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barZoomOut = new DevExpress.XtraBars.BarButtonItem();
            this.barZoomIn = new DevExpress.XtraBars.BarButtonItem();
            this.barZoom100 = new DevExpress.XtraBars.BarButtonItem();
            this.barZoomPage = new DevExpress.XtraBars.BarButtonItem();
            this.barZoom = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemZoomTrackBar = new DevExpress.XtraEditors.Repository.RepositoryItemZoomTrackBar();
            this.ribbonPagePicture = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupFile = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupPrint = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupZoom = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.Picture = new DevExpress.XtraEditors.PictureEdit();
            this.dlgOpen = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.svgFormIcon = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mvvmContext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemZoomTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgFormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // mvvmContext
            // 
            this.mvvmContext.ContainerControl = this;
            this.mvvmContext.ViewModelType = typeof(SpreadCommander.Documents.ViewModels.PictureDocumentViewModel);
            this.mvvmContext.ViewModelSet += new DevExpress.Utils.MVVM.ViewModelSetEventHandler(this.MvvmContext_ViewModelSet);
            this.mvvmContext.ViewModelCreate += new DevExpress.Utils.MVVM.ViewModelCreateEventHandler(this.MvvmContext_ViewModelCreate);
            // 
            // ribbonControl
            // 
            this.ribbonControl.AutoSizeItems = true;
            this.ribbonControl.ExpandCollapseItem.Id = 0;
            this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.barOpen,
            this.barPrint,
            this.barZoomOut,
            this.barZoomIn,
            this.barZoom100,
            this.barZoomPage,
            this.barZoom,
            this.ribbonControl.SearchEditItem});
            this.ribbonControl.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl.MaxItemId = 10;
            this.ribbonControl.Name = "ribbonControl";
            this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPagePicture});
            this.ribbonControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemZoomTrackBar});
            this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.ribbonControl.ShowSearchItem = true;
            this.ribbonControl.Size = new System.Drawing.Size(1158, 154);
            this.ribbonControl.StatusBar = this.ribbonStatusBar;
            // 
            // barOpen
            // 
            this.barOpen.Caption = "Open";
            this.barOpen.Id = 1;
            this.barOpen.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barOpen.ImageOptions.SvgImage")));
            this.barOpen.Name = "barOpen";
            this.barOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarOpen_ItemClick);
            // 
            // barPrint
            // 
            this.barPrint.Caption = "Print";
            this.barPrint.Id = 2;
            this.barPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barPrint.ImageOptions.SvgImage")));
            this.barPrint.Name = "barPrint";
            this.barPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarPrint_ItemClick);
            // 
            // barZoomOut
            // 
            this.barZoomOut.Caption = "Zoom Out";
            this.barZoomOut.Id = 5;
            this.barZoomOut.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barZoomOut.ImageOptions.SvgImage")));
            this.barZoomOut.Name = "barZoomOut";
            this.barZoomOut.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarZoomOut_ItemClick);
            // 
            // barZoomIn
            // 
            this.barZoomIn.Caption = "Zoom In";
            this.barZoomIn.Id = 6;
            this.barZoomIn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barZoomIn.ImageOptions.SvgImage")));
            this.barZoomIn.Name = "barZoomIn";
            this.barZoomIn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarZoomIn_ItemClick);
            // 
            // barZoom100
            // 
            this.barZoom100.Caption = "Zoom 100%";
            this.barZoom100.Id = 7;
            this.barZoom100.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barZoom100.ImageOptions.SvgImage")));
            this.barZoom100.Name = "barZoom100";
            this.barZoom100.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarZoom100_ItemClick);
            // 
            // barZoomPage
            // 
            this.barZoomPage.Caption = "Zoom Page";
            this.barZoomPage.Id = 8;
            this.barZoomPage.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barZoomPage.ImageOptions.SvgImage")));
            this.barZoomPage.Name = "barZoomPage";
            this.barZoomPage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarZoomPage_ItemClick);
            // 
            // barZoom
            // 
            this.barZoom.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barZoom.Caption = "Zoom:";
            this.barZoom.Edit = this.repositoryItemZoomTrackBar;
            this.barZoom.EditValue = "100";
            this.barZoom.EditWidth = 150;
            this.barZoom.Id = 9;
            this.barZoom.Name = "barZoom";
            // 
            // repositoryItemZoomTrackBar
            // 
            this.repositoryItemZoomTrackBar.LargeChange = 30;
            this.repositoryItemZoomTrackBar.Maximum = 190;
            this.repositoryItemZoomTrackBar.Middle = 100;
            this.repositoryItemZoomTrackBar.Minimum = 10;
            this.repositoryItemZoomTrackBar.Name = "repositoryItemZoomTrackBar";
            this.repositoryItemZoomTrackBar.SmallChange = 5;
            this.repositoryItemZoomTrackBar.EditValueChanged += new System.EventHandler(this.RepositoryItemZoomTrackBar_EditValueChanged);
            // 
            // ribbonPagePicture
            // 
            this.ribbonPagePicture.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupFile,
            this.ribbonPageGroupPrint,
            this.ribbonPageGroupZoom});
            this.ribbonPagePicture.Name = "ribbonPagePicture";
            this.ribbonPagePicture.Text = "Picture";
            // 
            // ribbonPageGroupFile
            // 
            this.ribbonPageGroupFile.ItemLinks.Add(this.barOpen);
            this.ribbonPageGroupFile.Name = "ribbonPageGroupFile";
            this.ribbonPageGroupFile.ShowCaptionButton = false;
            this.ribbonPageGroupFile.Text = "File";
            // 
            // ribbonPageGroupPrint
            // 
            this.ribbonPageGroupPrint.ItemLinks.Add(this.barPrint);
            this.ribbonPageGroupPrint.Name = "ribbonPageGroupPrint";
            this.ribbonPageGroupPrint.ShowCaptionButton = false;
            this.ribbonPageGroupPrint.Text = "Print";
            // 
            // ribbonPageGroupZoom
            // 
            this.ribbonPageGroupZoom.ItemLinks.Add(this.barZoomOut);
            this.ribbonPageGroupZoom.ItemLinks.Add(this.barZoomIn);
            this.ribbonPageGroupZoom.ItemLinks.Add(this.barZoom100);
            this.ribbonPageGroupZoom.ItemLinks.Add(this.barZoomPage);
            this.ribbonPageGroupZoom.ItemsLayout = DevExpress.XtraBars.Ribbon.RibbonPageGroupItemsLayout.TwoRows;
            this.ribbonPageGroupZoom.Name = "ribbonPageGroupZoom";
            this.ribbonPageGroupZoom.ShowCaptionButton = false;
            this.ribbonPageGroupZoom.Text = "Zoom";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barZoom);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 911);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonControl;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1158, 22);
            // 
            // Picture
            // 
            this.Picture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Picture.Location = new System.Drawing.Point(0, 154);
            this.Picture.MenuManager = this.ribbonControl;
            this.Picture.Name = "Picture";
            this.Picture.Properties.ReadOnly = true;
            this.Picture.Properties.ShowScrollBars = true;
            this.Picture.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            this.Picture.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.Picture.Size = new System.Drawing.Size(1158, 757);
            this.Picture.TabIndex = 2;
            this.Picture.ZoomPercentChanged += new System.EventHandler(this.Picture_ZoomPercentChanged);
            // 
            // dlgOpen
            // 
            this.dlgOpen.DefaultExt = "png";
            this.dlgOpen.Filter = "Images (*.png, *.tif, *.tiff, *.jpg, *.jpeg, *.gif, *.bmp)|*.png,*.tif,*.tiff,*.j" +
    "pg,*.jpeg,*.gif,*.bmp|All files (*.*)|*.*";
            this.dlgOpen.RestoreDirectory = true;
            this.dlgOpen.Title = "Select picture";
            // 
            // svgFormIcon
            // 
            this.svgFormIcon.Add("insertimage", "image://svgimages/richedit/insertimage.svg");
            // 
            // PictureDocumentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 933);
            this.Controls.Add(this.Picture);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbonControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PictureDocumentView";
            this.Ribbon = this.ribbonControl;
            this.StatusBar = this.ribbonStatusBar;
            this.Load += new System.EventHandler(this.PictureDocumentView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mvvmContext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemZoomTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgFormIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private DevExpress.Utils.MVVM.MVVMContext mvvmContext;
		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPagePicture;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupFile;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
		private DevExpress.XtraEditors.PictureEdit Picture;
		private DevExpress.XtraBars.BarButtonItem barOpen;
		private DevExpress.XtraBars.BarButtonItem barPrint;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupPrint;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupZoom;
		private DevExpress.XtraBars.BarButtonItem barZoomOut;
		private DevExpress.XtraBars.BarButtonItem barZoomIn;
		private DevExpress.XtraBars.BarButtonItem barZoom100;
		private DevExpress.XtraBars.BarButtonItem barZoomPage;
		private DevExpress.XtraEditors.XtraOpenFileDialog dlgOpen;
		private DevExpress.XtraBars.BarEditItem barZoom;
		private DevExpress.XtraEditors.Repository.RepositoryItemZoomTrackBar repositoryItemZoomTrackBar;
		private DevExpress.Utils.SvgImageCollection svgFormIcon;
	}
}
