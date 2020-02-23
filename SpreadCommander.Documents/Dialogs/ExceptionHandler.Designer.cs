namespace SpreadCommander.Documents.Dialogs
{
	partial class ExceptionHandler
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionHandler));
			this.NavBar = new DevExpress.XtraNavBar.NavBarControl();
			this.barException = new DevExpress.XtraNavBar.NavBarGroup();
			this.navDescription = new DevExpress.XtraNavBar.NavBarItem();
			this.navDetails = new DevExpress.XtraNavBar.NavBarItem();
			this.navConfig = new DevExpress.XtraNavBar.NavBarItem();
			this.TabControl = new DevExpress.XtraTab.XtraTabControl();
			this.tabDescription = new DevExpress.XtraTab.XtraTabPage();
			this.memoDescription = new DevExpress.XtraEditors.MemoEdit();
			this.tabDetails = new DevExpress.XtraTab.XtraTabPage();
			this.memoDetails = new DevExpress.XtraEditors.MemoEdit();
			this.tabConfig = new DevExpress.XtraTab.XtraTabPage();
			this.memoConfig = new DevExpress.XtraEditors.MemoEdit();
			this.btnContinue = new DevExpress.XtraEditors.SimpleButton();
			this.btnExit = new DevExpress.XtraEditors.SimpleButton();
			((System.ComponentModel.ISupportInitialize)(this.NavBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TabControl)).BeginInit();
			this.TabControl.SuspendLayout();
			this.tabDescription.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.memoDescription.Properties)).BeginInit();
			this.tabDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.memoDetails.Properties)).BeginInit();
			this.tabConfig.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.memoConfig.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// NavBar
			// 
			this.NavBar.ActiveGroup = this.barException;
			this.NavBar.ContentButtonHint = null;
			this.NavBar.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.barException});
			this.NavBar.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navDescription,
            this.navDetails,
            this.navConfig});
			this.NavBar.LinkSelectionMode = DevExpress.XtraNavBar.LinkSelectionModeType.OneInControl;
			this.NavBar.Location = new System.Drawing.Point(0, 0);
			this.NavBar.Name = "NavBar";
			this.NavBar.OptionsNavPane.ExpandedWidth = 112;
			this.NavBar.Size = new System.Drawing.Size(112, 258);
			this.NavBar.TabIndex = 0;
			this.NavBar.Text = "navBarControl1";
			this.NavBar.SelectedLinkChanged += new DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventHandler(this.NavBar_SelectedLinkChanged);
			// 
			// barException
			// 
			this.barException.Caption = "Exception";
			this.barException.Expanded = true;
			this.barException.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.LargeIconsText;
			this.barException.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navDescription),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navDetails),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navConfig)});
			this.barException.Name = "barException";
			// 
			// navDescription
			// 
			this.navDescription.CanDrag = false;
			this.navDescription.Caption = "Description";
			this.navDescription.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("navDescription.ImageOptions.SvgImage")));
			this.navDescription.Name = "navDescription";
			this.navDescription.Tag = 0;
			// 
			// navDetails
			// 
			this.navDetails.CanDrag = false;
			this.navDetails.Caption = "Details";
			this.navDetails.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("navDetails.ImageOptions.SvgImage")));
			this.navDetails.Name = "navDetails";
			this.navDetails.Tag = 1;
			// 
			// navConfig
			// 
			this.navConfig.CanDrag = false;
			this.navConfig.Caption = "Config";
			this.navConfig.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("navConfig.ImageOptions.SvgImage")));
			this.navConfig.Name = "navConfig";
			this.navConfig.Tag = 2;
			// 
			// TabControl
			// 
			this.TabControl.Location = new System.Drawing.Point(112, 0);
			this.TabControl.Name = "TabControl";
			this.TabControl.SelectedTabPage = this.tabDescription;
			this.TabControl.Size = new System.Drawing.Size(424, 258);
			this.TabControl.TabIndex = 1;
			this.TabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabDescription,
            this.tabDetails,
            this.tabConfig});
			this.TabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.TabControl_SelectedPageChanged);
			// 
			// tabDescription
			// 
			this.tabDescription.Controls.Add(this.memoDescription);
			this.tabDescription.Name = "tabDescription";
			this.tabDescription.Size = new System.Drawing.Size(422, 235);
			this.tabDescription.Tag = "0";
			this.tabDescription.Text = "Description";
			// 
			// memoDescription
			// 
			this.memoDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoDescription.Location = new System.Drawing.Point(0, 0);
			this.memoDescription.Name = "memoDescription";
			this.memoDescription.Properties.Appearance.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.memoDescription.Properties.Appearance.Options.UseFont = true;
			this.memoDescription.Properties.ReadOnly = true;
			this.memoDescription.Size = new System.Drawing.Size(422, 235);
			this.memoDescription.TabIndex = 0;
			// 
			// tabDetails
			// 
			this.tabDetails.Controls.Add(this.memoDetails);
			this.tabDetails.Name = "tabDetails";
			this.tabDetails.Size = new System.Drawing.Size(422, 235);
			this.tabDetails.Tag = "1";
			this.tabDetails.Text = "Details";
			// 
			// memoDetails
			// 
			this.memoDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoDetails.Location = new System.Drawing.Point(0, 0);
			this.memoDetails.Name = "memoDetails";
			this.memoDetails.Properties.Appearance.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.memoDetails.Properties.Appearance.Options.UseFont = true;
			this.memoDetails.Properties.ReadOnly = true;
			this.memoDetails.Size = new System.Drawing.Size(422, 235);
			this.memoDetails.TabIndex = 4;
			// 
			// tabConfig
			// 
			this.tabConfig.Controls.Add(this.memoConfig);
			this.tabConfig.Name = "tabConfig";
			this.tabConfig.Size = new System.Drawing.Size(422, 235);
			this.tabConfig.Tag = "2";
			this.tabConfig.Text = "Configuration";
			// 
			// memoConfig
			// 
			this.memoConfig.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoConfig.Location = new System.Drawing.Point(0, 0);
			this.memoConfig.Name = "memoConfig";
			this.memoConfig.Properties.Appearance.Font = new System.Drawing.Font("Courier New", 8.25F);
			this.memoConfig.Properties.Appearance.Options.UseFont = true;
			this.memoConfig.Properties.ReadOnly = true;
			this.memoConfig.Size = new System.Drawing.Size(422, 235);
			this.memoConfig.TabIndex = 0;
			// 
			// btnContinue
			// 
			this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnContinue.Location = new System.Drawing.Point(78, 264);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(128, 32);
			this.btnContinue.TabIndex = 2;
			this.btnContinue.Text = "&Continue";
			// 
			// btnExit
			// 
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btnExit.Location = new System.Drawing.Point(212, 264);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(128, 32);
			this.btnExit.TabIndex = 3;
			this.btnExit.Text = "Terminate";
			// 
			// ExceptionHandler
			// 
			this.AcceptButton = this.btnContinue;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(536, 302);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.TabControl);
			this.Controls.Add(this.NavBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ExceptionHandler";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Error";
			((System.ComponentModel.ISupportInitialize)(this.NavBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TabControl)).EndInit();
			this.TabControl.ResumeLayout(false);
			this.tabDescription.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.memoDescription.Properties)).EndInit();
			this.tabDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.memoDetails.Properties)).EndInit();
			this.tabConfig.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.memoConfig.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraNavBar.NavBarControl NavBar;
		private DevExpress.XtraNavBar.NavBarGroup barException;
		private DevExpress.XtraTab.XtraTabControl TabControl;
		private DevExpress.XtraTab.XtraTabPage tabDescription;
		private DevExpress.XtraTab.XtraTabPage tabDetails;
		private DevExpress.XtraTab.XtraTabPage tabConfig;
		private DevExpress.XtraEditors.SimpleButton btnContinue;
		private DevExpress.XtraEditors.SimpleButton btnExit;
		private DevExpress.XtraEditors.MemoEdit memoDescription;
		private DevExpress.XtraEditors.MemoEdit memoDetails;
		private DevExpress.XtraEditors.MemoEdit memoConfig;
		private DevExpress.XtraNavBar.NavBarItem navDescription;
		private DevExpress.XtraNavBar.NavBarItem navDetails;
		private DevExpress.XtraNavBar.NavBarItem navConfig;
	}
}