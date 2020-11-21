using DevExpress.XtraEditors;
using SpreadCommander.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Messages
{
    public class DocumentEndAddingMessage: BaseMessage
    {
        public XtraForm DocumentView { get; set; }
    }
}
