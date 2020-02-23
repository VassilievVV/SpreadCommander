using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common
{
    public class Parameters
    {
        public const string ApplicationName      = "SpreadCommander";
        public const string ApplicationNamespace = "https://SpreadCommander.net";

        public static string ApplicationDataFolder
        {
            get
            {
                string result = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                result = Path.Combine(result, string.Format(ApplicationName));
                if (!Directory.Exists(result))
                    Directory.CreateDirectory(result);
                return result;
            }
        }

        public static Form MainForm { get; set; }

        public static string BingMapKey
        {
            get
            {
                var bingKey = ConfigurationManager.AppSettings["BingMapKey"];
                if (string.IsNullOrWhiteSpace(bingKey))
                    return null;

                if (bingKey.StartsWith("ENC:", StringComparison.InvariantCultureIgnoreCase))
                    bingKey = Utils.Decrypt(bingKey.Substring("ENC:".Length));

                return bingKey;
            }
        }
    }
}
