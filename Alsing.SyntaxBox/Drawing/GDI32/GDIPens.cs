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
using System.Drawing;

namespace Alsing.Windows.Forms.Drawing.GDI32
{
    public class GDIPen : GDIObject
    {
        public IntPtr hPen;

        public GDIPen(Color color, int width)
        {
            hPen = NativeMethods.CreatePen(0, width, NativeMethods.ColorToInt(color));
            Create();
        }

        ~GDIPen()
        {
            Destroy();
        }

        //VVV
        public override void Dispose()
        {
            base.Dispose();
            Destroy();
        }

        protected override void Destroy()
        {
            if (hPen != (IntPtr) 0)
                NativeMethods.DeleteObject(hPen);
            base.Destroy();
            hPen = (IntPtr) 0;
        }
    }
}