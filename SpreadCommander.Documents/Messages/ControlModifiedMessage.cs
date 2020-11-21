using SpreadCommander.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Messages
{
    public class ControlModifiedMessage: BaseMessage
    {
        public Control Control  { get; set; }
        public bool Modified    { get; set; }
    }
}
