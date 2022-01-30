namespace SpreadCommander.Documents.Viewers
{
	partial class SyntaxViewer
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
            this.Editor = new SpreadCommander.Documents.Controls.ScriptEditorControl();
            this.SuspendLayout();
            // 
            // Editor
            // 
            this.Editor.DefaultExt = "";
            this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor.FileFilter = "All files|*.*";
            this.Editor.FileName = null;
            this.Editor.IsModified = false;
            this.Editor.Location = new System.Drawing.Point(0, 0);
            this.Editor.Name = "Editor";
            this.Editor.ReadOnly = false;
            this.Editor.ScriptText = "";
            this.Editor.ShowGutterMargin = false;
            this.Editor.ShowLineNumbers = false;
            this.Editor.Size = new System.Drawing.Size(889, 643);
            this.Editor.TabIndex = 0;
            // 
            // SyntaxViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Editor);
            this.Name = "SyntaxViewer";
            this.Size = new System.Drawing.Size(889, 643);
            this.ResumeLayout(false);

		}

		#endregion

		private Controls.ScriptEditorControl Editor;
	}
}
