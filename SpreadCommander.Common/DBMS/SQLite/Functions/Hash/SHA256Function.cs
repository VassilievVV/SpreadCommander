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
    [SQLiteFunction(Name = "SHA256", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class SHA256Function : SQLiteHashFunction
    {
        public SHA256Function(): base()
        {
            _Hash = SHA256.Create();
        }
    }
}
