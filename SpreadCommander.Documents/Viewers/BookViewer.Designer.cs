namespace SpreadCommander.Documents.Viewers
{
	partial class BookViewer
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
            DevExpress.XtraSpellChecker.OptionsSpelling optionsSpelling1 = new DevExpress.XtraSpellChecker.OptionsSpelling();
            this.Editor = new DevExpress.XtraRichEdit.RichEditControl();
            this.spellChecker = new DevExpress.XtraSpellChecker.SpellChecker(this.components);
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.hideContainerRight = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.dockPanelComments = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.Comments = new DevExpress.XtraRichEdit.RichEditCommentControl();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.hideContainerRight.SuspendLayout();
            this.dockPanelComments.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // Editor
            // 
            this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor.Location = new System.Drawing.Point(0, 0);
            this.Editor.Name = "Editor";
            this.Editor.ReadOnly = true;
            this.Editor.Size = new System.Drawing.Size(1010, 746);
            this.Editor.SpellChecker = this.spellChecker;
            this.spellChecker.SetSpellCheckerOptions(this.Editor, optionsSpelling1);
            this.Editor.TabIndex = 0;
            this.Editor.Views.DraftView.AdjustColorsToSkins = true;
            this.Editor.Views.PrintLayoutView.AdjustColorsToSkins = true;
            this.Editor.Views.SimpleView.AdjustColorsToSkins = true;
            // 
            // spellChecker
            // 
            this.spellChecker.Culture = new System.Globalization.CultureInfo("");
            this.spellChecker.ParentContainer = null;
            // 
            // dockManager
            // 
            this.dockManager.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerRight});
            this.dockManager.Form = this;
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl"});
            // 
            // hideContainerRight
            // 
            this.hideContainerRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.hideContainerRight.Controls.Add(this.dockPanelComments);
            this.hideContainerRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.hideContainerRight.Location = new System.Drawing.Point(1010, 0);
            this.hideContainerRight.Name = "hideContainerRight";
            this.hideContainerRight.Size = new System.Drawing.Size(29, 746);
            // 
            // dockPanelComments
            // 
            this.dockPanelComments.Controls.Add(this.dockPanel1_Container);
            this.dockPanelComments.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanelComments.ID = new System.Guid("27d4133f-bf60-4c73-9c85-ab9d9fce17bd");
            this.dockPanelComments.Location = new System.Drawing.Point(0, 0);
            this.dockPanelComments.Name = "dockPanelComments";
            this.dockPanelComments.OriginalSize = new System.Drawing.Size(250, 200);
            this.dockPanelComments.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanelComments.SavedIndex = 0;
            this.dockPanelComments.Size = new System.Drawing.Size(250, 746);
            this.dockPanelComments.Text = "Comments";
            this.dockPanelComments.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.Comments);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 46);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(243, 697);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // Comments
            // 
            this.Comments.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.Comments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Comments.Location = new System.Drawing.Point(0, 0);
            this.Comments.Name = "Comments";
            this.Comments.ReadOnly = true;
            this.Comments.RichEditControl = this.Editor;
            this.Comments.Size = new System.Drawing.Size(243, 697);
            this.Comments.TabIndex = 0;
            // 
            // BookViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Editor);
            this.Controls.Add(this.hideContainerRight);
            this.Name = "BookViewer";
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.hideContainerRight.ResumeLayout(false);
            this.dockPanelComments.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraRichEdit.RichEditControl Editor;
		private DevExpress.XtraBars.Docking.DockManager dockManager;
		private DevExpress.XtraBars.Docking.DockPanel dockPanelComments;
		private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
		private DevExpress.XtraRichEdit.RichEditCommentControl Comments;
		private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerRight;
		private DevExpress.XtraSpellChecker.SpellChecker spellChecker;
	}
}
