namespace SpreadCommander.Documents.Console
{
	partial class ConsoleInputControl
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
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue2 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleInputControl));
            DevExpress.Utils.Animation.Transition transition1 = new DevExpress.Utils.Animation.Transition();
            this.colMessageType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.memoMessageDescription = new DevExpress.XtraEditors.MemoEdit();
            this.bindingSqlMessages = new System.Windows.Forms.BindingSource(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imagesPopup = new DevExpress.Utils.SvgImageCollection(this.components);
            this.barCut = new DevExpress.XtraBars.BarButtonItem();
            this.barCopy = new DevExpress.XtraBars.BarButtonItem();
            this.barPaste = new DevExpress.XtraBars.BarButtonItem();
            this.barToggleBookmark = new DevExpress.XtraBars.BarButtonItem();
            this.barUndo = new DevExpress.XtraBars.BarButtonItem();
            this.barRedo = new DevExpress.XtraBars.BarButtonItem();
            this.editCommand = new SpreadCommander.Documents.Controls.SyntaxEditorControl();
            this.syntaxDocument = new Alsing.Windows.Forms.Document.SyntaxDocument(this.components);
            this.popupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.gridMessages = new DevExpress.XtraGrid.GridControl();
            this.viewMessages = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colLine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colErrorCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHelpLink = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHResult = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSource = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMessage1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInnerExceptionMessage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTimeStamp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridErrors = new DevExpress.XtraGrid.GridControl();
            this.bindingErrors = new System.Windows.Forms.BindingSource(this.components);
            this.viewErrors = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMessage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIncompleteInput = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colErrorID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.popupCommandHistory = new DevExpress.XtraBars.PopupControlContainer(this.components);
            this.splitContainer = new DevExpress.XtraEditors.SplitContainerControl();
            this.editHistory = new SpreadCommander.Documents.Controls.SyntaxEditorControl();
            this.syntaxDocumentHistory = new Alsing.Windows.Forms.Document.SyntaxDocument(this.components);
            this.gridHistory = new DevExpress.XtraGrid.GridControl();
            this.bindingHistory = new System.Windows.Forms.BindingSource(this.components);
            this.viewHistory = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemRichTextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.btnHistory = new DevExpress.XtraEditors.DropDownButton();
            this.btnExecute = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroupRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.tabbedControlGroupControls = new DevExpress.XtraLayout.TabbedControlGroup();
            this.layoutControlGroupMessages = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlMessages = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlMessageDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterMessageDescription = new DevExpress.XtraLayout.SplitterItem();
            this.layoutControlGroupCommand = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutCommand = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutExecute = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlHistory = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupErrors = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlErrors = new DevExpress.XtraLayout.LayoutControlItem();
            this.styleController = new DevExpress.XtraEditors.StyleController(this.components);
            this.transitionManager = new DevExpress.Utils.Animation.TransitionManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoMessageDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSqlMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupCommandHistory)).BeginInit();
            this.popupCommandHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroupControls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMessageDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterMessageDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutExecute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).BeginInit();
            this.SuspendLayout();
            // 
            // colMessageType
            // 
            this.colMessageType.Caption = "Type";
            this.colMessageType.FieldName = "MessageType";
            this.colMessageType.Name = "colMessageType";
            this.colMessageType.Visible = true;
            this.colMessageType.VisibleIndex = 0;
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.memoMessageDescription);
            this.layoutControl.Controls.Add(this.gridMessages);
            this.layoutControl.Controls.Add(this.gridErrors);
            this.layoutControl.Controls.Add(this.popupCommandHistory);
            this.layoutControl.Controls.Add(this.btnHistory);
            this.layoutControl.Controls.Add(this.btnExecute);
            this.layoutControl.Controls.Add(this.editCommand);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2577, 721, 650, 400);
            this.layoutControl.Root = this.layoutControlGroupRoot;
            this.layoutControl.Size = new System.Drawing.Size(680, 420);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl1";
            // 
            // memoMessageDescription
            // 
            this.memoMessageDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSqlMessages, "Description", true));
            this.memoMessageDescription.Location = new System.Drawing.Point(337, 24);
            this.memoMessageDescription.MenuManager = this.barManager;
            this.memoMessageDescription.Name = "memoMessageDescription";
            this.memoMessageDescription.Properties.Appearance.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.memoMessageDescription.Properties.Appearance.Options.UseFont = true;
            this.memoMessageDescription.Properties.ReadOnly = true;
            this.memoMessageDescription.Properties.UseReadOnlyAppearance = false;
            this.memoMessageDescription.Size = new System.Drawing.Size(319, 372);
            this.memoMessageDescription.StyleController = this.layoutControl;
            this.memoMessageDescription.TabIndex = 13;
            // 
            // bindingSqlMessages
            // 
            this.bindingSqlMessages.DataSource = typeof(SpreadCommander.Common.SqlScript.SqlMessage);
            // 
            // barManager
            // 
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
            this.barRedo});
            this.barManager.MaxItemId = 6;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(680, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 420);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(680, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 420);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(680, 0);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 420);
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
            // editCommand
            // 
            this.editCommand.ActiveView = Alsing.Windows.Forms.ActiveView.BottomRight;
            this.editCommand.AutoListPosition = null;
            this.editCommand.AutoListSelectedText = "a123";
            this.editCommand.AutoListVisible = false;
            this.editCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editCommand.BorderStyle = Alsing.Windows.Forms.Drawing.BorderStyle.None;
            this.editCommand.ChildBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editCommand.CopyAsRTF = true;
            this.editCommand.Document = this.syntaxDocument;
            this.editCommand.FontName = "Courier new";
            this.editCommand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(70)))), ((int)(((byte)(68)))));
            this.editCommand.GutterMarginBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editCommand.GutterMarginColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editCommand.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.editCommand.InactiveSelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editCommand.InactiveSelectionForeColor = System.Drawing.Color.Black;
            this.editCommand.InfoTipCount = 1;
            this.editCommand.InfoTipPosition = null;
            this.editCommand.InfoTipSelectedIndex = 1;
            this.editCommand.InfoTipVisible = false;
            this.editCommand.LineNumberBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editCommand.Location = new System.Drawing.Point(48, 24);
            this.editCommand.LockCursorUpdate = false;
            this.editCommand.MinimumSize = new System.Drawing.Size(0, 100);
            this.editCommand.Name = "editCommand";
            this.editCommand.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.barManager.SetPopupContextMenu(this.editCommand, this.popupMenu);
            this.editCommand.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.editCommand.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(70)))), ((int)(((byte)(68)))));
            this.editCommand.SeparatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editCommand.ShowScopeIndicator = false;
            this.editCommand.Size = new System.Drawing.Size(540, 372);
            this.editCommand.SmoothScroll = false;
            this.editCommand.SplitView = false;
            this.editCommand.SplitViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editCommand.SplitviewH = -4;
            this.editCommand.SplitviewV = -4;
            this.editCommand.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.editCommand.TabIndex = 5;
            this.editCommand.WhitespaceColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            // 
            // syntaxDocument
            // 
            this.syntaxDocument.Lines = new string[] {
        ""};
            this.syntaxDocument.MaxUndoBufferSize = 1000;
            this.syntaxDocument.Modified = false;
            this.syntaxDocument.UndoStep = 0;
            // 
            // popupMenu
            // 
            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barUndo, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barRedo),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCut, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barCopy),
            new DevExpress.XtraBars.LinkPersistInfo(this.barPaste),
            new DevExpress.XtraBars.LinkPersistInfo(this.barToggleBookmark, true)});
            this.popupMenu.Manager = this.barManager;
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.BeforePopup += new System.ComponentModel.CancelEventHandler(this.PopupMenu_BeforePopup);
            // 
            // gridMessages
            // 
            this.gridMessages.DataSource = this.bindingSqlMessages;
            this.gridMessages.Location = new System.Drawing.Point(48, 24);
            this.gridMessages.MainView = this.viewMessages;
            this.gridMessages.MenuManager = this.barManager;
            this.gridMessages.Name = "gridMessages";
            this.gridMessages.Size = new System.Drawing.Size(275, 372);
            this.gridMessages.TabIndex = 12;
            this.gridMessages.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewMessages});
            // 
            // viewMessages
            // 
            this.viewMessages.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMessageType,
            this.colLine,
            this.colErrorCode,
            this.colHelpLink,
            this.colHResult,
            this.colSource,
            this.colMessage1,
            this.colInnerExceptionMessage,
            this.colState,
            this.colDescription,
            this.colTimeStamp});
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Column = this.colMessageType;
            gridFormatRule1.Name = "FormatError";
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Expression;
            formatConditionRuleValue1.Expression = "[MessageType] = \'Error\'";
            formatConditionRuleValue1.PredefinedName = "Red Text";
            formatConditionRuleValue1.Value1 = "Error";
            gridFormatRule1.Rule = formatConditionRuleValue1;
            gridFormatRule2.ApplyToRow = true;
            gridFormatRule2.Column = this.colMessageType;
            gridFormatRule2.Name = "FormatLog";
            formatConditionRuleValue2.Condition = DevExpress.XtraEditors.FormatCondition.Expression;
            formatConditionRuleValue2.Expression = "[MessageType] = \'Log\'";
            formatConditionRuleValue2.PredefinedName = "Italic Text";
            formatConditionRuleValue2.Value1 = "Log";
            gridFormatRule2.Rule = formatConditionRuleValue2;
            this.viewMessages.FormatRules.Add(gridFormatRule1);
            this.viewMessages.FormatRules.Add(gridFormatRule2);
            this.viewMessages.GridControl = this.gridMessages;
            this.viewMessages.Name = "viewMessages";
            this.viewMessages.OptionsBehavior.Editable = false;
            this.viewMessages.OptionsBehavior.ReadOnly = true;
            this.viewMessages.OptionsFind.SearchInPreview = true;
            this.viewMessages.OptionsView.ShowGroupPanel = false;
            this.viewMessages.OptionsView.ShowPreview = true;
            this.viewMessages.PreviewFieldName = "ShortDescription";
            this.viewMessages.PreviewLineCount = 1;
            this.viewMessages.CustomColumnGroup += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewMessages_CustomColumnGroup);
            this.viewMessages.CustomColumnSort += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewMessages_CustomColumnSort);
            // 
            // colLine
            // 
            this.colLine.FieldName = "Line";
            this.colLine.Name = "colLine";
            this.colLine.Visible = true;
            this.colLine.VisibleIndex = 1;
            // 
            // colErrorCode
            // 
            this.colErrorCode.Caption = "Code";
            this.colErrorCode.FieldName = "ErrorCode";
            this.colErrorCode.Name = "colErrorCode";
            this.colErrorCode.Visible = true;
            this.colErrorCode.VisibleIndex = 2;
            // 
            // colHelpLink
            // 
            this.colHelpLink.FieldName = "HelpLink";
            this.colHelpLink.Name = "colHelpLink";
            // 
            // colHResult
            // 
            this.colHResult.FieldName = "HResult";
            this.colHResult.Name = "colHResult";
            // 
            // colSource
            // 
            this.colSource.FieldName = "Source";
            this.colSource.Name = "colSource";
            this.colSource.Visible = true;
            this.colSource.VisibleIndex = 3;
            // 
            // colMessage1
            // 
            this.colMessage1.FieldName = "Message";
            this.colMessage1.Name = "colMessage1";
            // 
            // colInnerExceptionMessage
            // 
            this.colInnerExceptionMessage.FieldName = "InnerExceptionMessage";
            this.colInnerExceptionMessage.Name = "colInnerExceptionMessage";
            // 
            // colState
            // 
            this.colState.FieldName = "State";
            this.colState.Name = "colState";
            this.colState.Visible = true;
            this.colState.VisibleIndex = 4;
            // 
            // colDescription
            // 
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.ReadOnly = true;
            // 
            // colTimeStamp
            // 
            this.colTimeStamp.DisplayFormat.FormatString = "HH:mm:ss.fff";
            this.colTimeStamp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colTimeStamp.FieldName = "TimeStamp";
            this.colTimeStamp.Name = "colTimeStamp";
            this.colTimeStamp.Visible = true;
            this.colTimeStamp.VisibleIndex = 5;
            // 
            // gridErrors
            // 
            this.gridErrors.DataSource = this.bindingErrors;
            this.gridErrors.Location = new System.Drawing.Point(48, 24);
            this.gridErrors.MainView = this.viewErrors;
            this.gridErrors.MenuManager = this.barManager;
            this.gridErrors.Name = "gridErrors";
            this.gridErrors.Size = new System.Drawing.Size(608, 372);
            this.gridErrors.TabIndex = 11;
            this.gridErrors.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewErrors});
            // 
            // bindingErrors
            // 
            this.bindingErrors.DataSource = typeof(SpreadCommander.Common.Code.ScriptParseError);
            // 
            // viewErrors
            // 
            this.viewErrors.AutoFillColumn = this.colMessage;
            this.viewErrors.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIncompleteInput,
            this.colErrorID,
            this.colMessage,
            this.colLocation});
            this.viewErrors.GridControl = this.gridErrors;
            this.viewErrors.Name = "viewErrors";
            this.viewErrors.OptionsBehavior.Editable = false;
            this.viewErrors.OptionsBehavior.ReadOnly = true;
            this.viewErrors.OptionsView.ShowGroupPanel = false;
            this.viewErrors.DoubleClick += new System.EventHandler(this.ViewErrors_DoubleClick);
            this.viewErrors.CustomColumnGroup += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewErrors_CustomColumnGroup);
            this.viewErrors.CustomColumnSort += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewErrors_CustomColumnSort);

            // 
            // colMessage
            // 
            this.colMessage.FieldName = "Message";
            this.colMessage.Name = "colMessage";
            this.colMessage.Visible = true;
            this.colMessage.VisibleIndex = 1;
            this.colMessage.Width = 231;
            // 
            // colIncompleteInput
            // 
            this.colIncompleteInput.Caption = " ";
            this.colIncompleteInput.FieldName = "IncompleteInput";
            this.colIncompleteInput.MaxWidth = 24;
            this.colIncompleteInput.Name = "colIncompleteInput";
            this.colIncompleteInput.Width = 24;
            // 
            // colErrorID
            // 
            this.colErrorID.FieldName = "ErrorID";
            this.colErrorID.Name = "colErrorID";
            this.colErrorID.Visible = true;
            this.colErrorID.VisibleIndex = 0;
            this.colErrorID.Width = 50;
            // 
            // colLocation
            // 
            this.colLocation.FieldName = "Location";
            this.colLocation.Name = "colLocation";
            this.colLocation.OptionsColumn.ReadOnly = true;
            this.colLocation.Visible = true;
            this.colLocation.VisibleIndex = 2;
            this.colLocation.Width = 50;
            // 
            // popupCommandHistory
            // 
            this.popupCommandHistory.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.popupCommandHistory.Controls.Add(this.splitContainer);
            this.popupCommandHistory.Location = new System.Drawing.Point(76, 99);
            this.popupCommandHistory.Manager = this.barManager;
            this.popupCommandHistory.Name = "popupCommandHistory";
            this.popupCommandHistory.Size = new System.Drawing.Size(404, 321);
            this.popupCommandHistory.TabIndex = 10;
            this.popupCommandHistory.Visible = false;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Panel1.Controls.Add(this.editHistory);
            this.splitContainer.Panel1.Text = "Panel1";
            this.splitContainer.Panel2.Controls.Add(this.gridHistory);
            this.splitContainer.Panel2.Text = "Panel2";
            this.splitContainer.Size = new System.Drawing.Size(404, 321);
            this.splitContainer.SplitterPosition = 268;
            this.splitContainer.TabIndex = 0;
            // 
            // editHistory
            // 
            this.editHistory.ActiveView = Alsing.Windows.Forms.ActiveView.BottomRight;
            this.editHistory.AutoListPosition = null;
            this.editHistory.AutoListSelectedText = "a123";
            this.editHistory.AutoListVisible = false;
            this.editHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editHistory.BorderStyle = Alsing.Windows.Forms.Drawing.BorderStyle.None;
            this.editHistory.ChildBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editHistory.CopyAsRTF = true;
            this.editHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editHistory.Document = this.syntaxDocumentHistory;
            this.editHistory.FontName = "Courier new";
            this.editHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(70)))), ((int)(((byte)(68)))));
            this.editHistory.GutterMarginBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editHistory.GutterMarginColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editHistory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.editHistory.InactiveSelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editHistory.InactiveSelectionForeColor = System.Drawing.Color.Black;
            this.editHistory.InfoTipCount = 1;
            this.editHistory.InfoTipPosition = null;
            this.editHistory.InfoTipSelectedIndex = 1;
            this.editHistory.InfoTipVisible = false;
            this.editHistory.LineNumberBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editHistory.Location = new System.Drawing.Point(0, 0);
            this.editHistory.LockCursorUpdate = false;
            this.editHistory.Name = "editHistory";
            this.editHistory.OutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editHistory.ReadOnly = true;
            this.editHistory.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.editHistory.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(70)))), ((int)(((byte)(68)))));
            this.editHistory.SeparatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.editHistory.ShowScopeIndicator = false;
            this.editHistory.Size = new System.Drawing.Size(126, 321);
            this.editHistory.SmoothScroll = false;
            this.editHistory.SplitViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editHistory.SplitviewH = -4;
            this.editHistory.SplitviewV = -4;
            this.editHistory.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
            this.editHistory.TabIndex = 0;
            this.editHistory.WhitespaceColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            // 
            // syntaxDocumentHistory
            // 
            this.syntaxDocumentHistory.Lines = new string[] {
        ""};
            this.syntaxDocumentHistory.MaxUndoBufferSize = 1000;
            this.syntaxDocumentHistory.Modified = false;
            this.syntaxDocumentHistory.UndoStep = 0;
            // 
            // gridHistory
            // 
            this.gridHistory.DataSource = this.bindingHistory;
            this.gridHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridHistory.Location = new System.Drawing.Point(0, 0);
            this.gridHistory.MainView = this.viewHistory;
            this.gridHistory.Name = "gridHistory";
            this.gridHistory.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemRichTextEdit});
            this.gridHistory.Size = new System.Drawing.Size(268, 321);
            this.gridHistory.TabIndex = 0;
            this.gridHistory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewHistory});
            // 
            // bindingHistory
            // 
            this.bindingHistory.DataSource = typeof(SpreadCommander.Documents.Console.ConsoleInputControl.HistoryItem);
            this.bindingHistory.CurrentChanged += new System.EventHandler(this.BindingHistory_CurrentChanged);
            // 
            // viewHistory
            // 
            this.viewHistory.AutoFillColumn = this.colText;
            this.viewHistory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colText});
            this.viewHistory.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.viewHistory.GridControl = this.gridHistory;
            this.viewHistory.Name = "viewHistory";
            this.viewHistory.OptionsBehavior.AllowIncrementalSearch = true;
            this.viewHistory.OptionsBehavior.Editable = false;
            this.viewHistory.OptionsBehavior.ReadOnly = true;
            this.viewHistory.OptionsFind.AlwaysVisible = true;
            this.viewHistory.OptionsFind.ShowClearButton = false;
            this.viewHistory.OptionsView.ShowColumnHeaders = false;
            this.viewHistory.OptionsView.ShowGroupPanel = false;
            this.viewHistory.OptionsView.ShowIndicator = false;
            this.viewHistory.RowHeight = 80;
            this.viewHistory.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.ViewHistory_RowClick);
            this.viewHistory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ViewHistory_KeyDown);
            this.viewHistory.CustomColumnGroup += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewHistory_CustomColumnGroup);
            this.viewHistory.CustomColumnSort += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.ViewHistory_CustomColumnSort);
            // 
            // colText
            // 
            this.colText.Caption = "History";
            this.colText.FieldName = "Text";
            this.colText.Name = "colText";
            this.colText.OptionsColumn.ReadOnly = true;
            this.colText.Visible = true;
            this.colText.VisibleIndex = 0;
            // 
            // repositoryItemRichTextEdit
            // 
            this.repositoryItemRichTextEdit.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.Rtf;
            this.repositoryItemRichTextEdit.EncodingWebName = "utf-8";
            this.repositoryItemRichTextEdit.MaxHeight = 100;
            this.repositoryItemRichTextEdit.Name = "repositoryItemRichTextEdit";
            this.repositoryItemRichTextEdit.OptionsBehavior.CreateNew = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
            this.repositoryItemRichTextEdit.OptionsBehavior.Open = DevExpress.XtraRichEdit.DocumentCapability.Enabled;
            this.repositoryItemRichTextEdit.ReadOnly = true;
            this.repositoryItemRichTextEdit.ShowCaretInReadOnly = false;
            // 
            // btnHistory
            // 
            this.btnHistory.DropDownArrowStyle = DevExpress.XtraEditors.DropDownArrowStyle.Hide;
            this.btnHistory.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnHistory.ImageOptions.SvgImage = global::SpreadCommander.Documents.Properties.Resources.Up;
            this.btnHistory.ImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.btnHistory.Location = new System.Drawing.Point(592, 24);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(64, 22);
            this.btnHistory.StyleController = this.layoutControl;
            this.btnHistory.TabIndex = 7;
            this.btnHistory.Click += new System.EventHandler(this.BtnHistory_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.AutoWidthInLayoutControl = true;
            this.btnExecute.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnExecute.ImageOptions.SvgImage = global::SpreadCommander.Documents.Properties.Resources.Send;
            this.btnExecute.Location = new System.Drawing.Point(592, 50);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(64, 346);
            this.btnExecute.StyleController = this.layoutControl;
            this.btnExecute.TabIndex = 6;
            this.btnExecute.Click += new System.EventHandler(this.BtnExecute_Click);
            // 
            // layoutControlGroupRoot
            // 
            this.layoutControlGroupRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupRoot.GroupBordersVisible = false;
            this.layoutControlGroupRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.tabbedControlGroupControls});
            this.layoutControlGroupRoot.Name = "Root";
            this.layoutControlGroupRoot.Size = new System.Drawing.Size(680, 420);
            this.layoutControlGroupRoot.TextVisible = false;
            // 
            // tabbedControlGroupControls
            // 
            this.tabbedControlGroupControls.CustomizationFormText = "Controls";
            this.tabbedControlGroupControls.Location = new System.Drawing.Point(0, 0);
            this.tabbedControlGroupControls.Name = "tabbedControlGroupControls";
            this.tabbedControlGroupControls.SelectedTabPage = this.layoutControlGroupMessages;
            this.tabbedControlGroupControls.Size = new System.Drawing.Size(660, 400);
            this.tabbedControlGroupControls.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroupMessages,
            this.layoutControlGroupCommand,
            this.layoutControlGroupErrors});
            this.tabbedControlGroupControls.Text = "Controls";
            this.tabbedControlGroupControls.TextLocation = DevExpress.Utils.Locations.Left;
            this.tabbedControlGroupControls.SelectedPageChanged += new DevExpress.XtraLayout.LayoutTabPageChangedEventHandler(this.TabbedControlGroupControls_SelectedPageChanged);
            this.tabbedControlGroupControls.SelectedPageChanging += new DevExpress.XtraLayout.LayoutTabPageChangingEventHandler(this.TabbedControlGroupControls_SelectedPageChanging);
            // 
            // layoutControlGroupMessages
            // 
            this.layoutControlGroupMessages.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("layoutControlGroupMessages.CaptionImageOptions.SvgImage")));
            this.layoutControlGroupMessages.CaptionImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.layoutControlGroupMessages.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlMessages,
            this.layoutControlMessageDescription,
            this.splitterMessageDescription});
            this.layoutControlGroupMessages.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupMessages.Name = "layoutControlGroupMessages";
            this.layoutControlGroupMessages.Size = new System.Drawing.Size(612, 376);
            this.layoutControlGroupMessages.Text = "Messages";
            this.layoutControlGroupMessages.TextVisible = false;
            // 
            // layoutControlMessages
            // 
            this.layoutControlMessages.Control = this.gridMessages;
            this.layoutControlMessages.Location = new System.Drawing.Point(0, 0);
            this.layoutControlMessages.Name = "layoutControlMessages";
            this.layoutControlMessages.Size = new System.Drawing.Size(279, 376);
            this.layoutControlMessages.Text = "layoutMessages";
            this.layoutControlMessages.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlMessages.TextVisible = false;
            // 
            // layoutControlMessageDescription
            // 
            this.layoutControlMessageDescription.Control = this.memoMessageDescription;
            this.layoutControlMessageDescription.Location = new System.Drawing.Point(289, 0);
            this.layoutControlMessageDescription.Name = "layoutControlMessageDescription";
            this.layoutControlMessageDescription.Size = new System.Drawing.Size(323, 376);
            this.layoutControlMessageDescription.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlMessageDescription.TextVisible = false;
            // 
            // splitterMessageDescription
            // 
            this.splitterMessageDescription.AllowHotTrack = true;
            this.splitterMessageDescription.Location = new System.Drawing.Point(279, 0);
            this.splitterMessageDescription.Name = "splitterMessageDescription";
            this.splitterMessageDescription.Size = new System.Drawing.Size(10, 376);
            // 
            // layoutControlGroupCommand
            // 
            this.layoutControlGroupCommand.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("layoutControlGroupCommand.CaptionImageOptions.SvgImage")));
            this.layoutControlGroupCommand.CaptionImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.layoutControlGroupCommand.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutCommand,
            this.layoutExecute,
            this.layoutControlHistory});
            this.layoutControlGroupCommand.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupCommand.Name = "layoutControlGroupCommand";
            this.layoutControlGroupCommand.OptionsToolTip.ToolTip = "Commands";
            this.layoutControlGroupCommand.Size = new System.Drawing.Size(612, 376);
            this.layoutControlGroupCommand.Text = "Command";
            this.layoutControlGroupCommand.TextVisible = false;
            // 
            // layoutCommand
            // 
            this.layoutCommand.Control = this.editCommand;
            this.layoutCommand.Location = new System.Drawing.Point(0, 0);
            this.layoutCommand.Name = "layoutCommand";
            this.layoutCommand.Size = new System.Drawing.Size(544, 376);
            this.layoutCommand.Text = "Command";
            this.layoutCommand.TextSize = new System.Drawing.Size(0, 0);
            this.layoutCommand.TextVisible = false;
            // 
            // layoutExecute
            // 
            this.layoutExecute.Control = this.btnExecute;
            this.layoutExecute.Location = new System.Drawing.Point(544, 26);
            this.layoutExecute.MaxSize = new System.Drawing.Size(68, 0);
            this.layoutExecute.MinSize = new System.Drawing.Size(68, 40);
            this.layoutExecute.Name = "layoutExecute";
            this.layoutExecute.Size = new System.Drawing.Size(68, 350);
            this.layoutExecute.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutExecute.TextSize = new System.Drawing.Size(0, 0);
            this.layoutExecute.TextVisible = false;
            // 
            // layoutControlHistory
            // 
            this.layoutControlHistory.Control = this.btnHistory;
            this.layoutControlHistory.Location = new System.Drawing.Point(544, 0);
            this.layoutControlHistory.MaxSize = new System.Drawing.Size(0, 26);
            this.layoutControlHistory.MinSize = new System.Drawing.Size(26, 26);
            this.layoutControlHistory.Name = "layoutControlHistory";
            this.layoutControlHistory.Size = new System.Drawing.Size(68, 26);
            this.layoutControlHistory.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlHistory.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlHistory.TextVisible = false;
            // 
            // layoutControlGroupErrors
            // 
            this.layoutControlGroupErrors.CaptionImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("layoutControlGroupErrors.CaptionImageOptions.SvgImage")));
            this.layoutControlGroupErrors.CaptionImageOptions.SvgImageSize = new System.Drawing.Size(16, 16);
            this.layoutControlGroupErrors.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlErrors});
            this.layoutControlGroupErrors.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupErrors.Name = "layoutControlGroupErrors";
            this.layoutControlGroupErrors.OptionsToolTip.ToolTip = "Errors";
            this.layoutControlGroupErrors.Size = new System.Drawing.Size(612, 376);
            this.layoutControlGroupErrors.Text = "Errors";
            this.layoutControlGroupErrors.TextVisible = false;
            // 
            // layoutControlErrors
            // 
            this.layoutControlErrors.Control = this.gridErrors;
            this.layoutControlErrors.CustomizationFormText = "Errors";
            this.layoutControlErrors.Location = new System.Drawing.Point(0, 0);
            this.layoutControlErrors.Name = "layoutControlErrors";
            this.layoutControlErrors.Size = new System.Drawing.Size(612, 376);
            this.layoutControlErrors.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlErrors.TextVisible = false;
            // 
            // transitionManager
            // 
            transition1.BarWaitingIndicatorProperties.Caption = "";
            transition1.BarWaitingIndicatorProperties.Description = "";
            transition1.Control = this;
            transition1.LineWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.LineWaitingIndicatorProperties.Caption = "";
            transition1.LineWaitingIndicatorProperties.Description = "";
            transition1.RingWaitingIndicatorProperties.AnimationElementCount = 5;
            transition1.RingWaitingIndicatorProperties.Caption = "";
            transition1.RingWaitingIndicatorProperties.Description = "";
            transition1.WaitingIndicatorProperties.Caption = "";
            transition1.WaitingIndicatorProperties.Description = "";
            this.transitionManager.Transitions.Add(transition1);
            // 
            // ConsoleInputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "ConsoleInputControl";
            this.Size = new System.Drawing.Size(680, 420);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoMessageDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSqlMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagesPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupCommandHistory)).EndInit();
            this.popupCommandHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupRoot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroupControls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMessageDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterMessageDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutExecute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleController)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl;
		private DevExpress.XtraEditors.SimpleButton btnExecute;
		private Controls.SyntaxEditorControl editCommand;
		private Alsing.Windows.Forms.Document.SyntaxDocument syntaxDocument;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupRoot;
		private DevExpress.XtraLayout.LayoutControlItem layoutExecute;
		private DevExpress.XtraEditors.DropDownButton btnHistory;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlHistory;
		private DevExpress.XtraEditors.StyleController styleController;
		private DevExpress.XtraEditors.SplitContainerControl splitContainer;
		private Controls.SyntaxEditorControl editHistory;
		private DevExpress.XtraGrid.GridControl gridHistory;
		private DevExpress.XtraGrid.Views.Grid.GridView viewHistory;
		private System.Windows.Forms.BindingSource bindingHistory;
		private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit;
		private DevExpress.XtraGrid.Columns.GridColumn colText;
		private DevExpress.XtraBars.PopupControlContainer popupCommandHistory;
		private DevExpress.XtraBars.BarManager barManager;
		private DevExpress.XtraBars.BarDockControl barDockControlTop;
		private DevExpress.XtraBars.BarDockControl barDockControlBottom;
		private DevExpress.XtraBars.BarDockControl barDockControlLeft;
		private DevExpress.XtraBars.BarDockControl barDockControlRight;
		private Alsing.Windows.Forms.Document.SyntaxDocument syntaxDocumentHistory;
		private DevExpress.XtraGrid.GridControl gridErrors;
		private DevExpress.XtraGrid.Views.Grid.GridView viewErrors;
		private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroupControls;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupCommand;
		private DevExpress.XtraLayout.LayoutControlItem layoutCommand;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupErrors;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlErrors;
		private System.Windows.Forms.BindingSource bindingErrors;
		private DevExpress.XtraGrid.Columns.GridColumn colMessage;
		private DevExpress.XtraGrid.Columns.GridColumn colIncompleteInput;
		private DevExpress.XtraGrid.Columns.GridColumn colErrorID;
		private DevExpress.XtraGrid.Columns.GridColumn colLocation;
		private DevExpress.Utils.SvgImageCollection imagesPopup;
		private DevExpress.XtraBars.BarButtonItem barCut;
		private DevExpress.XtraBars.BarButtonItem barCopy;
		private DevExpress.XtraBars.BarButtonItem barPaste;
		private DevExpress.XtraBars.BarButtonItem barToggleBookmark;
		private DevExpress.XtraBars.BarButtonItem barUndo;
		private DevExpress.XtraBars.BarButtonItem barRedo;
		private DevExpress.XtraBars.PopupMenu popupMenu;
		private DevExpress.XtraGrid.GridControl gridMessages;
		private DevExpress.XtraGrid.Views.Grid.GridView viewMessages;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupMessages;
		private System.Windows.Forms.BindingSource bindingSqlMessages;
		private DevExpress.XtraGrid.Columns.GridColumn colMessageType;
		private DevExpress.XtraGrid.Columns.GridColumn colLine;
		private DevExpress.XtraGrid.Columns.GridColumn colErrorCode;
		private DevExpress.XtraGrid.Columns.GridColumn colHelpLink;
		private DevExpress.XtraGrid.Columns.GridColumn colHResult;
		private DevExpress.XtraGrid.Columns.GridColumn colSource;
		private DevExpress.XtraGrid.Columns.GridColumn colMessage1;
		private DevExpress.XtraGrid.Columns.GridColumn colInnerExceptionMessage;
		private DevExpress.XtraGrid.Columns.GridColumn colState;
		private DevExpress.XtraGrid.Columns.GridColumn colDescription;
		private DevExpress.XtraEditors.MemoEdit memoMessageDescription;
		private DevExpress.XtraLayout.SplitterItem splitterMessageDescription;
		private DevExpress.Utils.Animation.TransitionManager transitionManager;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlMessages;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlMessageDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colTimeStamp;
    }
}
