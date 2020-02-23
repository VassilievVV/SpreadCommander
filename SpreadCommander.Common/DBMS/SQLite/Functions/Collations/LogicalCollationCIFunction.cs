using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Collations
{
    [SQLiteFunction(Name = "LogicalCI", FuncType = FunctionType.Collation)]
    public class LogicalCollationCIFunction : SQLiteFunction
    {
        public override int Compare(string param1, string param2)
        {
            return StringLogicalComparer.Compare(param1, param2, false);
        }
    }
}
