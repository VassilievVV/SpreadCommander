using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Heap
{
    public partial class SCHeap
    {
        public SCHeap OutFileViewer(string fileName)
        {
            PreviewFile(fileName);
            return this;
        }
    }
}
