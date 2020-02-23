﻿using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Aggregates
{
    [SQLiteFunction(Name = "SkewnessP", Arguments = 1, FuncType = FunctionType.Aggregate)]
    public class PopulationSkewnessFunction : OneArgAggregateFunction
    {
        protected override double? CalculateAggregate(IList<double> records) => records.PopulationSkewness();
    }
}
