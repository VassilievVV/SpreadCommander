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
    [SQLiteFunction(Name = "Ceiling", Arguments = 1, FuncType = FunctionType.Scalar)]
    public class CeilingFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 1)
                return null;

            if (args[0] == null)
                return null;

            try
            {
                double value = Convert.ToDouble(args[0]);
                if (double.IsNaN(value))
                    return null;

                double result = System.Math.Ceiling(value);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
