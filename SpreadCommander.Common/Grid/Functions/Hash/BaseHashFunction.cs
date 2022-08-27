using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Grid.Functions.Hash
{
    public class BaseHashFunction : ICustomFunctionOperatorBrowsable, IDisposable
    {
        protected HashAlgorithm _Hash;

        #region IDisposable Members
        public void Dispose()
        {
            _Hash?.Dispose();
            _Hash = null;
        }
        #endregion IDisposable Members

        #region ICustomFunctionOperatorBrowsable Members
        public FunctionCategory Category                                              => FunctionCategory.Math;
        public virtual string Description                                             => "Calculates hash of the input object.";
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
                var value = operands[0];
                if (value == null)
                    return null;

                var encoding = Encoding.UTF8;
                if (operands.Length >= 2)
                    encoding = Utils.GetEncoding(System.Convert.ToString(operands[1]));

                var data = Utils.GetBytes(value, encoding);

                if (data == null)
                    return null;

                var hash   = ComputeHash(data);
                var result = System.Convert.ToHexString(hash);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual string Name => null;

        public Type ResultType(params Type[] operands) => typeof(string);
        #endregion

        protected virtual byte[] ComputeHash(byte[] data)
        {
            lock (_Hash)
            {
                var result = _Hash.ComputeHash(data);
                return result;
            }
        }
    }
}
