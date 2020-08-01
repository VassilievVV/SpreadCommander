using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.SqlScript;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Grid
{
    [Cmdlet(VerbsData.Out, "Data")]
    public class OutDataCmdlet: BaseGridCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, HelpMessage = "Data source for data tables. Data source shall implement interface IList or IListSource and final IList shall implement ITypedList.")]
        public PSObject DataRecord { get; set; }

        [Parameter(HelpMessage = "Data source")]
        public object DataSource { get; set; }

        [Parameter(HelpMessage = "Table name")]
        [Alias("Table")]
        public string TableName { get; set; }

        [Parameter(HelpMessage = "List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Parameter(HelpMessage = "Ignore errors thrown when getting property values")]
        [Alias("NoErrors")]
        public SwitchParameter IgnoreErrors { get; set; }

        [Parameter(HelpMessage = "Format conditions for spreadsheet table")]
        public string Formatting { get; set; }

        [Parameter(HelpMessage = "Replace existing table if it exists.")]
        public SwitchParameter Replace { get; set; }

        [Parameter(HelpMessage = "Name of master-detail relation")]
        public string RelationName { get; set; }

        [Parameter(HelpMessage = "Master table for master-detail relationship")]
        public string ParentTable { get; set; }

        [Parameter(HelpMessage = "Create master-detail relationship. Comma-separated list of parent columns")]
        public string[] ParentColumns { get; set; }

        [Parameter(HelpMessage = "Create master-detail relationship. List of column names for relation. If not set - ParentColumns are using.")]
        public string[] ChildColumns { get; set; }

        [Parameter(HelpMessage = "Returns an object representing the item with which you are working. By default, this cmdlet does not generate any output.")]
        public SwitchParameter PassThru { get; set; }
        
        [Parameter(HelpMessage = "When set - current tables, relations etc. will be removed from the DataSet.")]
        [Alias("reset")]
        public SwitchParameter ResetDataSet { get; set; }
        

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
            WriteTable();

            if (PassThru)
                WriteObject(_Output, true);
        }

        protected void WriteTable()
        {
            var dataSet = CheckExternalHost().GridDataSet;
            if (dataSet == null)	//Some hosts may have no grids, in this case - do not output anywhere
                return;

           
            var dataSource = GetDataSource(_Output, DataSource, new DataSourceParameters() { IgnoreErrors = this.IgnoreErrors, Columns = this.SelectColumns });
            if (dataSource is DataView dataView)
                dataSource = dataView.ToTable();
            if (!(dataSource is DataTable))
            {
                using var reader = new TypedListDataReader(dataSource);
#pragma warning disable IDE0068 // Use recommended dispose pattern
                var table = new DataTable();
#pragma warning restore IDE0068 // Use recommended dispose pattern
                table.Load(reader);

                dataSource = table;
            }

            var dataTable = (DataTable)dataSource;
            try
            {
                List<BaseCommand> commands = null;

                if (!string.IsNullOrWhiteSpace(Formatting))
                {
                    var scanner = new Scanner();
                    var parser = new Parser(scanner);

                    var tree = parser.Parse(Formatting);
                    if (tree.Errors.Count > 0)
                    {
                        var strErrors = new StringBuilder();

                        foreach (var error in tree.Errors)
                        {
                            if (strErrors.Length > 0)
                                strErrors.AppendLine();
                            strErrors.Append(error.Message);
                        }

                        throw new Exception(strErrors.ToString());
                    }

                    try
                    {
                        commands = tree.Eval() as List<BaseCommand>;
                    }
                    catch (Exception)
                    {
                        //Do nothing, skip invalid commands
                    }

                    if (commands != null)
                    {
                        int counter = 1;
                        foreach (var formatCondition in commands.OfType<FormatCondition>())
                            dataTable.ExtendedProperties[$"Format_{counter++}"] = formatCondition.AsString();

                        counter = 1;
                        foreach (var computedColumn in commands.OfType<ComputedColumn>())
                            dataTable.ExtendedProperties[$"ComputedColumn_{counter++}"] = computedColumn.AsString();
                    }
                }

                ExecuteSynchronized(() => DoWriteTable(dataSet, dataTable, commands));
            }
            finally
            {
                if (dataTable != DataSource)
                    dataTable.Dispose();
            }
        }

        protected virtual void DoWriteTable(DataSet dataSet, DataTable dataTable, List<BaseCommand> commands)
        {
            if (ResetDataSet)
                dataSet.Reset();

            //Set table name
            var tableName = TableName ?? dataTable.TableName;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableNames = new List<string>();
                foreach (DataTable table in dataSet.Tables)
                    tableNames.Add(table.TableName);
                tableName = Utils.AddUniqueString(tableNames, "Table1", StringComparison.CurrentCultureIgnoreCase, false);
            }
            dataTable.TableName = tableName;

            if (Replace)
            {
                var table = dataSet.Tables[tableName];
                if (table != null)
                {
                    for (int i = dataSet.Relations.Count-1; i >= 0; i--)
                    {
                        var relation = dataSet.Relations[i];
                        if (relation.ParentTable == table || relation.ChildTable == table)
                            dataSet.Relations.RemoveAt(i);
                    }

                    dataSet.Tables.Remove(table);
                }
            }

            dataSet.Tables.Add(dataTable);

            if (!string.IsNullOrWhiteSpace(ParentTable))
            {
                AddRelation(RelationName, ParentTable, tableName, ParentColumns, ChildColumns);
            }

            if (commands != null)
            {
                foreach (var relation in commands.OfType<Relation>())
                {
                    AddRelation(relation.RelationName, relation.ParentTableName, relation.ChildTableName, 
                        relation.ParentColumnNames.ToArray(), relation.ChildColumnNames.ToArray());
                }
            }


            void AddRelation(string relationName, string parentTable, string childTable, string[] parentColumns, string[] childColumns)
            {
                if (string.IsNullOrWhiteSpace(parentTable) || string.IsNullOrWhiteSpace(childTable))
                    return;

                if (parentColumns != null && parentColumns.Length > 0)
                {
                    if (childColumns == null || childColumns.Length <= 0)
                        childColumns = parentColumns;

                    var parentDataTable = dataSet.Tables[parentTable];
                    var childDataTable  = dataSet.Tables[childTable];

                    if (parentDataTable != null && childDataTable != null)
                    {
                        var columns1 = new List<DataColumn>();
                        foreach (var columnName in parentColumns)
                        {
                            var column2 = parentDataTable.Columns[columnName];
                            if (column2 == null)
                            {
                                ReportError($"Cannot create dataset relation: table '{parentDataTable.TableName}' has no column '{columnName}'.");
                                break;
                            }
                            columns1.Add(column2);
                        }

                        var columns2 = new List<DataColumn>();
                        foreach (var columnName in childColumns)
                        {
                            var column1 = childDataTable.Columns[columnName];
                            if (column1 == null)
                            {
                                ReportError($"Cannot create dataset relation: table '{childDataTable.TableName}' has no column '{columnName}'.");
                                break;
                            }
                            columns2.Add(column1);
                        }

                        if (columns1.Count == parentColumns.Length ||
                            columns2.Count == childColumns.Length)
                        {
                            try
                            {
                                var relationNames = new List<string>();
                                foreach (DataRelation relation in dataSet.Relations)
                                    relationNames.Add(relation.RelationName);

                                if (string.IsNullOrWhiteSpace(relationName))
                                    relationName = Utils.AddUniqueString(relationNames, "Relation1", StringComparison.CurrentCultureIgnoreCase, false);
                                dataSet.Relations.Add(relationName, columns1.ToArray(), columns2.ToArray());
                            }
                            catch (Exception ex)
                            {
                                ReportError($"Cannot create dataset relation: {ex.Message}");
                            }
                        }
                    }
                    else
                        ReportError($"Cannot create dataset relation '{parentTable}'-'{childTable}': table does not exist.");
                }
                else
                    ReportError("$Cannot create dataset relation: ParentColumn is not set.");
            }
        }
    }
}
