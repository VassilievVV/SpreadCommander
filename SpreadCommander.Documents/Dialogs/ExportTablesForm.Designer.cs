using SpreadCommander.Documents.Controls;

namespace SpreadCommander.Documents.Dialogs
{
	partial class ExportTablesForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportTablesForm));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.listConnections = new DevExpress.XtraEditors.ImageListBoxControl();
            this.bindingConnections = new System.Windows.Forms.BindingSource(this.components);
            this.imageConnection = new DevExpress.Utils.SvgImageCollection(this.components);
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.textTableNamePrefix = new DevExpress.XtraEditors.TextEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.gridTableNames = new DevExpress.XtraGrid.GridControl();
            this.bindingTables = new System.Windows.Forms.BindingSource(this.components);
            this.viewTableNames = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTableName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNewTableName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnTestConnection = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlTestConnection = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlTableNames = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlExport = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlProgress = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlTableNamePrefix = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlLog = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutConections = new DevExpress.XtraLayout.LayoutControlItem();
            this.colNewTableSchema = new DevExpress.XtraGrid.Columns.GridColumn();
            this.textDefaultSchema = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlDefaultSchema = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listConnections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingConnections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTableNamePrefix.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTableNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTableNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTestConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTableNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProgress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTableNamePrefix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutConections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDefaultSchema.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlDefaultSchema)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.textDefaultSchema);
            this.layoutControl.Controls.Add(this.listConnections);
            this.layoutControl.Controls.Add(this.memoLog);
            this.layoutControl.Controls.Add(this.textTableNamePrefix);
            this.layoutControl.Controls.Add(this.btnCancel);
            this.layoutControl.Controls.Add(this.progressBar);
            this.layoutControl.Controls.Add(this.btnExport);
            this.layoutControl.Controls.Add(this.gridTableNames);
            this.layoutControl.Controls.Add(this.btnTestConnection);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2221, 614, 650, 400);
            this.layoutControl.Root = this.layoutControlGroup1;
            this.layoutControl.Size = new System.Drawing.Size(984, 561);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl1";
            // 
            // listConnections
            // 
            this.listConnections.DataSource = this.bindingConnections;
            this.listConnections.DisplayMember = "Name";
            this.listConnections.ImageIndexMember = "ImageIndex";
            this.listConnections.ImageList = this.imageConnection;
            this.listConnections.Location = new System.Drawing.Point(12, 12);
            this.listConnections.Name = "listConnections";
            this.listConnections.Size = new System.Drawing.Size(306, 464);
            this.listConnections.StyleController = this.layoutControl;
            this.listConnections.TabIndex = 13;
            // 
            // bindingConnections
            // 
            this.bindingConnections.DataSource = typeof(SpreadCommander.Common.Code.DBConnection);
            // 
            // imageConnection
            // 
            this.imageConnection.Add("actions_database", "image://svgimages/icon builder/actions_database.svg");
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(332, 393);
            this.memoLog.Name = "memoLog";
            this.memoLog.Properties.Appearance.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.memoLog.Properties.Appearance.Options.UseFont = true;
            this.memoLog.Size = new System.Drawing.Size(640, 130);
            this.memoLog.StyleController = this.layoutControl;
            this.memoLog.TabIndex = 12;
            // 
            // textTableNamePrefix
            // 
            this.textTableNamePrefix.Location = new System.Drawing.Point(332, 28);
            this.textTableNamePrefix.Name = "textTableNamePrefix";
            this.textTableNamePrefix.Size = new System.Drawing.Size(318, 20);
            this.textTableNamePrefix.StyleController = this.layoutControl;
            this.textTableNamePrefix.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(595, 527);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(204, 22);
            this.btnCancel.StyleController = this.layoutControl;
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(215, 527);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(376, 18);
            this.progressBar.StyleController = this.layoutControl;
            this.progressBar.TabIndex = 9;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(12, 527);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(199, 22);
            this.btnExport.StyleController = this.layoutControl;
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // gridTableNames
            // 
            this.gridTableNames.DataSource = this.bindingTables;
            this.gridTableNames.Location = new System.Drawing.Point(332, 52);
            this.gridTableNames.MainView = this.viewTableNames;
            this.gridTableNames.Name = "gridTableNames";
            this.gridTableNames.Size = new System.Drawing.Size(640, 337);
            this.gridTableNames.TabIndex = 7;
            this.gridTableNames.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewTableNames});
            // 
            // bindingTables
            // 
            this.bindingTables.DataSource = typeof(SpreadCommander.Documents.Dialogs.ExportTablesForm.ExportTable);
            // 
            // viewTableNames
            // 
            this.viewTableNames.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTableName,
            this.colNewTableSchema,
            this.colNewTableName});
            this.viewTableNames.GridControl = this.gridTableNames;
            this.viewTableNames.Name = "viewTableNames";
            this.viewTableNames.OptionsSelection.CheckBoxSelectorColumnWidth = 32;
            this.viewTableNames.OptionsSelection.CheckBoxSelectorField = "Selected";
            this.viewTableNames.OptionsSelection.MultiSelect = true;
            this.viewTableNames.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.viewTableNames.OptionsView.ShowGroupPanel = false;
            // 
            // colTableName
            // 
            this.colTableName.FieldName = "TableName";
            this.colTableName.Name = "colTableName";
            this.colTableName.OptionsColumn.ReadOnly = true;
            this.colTableName.Visible = true;
            this.colTableName.VisibleIndex = 1;
            this.colTableName.Width = 194;
            // 
            // colNewTableName
            // 
            this.colNewTableName.FieldName = "NewTableName";
            this.colNewTableName.Name = "colNewTableName";
            this.colNewTableName.Visible = true;
            this.colNewTableName.VisibleIndex = 3;
            this.colNewTableName.Width = 289;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(12, 480);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(306, 22);
            this.btnTestConnection.StyleController = this.layoutControl;
            this.btnTestConnection.TabIndex = 6;
            this.btnTestConnection.Text = "Test connection";
            this.btnTestConnection.Click += new System.EventHandler(this.BtnTestConnection_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlTestConnection,
            this.layoutControlTableNames,
            this.emptySpaceItem2,
            this.layoutControlExport,
            this.layoutControlProgress,
            this.layoutControlCancel,
            this.splitterItem1,
            this.emptySpaceItem3,
            this.layoutControlTableNamePrefix,
            this.layoutControlLog,
            this.layoutConections,
            this.layoutControlDefaultSchema});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(984, 561);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlTestConnection
            // 
            this.layoutControlTestConnection.Control = this.btnTestConnection;
            this.layoutControlTestConnection.Location = new System.Drawing.Point(0, 468);
            this.layoutControlTestConnection.Name = "layoutControlTestConnection";
            this.layoutControlTestConnection.Size = new System.Drawing.Size(310, 26);
            this.layoutControlTestConnection.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlTestConnection.TextVisible = false;
            // 
            // layoutControlTableNames
            // 
            this.layoutControlTableNames.Control = this.gridTableNames;
            this.layoutControlTableNames.Location = new System.Drawing.Point(320, 40);
            this.layoutControlTableNames.Name = "layoutControlTableNames";
            this.layoutControlTableNames.Size = new System.Drawing.Size(644, 341);
            this.layoutControlTableNames.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlTableNames.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(791, 515);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(173, 26);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlExport
            // 
            this.layoutControlExport.Control = this.btnExport;
            this.layoutControlExport.Location = new System.Drawing.Point(0, 515);
            this.layoutControlExport.Name = "layoutControlExport";
            this.layoutControlExport.Size = new System.Drawing.Size(203, 26);
            this.layoutControlExport.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlExport.TextVisible = false;
            // 
            // layoutControlProgress
            // 
            this.layoutControlProgress.Control = this.progressBar;
            this.layoutControlProgress.Location = new System.Drawing.Point(203, 515);
            this.layoutControlProgress.Name = "layoutControlProgress";
            this.layoutControlProgress.Size = new System.Drawing.Size(380, 26);
            this.layoutControlProgress.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlProgress.TextVisible = false;
            // 
            // layoutControlCancel
            // 
            this.layoutControlCancel.Control = this.btnCancel;
            this.layoutControlCancel.Location = new System.Drawing.Point(583, 515);
            this.layoutControlCancel.Name = "layoutControlCancel";
            this.layoutControlCancel.Size = new System.Drawing.Size(208, 26);
            this.layoutControlCancel.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlCancel.TextVisible = false;
            // 
            // splitterItem1
            // 
            this.splitterItem1.AllowHotTrack = true;
            this.splitterItem1.Location = new System.Drawing.Point(310, 0);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(10, 515);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 494);
            this.emptySpaceItem3.MaxSize = new System.Drawing.Size(0, 21);
            this.emptySpaceItem3.MinSize = new System.Drawing.Size(10, 21);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(310, 21);
            this.emptySpaceItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlTableNamePrefix
            // 
            this.layoutControlTableNamePrefix.Control = this.textTableNamePrefix;
            this.layoutControlTableNamePrefix.Location = new System.Drawing.Point(320, 0);
            this.layoutControlTableNamePrefix.Name = "layoutControlTableNamePrefix";
            this.layoutControlTableNamePrefix.Size = new System.Drawing.Size(322, 40);
            this.layoutControlTableNamePrefix.Text = "Table name prefix:";
            this.layoutControlTableNamePrefix.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlTableNamePrefix.TextSize = new System.Drawing.Size(90, 13);
            // 
            // layoutControlLog
            // 
            this.layoutControlLog.Control = this.memoLog;
            this.layoutControlLog.Location = new System.Drawing.Point(320, 381);
            this.layoutControlLog.Name = "layoutControlLog";
            this.layoutControlLog.Size = new System.Drawing.Size(644, 134);
            this.layoutControlLog.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlLog.TextVisible = false;
            // 
            // layoutConections
            // 
            this.layoutConections.Control = this.listConnections;
            this.layoutConections.Location = new System.Drawing.Point(0, 0);
            this.layoutConections.Name = "layoutConections";
            this.layoutConections.Size = new System.Drawing.Size(310, 468);
            this.layoutConections.TextSize = new System.Drawing.Size(0, 0);
            this.layoutConections.TextVisible = false;
            // 
            // colNewTableSchema
            // 
            this.colNewTableSchema.FieldName = "NewTableSchema";
            this.colNewTableSchema.Name = "colNewTableSchema";
            this.colNewTableSchema.Visible = true;
            this.colNewTableSchema.VisibleIndex = 2;
            this.colNewTableSchema.Width = 100;
            // 
            // textDefaultSchema
            // 
            this.textDefaultSchema.Location = new System.Drawing.Point(654, 28);
            this.textDefaultSchema.Name = "textDefaultSchema";
            this.textDefaultSchema.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.textDefaultSchema.Size = new System.Drawing.Size(318, 20);
            this.textDefaultSchema.StyleController = this.layoutControl;
            this.textDefaultSchema.TabIndex = 14;
            this.textDefaultSchema.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.TextDefaultSchema_ButtonClick);
            // 
            // layoutControlDefaultSchema
            // 
            this.layoutControlDefaultSchema.Control = this.textDefaultSchema;
            this.layoutControlDefaultSchema.Location = new System.Drawing.Point(642, 0);
            this.layoutControlDefaultSchema.Name = "layoutControlDefaultSchema";
            this.layoutControlDefaultSchema.Size = new System.Drawing.Size(322, 40);
            this.layoutControlDefaultSchema.Text = "Default schema:";
            this.layoutControlDefaultSchema.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlDefaultSchema.TextSize = new System.Drawing.Size(90, 13);
            // 
            // ExportTablesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.layoutControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExportTablesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listConnections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingConnections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTableNamePrefix.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridTableNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewTableNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTestConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTableNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProgress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTableNamePrefix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutConections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textDefaultSchema.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlDefaultSchema)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraEditors.ProgressBarControl progressBar;
		private DevExpress.XtraEditors.SimpleButton btnExport;
		private DevExpress.XtraGrid.GridControl gridTableNames;
		private DevExpress.XtraGrid.Views.Grid.GridView viewTableNames;
		private DevExpress.XtraEditors.SimpleButton btnTestConnection;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlTestConnection;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlTableNames;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlExport;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlProgress;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlCancel;
		private DevExpress.XtraLayout.SplitterItem splitterItem1;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
		private DevExpress.XtraEditors.MemoEdit memoLog;
		private DevExpress.XtraEditors.TextEdit textTableNamePrefix;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlTableNamePrefix;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlLog;
		private System.Windows.Forms.BindingSource bindingTables;
		private DevExpress.XtraGrid.Columns.GridColumn colTableName;
		private DevExpress.XtraGrid.Columns.GridColumn colNewTableName;
        private DevExpress.XtraEditors.ImageListBoxControl listConnections;
        private System.Windows.Forms.BindingSource bindingConnections;
        private DevExpress.XtraLayout.LayoutControlItem layoutConections;
        private DevExpress.Utils.SvgImageCollection imageConnection;
        private DevExpress.XtraEditors.ButtonEdit textDefaultSchema;
        private DevExpress.XtraGrid.Columns.GridColumn colNewTableSchema;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlDefaultSchema;
    }
}