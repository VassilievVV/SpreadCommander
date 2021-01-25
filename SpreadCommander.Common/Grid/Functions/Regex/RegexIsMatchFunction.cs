using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using RE = System.Text.RegularExpressions;

namespace SpreadCommander.Common.Grid.Functions.Regex
{
    public class RegexIsMatchFunction: ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName              = "RegexIsMatch";
        static readonly RegexIsMatchFunction instance = new RegexIsMatchFunction();

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
        public string Description                                                     => "Whether the regular expression specified in the Regex constructor finds a match in a specified input string.\r\nParameters: input, pattern."; 
        public bool IsValidOperandCount(int count)                                    => (count == 2);
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => (operandCount == 2);
        public int MaxOperandCount                                                    => 2;
        public int MinOperandCount                                                    => 2;
        #endregion

        #region ICustomFunctionOperator Members

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length != 2)
                throw new ArgumentException("Incorrect arguments", nameof(operands));

            try
            {
                var input   = operands[0]?.ToString();
                var pattern = operands[1]?.ToString();

                if (input == null || pattern == null)
                    return null;

                var result = RE.Regex.IsMatch(input, pattern, RegexOptions.ExplicitCapture);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Name => FunctionName;

        public Type ResultType(params Type[] operands) => typeof(bool);
        #endregion 
    }
}
