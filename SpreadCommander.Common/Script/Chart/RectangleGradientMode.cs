using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum RectangleGradientMode
    {
        TopToBottom          = 0,
        BottomToTop          = 1,
        LeftToRight          = 2,
        RightToLeft          = 3,
        TopLeftToBottomRight = 4,
        BottomRightToTopLeft = 5,
        TopRightToBottomLeft = 6,
        BottomLeftToTopRight = 7,
        FromCenterHorizontal = 8,
        ToCenterHorizontal   = 9,
        FromCenterVertical   = 10,
        ToCenterVertical     = 11
    }
}
