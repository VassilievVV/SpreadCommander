using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Host
{
    public partial class ScriptHost
    {
#pragma warning disable CA1822 // Mark members as static
        public string MapSCPath(string path)
#pragma warning restore CA1822 // Mark members as static
        {
            var result = Project.Current.MapPath(path);
            return result;
        }

#pragma warning disable CA1822 // Mark members as static
        public string CreateMappedSCPath(string path)
#pragma warning restore CA1822 // Mark members as static
        {
            var result = Project.Current.CreateMappedPath(path);
            return result;
        }
    }
}
