﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Set, "BookDefaultCharacterProperties")]
    public class SetBookDefaultCharacterPropertiesCmdlet: BaseBookCharacterPropertiesCmdlet
    {
        protected override void DoProcessRecord(Document book)
        {
            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                base.SetCharacterProperties(book.DefaultCharacterProperties);
            }
        }
    }
}
