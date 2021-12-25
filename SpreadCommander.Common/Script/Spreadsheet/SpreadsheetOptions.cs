using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Spreadsheet
{
    public class SpreadsheetOptions
    {
        [Description("Spreadsheet to execute command. Leave NULL to use UI spreadsheet.")]
        public SCSpreadsheet Spreadsheet { get; set; }

        [Description("Comment for the table. In case of multiple data sources - comment is added to each one")]
        public string Comment { get; set; }
    }
}
