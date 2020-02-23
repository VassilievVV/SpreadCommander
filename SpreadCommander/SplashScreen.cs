using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraSplashScreen;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander
{
    public partial class SplashScreen : DevExpress.XtraSplashScreen.SplashScreen
    {
        public SplashScreen()
        {
            InitializeComponent();

            UpdateColors();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void UpdateColors()
        {
            var skin      = CommonSkins.GetSkin(UserLookAndFeel.Default);
            var backColor = skin.TranslateColor(SystemColors.Window);

            bool isSkinDark = backColor.IsDark();

            labelSpreadCommander.ForeColor = isSkinDark ? Color.MidnightBlue : Color.Ivory;
        }
    }
}