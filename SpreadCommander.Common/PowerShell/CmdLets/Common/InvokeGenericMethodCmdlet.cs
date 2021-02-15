using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.PowerShell.CmdLets.Common
{
    [Cmdlet(VerbsLifecycle.Invoke, "GenericMethod")]
    public class InvokeGenericMethodCmdlet: SCCmdlet
    {
        [Parameter(ValueFromPipeline = true, HelpMessage = "Object for which to invoke generic method. Set to $null or skip to invoke static method.")]
        public object Object { get; set; }

        [Parameter(HelpMessage = "Type of the object. Mandatory for static methods.")]
        public Type Type { get; set; }

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Method name to invoke.")]
        public string MethodName { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Generic types of the method to invoke.")]
        public Type[] GenericTypes { get; set; }

        [Parameter(Position = 3, HelpMessage = "List of arguments to invoke.")]
        public object[] Arguments { get; set; }

        [Parameter(Position = 4, HelpMessage = "Method index in case there are multiple methods with same generic types.")]
        public int MethodIndex { get; set; }


        protected override void ProcessRecord()
        {
            Type objectType = Type;
            if (objectType == null)
            {
                if (Object is PSObject pSObject)
                    objectType = pSObject.BaseObject?.GetType();
                else
                    objectType = Object?.GetType();
            }

            if (objectType == null)
                throw new ArgumentException("Cannot determine object type", nameof(Type));
            if (string.IsNullOrWhiteSpace(MethodName))
                throw new ArgumentException("Method name is not provided", nameof(MethodName));
            if ((GenericTypes?.Length ?? 0) <= 0)
                throw new ArgumentException("Generic types are not provided", nameof(GenericTypes));

            object[] args = null;
            if (Arguments?.Length > 0)
            {
                args = new object[Arguments.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    var arg = Arguments[i];
                    if (arg is PSObject psArg)
                        arg = psArg.BaseObject;
                    args[i] = arg;
                }
            }

            var result = SCHost.DoInvokeGenericMethod(objectType, Object, MethodName, GenericTypes, args, MethodIndex);
            WriteObject(result);
        }
    }
}
