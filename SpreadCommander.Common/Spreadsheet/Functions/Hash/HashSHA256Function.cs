using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions.Hash
{
    public class HashSHA256Function: BaseHashFunction
    {
        public HashSHA256Function()
        {
            _Hash = SHA256.Create();
        }

        public override string Name        => "HASH.SHA256";
        public override string Description => "Calculates SHA256 hash of the string.";
    }
}
