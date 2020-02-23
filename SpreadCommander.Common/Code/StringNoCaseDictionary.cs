using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
    public class StringNoCaseDictionary<T>: Dictionary<string, T>
    {
        public StringNoCaseDictionary(): base(StringComparer.OrdinalIgnoreCase)
        {

        }
    }
}
