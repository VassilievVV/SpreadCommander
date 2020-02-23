using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.Host
{
    public interface ISpreadCommanderHostOwner
    {
        bool ShouldExit			                { get; set; }
        int ExitCode			                { get; set; }
        string Title			                { get; set; }

        ISynchronizeInvoke SynchronizeInvoke    { get; }

        IRichEditDocumentServer BookServer	    { get; }
        IWorkbook Spreadsheet	                { get; }
        DataSet GridDataSet		                { get; }
        IFileViewer FileViewer                  { get; }

        int DefaultChartDPI		                { get; set; }

        bool Silent                             { get; set; }

        void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value);
        void DisplayProgress(long sourceID, ProgressRecord progress);
        string ReadLine();
        void ScrollToCaret();
    }
}
