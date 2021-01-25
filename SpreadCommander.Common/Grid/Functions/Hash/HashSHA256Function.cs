using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Grid.Functions.Hash
{
    public class HashSHA256Function : BaseHashFunction
    {
        public const string FunctionName = "SHA256";
        static readonly HashSHA256Function instance = new HashSHA256Function();

        public HashSHA256Function()
        {
            _Hash = SHA256.Create();
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
        public override string Description => "Calculates SHA256 hash of the input object.";
    }
}
