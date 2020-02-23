using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Grid.Functions
{
    public static class GridFunctionFactory
    {
        public static void RegisterFunctions()
        {
            RegexIsMatchFunction.Register();
            RegexMatchFunction.Register();
            RegexNamedMatchFunction.Register();
            RegexReplaceFunction.Register();
            StringFormatFunction.Register();
            PathChangeExtensionFunction.Register();
            PathCombineFunction.Register();
            PathGetDirectoryNameFunction.Register();
            PathGetExtensionFunction.Register();
            PathGetFileNameFunction.Register();
            PathGetFileNameWithoutExtensionFunction.Register();
        }
        
        public static void UnregisterFunctions()
        {
            RegexIsMatchFunction.Unregister();
            RegexMatchFunction.Unregister();
            RegexNamedMatchFunction.Unregister();
            RegexReplaceFunction.Unregister();
            StringFormatFunction.Unregister();
            PathChangeExtensionFunction.Unregister();
            PathCombineFunction.Unregister();
            PathGetDirectoryNameFunction.Unregister();
            PathGetExtensionFunction.Unregister();
            PathGetFileNameFunction.Unregister();
            PathGetFileNameWithoutExtensionFunction.Unregister();
        }
    }
}
