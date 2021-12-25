using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class ColorizerOptions
    {
        [Description("Layer's name to which add map item.")]
        public string Layer { get; set; }
    }
}
