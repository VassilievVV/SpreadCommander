// *
// * Copyright (C) 2008 Roger Alsing : http://www.RogerAlsing.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System.Drawing;
using System.Globalization;

//namespace Alsing.Windows.Forms.FormatLabel
namespace Alsing.Windows.Forms.Controls.FormatLabel
{
    public enum TextEffect
    {
        None = 0,
        Outline,
        ShadowRB,
        ShadowLB,
        ShadowRT,
        ShadowLT,
    }

    public struct FontData
    {
        public string FontName  { get; set; }
        public int Size         { get; set; }
        public FontStyle Style  { get; set; }

        public FontData Clone()
        {
            return new FontData() { FontName = this.FontName, Size = this.Size, Style = this.Style };
        }
    }

    public class Element
    {
        protected string _Tag = "";
        protected string _TagName = "";
        public Color BackColor = Color.Black;
        public TextEffect Effect = 0;
        public Color EffectColor = Color.Black;
        //VVV
        //public Font Font;
        public FontData FontData { get; set; }
        public Color ForeColor = Color.Black;

        public Element Link;
        public bool NewLine;
        public string Text = "";
        public Word[] words;

        public string TagName
        {
            get { return _TagName; }
        }


        public string Tag
        {
            get { return _Tag; }
            set
            {
                _Tag = value.ToLowerInvariant();
                _Tag = _Tag.Replace("\t", " ");
                if (_Tag.IndexOf(" ") >= 0)
                {
                    _TagName = _Tag.Substring(0, _Tag.IndexOf(" "));
                }
                else
                {
                    _TagName = _Tag;
                }
            }
        }
    }
}