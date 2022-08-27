using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using static SpreadCommander.Common.Code.UnpivotDataReader;

namespace SpreadCommander.Common.DBMS.SQLite.Functions.Convert
{
    [SQLiteFunction(Name = "Base64Encode", Arguments = -1, FuncType = FunctionType.Scalar)]
    public class Base64EncodeFunction : SQLiteFunction
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

                    if (string.Compare(strEncoding, "HEX", true) == 0)
                    {
                        isHex = true;

                        var strValue = System.Convert.ToString(value);
                        if (strValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            value = strValue[2..];
                        else
                            value = strValue;
                    }
                    else
                        encoding = Utils.GetEncoding(strEncoding);
                }

                byte[] data;
                if (isHex)
                    data = System.Convert.FromHexString(System.Convert.ToString(value));
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
    }
}
