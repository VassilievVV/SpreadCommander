namespace SpreadCommander.Documents.Controls
{
	partial class SqlExecutionPlanControl
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
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule1 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleDataBar formatConditionRuleDataBar1 = new DevExpress.XtraEditors.FormatConditionRuleDataBar();
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule2 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleDataBar formatConditionRuleDataBar2 = new DevExpress.XtraEditors.FormatConditionRuleDataBar();
            DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule treeListFormatRule3 = new DevExpress.XtraTreeList.StyleFormatConditions.TreeListFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleDataBar formatConditionRuleDataBar3 = new DevExpress.XtraEditors.FormatConditionRuleDataBar();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlExecutionPlanControl));
            this.colTotalSubtreeCost = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEstimateIO = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEstimateCPU = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treePlan = new DevExpress.XtraTreeList.TreeList();
            this.colLogicalOp = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colPhysicalOp = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEstimateRows = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEstimateExecutions = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colAvgRowSize = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colArgument = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDefinedValues = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOutputList = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colWarnings = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParallel = new DevExpress.XtraTreeList.Columns.TreeListColumn();
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
            this.barManageFormatRules = new DevExpress.XtraBars.BarButtonItem();
            this.popupGridPlan = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treePlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingExecutionPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupGridPlan)).BeginInit();
            this.SuspendLayout();
            // 
            // colTotalSubtreeCost
            // 
            this.colTotalSubtreeCost.Caption = "Subtree cost";
            this.colTotalSubtreeCost.FieldName = "TotalSubtreeCost";
            this.colTotalSubtreeCost.Fixed = DevExpress.XtraTreeList.Columns.FixedStyle.Left;
            this.colTotalSubtreeCost.Name = "colTotalSubtreeCost";
            this.colTotalSubtreeCost.Visible = true;
            this.colTotalSubtreeCost.VisibleIndex = 1;
            this.colTotalSubtreeCost.Width = 120;
            // 
            // colEstimateIO
            // 
            this.colEstimateIO.Caption = "IO";
            this.colEstimateIO.FieldName = "EstimateIO";
            this.colEstimateIO.Name = "colEstimateIO";
            this.colEstimateIO.Visible = true;
            this.colEstimateIO.VisibleIndex = 3;
            this.colEstimateIO.Width = 100;
            // 
            // colEstimateCPU
            // 
            this.colEstimateCPU.Caption = "CPU";
            this.colEstimateCPU.FieldName = "EstimateCPU";
            this.colEstimateCPU.Name = "colEstimateCPU";
            this.colEstimateCPU.Visible = true;
            this.colEstimateCPU.VisibleIndex = 4;
            this.colEstimateCPU.Width = 100;
            // 
            // treePlan
            // 
            this.treePlan.Appearance.Preview.FontStyleDelta = System.Drawing.FontStyle.Italic;
            this.treePlan.Appearance.Preview.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.treePlan.Appearance.Preview.Options.UseFont = true;
            this.treePlan.Appearance.Preview.Options.UseForeColor = true;
            this.treePlan.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colLogicalOp,
            this.colTotalSubtreeCost,
            this.colPhysicalOp,
            this.colEstimateIO,
            this.colEstimateCPU,
            this.colEstimateRows,
            this.colEstimateExecutions,
            this.colAvgRowSize,
            this.colArgument,
            this.colDefinedValues,
            this.colOutputList,
            this.colWarnings,
            this.colType,
            this.colParallel});
            this.treePlan.Cursor = System.Windows.Forms.Cursors.Default;
            this.treePlan.DataSource = this.bindingExecutionPlan;
            this.treePlan.Dock = System.Windows.Forms.DockStyle.Fill;
            treeListFormatRule1.Column = this.colTotalSubtreeCost;
            treeListFormatRule1.Name = "FormatSubtreeCost";
            formatConditionRuleDataBar1.PredefinedName = "Blue Gradient";
            treeListFormatRule1.Rule = formatConditionRuleDataBar1;
            treeListFormatRule2.Column = this.colEstimateIO;
            treeListFormatRule2.Name = "FormatIO";
            formatConditionRuleDataBar2.PredefinedName = "Light Blue Gradient";
            treeListFormatRule2.Rule = formatConditionRuleDataBar2;
            treeListFormatRule3.Column = this.colEstimateCPU;
            treeListFormatRule3.Name = "FormatCPU";
            formatConditionRuleDataBar3.PredefinedName = "Light Blue Gradient";
            treeListFormatRule3.Rule = formatConditionRuleDataBar3;
            this.treePlan.FormatRules.Add(treeListFormatRule1);
            this.treePlan.FormatRules.Add(treeListFormatRule2);
            this.treePlan.FormatRules.Add(treeListFormatRule3);
            this.treePlan.KeyFieldName = "NodeId";
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
            this.treePlan.OptionsView.ShowPreview = true;
            this.treePlan.ParentFieldName = "Parent";
            this.treePlan.PreviewFieldName = "StmtText";
            this.treePlan.Size = new System.Drawing.Size(769, 462);
            this.treePlan.TabIndex = 0;
            this.treePlan.GetPreviewText += new DevExpress.XtraTreeList.GetPreviewTextEventHandler(this.TreePlan_GetPreviewText);
            this.treePlan.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreePlan_MouseUp);
            // 
            // colLogicalOp
            // 
            this.colLogicalOp.Caption = "Logical operation";
            this.colLogicalOp.FieldName = "LogicalOp";
            this.colLogicalOp.Fixed = DevExpress.XtraTreeList.Columns.FixedStyle.Left;
            this.colLogicalOp.Name = "colLogicalOp";
            this.colLogicalOp.Visible = true;
            this.colLogicalOp.VisibleIndex = 0;
            this.colLogicalOp.Width = 150;
            // 
            // colPhysicalOp
            // 
            this.colPhysicalOp.Caption = "Physical operation";
            this.colPhysicalOp.FieldName = "PhysicalOp";
            this.colPhysicalOp.Name = "colPhysicalOp";
            this.colPhysicalOp.Visible = true;
            this.colPhysicalOp.VisibleIndex = 2;
            this.colPhysicalOp.Width = 100;
            // 
            // colEstimateRows
            // 
            this.colEstimateRows.Caption = "Rows";
            this.colEstimateRows.FieldName = "EstimateRows";
            this.colEstimateRows.Name = "colEstimateRows";
            this.colEstimateRows.Visible = true;
            this.colEstimateRows.VisibleIndex = 5;
            this.colEstimateRows.Width = 60;
            // 
            // colEstimateExecutions
            // 
            this.colEstimateExecutions.Caption = "Executions";
            this.colEstimateExecutions.FieldName = "EstimateExecutions";
            this.colEstimateExecutions.Name = "colEstimateExecutions";
            this.colEstimateExecutions.Visible = true;
            this.colEstimateExecutions.VisibleIndex = 6;
            this.colEstimateExecutions.Width = 60;
            // 
            // colAvgRowSize
            // 
            this.colAvgRowSize.Caption = "Average row size";
            this.colAvgRowSize.FieldName = "AvgRowSize";
            this.colAvgRowSize.Name = "colAvgRowSize";
            this.colAvgRowSize.Visible = true;
            this.colAvgRowSize.VisibleIndex = 7;
            this.colAvgRowSize.Width = 60;
            // 
            // colArgument
            // 
            this.colArgument.Caption = "Argument";
            this.colArgument.FieldName = "Argument";
            this.colArgument.Name = "colArgument";
            this.colArgument.Visible = true;
            this.colArgument.VisibleIndex = 8;
            this.colArgument.Width = 100;
            // 
            // colDefinedValues
            // 
            this.colDefinedValues.Caption = "Defined values";
            this.colDefinedValues.FieldName = "DefinedValues";
            this.colDefinedValues.Name = "colDefinedValues";
            this.colDefinedValues.Visible = true;
            this.colDefinedValues.VisibleIndex = 9;
            this.colDefinedValues.Width = 100;
            // 
            // colOutputList
            // 
            this.colOutputList.Caption = "Output list";
            this.colOutputList.FieldName = "OutputList";
            this.colOutputList.Name = "colOutputList";
            this.colOutputList.Visible = true;
            this.colOutputList.VisibleIndex = 10;
            this.colOutputList.Width = 100;
            // 
            // colWarnings
            // 
            this.colWarnings.Caption = "Warnings";
            this.colWarnings.FieldName = "Warnings";
            this.colWarnings.Name = "colWarnings";
            this.colWarnings.Visible = true;
            this.colWarnings.VisibleIndex = 11;
            this.colWarnings.Width = 100;
            // 
            // colType
            // 
            this.colType.Caption = "Type";
            this.colType.FieldName = "Type";
            this.colType.Name = "colType";
            this.colType.Visible = true;
            this.colType.VisibleIndex = 12;
            this.colType.Width = 50;
            // 
            // colParallel
            // 
            this.colParallel.Caption = "Parallel";
            this.colParallel.FieldName = "Parallel";
            this.colParallel.Name = "colParallel";
            this.colParallel.Visible = true;
            this.colParallel.VisibleIndex = 13;
            this.colParallel.Width = 50;
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
            this.barManageFormatRules});
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
            // barManageFormatRules
            // 
            this.barManageFormatRules.Caption = "Manage format rules ...";
            this.barManageFormatRules.Id = 5;
            this.barManageFormatRules.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barManageFormatRules.ImageOptions.SvgImage")));
            this.barManageFormatRules.Name = "barManageFormatRules";
            this.barManageFormatRules.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarManageFormatRules_ItemClick);
            // 
            // popupGridPlan
            // 
            this.popupGridPlan.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuExpandAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuCollapseAll),
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuTreeView, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.pmnuShowWholeStatements, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barManageFormatRules, true)});
            this.popupGridPlan.Manager = this.barManager;
            this.popupGridPlan.Name = "popupGridPlan";
            this.popupGridPlan.BeforePopup += new System.ComponentModel.CancelEventHandler(this.PopupGridPlan_BeforePopup);
            // 
            // SqlExecutionPlanControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treePlan);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "SqlExecutionPlanControl";
            this.Size = new System.Drawing.Size(769, 462);
            this.Load += new System.EventHandler(this.SqlExecutionPlanControl_Load);
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
		private DevExpress.XtraTreeList.Columns.TreeListColumn colPhysicalOp;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colLogicalOp;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colArgument;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colDefinedValues;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colEstimateRows;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colEstimateIO;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colAvgRowSize;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colTotalSubtreeCost;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colOutputList;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colWarnings;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colType;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colParallel;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colEstimateExecutions;
		private DevExpress.XtraTreeList.Columns.TreeListColumn colEstimateCPU;
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
        private DevExpress.XtraBars.BarButtonItem barManageFormatRules;
    }
}