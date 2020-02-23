namespace SpreadCommander.Documents.Controls
{
	partial class SQLiteExecutionPlanControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLiteExecutionPlanControl));
            this.treePlan = new DevExpress.XtraTreeList.TreeList();
            this.colDetail = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.bindingExecutionPlan = new System.Windows.Forms.BindingSource(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.pmnuExpandAll = new DevExpress.XtraBars.BarButtonItem();
            this.pmnuCollapseAll = new DevExpress.XtraBars.BarButtonItem();
            this.pmnuTreeView = new DevExpress.XtraBars.BarButtonItem();
            this.pmnuShowWholeStatements = new DevExpress.XtraBars.BarButtonItem();
            this.popupGridPlan = new DevExpress.XtraBars.PopupMenu(this.components);
            this.pmnuManageFormatRules = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.treePlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingExecutionPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridPlan)).BeginInit();
            this.SuspendLayout();
            // 
            // treePlan
            // 
            this.treePlan.AutoFillColumn = this.colDetail;
            this.treePlan.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colDetail});
            this.treePlan.Cursor = System.Windows.Forms.Cursors.Default;
            this.treePlan.DataSource = this.bindingExecutionPlan;
            this.treePlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treePlan.KeyFieldName = "id";
            this.treePlan.Location = new System.Drawing.Point(0, 0);
            this.treePlan.Name = "treePlan";
            this.treePlan.OptionsBehavior.AllowIndeterminateCheckState = true;
            this.treePlan.OptionsBehavior.Editable = false;
            this.treePlan.OptionsBehavior.EditorShowMode = DevExpress.XtraTreeList.TreeListEditorShowMode.MouseDownFocused;
            this.treePlan.OptionsFind.AllowIncrementalSearch = true;
            this.treePlan.OptionsFind.ExpandNodesOnIncrementalSearch = true;
            this.treePlan.OptionsPrint.PrintPreview = true;
            this.treePlan.OptionsView.AutoWidth = false;
            this.treePlan.OptionsView.ShowAutoFilterRow = true;
            this.treePlan.ParentFieldName = "parent";
            this.treePlan.PreviewFieldName = "StmtText";
            this.treePlan.Size = new System.Drawing.Size(769, 462);
            this.treePlan.TabIndex = 0;
            this.treePlan.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreePlan_MouseUp);
            // 
            // colDetail
            // 
            this.colDetail.Caption = "Detail";
            this.colDetail.FieldName = "detail";
            this.colDetail.Name = "colDetail";
            this.colDetail.Visible = true;
            this.colDetail.VisibleIndex = 0;
            this.colDetail.Width = 744;
            // 
            // barManager
            // 
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.pmnuExpandAll,
            this.pmnuCollapseAll,
            this.pmnuTreeView,
            this.pmnuShowWholeStatements,
            this.pmnuManageFormatRules});
            this.barManager.MaxItemId = 6;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(769, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 462);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(769, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 462);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(769, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 462);
            // 
            // pmnuExpandAll
            // 
            this.pmnuExpandAll.Caption = "Expand All";
            this.pmnuExpandAll.Id = 0;
            this.pmnuExpandAll.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("pmnuExpandAll.ImageOptions.SvgImage")));
            this.pmnuExpandAll.Name = "pmnuExpandAll";
            this.pmnuExpandAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PmnuExpandAll_ItemClick);
            // 
            // pmnuCollapseAll
            // 
            this.pmnuCollapseAll.Caption = "Collapse All";
            this.pmnuCollapseAll.Id = 1;
            this.pmnuCollapseAll.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("pmnuCollapseAll.ImageOptions.SvgImage")));
            this.pmnuCollapseAll.Name = "pmnuCollapseAll";
            this.pmnuCollapseAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PmnuCollapseAll_ItemClick);
            // 
            // pmnuTreeView
            // 
            this.pmnuTreeView.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.pmnuTreeView.Caption = "Tree View";
            this.pmnuTreeView.Id = 2;
            this.pmnuTreeView.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("pmnuTreeView.ImageOptions.SvgImage")));
            this.pmnuTreeView.Name = "pmnuTreeView";
            this.pmnuTreeView.DownChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.PmnuTreeView_DownChanged);
            // 
            // pmnuShowWholeStatements
            // 
            this.pmnuShowWholeStatements.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.pmnuShowWholeStatements.Caption = "Show Whole Statements";
            this.pmnuShowWholeStatements.Id = 4;
            this.pmnuShowWholeStatements.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("pmnuShowWholeStatements.ImageOptions.SvgImage")));
            this.pmnuShowWholeStatements.Name = "pmnuShowWholeStatements";
            this.pmnuShowWholeStatements.DownChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.PmnuShowWholeStatements_DownChanged);
            // 
            // popupGridPlan
            // 
            this.popupGridPlan.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuExpandAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuCollapseAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuTreeView, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuShowWholeStatements, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuManageFormatRules, true)});
            this.popupGridPlan.Manager = this.barManager;
            this.popupGridPlan.Name = "popupGridPlan";
            this.popupGridPlan.BeforePopup += new System.ComponentModel.CancelEventHandler(this.PopupGridPlan_BeforePopup);
            // 
            // pmnuManageFormatRules
            // 
            this.pmnuManageFormatRules.Caption = "Manage format rules ...";
            this.pmnuManageFormatRules.Id = 5;
            this.pmnuManageFormatRules.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("pmnuManageFormatRules.ImageOptions.SvgImage")));
            this.pmnuManageFormatRules.Name = "pmnuManageFormatRules";
            this.pmnuManageFormatRules.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PmnuManageFormatRules_ItemClick);
            // 
            // SQLiteExecutionPlanControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treePlan);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "SQLiteExecutionPlanControl";
            this.Size = new System.Drawing.Size(769, 462);
            this.Load += new System.EventHandler(this.SQLiteExecutionPlanControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treePlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingExecutionPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridPlan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraTreeList.TreeList treePlan;
		private System.Windows.Forms.BindingSource bindingExecutionPlan;
		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraBars.BarButtonItem pmnuExpandAll;
		private DevExpress.XtraBars.BarButtonItem pmnuCollapseAll;
		private DevExpress.XtraBars.BarButtonItem pmnuTreeView;
		private DevExpress.XtraBars.BarButtonItem pmnuShowWholeStatements;
		private DevExpress.XtraBars.PopupMenu popupGridPlan;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDetail;
        private DevExpress.XtraBars.BarButtonItem pmnuManageFormatRules;
    }
}