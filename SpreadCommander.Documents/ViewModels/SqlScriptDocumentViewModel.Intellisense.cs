using SpreadCommander.Common.Code;
using SpreadCommander.Common.SqlScript;
using SpreadCommander.Documents.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common;

namespace SpreadCommander.Documents.ViewModels
{
    public partial class SqlScriptDocumentViewModel
    {
        public virtual void ListScriptIntellisenseItems(string text, string[] lines, Point caretPosition, ScriptIntellisense intellisense)
        {
            intellisense.UsePeriodInIntellisense = true;

            var script                = new SqlScript(text);
            string lastConnectionName = null;
            SqlScriptCommand command  = null;
            for (int i = 0; i < script.Commands.Count; i++)
            {
                if (script.Commands[i].StartLineNumber - 1 > caretPosition.Y)
                    break;

                command = script.Commands[i];
                lastConnectionName = command.GetConnectionName();
            }

            if (string.IsNullOrWhiteSpace(lastConnectionName) && _SelectedDbConnection == null)
                return;

            var itemCaptions = new Dictionary<(ScriptIntellisenseItem.IntellisenseItemType, string), int>();

            Connection conn = null;
            try
            {
                if (string.IsNullOrWhiteSpace(lastConnectionName))
                {
                    conn = _SelectedDbConnection.Factory.CreateConnection(_SelectedDbConnection.ConnectionString);
                }
                else
                {
                    var connections = DBConnections.LoadConnections();
                    var dbConn = connections.FindConnection(lastConnectionName);
                    if (dbConn != null)
                    {
                        conn = dbConn.GetConnectionFactory()?.CreateConnection(dbConn.ConnectionString);
                    }
                }

                if (conn == null)
                    return;

                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    return;
                }

                var parts   = new List<string>();
                var aliases = new StringNoCaseDictionary<TableAlias>();
                string dbName = conn.DbConnection.Database;

                if (!string.IsNullOrWhiteSpace(command?.Text))
                {
                    var reAlias = new Regex(@"(?i)\b(?:from|join)\s+(?:(?<Database>(\w+|\[.*?]\]|`.*?`|"".*?"")\.)?(?:(?<Schema>(?:\w+|\[.*?]\]|`.*?`|"".*?""))\.))?(?<Table>(?:\w+|\[.*?\]|`.*?`|"".*?""))[ \t]+(?<Alias>\w+)");
                    var collAliases = reAlias.Matches(command.Text);

                    if ((collAliases?.Count ?? 0) > 0)
                    {
                        foreach (Match matchAlias in collAliases)
                        {
                            if (!matchAlias.Success)
                                continue;

                            string alias         = matchAlias.Groups["Alias"].Value;
                            string aliasDatabase = Utils.NullString(Utils.UnquoteString(matchAlias.Groups["Database"].Value));
                            string aliasSchema   = Utils.NullString(Utils.UnquoteString(matchAlias.Groups["Schema"].Value));
                            string aliasTable    = Utils.NullString(Utils.UnquoteString(matchAlias.Groups["Table"].Value));

                            if (string.IsNullOrWhiteSpace(alias) || string.Compare(alias, "where", true) == 0 ||
                                string.Compare(alias, "order", true) == 0 || string.Compare(alias, "join", true) == 0 ||
                                string.Compare(alias, "left", true) == 0 || string.Compare(alias, "right", true) == 0 ||
                                string.Compare(alias, "inner", true) == 0 || string.Compare(alias, "outer", true) == 0 ||
                                string.Compare(alias, "full", true) == 0)
                                continue;

                            if (!string.IsNullOrWhiteSpace(aliasTable) && !aliases.ContainsKey(alias))
                                aliases.Add(alias, new TableAlias() { Alias = alias, Database = aliasDatabase, Schema = aliasSchema, Table = aliasTable });
                        }
                    }
                }

                string line = caretPosition.Y < (lines?.Length ?? 0) ? lines[caretPosition.Y] : null;
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (caretPosition.X < line.Length)
                        line = line.Substring(0, caretPosition.X);

                    var reParts = new Regex(@"((?<Part>(\w+|\[.*?]\]|`.*?`|"".*?""))\.)*([""`[]?(?<LastPart>\w*))$");
                    var matchParts = reParts.Match(line);
                    if (matchParts.Success)
                    {
                        foreach (Capture capture in matchParts.Groups["Part"].Captures)
                            parts.Add(capture.Value);
                    }
                }

                if (parts.Count == 0)
                    AddTablesAndProcedures(intellisense, itemCaptions, conn.DbConnection, null, null);
                else if (parts.Count == 1 && !string.IsNullOrWhiteSpace(parts[0]))
                {
                    var part0 = Utils.UnquoteString(parts[0]);
                    if (!string.IsNullOrWhiteSpace(part0))
                    {
                        AddTablesAndProcedures(intellisense, itemCaptions, conn.DbConnection, dbName, part0);
                        AddTablesAndProcedures(intellisense, itemCaptions, conn.DbConnection, part0, null);

                        AddTableColumns(intellisense, itemCaptions, conn.DbConnection, dbName, null, part0);

                        if (aliases.ContainsKey(part0))
                        {
                            var tableAlias = aliases[part0];
                            AddTableColumns(intellisense, itemCaptions, conn.DbConnection, tableAlias.Database ?? dbName, tableAlias.Schema, tableAlias.Table);
                        }
                    }
                }
                else if (parts.Count == 2 && !string.IsNullOrWhiteSpace(parts[0]))
                    AddTablesAndProcedures(intellisense, itemCaptions, conn.DbConnection, Utils.UnquoteString(parts[0]), Utils.UnquoteString(parts[1]));
                else
                    AddTablesAndProcedures(intellisense, itemCaptions, conn.DbConnection, null, null);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }


            static void AddTablesAndProcedures(ScriptIntellisense intellisense, Dictionary<(ScriptIntellisenseItem.IntellisenseItemType, string), int> itemCaptions,
                DbConnection connection, string databaseName, string schemaName)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(databaseName))
                        connection.ChangeDatabase(databaseName);

                    if (string.IsNullOrWhiteSpace(schemaName))
                        schemaName = null;

                    var dbConnectionType = connection.GetType();

                    var schemaTables = connection.GetSchema("Tables");
                    if (schemaTables != null)
                    {
                        foreach (DataRow rowTable in schemaTables.Rows)
                        {
                            var tableSchema = Convert.ToString(rowTable["TABLE_SCHEMA"]);
                            var tableName   = Convert.ToString(rowTable["TABLE_NAME"]);

                            if (!string.IsNullOrWhiteSpace(schemaName) && string.Compare(tableSchema, schemaName, true) != 0)
                                continue;

                            var caption = !string.IsNullOrWhiteSpace(tableSchema) ?
                                $"{DbSchemaBaseNode.QuoteDatabaseObjectName(dbConnectionType, tableSchema)}.{DbSchemaBaseNode.QuoteDatabaseObjectName(dbConnectionType, tableName)}" :
                                $"{DbSchemaBaseNode.QuoteDatabaseObjectName(dbConnectionType, tableName)}";

                            var item = new ScriptIntellisenseItem()
                            {
                                Caption     = caption,
                                Description = null,
                                ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Table,
                                Value       = caption
                            };
                            if (!itemCaptions.ContainsKey((ScriptIntellisenseItem.IntellisenseItemType.Table, caption)))
                            {
                                intellisense.Items.Add(item);
                                itemCaptions.Add((ScriptIntellisenseItem.IntellisenseItemType.Table, caption), 1);
                            }
                        }
                    }

                    var schemaProcedures          = connection.GetSchema("Procedures");
                    var schemaProcedureParameters = connection.GetSchema("ProcedureParameters");
                    if (schemaProcedures != null)
                    {
                        bool hasType = schemaProcedures.Columns.Contains("ROUTINE_TYPE");

                        foreach (DataRow rowProcedure in schemaProcedures.Rows)
                        {
                            string procType   = hasType ? Convert.ToString(rowProcedure["ROUTINE_TYPE"]) : null;
                            string procName   = Convert.ToString(rowProcedure["SPECIFIC_NAME"]);
                            string procSchema = Convert.ToString(rowProcedure["SPECIFIC_SCHEMA"]);

                            if (!string.IsNullOrWhiteSpace(schemaName) && string.Compare(procSchema, schemaName, true) != 0)
                                continue;

                            var caption = !string.IsNullOrWhiteSpace(procSchema) ?
                                $"{DbSchemaBaseNode.QuoteDatabaseObjectName(dbConnectionType, procSchema)}.{DbSchemaBaseNode.QuoteDatabaseObjectName(dbConnectionType, procName)}" :
                                $"{DbSchemaBaseNode.QuoteDatabaseObjectName(dbConnectionType, procName)}";

                            var description = caption;

                            if (schemaProcedureParameters != null)
                            {
                                try
                                {
                                    var strParameters = new StringBuilder(" (");

                                    using var viewProcedureParameters = new DataView(schemaProcedureParameters,
                                        $"SPECIFIC_SCHEMA='{procSchema.Replace("'", "''")}' and SPECIFIC_NAME='{procName.Replace("'", "''")}'",
                                        $"ORDINAL_POSITION", DataViewRowState.CurrentRows);
                                    string returnParameter = null;

                                    foreach (DataRowView rowParameter in viewProcedureParameters)
                                    {
                                        int position         = Convert.ToInt32(rowParameter["ORDINAL_POSITION"]);
                                        string paramName     = Convert.ToString(rowParameter["PARAMETER_NAME"]);
                                        string paramMode     = Convert.ToString(rowParameter["PARAMETER_MODE"])?.ToLower();
                                        string paramDataType = Convert.ToString(rowParameter["DATA_TYPE"]);

                                        int? paramMaxLength = null;
                                        var strLength = Convert.ToString(rowParameter["CHARACTER_MAXIMUM_LENGTH"]);
                                        if (int.TryParse(strLength, out int iLength))
                                            paramMaxLength = iLength;
                                        if (paramMaxLength.HasValue && string.Compare(paramDataType, "image") != 0)
                                            paramDataType = $"{paramDataType}({paramMaxLength.Value})";

                                        if (position == 0)
                                            returnParameter = paramDataType;
                                        else
                                        {
                                            if (strParameters.Length > 2)
                                                strParameters.Append(", ");

                                            strParameters.Append($"{paramMode} {paramName} {paramDataType}");
                                        }
                                    }

                                    strParameters.Append(')');

                                    if (!string.IsNullOrWhiteSpace(returnParameter))
                                        strParameters.Append(" returns ").Append(returnParameter);

                                    description += strParameters.ToString();
                                }
                                catch (Exception)
                                {
                                    description = null;
                                }
                            }

                            if (!string.IsNullOrEmpty(procName) && (!hasType || (string.Compare(procType, "PROCEDURE") == 0)))
                            {
                                var item = new ScriptIntellisenseItem()
                                {
                                    Caption     = caption,
                                    Description = HtmlEncode(description),
                                    ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Procedure,
                                    Value       = caption
                                };
                                if (!itemCaptions.ContainsKey((ScriptIntellisenseItem.IntellisenseItemType.Procedure, caption)))
                                {
                                    intellisense.Items.Add(item);
                                    itemCaptions.Add((ScriptIntellisenseItem.IntellisenseItemType.Procedure, caption), 1);
                                }
                            }
                            else if (!string.IsNullOrEmpty(procName) && (!hasType || (string.Compare(procType, "FUNCTION") == 0)))
                            {
                                var item = new ScriptIntellisenseItem()
                                {
                                    Caption     = caption,
                                    Description = HtmlEncode(description),
                                    ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Function,
                                    Value       = caption
                                };
                                if (!itemCaptions.ContainsKey((ScriptIntellisenseItem.IntellisenseItemType.Function, caption)))
                                {
                                    intellisense.Items.Add(item);
                                    itemCaptions.Add((ScriptIntellisenseItem.IntellisenseItemType.Function, caption), 1);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }

            static void AddTableColumns(ScriptIntellisense intellisense, Dictionary<(ScriptIntellisenseItem.IntellisenseItemType, string), int> itemCaptions,
                DbConnection connection, string databaseName, string schemaName, string tableName)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(databaseName))
                        connection.ChangeDatabase(databaseName);

                    if (string.IsNullOrWhiteSpace(schemaName))
                        schemaName = null;

                    var dbConnectionType = connection.GetType();

                    var schemaColumns = connection.GetSchema("Columns", new string[] { null, schemaName, tableName });
                    if (schemaColumns != null)
                    {
                        foreach (DataRow rowColumn in schemaColumns.Rows)
                        {
                            int? maxLength = null;
                            var strLength = Convert.ToString(rowColumn["CHARACTER_MAXIMUM_LENGTH"]);
                            if (int.TryParse(strLength, out int iLength))
                                maxLength = iLength;

                            var columnName = Convert.ToString(rowColumn["COLUMN_NAME"]);
                            var dataType   = Convert.ToString(rowColumn["DATA_TYPE"]);

                            if (maxLength.HasValue && string.Compare(dataType, "image") != 0 && string.Compare(dataType, "text") != 0)
                                dataType = $"{dataType}({(maxLength.Value == int.MaxValue ? "max" : maxLength.Value.ToString())})";

                            var node = new ScriptIntellisenseItem()
                            {
                                Caption     = columnName,
                                Description = HtmlEncode($"{columnName} {dataType}"),
                                ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Property,
                                Value       = DbSchemaBaseNode.QuoteDatabaseObjectName(dbConnectionType, columnName)
                            };
                            if (!itemCaptions.ContainsKey((ScriptIntellisenseItem.IntellisenseItemType.Property, columnName)))
                            {
                                intellisense.Items.Add(node);
                                itemCaptions.Add((ScriptIntellisenseItem.IntellisenseItemType.Property, columnName), 1);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }

            static string HtmlEncode(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return value;

                var result = value.Replace("<", "&lt;").Replace(">", "&gt;");
                return result;
            }
        }

        public virtual void ParseScriptErrors(string text, List<ScriptParseError> errors)
        {
            //No support for parsing SQL script.
        }
    }
}
