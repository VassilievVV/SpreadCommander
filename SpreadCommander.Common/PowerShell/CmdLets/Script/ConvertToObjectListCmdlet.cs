using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.SqlScript;

namespace SpreadCommander.Common.PowerShell.CmdLets.Script
{
    [Cmdlet(VerbsData.ConvertTo, "ObjectList")]
    public class ConvertToObjectListCmdlet : SCCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Data source for data tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source.")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values.")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Type of item in result list.")]
        [Alias("Type")]
        public Type ItemType { get; set; }

        [Parameter(HelpMessage = "If set - objects will be sending individually into pipeline.")]
        public SwitchParameter EnumerateCollection { get; set; }


        #region PropertyList
        private class PropertyList
        {
            public string Name              { get; set; }
            public Type Type                { get; set; }
            public MemberInfo MemberInfo    { get; set; }
            public bool IsField             { get; set; }
        }
        #endregion

        private readonly List<PSObject> _Input = new();

        protected override void BeginProcessing()
        {
            _Input.Clear();
        }

        protected override void ProcessRecord()
        {
            var obj = DataRecord;
            _Input.Add(obj);
        }

        protected override void EndProcessing()
        {
            var result = ConvertList();
            WriteObject(result, EnumerateCollection);
        }

        protected IList ConvertList()
        {
            var resultType = typeof(List<>).MakeGenericType(ItemType);
            var result     = Activator.CreateInstance(resultType) as IList;

            using var reader = GetDataSourceReader(_Input, DataSource, 
                new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns, SkipColumns = this.SkipColumns });
            if (reader == null)
                return result;

            var properties = new List<PropertyList>();
            for (int i = 0; i < reader.VisibleFieldCount; i++)
            {
                var propList = new PropertyList()
                {
                    Name = reader.GetName(i),
                    Type = reader.GetFieldType(i)
                };

                var propInfo = ItemType.GetProperty(propList.Name, BindingFlags.Instance | BindingFlags.Public);
                if (propInfo != null)
                {
                    propList.MemberInfo = propInfo;
                    properties.Add(propList);
                }
                else
                {
                    var fieldInfo = ItemType.GetField(propList.Name, BindingFlags.Instance | BindingFlags.Public);
                    if (fieldInfo != null)
                    {
                        propList.MemberInfo = fieldInfo;
                        propList.IsField = true;
                        properties.Add(propList);
                    }
                }
            }

            while (reader.Read())
            {
                var obj = Activator.CreateInstance(ItemType);

                for (int i = 0; i < properties.Count; i++)
                {
                    var propList = properties[i];
                    var value    = reader.GetValue(propList.Name);
                    value        = Utils.ChangeType(propList.Type, value);

                    if (propList.IsField)
                        ((FieldInfo)propList.MemberInfo).SetValue(obj, value);
                    else
                        ((PropertyInfo)propList.MemberInfo).SetValue(obj, value);
                }


                result.Add(obj);
            }

            return result;
        }
    }
}
