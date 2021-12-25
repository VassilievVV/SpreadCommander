using DevExpress.XtraMap;
using SpreadCommander.Common.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Map
{
    public class KeyColorizerOptions: ColorizerOptions 
    {
    }

    public partial class SCMap
    {
        public SCMap AddKeyColorizer(string keyField, Hashtable keys, string[] colors, KeyColorizerOptions options = null)
        {
            var layer = GetLayer(options?.Layer) as VectorItemsLayer ??
                throw new Exception("Cannot determine layer. Map items can be added only to vector items layer created with command AddMapLayerVectorItems.");

            if (string.IsNullOrWhiteSpace(keyField))
                throw new Exception("KeyField cannot be empty.");

            if (keys == null || keys.Count < 1)
                throw new Exception("Keys cannot be empty.");

            if (colors == null || colors.Length < 1)
                throw new Exception("Colors cannot be empty.");

            var colorizer = new KeyColorColorizer()
            {
                PredefinedColorSchema = PredefinedColorSchema.Palette
            };

            colorizer.ItemKeyProvider = new AttributeItemKeyProvider() { AttributeName = keyField };

            foreach (DictionaryEntry keyPair in keys)
                colorizer.Keys.Add(new ColorizerKeyItem() { Name = Convert.ToString(keyPair.Value), Key = keyPair.Key });

            foreach (var col in colors)
            {
                var color = Utils.ColorFromString(col);
                if (color == Color.Empty)
                    throw new Exception($"Color '{col}' is not valid.");

                colorizer.Colors.Add(color);
            }

            layer.Colorizer = colorizer;

            return this;
        }
    }
}
