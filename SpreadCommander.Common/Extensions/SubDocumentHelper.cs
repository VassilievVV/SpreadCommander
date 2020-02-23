using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Extensions
{
    public static class SubDocumentHelper
    {
        public delegate void SubDocumentDelegate(SubDocument subDocument);

        public static void UpdateAllFields(this Document document)
        {
            document.ForEachSubDocument(subdoc => subdoc.Fields.Update());
        }

        public static void ForEachSubDocument(this Document document, SubDocumentDelegate subDocumentProcessor)
        {
            subDocumentProcessor(document);
            ProcessShapes(document.Shapes, subDocumentProcessor);
            ProcessComments(document.Comments, subDocumentProcessor);
            foreach (Section section in document.Sections)
            {
                ProcessSection(section, HeaderFooterType.First, subDocumentProcessor);
                ProcessSection(section, HeaderFooterType.Odd, subDocumentProcessor);
                ProcessSection(section, HeaderFooterType.Even, subDocumentProcessor);
            }
        }

        private static void ProcessSection(Section section, HeaderFooterType headerFooterType, SubDocumentDelegate subDocumentProcessor)
        {
            if (section.HasHeader(headerFooterType))
            {
                SubDocument header = section.BeginUpdateHeader(headerFooterType);
                try
                {
                    subDocumentProcessor(header);
                    ProcessShapes(header.Shapes, subDocumentProcessor);
                }
                finally
                {
                    section.EndUpdateHeader(header);
                }
            }

            if (section.HasFooter(headerFooterType))
            {
                SubDocument footer = section.BeginUpdateFooter(headerFooterType);
                try
                {
                    subDocumentProcessor(footer);
                    ProcessShapes(footer.Shapes, subDocumentProcessor);
                }
                finally
                {
                    section.EndUpdateFooter(footer);
                }
            }
        }

        private static void ProcessShapes(ShapeCollection shapes, SubDocumentDelegate subDocumentProcessor)
        {
            foreach (Shape shape in shapes)
                if (shape.TextBox != null)
                    subDocumentProcessor(shape.TextBox.Document);
        }

        private static void ProcessComments(CommentCollection comments, SubDocumentDelegate subDocumentProcessor)
        {
            foreach (Comment comment in comments)
            {
                SubDocument commentSubDocument = comment.BeginUpdate();
                try
                {
                    subDocumentProcessor(commentSubDocument);
                }
                finally
                {
                    comment.EndUpdate(commentSubDocument);
                }
            }
        }
    }
}
