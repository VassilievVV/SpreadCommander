using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Math
{
    [SQLiteFunction(Name = "Log", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class LogFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 1 || args.Length > 2)
                return null;

            if (args[0] == null || (args.Length > 1 && args[1] == null))
                return null;

            try
            {
                double value1 = Convert.ToDouble(args[0]);
                double value2 = args.Length > 1 ? Convert.ToDouble(args[1]) : System.Math.E;
                if (double.IsNaN(value1) || double.IsNaN(value2))
                    return null;

                double result = args.Length <= 1 ? System.Math.Log(value1) : System.Math.Log(value1, value2);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
