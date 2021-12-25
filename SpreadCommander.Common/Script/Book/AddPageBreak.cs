﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public partial class SCBook
    {
        public SCBook AddPageBreak(BookOptions options = null)
        {
            ExecuteSynchronized(options, () => DoAddPageBreak(options));
            return this;
        }

        protected void DoAddPageBreak(BookOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            book.AppendText("\f");
            WriteTextToConsole("\f");
        }
    }
}
