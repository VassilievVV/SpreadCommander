using DevExpress.XtraRichEdit;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Book
{
    public partial class SCBook
    {
        protected IRichEditDocumentServer AddImage(ArgumentCollection arguments)
        {
            if (arguments.Count <= 0)
                throw new Exception("'DOCVARIABLE IMAGE' requires filename as first argument.");

            var fileName = arguments[0].Value;
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("DOCVARIABLE IMAGE does not contain valid filename.");

            object snippet = null;

            if (Snippets?.ContainsKey(fileName) ?? false)
            {
                snippet = Snippets[fileName];
                if (snippet is Image)
                {
                }
                else
                    throw new Exception($"Specified snippet '{fileName}' is not supported. Snippet shall be either Book or System.Drawing.Image.");
            }
            else
                fileName = Project.Current.MapPath(fileName);

            if (snippet == null && !File.Exists(fileName))
                throw new Exception($"File '{fileName}' does not exist.");

            using (new UsingProcessor(() => CheckNestedFile(fileName), () => RemoveNestedFile(fileName)))
            {
                var image = snippet as Image ?? Bitmap.FromFile(fileName);

                float? scale = null, scaleX = null, scaleY = null;

                if (arguments.Count > 1)
                {
                    var properties = Utils.SplitNameValueString(arguments[1].Value, ';');

                    foreach (var prop in properties)
                    {
                        if (string.IsNullOrWhiteSpace(prop.Key))
                            continue;

                        switch (prop.Key.ToLower())
                        {
                            case "dpi":
                                var dpi = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                if (image is Bitmap bmp)
                                    bmp.SetResolution(dpi, dpi);
                                break;
                            case "scale":
                                scale = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                break;
                            case "scalex":
                                scaleX = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                break;
                            case "scaley":
                                scaleY = float.Parse(prop.Value, CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                }

                var server   = new RichEditDocumentServer();
                var document = server.Document;
                var docImage = document.Images.Append(image);

                if (scale.HasValue)
                    docImage.ScaleX = docImage.ScaleY = scale.Value;
                else
                {
                    if (scaleX.HasValue)
                        docImage.ScaleX = scaleX.Value;
                    if (scaleY.HasValue)
                        docImage.ScaleY = scaleY.Value;
                }

                return server;
            }
        }
    }
}
