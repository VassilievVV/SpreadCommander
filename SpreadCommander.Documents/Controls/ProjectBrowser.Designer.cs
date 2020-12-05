namespace SpreadCommander.Documents.Controls
{
	partial class ProjectBrowser
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
            this.imageFiles = new DevExpress.Utils.SvgImageCollection(this.components);
            this.treeProjectFiles = new DevExpress.XtraTreeList.TreeList();
            this.treeFilesText = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.imageFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeProjectFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // imageFiles
            // 
            this.imageFiles.Add("folder", "image://svgimages/icon builder/actions_folderclose.svg");
            this.imageFiles.Add("file", "image://svgimages/dashboards/new.svg");
            this.imageFiles.Add("spreadsheet", "image://svgimages/richedit/inserttable.svg");
            this.imageFiles.Add("csv", "image://svgimages/spreadsheet/freezetoprow.svg");
            this.imageFiles.Add("sql", "image://svgimages/dashboards/filterquery.svg");
            this.imageFiles.Add("script", "image://svgimages/icon builder/travel_map.svg");
            this.imageFiles.Add("document", "image://svgimages/pdf viewer/singlepageview.svg");
            this.imageFiles.Add("image", "image://svgimages/richedit/insertimage.svg");
            this.imageFiles.Add("dashboard", "image://svgimages/dashboards/layout.svg");
            this.imageFiles.Add("pdf", "image://svgimages/pdf viewer/documentpdf.svg");
            // 
            // treeProjectFiles
            // 
            this.treeProjectFiles.AutoFillColumn = this.treeFilesText;
            this.treeProjectFiles.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeFilesText});
            this.treeProjectFiles.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeProjectFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeProjectFiles.Location = new System.Drawing.Point(0, 0);
            this.treeProjectFiles.Name = "treeProjectFiles";
            this.treeProjectFiles.OptionsView.EnableAppearanceEvenRow = true;
            this.treeProjectFiles.OptionsView.EnableAppearanceOddRow = true;
            this.treeProjectFiles.OptionsView.ShowAutoFilterRow = true;
            this.treeProjectFiles.OptionsView.ShowColumns = false;
            this.treeProjectFiles.OptionsView.ShowHorzLines = false;
            this.treeProjectFiles.OptionsView.ShowRowFooterSummary = true;
            this.treeProjectFiles.OptionsView.ShowVertLines = false;
            this.treeProjectFiles.Size = new System.Drawing.Size(281, 464);
            this.treeProjectFiles.StateImageList = this.imageFiles;
            this.treeProjectFiles.TabIndex = 1;
            this.treeProjectFiles.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(this.TreeProjectFiles_GetStateImage);
            this.treeProjectFiles.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TreeProjectFiles_FocusedNodeChanged);
            this.treeProjectFiles.CustomColumnSort += new DevExpress.XtraTreeList.CustomColumnSortEventHandler(this.TreeProjectFiles_CustomColumnSort);
            this.treeProjectFiles.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.TreeProjectFiles_CustomDrawNodeCell);
            this.treeProjectFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeProjectFiles_DragDrop);
            this.treeProjectFiles.DragOver += new System.Windows.Forms.DragEventHandler(this.TreeProjectFiles_DragOver);
            this.treeProjectFiles.DoubleClick += new System.EventHandler(this.TreeProjectFiles_DoubleClick);
            this.treeProjectFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeProjectFiles_MouseDown);
            this.treeProjectFiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeProjectFiles_MouseMove);
            // 
            // treeFilesText
            // 
            this.treeFilesText.Caption = "Text";
            this.treeFilesText.FieldName = "Text";
            this.treeFilesText.Name = "treeFilesText";
            this.treeFilesText.OptionsColumn.AllowEdit = false;
            this.treeFilesText.OptionsColumn.ReadOnly = true;
            this.treeFilesText.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.String;
            this.treeFilesText.Visible = true;
            this.treeFilesText.VisibleIndex = 0;
            // 
            // ProjectBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeProjectFiles);
            this.Name = "ProjectBrowser";
            this.Size = new System.Drawing.Size(281, 464);
            this.Load += new System.EventHandler(this.ProjectBrowser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeProjectFiles)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.Utils.SvgImageCollection imageFiles;
		private DevExpress.XtraTreeList.TreeList treeProjectFiles;
		private DevExpress.XtraTreeList.Columns.TreeListColumn treeFilesText;
    }
}
