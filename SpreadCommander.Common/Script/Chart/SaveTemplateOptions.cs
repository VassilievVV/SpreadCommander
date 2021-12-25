using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Chart
{
    public class SaveTemplateOptions
    {
        [Description("If set and file already exists - it will be overwritten")]
        public bool Replace { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }
    }

    public partial class SCChart
    {
        public SCChart SaveTemplate(string fileName, SaveTemplateOptions options = null)
        {
            options ??= new SaveTemplateOptions();

            if (Chart == null)
                throw new Exception("Chart is not provided. Please use one of New-Chart cmdlets to create a chart.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Filename for image is not specified.");


            fileName = Project.Current.MapPath(fileName);
            var dir  = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                throw new Exception($"Directory '{dir}' does not exist.");
            
            if (File.Exists(fileName))
            {
                if (options.Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{fileName}' already exists.");
            }

            ExecuteLocked(() =>
            {
                using var fileStream = new FileStream(fileName, FileMode.CreateNew);
                Chart.SaveLayout(fileStream);
            }, options.LockFiles ? LockObject : null);

            return this;
        }
    }
}
