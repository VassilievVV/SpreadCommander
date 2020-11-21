using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Windows.Forms;
using FlatFiles;
using DotNetDBF;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Import, "Dbf")]
    [OutputType(typeof(DataTable))]
    public class ImportDbfCmdlet: SCCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "Name of the file containing delimited data.")]
        public string FileName { get; set; }


        protected override void ProcessRecord()
        {
            var result = ImportText();
            WriteObject(result);
        }

        private object ImportText()
        {
            var fileName = Project.Current.MapPath(FileName);
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                throw new FileNotFoundException($"File '{fileName}' not found.");

            using var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var reader = new DBFReader(stream);

            var table = new DataTable("DbfData");
            
            foreach (var field in reader.Fields)
            {
                var column = table.Columns.Add(field.Name, field.Type);
                switch (field.DataType)
                {
                    case NativeDbType.Char:
                    case NativeDbType.Memo:
                        column.MaxLength = field.FieldLength;
                        break;
                }
            }

            table.BeginLoadData();
            try
            {
                object[] records;
                while ((records = reader.NextRecord()) != null)
                {
                    table.Rows.Add(records);
                }
            }
            finally
            {
                table.EndLoadData();
            }

            return table;
        }
    }
}
