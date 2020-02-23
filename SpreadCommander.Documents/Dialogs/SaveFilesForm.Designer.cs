namespace SpreadCommander.Documents.Dialogs
{
	partial class SaveFilesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveFilesForm));
			this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			this.btnOK = new DevExpress.XtraEditors.SimpleButton();
			this.gridFiles = new DevExpress.XtraGrid.GridControl();
			this.bindingFiles = new System.Windows.Forms.BindingSource();
			this.viewFiles = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colSelected = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colTitle = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colFileName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repositoryItemFileName = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.dlgSave = new DevExpress.XtraEditors.XtraSaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
			this.layoutControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingFiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.viewFiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemFileName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
			this.SuspendLayout();
			// 
			// layoutControl
			// 
			this.layoutControl.Controls.Add(this.btnCancel);
			this.layoutControl.Controls.Add(this.btnOK);
			this.layoutControl.Controls.Add(this.gridFiles);
			this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl.Location = new System.Drawing.Point(0, 0);
			this.layoutControl.Name = "layoutControl";
			this.layoutControl.Root = this.Root;
			this.layoutControl.Size = new System.Drawing.Size(600, 372);
			this.layoutControl.TabIndex = 0;
			this.layoutControl.Text = "layoutControl1";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(302, 338);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(286, 22);
			this.btnCancel.StyleController = this.layoutControl;
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(12, 338);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(286, 22);
			this.btnOK.StyleController = this.layoutControl;
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			// 
			// gridFiles
			// 
			this.gridFiles.DataSource = this.bindingFiles;
			this.gridFiles.EmbeddedNavigator.Buttons.Append.Visible = false;
			this.gridFiles.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gridFiles.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gridFiles.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gridFiles.EmbeddedNavigator.Buttons.Remove.Visible = false;
			this.gridFiles.Location = new System.Drawing.Point(12, 12);
			this.gridFiles.MainView = this.viewFiles;
			this.gridFiles.Name = "gridFiles";
			this.gridFiles.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemFileName});
			this.gridFiles.Size = new System.Drawing.Size(576, 322);
			this.gridFiles.TabIndex = 4;
			this.gridFiles.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewFiles});
			// 
			// bindingFiles
			// 
			this.bindingFiles.DataSource = typeof(SpreadCommander.Documents.Services.SaveFileData);
			// 
			// viewFiles
			// 
			this.viewFiles.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSelected,
            this.colTitle,
            this.colFileName});
			this.viewFiles.GridControl = this.gridFiles;
			this.viewFiles.Name = "viewFiles";
			this.viewFiles.OptionsSelection.CheckBoxSelectorColumnWidth = 24;
			this.viewFiles.OptionsSelection.CheckBoxSelectorField = "Selected";
			this.viewFiles.OptionsSelection.MultiSelect = true;
			this.viewFiles.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
			this.viewFiles.OptionsView.EnableAppearanceOddRow = true;
			this.viewFiles.OptionsView.ShowGroupPanel = false;
			this.viewFiles.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.ViewFiles_InvalidRowException);
			this.viewFiles.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.ViewFiles_ValidateRow);
			// 
			// colSelected
			// 
			this.colSelected.FieldName = "Selected";
			this.colSelected.Name = "colSelected";
			// 
			// colTitle
			// 
			this.colTitle.FieldName = "Title";
			this.colTitle.Name = "colTitle";
			this.colTitle.OptionsColumn.ReadOnly = true;
			this.colTitle.Visible = true;
			this.colTitle.VisibleIndex = 1;
			// 
			// colFileName
			// 
			this.colFileName.ColumnEdit = this.repositoryItemFileName;
			this.colFileName.FieldName = "FileName";
			this.colFileName.Name = "colFileName";
			this.colFileName.OptionsColumn.ReadOnly = true;
			this.colFileName.Visible = true;
			this.colFileName.VisibleIndex = 2;
			// 
			// repositoryItemFileName
			// 
			this.repositoryItemFileName.AutoHeight = false;
			this.repositoryItemFileName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.repositoryItemFileName.Name = "repositoryItemFileName";
			this.repositoryItemFileName.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.RepositoryItemFileName_ButtonClick);
			// 
			// Root
			// 
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
			this.Root.Name = "Root";
			this.Root.Size = new System.Drawing.Size(600, 372);
			this.Root.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gridFiles;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(580, 326);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.btnOK;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 326);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(290, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			// 
			// layoutControlItem3
			// 
			this.layoutControlItem3.Control = this.btnCancel;
			this.layoutControlItem3.Location = new System.Drawing.Point(290, 326);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(290, 26);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			// 
			// dlgSave
			// 
			this.dlgSave.FileName = "Save";
			this.dlgSave.RestoreDirectory = true;
			// 
			// SaveFilesForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(600, 372);
			this.Controls.Add(this.layoutControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SaveFilesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Save files";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveFilesForm_FormClosing);
			this.Load += new System.EventHandler(this.SaveFilesForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
			this.layoutControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingFiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.viewFiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemFileName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraEditors.SimpleButton btnOK;
		private DevExpress.XtraGrid.GridControl gridFiles;
		private DevExpress.XtraGrid.Views.Grid.GridView viewFiles;
		private DevExpress.XtraLayout.LayoutControlGroup Root;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
		private System.Windows.Forms.BindingSource bindingFiles;
		private DevExpress.XtraGrid.Columns.GridColumn colSelected;
		private DevExpress.XtraGrid.Columns.GridColumn colTitle;
		private DevExpress.XtraGrid.Columns.GridColumn colFileName;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemFileName;
		private DevExpress.XtraEditors.XtraSaveFileDialog dlgSave;
	}
}