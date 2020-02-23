using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Documents.Console
{
    public class ConsoleFixedDataSourceChartControl: ConsoleChartControl
    {
        public ConsoleFixedDataSourceChartControl(): base()
        {
            DocumentMode = ChartDocumentMode.FixedDataSource;
        }
    }
}
