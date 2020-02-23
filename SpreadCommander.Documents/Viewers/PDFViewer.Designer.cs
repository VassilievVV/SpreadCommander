namespace SpreadCommander.Documents.Viewers
{
	partial class PDFViewer
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
			this.Editor = new DevExpress.XtraPdfViewer.PdfViewer();
			this.SuspendLayout();
			// 
			// Editor
			// 
			this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Editor.Location = new System.Drawing.Point(0, 0);
			this.Editor.Name = "Editor";
			this.Editor.ReadOnly = true;
			this.Editor.Size = new System.Drawing.Size(1039, 746);
			this.Editor.TabIndex = 0;
			// 
			// PDFViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Editor);
			this.Name = "PDFViewer";
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraPdfViewer.PdfViewer Editor;
	}
}
