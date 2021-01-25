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
    public class RegexNamedMatchFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName = "RegexNamedMatch";
        static readonly RegexNamedMatchFunction instance = new RegexNamedMatchFunction();

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
        public string Description                                                     => "Searches the specified input string for the first occurrence of the regular expression and returns value of named group.\r\nParameters: input, pattern, group name, match number.";
        public bool IsValidOperandCount(int count)                                    => (Utils.IsValueInRange(count, 3, 4));
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => (Utils.IsValueInRange(operandCount, 3, 4));
        public int MaxOperandCount                                                    => 4;
        public int MinOperandCount                                                    => 3;
        #endregion

        #region ICustomFunctionOperator Members

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length < 3 || operands.Length > 4)
                throw new ArgumentException("Incorrect arguments", nameof(operands));

            try
            {
                var input     = operands[0]?.ToString();
                var pattern   = operands[1]?.ToString();
                var groupName = operands[2]?.ToString();
                var matchNum  = Convert.ToInt32(operands.Length > 3 ? Convert.ToInt32(operands[3]) : 0);

                if (input == null || pattern == null || string.IsNullOrWhiteSpace(groupName))
                    return null;

                var match = RE.Regex.Match(input, pattern, RegexOptions.ExplicitCapture);
                int counter = 0;
                while (counter < matchNum && match.Success)
                {
                    match = match.NextMatch();
                    counter++;
                }

                if (!match.Success)
                    return null;

                var group = match.Groups[groupName];
                if (!group.Success)
                    return null;

                return group.Value;
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
