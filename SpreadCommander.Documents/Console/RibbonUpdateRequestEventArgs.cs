using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Console
{
    public class RibbonUpdateRequestEventArgs : EventArgs
    {
        public IRibbonHolder RibbonHolder   { get; set; }
        public bool IsFloating              { get; set; }
    }
}
