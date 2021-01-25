#pragma warning disable CA1822 // Mark members as static

using SpreadCommander.Common.PowerShell.Host;
using SpreadCommander.Common.ScriptEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets
{
    public class SCHost
    {
        private readonly ExternalHost _ExternalHost;

        public SCHost(ExternalHost externalHost)
        {
            _ExternalHost = externalHost ?? throw new ArgumentNullException(nameof(externalHost));
        }

        public bool Silent
        {
            get => _ExternalHost.Silent;
            set => _ExternalHost.Silent = value;
        }

        public string ProjectPath    => Project.Current.ProjectPath;
        public string StartupCommand => PowerShellScriptEngine.StartupCommand;

        public string MapPath(string fileName) => Project.Current.MapPath(fileName);

        public object InvokeGenericNetMethod(object inputObject, string methodName, Type[] genericTypes, object[] argumentList, int methodIndex = 0)
        {
            if (inputObject == null)
                throw new ArgumentNullException(nameof(inputObject));

            var result = DoInvokeGenericMethod(inputObject.GetType(), inputObject, methodName, genericTypes, argumentList, methodIndex);
            return result;
        }

        public object InvokeStaticGenericNetMethod(Type type, string methodName, Type[] genericTypes, object[] argumentList, int methodIndex = 0)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var result = DoInvokeGenericMethod(type, null, methodName, genericTypes, argumentList, methodIndex);
            return result;
        }

        internal static object DoInvokeGenericMethod(Type type, object inputObject, string methodName, Type[] genericTypes, object[] argumentList, int methodIndex)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException(nameof(methodName));
            if (genericTypes == null || genericTypes.Length <= 0)
                throw new ArgumentNullException(nameof(genericTypes));

            var bindingFlags = inputObject != null ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Static | BindingFlags.Public;

            var methodInfo = type.GetMethods(bindingFlags).Where(m => m.IsGenericMethod && string.Compare(m.Name, methodName, true) == 0 &&
                genericTypes.Length == m.GetGenericArguments().Length).Skip(methodIndex).FirstOrDefault();
            if (methodInfo == null)
                throw new Exception($"Cannot find method '{methodName}' with specified generic types.");

            var genericMethodInfo = methodInfo.MakeGenericMethod(genericTypes);
            if (genericMethodInfo == null)
                throw new Exception($"Cannot make generic method '{methodName}' with specified generic types.");

            var result = genericMethodInfo.Invoke(inputObject, argumentList);
            return result;
        }
    }
}
