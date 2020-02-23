namespace SpreadCommander.Documents.Dialogs.SyntaxEditorDialogs
{
	partial class EditorGotoLineForm
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
			this.lblCaption = new DevExpress.XtraEditors.LabelControl();
			this.spinLineNumber = new DevExpress.XtraEditors.SpinEdit();
			this.btnOK = new DevExpress.XtraEditors.SimpleButton();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.spinLineNumber.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// lblCaption
			// 
			this.lblCaption.Location = new System.Drawing.Point(8, 0);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(4, 13);
			this.lblCaption.TabIndex = 0;
			this.lblCaption.Text = "-";
			// 
			// spinLineNumber
			// 
			this.spinLineNumber.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spinLineNumber.Location = new System.Drawing.Point(8, 24);
			this.spinLineNumber.Name = "spinLineNumber";
			this.spinLineNumber.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.spinLineNumber.Properties.MaxValue = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.spinLineNumber.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spinLineNumber.Size = new System.Drawing.Size(224, 20);
			this.spinLineNumber.TabIndex = 1;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(8, 56);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(104, 24);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "&OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(120, 56);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(104, 24);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "&Cancel";
			// 
			// EditorGotoLineForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(242, 87);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.spinLineNumber);
			this.Controls.Add(this.lblCaption);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditorGotoLineForm";
			this.Opacity = 0.9;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Goto Line";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorGotoLineForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.spinLineNumber.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl lblCaption;
		private DevExpress.XtraEditors.SpinEdit spinLineNumber;
		private DevExpress.XtraEditors.SimpleButton btnOK;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
	}
}