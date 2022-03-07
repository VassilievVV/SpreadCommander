using SpreadCommander.Common.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public partial class SCBook
    {
        public void SetDefaultCharacter(CharacterStyleOptions options) =>
            ExecuteSynchronized(options, () => DoSetDefaultCharacter(options));

        protected virtual void DoSetDefaultCharacter(CharacterStyleOptions options)
        {
            var book = options?.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
                SetCharacterOptions(book.DefaultCharacterProperties, options);
        }
    }
}
