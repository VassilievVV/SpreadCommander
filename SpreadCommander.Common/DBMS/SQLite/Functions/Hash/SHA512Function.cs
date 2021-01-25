using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Hash
{
    [SQLiteFunction(Name = "SHA512", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class SHA512Function : SQLiteHashFunction
    {
        public SHA512Function() : base()
        {
            _Hash = SHA512.Create();
        }
    }
}
