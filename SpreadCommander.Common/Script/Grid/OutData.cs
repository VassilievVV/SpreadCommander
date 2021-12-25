using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Grid
{
    public class OutDataOptions
    {
        [Description("Table name")]
        public string TableName { get; set; }

        [Description("List of data source columns to export. If not provided - all columns will be exported.")]
        public string[] SelectColumns { get; set; }

        [Description("Skip listed columns from data source.")]
        public string[] SkipColumns { get; set; }

        [Description("Ignore errors thrown when getting property values")]
        public bool IgnoreErrors { get; set; }

        [Description("Format conditions for spreadsheet table")]
        public string Formatting { get; set; }

        [Description("Replace existing table if it exists.")]
        public bool Replace { get; set; }

        [Description("Name of master-detail relation")]
        public string RelationName { get; set; }

        [Description("Master table for master-detail relationship")]
        public string ParentTable { get; set; }

        [Description("Create master-detail relationship. Comma-separated list of parent columns")]
        public string[] ParentColumns { get; set; }

        [Description("Create master-detail relationship. List of column names for relation. If not set - ParentColumns are using.")]
        public string[] ChildColumns { get; set; }

        [Description("When set - current tables, relations etc. will be removed from the DataSet.")]
        public bool ResetDataSet { get; set; }
    }

    public partial class SCGrid
    {
        public SCGrid OutData(object dataSource, OutDataOptions options = null)
        {
            options ??= new OutDataOptions();

            var dataTable = GetDataSource(dataSource,
                new DataSourceParameters() { IgnoreErrors = options.IgnoreErrors, Columns = options.SelectColumns, SkipColumns = options.SkipColumns });

            try
            {
                List<BaseCommand> commands = null;

                if (!string.IsNullOrWhiteSpace(options.Formatting))
                {
                    var scanner = new Scanner();
                    var parser  = new Parser(scanner);

                    var tree = parser.Parse(options.Formatting);
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

                ExecuteSynchronized(() => DoWriteTable(DataSet, dataTable, commands, options));
                return this;
            }
            finally
            {
                if (dataTable != dataSource)
                    dataTable.Dispose();
            }
        }

        protected virtual void DoWriteTable(DataSet dataSet, DataTable dataTable, List<BaseCommand> commands, OutDataOptions options)
        {
            options ??= new OutDataOptions();

            if (options.ResetDataSet)
                dataSet.Reset();

            //Set table name
            var tableName = options.TableName ?? dataTable.TableName;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableNames = new List<string>();
                foreach (DataTable table in dataSet.Tables)
                    tableNames.Add(table.TableName);
                tableName = Utils.AddUniqueString(tableNames, "Table1", StringComparison.CurrentCultureIgnoreCase, false);
            }
            dataTable.TableName = tableName;

            if (options.Replace)
            {
                var table = dataSet.Tables[tableName];
                if (table != null)
                {
                    for (int i = dataSet.Relations.Count - 1; i >= 0; i--)
                    {
                        var relation = dataSet.Relations[i];
                        if (relation.ParentTable == table || relation.ChildTable == table)
                            dataSet.Relations.RemoveAt(i);
                    }

                    dataSet.Tables.Remove(table);
                }
            }

            dataSet.Tables.Add(dataTable);

            if (!string.IsNullOrWhiteSpace(options.ParentTable))
            {
                AddRelation(options.RelationName, options.ParentTable, tableName, options.ParentColumns, options.ChildColumns);
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
                    var childDataTable = dataSet.Tables[childTable];

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
