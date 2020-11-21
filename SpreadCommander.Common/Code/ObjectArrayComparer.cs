using System;
using System.Collections.Generic;
using System.Text;

namespace SpreadCommander.Common.Code
{
    public class ObjectArrayComparer : IEqualityComparer<object[]>
    {
        public ObjectArrayComparer(): this(true)
        {
        }

        public ObjectArrayComparer(bool caseSensitive)
        {
            CaseSensitive = caseSensitive;
        }

        public bool CaseSensitive { get; } = true;

        public bool Equals(object[] x, object[] y)
        {
            if ((x == null || x.Length <= 0) && (y == null || y.Length <= 0))
                return true;

            if ((x == null || x.Length <= 0) || (y == null || y.Length <= 0))
                return false;

            if (x.Length != y.Length)
                return false;

            for (int i = 0; i < x.Length; i++)
            {
                var itemX = x[i];
                var itemY = y[i];

                if ((itemX == null || itemX == DBNull.Value) && (itemY == null || itemY == DBNull.Value))
                    continue;

                if ((itemX == null || itemX == DBNull.Value) || (itemY == null || itemY == DBNull.Value))
                    return false;

                if (itemX is string strX && itemY is string strY)
                {
                    if (string.Compare(strX, strY, !CaseSensitive) != 0)
                        return false;
                }
                else if (!itemX.Equals(itemY))
                    return false;
            }

            return true;
        }

        public int GetHashCode(object[] obj)
        {
            if (obj == null || obj.Length <= 0)
                return 0;

            int result = 0;

            for (int i = 0; i < obj.Length; i++)
            {
                result ^= i;

                var item = obj[i];
                if (item != null && item != DBNull.Value)
                {
                    if (item is string str && CaseSensitive)
                        item = str.ToLower();
                    result ^= item.GetHashCode();
                }
            }

            return result;
        }
    }
}
