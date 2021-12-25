using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public enum PolygonGradientMode
    {
        TopToBottom          = 0,
        BottomToTop          = 1,
        LeftToRight          = 2,
        RightToLeft          = 3,
        TopLeftToBottomRight = 4,
        BottomRightToTopLeft = 5,
        TopRightToBottomLeft = 6,
        BottomLeftToTopRight = 7,
        ToCenter             = 8,
        FromCenter           = 9
    }
}
