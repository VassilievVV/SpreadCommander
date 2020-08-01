#pragma warning disable CRR0047
#pragma warning disable CRR0048
#pragma warning disable CRR0050

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraTreeList;
using MySqlConnector;
using SpreadCommander.Common;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Code
{
    #region DbSchemaBaseNode
    public class DbSchemaBaseNode : TreeList.IVirtualTreeListData, IComparable
    {
        private readonly DbSchemaBaseNode _ParentNode;

        public DbSchemaBaseNode(DbSchemaBaseNode parentNode)
        {
            _ParentNode = parentNode;
        }

        public DbSchemaBaseNode ParentNode => _ParentNode;
        public virtual string Text { get; set; }

        public virtual bool CanDrag    => true;
        public virtual string DragText => QuoteDatabaseObjectName(DbConnectionType, Text);

        public virtual List<DbSchemaBaseNode> ListChildNodes()
        {
            return null;
        }

        public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
        {
            if (info.Node is DbSchemaBaseNode node)
                info.CellData = node.Text;
        }

        public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
        {
            if (info.Node is DbSchemaBaseNode node)
            {
                var childNodes = node.ListChildNodes();
                if (childNodes != null)
                    info.Children = childNodes;
            }
        }

        public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
        {
            throw new NotImplementedException();
        }

        public T FindParentNode<T>() where T: DbSchemaBaseNode
        {
            DbSchemaBaseNode result = this;

            while (result != null)
            {
                if (result is T t)
                    return t;

                result = result.ParentNode;
            }

            return null;
        }

        public DataTable GetSchema(string collectionName, string[] restrictionValues = null)
        {
            DatabaseSchemaNode dbNode         = null;
            DatabaseListSchemaNode dbListNode = null;
            DbSchemaBaseNode node             = this;

            while (node != null)
            {
                if (dbNode == null && node is DatabaseSchemaNode dbSchemaNode)
                    dbNode = dbSchemaNode;
                else if (dbListNode == null && node is DatabaseListSchemaNode dbListSchemaNode)
                    dbListNode = dbListSchemaNode;

                node = node.ParentNode;
            }

            if (dbListNode != null)
                return dbListNode.GetSchema(dbNode?.Text, collectionName, restrictionValues);

            return null;
        }

        public Type DbConnectionType
        {
            get
            {
                DatabaseListSchemaNode dbListNode = null;
                DbSchemaBaseNode node             = this;

                while (node != null)
                {
                    if (dbListNode == null && node is DatabaseListSchemaNode dbListSchemaNode)
                        dbListNode = dbListSchemaNode;

                    node = node.ParentNode;
                }

                if (dbListNode != null)
                    return dbListNode.ConnectionType;

                return null;
            }
        }

        public int CompareTo(object obj)
        {
            return StringLogicalComparer.Compare(Text, ((DbSchemaBaseNode)obj)?.Text);
        }

        public static string QuoteDatabaseObjectName(Type dbConnectionType, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return QuoteString("");

            return needQuote() ? QuoteString(value) : value;


            bool needQuote()
            {
                foreach (char c in value)
                    if (!char.IsLetterOrDigit(c))
                        return true;

                return false;
            }

            string QuoteString(string str)
            {
                if (dbConnectionType == null)
                    return $"\"{Utils.NonNullString(str).Replace("\"", "\"\"")}\"";

                if (dbConnectionType == typeof(SqlConnection) || dbConnectionType.IsSubclassOf(typeof(SqlConnection)))
                    return $"[{Utils.NonNullString(str).Replace("]", "]]")}]";
                else if (dbConnectionType == typeof(MySqlConnection) || dbConnectionType.IsSubclassOf(typeof(MySqlConnection)))
                    return $"`{Utils.NonNullString(str).Replace("`", "``")}`]";
                else if (dbConnectionType == typeof(SQLiteConnection) || dbConnectionType.IsSubclassOf(typeof(SQLiteConnection)))
                    return $"[{Utils.NonNullString(str).Replace("]", "]]")}]";

                return $"\"{Utils.NonNullString(str).Replace("\"", "\"\"")}\"";
            }
        }
    }
    #endregion

    #region DatabaseListSchemaNode
    public class DatabaseListSchemaNode : DbSchemaBaseNode
    {
        public DatabaseListSchemaNode() : base(null) { }

        public override string Text  => "Databases";
        public override bool CanDrag => false;

        public Type ConnectionType { get; set; }

        public string ConnectionString { get; set; }

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();

            var tblDatabases = GetSchema("Databases");
            if (tblDatabases == null)
                return null;

            foreach (DataRow rowDB in tblDatabases.Rows)
            {
                string dbName = Convert.ToString(rowDB["DATABASE_NAME"]);
                if (!string.IsNullOrEmpty(dbName))
                    result.Add(new DatabaseSchemaNode(this) { Text = dbName });
            }

            result.Sort();
            return result;
        }

        public DbConnection OpenConnection()
        {
            if (ConnectionType == null)
                return null;

            try
            {
                var result = Activator.CreateInstance(ConnectionType) as DbConnection;
                if (result == null)
                    return result;

                result.ConnectionString = ConnectionString;
                result.Open();

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetSchema(string databaseName, string collectionName, string[] restrictionValues)
        {
            var connection = OpenConnection();
            if (connection == null)
                return null;

            try
            {
                if (databaseName != null)
                {
                    if (restrictionValues == null)
                        restrictionValues = new string[] { databaseName };
                    else if (restrictionValues != null && restrictionValues.Length > 0 && restrictionValues[0] == null)
                        restrictionValues[0] = databaseName;
                }

                if (databaseName != null)
                    connection.ChangeDatabase(databaseName);

                if (collectionName == null)
                    return connection.GetSchema();

                if (restrictionValues == null)
                    return connection.GetSchema(collectionName);

                return connection.GetSchema(collectionName, restrictionValues);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    #endregion

    #region DatabaseSchemaNode
    public class DatabaseSchemaNode: DbSchemaBaseNode
    {
        public DatabaseSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>()
            {
                new TableListSchemaNode(this),
                new StoredProceduresListSchemaNode(this),
                new FunctionListSchemaNode(this)
            };
            return result;
        }
    }
    #endregion

    #region TableListSchemaNode
    public class TableListSchemaNode : DbSchemaBaseNode
    {
        public TableListSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override string Text  => "Tables";
        public override bool CanDrag => false;

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();

            var tblTables = GetSchema("Tables");
            if (tblTables == null)
                return null;

            foreach (DataRow rowTable in tblTables.Rows)
            {
                string tblType   = Convert.ToString(rowTable["TABLE_TYPE"]);
                string tblName   = Convert.ToString(rowTable["TABLE_NAME"]);
                string tblSchema = Convert.ToString(rowTable["TABLE_SCHEMA"]);
                if (!string.IsNullOrEmpty(tblName))
                {
                    var node = new TableSchemaNode(this)
                    {
                        TableType   = tblType,
                        TableName   = tblName,
                        TableSchema = tblSchema
                    };
                    result.Add(node);
                }
            }

            result.Sort();
            return result;
        }
    }
    #endregion

    #region StoredProceduresListSchemaNode
    public class StoredProceduresListSchemaNode : DbSchemaBaseNode
    {
        public StoredProceduresListSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override string Text  => "Stored procedures";
        public override bool CanDrag => false;

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();

            var tblProcedures = GetSchema("Procedures");
            if (tblProcedures == null)
                return null;

            bool hasType      = tblProcedures.Columns.Contains("ROUTINE_TYPE");

            foreach (DataRow rowProcedure in tblProcedures.Rows)
            {
                string procType   = hasType ? Convert.ToString(rowProcedure["ROUTINE_TYPE"]) : null;
                string procName   = Convert.ToString(rowProcedure["SPECIFIC_NAME"]);
                string procSchema = Convert.ToString(rowProcedure["SPECIFIC_SCHEMA"]);
                if (!string.IsNullOrEmpty(procName) && (!hasType || (string.Compare(procType, "PROCEDURE") == 0)))
                {
                    var node = new StoredProcedureSchemaNode(this)
                    {
                        ProcedureType   = procType,
                        ProcedureName   = procName,
                        ProcedureSchema = procSchema
                    };
                    result.Add(node);
                }
            }

            result.Sort();
            return result;
        }
    }
    #endregion

    #region FunctionListSchemaNode
    public class FunctionListSchemaNode : DbSchemaBaseNode
    {
        public FunctionListSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override string Text  => "Functions";
        public override bool CanDrag => false;

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();

            var tblProcedures = GetSchema("Procedures");
            if (tblProcedures == null)
                return null;

            bool hasType = tblProcedures.Columns.Contains("ROUTINE_TYPE");

            foreach (DataRow rowProcedure in tblProcedures.Rows)
            {
                string procType   = hasType ? Convert.ToString(rowProcedure["ROUTINE_TYPE"]) : null;
                string procName   = Convert.ToString(rowProcedure["SPECIFIC_NAME"]);
                string procSchema = Convert.ToString(rowProcedure["SPECIFIC_SCHEMA"]);
                if (!string.IsNullOrEmpty(procName) && (!hasType || (string.Compare(procType, "FUNCTION") == 0)))
                {
                    var node = new FunctionSchemaNode(this)
                    {
                        FunctionType = procType,
                        FunctionName = procName,
                        FunctionSchema = procSchema
                    };
                    result.Add(node);
                }
            }

            result.Sort();
            return result;
        }
    }
    #endregion

    #region TableSchemaNode
    public class TableSchemaNode : DbSchemaBaseNode
    {
        public TableSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override string Text     => $"{TableSchema}.{TableName}";
        public override string DragText => $"{QuoteDatabaseObjectName(DbConnectionType, TableSchema)}.{QuoteDatabaseObjectName(DbConnectionType, TableName)}";

        public string TableType { get; set; }
        public string TableName { get; set; }
        public string TableSchema { get; set; }

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>()
            {
                new ColumnListSchemaNode(this),
                new IndexListSchemaNode(this)
            };
            return result;
        }
    }
    #endregion

    #region StoredProcedureSchemaNode
    public class StoredProcedureSchemaNode : DbSchemaBaseNode
    {
        public StoredProcedureSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public string ProcedureType { get; set; }
        public string ProcedureName { get; set; }
        public string ProcedureSchema { get; set; }

        public override string Text     => Text = $"{ProcedureSchema}.{ProcedureName}";
        public override string DragText => $"{QuoteDatabaseObjectName(DbConnectionType, ProcedureSchema)}.{QuoteDatabaseObjectName(DbConnectionType, ProcedureName)}";

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();

            var tblParameters = GetSchema("ProcedureParameters", new string[] { null, ProcedureSchema, ProcedureName });
            if (tblParameters == null)
                return null;

            foreach (DataRow rowColumn in tblParameters.Rows)
            {
                var parameterName = Convert.ToString(rowColumn["PARAMETER_NAME"]);

                int? maxLength = null;
                var strLength = Convert.ToString(rowColumn["CHARACTER_MAXIMUM_LENGTH"]);
                if (int.TryParse(strLength, out int iLength))
                    maxLength = iLength;

                if (string.IsNullOrWhiteSpace(parameterName) && tblParameters.Columns.Contains("IS_RESULT") &&
                    Convert.ToString(rowColumn["IS_RESULT"])?.ToUpper() == "YES")
                    parameterName = "<RESULT>";


                var node = new StoredProcedureParameterSchemaNode(this)
                {
                    ParameterName   = parameterName,
                    DataType        = Convert.ToString(rowColumn["DATA_TYPE"]),
                    MaxLength       = maxLength,
                    OrdinalPosition = Convert.ToInt32(rowColumn["ORDINAL_POSITION"])
                };
                result.Add(node);
            }

            result.Sort((x, y) => ((StoredProcedureParameterSchemaNode)x).OrdinalPosition.CompareTo(((StoredProcedureParameterSchemaNode)y).OrdinalPosition));
            return result;
        }
    }
    #endregion

    #region FunctionSchemaNode
    public class FunctionSchemaNode : DbSchemaBaseNode
    {
        public FunctionSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public string FunctionType { get; set; }
        public string FunctionName { get; set; }
        public string FunctionSchema { get; set; }

        public override string Text     => Text = $"{FunctionSchema}.{FunctionName}";
        public override string DragText => $"{QuoteDatabaseObjectName(DbConnectionType, FunctionSchema)}.{QuoteDatabaseObjectName(DbConnectionType, FunctionName)}";

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();

            var tblParameters = GetSchema("ProcedureParameters", new string[] { null, FunctionSchema, FunctionName });
            if (tblParameters == null)
                return null;

            foreach (DataRow rowColumn in tblParameters.Rows)
            {
                var parameterName = Convert.ToString(rowColumn["PARAMETER_NAME"]);

                int? maxLength = null;
                var strLength = Convert.ToString(rowColumn["CHARACTER_MAXIMUM_LENGTH"]);
                if (int.TryParse(strLength, out int iLength))
                    maxLength = iLength;

                if (string.IsNullOrWhiteSpace(parameterName) && tblParameters.Columns.Contains("IS_RESULT") &&
                    Convert.ToString(rowColumn["IS_RESULT"])?.ToUpper() == "YES")
                    parameterName = "<RESULT>";


                var node = new FunctionParameterSchemaNode(this)
                {
                    ParameterName   = parameterName,
                    DataType        = Convert.ToString(rowColumn["DATA_TYPE"]),
                    MaxLength       = maxLength,
                    OrdinalPosition = Convert.ToInt32(rowColumn["ORDINAL_POSITION"])
                };
                result.Add(node);
            }

            result.Sort((x, y) => ((FunctionParameterSchemaNode)x).OrdinalPosition.CompareTo(((FunctionParameterSchemaNode)y).OrdinalPosition));
            return result;
        }
    }
    #endregion

    #region StoredProcedureParameterSchemaNode
    public class StoredProcedureParameterSchemaNode: DbSchemaBaseNode
    {
        public StoredProcedureParameterSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public string ParameterName	{ get; set; }
        public string DataType		{ get; set; }
        public int? MaxLength		{ get; set; }
        public int OrdinalPosition	{ get; set; }

        public override string Text     => $"{ParameterName} {DataType}{(MaxLength.HasValue ? $"({(MaxLength.Value >= 0 ? Convert.ToString(MaxLength.Value) : "max")})" : string.Empty)}";
        public override string DragText => ParameterName;
    }
    #endregion

    #region FunctionParameterSchemaNode
    public class FunctionParameterSchemaNode: DbSchemaBaseNode
    {
        public FunctionParameterSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public string ParameterName	{ get; set; }
        public string DataType		{ get; set; }
        public int? MaxLength		{ get; set; }
        public int OrdinalPosition	{ get; set; }

        public override string Text     => $"{ParameterName} {DataType}{(MaxLength.HasValue ? $"({(MaxLength.Value >= 0 ? Convert.ToString(MaxLength.Value) : "max")})" : string.Empty)}";
        public override string DragText => ParameterName;
    }
    #endregion

    #region ColumnListSchemaNode
    public class ColumnListSchemaNode : DbSchemaBaseNode
    {
        public ColumnListSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override string Text  => "Columns";
        public override bool CanDrag => false;

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result  = new List<DbSchemaBaseNode>();
            var tblNode = FindParentNode<TableSchemaNode>();

            if (tblNode == null)
                return null;

            var tblColumns = GetSchema("Columns", new string[] { null, tblNode.TableSchema, tblNode.TableName });
            if (tblColumns == null)
                return null;

            foreach (DataRow rowColumn in tblColumns.Rows)
            {
                int? maxLength = null;
                var strLength = Convert.ToString(rowColumn["CHARACTER_MAXIMUM_LENGTH"]);
                if (int.TryParse(strLength, out int iLength))
                    maxLength = iLength;

                var node = new ColumnSchemaNode(this)
                {
                    ColumnName      = Convert.ToString(rowColumn["COLUMN_NAME"]),
                    DataType        = Convert.ToString(rowColumn["DATA_TYPE"]),
                    MaxLength       = maxLength,
                    OrdinalPosition = Convert.ToInt32(rowColumn["ORDINAL_POSITION"])
                };
                result.Add(node);
            }

            result.Sort((x, y) => ((ColumnSchemaNode)x).OrdinalPosition.CompareTo(((ColumnSchemaNode)y).OrdinalPosition));
            return result;
        }
    }
    #endregion

    #region IndexListSchemaNode
    public class IndexListSchemaNode : DbSchemaBaseNode
    {
        public IndexListSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override string Text  => "Indexes";
        public override bool CanDrag => false;

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();
            var tblNode = FindParentNode<TableSchemaNode>();

            if (tblNode == null)
                return null;

            var tblColumns = GetSchema("Indexes", new string[] { null, tblNode.TableSchema, tblNode.TableName });
            if (tblColumns == null)
                return null;

            foreach (DataRow rowColumn in tblColumns.Rows)
            {
                var node = new IndexSchemaNode(this)
                {
                    Text = Convert.ToString(rowColumn["CONSTRAINT_NAME"])
                };
                result.Add(node);
            }

            result.Sort();
            return result;
        }
    }
    #endregion

    #region ColumnSchemaNode
    public class ColumnSchemaNode : DbSchemaBaseNode
    {
        public ColumnSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public string ColumnName	{ get; set; }
        public string DataType		{ get; set; }
        public int? MaxLength		{ get; set; }
        public int OrdinalPosition	{ get; set; }

        public override string Text     => $"{ColumnName} {DataType}{(MaxLength.HasValue ? $"({(MaxLength.Value >= 0 ? Convert.ToString(MaxLength.Value) : "max")})" : string.Empty)}";
        public override string DragText => QuoteDatabaseObjectName(DbConnectionType, ColumnName);
    }
    #endregion

    #region IndexSchemaNode
    public class IndexSchemaNode : DbSchemaBaseNode
    {
        public IndexSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }

        public override List<DbSchemaBaseNode> ListChildNodes()
        {
            var result = new List<DbSchemaBaseNode>();
            var tblNode = FindParentNode<TableSchemaNode>();

            if (tblNode == null)
                return null;

            var tblColumns = GetSchema("IndexColumns", new string[] { null, tblNode.TableSchema, tblNode.TableName, Text });
            if (tblColumns == null)
                return null;

            foreach (DataRow rowColumn in tblColumns.Rows)
            {
                var node = new IndexColumnSchemaNode(this)
                {
                    Text = Convert.ToString(rowColumn["COLUMN_NAME"]),
                    OrdinalPosition = Convert.ToInt32(rowColumn["ORDINAL_POSITION"])
                };
                result.Add(node);
            }

            result.Sort((x, y) => ((IndexColumnSchemaNode)x).OrdinalPosition.CompareTo(((IndexColumnSchemaNode)y).OrdinalPosition));
            return result;
        }
    }
    #endregion

    #region IndexColumnSchemaNode
    public class IndexColumnSchemaNode: DbSchemaBaseNode
    {
        public int OrdinalPosition { get; set; }

        public IndexColumnSchemaNode(DbSchemaBaseNode parentNode) : base(parentNode) { }
    }
    #endregion
}
