using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using DevExpress.XtraRichEdit;
using SpreadCommander.Common.PowerShell.CmdLets;
using System.Threading;
using SpreadCommander.Common.ScriptEngines;

namespace SpreadCommander.Common.PowerShell.Host
{
    public class ExternalHost
    {
        private readonly SpreadCommanderHost _Host;

        internal ExternalHost(SpreadCommanderHost host)
        {
            _Host = host ?? throw new ArgumentNullException(nameof(host), "Host must be assigned");
            SCHost = new SCHost(this);
        }

        public ISynchronizeInvoke SynchronizeInvoke => _Host.HostOwner.SynchronizeInvoke;

        public IRichEditDocumentServer BookServer => _Host.HostOwner.BookServer;
        public Document Book                      => _Host.HostOwner.BookServer?.Document;
        public IWorkbook Spreadsheet              => _Host.HostOwner.Spreadsheet;
        public DataSet GridDataSet                => _Host.HostOwner.GridDataSet;
        public IFileViewer FileViewer             => _Host.HostOwner.FileViewer;

        public SCHost SCHost { get; }

        public PowerShellScriptEngine Engine => (PowerShellScriptEngine)_Host.HostOwner;

        public bool Silent
        {
            get => _Host.HostOwner.Silent;
            set => _Host.HostOwner.Silent = value;
        }

        public readonly object LockUIObject               = new object();
        public readonly static object LockReadWriteObject = new object();

        public int DefaultDPI
        {
            get => _Host.HostOwner.DefaultChartDPI;
            set => _Host.HostOwner.DefaultChartDPI = Utils.ValueInRange(value, 48, 4800);
        }

        public virtual void ExecuteMethodSync(Action function)
        {
            var sync = SynchronizeInvoke;

            if (sync?.InvokeRequired ?? false)
                sync.Invoke(function, Array.Empty<object>());
            else
                function();
        }

        public virtual void ScrollToCaret()
        {
            _Host.HostOwner.ScrollToCaret();
        }
    }
}
