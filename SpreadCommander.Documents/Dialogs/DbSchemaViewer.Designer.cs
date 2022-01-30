namespace SpreadCommander.Documents.Dialogs
{
    partial class DbSchemaViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbSchemaViewer));
            this.SplitContainer = new DevExpress.XtraEditors.SplitContainerControl();
            this.listSchemas = new DevExpress.XtraEditors.ListBoxControl();
            this.gridSchema = new SpreadCommander.Documents.Viewers.GridViewer();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listSchemas)).BeginInit();
            this.SuspendLayout();
            // 
            // SplitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Panel1.Controls.Add(this.listSchemas);
            this.SplitContainer.Panel1.Text = "Panel1";
            this.SplitContainer.Panel2.Controls.Add(this.gridSchema);
            this.SplitContainer.Panel2.Text = "Panel2";
            this.SplitContainer.Size = new System.Drawing.Size(943, 622);
            this.SplitContainer.SplitterPosition = 200;
            this.SplitContainer.TabIndex = 0;
            // 
            // listSchemas
            // 
            this.listSchemas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSchemas.Location = new System.Drawing.Point(0, 0);
            this.listSchemas.Name = "listSchemas";
            this.listSchemas.Size = new System.Drawing.Size(200, 622);
            this.listSchemas.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.listSchemas.TabIndex = 0;
            this.listSchemas.SelectedIndexChanged += new System.EventHandler(this.ListSchemas_SelectedIndexChanged);
            // 
            // gridSchema
            // 
            this.gridSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSchema.Location = new System.Drawing.Point(0, 0);
            this.gridSchema.Name = "gridSchema";
            this.gridSchema.Size = new System.Drawing.Size(733, 622);
            this.gridSchema.TabIndex = 0;
            // 
            // DbSchemaViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 622);
            this.Controls.Add(this.SplitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "DbSchemaViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.SchemaViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listSchemas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl SplitContainer;
        private DevExpress.XtraEditors.ListBoxControl listSchemas;
        private SpreadCommander.Documents.Viewers.GridViewer gridSchema;
    }
}