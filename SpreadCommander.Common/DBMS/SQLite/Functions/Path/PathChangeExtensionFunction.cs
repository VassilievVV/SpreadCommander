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
    [SQLiteFunction(Name = "PathChangeExtension", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class PathChangeExtensionFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 2)
                return null;

            try
            {
                var path = args[0]?.ToString();
                var ext  = args[1]?.ToString();

                if (path == null)
                    return null;

                var result = System.IO.Path.ChangeExtension(path, ext);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
