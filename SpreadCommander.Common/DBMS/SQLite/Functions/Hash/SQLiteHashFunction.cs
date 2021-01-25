using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Hash
{
    public class SQLiteHashFunction: SQLiteFunction
    {
        protected HashAlgorithm _Hash;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_Hash != null)
            {
                _Hash.Dispose();
                _Hash = null;
            }
        }

        public override object Invoke(object[] args)
        {
            if (args == null || args.Length < 1)
                return null;

            try
            {
                var value = args[0];
                if (value == null)
                    return null;

                var encoding = Encoding.UTF8;
                if (args.Length >= 2)
                    encoding = GetEncoding(Convert.ToString(args[1]));

                var data = GetBytes(value, encoding);

                if (data == null)
                    return null;

                var result = ComputeHash(data);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

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
