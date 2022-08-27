using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

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
                    encoding = Utils.GetEncoding(System.Convert.ToString(args[1]));

                var data = Utils.GetBytes(value, encoding);

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
