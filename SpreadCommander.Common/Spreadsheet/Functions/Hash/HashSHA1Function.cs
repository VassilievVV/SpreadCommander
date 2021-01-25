using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions.Hash
{
    public class HashSHA1Function: BaseHashFunction
    {
        public HashSHA1Function()
        {
            _Hash = SHA1.Create();
        }

        public override string Name        => "HASH.SHA1";
        public override string Description => "Calculates SHA1 hash of the string.";
    }
}
