namespace SpreadCommander.Documents.Controls
{
	partial class SpreadsheetTableSelectorControl
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
			this.gridTables = new DevExpress.XtraGrid.GridControl();
			this.viewTables = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.bindingTables = new System.Windows.Forms.BindingSource(this.components);
			this.colSheetName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colTableName = new DevExpress.XtraGrid.Columns.GridColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.viewTables)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingTables)).BeginInit();
			this.SuspendLayout();
			// 
			// gridTables
			// 
			this.gridTables.DataSource = this.bindingTables;
			this.gridTables.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTables.Location = new System.Drawing.Point(0, 0);
			this.gridTables.MainView = this.viewTables;
			this.gridTables.Name = "gridTables";
			this.gridTables.Size = new System.Drawing.Size(431, 386);
			this.gridTables.TabIndex = 0;
			this.gridTables.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewTables});
			// 
			// viewTables
			// 
			this.viewTables.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSheetName,
            this.colTableName});
			this.viewTables.GridControl = this.gridTables;
			this.viewTables.Name = "viewTables";
			this.viewTables.OptionsBehavior.Editable = false;
			this.viewTables.OptionsBehavior.ReadOnly = true;
			this.viewTables.OptionsFind.AlwaysVisible = true;
			this.viewTables.OptionsFind.Behavior = DevExpress.XtraEditors.FindPanelBehavior.Search;
			this.viewTables.OptionsView.ShowGroupPanel = false;
			// 
			// bindingTables
			// 
			this.bindingTables.DataSource = typeof(SpreadCommander.Documents.Controls.SpreadsheetTableSelectorControl.TableData);
			// 
			// colSheetName
			// 
			this.colSheetName.Caption = "Sheet";
			this.colSheetName.FieldName = "SheetName";
			this.colSheetName.Name = "colSheetName";
			this.colSheetName.Visible = true;
			this.colSheetName.VisibleIndex = 0;
			// 
			// colTableName
			// 
			this.colTableName.Caption = "Table";
			this.colTableName.FieldName = "TableName";
			this.colTableName.Name = "colTableName";
			this.colTableName.Visible = true;
			this.colTableName.VisibleIndex = 1;
			// 
			// SpreadsheetTableSelectorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridTables);
			this.Name = "SpreadsheetTableSelectorControl";
			this.Size = new System.Drawing.Size(431, 386);
			this.Load += new System.EventHandler(this.SpreadsheetTableSelectorControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.viewTables)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingTables)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl gridTables;
		private DevExpress.XtraGrid.Views.Grid.GridView viewTables;
		private System.Windows.Forms.BindingSource bindingTables;
		private DevExpress.XtraGrid.Columns.GridColumn colSheetName;
		private DevExpress.XtraGrid.Columns.GridColumn colTableName;
	}
}
