using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Random
{
    [SQLiteFunction(Name = "RandTriangular", Arguments = 3, FuncType = FunctionType.Scalar)]
    public class RandTriangularFunction : SQLiteFunction
    {
        private static readonly System.Random _Random = new System.Random();

        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 3)
                return null;

            if (args[0] == null || args[1] == null || args[2] == null)
                return null;

            try
            {
                double a = Convert.ToDouble(args[0]);
                double b = Convert.ToDouble(args[1]);
                double c = Convert.ToDouble(args[2]);

                if (double.IsNaN(a) || double.IsNaN(b) || double.IsNaN(c))
                    return null;

                lock (_Random)
                {
                    double result = _Random.NextTriangular(a, b, c);
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
