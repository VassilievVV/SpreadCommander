using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class PushpinOptions: PointerOptions
    {
        [Description("Glow color of a text for a map custom element.")]
        public string TextGlowColor { get; set; }

        [Description("Exact XY position where you wish the text to be drawn on the map pushpin.")]
        public int[] TextOrigin { get; set; }


        protected internal override void ConfigureMapItem(SCMap map, MapItem item)
        {
            base.ConfigureMapItem(map, item);

            var mapItem = item as MapPushpin ?? throw new Exception("Map item must be MapPushpin.");

            var textGlowColor = Utils.ColorFromString(TextGlowColor);
            if (textGlowColor != Color.Empty)
                mapItem.TextGlowColor = textGlowColor;

            if (TextOrigin != null && TextOrigin.Length == 2)
                mapItem.TextOrigin = new Point(TextOrigin[0], TextOrigin[1]);
            else if (TextOrigin != null)
                throw new Exception("Invalid text origin. Text origin shall be an array with 2 integer values.");
        }
    }

    public partial class SCMap
    {
        public SCMap AddPushpin(double[] location, string text, PushpinOptions options = null)
        {
            options ??= new PushpinOptions();
            var mapItem = new MapPushpin();
            options.ConfigurePointerItem(this, mapItem, location, text);

            return this;
        }
    }
}
