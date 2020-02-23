namespace SpreadCommander.Documents.Dialogs
{
	partial class ObjectListEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectListEditor));
			this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
			this.PropertyGrid = new SpreadCommander.Documents.Controls.PropertyGridEx();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			this.btnOK = new DevExpress.XtraEditors.SimpleButton();
			this.listControls = new DevExpress.XtraEditors.ImageListBoxControl();
			this.Images = new DevExpress.Utils.ImageCollection(this.components);
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutList = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutOK = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutCancel = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutProperties = new DevExpress.XtraLayout.LayoutControlItem();
			this.splitterProperties = new DevExpress.XtraLayout.SplitterItem();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
			this.layoutControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.listControls)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Images)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutOK)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutCancel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitterProperties)).BeginInit();
			this.SuspendLayout();
			// 
			// layoutControl
			// 
			this.layoutControl.Controls.Add(this.PropertyGrid);
			this.layoutControl.Controls.Add(this.btnCancel);
			this.layoutControl.Controls.Add(this.btnOK);
			this.layoutControl.Controls.Add(this.listControls);
			this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl.Location = new System.Drawing.Point(0, 0);
			this.layoutControl.Name = "layoutControl";
			this.layoutControl.Root = this.layoutControlGroup1;
			this.layoutControl.Size = new System.Drawing.Size(748, 464);
			this.layoutControl.TabIndex = 0;
			// 
			// PropertyGrid
			// 
			this.PropertyGrid.Location = new System.Drawing.Point(348, 12);
			this.PropertyGrid.Name = "PropertyGrid";
			this.PropertyGrid.SelectedObject = null;
			this.PropertyGrid.SelectedObjects = new object[0];
			this.PropertyGrid.Size = new System.Drawing.Size(388, 440);
			this.PropertyGrid.TabIndex = 7;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(151, 428);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(135, 24);
			this.btnCancel.StyleController = this.layoutControl;
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(12, 428);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(135, 24);
			this.btnOK.StyleController = this.layoutControl;
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "&OK";
			// 
			// listControls
			// 
			this.listControls.ImageList = this.Images;
			this.listControls.Location = new System.Drawing.Point(12, 12);
			this.listControls.Name = "listControls";
			this.listControls.Size = new System.Drawing.Size(322, 412);
			this.listControls.StyleController = this.layoutControl;
			this.listControls.TabIndex = 4;
			this.listControls.SelectedIndexChanged += new System.EventHandler(this.ListControls_SelectedIndexChanged);
			// 
			// Images
			// 
			this.Images.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("Images.ImageStream")));
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutList,
            this.emptySpaceItem1,
            this.layoutOK,
            this.layoutCancel,
            this.layoutProperties,
            this.splitterProperties});
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Size = new System.Drawing.Size(748, 464);
			this.layoutControlGroup1.TextVisible = false;
			// 
			// layoutList
			// 
			this.layoutList.Control = this.listControls;
			this.layoutList.CustomizationFormText = "layoutList";
			this.layoutList.Location = new System.Drawing.Point(0, 0);
			this.layoutList.Name = "layoutList";
			this.layoutList.Size = new System.Drawing.Size(326, 416);
			this.layoutList.TextSize = new System.Drawing.Size(0, 0);
			this.layoutList.TextVisible = false;
			// 
			// emptySpaceItem1
			// 
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
			this.emptySpaceItem1.Location = new System.Drawing.Point(278, 416);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(48, 28);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			// 
			// layoutOK
			// 
			this.layoutOK.Control = this.btnOK;
			this.layoutOK.CustomizationFormText = "layoutOK";
			this.layoutOK.Location = new System.Drawing.Point(0, 416);
			this.layoutOK.MaxSize = new System.Drawing.Size(139, 28);
			this.layoutOK.MinSize = new System.Drawing.Size(139, 28);
			this.layoutOK.Name = "layoutOK";
			this.layoutOK.Size = new System.Drawing.Size(139, 28);
			this.layoutOK.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutOK.TextSize = new System.Drawing.Size(0, 0);
			this.layoutOK.TextVisible = false;
			// 
			// layoutCancel
			// 
			this.layoutCancel.Control = this.btnCancel;
			this.layoutCancel.CustomizationFormText = "layoutCancel";
			this.layoutCancel.Location = new System.Drawing.Point(139, 416);
			this.layoutCancel.MaxSize = new System.Drawing.Size(139, 28);
			this.layoutCancel.MinSize = new System.Drawing.Size(139, 28);
			this.layoutCancel.Name = "layoutCancel";
			this.layoutCancel.Size = new System.Drawing.Size(139, 28);
			this.layoutCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutCancel.TextSize = new System.Drawing.Size(0, 0);
			this.layoutCancel.TextVisible = false;
			// 
			// layoutProperties
			// 
			this.layoutProperties.Control = this.PropertyGrid;
			this.layoutProperties.CustomizationFormText = "layoutProperties";
			this.layoutProperties.Location = new System.Drawing.Point(336, 0);
			this.layoutProperties.Name = "layoutProperties";
			this.layoutProperties.Size = new System.Drawing.Size(392, 444);
			this.layoutProperties.TextSize = new System.Drawing.Size(0, 0);
			this.layoutProperties.TextVisible = false;
			this.layoutProperties.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			// 
			// splitterProperties
			// 
			this.splitterProperties.AllowHotTrack = true;
			this.splitterProperties.CustomizationFormText = "splitterProperties";
			this.splitterProperties.Location = new System.Drawing.Point(326, 0);
			this.splitterProperties.Name = "splitterProperties";
			this.splitterProperties.Size = new System.Drawing.Size(10, 444);
			// 
			// ObjectListEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(748, 464);
			this.Controls.Add(this.layoutControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "ObjectListEditor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Object List Editor";
			((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
			this.layoutControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.listControls)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Images)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutOK)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutCancel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.splitterProperties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private SpreadCommander.Documents.Controls.PropertyGridEx PropertyGrid;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraEditors.SimpleButton btnOK;
		private DevExpress.XtraEditors.ImageListBoxControl listControls;
		private DevExpress.XtraLayout.LayoutControlItem layoutList;
		private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
		private DevExpress.XtraLayout.LayoutControlItem layoutOK;
		private DevExpress.XtraLayout.LayoutControlItem layoutCancel;
		private DevExpress.XtraLayout.LayoutControlItem layoutProperties;
		private DevExpress.XtraLayout.SplitterItem splitterProperties;
		private DevExpress.Utils.ImageCollection Images;
	}
}