using DevExpress.Office.Services;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
    public class ProjectUriStreamProvider: IUriStreamProvider
    {
        public ProjectUriStreamProvider()
        {
        }

        public Stream GetStream(string uri)
        {
            var fileName = Project.Current.MapPath(uri);
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                return Stream.Null;

            using var fileStream = new FileStream(fileName, FileMode.Open);
            var result = new MemoryStream((int)fileStream.Length);
            fileStream.CopyTo(result);
            result.Seek(0, SeekOrigin.Begin);

            return result;
        }

        public static void RegisterProvider(IRichEditDocumentServer server)
        {
            var uriStreamService = server.GetService<IUriStreamService>();
            uriStreamService.RegisterProvider(new ProjectUriStreamProvider());
        }
    }
}
