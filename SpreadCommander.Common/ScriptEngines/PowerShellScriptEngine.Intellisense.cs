using DevExpress.Mvvm;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Automation = System.Management.Automation;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class PowerShellScriptEngine
    {
        #region AstNode
        protected class AstNode
        {
            public Ast Node { get; }

            public List<AstNode> ChildNodes { get; } = new ();

            public AstNode(Ast ast)
            {
                Node = ast;
            }
        }
        #endregion

        #region PowerShellCommand
        public class PowerShellCommand
        {
            public string Name { get; set; }

            public string Source { get; set; }

            public string ModuleName { get; set; }

            public string Description { get; set; }

            public string HelpFile { get; set; }


            public PowerShellCommand Clone()
            {
                var result = new PowerShellCommand()
                {
                    Name        = this.Name,
                    Source      = this.Source,
                    ModuleName  = this.ModuleName,
                    Description = this.Description,
                    HelpFile    = this.HelpFile
                };
                return result;
            }
        }
        #endregion

        public static readonly List<PowerShellCommand> PSCommands = new ();

        public static void RefreshAvailableCommandLets()
        {
            Task.Run(() => ListAvailableCommandLets());
        }

        private static void ListAvailableCommandLets()
        {
            lock (PSCommands)
            {
                if (PSCommands.Count > 0)
                    return;
            }

            var listCommands = new List<PowerShellCommand>();

            using (var runspace = CreateRunspace())
            {
                using var ps = Automation.PowerShell.Create();
                ps.Runspace = runspace;
                ps.AddCommand("get-command");

                var commands = ps.Invoke();

                foreach (var obj in commands)
                {
                    var name        = Convert.ToString(obj.Properties["Name"]?.Value);
                    var source      = Convert.ToString(obj.Properties["Source"]?.Value);
                    var moduleName  = Convert.ToString(obj.Properties["ModuleName"]?.Value);
                    var description = Convert.ToString(obj.Properties["Description"]?.Value);
                    var helpFile    = Convert.ToString(obj.Properties["HelpFile"]?.Value);

                    var command = new PowerShellCommand()
                    {
                        Name        = name,
                        Source      = source,
                        ModuleName  = moduleName,
                        Description = description,
                        HelpFile    = helpFile
                    };

                    listCommands.Add(command);
                }
            }

            listCommands.Sort((x, y) => StringLogicalComparer.Compare(x.Name, y.Name));

            lock (PSCommands)
            {
                PSCommands.Clear();
                PSCommands.AddRange(listCommands);
            }

            Messenger.Default.Send(new PSCmdletListChangedMessage());
        }

        protected static AstNode ParseScript(string fileName, string script)
        {
            var parser = Parser.ParseInput(script, fileName, out Token[] _, out ParseError[] _);
            var result = new AstNode(parser);

            AddChildCommands(parser.FindAll(ast => true, true));

            return result;


            void AddChildCommands(IEnumerable<Ast> childCommands)
            {
                var commands = new Dictionary<Ast, AstNode>
                {
                    [parser] = result
                };

                foreach (var command in childCommands)
                {
                    var node = new AstNode(command);
                    commands[command] = node;

                    if (result == null)
                        result = node;	//Result is first (top) node

                    var parentNode = command.Parent != null && commands.ContainsKey(command.Parent) ? commands[command.Parent] : result;
                    if (parentNode != null && parentNode != node)
                        parentNode.ChildNodes.Add(node);
                }
            }
        }

        public override void ListScriptIntellisenseItems(string fileName, string text, string[] lines, Point caretPosition, ScriptIntellisense intellisense)
        {
            Point caretPos = caretPosition;

            //Make caretPosition 1-based.
            caretPos.X++;
            caretPos.Y++;

            var rootNode = ParseScript(fileName, text);
            if (rootNode == null)
            {
                ListCommandlets();
                return;
            }

            var usingNodes = new List<AstNode>();
            FindAllNodes(rootNode, typeof(UsingStatementAst), usingNodes);

            var usingNamespaces = new List<string>
            {
                "System"
            };
            foreach (var usingNode in usingNodes)
            {
                var node = usingNode.Node as UsingStatementAst;
                if (node.UsingStatementKind == UsingStatementKind.Namespace)
                {
                    if (!Utils.ContainsString(usingNamespaces, node.Name.Value, StringComparison.CurrentCultureIgnoreCase))
                        usingNamespaces.Add(node.Name.Value);
                }
            }

            var commandNode = FindNodeUnderCaret(rootNode, typeof(CommandAst));
            if (commandNode != null && commandNode.ChildNodes.Count > 0 && commandNode.ChildNodes[0].Node is StringConstantExpressionAst commandNameAst)
            {
                var commandName = commandNameAst.Value;

                if (!string.IsNullOrWhiteSpace(commandName))
                    intellisense.Help = new CmdletIntellisenseHelp(commandName);

                int unnamedParameterPosition          = 0;
                int parameterUnderCaret               = -1;
                string parameterNameUnderCaret        = null;
                bool isInsideNamedParameter           = false;
                bool isInsideNamedParameterValue      = false;
                Type parameterEnumType                = null;
                CommandParameterInfo currentParameter = null;
                var unnamedParameters                 = new List<object>();

                for (int i = 1; i < commandNode.ChildNodes.Count; i++)  //0 is command name, next nodes - parameters
                {
                    var commandChildNode = commandNode.ChildNodes[i];
                    bool isCaretInNode = IsCaretInAstNode(commandChildNode);

                    switch (commandChildNode.Node)
                    {
                        case ConstantExpressionAst constantAst:
                            if (isCaretInNode)
                                parameterUnderCaret = unnamedParameterPosition;

                            unnamedParameterPosition++;
                            unnamedParameters.Add(constantAst.Value);
                            break;
                        case VariableExpressionAst variableAst:
                            if (isCaretInNode)
                            {
                                if (!variableAst.Splatted)
                                    parameterUnderCaret = unnamedParameterPosition;
                            }

                            unnamedParameterPosition++;
                            unnamedParameters.Add(null);	//Variable value is not known at parse time.
                            break;
                        case CommandParameterAst commandParameterAst:
                            if (isCaretInNode)
                            {
                                isInsideNamedParameter      = true;
                                isInsideNamedParameterValue = IsCaretInExtent(commandParameterAst.Argument?.Extent);

                                if (isInsideNamedParameterValue && (Utils.GetUnderlyingType(commandParameterAst.Argument.StaticType)?.IsEnum ?? false))
                                    parameterEnumType = Utils.GetUnderlyingType(commandParameterAst.Argument.StaticType);

                                if (!string.IsNullOrWhiteSpace(commandParameterAst.ParameterName))
                                {
                                    parameterNameUnderCaret = commandParameterAst.ParameterName;
                                }
                            }
                            break;
                    }

                    if (isCaretInNode)
                        break;
                }

                var parameters = ListCommandParameters(commandName, unnamedParameters.ToArray());

                if (currentParameter == null && !string.IsNullOrWhiteSpace(parameterNameUnderCaret))
                {
                    currentParameter = parameters.FirstOrDefault(p => string.Compare(p.Name, parameterNameUnderCaret) == 0);
                }
                if (currentParameter != null)
                {
                    var parameterType = Utils.GetUnderlyingType(currentParameter?.ParameterType);
                    if (parameterEnumType == null && (parameterType?.IsEnum ?? false))
                        parameterEnumType = parameterType;
                }
                else if (currentParameter == null && parameterUnderCaret >= 0)
                {
                    currentParameter = parameters.FirstOrDefault(p => p.Position == parameterUnderCaret);
                    var parameterType = Utils.GetUnderlyingType(currentParameter?.ParameterType);
                    if (parameterEnumType == null && (parameterType?.IsEnum ?? false))
                        parameterEnumType = parameterType;
                }

                //Special case for colors. Storing colors as string allows to use HTML colors etc.
                if (!string.IsNullOrWhiteSpace(currentParameter?.Name) && currentParameter.ParameterType == typeof(string) &&
                    currentParameter.Name.ToLower().Contains("color"))
                    parameterEnumType = typeof(KnownColor);

                if (parameterEnumType?.IsEnum ?? false)
                    AddEnumToIntellisense(parameterEnumType);
                else if (isInsideNamedParameter)
                {
                    if (isInsideNamedParameterValue)
                        AddEnumToIntellisense(parameterEnumType);
                    else
                        AddParametersToIntellisense(parameters);
                }
                else
                {
                    var currentUnnamedParameter = parameters.FirstOrDefault(p => p.Position == unnamedParameterPosition);
                    if (currentUnnamedParameter != null)
                    {
                        //Unnamed parameter can be entered at caret position
                        var currentUnnamedParameterType = Utils.GetUnderlyingType(currentUnnamedParameter.ParameterType);
                        if (currentUnnamedParameterType?.IsEnum ?? false)
                        {
                            AddEnumToIntellisense(currentUnnamedParameterType);
                            if (!(currentUnnamedParameter?.IsMandatory ?? false))
                                AddParametersToIntellisense(parameters);
                        }
                        else
                            AddParametersToIntellisense(parameters);
                    }
                    else 
                        AddParametersToIntellisense(parameters);
                }

                if (intellisense.Items.Count <= 0)
                    ListCommandlets();

                return;
            }


            var variables = new List<AstNode>();
            FindAllNodes(rootNode, typeof(VariableExpressionAst), variables);

            var variableTypes = new StringNoCaseDictionary<Type>();	//Case-insensitive keys
            foreach (var variable in variables)
            {
                var node = (VariableExpressionAst)variable.Node;
                if (node.VariablePath.IsVariable)
                    variableTypes[node.VariablePath.UserPath] = null;
            }

            var assignments = new List<AstNode>();
            FindAllNodes(rootNode, typeof(AssignmentStatementAst), assignments);
            foreach (var assignment in assignments)
            {
                var node = (AssignmentStatementAst)assignment.Node;
                if (node.Operator != TokenKind.Equals)
                    continue;

                if (node.Left is ConvertExpressionAst convertExpr && convertExpr.StaticType != null)
                {
                    //[int]$i = 10;
                    var leftNode = assignment.ChildNodes.FirstOrDefault(n => n.Node == node.Left);
                    if (leftNode?.ChildNodes.FirstOrDefault(n => n.Node is VariableExpressionAst).Node is VariableExpressionAst varNode &&
                        varNode.VariablePath.IsVariable && !varNode.IsConstantVariable() &&
                        (variableTypes.ContainsKey(varNode.VariablePath.UserPath) && variableTypes[varNode.VariablePath.UserPath] == null))
                    {
                        var variableName = varNode.VariablePath.UserPath;
                        if (string.Compare(variableName, "$true", true) == 0 || string.Compare(variableName, "$false", true) == 0 ||
                            string.Compare(variableName, "$null", true) == 0)
                            continue;
                        variableTypes[variableName] = convertExpr.StaticType;
                    }
                }
                else if (node.Left is VariableExpressionAst variable && variable.VariablePath.IsVariable && !variable.IsConstantVariable() &&
                    (variableTypes.ContainsKey(variable.VariablePath.UserPath) && variableTypes[variable.VariablePath.UserPath] == null))
                {
                    //$i = [int]SomeFunction();
                    var variableName = variable.VariablePath.UserPath;
                    if (string.Compare(variableName, "true", true) == 0 || string.Compare(variableName, "false", true) == 0 ||
                        string.Compare(variableName, "null", true) == 0)
                        continue;

                    if (node.Right is CommandExpressionAst command)
                    {
                        var cmdNode = assignment.ChildNodes.FirstOrDefault(n => n.Node == command);
                        if (cmdNode != null && cmdNode.ChildNodes.Count > 0)
                        {
                            if (cmdNode.ChildNodes[0].Node is ConvertExpressionAst convert && convert.StaticType != null)
                                variableTypes[variableName] = convert.StaticType;
                            else if (cmdNode.ChildNodes[0].Node is InvokeMemberExpressionAst invoke && invoke.Expression is TypeExpressionAst typeAst)
                            {
                                var typeName = typeAst.TypeName.FullName;
                                var type     = FindType(typeName);
                                if (type != null)
                                    variableTypes[variableName] = type;
                            }
                        }
                    }
                    else if (node.Right is PipelineAst pipeline)
                    {
                        var pipelineNode = assignment.ChildNodes.FirstOrDefault(n => n.Node == pipeline);
                        if (pipelineNode != null && pipelineNode.ChildNodes.Count > 0 &&
                            pipelineNode.ChildNodes[^1].Node is CommandAst cmdPipeline &&
                            cmdPipeline.CommandElements.Count >= 2 && cmdPipeline.CommandElements[0] is StringConstantExpressionAst cmdName &&
                            string.Compare(cmdName.Value, "new-object", false) == 0 && cmdPipeline.CommandElements[1] is StringConstantExpressionAst cmdNewObjectType)
                        {
                            var type = FindType(cmdNewObjectType.Value);
                            if (type != null)
                                variableTypes[variableName] = type;
                        }
                    }
                }
            }

            //$ds.Tables - property
            var memberNode = FindNodeUnderCaret(rootNode, typeof(MemberExpressionAst));
            //$ds.Tables.Add() - method
            if (memberNode == null)
                memberNode = FindNodeUnderCaret(rootNode, typeof(InvokeMemberExpressionAst));
            if (memberNode != null)
            {
                var node = memberNode.Node as MemberExpressionAst;

                var propertyNames       = new List<string>();
                ExpressionAst childNode = node;
                bool listProperties     = false;    //list only properties under and before caret

                while (childNode != null && childNode is MemberExpressionAst childMemberNode)
                {
                    if (childMemberNode.Member is ConstantExpressionAst constMember)
                    {
                        if (listProperties)
                            propertyNames.Add(Convert.ToString(constMember.Value));
                        listProperties |= IsCaretInExtent(constMember.Extent);
                    }

                    childNode = childMemberNode.Expression;
                    listProperties |= IsCaretInExtent(childNode.Extent);
                }

                Type type = null;
                bool isStatic = false;
                if (childNode is VariableExpressionAst variableAst)
                {
                    type = variableTypes[variableAst.VariablePath.UserPath];
                }
                else if (childNode is TypeExpressionAst typeAst)
                {
                    isStatic     = true;
                    var typeName = typeAst.TypeName.Name;
                    type         = FindType(typeName);
                }

                if (type != null)
                {
                    for (int i = propertyNames.Count-1; i >= 0; i--)
                    {
                        type = GetPropertyType(type, propertyNames[i], isStatic);
                        if (type == null)
                            break;
                    }

                    if (type != null)
                        AddTypePropertiesAndMethodsToIntellisense(type, isStatic);
                }

                return;
            }

            AddVariablesToIntellisense(variableTypes);

            var variableUnderCaret = FindNodeUnderCaret(rootNode, typeof(VariableExpressionAst));
            if (variableUnderCaret == null)
                ListCommandlets();

            bool IsCaretInAstNode(AstNode node) =>
                IsCaretInExtent(node.Node.Extent);

            bool IsCaretInExtent(IScriptExtent extent)
            {
                if (extent == null)
                    return false;

                if (extent.StartLineNumber == extent.EndLineNumber && extent.StartLineNumber == caretPos.Y)
                    return (caretPos.X >= extent.StartColumnNumber && caretPos.X <= extent.EndColumnNumber);
                else if (caretPos.Y > extent.StartLineNumber && caretPos.Y < extent.EndLineNumber)
                    return true;
                else if (caretPos.Y == extent.StartLineNumber && caretPos.Y < extent.EndLineNumber)
                    return (caretPos.X >= extent.StartColumnNumber);
                else if (caretPos.Y > extent.StartLineNumber && caretPos.Y == extent.EndLineNumber)
                    return (caretPos.X <= extent.EndColumnNumber);

                return false;
            }

            AstNode FindNodeUnderCaret(AstNode parentNode, Type nodeType)
            {
                foreach (var childNode in parentNode.ChildNodes)
                {
                    if (IsCaretInAstNode(childNode))
                    {
                        AstNode topResultNode = null;
                        if (childNode.Node.GetType() == nodeType)
                            topResultNode = childNode;

                        //Try to find lowest result node.
                        var resultNode = FindNodeUnderCaret(childNode, nodeType);
                        return resultNode ?? topResultNode;
                    }
                }

                return null;
            }

            static void FindAllNodes(AstNode parentNode, Type nodeType, List<AstNode> nodes)
            {
                foreach (var childNode in parentNode.ChildNodes)
                {
                    if (childNode.Node.GetType() == nodeType)
                        nodes.Add(childNode);

                    FindAllNodes(childNode, nodeType, nodes);
                }
            }

            void ListCommandlets()
            {
                lock (PSCommands)
                {
                    if (intellisense.Help == null)
                        intellisense.Help = new CmdletIntellisenseHelp(null);

                    foreach (var command in PSCommands)
                    {
                        var item = new ScriptIntellisenseItem()
                        {
                            Caption     = command.Name,
                            Value       = command.Name,
                            ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Command,
                            Description = $"<b>Module:</b> { HtmlEncode(command.ModuleName) }<br>{ HtmlEncode(command.Description) }"
                        };
                        intellisense.Items.Add(item);
                    }
                }
            }

            static List<CommandParameterInfo> ListCommandParameters(string commandName, object[] unnamedArguments)
            {
                var result = new List<CommandParameterInfo>();

                try
                {
                    using var runspace = CreateRunspace();
                    using (var ps = Automation.PowerShell.Create())
                    {
                        ps.Runspace = runspace;

                        ps.AddCommand("get-command").AddArgument(commandName);
                        if ((unnamedArguments?.Length ?? 0) > 0)
                        {
                            for (int i = 0; i < unnamedArguments.Length; i++)
                                if (unnamedArguments[i] is string str && str == "-")
                                    unnamedArguments[i] = null;

                            ps.AddParameter("ArgumentList", unnamedArguments);
                        }

                        var commands = ps.Invoke();
                        if (commands.Count > 0)
                        {
                            var command = commands[0];
                            var parameterSets = command.Properties["ParameterSets"];
                            if (parameterSets?.Value is IList listParameterSets)
                            {
                                var parameters = new StringNoCaseDictionary<int>();

                                foreach (CommandParameterSetInfo parameterSet in listParameterSets)
                                {
                                    foreach (var parameter in parameterSet.Parameters)
                                    {
                                        if (parameters.ContainsKey(parameter.Name))
                                            continue;
                                        parameters[parameter.Name] = 1;

                                        result.Add(parameter);
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
                catch (Exception)
                {
                    return result;
                }
            }

            void AddParametersToIntellisense(List<CommandParameterInfo> parameters)
            {
                foreach (var parameter in parameters)
                {
                    var typeName = Utils.GetTypeName(parameter.ParameterType ?? typeof(object));
                    var name     = parameter.Name;
                    var caption  = $"{name} [{typeName}]";
                    string value = (parameter.ParameterType == typeof(SwitchParameter)) ? $"-{name}" : $"-{name}:";

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        var item = new ScriptIntellisenseItem()
                        {
                            Caption     = caption,
                            Value       = value,
                            Position    = parameter.Position >= 0 ? (int?)parameter.Position : null,
                            IsMandatory = parameter.IsMandatory,
                            ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Property,
                            Description = HtmlEncode(parameter.HelpMessage)
                        };
                        intellisense.Items.Add(item);
                    }
                }
            }

            void AddEnumToIntellisense(Type enumType)
            {
                enumType = Utils.GetUnderlyingType(enumType);
                
                if (!(enumType?.IsEnum ?? false))
                    return;

                var names = Enum.GetNames(enumType);
                if (names == null || names.Length <= 0)
                    return;

                foreach (var name in names)
                {
                    var item = new ScriptIntellisenseItem()
                    {
                        Caption  = name,
                        Value    = name,
                        ItemType = (int)ScriptIntellisenseItem.IntellisenseItemType.EnumValue
                    };
                    intellisense.Items.Add(item);
                }
            }

            void AddVariablesToIntellisense(Dictionary<string, Type> variableWithTypes)
            {
                if (variableWithTypes == null)
                    return;

                foreach (var pair in variableTypes)
                {
                    var caption = $"${pair.Key}";
                    if (pair.Value != null)
                    {
                        var typeName = Utils.GetTypeName(pair.Value);
                        caption      = !string.IsNullOrWhiteSpace(typeName) ? $"${pair.Key} [{typeName}]" : pair.Key;
                    }

                    var item = new ScriptIntellisenseItem()
                    {
                        Caption     = caption,
                        Value       = $"${pair.Key}",
                        ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Variable,
                        Description = HtmlEncode(caption)
                    };
                    intellisense.Items.Add(item);
                }
            }

            Type FindSimpleType(string typeName)
            {
                if (string.IsNullOrWhiteSpace(typeName))
                    return null;

                var type = Utils.GetType(typeName, true);
                if (type != null)
                    return type;

                for (int i = 0; type == null && i < usingNamespaces.Count; i++)
                {
                    var typeNameWithNamespace = $"{usingNamespaces[i]}.{typeName}";
                    type = Utils.GetType(typeNameWithNamespace, true);
                    if (type != null)
                        return type;
                }

                return null;
            }

            Type FindType(string typeName)
            {
                try
                {
                    var reTypeName = new Regex(@"^(?<TypeName>[\w\.]+)\s*(\[((?<GenType>[\w\.]+\??),?\s*)+\])?(?<Nullable>\?)?$");

                    var match = reTypeName.Match(typeName);
                    if (!match.Success)
                        return null;

                    var rootTypeName = match.Groups["TypeName"].Value;
                    if (match.Groups["Nullable"].Success)
                        rootTypeName += '?';

                    var genTypeNames = new List<string>();
                    foreach (Capture genTypeNameCapture in match.Groups["GenType"].Captures)
                        genTypeNames.Add(genTypeNameCapture.Value);

                    rootTypeName = UpdateTypeName(rootTypeName);
                    for (int i = 0; i < genTypeNames.Count; i++)
                        genTypeNames[i] = UpdateTypeName(genTypeNames[i]);

                    if (genTypeNames.Count > 0)
                        rootTypeName = $"{rootTypeName}`{genTypeNames.Count}";

                    var result = FindSimpleType(rootTypeName);
                    if (result == null)
                        return null;

                    var genTypes = new List<Type>();
                    foreach (var genTypeName in genTypeNames)
                    {
                        var genType = FindSimpleType(genTypeName);
                        if (genType == null)
                            return null;

                        genTypes.Add(genType);
                    }

                    if (genTypeNames.Count > 0)
                        result = result.MakeGenericType(genTypes.ToArray());
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            static Type GetPropertyType(Type type, string propertyName, bool isStatic)
            {
                if (type == null)
                    return null;

                var bindingFlags = isStatic ?
                    BindingFlags.Static | BindingFlags.Public :
                    BindingFlags.Instance | BindingFlags.Public;

                var propertyInfo = type.GetProperty(propertyName, bindingFlags);
                if (propertyInfo == null)
                    return null;

                return propertyInfo.PropertyType;
            }

            void AddTypePropertiesAndMethodsToIntellisense(Type type, bool staticPropertiesMethods)
            {
                if (type == null)
                    return;

                type = Utils.GetUnderlyingType(type);

                if (intellisense.Help == null)
                    intellisense.Help = new TypeIntellisenseHelp(type, staticPropertiesMethods);

                if (staticPropertiesMethods && type.IsEnum)
                {
                    AddEnumToIntellisense(type);
                    return;
                }

                if (staticPropertiesMethods)
                {
                    var constructors = type.GetConstructors();
                    foreach (var constructor in constructors)
                    {
                        var caption = "new";

                        var parameters = constructor.GetParameters();
                        var strParameters = new StringBuilder();
                        strParameters.Append(" (");

                        foreach (var parameter in parameters)
                        {
                            var paramTypeName = Utils.GetTypeName(parameter.ParameterType);
                            if (string.IsNullOrWhiteSpace(paramTypeName))
                                paramTypeName = "object";

                            if (strParameters.Length > 2)   // " (" was added initially
                                strParameters.Append(", ");

                            if (parameter.ParameterType.IsByRef)
                                strParameters.Append(parameter.IsOut ? "out " : "ref ");
                            strParameters.Append($"[{paramTypeName}] {parameter.Name}");
                        }

                        strParameters.Append(')');

                        caption += strParameters.ToString();

                        var attributeDescription = constructor.GetCustomAttribute<DescriptionAttribute>(true);
                        var description = $"<b>Constructor</b><br>{HtmlEncode(attributeDescription?.Description)}";

                        var item = new ScriptIntellisenseItem()
                        {
                            Caption     = caption,
                            Value       = "new",
                            ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Constructor,
                            Description = description
                        };
                        intellisense.Items.Add(item);
                    }
                }

                var bindingFlags = staticPropertiesMethods ?
                    BindingFlags.Static | BindingFlags.Public :
                    BindingFlags.Instance | BindingFlags.Public;

                var properties = type.GetProperties(bindingFlags);
                foreach (var property in properties)
                {
                    if (property.MemberType != MemberTypes.Property || property.IsSpecialName)
                        continue;

                    var attributeBrowsable = property.GetCustomAttribute<BrowsableAttribute>(true);
                    if (!(attributeBrowsable?.Browsable ?? true))
                        continue;

                    var typeName = Utils.GetTypeName(property.PropertyType);
                    var strParameters = new StringBuilder();
                    var parameters = property.GetIndexParameters();
                    if (parameters.Length > 0)
                    {
                        strParameters.Append(" [");
                        foreach (var parameter in parameters)
                        {
                            var paramTypeName = parameters[0].ParameterType;
                            if (strParameters.Length > 2)	// "[ " was added initially
                                strParameters.Append(", ");
                            if (parameter.ParameterType.IsByRef)
                                strParameters.Append(parameter.IsOut ? "out " : "ref ");
                            strParameters.Append($"[{paramTypeName}] {parameter.Name}");
                        }
                        strParameters.Append(']');
                    }

                    var caption = !string.IsNullOrWhiteSpace(typeName) ? $"{property.Name} [{typeName}]" : property.Name;
                    var attributeDescription = property.GetCustomAttribute<DescriptionAttribute>(true);
                    var description = $"<b>{HtmlEncode(property.Name)}</b> [{HtmlEncode(typeName)}]<br>{HtmlEncode(attributeDescription?.Description)}";

                    var item = new ScriptIntellisenseItem()
                    {
                        Caption     = caption,
                        Value       = property.Name,
                        ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Property,
                        Description = description
                    };
                    intellisense.Items.Add(item);
                }

                var methods = type.GetMethods(bindingFlags | BindingFlags.InvokeMethod);
                foreach (var method in methods)
                {
                    if (method.IsConstructor || method.MemberType != MemberTypes.Method || method.IsSpecialName || IsPropertyMethod(method))
                        continue;

                    var attributeBrowsable = method.GetCustomAttribute<BrowsableAttribute>(true);
                    if (!(attributeBrowsable?.Browsable ?? true))
                        continue;

                    var caption = $"{method.Name} ";
                    string typeName = "object";
                    if (method.ReturnType != typeof(void))
                    {
                        typeName = Utils.GetTypeName(method.ReturnType);
                        caption  = !string.IsNullOrWhiteSpace(typeName) ? $"{method.Name} [{typeName}]" : method.Name;
                    }

                    var parameters = method.GetParameters();
                    var strParameters = new StringBuilder();
                    strParameters.Append(" (");

                    foreach (var parameter in parameters)
                    {
                        var paramTypeName = Utils.GetTypeName(parameter.ParameterType);
                        if (string.IsNullOrWhiteSpace(paramTypeName))
                            paramTypeName = "object";

                        if (strParameters.Length > 2)	// " (" was added initially
                            strParameters.Append(", ");

                        if (parameter.ParameterType.IsByRef)
                            strParameters.Append(parameter.IsOut ? "out " : "ref ");
                        strParameters.Append($"[{paramTypeName}] {parameter.Name}");
                    }

                    strParameters.Append(')');

                    caption += strParameters.ToString();
                    var attributeDescription = method.GetCustomAttribute<DescriptionAttribute>(true);
                    var description = $"<b>{HtmlEncode(method.Name)}</b> [{HtmlEncode(typeName)}] {HtmlEncode(strParameters.ToString())}<br>{HtmlEncode(attributeDescription?.Description)}";

                    var item = new ScriptIntellisenseItem()
                    {
                        Caption     = caption,
                        Value       = $"{method.Name}(",
                        ItemType    = ScriptIntellisenseItem.IntellisenseItemType.Function,
                        Description = description
                    };
                    intellisense.Items.Add(item);
                }
            }

            static string UpdateTypeName(string initialTypeName)
            {
                if (string.IsNullOrWhiteSpace(initialTypeName))
                    return null;

                initialTypeName = initialTypeName.Trim();
                bool isNullable = initialTypeName.EndsWith("?");

                if (isNullable)
                    initialTypeName = initialTypeName[0..^1].Trim();
                if (string.IsNullOrWhiteSpace(initialTypeName))
                    return null;

                var resultTypeName = initialTypeName;

                switch (initialTypeName.ToLower())
                {
                    case "dbnull":
                        resultTypeName = "System.DBNull";
                        break;
                    case "bool":
                        resultTypeName = "System.Boolean";
                        break;
                    case "char":
                        resultTypeName = "System.Char";
                        break;
                    case "sbyte":
                        resultTypeName = "System.SByte";
                        break;
                    case "byte":
                        resultTypeName = "System.Byte";
                        break;
                    case "short":
                        resultTypeName = "System.Int16";
                        break;
                    case "ushort":
                        resultTypeName = "System.UInt16";
                        break;
                    case "int":
                        resultTypeName = "System.Int32";
                        break;
                    case "uint":
                        resultTypeName = "System.UInt32";
                        break;
                    case "long":
                        resultTypeName = "System.Int64";
                        break;
                    case "ulong":
                        resultTypeName = "System.UInt64";
                        break;
                    case "float":
                        resultTypeName = "System.Single";
                        break;
                    case "double":
                        resultTypeName = "System.Double";
                        break;
                    case "decimal":
                        resultTypeName = "System.Decimal";
                        break;
                    case "datetime":
                        resultTypeName = "System.DateTime";
                        break;
                    case "string":
                        resultTypeName = "System.String";
                        break;
                }

                if (isNullable)
                    resultTypeName = $"System.Nullable`1[{resultTypeName}]";

                return resultTypeName;
            }

            static string HtmlEncode(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return value;

                var result = value.Replace("<", "&lt;").Replace(">", "&gt;");
                return result;
            }

            static bool IsPropertyMethod(MethodInfo method)
            {
                foreach (var methodProperty in method.DeclaringType.GetProperties())
                {
                    if (methodProperty.GetMethod == method || methodProperty.SetMethod == method)
                        return true;
                }

                return false;
            }
        }
    }
}
