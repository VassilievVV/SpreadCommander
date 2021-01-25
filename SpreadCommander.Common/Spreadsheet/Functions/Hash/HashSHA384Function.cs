using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions.Hash
{
    public class HashSHA384Function : BaseHashFunction
    {
        public HashSHA384Function()
        {
            _Hash = SHA384.Create();
        }

        public override string Name        => "HASH.SHA384";
        public override string Description => "Calculates SHA384 hash of the string.";
    }
}
