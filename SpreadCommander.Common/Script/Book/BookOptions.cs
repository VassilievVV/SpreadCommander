using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class BookOptions
    {
        public const int DefaultDPI = 300;

        [Description("Book to execute command. Leave NULL to use UI book.")]
        public SCBook Book { get; set; }
    }
}
