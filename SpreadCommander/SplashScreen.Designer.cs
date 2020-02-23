namespace SpreadCommander
{
    partial class SplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.progressBarControl = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelCopyright = new DevExpress.XtraEditors.LabelControl();
            this.labelStatus = new DevExpress.XtraEditors.LabelControl();
            this.peDevExpressLogo = new DevExpress.XtraEditors.PictureEdit();
            this.labelSpreadCommander = new DevExpress.XtraEditors.LabelControl();
            this.peLogo = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.peDevExpressLogo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.peLogo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarControl
            // 
            this.progressBarControl.EditValue = 0;
            this.progressBarControl.Location = new System.Drawing.Point(12, 227);
            this.progressBarControl.Name = "progressBarControl";
            this.progressBarControl.Size = new System.Drawing.Size(426, 12);
            this.progressBarControl.TabIndex = 5;
            // 
            // labelCopyright
            // 
            this.labelCopyright.AllowHtmlString = true;
            this.labelCopyright.Appearance.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCopyright.Appearance.Options.UseFont = true;
            this.labelCopyright.Appearance.Options.UseTextOptions = true;
            this.labelCopyright.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelCopyright.AppearanceDisabled.Options.UseTextOptions = true;
            this.labelCopyright.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelCopyright.AppearanceHovered.Options.UseTextOptions = true;
            this.labelCopyright.AppearanceHovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelCopyright.AppearancePressed.Options.UseTextOptions = true;
            this.labelCopyright.AppearancePressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelCopyright.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelCopyright.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelCopyright.Location = new System.Drawing.Point(80, 262);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(195, 46);
            this.labelCopyright.TabIndex = 6;
            this.labelCopyright.Text = "<b>Viatcheslav V. Vassiliev</b><br><i>Since © 2019</i>";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(23, 206);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(50, 13);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Starting...";
            // 
            // peDevExpressLogo
            // 
            this.peDevExpressLogo.EditValue = ((object)(resources.GetObject("peDevExpressLogo.EditValue")));
            this.peDevExpressLogo.Location = new System.Drawing.Point(278, 266);
            this.peDevExpressLogo.Name = "peDevExpressLogo";
            this.peDevExpressLogo.Properties.AllowFocused = false;
            this.peDevExpressLogo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.peDevExpressLogo.Properties.Appearance.Options.UseBackColor = true;
            this.peDevExpressLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.peDevExpressLogo.Properties.ShowMenu = false;
            this.peDevExpressLogo.Size = new System.Drawing.Size(160, 48);
            this.peDevExpressLogo.TabIndex = 8;
            // 
            // labelSpreadCommander
            // 
            this.labelSpreadCommander.Appearance.BackColor = System.Drawing.Color.RoyalBlue;
            this.labelSpreadCommander.Appearance.BackColor2 = System.Drawing.Color.Azure;
            this.labelSpreadCommander.Appearance.BorderColor = System.Drawing.Color.Black;
            this.labelSpreadCommander.Appearance.Font = new System.Drawing.Font("Times New Roman", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSpreadCommander.Appearance.ForeColor = System.Drawing.Color.Ivory;
            this.labelSpreadCommander.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.labelSpreadCommander.Appearance.Options.UseBackColor = true;
            this.labelSpreadCommander.Appearance.Options.UseBorderColor = true;
            this.labelSpreadCommander.Appearance.Options.UseFont = true;
            this.labelSpreadCommander.Appearance.Options.UseForeColor = true;
            this.labelSpreadCommander.Appearance.Options.UseTextOptions = true;
            this.labelSpreadCommander.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelSpreadCommander.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelSpreadCommander.Location = new System.Drawing.Point(12, 12);
            this.labelSpreadCommander.Name = "labelSpreadCommander";
            this.labelSpreadCommander.Size = new System.Drawing.Size(426, 180);
            this.labelSpreadCommander.TabIndex = 10;
            this.labelSpreadCommander.Text = "Spread\r\nCommander";
            // 
            // peLogo
            // 
            this.peLogo.EditValue = ((object)(resources.GetObject("peLogo.EditValue")));
            this.peLogo.Location = new System.Drawing.Point(13, 249);
            this.peLogo.Name = "peLogo";
            this.peLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.peLogo.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.peLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.peLogo.Size = new System.Drawing.Size(64, 64);
            this.peLogo.TabIndex = 11;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 320);
            this.Controls.Add(this.peLogo);
            this.Controls.Add(this.labelSpreadCommander);
            this.Controls.Add(this.peDevExpressLogo);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.progressBarControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SplashScreen";
            this.Text = "Spread Commander";
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.peDevExpressLogo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.peLogo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl progressBarControl;
        private DevExpress.XtraEditors.LabelControl labelCopyright;
        private DevExpress.XtraEditors.LabelControl labelStatus;
        private DevExpress.XtraEditors.PictureEdit peDevExpressLogo;
        private DevExpress.XtraEditors.LabelControl labelSpreadCommander;
        private DevExpress.XtraEditors.PictureEdit peLogo;
    }
}
