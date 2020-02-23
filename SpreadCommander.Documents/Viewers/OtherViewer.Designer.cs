namespace SpreadCommander.Documents.Viewers
{
	partial class OtherViewer
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
			this.lblMessage = new DevExpress.XtraEditors.LabelControl();
			this.SuspendLayout();
			// 
			// lblNotSupported
			// 
			this.lblMessage.Appearance.FontSizeDelta = 3;
			this.lblMessage.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
			this.lblMessage.Appearance.ForeColor = System.Drawing.Color.Red;
			this.lblMessage.Appearance.Options.UseFont = true;
			this.lblMessage.Appearance.Options.UseForeColor = true;
			this.lblMessage.Appearance.Options.UseTextOptions = true;
			this.lblMessage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.lblMessage.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
			this.lblMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblMessage.Location = new System.Drawing.Point(0, 0);
			this.lblMessage.Name = "lblNotSupported";
			this.lblMessage.Size = new System.Drawing.Size(781, 488);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "Preview is not available.";
			// 
			// OtherViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblMessage);
			this.Name = "OtherViewer";
			this.Size = new System.Drawing.Size(781, 488);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl lblMessage;
	}
}
