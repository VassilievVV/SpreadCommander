using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.ScriptEngines.ConsoleCommands
{
    public class BaseCommand
    {
        //public List<Property> Properties { get; } = new List<Property>();
        public StringNoCaseDictionary<string> Properties { get; } = new StringNoCaseDictionary<string>();

        public void AddProperty(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (Properties.ContainsKey(name))
                return;

            Properties.Add(name, value);
        }

        public T GetProperty<T>(string propertyName, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(propertyName))
                return defaultValue;

            if (!Properties.ContainsKey(propertyName))
                return defaultValue;

            try
            {
                var result = Utils.ChangeType<T>(Properties[propertyName]);
                return result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public bool HasProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return false;

            var result   = Properties.ContainsKey(propertyName);
            return result;
        }

        public virtual void Clear()
        {
            Properties.Clear();
        }
    }
}
