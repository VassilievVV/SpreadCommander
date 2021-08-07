using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Code
{
    public static class UIUtils
    {
        public static void ConfigureRibbonBar(RibbonControl ribbon)
        {
            ribbon.AutoHideEmptyItems = true;
            ribbon.AutoSizeItems      = true;
            ribbon.CommandLayout      = (CommandLayout)(int)ApplicationSettings.Default.RibbonCommandLayout;
            ribbon.PopupShowMode      = PopupShowMode.Classic;
            ribbon.RibbonStyle        = RibbonControlStyle.Office2019;
            ribbon.ShowSearchItem     = true;
        }
        
        public static bool IsSkinVector(string skinName)
        {
            if (skinName == SkinStyle.Basic || skinName == SkinStyle.Bezier || skinName == SkinStyle.Office2019Colorful || skinName == SkinStyle.HighContrast)
                return true;
            return false;
        }
    }
}
