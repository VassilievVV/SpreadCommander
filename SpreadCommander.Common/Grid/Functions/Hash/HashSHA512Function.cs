using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Grid.Functions.Hash
{
    public class HashSHA512Function : BaseHashFunction
    {
        public const string FunctionName = "SHA512";
        static readonly HashSHA512Function instance = new HashSHA512Function();

        public HashSHA512Function()
        {
            _Hash = SHA512.Create();
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
        public override string Description => "Calculates SHA512 hash of the input object.";
    }
}
