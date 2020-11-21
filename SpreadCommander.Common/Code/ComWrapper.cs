/*
* 	Based on code by Ranjeet Chakraborty 29/05/2001 ranjeetc@hotmail.com
*/

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SpreadCommander.Common.Code
{
    public class ComWrapper: IDisposable
    {
        private object      	_ComObject;
        private readonly Type	_Type;

        public ComWrapper(object o)
        {
            _ComObject = o;
            _Type		= _ComObject.GetType();
        }

        public ComWrapper(string ProgId)
        {
            Type type	= Type.GetTypeFromProgID(ProgId);
            _ComObject	= Activator.CreateInstance(type);
            _Type		= _ComObject.GetType();
        }

        ~ComWrapper()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_ComObject != null)
                Marshal.ReleaseComObject(_ComObject);
            _ComObject = null;
        }

        public object GetProperty(string PropName)
        {
            object PropValue = this._Type.InvokeMember(PropName,
                BindingFlags.InvokeMethod|BindingFlags.GetProperty,
                null, _ComObject, Array.Empty<object>());
            return PropValue;
        }

        public object GetProperty(string PropName, string PropType)
        {
            object PropValue = _Type.InvokeMember(PropName,
                BindingFlags.InvokeMethod|BindingFlags.GetProperty,
                null, _ComObject, new object [] {PropType});
            return PropValue;
        }

        public object GetProperty(string PropName, int PropType)
        {
            object PropValue = _Type.InvokeMember(PropName,
                BindingFlags.InvokeMethod|BindingFlags.GetProperty,
                null, _ComObject, new object [] {PropType});
            return PropValue;
        }

        public object GetProperty(string PropName, params object[] Parameters)
        {
            object PropValue = _Type.InvokeMember(PropName,
                BindingFlags.InvokeMethod|BindingFlags.GetProperty,
                null, _ComObject, Parameters);
            return PropValue;
        }

        public void SetProperty(string PropName, params object[] args)
        {
            this._Type.InvokeMember(PropName,
                BindingFlags.Default | BindingFlags.SetProperty,
                null, _ComObject, args);
            return;
        }

        public object this[string PropName]
        {
            get {return GetProperty(PropName);}
            set {SetProperty(PropName, new object[] {value});}
        }

        public object CallMethod(string MethodName, params object[] args)
        {
            object RetVal = _Type.InvokeMember(MethodName,
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null, _ComObject, args);
            return RetVal;
        }
    }
}