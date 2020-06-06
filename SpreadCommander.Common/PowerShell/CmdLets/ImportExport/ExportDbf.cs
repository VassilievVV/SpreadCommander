using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Windows.Forms;
using SpreadCommander.Common.Code;
using DotNetDBF;
using System.Linq;
using DevExpress.XtraMap.Native;

namespace SpreadCommander.Common.PowerShell.CmdLets.ImportExport
{
    [Cmdlet(VerbsData.Export, "Dbf")]
    public class ExportDbf: SCCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Data source for spreadsheet tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of the file containing delimited data.")]
        public string FileName { get; set; }

        [Parameter(HelpMessage = "Whether to allow overwrite file or no.")]
        public SwitchParameter Overwrite { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output")]
        public SwitchParameter PassThru { get; set; }


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
            ExportData();

            if (PassThru)
                WriteObject(_Output, true);
        }

        private void ExportData()
        {
            var fileName = Project.Current.MapPath(FileName);

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("Filename is not provided.");

            if (!Overwrite && File.Exists(fileName))
                throw new Exception($"File '{FileName}' already exists.");

            if (Overwrite && File.Exists(fileName))
                File.Delete(fileName);

            using var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
            using var dataReader = GetDataSourceReader(_Output, DataSource, IgnoreErrors);

            var writer      = new DBFWriter(stream);
            var dbFields    = new List<DBFField>();
            int columnCount = dataReader.FieldCount;

            var schema = dataReader.GetSchemaTable();

            for (int i = 0; i < columnCount; i++)
            {
                var fieldName = dataReader.GetName(i) ?? string.Empty;
                var name      = fieldName.Length <= 10 ? fieldName : fieldName.Substring(0, 10);
                var type      = dataReader.GetFieldType(i);

                DBFField dbField = null;
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Empty:
                    case TypeCode.Object:
                    case TypeCode.DBNull:
                        throw new Exception($"Type '{type.FullName}' cannot be exported into DBF.");
                    case TypeCode.Boolean:
                        dbField = new DBFField(name, NativeDbType.Logical);
                        break;
                    case TypeCode.Char:
                        int charSize = GetColumnMaxLength(name);
                        dbField = new DBFField(name, NativeDbType.Char, charSize);
                        break;
                    case TypeCode.SByte:
                        dbField = new DBFField(name, NativeDbType.Numeric, 5);
                        break;
                    case TypeCode.Byte:
                        dbField = new DBFField(name, NativeDbType.Numeric, 5);
                        break;
                    case TypeCode.Int16:
                        dbField = new DBFField(name, NativeDbType.Numeric, 6);
                        break;
                    case TypeCode.UInt16:
                        dbField = new DBFField(name, NativeDbType.Numeric, 6);
                        break;
                    case TypeCode.Int32:
                        dbField = new DBFField(name, NativeDbType.Numeric, 12);
                        break;
                    case TypeCode.UInt32:
                        dbField = new DBFField(name, NativeDbType.Numeric, 12);
                        break;
                    case TypeCode.Int64:
                        dbField = new DBFField(name, NativeDbType.Numeric, 19);
                        break;
                    case TypeCode.UInt64:
                        dbField = new DBFField(name, NativeDbType.Numeric, 19);
                        break;
                    case TypeCode.Single:
                        dbField = new DBFField(name, NativeDbType.Float, 19, 4);
                        break;
                    case TypeCode.Double:
                        dbField = new DBFField(name, NativeDbType.Float, 19, 4);
                        break;
                    case TypeCode.Decimal:
                        dbField = new DBFField(name, NativeDbType.Float, 19, 4);
                        break;
                    case TypeCode.DateTime:
                        dbField = new DBFField(name, NativeDbType.Date);
                        break;
                    case TypeCode.String:
                        int strSize = GetColumnMaxLength(fieldName);
                        dbField = new DBFField(name, NativeDbType.Char, strSize);
                        break;
                }

                dbFields.Add(dbField);
            }

            writer.Fields = dbFields.ToArray();
            
            object[] values = new object[columnCount];
            int[] lengths   = new int[columnCount];

            for (int i = 0; i < writer.Fields.Length; i++)
                lengths[i] = writer.Fields[i].FieldLength;

            while (dataReader.Read())
            {
                for (int i = 0; i < columnCount; i++)
                {
                    object value = dataReader.GetValue(i);
                    if (value is string str && str.Length > lengths[i])
                        value = str.Substring(0, lengths[i]);

                    values[i] = value;
                }

                writer.WriteRecord(values);
            }


            int GetColumnMaxLength(string columnName)
            {
                const int DefaultStringColumnSize = 254;

                if (schema == null || schema.Rows.Count <= 0)
                    return DefaultStringColumnSize;

                if (schema.Columns["ColumnName"] == null || schema.Columns["ColumnSize"] == null)
                    return DefaultStringColumnSize;

                foreach (DataRow row in schema.Rows)
                {
                    var colName = Convert.ToString(row["ColumnName"]);
                    if (string.Compare(colName, columnName, true) == 0)
                    {
                        int size = Convert.ToInt32(row["ColumnSize"]);
                        if (size > 0 && size <= DefaultStringColumnSize)
                            return size;
                        return DefaultStringColumnSize;
                    }
                }

                return DefaultStringColumnSize;
            }
        }
    }
}
