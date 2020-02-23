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
    [SQLiteFunction(Name = "PathCombine", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class PathCombineFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 2)
                return null;

            try
            {
                var parts = new List<string>();

                foreach (var arg in args)
                    parts.Add(arg?.ToString() ?? string.Empty);

                var result = System.IO.Path.Combine(parts.ToArray());
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
