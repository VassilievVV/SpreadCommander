namespace SpreadCommander.Documents.Dialogs.SyntaxEditorDialogs
{
	partial class EditorFindReplaceForm
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
            this.layoutRoot = new DevExpress.XtraLayout.LayoutControl();
            this.btnReplaceSelection = new DevExpress.XtraEditors.SimpleButton();
            this.listOptions = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnMarkAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnReplaceAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnReplace = new DevExpress.XtraEditors.SimpleButton();
            this.btnFindNext = new DevExpress.XtraEditors.SimpleButton();
            this.comboReplaceWith = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comboFindWhat = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutGroupRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutFindWhat = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutReplaceWith = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutButtonFindNext = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutButtonReplace = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutButtonReplaceAll = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutButtonMarkAll = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutButtonClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutOptions = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutButtonReplaceSelection = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutRoot)).BeginInit();
            this.layoutRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboReplaceWith.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFindWhat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutFindWhat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReplaceWith)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonFindNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonReplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonReplaceAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonMarkAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonReplaceSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutRoot
            // 
            this.layoutRoot.Controls.Add(this.btnReplaceSelection);
            this.layoutRoot.Controls.Add(this.listOptions);
            this.layoutRoot.Controls.Add(this.btnClose);
            this.layoutRoot.Controls.Add(this.btnMarkAll);
            this.layoutRoot.Controls.Add(this.btnReplaceAll);
            this.layoutRoot.Controls.Add(this.btnReplace);
            this.layoutRoot.Controls.Add(this.btnFindNext);
            this.layoutRoot.Controls.Add(this.comboReplaceWith);
            this.layoutRoot.Controls.Add(this.comboFindWhat);
            this.layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRoot.Location = new System.Drawing.Point(0, 0);
            this.layoutRoot.Name = "layoutRoot";
            this.layoutRoot.OptionsFocus.EnableAutoTabOrder = false;
            this.layoutRoot.Root = this.layoutGroupRoot;
            this.layoutRoot.Size = new System.Drawing.Size(526, 180);
            this.layoutRoot.TabIndex = 0;
            this.layoutRoot.Text = "layoutControl1";
            // 
            // btnReplaceSelection
            // 
            this.btnReplaceSelection.Location = new System.Drawing.Point(392, 64);
            this.btnReplaceSelection.Name = "btnReplaceSelection";
            this.btnReplaceSelection.Size = new System.Drawing.Size(122, 22);
            this.btnReplaceSelection.StyleController = this.layoutRoot;
            this.btnReplaceSelection.TabIndex = 5;
            this.btnReplaceSelection.Text = "Replace &selection";
            this.btnReplaceSelection.Click += new System.EventHandler(this.BtnReplaceSelection_Click);
            // 
            // listOptions
            // 
            this.listOptions.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("Match case"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("Match whole word")});
            this.listOptions.Location = new System.Drawing.Point(12, 60);
            this.listOptions.Name = "listOptions";
            this.listOptions.Size = new System.Drawing.Size(376, 108);
            this.listOptions.StyleController = this.layoutRoot;
            this.listOptions.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(392, 142);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(122, 22);
            this.btnClose.StyleController = this.layoutRoot;
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnMarkAll
            // 
            this.btnMarkAll.Location = new System.Drawing.Point(392, 116);
            this.btnMarkAll.Name = "btnMarkAll";
            this.btnMarkAll.Size = new System.Drawing.Size(122, 22);
            this.btnMarkAll.StyleController = this.layoutRoot;
            this.btnMarkAll.TabIndex = 7;
            this.btnMarkAll.Text = "&Mark all";
            this.btnMarkAll.Click += new System.EventHandler(this.BtnMarkAll_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(392, 90);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(122, 22);
            this.btnReplaceAll.StyleController = this.layoutRoot;
            this.btnReplaceAll.TabIndex = 6;
            this.btnReplaceAll.Text = "Replace &all";
            this.btnReplaceAll.Click += new System.EventHandler(this.BtnReplaceAll_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(392, 38);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(122, 22);
            this.btnReplace.StyleController = this.layoutRoot;
            this.btnReplace.TabIndex = 4;
            this.btnReplace.Text = "&Replace";
            this.btnReplace.Click += new System.EventHandler(this.BtnReplace_Click);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(392, 12);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(122, 22);
            this.btnFindNext.StyleController = this.layoutRoot;
            this.btnFindNext.TabIndex = 3;
            this.btnFindNext.Text = "Find &next";
            this.btnFindNext.Click += new System.EventHandler(this.BtnFindNext_Click);
            // 
            // comboReplaceWith
            // 
            this.comboReplaceWith.Location = new System.Drawing.Point(89, 36);
            this.comboReplaceWith.Name = "comboReplaceWith";
            this.comboReplaceWith.Properties.AutoComplete = false;
            this.comboReplaceWith.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboReplaceWith.Size = new System.Drawing.Size(299, 20);
            this.comboReplaceWith.StyleController = this.layoutRoot;
            this.comboReplaceWith.TabIndex = 1;
            // 
            // comboFindWhat
            // 
            this.comboFindWhat.Location = new System.Drawing.Point(89, 12);
            this.comboFindWhat.Name = "comboFindWhat";
            this.comboFindWhat.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.comboFindWhat.Properties.AutoComplete = false;
            this.comboFindWhat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboFindWhat.Size = new System.Drawing.Size(299, 20);
            this.comboFindWhat.StyleController = this.layoutRoot;
            this.comboFindWhat.TabIndex = 0;
            // 
            // layoutGroupRoot
            // 
            this.layoutGroupRoot.CustomizationFormText = "layoutControlGroup1";
            this.layoutGroupRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutFindWhat,
            this.layoutReplaceWith,
            this.layoutButtonFindNext,
            this.layoutButtonReplace,
            this.layoutButtonReplaceAll,
            this.layoutButtonMarkAll,
            this.layoutButtonClose,
            this.layoutOptions,
            this.layoutButtonReplaceSelection});
            this.layoutGroupRoot.Name = "Root";
            this.layoutGroupRoot.Size = new System.Drawing.Size(526, 180);
            this.layoutGroupRoot.TextVisible = false;
            // 
            // layoutFindWhat
            // 
            this.layoutFindWhat.Control = this.comboFindWhat;
            this.layoutFindWhat.CustomizationFormText = "Find what:";
            this.layoutFindWhat.Location = new System.Drawing.Point(0, 0);
            this.layoutFindWhat.Name = "layoutFindWhat";
            this.layoutFindWhat.Size = new System.Drawing.Size(380, 24);
            this.layoutFindWhat.Text = "Find what:";
            this.layoutFindWhat.TextSize = new System.Drawing.Size(65, 13);
            // 
            // layoutReplaceWith
            // 
            this.layoutReplaceWith.Control = this.comboReplaceWith;
            this.layoutReplaceWith.CustomizationFormText = "Replace with:";
            this.layoutReplaceWith.Location = new System.Drawing.Point(0, 24);
            this.layoutReplaceWith.Name = "layoutReplaceWith";
            this.layoutReplaceWith.Size = new System.Drawing.Size(380, 24);
            this.layoutReplaceWith.Text = "Replace with:";
            this.layoutReplaceWith.TextSize = new System.Drawing.Size(65, 13);
            // 
            // layoutButtonFindNext
            // 
            this.layoutButtonFindNext.Control = this.btnFindNext;
            this.layoutButtonFindNext.CustomizationFormText = "layoutControlItem2";
            this.layoutButtonFindNext.Location = new System.Drawing.Point(380, 0);
            this.layoutButtonFindNext.Name = "layoutButtonFindNext";
            this.layoutButtonFindNext.Size = new System.Drawing.Size(126, 26);
            this.layoutButtonFindNext.TextSize = new System.Drawing.Size(0, 0);
            this.layoutButtonFindNext.TextVisible = false;
            // 
            // layoutButtonReplace
            // 
            this.layoutButtonReplace.Control = this.btnReplace;
            this.layoutButtonReplace.CustomizationFormText = "layoutControlItem3";
            this.layoutButtonReplace.Location = new System.Drawing.Point(380, 26);
            this.layoutButtonReplace.Name = "layoutButtonReplace";
            this.layoutButtonReplace.Size = new System.Drawing.Size(126, 26);
            this.layoutButtonReplace.TextSize = new System.Drawing.Size(0, 0);
            this.layoutButtonReplace.TextVisible = false;
            // 
            // layoutButtonReplaceAll
            // 
            this.layoutButtonReplaceAll.Control = this.btnReplaceAll;
            this.layoutButtonReplaceAll.CustomizationFormText = "layoutControlItem4";
            this.layoutButtonReplaceAll.Location = new System.Drawing.Point(380, 78);
            this.layoutButtonReplaceAll.Name = "layoutButtonReplaceAll";
            this.layoutButtonReplaceAll.Size = new System.Drawing.Size(126, 26);
            this.layoutButtonReplaceAll.TextSize = new System.Drawing.Size(0, 0);
            this.layoutButtonReplaceAll.TextVisible = false;
            // 
            // layoutButtonMarkAll
            // 
            this.layoutButtonMarkAll.Control = this.btnMarkAll;
            this.layoutButtonMarkAll.CustomizationFormText = "layoutControlItem5";
            this.layoutButtonMarkAll.Location = new System.Drawing.Point(380, 104);
            this.layoutButtonMarkAll.Name = "layoutButtonMarkAll";
            this.layoutButtonMarkAll.Size = new System.Drawing.Size(126, 26);
            this.layoutButtonMarkAll.TextSize = new System.Drawing.Size(0, 0);
            this.layoutButtonMarkAll.TextVisible = false;
            // 
            // layoutButtonClose
            // 
            this.layoutButtonClose.Control = this.btnClose;
            this.layoutButtonClose.CustomizationFormText = "layoutControlItem6";
            this.layoutButtonClose.Location = new System.Drawing.Point(380, 130);
            this.layoutButtonClose.Name = "layoutButtonClose";
            this.layoutButtonClose.Size = new System.Drawing.Size(126, 30);
            this.layoutButtonClose.TextSize = new System.Drawing.Size(0, 0);
            this.layoutButtonClose.TextVisible = false;
            // 
            // layoutOptions
            // 
            this.layoutOptions.Control = this.listOptions;
            this.layoutOptions.CustomizationFormText = "layoutOptions";
            this.layoutOptions.Location = new System.Drawing.Point(0, 48);
            this.layoutOptions.MinSize = new System.Drawing.Size(27, 27);
            this.layoutOptions.Name = "layoutOptions";
            this.layoutOptions.Size = new System.Drawing.Size(380, 112);
            this.layoutOptions.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutOptions.TextSize = new System.Drawing.Size(0, 0);
            this.layoutOptions.TextVisible = false;
            // 
            // layoutButtonReplaceSelection
            // 
            this.layoutButtonReplaceSelection.Control = this.btnReplaceSelection;
            this.layoutButtonReplaceSelection.Location = new System.Drawing.Point(380, 52);
            this.layoutButtonReplaceSelection.Name = "layoutButtonReplaceSelection";
            this.layoutButtonReplaceSelection.Size = new System.Drawing.Size(126, 26);
            this.layoutButtonReplaceSelection.TextSize = new System.Drawing.Size(0, 0);
            this.layoutButtonReplaceSelection.TextVisible = false;
            // 
            // EditorFindReplaceForm
            // 
            this.AcceptButton = this.btnFindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(526, 180);
            this.Controls.Add(this.layoutRoot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditorFindReplaceForm";
            this.Opacity = 0.9D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find/Replace";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditorFindReplaceForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.layoutRoot)).EndInit();
            this.layoutRoot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboReplaceWith.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFindWhat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutFindWhat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReplaceWith)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonFindNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonReplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonReplaceAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonMarkAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutButtonReplaceSelection)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutRoot;
		private DevExpress.XtraLayout.LayoutControlGroup layoutGroupRoot;
		private DevExpress.XtraEditors.ComboBoxEdit comboReplaceWith;
		private DevExpress.XtraEditors.ComboBoxEdit comboFindWhat;
		private DevExpress.XtraLayout.LayoutControlItem layoutFindWhat;
		private DevExpress.XtraLayout.LayoutControlItem layoutReplaceWith;
		private DevExpress.XtraEditors.SimpleButton btnClose;
		private DevExpress.XtraEditors.SimpleButton btnMarkAll;
		private DevExpress.XtraEditors.SimpleButton btnReplaceAll;
		private DevExpress.XtraEditors.SimpleButton btnReplace;
		private DevExpress.XtraEditors.SimpleButton btnFindNext;
		private DevExpress.XtraLayout.LayoutControlItem layoutButtonFindNext;
		private DevExpress.XtraLayout.LayoutControlItem layoutButtonReplace;
		private DevExpress.XtraLayout.LayoutControlItem layoutButtonReplaceAll;
		private DevExpress.XtraLayout.LayoutControlItem layoutButtonMarkAll;
		private DevExpress.XtraLayout.LayoutControlItem layoutButtonClose;
		private DevExpress.XtraEditors.CheckedListBoxControl listOptions;
		private DevExpress.XtraLayout.LayoutControlItem layoutOptions;
		private DevExpress.XtraEditors.SimpleButton btnReplaceSelection;
		private DevExpress.XtraLayout.LayoutControlItem layoutButtonReplaceSelection;
	}
}