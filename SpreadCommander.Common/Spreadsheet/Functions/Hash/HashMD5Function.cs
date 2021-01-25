using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Spreadsheet.Functions.Hash
{
    public class HashMD5Function: BaseHashFunction
    {
        public HashMD5Function()
        {
            _Hash = MD5.Create();
        }

        public override string Name        => "HASH.MD5";
        public override string Description => "Calculates MD5 hash of the string.";
    }
}
