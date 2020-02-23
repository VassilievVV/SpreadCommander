using DevExpress.XtraMap;
using DevExpress.XtraMap.Native;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.PowerShell.CmdLets.Book;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpreadCommander.Common.PowerShell.CmdLets.Map
{
    public class BaseMapCmdlet: BaseBookWithCommentsCmdlet
    {
        protected virtual Bitmap PaintMap(InnerMap map)
        {
            using var stream = new MemoryStream();
            map.ExportToImage(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);

            var image = new Bitmap(stream);
            return image;
        }

        protected void FlushMap(InnerMap map)
        {
            FlushMapImage(GetCmdletBook(), map);
        }

        protected void FlushMapImage(Document book, InnerMap map)
        {
            if (map == null)
                throw new Exception("Map is not provided. Please use one of New-Map cmdlets to create a map.");

            var mapBitmap = PaintMap(map);

            ExecuteSynchronized(() => DoWriteImage(book, mapBitmap));
        }

        protected virtual void DoWriteImage(Document book, Image mapBitmap)
        {
            if (mapBitmap == null)
                return;

            using (new UsingProcessor(() => book.BeginUpdate(), () => { ResetBookFormatting(book); book.EndUpdate(); }))
            {
                var image = book.Images.Append(mapBitmap);

                var range = image.Range;
                var rangeNewLine = book.AppendText(Environment.NewLine);

                AddComments(book, range);

                book.CaretPosition = rangeNewLine.End;
                ScrollToCaret();
            }
        }
    }
}
