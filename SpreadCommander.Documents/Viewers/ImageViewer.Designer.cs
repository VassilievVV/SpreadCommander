namespace SpreadCommander.Documents.Viewers
{
	partial class ImageViewer
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
			this.Editor = new DevExpress.XtraEditors.PictureEdit();
			this.transitionManager1 = new DevExpress.Utils.Animation.TransitionManager();
			((System.ComponentModel.ISupportInitialize)(this.Editor.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// Editor
			// 
			this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Editor.Location = new System.Drawing.Point(0, 0);
			this.Editor.Name = "Editor";
			this.Editor.Properties.ReadOnly = true;
			this.Editor.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
			this.Editor.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
			this.Editor.Size = new System.Drawing.Size(1039, 746);
			this.Editor.TabIndex = 0;
			// 
			// ImageViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Editor);
			this.Name = "ImageViewer";
			((System.ComponentModel.ISupportInitialize)(this.Editor.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.PictureEdit Editor;
		private DevExpress.Utils.Animation.TransitionManager transitionManager1;
	}
}
