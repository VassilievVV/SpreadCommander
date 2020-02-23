using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Common.Messages
{
    public class ControlModifiedMessage: BaseMessage
    {
        public Control Control  { get; set; }
        public bool Modified    { get; set; }
    }
}
