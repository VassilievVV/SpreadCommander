using DevExpress.Office;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using System.Drawing;
using System.Collections;

namespace SpreadCommander.Common.PowerShell.CmdLets.Book
{
    [Cmdlet(VerbsCommon.Set, "BookDefaultProperties")]
    public class SetBookDefaultPropertiesCmdlet: BaseBookCmdlet
    {
        [Parameter(HelpMessage = "Default value of a tab stop width")]
        public float? DefaultTabWidth { get; set; }

        [Parameter(HelpMessage = "Whether the document has different odd and even pages")]
        public SwitchParameter DifferentOddAndEvenPages { get; set; }

        [Parameter(HelpMessage = "Reset DifferentOddAndEvenPages")]
        public SwitchParameter ResetDifferentOddAndEvenPages { get; set; }

        [Parameter(HelpMessage = "Page's background color")]
        public string PageBackColor { get; set; }

        [Parameter(HelpMessage = "Shows the background color of the page")]
        public SwitchParameter ShowPageBackground { get; set; }

        [Parameter(HelpMessage = "Hides the background color of the page")]
        public SwitchParameter HidePageBackground { get; set; }

        [Parameter(HelpMessage = "Document's measure units")]
        public DocumentUnit? Unit { get; set; }

        [Parameter(HelpMessage = "Document's custom properties")]
        public Hashtable DocumentProperties { get; set; }


        protected override void ProcessRecord()
        {
            ExecuteSynchronized(() => DoProcessRecord(GetCmdletBook()));
        }

        protected virtual void DoProcessRecord(Document book)
        {
            using (new UsingProcessor(() => book.BeginUpdate(), () => book.EndUpdate()))
            {
                //Set Unit first, before setting properties that depends on it
                if (Unit.HasValue)
                    book.Unit = Unit.Value;

                if (DefaultTabWidth.HasValue)
                    book.DefaultTabWidth = DefaultTabWidth.Value;

                if (DifferentOddAndEvenPages) book.DifferentOddAndEvenPages = true;
                if (ResetDifferentOddAndEvenPages) book.DifferentOddAndEvenPages = false;

                var pageBackColor = Utils.ColorFromString(PageBackColor);
                if (ShowPageBackground && pageBackColor != Color.Empty)
                    book.SetPageBackground(pageBackColor, true);
                else if (HidePageBackground && pageBackColor != Color.Empty)
                    book.SetPageBackground(pageBackColor, false);
                else if (ShowPageBackground)
                    book.SetPageBackground(true);
                else if (HidePageBackground)
                    book.SetPageBackground(false);
                else if (pageBackColor != Color.Empty)
                    book.SetPageBackground(pageBackColor);

                if (DocumentProperties != null && DocumentProperties.Count > 0)
                {
                    foreach (DictionaryEntry keyValue in DocumentProperties)
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
