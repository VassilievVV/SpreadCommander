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
    [SQLiteFunction(Name = "RandNormal", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class RandNormalFunction : SQLiteFunction
    {
        private static readonly System.Random _Random = new System.Random();

        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 2)
                return null;

            if (args[0] == null || args[1] == null)
                return null;

            try
            {
                double mu    = System.Convert.ToDouble(args[0]);
                double sigma = System.Convert.ToDouble(args[1]);

                if (double.IsNaN(mu) || double.IsNaN(sigma))
                    return null;

                lock (_Random)
                {
                    double result = _Random.NextGaussian(mu, sigma);
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
