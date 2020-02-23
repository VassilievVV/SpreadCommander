using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
    public class ScriptIntellisenseItem
    {
        public enum IntellisenseItemType { EnumValue, Variable, Command, Constructor, Property, Function, Table, Procedure, Column }

        public string Caption { get; set; }

        public IntellisenseItemType ItemType { get; set; }

        public string Value { get; set; }

        public int? Position { get; set; }

        public bool IsMandatory { get; set; }

        public int ImageIndex => (int)ItemType;

        //Supports HTML and has to be HTML compatible
        public string Description { get; set; }

        public int DisplayOrder
        {
            get
            {
                return ItemType switch
                {
                    IntellisenseItemType.EnumValue   => 0,
                    IntellisenseItemType.Variable    => 1,
                    IntellisenseItemType.Command     => 2,
                    IntellisenseItemType.Constructor => 3,
                    IntellisenseItemType.Property    => 4,
                    IntellisenseItemType.Function    => 4,
                    IntellisenseItemType.Table       => 4,
                    IntellisenseItemType.Procedure   => 4,
                    IntellisenseItemType.Column      => 4,
                    _                                => 100
                };
            }
        }
    }
}
