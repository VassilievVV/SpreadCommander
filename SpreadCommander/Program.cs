using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using System.IO;
using System.Runtime;
using DevExpress.XtraEditors;
using DevExpress.LookAndFeel;
using SpreadCommander.Documents.Dialogs;
using SpreadCommander.Common;
using DevExpress.Utils;
using System.Threading;
using System.Globalization;
using DevExpress.XtraReports.Security;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Code;
using DevExpress.Utils.DPI;

namespace SpreadCommander
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo.DefaultThreadCurrentCulture   = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);

            Application.ThreadException                += ExceptionHandler.OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler.CurrentDomain_UnhandledException;

            StartProfiling();

            WindowsFormsSettings.SetDPIAware();
            DpiAwarenessHelper.Default.SetDpiAware(DpiAwarenessKind.PerMonitorV2);

            /*
            //Do not force DirectX on Windows 7
            if (Environment.OSVersion.Version.Major >= 10)
                WindowsFormsSettings.ForceDirectXPaint();
            else
                WindowsFormsSettings.ForceGDIPlusPaint();
            */

            Startup.Initialize();            
            SetupDefaultValues();

            BonusSkins.Register();
            SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.WinterJoy).Assembly);
            SkinManager.EnableFormSkins();

            MainView = new MainView();
            Parameters.MainForm = MainView;
            Application.Run(MainView);
        }

        public static Form MainView { get; set; }

        public static void StartProfiling()
        {
            Utils.InitiateProfiling();
            Utils.StartProfile("Main");
        }

        private static void SetupDefaultValues()
        {
            WindowsFormsSettings.AllowArrowDragIndicators            = true;
            WindowsFormsSettings.AllowAutoFilterConditionChange      = DefaultBoolean.True;
            WindowsFormsSettings.AllowAutoScale                      = DefaultBoolean.True;
            WindowsFormsSettings.AllowDefaultSvgImages               = DefaultBoolean.True;
            WindowsFormsSettings.AllowDpiScale                       = true;
            WindowsFormsSettings.AllowHoverAnimation                 = DefaultBoolean.True;
            WindowsFormsSettings.AllowOverpanApplicationWindow       = DefaultBoolean.True;
            WindowsFormsSettings.AllowPixelScrolling                 = DefaultBoolean.True;
            WindowsFormsSettings.AnimationMode                       = AnimationMode.Default;
            WindowsFormsSettings.CustomizationFormSnapMode           = DevExpress.Utils.Controls.SnapMode.OwnerControl;
            WindowsFormsSettings.DockingViewStyle                    = DevExpress.XtraBars.Docking2010.Views.DockingViewStyle.Classic;
            WindowsFormsSettings.FilterCriteriaDisplayStyle          = FilterCriteriaDisplayStyle.Visual;
            WindowsFormsSettings.FindPanelBehavior                   = FindPanelBehavior.Search;
            WindowsFormsSettings.FocusRectStyle                      = DevExpress.Utils.Paint.DXDashStyle.Solid;
            WindowsFormsSettings.FontBehavior                        = WindowsFormsFontBehavior.UseSegoeUI;
            //WindowsFormsSettings.FormThickBorder                   = true;
            //WindowsFormsSettings.MdiFormThickBorder                = true;
            WindowsFormsSettings.PopupAnimation                      = PopupAnimation.Office2016;
            WindowsFormsSettings.PopupMenuStyle                      = DevExpress.XtraEditors.Controls.PopupMenuStyle.Classic;
            WindowsFormsSettings.PopupShadowStyle                    = PopupShadowStyle.Office2016;
            WindowsFormsSettings.ScrollUIMode                        = ScrollUIMode.Default;
            WindowsFormsSettings.SvgImageRenderingMode               = DevExpress.Utils.Svg.SvgImageRenderingMode.HighQuality;
            WindowsFormsSettings.UseAdvancedFilterEditorControl      = DefaultBoolean.True;
            WindowsFormsSettings.UseDXDialogs                        = DefaultBoolean.True;
            WindowsFormsSettings.CompactUIMode                       = DefaultBoolean.True;
            WindowsFormsSettings.DefaultRibbonStyle                  = DefaultRibbonControlStyle.Office2019;
            WindowsFormsSettings.UseAdvancedTextEdit                 = DefaultBoolean.True;
            WindowsFormsSettings.OptimizeRemoteConnectionPerformance = ApplicationSettings.Default.OptimizeRemoteConnectionPerformance ?? SystemInformation.TerminalServerSession ? DefaultBoolean.True : DefaultBoolean.False;

            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Deny);

            DevExpress.XtraPrinting.Native.PrintingSettings.UseGdiPlusLineBreakAlgorithm = true;
        }
    }
}
