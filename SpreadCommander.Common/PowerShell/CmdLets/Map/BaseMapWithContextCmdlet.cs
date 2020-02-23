using DevExpress.XtraMap.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    public class BaseMapWithContextCmdlet: BaseMapCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Map context generated in other cmdlets such as New-GeoMap.")]
        public MapContext MapContext { get; set; }

        protected virtual bool PassThruMapContext => true;


        protected override void ProcessRecord()
        {
            if (MapContext?.Map == null)
                throw new ArgumentNullException("Map is not provided. Please provide MapContext from pipeline, using New-Map or another map cmdlet.",
                    nameof(MapContext));

            UpdateMapRecord();
        }

        protected override void EndProcessing()
        {
            if (MapContext?.Map == null)
                throw new ArgumentNullException("Map is not provided. Please provide MapContext from pipeline, using New-Map or another map cmdlet.",
                    nameof(MapContext));

            UpdateMap();

            if (PassThruMapContext)
                WriteObject(MapContext);
            else
                MapContext.Clear();
        }

        protected void FlushMap()
        {
            base.FlushMap(MapContext.Map);
        }

        protected virtual void UpdateMapRecord()
        {
        }

        protected virtual void UpdateMap()
        {
        }
    }
}
