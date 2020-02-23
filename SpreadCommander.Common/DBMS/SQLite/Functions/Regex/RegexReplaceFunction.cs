using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Regex
{
    [SQLiteFunction(Name = "RegexReplace", Arguments = 3, FuncType = FunctionType.Scalar)]
    public class RegexReplaceFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 3)
                return null;

            try
            {
                var input       = args[0]?.ToString();
                var pattern     = args[1]?.ToString();
                var replacement = args[2]?.ToString() ?? string.Empty;

                if (input == null || pattern == null)
                    return null;

                var result = System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
