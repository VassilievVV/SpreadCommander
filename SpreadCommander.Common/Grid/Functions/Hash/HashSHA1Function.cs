using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Grid.Functions.Hash
{
    public class HashSHA1Function : BaseHashFunction
    {
        public const string FunctionName = "SHA1";
        static readonly HashSHA1Function instance = new HashSHA1Function();

        public HashSHA1Function()
        {
            _Hash = SHA1.Create();
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
        public override string Description => "Calculates SHA1 hash of the input object.";
    }
}
