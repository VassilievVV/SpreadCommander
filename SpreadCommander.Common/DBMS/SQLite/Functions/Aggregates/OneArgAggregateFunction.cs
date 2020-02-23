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
    public class OneArgAggregateFunction : SQLiteFunction
    {
        //Allow parameters that are not aggregated. For example Quantile(tau).
        protected object[] _Parameters;
        
        public override void Step(object[] args, int stepNumber, ref object contextData)
        {
            if (args == null || args.Length == 0 || args[0] == null)
                return;
            
            if (args.Length > 1 && _Parameters == null)
            {
                _Parameters = new object[args.Length - 1];
                for (int i = 1; i < args.Length; i++)
                    _Parameters[i - 1] = args[i];
            }

            var value = Convert.ToDouble(args[0]);
            if (value == double.NaN)
                return;

            if (contextData == null)
                contextData = new List<double>();

            var records = (List<double>)contextData;
            records.Add(value);
        }

        public override object Final(object contextData)
        {
            var records = (List<double>)contextData;
            if (records.Count <= 1)
                return null;

            var result = CalculateAggregate(records);
            if (result == double.NaN)
                result = null;
            
            return result.HasValue ? (object)result.Value : null;
        }
        
        protected virtual double? CalculateAggregate(IList<double> records) => null;
    }
}
