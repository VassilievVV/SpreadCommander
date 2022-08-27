using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Convert
{
    [SQLiteFunction(Name = "Base64Decode", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class Base64DecodeFunction : SQLiteFunction
    {
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
                bool isHex   = false;
                if (args.Length >= 2)
                {
                    var strEncoding = System.Convert.ToString(args[1]);
                    if (string.Compare(strEncoding, "NONE", true) == 0)
                        encoding = null;
                    else if (string.Compare(strEncoding, "HEX", true) == 0)
                        isHex = true;
                    else 
                        encoding = Utils.GetEncoding(strEncoding);
                }

                var data = System.Convert.FromBase64String(value.ToString());

                if (data == null)
                    return null;

                object result;
                if (isHex)
                    result = System.Convert.ToHexString(data);
                else
                    result = encoding != null ? (object)encoding.GetString(data) : data;
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
