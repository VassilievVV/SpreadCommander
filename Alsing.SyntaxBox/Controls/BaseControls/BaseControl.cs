// *
// * Copyright (C) 2008 Roger Alsing : http://www.RogerAlsing.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using Alsing.Windows.Forms.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

//namespace Alsing.Windows.Forms
namespace Alsing.Windows.Forms.Controls.BaseControls
{
    [ToolboxItem(true)]
    public class BaseControl : Control
    {
        private const int WS_BORDER = unchecked(0x00800000);
        private const int WS_EX_CLIENTEDGE = unchecked(0x00000200);
        private Color borderColor = Color.Black;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Drawing.BorderStyle borderStyle;

        //private Container components;
        private bool RunOnce = true;


        public BaseControl(): base()
        {
            SetStyle(ControlStyles.EnableNotifyMessage, true);
            BorderStyle = Drawing.BorderStyle.FixedSingle;
            InitializeComponent();
        }

        [Browsable(false)]
        public Size WindowSize
        {
            get
            {
                var s = new APIRect();
                NativeMethods.GetWindowRect(Handle, ref s);
                return new Size(s.Width, s.Height);
            }
        }

        [Category("Appearance - Borders"), Description("The border color")]
        [DefaultValue(typeof (Color), "Black")]
        public Color BorderColor
        {
            get { return borderColor; }

            set
            {
                borderColor = value;
                Refresh();
                Invalidate();
                UpdateStyles();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (BorderStyle == Drawing.BorderStyle.None)
                    return cp;

                cp.ExStyle &= (~WS_EX_CLIENTEDGE);
                cp.Style &= (~WS_BORDER);

                return cp;
            }
        }

        [Browsable(true),
         EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance - Borders"), Description("The border style")]
        public Drawing.BorderStyle BorderStyle
        {
            get { return borderStyle; }
            set
            {
                if (borderStyle != value)
                {
                    if (!Enum.IsDefined(typeof (Drawing.BorderStyle), value))
                    {
                        throw new InvalidEnumArgumentException("value", (int) value, typeof (Drawing.BorderStyle));
                    }
                    borderStyle = value;
                    UpdateStyles();
                    Refresh();
                }
            }
        }

		/*
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Do not use!", true)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }
		*/


        [Browsable(false)]
        public int ClientWidth
        {
            get { return WindowSize.Width - (BorderWidth*2); }
        }

        [Browsable(false)]
        public int ClientHeight
        {
            get { return WindowSize.Height - (BorderWidth*2); }
        }

        [Browsable(false)]
        public int BorderWidth
        {
            get
            {
                switch (borderStyle)
                {
                    case Drawing.BorderStyle.None:
                        {
                            return 0;
                        }
                    case Drawing.BorderStyle.Sunken:
                        {
                            return 2;
                        }
                    case Drawing.BorderStyle.SunkenThin:
                        {
                            return 1;
                        }
                    case Drawing.BorderStyle.Raised:
                        {
                            return 2;
                        }

                    case Drawing.BorderStyle.Etched:
                        {
                            return 2;
                        }
                    case Drawing.BorderStyle.Bump:
                        {
                            return 6;
                        }
                    case Drawing.BorderStyle.FixedSingle:
                        {
                            return 1;
                        }
                    case Drawing.BorderStyle.FixedDouble:
                        {
                            return 2;
                        }
                    case Drawing.BorderStyle.RaisedThin:
                        {
                            return 1;
                        }
                    case Drawing.BorderStyle.Dotted:
                        {
                            return 1;
                        }
                    case Drawing.BorderStyle.Dashed:
                        {
                            return 1;
                        }
                }


                return Height;
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // BaseControl
            // 
            this.Size = new System.Drawing.Size(272, 264);
        }

        #endregion

        public event EventHandler Load = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				/*
                if (components != null)
                {
                    components.Dispose();
                }
				*/
            }
            base.Dispose(disposing);
        }

        protected virtual void OnLoad(EventArgs e)
        {
			Load?.Invoke(this, e);
			Refresh();
        }

        protected override unsafe void WndProc(ref Message m)
        {
            if (m.Msg == (int)WindowMessage.WM_NCPAINT)
            {
                RenderBorder();
            }
            else if (m.Msg == (int)WindowMessage.WM_SHOWWINDOW)
            {
                if (RunOnce)
                {
                    RunOnce = false;
                    OnLoad(null);
                    base.WndProc(ref m);
                    UpdateStyles();
                }
                else
                {
                    UpdateStyles();
                    base.WndProc(ref m);
                }
            }
            else if (m.Msg == (int)WindowMessage.WM_NCCREATE)
            {
                base.WndProc(ref m);
            }
            else if (m.Msg == (int)WindowMessage.WM_NCCALCSIZE)
            {
                if (m.WParam == (IntPtr) 0)
                {
                    var pRC = (APIRect*) m.LParam;
                    //pRC->left -=3;
                    base.WndProc(ref m);
                }
                else if (m.WParam == (IntPtr) 1)
                {
                    var pNCP = (_NCCALCSIZE_PARAMS*) m.LParam;


                    int t = pNCP->NewRect.top + BorderWidth;
                    int l = pNCP->NewRect.left + BorderWidth;
                    int b = pNCP->NewRect.bottom - BorderWidth;
                    int r = pNCP->NewRect.right - BorderWidth;

                    base.WndProc(ref m);

                    pNCP->NewRect.top = t;
                    pNCP->NewRect.left = l;
                    pNCP->NewRect.right = r;
                    pNCP->NewRect.bottom = b;

                    return;
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void RenderBorder()
        {
            IntPtr hdc = NativeMethods.GetWindowDC(Handle);
            var s = new APIRect();
            NativeMethods.GetWindowRect(Handle, ref s);

            using (Graphics g = Graphics.FromHdc(hdc))
            {
                Drawing.DrawingTools.DrawBorder((Drawing.BorderStyle2) (int) BorderStyle, BorderColor, g,
                                        new Rectangle(0, 0, s.Width, s.Height));
            }
            NativeMethods.ReleaseDC(Handle, hdc);
        }


        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
        }
    }
}