using SpreadCommander.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Archive
{
    [SQLiteFunction(Name = "DecompressText", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class DecompressTextFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 1)
                return null;

            if (args[0] == null)
                return null;

            if (args[0].GetType() != typeof(byte[]))
                throw new Exception("DecompressText() requires BLOB as parameter.");

            var text = args[0] as byte[];
            string encoding = null;
            if (args.Length > 1)
                encoding = Convert.ToString(args[1]);

            var result = ArchiveHelper.DecompressText(text, encoding);
            return result;
        }
    }
}
