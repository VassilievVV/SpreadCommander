using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Path
{
    [SQLiteFunction(Name = "PathGetFileNameWithoutExtension", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class PathGetFileNameWithoutExtensionFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 1)
                return null;

            try
            {
                var path = args[0]?.ToString();

                if (path == null)
                    return null;

                var result = System.IO.Path.GetFileNameWithoutExtension(path);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
