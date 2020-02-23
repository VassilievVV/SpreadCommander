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
    [SQLiteFunction(Name = "Pow", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class PowFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 1 || args.Length > 2)
                return null;

            if (args[0] == null || args[1] == null)
                return null;

            try
            {
                double value1 = Convert.ToDouble(args[0]);
                double value2 = Convert.ToDouble(args[1]);
                if (value1 == double.NaN || value2 == double.NaN)
                    return null;

                double result = System.Math.Pow(value1, value2);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
