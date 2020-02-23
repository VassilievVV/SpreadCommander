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
    public class PathChangeExtensionFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName = "PathChangeExtension";
        static readonly PathChangeExtensionFunction instance = new PathChangeExtensionFunction();

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
        public string Description                                                     => "Changes the extension of a path string.";
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
                var path = operands[0]?.ToString();
                var ext  = operands[1]?.ToString();

                if (path == null)
                    return null;

                var result = Path.ChangeExtension(path, ext);
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
