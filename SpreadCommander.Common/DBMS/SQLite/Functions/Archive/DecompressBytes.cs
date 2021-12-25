using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using SpreadCommander.Common.Helpers;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Archive
{
    [SQLiteFunction(Name = "DecompressBytes", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class DecompressBytesFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 1)
                return null;

            if (args[0] == null)
                return null;

            if (args[0].GetType() != typeof(byte[]))
                throw new Exception("DecompressBytes() requires BLOB as parameter.");

            byte[] result = ArchiveHelper.DecompressBytes(args[0] as byte[]);
            return result;
        }
    }
}
