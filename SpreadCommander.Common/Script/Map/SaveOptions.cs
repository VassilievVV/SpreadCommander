﻿using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class SaveOptions
    {
        [Description("If set and file already exists - it will be overwritten")]
        public bool Replace { get; set; }

        [Description("Center point of a map.")]
        public double[] CenterPoint { get; set; }

        [Description("Zoom level of a map.")]
        [DefaultValue(1.0)]
        public double ZoomLevel { get; set; } = 1.0;

        [Description("Width of the image in document units (1/300 of inch). Default value is 2000.")]
        [DefaultValue(2000)]
        public int Width { get; set; } = 2000;

        [Description("Height of the image in document units (1/300 of inch). Default value is 1200.")]
        [DefaultValue(1200)]
        public int Height { get; set; } = 1200;

        [Description("Scaling factor of the image.")]
        [DefaultValue(1.0f)]
        public float Scale { get; set; } = 1;

        [Description("Specifies the format of the image")]
        public ImageFileFormat? Format { get; set; }

        [Description("If set - sends file to FileViewer")]
        public bool Preview { get; set; }

        [Description("Whether to lock file operations or not. Set it if multiple threads can access same file simultaneously.")]
        public bool LockFiles { get; set; }


        [Description("When set - produced sheet will be copied to book.")]
        public bool CopyToBook { get; set; }

        [Description("Book to copy table to. If NULL and CopyToBook is set - copy to host's Book.")]
        public Book.SCBook TargetBook { get; set; }

        [Description("Comment to add to Book")]
        public string BookComment { get; set; }

        [Description("If set - BookComment is treated as HTML")]
        public bool BookCommentHtml { get; set; }

        [Description("Bookmark in the document")]
        public string BookBookmark { get; set; }

        [Description("Hyperlink to existing bookmark")]
        public string BookHyperlink { get; set; }

        [Description("Text for the tooltip displayed when the mouse hovers over a hyperlink")]
        public string BookHyperlinkTooltip { get; set; }

        [Description("Target window or frame in which to display the web page content when the hyperlink is clicked")]
        public string BookHyperlinkTarget { get; set; }
    }

    public partial class SCMap
    {
        public void Save(string fileName, SaveOptions options = null)
        {
            options ??= new SaveOptions();

            var map = Map;
            if (map == null)
                throw new Exception("Map is not provided. Please use one of New-Map cmdlets to create a map.");

            fileName = Project.Current.MapPath(fileName);
            var dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                throw new Exception($"Directory '{dir}' does not exist.");

            if (File.Exists(fileName))
            {
                if (options.Replace)
                    File.Delete(fileName);
                else
                    throw new Exception($"File '{fileName}' already exists.");
            }

            if (options.CenterPoint != null && options.CenterPoint.Length != 2)
                throw new Exception("CanterPoint shall be array of exactly 2 double values.");

            if (options.CenterPoint != null)
                map.PublicCenterPoint = CreateCoordPoint(options.CenterPoint[0], options.CenterPoint[1]);

            int w = Convert.ToInt32(options.Width * 96.0 / 300.0);
            int h = Convert.ToInt32(options.Height * 96.0 / 300.0);

            map.ZoomLevel = options.ZoomLevel;
            map.SetClientRectangle(new Rectangle(0, 0, w, h));

            var mapBitmap = PaintMap(map);

            DoWriteImage(mapBitmap, fileName, options);

            if (options.CopyToBook)
            {
                var book = options.TargetBook?.Document ?? Host?.Book?.Document;
                if (book != null)
                    DoCopyImageToBook(mapBitmap, book, null, null, options);
            }

            if (options.Preview)
                PreviewFile(fileName);
        }

        protected virtual void DoWriteImage(Image mapBitmap, string fileName, SaveOptions options)
        {
            fileName = Project.Current.MapPath(fileName);

            if (mapBitmap == null)
                throw new Exception("No image to save.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Filename for image is not specified.");
            System.Drawing.Imaging.ImageFormat format = (options.Format ?? GetImageFormatFromFileName(fileName)) switch
            {
                ImageFileFormat.Png  => System.Drawing.Imaging.ImageFormat.Png,
                ImageFileFormat.Tiff => System.Drawing.Imaging.ImageFormat.Tiff,
                ImageFileFormat.Bmp  => System.Drawing.Imaging.ImageFormat.Bmp,
                ImageFileFormat.Gif  => System.Drawing.Imaging.ImageFormat.Gif,
                ImageFileFormat.Jpeg => System.Drawing.Imaging.ImageFormat.Jpeg,
                _                    => System.Drawing.Imaging.ImageFormat.Png
            };
            ExecuteLocked(() => mapBitmap.Save(fileName, format), options.LockFiles ? LockObject : null);
        }

        protected virtual void DoCopyImageToBook(Image image, Document book, float? scaleX, float? scaleY, SaveOptions options)
        {
            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                var docImage = book.Images.Append(image);

                if (scaleX.HasValue)
                    docImage.ScaleX = scaleX.Value;
                if (scaleY.HasValue)
                    docImage.ScaleY = scaleY.Value;

                book.Paragraphs.Append();

                AddBookComments(book, docImage.Range, options);

                book.CaretPosition = docImage.Range.End;
                Book.SCBook.ResetBookFormatting(book, book.CaretPosition);
                ScrollToCaret();
            }
        }

#pragma warning disable CA1822 // Mark members as static
        protected void AddBookComments(Document book, DocumentRange range, SaveOptions options)
#pragma warning restore CA1822 // Mark members as static
        {
            var comment = options.BookComment;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                var bookComment = book.Comments.Create(range, Environment.UserName);
                var docComment = bookComment.BeginUpdate();
                try
                {
                    if (options.BookCommentHtml)
                        docComment.AppendHtmlText(comment, InsertOptions.KeepSourceFormatting);
                    else
                        docComment.AppendText(comment);
                }
                finally
                {
                    bookComment.EndUpdate(docComment);
                }
            }

            var bookmark = options.BookBookmark;
            if (!string.IsNullOrWhiteSpace(bookmark))
            {
                var oldBookmark = book.Bookmarks[bookmark];
                if (oldBookmark != null)
                    book.Bookmarks.Remove(oldBookmark);

                book.Bookmarks.Create(range, bookmark);
            }

            var hyperlink = options.BookHyperlink;
            if (!string.IsNullOrWhiteSpace(hyperlink))
            {
                var hyperlinkBookmark = book.Bookmarks[hyperlink];
                var link = book.Hyperlinks.Create(range);
                if (hyperlinkBookmark != null)
                    link.Anchor = hyperlinkBookmark.Name;
                else
                    link.NavigateUri = hyperlink;

                if (!string.IsNullOrWhiteSpace(options.BookHyperlinkTooltip))
                    link.ToolTip = options.BookHyperlinkTooltip;

                if (!string.IsNullOrWhiteSpace(options.BookHyperlinkTarget))
                    link.Target = options.BookHyperlinkTarget;
            }
        }
    }
}
