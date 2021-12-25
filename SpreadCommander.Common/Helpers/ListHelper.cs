using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Helpers
{
    public static class ListHelper
    {
        public static void ShuffleList(IList list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            var rand = new Random();
            var ints = Enumerable.Range(0, list.Count)
                .Select(i => new Tuple<int, int>(rand.Next(int.MaxValue), i))
                .OrderBy(i => i.Item1)
                .Select(i => i.Item2)
                .ToArray();

            var temp = new List<object>();
            for (int i = 0; i < list.Count; i++)
                temp.Add(list[ints[i]]);

            list.Clear();
            for (int i = 0; i < temp.Count; i++)
                list.Add(temp[i]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap(IList list, int i, int j)
        {
            var temp = list[i];
            list[i]  = list[j];
            list[j]  = temp;
        }

        public static int ValueCount(IList list, object value) =>
            list.Cast<object>().Count(item => item == value);

        public static int NotValueCount(IList list, object value) =>
            list.Cast<object>().Count(item => item != value);
    }
}
