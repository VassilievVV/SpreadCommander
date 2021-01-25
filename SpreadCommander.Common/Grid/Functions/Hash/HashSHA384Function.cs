using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Grid.Functions.Hash
{
    public class HashSHA384Function : BaseHashFunction
    {
        public const string FunctionName = "SHA384";
        static readonly HashSHA384Function instance = new HashSHA384Function();

        public HashSHA384Function()
        {
            _Hash = SHA384.Create();
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
        public override string Description => "Calculates SHA384 hash of the input object.";
    }
}
