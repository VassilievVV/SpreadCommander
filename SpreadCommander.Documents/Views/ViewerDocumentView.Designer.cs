namespace SpreadCommander.Documents.Views
{
	partial class ViewerDocumentView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerDocumentView));
            this.mvvmContext = new DevExpress.Utils.MVVM.MVVMContext(this.components);
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barSearch = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSearch = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.barZoomIn = new DevExpress.XtraBars.BarButtonItem();
            this.barZoomOut = new DevExpress.XtraBars.BarButtonItem();
            this.barZoom100 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageFile = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupPrint = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupFind = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupZoom = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemSearchControl1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchControl();
            this.svgFormIcon = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mvvmContext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgFormIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // mvvmContext
            // 
            this.mvvmContext.ContainerControl = this;
            this.mvvmContext.ViewModelType = typeof(SpreadCommander.Documents.ViewModels.ViewerDocumentViewModel);
            this.mvvmContext.ViewModelSet += new DevExpress.Utils.MVVM.ViewModelSetEventHandler(this.MvvmContext_ViewModelSet);
            this.mvvmContext.ViewModelCreate += new DevExpress.Utils.MVVM.ViewModelCreateEventHandler(this.MvvmContext_ViewModelCreate);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.AutoSizeItems = true;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barPrint,
            this.barSearch,
            this.barZoomIn,
            this.barZoomOut,
            this.barZoom100,
            this.ribbonControl1.SearchEditItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 7;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageFile});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSearchControl1,
            this.repositoryItemSearch});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.ribbonControl1.ShowSearchItem = true;
            this.ribbonControl1.Size = new System.Drawing.Size(1010, 154);
            // 
            // barPrint
            // 
            this.barPrint.Caption = "Preview";
            this.barPrint.Hint = "Print preview";
            this.barPrint.Id = 1;
            this.barPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barPrint.ImageOptions.SvgImage")));
            this.barPrint.Name = "barPrint";
            this.barPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarPrint_ItemClick);
            // 
            // barSearch
            // 
            this.barSearch.Edit = this.repositoryItemSearch;
            this.barSearch.EditWidth = 200;
            this.barSearch.Id = 3;
            this.barSearch.Name = "barSearch";
            // 
            // repositoryItemSearch
            // 
            this.repositoryItemSearch.AutoHeight = false;
            this.repositoryItemSearch.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)});
            this.repositoryItemSearch.Name = "repositoryItemSearch";
            this.repositoryItemSearch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.RepositoryItemSearch_ButtonClick);
            // 
            // barZoomIn
            // 
            this.barZoomIn.Caption = "Zoom In";
            this.barZoomIn.Id = 4;
            this.barZoomIn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barZoomIn.ImageOptions.SvgImage")));
            this.barZoomIn.Name = "barZoomIn";
            this.barZoomIn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarZoomIn_ItemClick);
            // 
            // barZoomOut
            // 
            this.barZoomOut.Caption = "Zoom Out";
            this.barZoomOut.Id = 5;
            this.barZoomOut.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barZoomOut.ImageOptions.SvgImage")));
            this.barZoomOut.Name = "barZoomOut";
            this.barZoomOut.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarZoomOut_ItemClick);
            // 
            // barZoom100
            // 
            this.barZoom100.Caption = "Zoom 100%";
            this.barZoom100.Id = 6;
            this.barZoom100.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barZoom100.ImageOptions.SvgImage")));
            this.barZoom100.Name = "barZoom100";
            this.barZoom100.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarZoom100_ItemClick);
            // 
            // ribbonPageFile
            // 
            this.ribbonPageFile.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupPrint,
            this.ribbonPageGroupFind,
            this.ribbonPageGroupZoom});
            this.ribbonPageFile.Name = "ribbonPageFile";
            this.ribbonPageFile.Text = "File";
            // 
            // ribbonPageGroupPrint
            // 
            this.ribbonPageGroupPrint.ItemLinks.Add(this.barPrint);
            this.ribbonPageGroupPrint.Name = "ribbonPageGroupPrint";
            this.ribbonPageGroupPrint.ShowCaptionButton = false;
            this.ribbonPageGroupPrint.Text = "Print";
            // 
            // ribbonPageGroupFind
            // 
            this.ribbonPageGroupFind.ItemLinks.Add(this.barSearch);
            this.ribbonPageGroupFind.Name = "ribbonPageGroupFind";
            this.ribbonPageGroupFind.ShowCaptionButton = false;
            this.ribbonPageGroupFind.Text = "Find";
            // 
            // ribbonPageGroupZoom
            // 
            this.ribbonPageGroupZoom.ItemLinks.Add(this.barZoomIn);
            this.ribbonPageGroupZoom.ItemLinks.Add(this.barZoomOut);
            this.ribbonPageGroupZoom.ItemLinks.Add(this.barZoom100);
            this.ribbonPageGroupZoom.Name = "ribbonPageGroupZoom";
            this.ribbonPageGroupZoom.ShowCaptionButton = false;
            this.ribbonPageGroupZoom.Text = "Zoom";
            // 
            // repositoryItemSearchControl1
            // 
            this.repositoryItemSearchControl1.AutoHeight = false;
            this.repositoryItemSearchControl1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.repositoryItemSearchControl1.Name = "repositoryItemSearchControl1";
            // 
            // svgFormIcon
            // 
            this.svgFormIcon.Add("viewmergeddata", "image://svgimages/richedit/viewmergeddata.svg");
            // 
            // ViewerDocumentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 750);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewerDocumentView";
            this.Ribbon = this.ribbonControl1;
            ((System.ComponentModel.ISupportInitialize)(this.mvvmContext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgFormIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private DevExpress.Utils.MVVM.MVVMContext mvvmContext;
		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageFile;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupPrint;
		private DevExpress.XtraBars.BarButtonItem barPrint;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupFind;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupZoom;
		private DevExpress.XtraBars.BarEditItem barSearch;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemSearch;
		private DevExpress.XtraEditors.Repository.RepositoryItemSearchControl repositoryItemSearchControl1;
		private DevExpress.XtraBars.BarButtonItem barZoomIn;
		private DevExpress.XtraBars.BarButtonItem barZoomOut;
		private DevExpress.XtraBars.BarButtonItem barZoom100;
		private DevExpress.Utils.SvgImageCollection svgFormIcon;
	}
}
