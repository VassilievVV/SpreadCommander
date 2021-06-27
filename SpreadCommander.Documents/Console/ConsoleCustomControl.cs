using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadCommander.Documents.Console
{
    public partial class ConsoleCustomControl : ConsoleBaseControl
    {
        public event EventHandler<RibbonUpdateRequestEventArgs> RibbonUpdateRequest;

        public ConsoleCustomControl()
        {
            InitializeComponent();
        }

        public virtual string Caption => "CustomControl";
        public virtual DevExpress.Utils.Svg.SvgImage CaptionSvgImage => null;

        private DataSet _DataSet;
        public virtual DataSet DataSet
        {
            get => _DataSet;
            set
            {
                if (_DataSet == value)
                    return;
                
                var oldDataSet = _DataSet;
                _DataSet = value;
                DataSetChanged(oldDataSet, _DataSet);
            }
        }

        protected virtual void DataSetChanged(DataSet oldDataSet, DataSet newDataSet)
        {
            //Do nothing, just collect garbage
            GC.Collect();
        }

        protected virtual void FireRibbonUpdateRequest()
        {
            if (this is IRibbonHolder ribbonHolder)
                RibbonUpdateRequest?.Invoke(this, new RibbonUpdateRequestEventArgs() { RibbonHolder = ribbonHolder, IsFloating = false });
        }
    }
}
