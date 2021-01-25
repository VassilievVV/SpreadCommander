using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using SpreadCommander.Common.Code;
using RE = System.Text.RegularExpressions;

namespace SpreadCommander.Common.Grid.Functions.Regex
{
    public class RegexReplaceFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName = "RegexReplace";
        static readonly RegexReplaceFunction instance = new RegexReplaceFunction();

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
        public string Description                                                     => "Replaces all strings that match a specified regular expression with a specified replacement string.";
        public bool IsValidOperandCount(int count)                                    => (count == 3);
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => (operandCount == 3);
        public int MaxOperandCount                                                    => 3;
        public int MinOperandCount                                                    => 3;
        #endregion

        #region ICustomFunctionOperator Members

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length != 3)
                throw new ArgumentException("Incorrect arguments", nameof(operands));

            try
            {
                var input       = operands[0]?.ToString();
                var pattern     = operands[1]?.ToString();
                var replacement = operands[2]?.ToString() ?? string.Empty;

                if (input == null || pattern == null)
                    return null;

                var result = RE.Regex.Replace(input, pattern, replacement);
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
