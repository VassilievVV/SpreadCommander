using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
	public class TypeIntellisenseHelp : ScriptIntellisenseHelp
	{
		private Type Type { get; }
		private bool IsStatic { get; }

		public TypeIntellisenseHelp(Type type, bool isStatic)
		{
			Type = type ?? throw new ArgumentNullException(nameof(type), "Parameter Type is required.");
			IsStatic = isStatic;
		}

		public override bool SupportsHelp       => true;
		public override bool SupportsOnlineHelp => IsFrameworkAssembly;


		public bool IsFrameworkAssembly
		{
			get
			{
				var type = Utils.GetUnderlyingType(Type);
				if (type == null && type.Assembly == null)
					return false;

				var attrProduct = type.Assembly.GetCustomAttribute<AssemblyProductAttribute>();
				if (attrProduct == null)
					return false;

				return string.Compare(attrProduct.Product, "Microsoft® .NET") == 0;
			}
		}

		public override string GetHelpHtmlContent(ScriptIntellisenseItem item)
		{
			var type = Utils.GetUnderlyingType(Type);

			if (type == null)
				return null;

			var result = new StringBuilder();

			result.AppendLine($"<h1><b>{HtmlEncode(type.Name)}</b> ({HtmlEncode(type.FullName)})</h1><br>");

			var attrTypeDescription = type.GetCustomAttribute<DescriptionAttribute>(true);
			if (!string.IsNullOrWhiteSpace(attrTypeDescription?.Description))
				result.AppendLine($"<i>{HtmlEncode(attrTypeDescription?.Description)}</i><br>");

			result.AppendLine("<br>");

			if (IsStatic && type.IsEnum)
			{
				var names = Enum.GetNames(type);
				if (names == null || names.Length <= 0)
					return result.ToString();

				foreach (var name in names)
					result.AppendLine($"  -{HtmlEncode(name)}<br>");

				return result.ToString();
			}

			var bindingFlags = IsStatic ?
				BindingFlags.Static | BindingFlags.Public :
				BindingFlags.Instance | BindingFlags.Public;

			result.AppendLine("<h2>Properties:</h2><br>");
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
						if (strParameters.Length > 2)   // "[ " was added initially
							strParameters.Append(", ");
						if (parameter.ParameterType.IsByRef)
							strParameters.Append(parameter.IsOut ? "out " : "ref ");
						strParameters.Append($"[{paramTypeName}] {parameter.Name}");
					}
					strParameters.Append(']');
				}

				var attributeDescription = property.GetCustomAttribute<DescriptionAttribute>(true);
				var description = $"<b>{HtmlEncode(property.Name)}</b> [{HtmlEncode(typeName)}]<br>{HtmlEncode(attributeDescription?.Description)}<br><br>";

				result.AppendLine(description);
			}

			result.AppendLine("<h2>Methods:</h2><br>");
			var methods = type.GetMethods(bindingFlags | BindingFlags.InvokeMethod);
			foreach (var method in methods)
			{
				if (method.IsConstructor || method.MemberType != MemberTypes.Method || method.IsSpecialName)
					continue;

				var attributeBrowsable = method.GetCustomAttribute<BrowsableAttribute>(true);
				if (!(attributeBrowsable?.Browsable ?? true))
					continue;

				string typeName = "object";
				if (method.ReturnType != typeof(void))
					typeName = Utils.GetTypeName(method.ReturnType);

				var parameters = method.GetParameters();
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

				var attributeDescription = method.GetCustomAttribute<DescriptionAttribute>(true);
				var description = $"<b>{HtmlEncode(method.Name)}</b> [{HtmlEncode(typeName)}] {HtmlEncode(strParameters.ToString())}<br>{HtmlEncode(attributeDescription?.Description)}<br><br>";

				result.AppendLine(description);
			}

			return result.ToString();


			static string HtmlEncode(string value)
			{
				if (string.IsNullOrWhiteSpace(value))
					return value;

				var res = value.Replace("<", "&lt;").Replace(">", "&gt;").Replace(Environment.NewLine, "<br>\r\n");
				return res;
			}
		}

		public override void ShowOnlineHelp(ScriptIntellisenseItem item)
		{
			if (!IsFrameworkAssembly)
				return;

			var url = $"https://docs.microsoft.com/en-us/dotnet/api/{Type.FullName}";
			Process.Start(new System.Diagnostics.ProcessStartInfo() { FileName = url, UseShellExecute = true });
		}
	}
}
