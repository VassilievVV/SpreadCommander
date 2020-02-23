using DevExpress.XtraEditors;
using SpreadCommander.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadCommander.Common.Code;
using SpreadCommander.Documents.Code;

namespace SpreadCommander.Documents.Extensions
{
    public static class DialogExtensions
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(this Control control)
        {
            SendMessage(control.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(this Control control)
        {
            SendMessage(control.Handle, WM_SETREDRAW, true, 0);
            control.Refresh();
        }

        public static void MakeXtraDialogFormResizeable(this XtraDialogForm form)
        {
            using (new UsingProcessor(() => SuspendDrawing(form), () => ResumeDrawing(form)))
            {
                foreach (SimpleButton btn in form.Controls.OfType<SimpleButton>())
                    btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                Control content      = form.Controls.OfType<Control>().Last();
                content.Anchor       = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                form.MaximizeBox     = true;
                form.ShowIcon        = false;
            }
        }

        public static void MakeXtraDialogFormResizeableWithDelay(this XtraDialogForm form)
        {
            Task.Run(() =>
            {
                Thread.Sleep(100);
                if (form.IsHandleCreated)
                    form.Invoke((MethodInvoker)(() => form.MakeXtraDialogFormResizeable()));
            });
        }

        public static void FocusWithDelay(this Control control)
        {
            Task.Run(() =>
            {
                Thread.Sleep(100);
                if (control.IsHandleCreated)
                    control.Invoke((MethodInvoker)(() => control.Focus()));
            });
        }

        public static void AdjustSizeForMonitor(this Control control)
        {
            var screen = Parameters.MainForm != null ? Screen.FromControl(Parameters.MainForm) : Screen.PrimaryScreen;

            var formState = LoadFormState();
            if (formState != null)
            {
                if (formState.Width > 0) control.Width = Utils.ValueInRange(formState.Width, 200, screen.Bounds.Width - 20);
                if (formState.Height > 0) control.Height = Utils.ValueInRange(formState.Height, 200, screen.Bounds.Height - 20);
            }

            var diffHeight = Convert.ToInt32(screen.Bounds.Height * 0.1);
            if (control.Height > screen.Bounds.Height - diffHeight)
            {
                control.Height = screen.Bounds.Height - diffHeight;
                control.Top = 0;
            }
            if (control.Width > screen.Bounds.Width - 20)
            {
                control.Width = screen.Bounds.Width - 20;
                control.Left = 0;
            }

            control.ParentChanged += ((s, e) =>
            {
                var ctrl = (Control)s;
                if (ctrl is Form)
                    return;

                if (ctrl.Parent == null)
                {
                    var settings = ApplicationVisualState.Default;
                    if (settings == null)
                        return;

                    var ctrlState = new ApplicationVisualState.FormState(ctrl.Name) { Width = ctrl.Width, Height = ctrl.Height };
                    settings.SaveFormState(ctrlState);
                    settings.SaveSettings();
                }
            });

            if (control is Form frm)
            {
                frm.FormClosing += ((s, e) =>
                {
                    var settings = ApplicationVisualState.Default;
                    if (settings == null)
                        return;

                    var ctrlState = new ApplicationVisualState.FormState(frm.Name) { Width = frm.Width, Height = frm.Height };
                    settings.SaveFormState(ctrlState);

                    settings.SaveSettings();
                });
            }


            ApplicationVisualState.FormState LoadFormState()
            {
                var settings = ApplicationVisualState.Default;
                if (settings == null)
                    return null;
                return settings.FindFormState(control.Name);
            }
        }
    }
}
