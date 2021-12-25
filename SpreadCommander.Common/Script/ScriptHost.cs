using DevExpress.Spreadsheet;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.ScriptEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script
{
    public partial class ScriptHost
    {
        public ScriptHost(BaseScriptEngine engine)
        {
            Engine      = engine;
            Book        = new Book.SCBook(this);
            Spreadsheet = new Spreadsheet.SCSpreadsheet(this);
            Grid        = new Grid.SCGrid(this);
            Heap        = new Heap.SCHeap(this);
        }

        protected internal BaseScriptEngine Engine { get; }

        public Book.SCBook Book { get; }

        public Spreadsheet.SCSpreadsheet Spreadsheet { get; }

        public Grid.SCGrid Grid { get; }

        public Heap.SCHeap Heap { get; }

        protected internal IFileViewer FileViewer => Engine.FileViewer;

        public bool Silent 
        {
            get => Engine.Silent;
            set => Engine.Silent = value; 
        }
    }
}
