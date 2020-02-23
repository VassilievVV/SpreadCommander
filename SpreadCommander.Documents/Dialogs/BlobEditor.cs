#pragma warning disable CRR0047

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using SpreadCommander.Common;
using System.IO;
using SpreadCommander.Common.Code;

namespace SpreadCommander.Documents.Dialogs
{
    public partial class BlobEditor: BaseForm
    {
        #region EncodingItem
        private class EncodingItem
        {
            private readonly EncodingInfo _Info;

            public EncodingItem(EncodingInfo info)
            {
                _Info = info;
            }

            public EncodingInfo Info
            {
                get {return _Info;}
            }

            public override string ToString()
            {
                return _Info != null ? _Info.DisplayName : "Unknown";
            }
        }
        #endregion

        private readonly VirtualList	_DataList;
#pragma warning disable IDE0069 // Disposable fields should be disposed
        private Font					_CellFont;
#pragma warning restore IDE0069 // Disposable fields should be disposed
        private Encoding				_Encoding;
        private string					_MimeType;

        public BlobEditor(byte[] data)
        {
            using (new WaitCursor())
            {
                InitializeComponent();
                _DataList = new VirtualList(data);
            }

            Disposed += (s, e) =>
            {
                _CellFont?.Dispose();
                _CellFont = null;
            };
        }

        private void BlobEditor_Load(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                _CellFont          = new Font(FontFamily.GenericMonospace, 8.25F);
                gridHex.DataSource = _DataList;
                _Encoding          = Encoding.ASCII;

                _MimeType          = WinAPI.FindMimeFromData(_DataList.BlobData) ?? "<BLOB>";
                textMime.Text      = _MimeType;

                if (string.Compare(_MimeType, "text/rtf", true) == 0 ||
                    string.Compare(_MimeType, "application/rtf", true) == 0 ||
                    string.Compare(_MimeType, "text/richtext", true) == 0 ||
                    string.Compare(_MimeType, "text/html", true) == 0 ||
                    string.Compare(_MimeType, "message/rfc822", true) == 0)
                    tabHex.SelectedTabPage = layoutFormatted;
                else if (_MimeType.StartsWith("text/", StringComparison.InvariantCultureIgnoreCase))
                    tabHex.SelectedTabPage = layoutText;
                else if (_MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase))
                    tabHex.SelectedTabPage = layoutImage;
                else
                    tabHex.SelectedTabPage = layoutHex;

                EncodingInfo[] encodings     = Encoding.GetEncodings();
                ComboBoxItemCollection items = comboEncodings.Properties.Items;
                using (new UsingProcessor(() => items.BeginUpdate(), () => items.EndUpdate()))
                {
                    foreach (EncodingInfo info in encodings)
                    {
                        EncodingItem encodingItem = new EncodingItem(info);
                        items.Add(encodingItem);
                    }
                }

                string asciiName = Encoding.ASCII.EncodingName;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] is EncodingItem encodingItem && string.Compare(encodingItem.ToString(), asciiName, true) == 0)
                    {
                        comboEncodings.SelectedIndex = i;
                        break;
                    }
                }

                if (_MimeType.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        using MemoryStream stream = new MemoryStream(_DataList.BlobData);
                        Image image = Image.FromStream(stream);
                        pictureBlob.Image = image;
                    }
                    catch (Exception)
                    {
                        pictureBlob.Image = null;
                    }
                }
            }
        }

        private void ViewHex_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            e.Appearance.Font = _CellFont;
        }

        private void ComboEncodings_SelectedIndexChanged(object sender, EventArgs e)
        {
            const string textNotAvailable = "Text is not available";

            _Encoding = comboEncodings.SelectedItem is EncodingItem item && item.Info != null ? item.Info.GetEncoding() : Encoding.ASCII;

            _DataList.Encoding = _Encoding;
            gridHex.RefreshDataSource();

            try
            {
                if (string.Compare(_MimeType, "application/rtf", true) == 0 ||
                    string.Compare(_MimeType, "message/rfc822", true) == 0 ||
                    _MimeType.StartsWith("text/", StringComparison.InvariantCultureIgnoreCase))
                {
                    string text   = Utils.NonNullString(_Encoding.GetString(_DataList.BlobData));
                    memoBlob.Text = text;

                    if (string.Compare(_MimeType, "text/rtf", true) == 0 ||
                        string.Compare(_MimeType, "application/rtf", true) == 0 ||
                        string.Compare(_MimeType, "text/richtext", true) == 0)
                        richEditBlob.RtfText = text;
                    else if (string.Compare(_MimeType, "text/html", true) == 0)
                        richEditBlob.HtmlText = text;
                    else if (string.Compare(_MimeType, "message/rfc822", true) == 0)
                        richEditBlob.MhtText = text;
                    else if (_MimeType.StartsWith("text/", StringComparison.InvariantCultureIgnoreCase))
                        richEditBlob.Text = text;
                    else
                        richEditBlob.Text = string.Empty;
                }
                else
                {
                    memoBlob.Text     = textNotAvailable;
                    richEditBlob.Text = textNotAvailable;
                }
            }
            catch (Exception)
            {
                memoBlob.Text     = textNotAvailable;
                richEditBlob.Text = textNotAvailable;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dlg = new SaveFileDialog()
            {
                CheckPathExists = true,
                Filter          = "All files (*.*)|*.*",
                OverwritePrompt = true, Title = "Save BLOB"
            };

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                using (new WaitCursor())
                {
                    using FileStream stream = new FileStream(dlg.FileName, FileMode.Create);
                    stream.Write(_DataList.BlobData, 0, _DataList.BlobData.Length);
                }
            }
        }
    }

    #region VirtualPropertyDescriptor
    public class VirtualPropertyDescriptor: PropertyDescriptor
    {
        private readonly string			_PropertyName;
        private readonly Type			_PropertyType;
        private readonly bool			_IsReadOnly;
        private readonly VirtualList	_List;
        private readonly int			_Index;

        public VirtualPropertyDescriptor(VirtualList list, int index, string propertyName, Type propertyType, bool isReadOnly)
            : base(propertyName, null)
        {
            _PropertyName = propertyName;
            _PropertyType = propertyType;
            _IsReadOnly = isReadOnly;
            _List = list;
            _Index = index;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return _List.GetRowValue(component, _Index);
        }

        public override void SetValue(object component, object val)
        {
            //Do nothing
        }

        public override bool IsReadOnly {get {return _IsReadOnly;}}

        public override string Name {get {return _PropertyName;}}

        public override Type ComponentType {get {return typeof(VirtualList);}}

        public override Type PropertyType {get {return _PropertyType;}}

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component) {return true;}
    }
    #endregion

    #region VirtualList
    public class VirtualList: IList, ITypedList
    {
        private PropertyDescriptorCollection	_ColumnCollection;
        private readonly byte[]					_BlobData;
        private Encoding						_Encoding			= Encoding.ASCII;

        public VirtualList(byte[] blobData)
        {
            _BlobData = blobData;
            CreateColumnCollection();
        }

        public byte[] BlobData
        {
            get {return _BlobData;}
        }

        public Encoding Encoding
        {
            get {return _Encoding;}
            set {_Encoding = value ?? Encoding.ASCII;}
        }

        internal object GetRowValue(object row, int colIndex)
        {
            if (_BlobData == null)
                return null;

            int rowIndex = (int)row;

            if (colIndex == 0)
                return rowIndex * 16;
            else if (colIndex < 17)
            {
                int index = rowIndex * 16 + (colIndex - 1);
                if (index >= 0 && index < _BlobData.Length)
                    return _BlobData[index];
                return null;
            }
            else if (colIndex == 17)
            {
                int index = rowIndex * 16;
                int i = 0;
                StringBuilder result = new StringBuilder();
                while (index >= 0 && index < _BlobData.Length && i++ < 16)
                {
                    char c = _Encoding.GetChars(_BlobData, index, 1)[0];
                    if (char.IsLetterOrDigit(c) || char.IsPunctuation(c))
                        result.Append(c);
                    else
                        result.Append('.');
                    index++;
                }
                return result.ToString();
            }

            return null;
        }

        public int RecordCount
        {
            get {return _BlobData != null ? Math.Max(_BlobData.Length - 1, 0) / 16 + 1 : 0;}
        }

        protected virtual void CreateColumnCollection()
        {
            VirtualPropertyDescriptor[] pds = new VirtualPropertyDescriptor[18];

            pds[0] = new VirtualPropertyDescriptor(this, 0, "Address", typeof(long), true);
            for (int i = 0; i < 16; i++)
                pds[i + 1] = new VirtualPropertyDescriptor(this, i + 1, $"Field{i:X1}", typeof(byte?), true);
            pds[17] = new VirtualPropertyDescriptor(this, 17, "Text", typeof(string), true);

            _ColumnCollection = new PropertyDescriptorCollection(pds);
        }

        #region ITypedList Interface
        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] descs) {return _ColumnCollection;}
        string ITypedList.GetListName(PropertyDescriptor[] descs) {return "";}
        #endregion

        #region IList Interface
        public virtual int Count
        {
            get {return RecordCount;}
        }

        public virtual bool IsSynchronized
        {
            get {return true;}
        }

        public virtual object SyncRoot
        {
            get {return true;}
        }

        public virtual bool IsReadOnly
        {
            get {return false;}
        }

        public virtual bool IsFixedSize
        {
            get {return true;}
        }

        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }

        public virtual void CopyTo(Array array, int index)
        {
        }

        public virtual int Add(object val)
        {
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual bool Contains(object val)
        {
            throw new NotImplementedException();
        }

        public virtual int IndexOf(object val)
        {
            throw new NotImplementedException();
        }

        public virtual void Insert(int index, object val)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(object val)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        object IList.this[int index]
        {
            get {return index;}
            set {}
        }
        #endregion
    }
    #endregion
}