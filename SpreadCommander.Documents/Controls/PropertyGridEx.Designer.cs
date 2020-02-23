using SpreadCommander.Documents.Dialogs;

namespace SpreadCommander.Documents.Controls
{
	partial class PropertyGridEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyGridEx));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barTop = new DevExpress.XtraBars.Bar();
            this.checkCategories = new DevExpress.XtraBars.BarCheckItem();
            this.checkOrder = new DevExpress.XtraBars.BarCheckItem();
            this.btnDescription = new DevExpress.XtraBars.BarButtonItem();
            this.btnPrintPreview = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.PropertyGridControl = new SpreadCommander.Documents.Dialogs.PropertyGridControl();
            this.editorCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editorColor = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.editorPicture = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.editorDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editorCalc = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.SplitContainer = new DevExpress.XtraEditors.SplitContainerControl();
            this.propertyDescription = new DevExpress.XtraVerticalGrid.PropertyDescriptionControl();
            this.Image = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorCalc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barTop});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.checkCategories,
            this.checkOrder,
            this.btnDescription,
            this.btnPrintPreview});
            this.barManager.MaxItemId = 4;
            // 
            // barTop
            // 
            this.barTop.BarName = "Main menu";
            this.barTop.DockCol = 0;
            this.barTop.DockRow = 0;
            this.barTop.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barTop.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.checkCategories),
            new DevExpress.XtraBars.LinkPersistInfo(this.checkOrder),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDescription, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnPrintPreview, true)});
            this.barTop.OptionsBar.AllowQuickCustomization = false;
            this.barTop.OptionsBar.DrawDragBorder = false;
            this.barTop.OptionsBar.UseWholeRow = true;
            this.barTop.Text = "Main menu";
            // 
            // checkCategories
            // 
            this.checkCategories.BindableChecked = true;
            this.checkCategories.Caption = "Categories";
            this.checkCategories.Checked = true;
            this.checkCategories.GroupIndex = 1;
            this.checkCategories.Id = 0;
            this.checkCategories.ImageOptions.ImageIndex = 0;
            this.checkCategories.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("checkCategories.ImageOptions.SvgImage")));
            this.checkCategories.Name = "checkCategories";
            toolTipTitleItem1.Text = "Categories";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Group properties by categories.";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.checkCategories.SuperTip = superToolTip1;
            this.checkCategories.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.CheckCategories_CheckedChanged);
            // 
            // checkOrder
            // 
            this.checkOrder.Caption = "Order";
            this.checkOrder.GroupIndex = 1;
            this.checkOrder.Id = 1;
            this.checkOrder.ImageOptions.ImageIndex = 1;
            this.checkOrder.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("checkOrder.ImageOptions.SvgImage")));
            this.checkOrder.Name = "checkOrder";
            toolTipTitleItem2.Text = "Order";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Sort properties alphabetically.";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            this.checkOrder.SuperTip = superToolTip2;
            this.checkOrder.CheckedChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.CheckOrder_CheckedChanged);
            // 
            // btnDescription
            // 
            this.btnDescription.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
            this.btnDescription.Caption = "Description";
            this.btnDescription.Down = true;
            this.btnDescription.Id = 2;
            this.btnDescription.ImageOptions.ImageIndex = 2;
            this.btnDescription.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDescription.ImageOptions.SvgImage")));
            this.btnDescription.Name = "btnDescription";
            toolTipTitleItem3.Text = "Description";
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "Show / hide description of the properties.";
            superToolTip3.Items.Add(toolTipTitleItem3);
            superToolTip3.Items.Add(toolTipItem3);
            this.btnDescription.SuperTip = superToolTip3;
            this.btnDescription.DownChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnDescription_DownChanged);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Caption = "Print Preview";
            this.btnPrintPreview.Id = 3;
            this.btnPrintPreview.ImageOptions.ImageIndex = 3;
            this.btnPrintPreview.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPrintPreview.ImageOptions.SvgImage")));
            this.btnPrintPreview.Name = "btnPrintPreview";
            toolTipTitleItem4.Text = "Print preview";
            toolTipItem4.LeftIndent = 6;
            toolTipItem4.Text = "Print preview, print or export properties for selected object(s).";
            superToolTip4.Items.Add(toolTipTitleItem4);
            superToolTip4.Items.Add(toolTipItem4);
            this.btnPrintPreview.SuperTip = superToolTip4;
            this.btnPrintPreview.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnPrintPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnPrintPreview_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(358, 27);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 481);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(358, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 27);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 454);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(358, 27);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 454);
            // 
            // PropertyGridControl
            // 
            this.PropertyGridControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PropertyGridControl.DefaultEditors.AddRange(new DevExpress.XtraVerticalGrid.Rows.DefaultEditor[] {
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(bool), this.editorCheck),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(System.Drawing.Color), this.editorColor),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(System.Drawing.Image), this.editorPicture),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(System.DateTime), this.editorDate),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(int), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(long), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(byte), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(short), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(decimal), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(float), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(double), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(ushort), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(uint), this.editorCalc),
            new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(ulong), this.editorCalc)});
            this.PropertyGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGridControl.Location = new System.Drawing.Point(0, 0);
            this.PropertyGridControl.Name = "PropertyGridControl";
            this.PropertyGridControl.OptionsCollectionEditor.UseDXCollectionEditor = true;
            this.PropertyGridControl.OptionsView.MaxRowAutoHeight = 200;
            this.PropertyGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editorCheck,
            this.editorColor,
            this.editorPicture,
            this.editorDate,
            this.editorCalc});
            this.PropertyGridControl.ShowButtonMode = DevExpress.XtraVerticalGrid.ShowButtonModeEnum.ShowForFocusedRecord;
            this.PropertyGridControl.Size = new System.Drawing.Size(358, 383);
            this.PropertyGridControl.TabIndex = 0;
            this.PropertyGridControl.FocusedRowChanged += new DevExpress.XtraVerticalGrid.Events.FocusedRowChangedEventHandler(this.PropertyGridControl_FocusedRowChanged);
            this.PropertyGridControl.CellValueChanging += new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler(this.PropertyGridControl_CellValueChanging);
            this.PropertyGridControl.CellValueChanged += new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler(this.PropertyGridControl_CellValueChanged);
            // 
            // editorCheck
            // 
            this.editorCheck.AutoHeight = false;
            this.editorCheck.Name = "editorCheck";
            // 
            // editorColor
            // 
            this.editorColor.AutoHeight = false;
            this.editorColor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editorColor.Name = "editorColor";
            this.editorColor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            // 
            // editorPicture
            // 
            this.editorPicture.Name = "editorPicture";
            this.editorPicture.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            // 
            // editorDate
            // 
            this.editorDate.AutoHeight = false;
            this.editorDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editorDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editorDate.Name = "editorDate";
            // 
            // editorCalc
            // 
            this.editorCalc.AutoHeight = false;
            this.editorCalc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editorCalc.Name = "editorCalc";
            this.editorCalc.ParseEditValue += new DevExpress.XtraEditors.Controls.ConvertEditValueEventHandler(this.EditorCalc_ParseEditValue);
            // 
            // SplitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.SplitContainer.Horizontal = false;
            this.SplitContainer.Location = new System.Drawing.Point(0, 27);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Panel1.Controls.Add(this.PropertyGridControl);
            this.SplitContainer.Panel1.Text = "Panel1";
            this.SplitContainer.Panel2.Controls.Add(this.propertyDescription);
            this.SplitContainer.Panel2.Controls.Add(this.Image);
            this.SplitContainer.Panel2.Text = "Panel2";
            this.SplitContainer.Size = new System.Drawing.Size(358, 454);
            this.SplitContainer.SplitterPosition = 61;
            this.SplitContainer.TabIndex = 4;
            this.SplitContainer.Text = "splitContainerControl1";
            // 
            // propertyDescription
            // 
            this.propertyDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyDescription.Location = new System.Drawing.Point(56, 0);
            this.propertyDescription.Name = "propertyDescription";
            this.propertyDescription.PropertyGrid = this.PropertyGridControl;
            this.propertyDescription.Size = new System.Drawing.Size(302, 61);
            this.propertyDescription.TabIndex = 4;
            this.propertyDescription.TabStop = false;
            // 
            // Image
            // 
            this.Image.Dock = System.Windows.Forms.DockStyle.Left;
            this.Image.EditValue = ((object)(resources.GetObject("Image.EditValue")));
            this.Image.Location = new System.Drawing.Point(0, 0);
            this.Image.Name = "Image";
            this.Image.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Image.Properties.Appearance.Options.UseBackColor = true;
            this.Image.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Image.Size = new System.Drawing.Size(56, 61);
            this.Image.TabIndex = 5;
            // 
            // PropertyGridEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SplitContainer);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "PropertyGridEx";
            this.Size = new System.Drawing.Size(358, 481);
            this.Load += new System.EventHandler(this.PropertyGridEx_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropertyGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editorCalc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Image.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.Bar barTop;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraBars.BarCheckItem checkCategories;
		private DevExpress.XtraBars.BarCheckItem checkOrder;
		private DevExpress.XtraBars.BarButtonItem btnDescription;
		private DevExpress.XtraEditors.SplitContainerControl SplitContainer;
		private DevExpress.XtraBars.BarButtonItem btnPrintPreview;
		public PropertyGridControl PropertyGridControl;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editorCheck;
		private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit editorColor;
		private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit editorPicture;
		private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editorDate;
		private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit editorCalc;
		private DevExpress.XtraVerticalGrid.PropertyDescriptionControl propertyDescription;
		private DevExpress.XtraEditors.PictureEdit Image;

	}
}