namespace SpreadCommander.Documents.Console
{
	partial class ConsoleOutputControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleOutputControl));
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            DevExpress.Utils.Animation.Transition transition1 = new DevExpress.Utils.Animation.Transition();
            DevExpress.Utils.Animation.FadeTransition fadeTransition1 = new DevExpress.Utils.Animation.FadeTransition();
            this.documentGroupMain = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.docBook = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.docSpreadsheet = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.docGrid = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.docHeap = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.panelConsole = new DevExpress.XtraEditors.XtraUserControl();
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.progressMarqueeBar = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutConsole = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlProgressBar = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlProgressMarquee = new DevExpress.XtraLayout.LayoutControlItem();
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.viewDocuments = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.transitionManager = new DevExpress.Utils.Animation.TransitionManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroupMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docBook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docSpreadsheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docHeap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressMarqueeBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutConsole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProgressMarquee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewDocuments)).BeginInit();
            this.SuspendLayout();
            // 
            // documentGroupMain
            // 
            this.documentGroupMain.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
            this.docBook,
            this.docSpreadsheet,
            this.docGrid,
            this.docHeap});
            // 
            // docBook
            // 
            this.docBook.Caption = "Book";
            this.docBook.ControlName = "Book";
            this.docBook.Footer = "";
            this.docBook.Header = "";
            this.docBook.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("docBook.ImageOptions.SvgImage")));
            this.docBook.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.docBook.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            // 
            // docSpreadsheet
            // 
            this.docSpreadsheet.Caption = "Spreadsheet";
            this.docSpreadsheet.ControlName = "Spreadsheet";
            this.docSpreadsheet.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("docSpreadsheet.ImageOptions.SvgImage")));
            this.docSpreadsheet.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.docSpreadsheet.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            // 
            // docGrid
            // 
            this.docGrid.Caption = "Data";
            this.docGrid.ControlName = "Grid";
            this.docGrid.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("docGrid.ImageOptions.SvgImage")));
            this.docGrid.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.docGrid.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            // 
            // docHeap
            // 
            this.docHeap.Caption = "Heap";
            this.docHeap.ControlName = "Heap";
            this.docHeap.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("docHeap.ImageOptions.SvgImage")));
            this.docHeap.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.docHeap.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            // 
            // panelConsole
            // 
            this.panelConsole.Location = new System.Drawing.Point(12, 12);
            this.panelConsole.Name = "panelConsole";
            this.panelConsole.Size = new System.Drawing.Size(960, 887);
            this.panelConsole.TabIndex = 13;
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.panelConsole);
            this.layoutControl.Controls.Add(this.progressBar);
            this.layoutControl.Controls.Add(this.progressMarqueeBar);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2344, 780, 650, 400);
            this.layoutControl.Root = this.layoutControlGroup1;
            this.layoutControl.Size = new System.Drawing.Size(984, 955);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl1";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 933);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(960, 10);
            this.progressBar.StyleController = this.layoutControl;
            this.progressBar.TabIndex = 11;
            // 
            // progressMarqueeBar
            // 
            this.progressMarqueeBar.EditValue = 0;
            this.progressMarqueeBar.Location = new System.Drawing.Point(12, 903);
            this.progressMarqueeBar.Name = "progressMarqueeBar";
            this.progressMarqueeBar.Size = new System.Drawing.Size(960, 10);
            this.progressMarqueeBar.StyleController = this.layoutControl;
            this.progressMarqueeBar.TabIndex = 8;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutConsole,
            this.layoutControlProgressBar,
            this.layoutControlProgressMarquee});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(984, 955);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutConsole
            // 
            this.layoutConsole.Control = this.panelConsole;
            this.layoutConsole.Location = new System.Drawing.Point(0, 0);
            this.layoutConsole.Name = "layoutConsole";
            this.layoutConsole.Size = new System.Drawing.Size(964, 891);
            this.layoutConsole.TextSize = new System.Drawing.Size(0, 0);
            this.layoutConsole.TextVisible = false;
            // 
            // layoutControlProgressBar
            // 
            this.layoutControlProgressBar.Control = this.progressBar;
            this.layoutControlProgressBar.Location = new System.Drawing.Point(0, 905);
            this.layoutControlProgressBar.MaxSize = new System.Drawing.Size(0, 30);
            this.layoutControlProgressBar.MinSize = new System.Drawing.Size(54, 30);
            this.layoutControlProgressBar.Name = "layoutControlProgressBar";
            this.layoutControlProgressBar.Size = new System.Drawing.Size(964, 30);
            this.layoutControlProgressBar.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlProgressBar.Text = "Progress: ";
            this.layoutControlProgressBar.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlProgressBar.TextSize = new System.Drawing.Size(49, 13);
            this.layoutControlProgressBar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlProgressMarquee
            // 
            this.layoutControlProgressMarquee.Control = this.progressMarqueeBar;
            this.layoutControlProgressMarquee.Location = new System.Drawing.Point(0, 891);
            this.layoutControlProgressMarquee.MaxSize = new System.Drawing.Size(0, 14);
            this.layoutControlProgressMarquee.MinSize = new System.Drawing.Size(54, 14);
            this.layoutControlProgressMarquee.Name = "layoutControlProgressMarquee";
            this.layoutControlProgressMarquee.Size = new System.Drawing.Size(964, 14);
            this.layoutControlProgressMarquee.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlProgressMarquee.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlProgressMarquee.TextVisible = false;
            this.layoutControlProgressMarquee.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // documentManager
            // 
            this.documentManager.ContainerControl = this.panelConsole;
            this.documentManager.View = this.viewDocuments;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.viewDocuments});
            this.documentManager.DocumentActivate += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.DocumentManager_DocumentActivate);
            // 
            // viewDocuments
            // 
            this.viewDocuments.AllowHotkeyNavigation = DevExpress.Utils.DefaultBoolean.True;
            this.viewDocuments.AllowResetLayout = DevExpress.Utils.DefaultBoolean.False;
            this.viewDocuments.DocumentGroups.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup[] {
            this.documentGroupMain});
            this.viewDocuments.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.docGrid,
            this.docHeap,
            this.docSpreadsheet,
            this.docBook});
            this.viewDocuments.EnableFreeLayoutMode = DevExpress.Utils.DefaultBoolean.True;
            this.viewDocuments.EnableStickySplitters = DevExpress.Utils.DefaultBoolean.True;
            dockingContainer1.Element = this.documentGroupMain;
            this.viewDocuments.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            dockingContainer1});
            this.viewDocuments.ShowDockGuidesOnPressingShift = DevExpress.Utils.DefaultBoolean.False;
            this.viewDocuments.ShowDocumentSelectorMenuOnCtrlAltDownArrow = DevExpress.Utils.DefaultBoolean.True;
            this.viewDocuments.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.True;
            this.viewDocuments.UseLoadingIndicator = DevExpress.Utils.DefaultBoolean.False;
            this.viewDocuments.UseSnappingEmulation = DevExpress.Utils.DefaultBoolean.True;
            this.viewDocuments.TabMouseActivating += new DevExpress.XtraBars.Docking2010.Views.DocumentCancelEventHandler(this.ViewDocuments_TabMouseActivating);
            this.viewDocuments.DocumentActivated += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewDocuments_DocumentActivated);
            this.viewDocuments.DocumentDeactivated += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewDocuments_DocumentDeactivated);
            this.viewDocuments.Floating += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewDocuments_Floating);
            this.viewDocuments.EndFloating += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewDocuments_EndFloating);
            this.viewDocuments.EndDocking += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.ViewDocuments_EndDocking);
            this.viewDocuments.QueryControl += new DevExpress.XtraBars.Docking2010.Views.QueryControlEventHandler(this.ViewDocuments_QueryControl);
            // 
            // transitionManager
            // 
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.Control = this.panelConsole;
            transition1.LineWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.LineWaitingIndicatorProperties.Caption = "";
            transition1.LineWaitingIndicatorProperties.Description = "";
            transition1.RingWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.RingWaitingIndicatorProperties.Caption = "";
            transition1.RingWaitingIndicatorProperties.Description = "";
            transition1.ShowWaitingIndicator = DevExpress.Utils.DefaultBoolean.False;
            transition1.TransitionType = fadeTransition1;
            transition1.WaitingIndicatorProperties.Caption = "";
            transition1.WaitingIndicatorProperties.Description = "";
            this.transitionManager.Transitions.Add(transition1);
            // 
            // ConsoleOutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl);
            this.Name = "ConsoleOutputControl";
            this.Size = new System.Drawing.Size(984, 955);
            this.Load += new System.EventHandler(this.ConsoleOutputControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroupMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docBook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docSpreadsheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docHeap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressMarqueeBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutConsole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProgressMarquee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewDocuments)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraEditors.MarqueeProgressBarControl progressMarqueeBar;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlProgressMarquee;
		private DevExpress.XtraEditors.ProgressBarControl progressBar;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlProgressBar;
		private DevExpress.XtraEditors.XtraUserControl panelConsole;
		private DevExpress.XtraLayout.LayoutControlItem layoutConsole;
		private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView viewDocuments;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroupMain;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document docBook;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document docSpreadsheet;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document docGrid;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document docHeap;
		private DevExpress.Utils.Animation.TransitionManager transitionManager;
	}
}
