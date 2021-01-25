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
    [SQLiteFunction(Name = "MD5", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class MD5Function : SQLiteHashFunction
    {
        public MD5Function() : base()
        {
            _Hash = MD5.Create();
        }
    }
}
