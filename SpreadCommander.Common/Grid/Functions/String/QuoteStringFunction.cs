using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Grid.Functions.String
{
    public class QuoteStringFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName             = "QuoteString";
        static readonly QuoteStringFunction instance = new ();

        public static void Register()
        {
            CriteriaOperator.RegisterCustomFunction(instance);
        }
        public static bool Unregister()
        {
            return CriteriaOperator.UnregisterCustomFunction(instance);
        }

        #region ICustomFunctionOperatorBrowsable Members
        public FunctionCategory Category                                              => FunctionCategory.Text;
        public string Description                                                     => "Quotes string.";
        public bool IsValidOperandCount(int count)                                    => (Utils.IsValueInRange(count, 1, 2));
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => (Utils.IsValueInRange(operandCount, 1, 2));
        public int MaxOperandCount                                                    => 2;
        public int MinOperandCount                                                    => 1;
        #endregion

        #region ICustomFunctionOperator Members

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length < 1 || operands.Length > 2)
                throw new ArgumentException("Incorrect arguments", nameof(operands));

            try
            {
                string value = operands[0]?.ToString();

                var quote = operands.Length >= 2 ? operands[1]?.ToString() : "\"";
                if (string.IsNullOrWhiteSpace(quote))
                    quote = "\"";

                var result = Utils.QuoteString(value?.ToString(), quote);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Name => FunctionName;

        public Type ResultType(params Type[] operands) => typeof(string);
        #endregion 
    }
}
