// *
// * Copyright (C) 2008 Roger Alsing : http://www.RogerAlsing.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.Collections;
using System.Collections.Generic;

namespace Alsing.Windows.Forms.Document.SyntaxDefinition.Scope
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ScopeList : List<Scope>
    {
        public SpanDefinition.SpanDefinition Parent { get; private set; }

        public ScopeList(SpanDefinition.SpanDefinition parent)
        {
            Parent = parent;
        }
    }
}