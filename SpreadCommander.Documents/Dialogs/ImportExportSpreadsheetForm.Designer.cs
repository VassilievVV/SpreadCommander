namespace SpreadCommander.Documents.Dialogs
{
    partial class ImportExportSpreadsheetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportExportSpreadsheetForm));
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.Animation.Transition transition1 = new DevExpress.Utils.Animation.Transition();
            this.SpreadsheetViewer = new DevExpress.XtraSpreadsheet.SpreadsheetControl();
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridSheetNames = new DevExpress.XtraGrid.GridControl();
            this.bindingSheetNames = new System.Windows.Forms.BindingSource(this.components);
            this.viewSheetNames = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSelected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNewName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.comboSpreadsheetName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutOK = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutCancel = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupFile = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutSpreadsheetName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupSpreadsheet = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutSheetNames = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutSpreadsheetViewer = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            this.xtraOpenFileDialog = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.transitionManager = new DevExpress.Utils.Animation.TransitionManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSheetNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSheetNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSheetNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboSpreadsheetName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSpreadsheetName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSpreadsheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSheetNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSpreadsheetViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // SpreadsheetViewer
            // 
            this.SpreadsheetViewer.Location = new System.Drawing.Point(364, 114);
            this.SpreadsheetViewer.Name = "SpreadsheetViewer";
            this.SpreadsheetViewer.ReadOnly = true;
            this.SpreadsheetViewer.Size = new System.Drawing.Size(610, 404);
            this.SpreadsheetViewer.TabIndex = 7;
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridSheetNames);
            this.layoutControl.Controls.Add(this.btnCancel);
            this.layoutControl.Controls.Add(this.btnOK);
            this.layoutControl.Controls.Add(this.SpreadsheetViewer);
            this.layoutControl.Controls.Add(this.comboSpreadsheetName);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2232, 799, 650, 400);
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(998, 568);
            this.layoutControl.TabIndex = 0;
            // 
            // gridSheetNames
            // 
            this.gridSheetNames.DataSource = this.bindingSheetNames;
            this.gridSheetNames.Location = new System.Drawing.Point(24, 114);
            this.gridSheetNames.MainView = this.viewSheetNames;
            this.gridSheetNames.Name = "gridSheetNames";
            this.gridSheetNames.Size = new System.Drawing.Size(326, 404);
            this.gridSheetNames.TabIndex = 10;
            this.gridSheetNames.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewSheetNames});
            // 
            // bindingSheetNames
            // 
            this.bindingSheetNames.DataSource = typeof(SpreadCommander.Documents.Dialogs.ImportExportSpreadsheetForm.SheetName);
            // 
            // viewSheetNames
            // 
            this.viewSheetNames.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSelected,
            this.colName,
            this.colNewName,
            this.colIndex});
            this.viewSheetNames.GridControl = this.gridSheetNames;
            this.viewSheetNames.Name = "viewSheetNames";
            this.viewSheetNames.OptionsSelection.CheckBoxSelectorField = "Selected";
            this.viewSheetNames.OptionsView.ShowAutoFilterRow = true;
            this.viewSheetNames.OptionsView.ShowGroupPanel = false;
            // 
            // colSelected
            // 
            this.colSelected.Caption = " ";
            this.colSelected.FieldName = "Selected";
            this.colSelected.MaxWidth = 24;
            this.colSelected.Name = "colSelected";
            this.colSelected.Visible = true;
            this.colSelected.VisibleIndex = 0;
            this.colSelected.Width = 24;
            // 
            // colName
            // 
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.ReadOnly = true;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            this.colName.Width = 137;
            // 
            // colNewName
            // 
            this.colNewName.FieldName = "NewName";
            this.colNewName.Name = "colNewName";
            this.colNewName.Visible = true;
            this.colNewName.VisibleIndex = 2;
            this.colNewName.Width = 140;
            // 
            // colIndex
            // 
            this.colIndex.FieldName = "Index";
            this.colIndex.Name = "colIndex";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(501, 534);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(485, 22);
            this.btnCancel.StyleController = this.layoutControl;
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Close";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 534);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(485, 22);
            this.btnOK.StyleController = this.layoutControl;
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "Import/Export";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // comboSpreadsheetName
            // 
            this.comboSpreadsheetName.Location = new System.Drawing.Point(92, 45);
            this.comboSpreadsheetName.Name = "comboSpreadsheetName";
            this.comboSpreadsheetName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "Open spreadsheet", "OpenSpreadsheet", null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.comboSpreadsheetName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboSpreadsheetName.Properties.SelectedIndexChanged += new System.EventHandler(this.ComboSpreadsheetName_Properties_SelectedIndexChanged);
            this.comboSpreadsheetName.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ComboSpreadsheetName_Properties_ButtonClick);
            this.comboSpreadsheetName.Size = new System.Drawing.Size(882, 20);
            this.comboSpreadsheetName.StyleController = this.layoutControl;
            this.comboSpreadsheetName.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutOK,
            this.layoutCancel,
            this.layoutControlGroupFile,
            this.layoutControlGroupSpreadsheet});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(998, 568);
            this.Root.TextVisible = false;
            // 
            // layoutOK
            // 
            this.layoutOK.Control = this.btnOK;
            this.layoutOK.Location = new System.Drawing.Point(0, 522);
            this.layoutOK.Name = "layoutOK";
            this.layoutOK.Size = new System.Drawing.Size(489, 26);
            this.layoutOK.TextSize = new System.Drawing.Size(0, 0);
            this.layoutOK.TextVisible = false;
            // 
            // layoutCancel
            // 
            this.layoutCancel.Control = this.btnCancel;
            this.layoutCancel.Location = new System.Drawing.Point(489, 522);
            this.layoutCancel.Name = "layoutCancel";
            this.layoutCancel.Size = new System.Drawing.Size(489, 26);
            this.layoutCancel.TextSize = new System.Drawing.Size(0, 0);
            this.layoutCancel.TextVisible = false;
            // 
            // layoutControlGroupFile
            // 
            this.layoutControlGroupFile.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutSpreadsheetName});
            this.layoutControlGroupFile.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupFile.Name = "layoutControlGroupFile";
            this.layoutControlGroupFile.Size = new System.Drawing.Size(978, 69);
            this.layoutControlGroupFile.Text = "File";
            // 
            // layoutSpreadsheetName
            // 
            this.layoutSpreadsheetName.Control = this.comboSpreadsheetName;
            this.layoutSpreadsheetName.Location = new System.Drawing.Point(0, 0);
            this.layoutSpreadsheetName.Name = "layoutSpreadsheetName";
            this.layoutSpreadsheetName.Size = new System.Drawing.Size(954, 24);
            this.layoutSpreadsheetName.Text = "Spreadsheet:";
            this.layoutSpreadsheetName.TextSize = new System.Drawing.Size(65, 13);
            // 
            // layoutControlGroupSpreadsheet
            // 
            this.layoutControlGroupSpreadsheet.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutSheetNames,
            this.layoutSpreadsheetViewer,
            this.splitterItem1});
            this.layoutControlGroupSpreadsheet.Location = new System.Drawing.Point(0, 69);
            this.layoutControlGroupSpreadsheet.Name = "layoutControlGroupSpreadsheet";
            this.layoutControlGroupSpreadsheet.Size = new System.Drawing.Size(978, 453);
            this.layoutControlGroupSpreadsheet.Text = "Spreadsheet";
            // 
            // layoutSheetNames
            // 
            this.layoutSheetNames.Control = this.gridSheetNames;
            this.layoutSheetNames.Location = new System.Drawing.Point(0, 0);
            this.layoutSheetNames.Name = "layoutSheetNames";
            this.layoutSheetNames.Size = new System.Drawing.Size(330, 408);
            this.layoutSheetNames.TextSize = new System.Drawing.Size(0, 0);
            this.layoutSheetNames.TextVisible = false;
            // 
            // layoutSpreadsheetViewer
            // 
            this.layoutSpreadsheetViewer.Control = this.SpreadsheetViewer;
            this.layoutSpreadsheetViewer.Location = new System.Drawing.Point(340, 0);
            this.layoutSpreadsheetViewer.Name = "layoutSpreadsheetViewer";
            this.layoutSpreadsheetViewer.Size = new System.Drawing.Size(614, 408);
            this.layoutSpreadsheetViewer.TextSize = new System.Drawing.Size(0, 0);
            this.layoutSpreadsheetViewer.TextVisible = false;
            // 
            // splitterItem1
            // 
            this.splitterItem1.AllowHotTrack = true;
            this.splitterItem1.Location = new System.Drawing.Point(330, 0);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(10, 408);
            // 
            // xtraOpenFileDialog
            // 
            this.xtraOpenFileDialog.DefaultExt = "xlsx";
            this.xtraOpenFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx|Excel 97-2003 Workbook (*.xls)|*.xls";
            this.xtraOpenFileDialog.RestoreDirectory = true;
            this.xtraOpenFileDialog.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.xtraOpenFileDialog.Title = "Open spreadsheet";
            // 
            // transitionManager
            // 
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.Control = this.SpreadsheetViewer;
            transition1.LineWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.LineWaitingIndicatorProperties.Caption = "";
            transition1.LineWaitingIndicatorProperties.Description = "";
            transition1.RingWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.RingWaitingIndicatorProperties.Caption = "";
            transition1.RingWaitingIndicatorProperties.Description = "";
            transition1.WaitingIndicatorProperties.Caption = "";
            transition1.WaitingIndicatorProperties.Description = "";
            this.transitionManager.Transitions.Add(transition1);
            // 
            // ImportExportSpreadsheetForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(998, 568);
            this.Controls.Add(this.layoutControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportExportSpreadsheetForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import/Export sheets";
            this.Load += new System.EventHandler(this.ImportExportSpreadsheetForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSheetNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSheetNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSheetNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboSpreadsheetName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSpreadsheetName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSpreadsheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSheetNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSpreadsheetViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.ComboBoxEdit comboSpreadsheetName;
        private DevExpress.XtraLayout.LayoutControlItem layoutSpreadsheetName;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraSpreadsheet.SpreadsheetControl SpreadsheetViewer;
        private DevExpress.XtraLayout.LayoutControlItem layoutSpreadsheetViewer;
        private DevExpress.XtraLayout.LayoutControlItem layoutOK;
        private DevExpress.XtraLayout.LayoutControlItem layoutCancel;
        private DevExpress.XtraGrid.GridControl gridSheetNames;
        private DevExpress.XtraGrid.Views.Grid.GridView viewSheetNames;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupFile;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupSpreadsheet;
        private DevExpress.XtraLayout.LayoutControlItem layoutSheetNames;
        private DevExpress.XtraLayout.SplitterItem splitterItem1;
        private DevExpress.XtraEditors.XtraOpenFileDialog xtraOpenFileDialog;
        private System.Windows.Forms.BindingSource bindingSheetNames;
        private DevExpress.XtraGrid.Columns.GridColumn colSelected;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colNewName;
        private DevExpress.XtraGrid.Columns.GridColumn colIndex;
        private DevExpress.Utils.Animation.TransitionManager transitionManager;
    }
}