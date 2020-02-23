// *
// * Copyright (C) 2008 Roger Alsing : http://www.RogerAlsing.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using Alsing.Windows.Forms.Document.SyntaxDefinition.Scope;
using Alsing.Windows.Forms.Document.SyntaxDefinition.SpanDefinition;

namespace Alsing.Windows.Forms.Document.DocumentStructure.Span
{
    /// <summary>
    /// 
    /// </summary>
    public class Span
    {
        /// <summary>
        /// The owner spanDefinition
        /// </summary>
        public SpanDefinition spanDefinition;

        /// <summary>
        /// The depth of this span in the span hirarchy
        /// </summary>
        public int Depth;

        /// <summary>
        /// The row that the span ends on
        /// </summary>
        public Row.Row EndRow;

        /// <summary>
        /// The word that ends this span
        /// </summary>
        public Word.Word EndWord;

        /// <summary>
        /// Gets or Sets if this span is expanded
        /// </summary>
        public bool Expanded = true;

        /// <summary>
        /// The parent span
        /// </summary>
        public Span Parent;

        /// <summary>
        /// Gets or Sets what scope triggered this span
        /// </summary>
        public Scope Scope;

        /// <summary>
        /// The row on which the span starts
        /// </summary>
        public Row.Row StartRow;

        /// <summary>
        /// The word that starts this span
        /// </summary>
        public Word.Word StartWord;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startrow"></param>
        public Span(Row.Row startrow)
        {
            StartRow = startrow;
        }

        /// <summary>
        /// 
        /// </summary>
        public Span() {}
    }
}