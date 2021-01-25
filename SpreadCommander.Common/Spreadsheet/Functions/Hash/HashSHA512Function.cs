using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions.Hash
{
    public class HashSHA512Function: BaseHashFunction
    {
        public HashSHA512Function()
        {
            _Hash = SHA512.Create();
        }

        public override string Name        => "HASH.SHA512";
        public override string Description => "Calculates SHA512 hash of the string.";
    }
}
