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
    public class StringFormatFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName = "StringFormat";
        static readonly StringFormatFunction instance = new StringFormatFunction();

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
        public string Description                                                     => "Formats a string using .Net pattern, such as {0}.";
        public bool IsValidOperandCount(int count)                                    => (Utils.IsValueInRange(count, 2, 20));
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => (Utils.IsValueInRange(operandCount, 2, 20));
        public int MaxOperandCount                                                    => 20;
        public int MinOperandCount                                                    => 2;
        #endregion

        #region ICustomFunctionOperator Members

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length < 2 || operands.Length > 20)
                throw new ArgumentException("Incorrect arguments", nameof(operands));

            try
            {
                var parts = new List<object>();

                string format = operands[0]?.ToString();

                for (int i = 1; i < operands.Length; i++)
                    parts.Add(operands[i]);

                var result = string.Format(format, parts.ToArray());
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
