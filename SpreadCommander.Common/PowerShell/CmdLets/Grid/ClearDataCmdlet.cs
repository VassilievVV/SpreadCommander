using System;
using System.Collections.Generic;
using System.Data;
using System.Management.Automation;
using System.Text;

namespace SpreadCommander.Common.PowerShell.CmdLets.Grid
{
    [Cmdlet(VerbsCommon.Clear, "Data")]
    public class ClearDataCmdlet: BaseGridCmdlet
    {
        protected override void EndProcessing()
        {
            var dataSet = CheckExternalHost().GridDataSet;
            if (dataSet == null)	//Some hosts may have no grids, in this case - do not output anywhere
                return;

            ExecuteSynchronized(() => DoClearData(dataSet));
        }

        protected virtual void DoClearData(DataSet dataSet)
        {
            dataSet.Reset();
        }
    }
}
