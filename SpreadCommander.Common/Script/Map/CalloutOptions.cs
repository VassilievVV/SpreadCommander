using DevExpress.XtraMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class CalloutOptions: PointerOptions
    {
        [Description("Whether HTML formatting is allowed")]
        public bool HtmlText { get; set; }


        protected internal override void ConfigureMapItem(SCMap map, MapItem item)
        {
            base.ConfigureMapItem(map, item);

            var mapItem = item as MapCallout ?? throw new Exception("Map item must be MapCallout.");
            mapItem.AllowHtmlText = HtmlText;
        }
    }

    public partial class SCMap
    {
        public SCMap AddCallout(double[] location, string text, CalloutOptions options = null)
        {
            options ??= new CalloutOptions();
            var mapItem = new MapCallout();
            options.ConfigurePointerItem(this, mapItem, location, text);

            return this;
        }
    }
}
