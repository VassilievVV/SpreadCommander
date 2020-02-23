using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Grid.Functions
{
    public class PathCombineFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName = "PathCombine";
        static readonly PathCombineFunction instance = new PathCombineFunction();

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
        public string Description                                                     => "Combines an array of strings into a path.";
        public bool IsValidOperandCount(int count)                                    => (Utils.IsValueInRange(count, 2, 5));
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => (Utils.IsValueInRange(operandCount, 2, 5));
        public int MaxOperandCount                                                    => 5;
        public int MinOperandCount                                                    => 2;
        #endregion

        #region ICustomFunctionOperator Members

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length < 2 || operands.Length > 20)
                throw new ArgumentException("Incorrect arguments", nameof(operands));

            try
            {
                var parts = new List<string>();

                foreach (var operand in operands)
                    parts.Add(operand?.ToString() ?? string.Empty);

                var result = Path.Combine(parts.ToArray());
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
