using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Guid
{
    [SQLiteFunction(Name = "NewID", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class NewIdFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length > 1)
                return null;

            try
            {
                var format = args.Length > 0 ? Convert.ToString(args[0]) : "D";
                if (string.IsNullOrEmpty(format) || !FormatInRange(format, "N", "D", "B", "P", "X"))
                    format = "D";

                var result = System.Guid.NewGuid().ToString(format);
                return result;
            }
            catch (Exception)
            {
                return null;
            }


            static bool FormatInRange(string format, params string[] values)
            {
                for (int i = 0; i < values.Length; i++)
                    if (string.Compare(format, values[i], true) == 0)
                        return true;
                return false;
            }
        }
    }
}
