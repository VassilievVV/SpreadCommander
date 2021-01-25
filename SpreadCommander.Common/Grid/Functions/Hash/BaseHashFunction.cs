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
                    encoding = GetEncoding(Convert.ToString(operands[1]));

                var data = GetBytes(value, encoding);

                if (data == null)
                    return null;

                var hash   = ComputeHash(data);
                var result = Convert.ToHexString(hash);
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

        protected virtual Encoding GetEncoding(string strEncoding)
        {
            Encoding encoding = Encoding.UTF8;

            switch (strEncoding?.ToLower())
            {
                case "unicode":
                    encoding = Encoding.Unicode;
                    break;
                case "utf8":
                case "utf-8":
                    encoding = Encoding.UTF8;
                    break;
                case "utf32":
                case "utf-32":
                    encoding = Encoding.UTF32;
                    break;
                case "ascii":
                    encoding = Encoding.ASCII;
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(strEncoding))
                    {
                        if (int.TryParse(strEncoding, out int codePage))
                            encoding = Encoding.GetEncoding(codePage);
                        else
                            encoding = Encoding.GetEncoding(strEncoding);
                    }
                    break;
            }

            return encoding;
        }

        protected virtual byte[] ComputeHash(byte[] data)
        {
            lock (_Hash)
            {
                var result = _Hash.ComputeHash(data);
                return result;
            }
        }

        protected virtual byte[] GetBytes(object value, Encoding encoding)
        {
            byte[] data = null;
            if (value is byte[] v)
                data = v;
            else
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Object:
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Boolean:
                        data = BitConverter.GetBytes(Convert.ToBoolean(value));
                        break;
                    case TypeCode.Char:
                        data = BitConverter.GetBytes(Convert.ToChar(value));
                        break;
                    case TypeCode.SByte:
                        data = BitConverter.GetBytes(Convert.ToSByte(value));
                        break;
                    case TypeCode.Byte:
                        data = BitConverter.GetBytes(Convert.ToByte(value));
                        break;
                    case TypeCode.Int16:
                        data = BitConverter.GetBytes(Convert.ToInt16(value));
                        break;
                    case TypeCode.UInt16:
                        data = BitConverter.GetBytes(Convert.ToUInt16(value));
                        break;
                    case TypeCode.Int32:
                        data = BitConverter.GetBytes(Convert.ToInt32(value));
                        break;
                    case TypeCode.UInt32:
                        data = BitConverter.GetBytes(Convert.ToUInt32(value));
                        break;
                    case TypeCode.Int64:
                        data = BitConverter.GetBytes(Convert.ToInt64(value));
                        break;
                    case TypeCode.UInt64:
                        data = BitConverter.GetBytes(Convert.ToUInt64(value));
                        break;
                    case TypeCode.Single:
                        data = BitConverter.GetBytes(Convert.ToSingle(value));
                        break;
                    case TypeCode.Double:
                        data = BitConverter.GetBytes(Convert.ToDouble(value));
                        break;
                    case TypeCode.Decimal:
                        data = BitConverter.GetBytes(Convert.ToDouble(value));
                        break;
                    case TypeCode.DateTime:
                        data = BitConverter.GetBytes(Convert.ToDouble(value));
                        break;
                    case TypeCode.String:
                        if (encoding == null)
                            encoding = Encoding.UTF8;
                        data = encoding.GetBytes(Convert.ToString(value));
                        break;
                    default:
                        break;
                }
            }

            return data;
        }
    }
}
