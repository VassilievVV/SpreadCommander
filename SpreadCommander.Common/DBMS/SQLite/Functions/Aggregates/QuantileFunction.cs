using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Aggregates
{
    [SQLiteFunction(Name = "Quantile", Arguments = 1, FuncType = FunctionType.Aggregate)]
    public class QuantileFunction : OneArgAggregateFunction
    {
        protected override double? CalculateAggregate(IList<double> records)
        {
            if (_Parameters == null || _Parameters.Length < 1)
                return null;

            double tau = Convert.ToDouble(_Parameters[0]);
            if (tau == double.NaN)
                return null;

            return records.Quantile(tau);
        }
    }
}
