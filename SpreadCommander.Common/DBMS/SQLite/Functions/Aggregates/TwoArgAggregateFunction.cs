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
    public class TwoArgAggregateFunction : SQLiteFunction
    {
        #region Context
        private class Context
        {
            public List<double> Values1 = new ();
            public List<double> Values2 = new ();
        }
        #endregion

        //Allow parameters that are not aggregated.
        protected object[] _Parameters;

        public override void Step(object[] args, int stepNumber, ref object contextData)
        {
            if (args == null || args.Length < 2 || args[0] == null || args[1] == null)
                return;

            if (args.Length > 2 && _Parameters == null)
            {
                _Parameters = new object[args.Length - 2];
                for (int i = 2; i < args.Length; i++)
                    _Parameters[i - 2] = args[i];
            }

            var value1 = System.Convert.ToDouble(args[0]);
            var value2 = System.Convert.ToDouble(args[1]);
            if (double.IsNaN(value1) || double.IsNaN(value2))
                return;

            if (contextData == null)
                contextData = new Context();

            var context = (Context)contextData;
            context.Values1.Add(value1);
            context.Values2.Add(value2);
        }

        public override object Final(object contextData)
        {
            var records = (Context)contextData;
            if (records.Values1.Count <= 1)
                return null;

            var result = CalculateAggregate(records.Values1, records.Values2);
            if (result.HasValue && double.IsNaN(result.Value))
                result = null;

            return result.HasValue ? (object)result.Value : null;
        }

        protected virtual double? CalculateAggregate(IList<double> records1, IList<double> records2) => null;
    }
}
