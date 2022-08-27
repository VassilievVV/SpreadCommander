using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.String
{
    [SQLiteFunction(Name = "QuoteString", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class QuoteStringFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 1)
                return null;

            try
            {
                var value = args[0];
                if (value == null)
                    return null;

                var quote = args.Length >= 2 ? args[1]?.ToString() : "\"";
                if (string.IsNullOrWhiteSpace(quote))
                    quote = "\"";

                var result = Utils.QuoteString(value?.ToString(), quote);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
