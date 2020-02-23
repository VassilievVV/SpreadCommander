using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Regex
{
    [SQLiteFunction(Name = "RegexIsMatch", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class RegexIsMatchFunction: SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 2)
                return null;

            try
            {
                var input   = args[0]?.ToString();
                var pattern = args[1]?.ToString();

                if (input == null || pattern == null)
                    return null;

                var result = System.Text.RegularExpressions.Regex.IsMatch(input, pattern, RegexOptions.ExplicitCapture);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
