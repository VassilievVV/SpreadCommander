using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Extensions;

namespace SpreadCommander.Documents.Dialogs
{
    public class BaseForm : DevExpress.XtraEditors.XtraForm
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static Icon ApplicationIcon;
#pragma warning restore CA2211 // Non-constant fields should not be visible

        protected override void OnLoad(EventArgs e)
        {
            Opacity = ((double)Utils.ValueInRange(ApplicationSettings.Default.DialogOpacity, 20, 100)) / 100;
            if (ApplicationIcon != null)
                Icon = ApplicationIcon;

            this.AdjustSizeForMonitor();

            base.OnLoad(e);
        }

        protected override FormShowMode ShowMode => FormShowMode.AfterInitialization;
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

        protected override FormShowMode ShowMode => FormShowMode.AfterInitialization;
    }
}
