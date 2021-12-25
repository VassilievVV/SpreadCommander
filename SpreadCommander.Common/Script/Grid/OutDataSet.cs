using SpreadCommander.Common.Code;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Grid
{
    public class OutDataSetOptions
    {
    }

    public partial class SCGrid
    {
        [Description("Replace existing tables if they exist.")]
        public bool Replace { get; set; }

        [Description("Format conditions for spreadsheet table")]
        public string Formatting { get; set; }

        [Description("When set - current tables, relations etc. will be removed from the DataSet.")]
        public bool ResetDataSet { get; set; }

        [Description("When set - tables are removed from DataSource. This is more efficient but DataSource cannot be used later.")]
        public bool NoCopyTables { get; set; }

        public SCGrid OutDataSet(DataSet dataSource, OutDataSetOptions options = null)
        {
            options ??= new OutDataSetOptions();

            if (dataSource == null || dataSource.Tables.Count <= 0)
                return this;

            var dataSet = DataSet;
            if (dataSet == null)	//Some hosts may have no grids, in this case - do not output anywhere
                return this;

            List<BaseCommand> commands = null;

            if (!string.IsNullOrWhiteSpace(Formatting))
            {
                var scanner = new Scanner();
                var parser  = new Parser(scanner);

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
                    {
                        if (string.IsNullOrWhiteSpace(formatCondition.TableName))
                            continue;

                        var formatTable = dataSet.Tables[formatCondition.TableName];
                        if (formatTable != null)
                            formatTable.ExtendedProperties[$"Format_{counter++}"] = formatCondition.AsString();
                    }

                    counter = 1;
                    foreach (var computedColumn in commands.OfType<ComputedColumn>())
                    {
                        if (string.IsNullOrWhiteSpace(computedColumn.TableName))
                            continue;

                        var formatTable = dataSet.Tables[computedColumn.TableName];
                        if (formatTable != null)
                            formatTable.ExtendedProperties[$"ComputedColumn_{counter++}"] = computedColumn.AsString();
                    }
                }
            }

            var listTables = new List<DataTable>();
            var listRelations = new List<Relation>();
            var dictTableNames = new StringNoCaseDictionary<string>();

            foreach (var relation in dataSource.Relations.OfType<DataRelation>())
            {
                var rel = new Relation()
                {
                    RelationName = relation.RelationName,
                    ParentTableName = relation.ParentTable?.TableName,
                    ChildTableName = relation.ChildTable?.TableName,
                };

                rel.ParentColumnNames.AddRange(relation.ParentColumns?.Select(c => c.ColumnName));
                rel.ChildColumnNames.AddRange(relation.ChildColumns?.Select(c => c.ColumnName));

                listRelations.Add(rel);
            }

            foreach (var dataTable in dataSource.Tables.OfType<DataTable>().ToList())
            {
                //Set table name
                var tableName = dataTable.TableName;
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    var tableNames = new List<string>();
                    foreach (DataTable table in dataSet.Tables)
                        tableNames.Add(table.TableName);
                    tableName = Utils.AddUniqueString(tableNames, "Table1", StringComparison.CurrentCultureIgnoreCase, false);
                }

                dictTableNames[tableName] = dataTable.TableName;

                DataTable newDataTable;
                if (NoCopyTables)
                {
                    RemoveTableFromDataSet(dataTable);
                    newDataTable = dataTable;
                }
                else
                    newDataTable = dataTable.Copy();
                newDataTable.TableName = tableName;

                listTables.Add(newDataTable);
            }

            ExecuteSynchronized(() => DoWriteDataSet(dataSet, listTables, listRelations, dataSource, commands, dictTableNames));
            return this;
        }

        protected virtual void DoWriteDataSet(DataSet dataSet, List<DataTable> listTables, List<Relation> listRelations,
            DataSet dataSource, List<BaseCommand> commands, StringNoCaseDictionary<string> dictTableNames)
        {
            if (ResetDataSet)
                dataSet.Reset();

            foreach (var dataTable in listTables)
            {
                if (Replace)
                    RemoveTableFromDataSet(dataSet.Tables[dataTable.TableName]);

                dataSet.Tables.Add(dataTable);
            }

            if (listRelations != null)
            {
                foreach (var relation in listRelations)
                    AddRelation(dataSet, relation, dictTableNames);
            }

            if (commands != null)
            {
                foreach (var relation in commands.OfType<Relation>())
                    AddRelation(dataSet, relation, dictTableNames);
            }
        }

        private static void RemoveTableFromDataSet(DataTable tbl)
        {
            if (tbl == null)
                return;

            var ds = tbl.DataSet;
            if (ds == null)
                return;

            for (int i = ds.Relations.Count - 1; i >= 0; i--)
            {
                var relation = ds.Relations[i];
                if (relation.ParentTable == tbl || relation.ChildTable == tbl)
                    ds.Relations.RemoveAt(i);
            }
            for (int i = tbl.Constraints.Count - 1; i >= 0; i--)
            {
                if (tbl.Constraints[i] is ForeignKeyConstraint)
                    tbl.Constraints.RemoveAt(i);
            }
            for (int t = 0; t < ds.Tables.Count; t++)
            {
                var relTable = ds.Tables[t];

                for (int i = relTable.Constraints.Count - 1; i >= 0; i--)
                {
                    if (relTable.Constraints[i] is ForeignKeyConstraint fk && fk.RelatedTable == tbl)
                        relTable.Constraints.RemoveAt(i);
                }
            }

            ds.Tables.Remove(tbl);
        }

        private void AddRelation(DataSet dataSet, Relation relation, StringNoCaseDictionary<string> dictTableNames)
        {
            var parentTableName = dictTableNames.ContainsKey(relation.ParentTableName) ? dictTableNames[relation.ParentTableName] : null;
            var childTableName = dictTableNames.ContainsKey(relation.ChildTableName) ? dictTableNames[relation.ChildTableName] : null;

            if (!string.IsNullOrWhiteSpace(parentTableName) && !string.IsNullOrWhiteSpace(childTableName))
                AddRelation(dataSet, relation.RelationName, parentTableName, childTableName,
                    relation.ParentColumnNames.ToArray(), relation.ChildColumnNames.ToArray());
        }

        private void AddRelation(DataSet dataSet, string relationName, string parentTable, string childTable, string[] parentColumns, string[] childColumns)
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
