using SpreadCommander.Common.Grid.Functions.Hash;
using SpreadCommander.Common.Grid.Functions.Path;
using SpreadCommander.Common.Grid.Functions.Regex;
using SpreadCommander.Common.Grid.Functions.String;
using SpreadCommander.Common.Grid.Functions.Convert;
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
            QuoteStringFunction.Register();
            
            PathChangeExtensionFunction.Register();
            PathCombineFunction.Register();
            PathGetDirectoryNameFunction.Register();
            PathGetExtensionFunction.Register();
            PathGetFileNameFunction.Register();
            PathGetFileNameWithoutExtensionFunction.Register();

            HashMD5Function.Register();
            HashSHA1Function.Register();
            HashSHA256Function.Register();
            HashSHA384Function.Register();
            HashSHA512Function.Register();

            Base64DecodeFunction.Register();
            Base64EncodeFunction.Register();
        }
        
        public static void UnregisterFunctions()
        {
            RegexIsMatchFunction.Unregister();
            RegexMatchFunction.Unregister();
            RegexNamedMatchFunction.Unregister();
            RegexReplaceFunction.Unregister();
            
            StringFormatFunction.Unregister();
            QuoteStringFunction.Unregister();
            
            PathChangeExtensionFunction.Unregister();
            PathCombineFunction.Unregister();
            PathGetDirectoryNameFunction.Unregister();
            PathGetExtensionFunction.Unregister();
            PathGetFileNameFunction.Unregister();
            PathGetFileNameWithoutExtensionFunction.Unregister();

            HashMD5Function.Unregister();
            HashSHA1Function.Unregister();
            HashSHA256Function.Unregister();
            HashSHA384Function.Unregister();
            HashSHA512Function.Unregister();

            Base64DecodeFunction.Unregister();
            Base64EncodeFunction.Unregister();
        }
    }
}
