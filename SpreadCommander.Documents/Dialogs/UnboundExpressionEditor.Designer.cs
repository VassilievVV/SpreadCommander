namespace SpreadCommander.Documents.Dialogs
{
    partial class UnboundExpressionEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnboundExpressionEditor));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.lookUpColumns = new DevExpress.XtraEditors.LookUpEdit();
            this.bindingColumns = new System.Windows.Forms.BindingSource(this.components);
            this.btnDeleteColumn = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddColumn = new DevExpress.XtraEditors.SimpleButton();
            this.pnlExpression = new SpreadCommander.Documents.Controls.UnboundExpressionPanel();
            this.radioType = new DevExpress.XtraEditors.RadioGroup();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutType = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutExpression = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlAddColumn = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlDeleteColumn = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlColumns = new DevExpress.XtraLayout.LayoutControlItem();
            this.images = new DevExpress.Utils.SvgImageCollection(this.components);
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumns.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlExpression)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutExpression)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlAddColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlDeleteColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.images)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btnClose);
            this.layoutControl.Controls.Add(this.lookUpColumns);
            this.layoutControl.Controls.Add(this.btnDeleteColumn);
            this.layoutControl.Controls.Add(this.btnAddColumn);
            this.layoutControl.Controls.Add(this.pnlExpression);
            this.layoutControl.Controls.Add(this.radioType);
            this.layoutControl.Controls.Add(this.txtName);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2253, 831, 650, 400);
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(670, 729);
            this.layoutControl.TabIndex = 0;
            // 
            // lookUpColumns
            // 
            this.lookUpColumns.Location = new System.Drawing.Point(59, 38);
            this.lookUpColumns.Name = "lookUpColumns";
            this.lookUpColumns.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpColumns.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name")});
            this.lookUpColumns.Properties.DataSource = this.bindingColumns;
            this.lookUpColumns.Properties.DisplayMember = "Name";
            this.lookUpColumns.Properties.NullText = "";
            this.lookUpColumns.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.lookUpColumns.Size = new System.Drawing.Size(599, 20);
            this.lookUpColumns.StyleController = this.layoutControl;
            this.lookUpColumns.TabIndex = 10;
            // 
            // bindingColumns
            // 
            this.bindingColumns.DataSource = typeof(SpreadCommander.Documents.Dialogs.UnboundExpressionEditor.ComboItem);
            this.bindingColumns.CurrentChanged += new System.EventHandler(this.BindingColumns_CurrentChanged);
            // 
            // btnDeleteColumn
            // 
            this.btnDeleteColumn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDeleteColumn.ImageOptions.SvgImage")));
            this.btnDeleteColumn.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnDeleteColumn.Location = new System.Drawing.Point(239, 12);
            this.btnDeleteColumn.Name = "btnDeleteColumn";
            this.btnDeleteColumn.Size = new System.Drawing.Size(207, 22);
            this.btnDeleteColumn.StyleController = this.layoutControl;
            this.btnDeleteColumn.TabIndex = 9;
            this.btnDeleteColumn.Text = "DeleteColumn";
            this.btnDeleteColumn.Click += new System.EventHandler(this.BtnDeleteColumn_Click);
            // 
            // btnAddColumn
            // 
            this.btnAddColumn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAddColumn.ImageOptions.SvgImage")));
            this.btnAddColumn.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnAddColumn.Location = new System.Drawing.Point(12, 12);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(223, 22);
            this.btnAddColumn.StyleController = this.layoutControl;
            this.btnAddColumn.TabIndex = 8;
            this.btnAddColumn.Text = "Add column";
            this.btnAddColumn.Click += new System.EventHandler(this.BtnAddColumn_Click);
            // 
            // pnlExpression
            // 
            this.pnlExpression.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlExpression.Location = new System.Drawing.Point(12, 124);
            this.pnlExpression.Name = "pnlExpression";
            this.pnlExpression.Size = new System.Drawing.Size(646, 593);
            this.pnlExpression.TabIndex = 7;
            this.pnlExpression.UpdateColumn += new System.EventHandler(this.PnlExpression_UpdateColumn);
            this.pnlExpression.CancelUpdate += new System.EventHandler(this.PnlExpression_CancelUpdate);
            // 
            // radioType
            // 
            this.radioType.Location = new System.Drawing.Point(59, 86);
            this.radioType.Name = "radioType";
            this.radioType.Properties.Columns = 5;
            this.radioType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "String"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Integer"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Decimal"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "DateTime"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Boolean")});
            this.radioType.Size = new System.Drawing.Size(599, 34);
            this.radioType.StyleController = this.layoutControl;
            this.radioType.TabIndex = 6;
            // 
            // txtName
            // 
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingColumns, "Name", true));
            this.txtName.Location = new System.Drawing.Point(59, 62);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(599, 20);
            this.txtName.StyleController = this.layoutControl;
            this.txtName.TabIndex = 5;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutName,
            this.layoutType,
            this.layoutExpression,
            this.layoutControlColumns,
            this.layoutControlAddColumn,
            this.layoutControlDeleteColumn,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(670, 729);
            this.Root.TextVisible = false;
            // 
            // layoutName
            // 
            this.layoutName.Control = this.txtName;
            this.layoutName.Location = new System.Drawing.Point(0, 50);
            this.layoutName.Name = "layoutName";
            this.layoutName.Size = new System.Drawing.Size(650, 24);
            this.layoutName.Text = "Name:";
            this.layoutName.TextSize = new System.Drawing.Size(44, 13);
            // 
            // layoutType
            // 
            this.layoutType.Control = this.radioType;
            this.layoutType.Location = new System.Drawing.Point(0, 74);
            this.layoutType.MaxSize = new System.Drawing.Size(0, 38);
            this.layoutType.MinSize = new System.Drawing.Size(101, 38);
            this.layoutType.Name = "layoutType";
            this.layoutType.Size = new System.Drawing.Size(650, 38);
            this.layoutType.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutType.Text = "Type:";
            this.layoutType.TextSize = new System.Drawing.Size(44, 13);
            // 
            // layoutExpression
            // 
            this.layoutExpression.Control = this.pnlExpression;
            this.layoutExpression.Location = new System.Drawing.Point(0, 112);
            this.layoutExpression.Name = "layoutExpression";
            this.layoutExpression.Size = new System.Drawing.Size(650, 597);
            this.layoutExpression.TextSize = new System.Drawing.Size(0, 0);
            this.layoutExpression.TextVisible = false;
            // 
            // layoutControlAddColumn
            // 
            this.layoutControlAddColumn.Control = this.btnAddColumn;
            this.layoutControlAddColumn.Location = new System.Drawing.Point(0, 0);
            this.layoutControlAddColumn.Name = "layoutControlAddColumn";
            this.layoutControlAddColumn.Size = new System.Drawing.Size(227, 26);
            this.layoutControlAddColumn.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlAddColumn.TextVisible = false;
            // 
            // layoutControlDeleteColumn
            // 
            this.layoutControlDeleteColumn.Control = this.btnDeleteColumn;
            this.layoutControlDeleteColumn.Location = new System.Drawing.Point(227, 0);
            this.layoutControlDeleteColumn.Name = "layoutControlDeleteColumn";
            this.layoutControlDeleteColumn.Size = new System.Drawing.Size(211, 26);
            this.layoutControlDeleteColumn.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlDeleteColumn.TextVisible = false;
            // 
            // layoutControlColumns
            // 
            this.layoutControlColumns.Control = this.lookUpColumns;
            this.layoutControlColumns.Location = new System.Drawing.Point(0, 26);
            this.layoutControlColumns.Name = "layoutControlColumns";
            this.layoutControlColumns.Size = new System.Drawing.Size(650, 24);
            this.layoutControlColumns.Text = "Columns:";
            this.layoutControlColumns.TextSize = new System.Drawing.Size(44, 13);
            // 
            // images
            // 
            this.images.Add("column", "image://svgimages/richedit/selecttablecolumn.svg");
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClose.ImageOptions.SvgImage")));
            this.btnClose.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnClose.Location = new System.Drawing.Point(450, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(208, 22);
            this.btnClose.StyleController = this.layoutControl;
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.ToolTip = "Close editor";
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnClose;
            this.layoutControlItem1.Location = new System.Drawing.Point(438, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(212, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // UnboundExpressionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(670, 729);
            this.Controls.Add(this.layoutControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UnboundExpressionEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Expression Editor";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpColumns.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlExpression)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutExpression)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlAddColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlDeleteColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.images)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private Controls.UnboundExpressionPanel pnlExpression;
        private DevExpress.XtraEditors.RadioGroup radioType;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraLayout.LayoutControlItem layoutName;
        private DevExpress.XtraLayout.LayoutControlItem layoutType;
        private DevExpress.XtraLayout.LayoutControlItem layoutExpression;
        private DevExpress.Utils.SvgImageCollection images;
        private System.Windows.Forms.BindingSource bindingColumns;
        private DevExpress.XtraEditors.SimpleButton btnDeleteColumn;
        private DevExpress.XtraEditors.SimpleButton btnAddColumn;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlAddColumn;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlDeleteColumn;
        private DevExpress.XtraEditors.LookUpEdit lookUpColumns;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlColumns;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}