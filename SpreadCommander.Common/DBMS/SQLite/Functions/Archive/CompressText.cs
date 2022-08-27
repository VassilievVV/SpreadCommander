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
    [SQLiteFunction(Name = "CompressText", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class CompressTextFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 1)
                return null;

            var text        = System.Convert.ToString(args[0]);
            string encoding = null;
            if (args.Length > 1)
                encoding = System.Convert.ToString(args[1]);
            
            var result = ArchiveHelper.CompressText(text, encoding);
            return result;
        }
    }
}
