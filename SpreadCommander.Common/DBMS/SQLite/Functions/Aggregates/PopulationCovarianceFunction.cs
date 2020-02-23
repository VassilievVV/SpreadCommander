using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Aggregates
{
    [SQLiteFunction(Name = "COVARP", Arguments = 2, FuncType = FunctionType.Aggregate)]
    public class PopulationCovarianceFunction : TwoArgAggregateFunction
    {
        protected override double? CalculateAggregate(IList<double> records1, IList<double> records2) => records1.PopulationCovariance(records2);
    }
}
