using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.String
{
    [SQLiteFunction(Name = "StringFormat", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class StringFormatFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 2)
                return null;

            try
            {
                var parts = new List<object>();

                string format = args[0]?.ToString();

                for (int i = 1; i < args.Length; i++)
                    parts.Add(args[i]);

                var result = string.Format(format, parts.ToArray());
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
