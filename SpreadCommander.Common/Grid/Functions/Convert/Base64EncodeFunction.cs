using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Grid.Functions.Convert
{
    public class Base64EncodeFunction : ICustomFunctionOperatorBrowsable
    {
        public const string FunctionName              = "Base64Encode";
        static readonly Base64EncodeFunction instance = new();

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
        public string Description                                                     => "Encodes Base64-encoded text.";
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
                var value = operands[0]?.ToString();
                if (value == null)
                    return null;

                var encoding = Encoding.UTF8;
                bool isHex   = false;
                if (operands.Length >= 2)
                {
                    var strEncoding = System.Convert.ToString(operands[1]);

                    if (string.Compare(strEncoding, "HEX", true) == 0)
                    {
                        isHex = true;

                        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            value = value[2..];
                    }
                    else
                        encoding = Utils.GetEncoding(strEncoding);
                }

                byte[] data;
                if (isHex)
                    data = System.Convert.FromHexString(value);
                else
                    data = Utils.GetBytes(value, encoding);

                if (data == null)
                    return null;

                var result = System.Convert.ToBase64String(data, Base64FormattingOptions.None);
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
