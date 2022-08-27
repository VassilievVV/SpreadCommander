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
    [SQLiteFunction(Name = "Atan2", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class Atan2Function : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 2)
                return null;

            if (args[0] == null || args[1] == null)
                return null;

            try
            {
                double value1 = System.Convert.ToDouble(args[0]);
                double value2 = System.Convert.ToDouble(args[1]);
                if (double.IsNaN(value1) || double.IsNaN(value2))
                    return null;

                double result = System.Math.Atan2(value1, value2);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
