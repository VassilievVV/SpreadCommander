namespace SpreadCommander.Documents.Dialogs
{
	partial class MemoEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoEditor));
			this.LayoutRoot = new DevExpress.XtraLayout.LayoutControl();
			this.lblCaption = new DevExpress.XtraEditors.LabelControl();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			this.btnOK = new DevExpress.XtraEditors.SimpleButton();
			this.Memo = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlGroupRoot = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutMemo = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutOK = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutCancel = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutCaption = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.LayoutRoot)).BeginInit();
			this.LayoutRoot.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Memo.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupRoot)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutMemo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutOK)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutCancel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutCaption)).BeginInit();
			this.SuspendLayout();
			// 
			// LayoutRoot
			// 
			this.LayoutRoot.Controls.Add(this.lblCaption);
			this.LayoutRoot.Controls.Add(this.btnCancel);
			this.LayoutRoot.Controls.Add(this.btnOK);
			this.LayoutRoot.Controls.Add(this.Memo);
			this.LayoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LayoutRoot.Location = new System.Drawing.Point(0, 0);
			this.LayoutRoot.Name = "LayoutRoot";
			this.LayoutRoot.Root = this.layoutControlGroupRoot;
			this.LayoutRoot.Size = new System.Drawing.Size(552, 347);
			this.LayoutRoot.TabIndex = 0;
			this.LayoutRoot.Text = "layoutControl1";
			// 
			// lblCaption
			// 
			this.lblCaption.AllowHtmlString = true;
			this.lblCaption.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
			this.lblCaption.Location = new System.Drawing.Point(12, 12);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(528, 13);
			this.lblCaption.StyleController = this.LayoutRoot;
			this.lblCaption.TabIndex = 9;
			this.lblCaption.Text = "Caption";
			this.lblCaption.Visible = false;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(134, 305);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(118, 30);
			this.btnCancel.StyleController = this.LayoutRoot;
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(12, 305);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(118, 30);
			this.btnOK.StyleController = this.LayoutRoot;
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "&OK";
			// 
			// Memo
			// 
			this.Memo.Location = new System.Drawing.Point(12, 29);
			this.Memo.Name = "Memo";
			this.Memo.Size = new System.Drawing.Size(528, 272);
			this.Memo.StyleController = this.LayoutRoot;
			this.Memo.TabIndex = 4;
			// 
			// layoutControlGroupRoot
			// 
			this.layoutControlGroupRoot.CustomizationFormText = "Root";
			this.layoutControlGroupRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutMemo,
            this.layoutOK,
            this.layoutCancel,
            this.layoutCaption});
			this.layoutControlGroupRoot.Name = "Root";
			this.layoutControlGroupRoot.Size = new System.Drawing.Size(552, 347);
			this.layoutControlGroupRoot.TextVisible = false;
			// 
			// layoutMemo
			// 
			this.layoutMemo.Control = this.Memo;
			this.layoutMemo.CustomizationFormText = "layoutMemo";
			this.layoutMemo.Location = new System.Drawing.Point(0, 17);
			this.layoutMemo.Name = "layoutMemo";
			this.layoutMemo.Size = new System.Drawing.Size(532, 276);
			this.layoutMemo.TextSize = new System.Drawing.Size(0, 0);
			this.layoutMemo.TextVisible = false;
			// 
			// layoutOK
			// 
			this.layoutOK.Control = this.btnOK;
			this.layoutOK.CustomizationFormText = "layoutOK";
			this.layoutOK.Location = new System.Drawing.Point(0, 293);
			this.layoutOK.MaxSize = new System.Drawing.Size(122, 34);
			this.layoutOK.MinSize = new System.Drawing.Size(122, 34);
			this.layoutOK.Name = "layoutOK";
			this.layoutOK.Size = new System.Drawing.Size(122, 34);
			this.layoutOK.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutOK.TextSize = new System.Drawing.Size(0, 0);
			this.layoutOK.TextVisible = false;
			// 
			// layoutCancel
			// 
			this.layoutCancel.Control = this.btnCancel;
			this.layoutCancel.CustomizationFormText = "layoutControlItem3";
			this.layoutCancel.Location = new System.Drawing.Point(122, 293);
			this.layoutCancel.MaxSize = new System.Drawing.Size(122, 34);
			this.layoutCancel.MinSize = new System.Drawing.Size(122, 34);
			this.layoutCancel.Name = "layoutCancel";
			this.layoutCancel.Size = new System.Drawing.Size(410, 34);
			this.layoutCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutCancel.TextSize = new System.Drawing.Size(0, 0);
			this.layoutCancel.TextVisible = false;
			// 
			// layoutCaption
			// 
			this.layoutCaption.Control = this.lblCaption;
			this.layoutCaption.CustomizationFormText = "layoutCaption";
			this.layoutCaption.Location = new System.Drawing.Point(0, 0);
			this.layoutCaption.Name = "layoutCaption";
			this.layoutCaption.Size = new System.Drawing.Size(532, 17);
			this.layoutCaption.TextSize = new System.Drawing.Size(0, 0);
			this.layoutCaption.TextVisible = false;
			this.layoutCaption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			// 
			// MemoEditor
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(552, 347);
			this.Controls.Add(this.LayoutRoot);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "MemoEditor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Text";
			((System.ComponentModel.ISupportInitialize)(this.LayoutRoot)).EndInit();
			this.LayoutRoot.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.Memo.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupRoot)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutMemo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutOK)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutCancel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutCaption)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl LayoutRoot;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraEditors.SimpleButton btnOK;
		private DevExpress.XtraEditors.MemoEdit Memo;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupRoot;
		private DevExpress.XtraLayout.LayoutControlItem layoutMemo;
		private DevExpress.XtraLayout.LayoutControlItem layoutOK;
		private DevExpress.XtraLayout.LayoutControlItem layoutCancel;
		private DevExpress.XtraEditors.LabelControl lblCaption;
		private DevExpress.XtraLayout.LayoutControlItem layoutCaption;
	}
}