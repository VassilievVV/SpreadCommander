using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class MapOptions
    {
        [Description("Coordinate system of a map.")]
        public MapCoordinateSystem CoordinateSystem { get; set; }

        [Description("Projection used by the map data provider.")]
        public MapProjection? Projection { get; set; }

        [Description("List of images to use in map item. All images shall have same size.")]
        public string[] ImageList { get; set; }

        [Description("Size of images in ImageList.")]
        public Size? ImageSize { get; set; }

        [Description("Background color of the map.")]
        public string BackColor { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }
    }
}
