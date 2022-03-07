namespace SpreadCommander.Documents.Controls
{
	partial class ScriptEditorControl
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
            this.syntaxDocument = new Alsing.Windows.Forms.Document.SyntaxDocument(this.components);
            this.dlgOpen = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.dlgSave = new DevExpress.XtraEditors.XtraSaveFileDialog(this.components);
            this.syntaxEditor = new SpreadCommander.Documents.Controls.SyntaxEditorControl();
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.barUndo = new DevExpress.XtraBars.BarButtonItem();
            this.barRedo = new DevExpress.XtraBars.BarButtonItem();
            this.barCut = new DevExpress.XtraBars.BarButtonItem();
            this.barCopy = new DevExpress.XtraBars.BarButtonItem();
            this.barPaste = new DevExpress.XtraBars.BarButtonItem();
            this.barToggleBookmark = new DevExpress.XtraBars.BarButtonItem();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barStatus = new DevExpress.XtraBars.Bar();
            this.barCursor = new DevExpress.XtraBars.BarStaticItem();
            this.barSelection = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imagesPopup = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesPopup)).BeginInit();
            this.SuspendLayout();
            // 
            // syntaxDocument
            // 
            this.syntaxDocument.Lines = new string[] {
        ""};
            this.syntaxDocument.MaxUndoBufferSize = 1000;
            this.syntaxDocument.Modified = false;
            this.syntaxDocument.UndoStep = 0;
            this.syntaxDocument.Change += new System.EventHandler(this.SyntaxDocument_Change);
            this.syntaxDocument.ModifiedChanged += new System.EventHandler(this.SyntaxDocument_ModifiedChanged);
            this.syntaxDocument.RowParsed += new Alsing.Windows.Forms.Document.Parser.ParserEventHandler(this.SyntaxDocument_RowParsed);
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = null;
            this.dlgOpen.Filter = "All files|*.*";
            this.dlgOpen.RestoreDirectory = true;
            this.dlgOpen.Title = "Open script";
            // 
            // dlgSave
            // 
            this.dlgSave.FileName = null;
            this.dlgSave.Filter = "All files|*.*";
            this.dlgSave.RestoreDirectory = true;
            this.dlgSave.Title = "Save script";
            // 
            // syntaxEditor
            // 
            this.syntaxEditor.ActiveView = Alsing.Windows.Forms.ActiveView.BottomRight;
            this.syntaxEditor.AllowDrop = true;
            this.syntaxEditor.AutoListPosition = null;
            this.syntaxEditor.AutoListSelectedText = "a123";
            this.syntaxEditor.AutoListVisible = false;
            this.syntaxEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.syntaxEditor.BorderStyle = Alsing.Windows.Forms.Drawing.BorderStyle.None;
            this.syntaxEditor.ChildBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.syntaxEditor.CopyAsRTF = true;
            this.syntaxEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor.Document = this.syntaxDocument;
            this.syntaxEditor.FontName = "Courier new";
            this.syntaxEditor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.syntaxEditor.GutterMarginBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.syntaxEditor.GutterMarginColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.syntaxEditor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.syntaxEditor.InactiveSelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.syntaxEditor.InactiveSelectionForeColor = System.Drawing.Color.Black;
            this.syntaxEditor.InfoTipCount = 1;
            this.syntaxEditor.InfoTipPosition = null;
            this.syntaxEditor.InfoTipSelectedIndex = 1;
            this.syntaxEditor.InfoTipVisible = false;
            this.syntaxEditor.LineNumberBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.syntaxEditor.Location = new System.Drawing.Point(0, 0);
            this.syntaxEditor.LockCursorUpdate = false;
            this.syntaxEditor.Name = "syntaxEditor";
            this.syntaxEditor.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.barManager.SetPopupContextMenu(this.syntaxEditor, this.popupMenu);
            this.syntaxEditor.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(221)))), ((int)(((byte)(245)))));
            this.syntaxEditor.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.syntaxEditor.SeparatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.syntaxEditor.ShowScopeIndicator = false;
            this.syntaxEditor.Size = new System.Drawing.Size(992, 685);
            this.syntaxEditor.SmoothScroll = false;
            this.syntaxEditor.SplitViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.syntaxEditor.SplitviewH = -4;
            this.syntaxEditor.SplitviewV = -4;
            this.syntaxEditor.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.syntaxEditor.TabIndex = 0;
            this.syntaxEditor.TabsToSpaces = true;
            this.syntaxEditor.WhitespaceColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.syntaxEditor.CaretChange += new System.EventHandler(this.SyntaxEditor_CaretChange);
            this.syntaxEditor.SelectionChange += new System.EventHandler(this.SyntaxEditor_SelectionChange);
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barUndo),
            new DevExpress.XtraBars.LinkPersistInfo(this.barRedo),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCut, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopy),
            new DevExpress.XtraBars.LinkPersistInfo(this.barPaste),
            new DevExpress.XtraBars.LinkPersistInfo(this.barToggleBookmark, true)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.BeforePopup += new System.ComponentModel.CancelEventHandler(this.PopupMenu_BeforePopup);
            // 
            // barUndo
            // 
            this.barUndo.Caption = "Undo";
            this.barUndo.Id = 4;
            this.barUndo.ImageOptions.ImageIndex = 4;
            this.barUndo.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z));
            this.barUndo.Name = "barUndo";
            this.barUndo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarUndo_ItemClick);
            // 
            // barRedo
            // 
            this.barRedo.Caption = "Redo";
            this.barRedo.Id = 5;
            this.barRedo.ImageOptions.ImageIndex = 5;
            this.barRedo.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y));
            this.barRedo.Name = "barRedo";
            this.barRedo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarRedo_ItemClick);
            // 
            // barCut
            // 
            this.barCut.Caption = "Cut";
            this.barCut.Id = 0;
            this.barCut.ImageOptions.ImageIndex = 0;
            this.barCut.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X));
            this.barCut.Name = "barCut";
            this.barCut.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCut_ItemClick);
            // 
            // barCopy
            // 
            this.barCopy.Caption = "Copy";
            this.barCopy.Id = 1;
            this.barCopy.ImageOptions.ImageIndex = 1;
            this.barCopy.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C));
            this.barCopy.Name = "barCopy";
            this.barCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarCopy_ItemClick);
            // 
            // barPaste
            // 
            this.barPaste.Caption = "Paste";
            this.barPaste.Id = 2;
            this.barPaste.ImageOptions.ImageIndex = 2;
            this.barPaste.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V));
            this.barPaste.Name = "barPaste";
            this.barPaste.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarPaste_ItemClick);
            // 
            // barToggleBookmark
            // 
            this.barToggleBookmark.Caption = "Toggle bookmark";
            this.barToggleBookmark.Id = 3;
            this.barToggleBookmark.ImageOptions.ImageIndex = 3;
            this.barToggleBookmark.Name = "barToggleBookmark";
            this.barToggleBookmark.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarToggleBookmark_ItemClick);
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barStatus});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Images = this.imagesPopup;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barCut,
            this.barCopy,
            this.barPaste,
            this.barToggleBookmark,
            this.barUndo,
            this.barRedo,
            this.barCursor,
            this.barSelection});
            this.barManager.MaxItemId = 8;
            this.barManager.StatusBar = this.barStatus;
            // 
            // barStatus
            // 
            this.barStatus.BarName = "Status Bar";
            this.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barStatus.DockCol = 0;
            this.barStatus.DockRow = 0;
            this.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.barStatus.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barCursor),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSelection)});
            this.barStatus.OptionsBar.AllowQuickCustomization = false;
            this.barStatus.OptionsBar.DrawDragBorder = false;
            this.barStatus.OptionsBar.UseWholeRow = true;
            this.barStatus.Text = "Status Bar";
            // 
            // barCursor
            // 
            this.barCursor.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barCursor.Caption = "Cursor";
            this.barCursor.Id = 6;
            this.barCursor.MinWidth = 100;
            this.barCursor.Name = "barCursor";
            // 
            // barSelection
            // 
            this.barSelection.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barSelection.Caption = "Selection";
            this.barSelection.Id = 7;
            this.barSelection.MinWidth = 200;
            this.barSelection.Name = "barSelection";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(992, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 685);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(992, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 685);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(992, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 685);
            // 
            // imagesPopup
            // 
            this.imagesPopup.Add("cut", "image://svgimages/outlook inspired/cut.svg");
            this.imagesPopup.Add("copy", "image://svgimages/edit/copy.svg");
            this.imagesPopup.Add("pastespecial", "image://svgimages/richedit/pastespecial.svg");
            this.imagesPopup.Add("actions_bookmark", "image://svgimages/icon builder/actions_bookmark.svg");
            this.imagesPopup.Add("undo", "image://svgimages/dashboards/undo.svg");
            this.imagesPopup.Add("redo", "image://svgimages/dashboards/redo.svg");
            // 
            // ScriptEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.syntaxEditor);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ScriptEditorControl";
            this.Size = new System.Drawing.Size(992, 707);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesPopup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private SyntaxEditorControl syntaxEditor;
		private Alsing.Windows.Forms.Document.SyntaxDocument syntaxDocument;
		private DevExpress.XtraEditors.XtraOpenFileDialog dlgOpen;
		private DevExpress.XtraEditors.XtraSaveFileDialog dlgSave;
		private DevExpress.XtraBars.PopupMenu popupMenu;
		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private DevExpress.XtraBars.BarButtonItem barCut;
		private DevExpress.XtraBars.BarButtonItem barCopy;
		private DevExpress.XtraBars.BarButtonItem barPaste;
		private DevExpress.XtraBars.BarButtonItem barToggleBookmark;
		private DevExpress.Utils.SvgImageCollection imagesPopup;
		private DevExpress.XtraBars.BarButtonItem barUndo;
		private DevExpress.XtraBars.BarButtonItem barRedo;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraBars.BarStaticItem barCursor;
        private DevExpress.XtraBars.BarStaticItem barSelection;
    }
}
