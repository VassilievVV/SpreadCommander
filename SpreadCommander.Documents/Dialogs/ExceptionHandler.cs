using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraEditors;
using SpreadCommander.Common;
using DevExpress.XtraNavBar;
using System.Reflection;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class ExceptionHandler : XtraForm
    {
        private static Form _SplashScreen;

        public ExceptionHandler(Exception ex)
        {
            InitializeComponent();
            
            barException.SelectedLink = new NavBarItemLink(navDescription);

            //Description
            UpdateMemo(memoDescription, ex != null ? ex.Message : "Unknown exception");
            
            //Details
            var output = new StringBuilder();

            while (ex != null)
            {
                output.Append($@"Message: 
--------
{ex.Message}

");
                output.Append($@"Exception class: 
----------------
{ex.GetType().FullName}

");
                output.Append($@"Source: 
-------
{ex.Source}

");
                output.Append($@"Stack trace:
-----------
{ex.StackTrace}

");

                ex = ex.InnerException;
                if (ex != null)
                    output.Append(
@"Inner exception:
----------------
");
            }

            UpdateMemo(memoDetails, output.ToString());


            //Configuration
            output.Length = 0;
            output.Append($@"Machine name: 
-------------
{Environment.MachineName}

");
            output.Append($@"OS version: 
-----------
{Environment.OSVersion}

");
            output.Append($@"User domain name: 
-----------------
{Environment.UserDomainName}

");
            output.Append($@"Working set: 
------------
{Environment.WorkingSet}

");

            UpdateMemo(memoConfig, output.ToString());
        }

        private static void UpdateMemo(MemoEdit memo, string str)
        {
            memo.Text				= Utils.NonNullString(str);
            memo.SelectionStart		= 0;
            memo.SelectionLength	= 0;
        }

        public static void HandleException(Exception ex)
        {
            if (_SplashScreen != null)
                _SplashScreen.Close();

            using var frm = new ExceptionHandler(ex);
            switch (frm.ShowDialog(null))
            {
                case DialogResult.Abort:
                    Application.Exit();
                    break;
            }
        }

        public static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!e.IsTerminating)
            {
                HandleException(e.ExceptionObject as Exception);
            }
            else
            {
                if (!Environment.HasShutdownStarted)
                {
                    if (e.ExceptionObject is Exception ex)
                    {
                        string msg =
#if (DEBUG)
                        ex != null ? $"{ex.Message}\r\n\r\n{ex.StackTrace}" : "unknown exception";
#else
                        ex != null ? ex.Message : "unknown exception";
#endif
                        MessageBox.Show(msg);
                    }
                }
            }
        }

        private void NavBar_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
        {
            if (e.Link == null)
                return;
        
            if (e.Link.Item == navDescription)
            {
                if (TabControl.SelectedTabPage != tabDescription)
                    TabControl.SelectedTabPage = tabDescription;
            }
            else if (e.Link.Item == navDetails)
            {
                if (TabControl.SelectedTabPage != tabDetails)
                    TabControl.SelectedTabPage = tabDetails;
            }
            else if (e.Link.Item == navConfig)
            {
                if (TabControl.SelectedTabPage != tabConfig)
                    TabControl.SelectedTabPage = tabConfig;
            }
        }

        private void TabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == null)
                return;
                
            if (e.Page == tabDescription)
            {
                if (barException.SelectedLink == null || barException.SelectedLink.Item != navDescription)
                    barException.SelectedLink = new NavBarItemLink(navDescription);
            }
            else if (e.Page == tabDetails)
            {
                if (barException.SelectedLink == null || barException.SelectedLink.Item != navDetails)
                    barException.SelectedLink = new NavBarItemLink(navDetails);
            }
            else if (e.Page == tabConfig)
            {
                if (barException.SelectedLink == null || barException.SelectedLink.Item != navConfig)
                    barException.SelectedLink = new NavBarItemLink(navConfig);
            }
        }

        public static Form SplashScreen
        {
            get {return _SplashScreen;}
            set
            {
                if (_SplashScreen != null)
                    _SplashScreen.FormClosed -= SplashScreen_FormClosed;

                _SplashScreen = value;

                if (_SplashScreen != null)
                    _SplashScreen.FormClosed += SplashScreen_FormClosed;
            }
        }

        private static void SplashScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            _SplashScreen = null;
        }
    }
}