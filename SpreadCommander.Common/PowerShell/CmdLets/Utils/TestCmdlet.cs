using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Test
{
    [Cmdlet(VerbsDiagnostic.Test, "ParameterSet")]
    public class TestCmdlet : SCCmdlet //, IDynamicParameters
    {
        public enum Test1Values { Value1, Value2 }
        public enum Test2Values { Value3, Value4 }
        public enum Test3Values { Value5, Value6 }

        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(ParameterSetName = "Test1", Position = 0, Mandatory = true)]
        public Test1Values Param1 { get; set; }

        [Parameter(ParameterSetName = "Test2", Position = 0, Mandatory = true)]
        public Test2Values Param2 { get; set; }

        [Parameter(ParameterSetName = "Test3", Position = 0, Mandatory = true)]
        public Test3Values Param3 { get; set; }

        [Parameter()]
        public object[] ArrayParameter { get; set; }

        [Parameter()]
        public string AnotherParameter { get; set; }

        [Parameter()]
        public double[] Point { get; set; }

        [Parameter()]
        public double[][] Line { get; set; }

        [Parameter()]
        public PointF Point2 { get; set; }

        [Parameter()]
        public Hashtable Hashtable { get; set; }

        [Parameter()]
        [ValidateRange(1, 100)]
        public double? DoubleParameter { get; set; }


        private readonly List<PSObject> _Output = new List<PSObject>();

        protected override void BeginProcessing()
        {
            _Output.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Output.Add(obj);
        }

        protected override void EndProcessing()
        {
            Host.UI.WriteLine($"Parameter set: {ParameterSetName}");
            throw new Exception("Some error");
        }
    }
}
