using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public partial class ScriptHost
    {
        public string MapPath(string path) =>
            Project.Current.MapPath(path);
    }
}
