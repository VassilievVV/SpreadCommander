using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Random
{
    [SQLiteFunction(Name = "RandUniform", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class RandUniformFunction : SQLiteFunction
    {
        private static readonly System.Random _Random = new ();
        
        public override object Invoke(object[] args)
        {
            if (args == null || args.Length != 2)
                return null;

            if (args[0] == null || args[1] == null)
                return null;
            
            try
            {
                double min = System.Convert.ToDouble(args[0]);
                double max = System.Convert.ToDouble(args[1]);

                if (double.IsNaN(min) || double.IsNaN(max))
                    return null;

                lock (_Random)
                {
                    double rand   = _Random.NextDouble();
                    double result = min + rand * (max - min);
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
