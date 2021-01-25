using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Grid.Functions.Hash
{
    public class HashMD5Function: BaseHashFunction
    {
        public const string FunctionName = "MD5";
        static readonly HashMD5Function instance = new HashMD5Function();

        public HashMD5Function()
        {
            _Hash = MD5.Create();
        }

        public static void Register()
        {
            CriteriaOperator.RegisterCustomFunction(instance);
        }
        public static bool Unregister()
        {
            bool result = CriteriaOperator.UnregisterCustomFunction(instance);
            instance?.Dispose();
            return result;
        }

        public override string Name        => FunctionName;
        public override string Description => "Calculates MD5 hash of the input object.";
    }
}
