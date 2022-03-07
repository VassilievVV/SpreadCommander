using DevExpress.Office;
using SpreadCommander.Common.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Script.Book
{
    public class DefaultOptions: BookOptions
    {
        [Description("Default value of a tab stop width")]
        public float? DefaultTabWidth { get; set; }

        [Description("Whether the document has different odd and even pages")]
        public bool? DifferentOddAndEvenPages { get; set; }

        [Description("Page's background color")]
        public string PageBackColor { get; set; }

        [Description("Shows the background color of the page")]
        public bool? ShowPageBackground { get; set; }

        [Description("Document's measure units")]
        public DocumentUnit? Unit { get; set; }

        [Description("Document's custom properties")]
        public Hashtable DocumentProperties { get; set; }
    }

    public partial class SCBook
    {
        public void SetDefaultOptions(DefaultOptions options) =>
            ExecuteSynchronized(options, () => DoSetDefaultOptions(options));

        protected virtual void DoSetDefaultOptions(DefaultOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Options are not provided.");

            var book = options.Book?.Document ?? Document;

            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                //Set Unit first, before setting properties that depends on it
                if (options.Unit.HasValue)
                    book.Unit = options.Unit.Value;

                if (options.DefaultTabWidth.HasValue)
                    book.DefaultTabWidth = options.DefaultTabWidth.Value;

                if (options.DifferentOddAndEvenPages.HasValue)
                    book.DifferentOddAndEvenPages = options.DifferentOddAndEvenPages.Value;

                var pageBackColor = Utils.ColorFromString(options.PageBackColor);
                if (options.ShowPageBackground.HasValue && pageBackColor != Color.Empty)
                    book.SetPageBackground(pageBackColor, options.ShowPageBackground.Value);
                else if (options.ShowPageBackground.HasValue)
                    book.SetPageBackground(options.ShowPageBackground.Value);
                else if (pageBackColor != Color.Empty)
                    book.SetPageBackground(pageBackColor);

                if (options.DocumentProperties != null && options.DocumentProperties.Count > 0)
                {
                    foreach (DictionaryEntry keyValue in options.DocumentProperties)
                    {
                        var propertyName = Convert.ToString(keyValue.Key);
                        if (string.IsNullOrWhiteSpace(propertyName))
                            throw new Exception("Empty custom property name is not allowed.");

                        if (book.CustomProperties.Names.Contains(propertyName))
                            book.CustomProperties.Remove(propertyName);

                        var propertyValue = keyValue.Value;

                        if (string.Compare(propertyName, nameof(book.DocumentProperties.Category), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Category = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.ContentStatus), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.ContentStatus = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.ContentType), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.ContentType = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Created), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Created = Convert.ToDateTime(propertyValue ?? DateTime.Now);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Creator), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Creator = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Description), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Description = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Identifier), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Identifier = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Keywords), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Keywords = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Language), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Language = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.LastModifiedBy), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.LastModifiedBy = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.LastPrinted), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.LastPrinted = Convert.ToDateTime(propertyValue ?? DateTime.Now);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Modified), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Modified = Convert.ToDateTime(propertyValue ?? DateTime.Now);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Revision), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Revision = Convert.ToInt32(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Subject), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Subject = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Title), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Title = Convert.ToString(propertyValue);
                        else if (string.Compare(propertyName, nameof(book.DocumentProperties.Version), StringComparison.InvariantCultureIgnoreCase) == 0)
                            book.DocumentProperties.Version = Convert.ToString(propertyValue);
                        else
                        {
                            if (propertyValue == null)  //Allow null to remove property
                                continue;

                            switch (Type.GetTypeCode(propertyValue.GetType()))
                            {
                                case TypeCode.Empty:
                                    throw new Exception($"Invalid custom property '{propertyName}' - it shall have type string, integer, double, date/time or boolean.");
                                case TypeCode.Object:
                                    throw new Exception($"Invalid custom property '{propertyName}' - it shall have type string, integer, double, date/time or boolean.");
                                case TypeCode.DBNull:
                                    throw new Exception($"Invalid custom property '{propertyName}' - it shall have type string, integer, double, date/time or boolean.");
                                case TypeCode.Boolean:
                                    book.CustomProperties.Add(propertyName, Convert.ToBoolean(propertyValue));
                                    break;
                                case TypeCode.Char:
                                    book.CustomProperties.Add(propertyName, Convert.ToString(propertyValue));
                                    break;
                                case TypeCode.SByte:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.Byte:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.Int16:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.UInt16:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.Int32:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.UInt32:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.Int64:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.UInt64:
                                    book.CustomProperties.Add(propertyName, Convert.ToInt32(propertyValue));
                                    break;
                                case TypeCode.Single:
                                    book.CustomProperties.Add(propertyName, Convert.ToDouble(propertyValue));
                                    break;
                                case TypeCode.Double:
                                    book.CustomProperties.Add(propertyName, Convert.ToDouble(propertyValue));
                                    break;
                                case TypeCode.Decimal:
                                    book.CustomProperties.Add(propertyName, Convert.ToDouble(propertyValue));
                                    break;
                                case TypeCode.DateTime:
                                    book.CustomProperties.Add(propertyName, Convert.ToDateTime(propertyValue));
                                    break;
                                case TypeCode.String:
                                    book.CustomProperties.Add(propertyName, Convert.ToString(propertyValue));
                                    break;
                                default:
                                    throw new Exception($"Invalid custom property '{propertyName}' - it shall have type string, integer, double, date/time or boolean.");
                            }
                        }
                    }
                }
            }
        }
    }
}
