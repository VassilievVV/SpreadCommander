// Generated by TinyPG v1.3 available at www.codeproject.com

#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0066

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.Parsers.ConsoleScript
{
    #region ParseTree
    [Serializable]
    public class ParseErrors : List<ParseError>
    {
    }

    [Serializable]
    public class ParseError
    {
        private string message;
        private int code;
        private int line;
        private int col;
        private int pos;
        private int length;

        public int Code { get { return code; } }
        public int Line { get { return line; } }
        public int Column { get { return col; } }
        public int Position { get { return pos; } }
        public int Length { get { return length; } }
        public string Message { get { return message; } }

        // just for the sake of serialization
        public ParseError()
        {
        }

        public ParseError(string message, int code, ParseNode node) : this(message, code,  0, node.Token.StartPos, node.Token.StartPos, node.Token.Length)
        {
        }

        public ParseError(string message, int code, int line, int col, int pos, int length)
        {
            this.message = message;
            this.code = code;
            this.line = line;
            this.col = col;
            this.pos = pos;
            this.length = length;
        }
    }

    // rootlevel of the node tree
    [Serializable]
    public partial class ParseTree : ParseNode
    {
        public ParseErrors Errors;

        public List<Token> Skipped;

        public ParseTree() : base(new Token(), "ParseTree")
        {
            Token.Type = TokenType.Start;
            Token.Text = "Root";
            Errors = new ParseErrors();
        }

        public string PrintTree()
        {
            StringBuilder sb = new StringBuilder();
            int indent = 0;
            PrintNode(sb, this, indent);
            return sb.ToString();
        }

        private void PrintNode(StringBuilder sb, ParseNode node, int indent)
        {
            
            string space = "".PadLeft(indent, ' ');

            sb.Append(space);
            sb.AppendLine(node.Text);

            foreach (ParseNode n in node.Nodes)
                PrintNode(sb, n, indent + 2);
        }
        
        /// <summary>
        /// this is the entry point for executing and evaluating the parse tree.
        /// </summary>
        /// <param name="paramlist">additional optional input parameters</param>
        /// <returns>the output of the evaluation function</returns>
        public object Eval(params object[] paramlist)
        {
            return Nodes[0].Eval(this, paramlist);
        }
    }

    [Serializable]
    [XmlInclude(typeof(ParseTree))]
    public partial class ParseNode
    {
        protected string text;
        protected List<ParseNode> nodes;
        
        public List<ParseNode> Nodes { get {return nodes;} }
        
        [XmlIgnore] // avoid circular references when serializing
        public ParseNode Parent;
        public Token Token; // the token/rule

        [XmlIgnore] // skip redundant text (is part of Token)
        public string Text { // text to display in parse tree 
            get { return text;} 
            set { text = value; }
        } 

        public virtual ParseNode CreateNode(Token token, string text)
        {
            ParseNode node = new ParseNode(token, text);
            node.Parent = this;
            return node;
        }

        protected ParseNode(Token token, string text)
        {
            this.Token = token;
            this.text = text;
            this.nodes = new List<ParseNode>();
        }

        protected object GetValue(ParseTree tree, TokenType type, int index)
        {
            return GetValue(tree, type, ref index);
        }

        protected object GetValue(ParseTree tree, TokenType type, ref int index)
        {
            object o = null;
            if (index < 0) return o;

            // left to right
            foreach (ParseNode node in nodes)
            {
                if (node.Token.Type == type)
                {
                    index--;
                    if (index < 0)
                    {
                        o = node.Eval(tree);
                        break;
                    }
                }
            }
            return o;
        }

        /// <summary>
        /// this implements the evaluation functionality, cannot be used directly
        /// </summary>
        /// <param name="tree">the parsetree itself</param>
        /// <param name="paramlist">optional input parameters</param>
        /// <returns>a partial result of the evaluation</returns>
        internal object Eval(ParseTree tree, params object[] paramlist)
        {
            object Value;

            switch (Token.Type)
            {
                case TokenType.Start:
                    Value = EvalStart(tree, paramlist);
                    break;
                case TokenType.COMPUTED_RETURNTYPE:
                    Value = EvalCOMPUTED_RETURNTYPE(tree, paramlist);
                    break;
                case TokenType.Command:
                    Value = EvalCommand(tree, paramlist);
                    break;
                case TokenType.InsertFile:
                    Value = EvalInsertFile(tree, paramlist);
                    break;
                case TokenType.ViewFile:
                    Value = EvalViewFile(tree, paramlist);
                    break;
                case TokenType.FormatCondition:
                    Value = EvalFormatCondition(tree, paramlist);
                    break;
                case TokenType.ComputedColumn:
                    Value = EvalComputedColumn(tree, paramlist);
                    break;
                case TokenType.Table:
                    Value = EvalTable(tree, paramlist);
                    break;
                case TokenType.Relation:
                    Value = EvalRelation(tree, paramlist);
                    break;
                case TokenType.Connection:
                    Value = EvalConnection(tree, paramlist);
                    break;
                case TokenType.Parameter:
                    Value = EvalParameter(tree, paramlist);
                    break;
                case TokenType.ObjectName:
                    Value = EvalObjectName(tree, paramlist);
                    break;
                case TokenType.ColumnName:
                    Value = EvalColumnName(tree, paramlist);
                    break;
                case TokenType.TableName:
                    Value = EvalTableName(tree, paramlist);
                    break;
                case TokenType.TableWithColumns:
                    Value = EvalTableWithColumns(tree, paramlist);
                    break;
                case TokenType.RelationName:
                    Value = EvalRelationName(tree, paramlist);
                    break;
                case TokenType.ConnectionName:
                    Value = EvalConnectionName(tree, paramlist);
                    break;
                case TokenType.ParameterName:
                    Value = EvalParameterName(tree, paramlist);
                    break;
                case TokenType.FormatFilter:
                    Value = EvalFormatFilter(tree, paramlist);
                    break;
                case TokenType.PropertyName:
                    Value = EvalPropertyName(tree, paramlist);
                    break;
                case TokenType.PropertyValue:
                    Value = EvalPropertyValue(tree, paramlist);
                    break;
                case TokenType.Property:
                    Value = EvalProperty(tree, paramlist);
                    break;

                default:
                    Value = Token.Text;
                    break;
            }
            return Value;
        }

        protected virtual object EvalStart(ParseTree tree, params object[] paramlist)
        {
            var result = new List<BaseCommand>();
        
        	int i = 0;
        	while (true)
        	{
        		var command = (BaseCommand)this.GetValue(tree, TokenType.Command, i++);
        		if (command == null)
        			break;
        
        		result.Add(command);
        	}
        	
        	return result;
        }

        protected virtual object EvalCOMPUTED_RETURNTYPE(ParseTree tree, params object[] paramlist)
        {
            return this.GetValue(tree, TokenType.STRING, 0) ?? this.GetValue(tree, TokenType.INTEGER, 0) ?? this.GetValue(tree, TokenType.DECIMAL, 0) ?? this.GetValue(tree, TokenType.DATETIME, 0) ?? this.GetValue(tree, TokenType.BOOLEAN, 0);
        }

        protected virtual object EvalCommand(ParseTree tree, params object[] paramlist)
        {
            return this.GetValue(tree, TokenType.InsertFile, 0) ?? this.GetValue(tree, TokenType.ViewFile, 0) ?? this.GetValue(tree, TokenType.FormatCondition, 0) ?? this.GetValue(tree, TokenType.ComputedColumn, 0) ?? this.GetValue(tree, TokenType.Table, 0) ?? this.GetValue(tree, TokenType.Relation, 0) ?? this.GetValue(tree, TokenType.Connection, 0) ?? this.GetValue(tree, TokenType.Parameter, 0);
        }

        protected virtual object EvalInsertFile(ParseTree tree, params object[] paramlist)
        {
            var result = new InsertFile();
        	result.FileName = (string)this.GetValue(tree, TokenType.ObjectName, 0);
        
        	int i = 0;
        	while (true)
        	{
        		var prop = (Property)this.GetValue(tree, TokenType.Property, i++);
        		if (prop == null)
        			break;
        
        		if (!string.IsNullOrEmpty(prop.Name))
        			result.AddProperty(prop.Name, prop.Value);
        	}
        	
        	return result;
        }

        protected virtual object EvalViewFile(ParseTree tree, params object[] paramlist)
        {
            var result = new ViewFile();
        	result.FileName = (string)this.GetValue(tree, TokenType.ObjectName, 0);
        
        	int i = 0;
        	while (true)
        	{
        		var prop = (Property)this.GetValue(tree, TokenType.Property, i++);
        		if (prop == null)
        			break;
        
        		if (!string.IsNullOrEmpty(prop.Name))
        			result.AddProperty(prop.Name, prop.Value);
        	}
        	
        	return result;
        }

        protected virtual object EvalFormatCondition(ParseTree tree, params object[] paramlist)
        {
            var result = new FormatCondition();
        	result.ColumnName   = (string)this.GetValue(tree, TokenType.ColumnName, 0);
        	result.TableName    = (string)this.GetValue(tree, TokenType.TableName, 0);
        	result.FormatFilter = (string)this.GetValue(tree, TokenType.FormatFilter, 0);
        	
        	int i = 0;
        	while (true)
        	{
        		var prop = (Property)this.GetValue(tree, TokenType.Property, i++);
        		if (prop == null)
        			break;
        
        		if (!string.IsNullOrEmpty(prop.Name))
        			result.AddProperty(prop.Name, prop.Value);
        	}
        	
        	return result;
        }

        protected virtual object EvalComputedColumn(ParseTree tree, params object[] paramlist)
        {
            var result = new ComputedColumn();
        	result.ColumnName   = (string)this.GetValue(tree, TokenType.ColumnName, 0);
        	result.TableName    = (string)this.GetValue(tree, TokenType.TableName, 0);
        	result.ReturnType   = ComputedColumn.StringToReturnType((string)this.GetValue(tree, TokenType.COMPUTED_RETURNTYPE, 0));
        	result.Expression   = (string)this.GetValue(tree, TokenType.PropertyValue, 0);
        	
        	return result;
        }

        protected virtual object EvalTable(ParseTree tree, params object[] paramlist)
        {
            var result = new Table();
        	result.TableName = (string)this.GetValue(tree, TokenType.TableName, 0);
        
        	int i = 0;
        	while (true)
        	{
        		var prop = (Property)this.GetValue(tree, TokenType.Property, i++);
        		if (prop == null)
        			break;
        
        		if (!string.IsNullOrEmpty(prop.Name))
        			result.AddProperty(prop.Name, prop.Value);
        	}
        	
        	return result;
        }

        protected virtual object EvalRelation(ParseTree tree, params object[] paramlist)
        {
            var result = new Relation();
        	result.RelationName = (string)this.GetValue(tree, TokenType.RelationName, 0);
        
        	var table1 = (TableWithColumns)this.GetValue(tree, TokenType.TableWithColumns, 0);
        	var table2 = (TableWithColumns)this.GetValue(tree, TokenType.TableWithColumns, 1);
        
        	result.ParentTableName = table1.TableName;
        	result.ParentColumnNames.AddRange(table1.ColumnNames);
        
        	result.ChildTableName = table2.TableName;
        	result.ChildColumnNames.AddRange(table2.ColumnNames);
        
        	return result;
        }

        protected virtual object EvalConnection(ParseTree tree, params object[] paramlist)
        {
            var result = new SpreadCommander.Common.ScriptEngines.ConsoleCommands.Connection();
        	result.Name = (string)this.GetValue(tree, TokenType.ConnectionName, 0);
        
        	int i = 0;
        	while (true)
        	{
        		var prop = (Property)this.GetValue(tree, TokenType.Property, i++);
        		if (prop == null)
        			break;
        
        		if (!string.IsNullOrEmpty(prop.Name))
        			result.AddProperty(prop.Name, prop.Value);
        	}
        	
        	return result;
        }

        protected virtual object EvalParameter(ParseTree tree, params object[] paramlist)
        {
            var result = new SpreadCommander.Common.ScriptEngines.ConsoleCommands.Parameter();
        	result.Name = (string)this.GetValue(tree, TokenType.ParameterName, 0);
        
        	int i = 0;
        	while (true)
        	{
        		var prop = (Property)this.GetValue(tree, TokenType.Property, i++);
        		if (prop == null)
        			break;
        
        		if (!string.IsNullOrEmpty(prop.Name))
        			result.AddProperty(prop.Name, prop.Value);
        	}
        	
        	return result;
        }

        protected virtual object EvalObjectName(ParseTree tree, params object[] paramlist)
        {
            string result = nodes.Count > 0 ? (string)nodes[0].Eval(tree) : null;
        	if (result != null)
        		result = Utils.UnquoteString(result);
        	return result;
        }

        protected virtual object EvalColumnName(ParseTree tree, params object[] paramlist)
        {
            return this.GetValue(tree, TokenType.ObjectName, 0);
        }

        protected virtual object EvalTableName(ParseTree tree, params object[] paramlist)
        {
            return this.GetValue(tree, TokenType.ObjectName, 0);
        }

        protected virtual object EvalTableWithColumns(ParseTree tree, params object[] paramlist)
        {
            var result = new TableWithColumns();
        	result.TableName = (string)this.GetValue(tree, TokenType.TableName, 0);
        
        	int i = 0;
        	while (true)
        	{
        		var columnName = (string)this.GetValue(tree, TokenType.ColumnName, i++);
        		if (columnName == null)
        			break;
        
        		if (!string.IsNullOrWhiteSpace(columnName))
        			result.ColumnNames.Add(columnName);
        	}
        	
        	return result;
        }

        protected virtual object EvalRelationName(ParseTree tree, params object[] paramlist)
        {
            return this.GetValue(tree, TokenType.ObjectName, 0);
        }

        protected virtual object EvalConnectionName(ParseTree tree, params object[] paramlist)
        {
            return this.GetValue(tree, TokenType.ObjectName, 0);
        }

        protected virtual object EvalParameterName(ParseTree tree, params object[] paramlist)
        {
            return this.GetValue(tree, TokenType.ObjectName, 0);
        }

        protected virtual object EvalFormatFilter(ParseTree tree, params object[] paramlist)
        {
            var value = (string)this.GetValue(tree, TokenType.IDENT_STRING, 0) ?? (string)this.GetValue(tree, TokenType.IDENT_QUOTED, 0) ?? (string)this.GetValue(tree, TokenType.IDENT_STRING2, 0);
        
        	var result = Utils.UnquoteString(value);
        	return result;
        }

        protected virtual object EvalPropertyName(ParseTree tree, params object[] paramlist)
        {
            return (string)this.GetValue(tree, TokenType.IDENT, 0);
        }

        protected virtual object EvalPropertyValue(ParseTree tree, params object[] paramlist)
        {
            string value = nodes.Count > 0 ? (string)nodes[0].Eval(tree) : null;
        	value = Utils.UnquoteString(value);
        	return value;
        }

        protected virtual object EvalProperty(ParseTree tree, params object[] paramlist)
        {
            string name = (string)this.GetValue(tree, TokenType.PropertyName, 0);
        	if (name == null)
        		return new Property();
        
        	string value = (string)this.GetValue(tree, TokenType.PropertyValue, 0);
        
        	var result = new Property();
        	result.Name  = name;
        	result.Value = Utils.UnquoteString(value);
        
        	return result;
        }


    }
    
    #endregion ParseTree
}