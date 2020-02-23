namespace SpreadCommander.Documents.Console
{
	partial class ConsoleHeapControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleHeapControl));
            this.Ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barShowFiles = new DevExpress.XtraBars.BarButtonItem();
            this.barPreview = new DevExpress.XtraBars.BarButtonItem();
            this.barOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barNavigationFirst = new DevExpress.XtraBars.BarButtonItem();
            this.barNavigationPrevious = new DevExpress.XtraBars.BarButtonItem();
            this.barNavigationNext = new DevExpress.XtraBars.BarButtonItem();
            this.barNavigationLast = new DevExpress.XtraBars.BarButtonItem();
            this.barViewList = new DevExpress.XtraBars.BarButtonItem();
            this.barViewTiles = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageHeap = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroupFiles = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupNavigation = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroupView = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.Heap = new SpreadCommander.Documents.Controls.HeapControl();
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).BeginInit();
            this.SuspendLayout();
            // 
            // Ribbon
            // 
            this.Ribbon.AutoSizeItems = true;
            this.Ribbon.CommandLayout = DevExpress.XtraBars.Ribbon.CommandLayout.Simplified;
            this.Ribbon.ExpandCollapseItem.Id = 0;
            this.Ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Ribbon.ExpandCollapseItem,
            this.Ribbon.SearchEditItem,
            this.barShowFiles,
            this.barPreview,
            this.barOpen,
            this.barNavigationFirst,
            this.barNavigationPrevious,
            this.barNavigationNext,
            this.barNavigationLast,
            this.barViewList,
            this.barViewTiles});
            this.Ribbon.Location = new System.Drawing.Point(0, 0);
            this.Ribbon.MaxItemId = 10;
            this.Ribbon.Name = "Ribbon";
            this.Ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageHeap});
            this.Ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2019;
            this.Ribbon.ShowSearchItem = true;
            this.Ribbon.Size = new System.Drawing.Size(997, 81);
            this.Ribbon.StatusBar = this.ribbonStatusBar;
            this.Ribbon.Visible = false;
            // 
            // barShowFiles
            // 
            this.barShowFiles.Caption = "Show files";
            this.barShowFiles.Id = 1;
            this.barShowFiles.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barShowFiles.ImageOptions.SvgImage")));
            this.barShowFiles.Name = "barShowFiles";
            this.barShowFiles.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarShowFiles_ItemClick);
            // 
            // barPreview
            // 
            this.barPreview.Caption = "Preview";
            this.barPreview.Id = 2;
            this.barPreview.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barPreview.ImageOptions.SvgImage")));
            this.barPreview.Name = "barPreview";
            this.barPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarPreview_ItemClick);
            // 
            // barOpen
            // 
            this.barOpen.Caption = "Open";
            this.barOpen.Id = 3;
            this.barOpen.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barOpen.ImageOptions.SvgImage")));
            this.barOpen.Name = "barOpen";
            this.barOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarOpen_ItemClick);
            // 
            // barNavigationFirst
            // 
            this.barNavigationFirst.Caption = "First";
            this.barNavigationFirst.Id = 4;
            this.barNavigationFirst.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barNavigationFirst.ImageOptions.SvgImage")));
            this.barNavigationFirst.Name = "barNavigationFirst";
            this.barNavigationFirst.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarNavigationFirst_ItemClick);
            // 
            // barNavigationPrevious
            // 
            this.barNavigationPrevious.Caption = "Previous";
            this.barNavigationPrevious.Id = 5;
            this.barNavigationPrevious.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barNavigationPrevious.ImageOptions.SvgImage")));
            this.barNavigationPrevious.Name = "barNavigationPrevious";
            this.barNavigationPrevious.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarNavigationPrevious_ItemClick);
            // 
            // barNavigationNext
            // 
            this.barNavigationNext.Caption = "Next";
            this.barNavigationNext.Id = 6;
            this.barNavigationNext.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barNavigationNext.ImageOptions.SvgImage")));
            this.barNavigationNext.Name = "barNavigationNext";
            this.barNavigationNext.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarNavigationNext_ItemClick);
            // 
            // barNavigationLast
            // 
            this.barNavigationLast.Caption = "Last";
            this.barNavigationLast.Id = 7;
            this.barNavigationLast.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barNavigationLast.ImageOptions.SvgImage")));
            this.barNavigationLast.Name = "barNavigationLast";
            this.barNavigationLast.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarNavigationLast_ItemClick);
            // 
            // barViewList
            // 
            this.barViewList.Caption = "List";
            this.barViewList.Id = 8;
            this.barViewList.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barViewList.ImageOptions.SvgImage")));
            this.barViewList.Name = "barViewList";
            this.barViewList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarViewList_ItemClick);
            // 
            // barViewTiles
            // 
            this.barViewTiles.Caption = "Tiles";
            this.barViewTiles.Id = 9;
            this.barViewTiles.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barViewTiles.ImageOptions.SvgImage")));
            this.barViewTiles.Name = "barViewTiles";
            this.barViewTiles.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarViewTiles_ItemClick);
            // 
            // ribbonPageHeap
            // 
            this.ribbonPageHeap.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroupFiles,
            this.ribbonPageGroupNavigation,
            this.ribbonPageGroupView});
            this.ribbonPageHeap.Name = "ribbonPageHeap";
            this.ribbonPageHeap.Text = "Heap";
            // 
            // ribbonPageGroupFiles
            // 
            this.ribbonPageGroupFiles.ItemLinks.Add(this.barShowFiles);
            this.ribbonPageGroupFiles.ItemLinks.Add(this.barPreview);
            this.ribbonPageGroupFiles.ItemLinks.Add(this.barOpen);
            this.ribbonPageGroupFiles.Name = "ribbonPageGroupFiles";
            this.ribbonPageGroupFiles.ShowCaptionButton = false;
            this.ribbonPageGroupFiles.Text = "Files";
            // 
            // ribbonPageGroupNavigation
            // 
            this.ribbonPageGroupNavigation.ItemLinks.Add(this.barNavigationFirst);
            this.ribbonPageGroupNavigation.ItemLinks.Add(this.barNavigationPrevious);
            this.ribbonPageGroupNavigation.ItemLinks.Add(this.barNavigationNext);
            this.ribbonPageGroupNavigation.ItemLinks.Add(this.barNavigationLast);
            this.ribbonPageGroupNavigation.Name = "ribbonPageGroupNavigation";
            this.ribbonPageGroupNavigation.ShowCaptionButton = false;
            this.ribbonPageGroupNavigation.Text = "Navigation";
            // 
            // ribbonPageGroupView
            // 
            this.ribbonPageGroupView.ItemLinks.Add(this.barViewList);
            this.ribbonPageGroupView.ItemLinks.Add(this.barViewTiles);
            this.ribbonPageGroupView.Name = "ribbonPageGroupView";
            this.ribbonPageGroupView.ShowCaptionButton = false;
            this.ribbonPageGroupView.Text = "View";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 697);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.Ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(997, 22);
            this.ribbonStatusBar.Visible = false;
            // 
            // Heap
            // 
            this.Heap.CurrentHeapFolder = "Project\\";
            this.Heap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Heap.FileMask = null;
            this.Heap.Location = new System.Drawing.Point(0, 81);
            this.Heap.Name = "Heap";
            this.Heap.Size = new System.Drawing.Size(997, 616);
            this.Heap.TabIndex = 2;
            // 
            // ConsoleHeapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Heap);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.Ribbon);
            this.Name = "ConsoleHeapControl";
            this.Size = new System.Drawing.Size(997, 719);
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.Ribbon.RibbonControl Ribbon;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageHeap;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupFiles;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
		private Controls.HeapControl Heap;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupNavigation;
		private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupView;
		private DevExpress.XtraBars.BarButtonItem barShowFiles;
		private DevExpress.XtraBars.BarButtonItem barPreview;
		private DevExpress.XtraBars.BarButtonItem barOpen;
		private DevExpress.XtraBars.BarButtonItem barNavigationFirst;
		private DevExpress.XtraBars.BarButtonItem barNavigationPrevious;
		private DevExpress.XtraBars.BarButtonItem barNavigationNext;
		private DevExpress.XtraBars.BarButtonItem barNavigationLast;
		private DevExpress.XtraBars.BarButtonItem barViewList;
		private DevExpress.XtraBars.BarButtonItem barViewTiles;
	}
}
