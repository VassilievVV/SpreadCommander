namespace SpreadCommander.Documents.Controls
{
	partial class ScriptControl
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
			this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager();
			this.viewScripts = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView();
			this.dlgOpen = new DevExpress.XtraEditors.XtraOpenFileDialog();
			this.dlgSave = new DevExpress.XtraEditors.XtraSaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.viewScripts)).BeginInit();
			this.SuspendLayout();
			// 
			// documentManager
			// 
			this.documentManager.ContainerControl = this;
			this.documentManager.View = this.viewScripts;
			this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.viewScripts});
			// 
			// viewScripts
			// 
			this.viewScripts.AllowHotkeyNavigation = DevExpress.Utils.DefaultBoolean.True;
			this.viewScripts.AllowResetLayout = DevExpress.Utils.DefaultBoolean.True;
			this.viewScripts.EnableFreeLayoutMode = DevExpress.Utils.DefaultBoolean.True;
			this.viewScripts.EnableStickySplitters = DevExpress.Utils.DefaultBoolean.True;
			this.viewScripts.ShowDockGuidesOnPressingShift = DevExpress.Utils.DefaultBoolean.False;
			this.viewScripts.ShowDocumentSelectorMenuOnCtrlAltDownArrow = DevExpress.Utils.DefaultBoolean.True;
			this.viewScripts.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.True;
			this.viewScripts.UseLoadingIndicator = DevExpress.Utils.DefaultBoolean.False;
			this.viewScripts.UseSnappingEmulation = DevExpress.Utils.DefaultBoolean.True;
			this.viewScripts.QueryControl += new DevExpress.XtraBars.Docking2010.Views.QueryControlEventHandler(this.ViewScripts_QueryControl);
			// 
			// dlgOpen
			// 
			this.dlgOpen.FileName = null;
			this.dlgOpen.Filter = "All files (*.*)|*.*";
			this.dlgOpen.RestoreDirectory = true;
			this.dlgOpen.Title = "Open script";
			// 
			// dlgSave
			// 
			this.dlgSave.FileName = null;
			this.dlgSave.Filter = "All files (*.*)|*.*";
			this.dlgSave.RestoreDirectory = true;
			this.dlgSave.Title = "Save script";
			// 
			// ScriptControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "ScriptControl";
			this.Size = new System.Drawing.Size(910, 696);
			this.Load += new System.EventHandler(this.ScriptControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.viewScripts)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
		private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView viewScripts;
		private DevExpress.XtraEditors.XtraOpenFileDialog dlgOpen;
		private DevExpress.XtraEditors.XtraSaveFileDialog dlgSave;
	}
}
