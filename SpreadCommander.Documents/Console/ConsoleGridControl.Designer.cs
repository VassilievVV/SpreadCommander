namespace SpreadCommander.Documents.Console
{
	partial class ConsoleGridControl
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
            this.Ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ribbonPageData = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.RibbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.DataSetView = new SpreadCommander.Documents.Controls.DataSetViewControl();
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).BeginInit();
            this.SuspendLayout();
            // 
            // Ribbon
            // 
            this.Ribbon.AutoSizeItems = true;
            this.Ribbon.ExpandCollapseItem.Id = 0;
            this.Ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Ribbon.ExpandCollapseItem,
            this.Ribbon.SearchEditItem});
            this.Ribbon.Location = new System.Drawing.Point(0, 0);
            this.Ribbon.MaxItemId = 11;
            this.Ribbon.Name = "Ribbon";
            this.Ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPageData});
            this.Ribbon.ShowSearchItem = true;
            this.Ribbon.Size = new System.Drawing.Size(1034, 146);
            this.Ribbon.StatusBar = this.RibbonStatusBar;
            // 
            // ribbonPageData
            // 
            this.ribbonPageData.Name = "ribbonPageData";
            this.ribbonPageData.Text = "Data";
            // 
            // RibbonStatusBar
            // 
            this.RibbonStatusBar.Location = new System.Drawing.Point(0, 770);
            this.RibbonStatusBar.Name = "RibbonStatusBar";
            this.RibbonStatusBar.Ribbon = this.Ribbon;
            this.RibbonStatusBar.Size = new System.Drawing.Size(1034, 22);
            this.RibbonStatusBar.Visible = false;
            // 
            // DataSetView
            // 
            this.DataSetView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataSetView.Location = new System.Drawing.Point(0, 146);
            this.DataSetView.Name = "DataSetView";
            this.DataSetView.Size = new System.Drawing.Size(1034, 624);
            this.DataSetView.TabIndex = 9;
            this.DataSetView.TargetSpreadsheet = null;
            this.DataSetView.Modified += new System.EventHandler(this.DataSetView_Modified);
            this.DataSetView.RibbonUpdateRequest += new System.EventHandler<SpreadCommander.Documents.Console.RibbonUpdateRequestEventArgs>(this.DataSetView_RibbonUpdateRequest);
            // 
            // ConsoleGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DataSetView);
            this.Controls.Add(this.Ribbon);
            this.Controls.Add(this.RibbonStatusBar);
            this.Name = "ConsoleGridControl";
            this.Size = new System.Drawing.Size(1034, 792);
            this.Load += new System.EventHandler(this.ConsoleGridControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Ribbon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private DevExpress.XtraBars.Ribbon.RibbonControl Ribbon;
		private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPageData;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar RibbonStatusBar;
        private Controls.DataSetViewControl DataSetView;
    }
}
