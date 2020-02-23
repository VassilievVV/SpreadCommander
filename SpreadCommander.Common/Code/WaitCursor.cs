using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SpreadCommander.Common.Code
{
    public class WaitCursor : IDisposable
    {
        private readonly Cursor _Cursor;

        public WaitCursor()
        {
            _Cursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        public void Dispose()
        {
            Cursor.Current = _Cursor;
        }
    }
}
