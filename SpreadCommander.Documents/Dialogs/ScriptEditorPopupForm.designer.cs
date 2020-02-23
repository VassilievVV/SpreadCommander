namespace SpreadCommander.Documents.Dialogs
{
	partial class ScriptEditorPopupForm
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
			FormDispose();
		
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptEditorPopupForm));
            this.comboIcon = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.svgImageCollection = new DevExpress.Utils.SvgImageCollection(this.components);
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIcon = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCaption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Grid = new DevExpress.XtraGrid.GridControl();
            this.splitterDescription = new DevExpress.XtraEditors.SplitterControl();
            this.lblDescription = new DevExpress.XtraEditors.LabelControl();
            this.lblComments = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.comboIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // comboIcon
            // 
            this.comboIcon.AutoComplete = false;
            this.comboIcon.AutoHeight = false;
            this.comboIcon.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboIcon.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, 0),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 1, 1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 2, 2),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 3, 3),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 4, 4),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 5, 5),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 6, 6)});
            this.comboIcon.Name = "comboIcon";
            this.comboIcon.SmallImages = this.svgImageCollection;
            // 
            // svgImageCollection
            // 
            this.svgImageCollection.Add("enum", "image://svgimages/snap/enum.svg");
            this.svgImageCollection.Add("variable", "image://svgimages/richedit/floatingobjectbringtofront.svg");
            this.svgImageCollection.Add("command", "image://svgimages/spreadsheet/showformulas.svg");
            this.svgImageCollection.Add("constructor", "image://svgimages/actions/new.svg");
            this.svgImageCollection.Add("property", "image://svgimages/snap/calcguid.svg");
            this.svgImageCollection.Add("function", "image://svgimages/snap/calcdefault.svg");
            this.svgImageCollection.Add("table", "image://svgimages/icon builder/actions_table.svg");
            this.svgImageCollection.Add("procedure", "image://svgimages/spreadsheet/calculatesheet.svg");
            this.svgImageCollection.Add("column", "image://svgimages/richedit/selecttablecolumn.svg");
            // 
            // GridView
            // 
            this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIcon,
            this.colCaption});
            this.GridView.GridControl = this.Grid;
            this.GridView.Images = this.svgImageCollection;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.Editable = false;
            this.GridView.OptionsView.ShowColumnHeaders = false;
            this.GridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.GridView.OptionsView.ShowGroupPanel = false;
            this.GridView.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.GridView.OptionsView.ShowIndicator = false;
            this.GridView.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            this.GridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GridView_FocusedRowChanged);
            this.GridView.DoubleClick += new System.EventHandler(this.GridView_DoubleClick);
            // 
            // colIcon
            // 
            this.colIcon.ColumnEdit = this.comboIcon;
            this.colIcon.FieldName = "ImageIndex";
            this.colIcon.Name = "colIcon";
            this.colIcon.OptionsColumn.AllowEdit = false;
            this.colIcon.OptionsColumn.AllowFocus = false;
            this.colIcon.OptionsColumn.AllowIncrementalSearch = false;
            this.colIcon.OptionsColumn.AllowMove = false;
            this.colIcon.OptionsColumn.AllowSize = false;
            this.colIcon.OptionsColumn.FixedWidth = true;
            this.colIcon.OptionsColumn.ReadOnly = true;
            this.colIcon.OptionsColumn.ShowCaption = false;
            this.colIcon.OptionsColumn.ShowInCustomizationForm = false;
            this.colIcon.OptionsFilter.AllowAutoFilter = false;
            this.colIcon.OptionsFilter.AllowFilter = false;
            this.colIcon.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.colIcon.Visible = true;
            this.colIcon.VisibleIndex = 0;
            this.colIcon.Width = 20;
            // 
            // colCaption
            // 
            this.colCaption.Caption = "Caption";
            this.colCaption.FieldName = "Caption";
            this.colCaption.Name = "colCaption";
            this.colCaption.OptionsColumn.AllowEdit = false;
            this.colCaption.OptionsColumn.AllowIncrementalSearch = false;
            this.colCaption.OptionsColumn.AllowMove = false;
            this.colCaption.OptionsColumn.AllowSize = false;
            this.colCaption.OptionsColumn.ShowCaption = false;
            this.colCaption.OptionsColumn.ShowInCustomizationForm = false;
            this.colCaption.Visible = true;
            this.colCaption.VisibleIndex = 1;
            // 
            // Grid
            // 
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 0);
            this.Grid.MainView = this.GridView;
            this.Grid.Name = "Grid";
            this.Grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.comboIcon});
            this.Grid.Size = new System.Drawing.Size(365, 258);
            this.Grid.TabIndex = 0;
            this.Grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView});
            // 
            // splitterDescription
            // 
            this.splitterDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterDescription.Location = new System.Drawing.Point(0, 258);
            this.splitterDescription.Name = "splitterDescription";
            this.splitterDescription.Size = new System.Drawing.Size(365, 10);
            this.splitterDescription.TabIndex = 1;
            this.splitterDescription.TabStop = false;
            // 
            // lblDescription
            // 
            this.lblDescription.AllowHtmlString = true;
            this.lblDescription.Appearance.Options.UseTextOptions = true;
            this.lblDescription.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblDescription.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblDescription.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lblDescription.AppearanceDisabled.Options.UseTextOptions = true;
            this.lblDescription.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblDescription.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblDescription.AppearanceDisabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lblDescription.AppearanceHovered.Options.UseTextOptions = true;
            this.lblDescription.AppearanceHovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblDescription.AppearanceHovered.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblDescription.AppearanceHovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lblDescription.AppearancePressed.Options.UseTextOptions = true;
            this.lblDescription.AppearancePressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblDescription.AppearancePressed.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblDescription.AppearancePressed.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lblDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDescription.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblDescription.ImageOptions.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDescription.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("lblDescription.ImageOptions.SvgImage")));
            this.lblDescription.IndentBetweenImageAndText = 10;
            this.lblDescription.Location = new System.Drawing.Point(0, 288);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblDescription.Size = new System.Drawing.Size(365, 68);
            this.lblDescription.TabIndex = 3;
            // 
            // lblComments
            // 
            this.lblComments.AllowHtmlString = true;
            this.lblComments.Appearance.BackColor = System.Drawing.SystemColors.Info;
            this.lblComments.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Question;
            this.lblComments.Appearance.Options.UseBackColor = true;
            this.lblComments.Appearance.Options.UseForeColor = true;
            this.lblComments.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lblComments.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblComments.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.lblComments.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("lblComments.ImageOptions.SvgImage")));
            this.lblComments.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.lblComments.IndentBetweenImageAndText = 10;
            this.lblComments.Location = new System.Drawing.Point(0, 268);
            this.lblComments.Name = "lblComments";
            this.lblComments.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblComments.Size = new System.Drawing.Size(365, 20);
            this.lblComments.TabIndex = 2;
            this.lblComments.Text = "Press <b>F1</b> for more help , <b>F2</b> for online help";
            // 
            // ScriptEditorPopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 356);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.splitterDescription);
            this.Controls.Add(this.lblComments);
            this.Controls.Add(this.lblDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "ScriptEditorPopupForm";
            this.Opacity = 0.8D;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PopupForm";
            this.Deactivate += new System.EventHandler(this.PopupForm_Deactivate);
            this.Load += new System.EventHandler(this.PopupForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PopupForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PopupForm_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PopupForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.comboIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox comboIcon;
		private DevExpress.XtraGrid.Views.Grid.GridView GridView;
		private DevExpress.XtraGrid.Columns.GridColumn colIcon;
		private DevExpress.XtraGrid.Columns.GridColumn colCaption;
		private DevExpress.XtraGrid.GridControl Grid;
		private DevExpress.Utils.SvgImageCollection svgImageCollection;
		private DevExpress.XtraEditors.SplitterControl splitterDescription;
		private DevExpress.XtraEditors.LabelControl lblDescription;
		private DevExpress.XtraEditors.LabelControl lblComments;
	}
}