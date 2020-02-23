#pragma warning disable CRR0047

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Extensions;

namespace SpreadCommander.Documents.Dialogs
{
    public class BaseForm : DevExpress.XtraEditors.XtraForm
    {
        public static Icon ApplicationIcon;

        protected override void OnLoad(EventArgs e)
        {
            Opacity = ((double)Utils.ValueInRange(ApplicationSettings.Default.DialogOpacity, 20, 100)) / 100;
            if (ApplicationIcon != null)
                Icon = ApplicationIcon;

            this.AdjustSizeForMonitor();

            base.OnLoad(e);
        }
    }

    public class BaseRibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        protected override void OnLoad(EventArgs e)
        {
            Opacity = ((double)Utils.ValueInRange(ApplicationSettings.Default.DialogOpacity, 20, 100)) / 100;
            if (BaseForm.ApplicationIcon != null)
                Icon = BaseForm.ApplicationIcon;

            this.AdjustSizeForMonitor();

            base.OnLoad(e);
        }
    }
}
