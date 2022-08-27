using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Regex
{
    [SQLiteFunction(Name = "RegexMatch", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class RegexMatchFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 2 || args.Length > 3)
                return null;

            try
            {
                var input    = args[0]?.ToString();
                var pattern  = args[1]?.ToString();
                var matchNum = System.Convert.ToInt32(args.Length > 2 ? System.Convert.ToInt32(args[2]) : 0);

                if (input == null || pattern == null)
                    return null;

                var match = System.Text.RegularExpressions.Regex.Match(input, pattern, RegexOptions.ExplicitCapture);
                int counter = 0;
                while (counter < matchNum && match.Success)
                {
                    match = match.NextMatch();
                    counter++;
                }

                if (!match.Success)
                    return null;

                return match.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
