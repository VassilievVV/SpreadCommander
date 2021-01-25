using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using SpreadCommander.Common.Code;
using IO = System.IO;

namespace SpreadCommander.Common.Grid.Functions.Path
{
    public class PathGetFileNameWithoutExtensionFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName = "PathGetFileNameWithoutExtension";
        static readonly PathGetFileNameWithoutExtensionFunction instance = new PathGetFileNameWithoutExtensionFunction();

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
        public string Description                                                     => "Name of the path string without extension.";
        public bool IsValidOperandCount(int count)                                    => (count == 1);
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => (operandCount == 1);
        public int MaxOperandCount                                                    => 1;
        public int MinOperandCount                                                    => 1;
        #endregion

        #region ICustomFunctionOperator Members

        public object Evaluate(params object[] operands)
        {
            if (operands == null || operands.Length != 1)
                throw new ArgumentException("Incorrect arguments", nameof(operands));

            try
            {
                var path = operands[0]?.ToString();

                var result = IO.Path.GetFileNameWithoutExtension(path);
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
