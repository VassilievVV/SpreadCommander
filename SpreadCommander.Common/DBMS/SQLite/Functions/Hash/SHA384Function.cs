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
    [SQLiteFunction(Name = "SHA384", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class SHA384Function : SQLiteHashFunction
    {
        public SHA384Function() : base()
        {
            _Hash = SHA384.Create();
        }
    }
}
