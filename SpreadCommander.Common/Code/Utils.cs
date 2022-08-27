using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Drawings;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace SpreadCommander.Common.Code
{
    public static class Utils
    {
        private const string DefaultPassword = "35E9414FA38C48D7A6D28550694C35DC";
        private const string PasswordSalt    = "6B0C5D61A26A4BE7886A85AAFD3CB75B";

        public static string NullString(string value)
        {
            if (value != null && value.Length <= 0)
                return null;
            return value;
        }

        public static string NonNullString(string value)
        {
            if (value == null)
                return string.Empty;
            return value;
        }

        public static string TrimString(string value) => value?.Trim();

        public static string IncrementStringNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "1";

            int counter = value.Length - 1;
            while (counter > 0 && char.IsDigit(value[counter]))
                counter--;
            if (counter >= value.Length - 1)
                return $"{value}1";

            string strNum = value[(counter + 1)..];
            int num = int.Parse(strNum);
            string result = $"{value[..(counter + 1)]}{++num}";
            return result;
        }

        public static string TrimLineBreaks(string value)
        {
            if (value == null)
                return string.Empty;

            string result = value.Trim('\r', '\n');
            return result;
        }

        public static string AddUniqueString(IList<string> lines, string value, StringComparison comparison, bool addValue)
        {
            if (string.IsNullOrEmpty(value))
                value = "1";

            if (!ContainsValue(value))
            {
                if (addValue)
                    lines.Add(value);
                return value;
            }

            string baseValue;
            int num = 1;
            int counter = value.Length - 1;
            while (counter > 0 && char.IsDigit(value[counter]))
                counter--;
            if (counter >= 0 && value[counter] == '_')	//Remove '_' before digits
                counter--;
            if (counter >= value.Length - 1)
                baseValue = value;
            else
            {
                baseValue     = value[..(counter + 1)];
                string strNum = value[counter + 1] == '_' ? value[(counter + 2)..] : value[(counter + 1)..];
                num           = int.Parse(strNum, CultureInfo.InvariantCulture);
            }

            while (true)
            {
                value = $"{baseValue}{num}";
                if (!ContainsValue(value))
                {
                    if (addValue)
                        lines.Add(value);
                    return value;
                }

                num++;
            }

            bool ContainsValue(string line)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (string.Compare(lines[i], line, comparison) == 0)
                        return true;
                }
                return false;
            }
        }

        public static string AddUniqueStringSorted(IList<string> lines, string value, bool caseSensitive, bool addValue)
        {
            if (string.IsNullOrEmpty(value))
                value = "1";

            if (!FindSortedStringIndex(lines, value, caseSensitive, out int _))
            {
                if (addValue)
                    AddStringToSortedList(lines, value, caseSensitive);
                return value;
            }

            string baseValue;
            int num = 1;
            int counter = value.Length - 1;
            while (counter > 0 && char.IsDigit(value[counter]))
                counter--;
            if (counter >= value.Length - 1)
                baseValue = value;
            else
            {
                baseValue = value[..counter];
                string strNum = value[(counter + 1)..];
                num = int.Parse(strNum);
            }

            while (true)
            {
                value = $"{baseValue}{num}";
                if (!FindSortedStringIndex(lines, value, caseSensitive, out _))
                {
                    if (addValue)
                        AddStringToSortedList(lines, value, caseSensitive);
                    return value;
                }

                num++;
            }
        }

        public static string AddUniqueKey<TValue>(SortedList<string, TValue> list, string value, TValue obj)
        {
            if (string.IsNullOrEmpty(value))
                value = "1";

            if (!list.ContainsKey(value))
            {
                list[value] = obj;
                return value;
            }

            string baseValue;
            int num = 1;
            int counter = value.Length - 1;
            while (counter > 0 && char.IsDigit(value[counter]))
                counter--;
            if (counter >= value.Length - 1)
                baseValue = value;
            else
            {
                baseValue = value[..counter];
                string strNum = value[(counter + 1)..];
                num = int.Parse(strNum);
            }

            while (true)
            {
                value = $"{baseValue}{num}";
                if (!list.ContainsKey(value))
                {
                    list[value] = obj;
                    return value;
                }

                num++;
            }
        }

        public static string UnquoteString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            value = value.Trim();
            if (value.Length <= 1)
                return NonNullString(value);

            switch (value[0])
            {
                case '[':
                    if (value[^1] == ']')
                        return value[1..^1].Replace("]]", "]");
                    break;
                case '\"':
                    if (value[^1] == '\"')
                        return value[1..^1].Replace("\"\"", "\"");
                    break;
                case '\'':
                    if (value[^1] == '\'')
                        return value[1..^1].Replace("\'\'", "\'");
                    break;
                case '`':
                    if (value[^1] == '`')
                        return value[1..^1].Replace("``", "`");
                    break;
            }

            return value;
        }

        public static string UnpackString(string value)
        {
            string result = Utils.NonNullString(value);
            result = Utils.TrimString(result);
            result = Utils.UnquoteString(result);
            return result;
        }

        public static bool FindSortedStringIndex(IList<string> list, string value,
            bool caseSensitive, out int index)
        {
            int low, high, current, compare;

            low = 0;
            high = list.Count - 1;
            while (low <= high)
            {
                current = (low + high) >> 1;
                //compare = string.Compare(list[current], value, comparison);
                compare = StringLogicalComparer.Compare(list[current], value, caseSensitive);
                if (compare < 0)
                    low = current + 1;
                else
                {
                    high = current - 1;
                    if (compare == 0)
                    {
                        index = current;
                        return true;
                    }
                }
            }

            index = low;
            return false;
        }

        public static bool AddStringToSortedList(IList<string> list, string value, bool caseSensitive)
        {
            if (FindSortedStringIndex(list, value, caseSensitive, out int index))
                return false;

            if (index >= 0)
                list.Insert(index, value);
            else
                list.Add(value);

            return true;
        }

        public static bool FindSortedIntIndex(IList<int> list, int value, out int index)
        {
            int low, high, current, compare;

            low = 0;
            high = list.Count - 1;
            while (low <= high)
            {
                current = (low + high) >> 1;
                compare = list[current] - value;
                if (compare < 0)
                    low = current + 1;
                else
                {
                    high = current - 1;
                    if (compare == 0)
                    {
                        index = current;
                        return true;
                    }
                }
            }

            index = low;
            return false;
        }

        public static bool AddIntToSortedList(IList<int> list, int value)
        {
            if (FindSortedIntIndex(list, value, out int index))
                return false;

            if (index >= 0)
                list.Insert(index, value);
            else
                list.Add(value);

            return true;
        }

        public static string GetExcelSheetName(string sheetName, IList<string> sheetNames)
        {
            //A sheet name must be 31 or fewer characters and cannot contain the characters [ ] * ? / \ .
            if (string.IsNullOrWhiteSpace(sheetName))
                sheetName = "Sheet ";
            sheetName = Utils.GetValidName(sheetName, "Sheet ", new char[] { '[', ']', '*', '?', '/', '\\' });
            if (sheetName.Length > 24)
                sheetName = sheetName[..24];

            string result = Utils.AddUniqueString(sheetNames, sheetName, StringComparison.CurrentCultureIgnoreCase, true);
            return result;
        }

        public static string GetManifestResourceString(Assembly assembly, string ResourceName)
        {
            using var s = assembly.GetManifestResourceStream(ResourceName);
            var sr      = new StreamReader(s);
            return sr.ReadToEnd();
        }

        [Flags]
        public enum SplitStringOptions
        {
            None          = 0x0,
            EnableQuotes  = 0x1,
            KeyUpper      = 0x2,
            KeyLower      = 0x4
            //KeyTrim			= 0x8
        }

        private static string ProcessSplitStringOptions(string str, SplitStringOptions options)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if ((options & SplitStringOptions.KeyUpper) != 0)
                str = str.ToUpper();
            else if ((options & SplitStringOptions.KeyLower) != 0)
                str = str.ToLower();
            //if ((options & SplitStringOptions.KeyTrim) != 0)
            //	str = str.Trim();

            return str;
        }

        public static string[] SplitString(string str, char Delimiter)
        {
            return SplitString(str, Delimiter, SplitStringOptions.None);
        }

        public static string[] SplitString(string str, char Delimiter, SplitStringOptions options)
        {
            if (string.IsNullOrEmpty(str))
                return Array.Empty<string>();

            var result = new List<string>();

            int Start, i;
            string value;
            char Quote;
            char CloseQuote    = '\0';
            bool inQuotes      = false;

            Start = 0;

            i = 0;
            while (i < str.Length)
            {
                if (str[i] < ' ' && str[i] != Delimiter)
                {
                    i++;
                    continue;
                }

                switch (str[i])
                {
                    case '"':
                    case '\'':
                    case '`':
                    case '[':
                    case ']':
                        if (/*firstPartChar &&*/ (options & SplitStringOptions.EnableQuotes) > 0)
                        {
                            if (inQuotes)
                            {
                                if ((str[i] == CloseQuote) && (i >= str.Length - 1 || str[i + 1] != CloseQuote))
                                    inQuotes = false;
                                else if (i < str.Length - 1 && str[i + 1] == CloseQuote)
                                    i++;
                            }
                            else
                            {
                                if (str[i] != ']')
                                {
                                    Quote      = str[i];
                                    CloseQuote = Quote != '[' ? Quote : ']';
                                    inQuotes   = true;
                                }
                                break;
                            }
                        }
                        break;
                }

                if (i < str.Length && str[i] == Delimiter)
                {
                    if (!inQuotes)
                    {
                        value = str[Start..i];
                        value = TrimString(value);
                        if ((options & SplitStringOptions.EnableQuotes) > 0 && CloseQuote != '\0')
                            value = value.Replace(string.Concat(CloseQuote, CloseQuote), CloseQuote.ToString());
                        value = ProcessSplitStringOptions(value, options);
                        value = UnquoteString(value);
                        result.Add(value);
                        Start = i + 1;
                    }
                }

                i++;
            }

            //Process last part.
            if (i > Start)
            {
                value = str[Start..Math.Min(i, str.Length)];
                value = TrimString(value);
                if ((options & SplitStringOptions.EnableQuotes) > 0 && CloseQuote != '\0')
                    value = value.Replace(string.Concat(CloseQuote, CloseQuote), CloseQuote.ToString());
                value = ProcessSplitStringOptions(value, options);
                value = UnquoteString(value);
                result.Add(value);
            }

            return result.ToArray();
        }

        public static StringNoCaseDictionary<string> SplitNameValueString(string str, char Delimiter)
        {
            return SplitNameValueString(str, Delimiter, SplitStringOptions.EnableQuotes);
        }

        public static StringNoCaseDictionary<string> SplitNameValueString(string str, char Delimiter,
            SplitStringOptions options)
        {
            var result = new StringNoCaseDictionary<string>();

            if (string.IsNullOrEmpty(str))
                return result;

            string[] parts = SplitString(str, Delimiter, options & ~(SplitStringOptions.KeyLower | SplitStringOptions.KeyUpper));
            for (int i = 0; i < parts.Length; i++)
            {
                string part = Utils.TrimString(parts[i]);

                if (string.IsNullOrEmpty(part))
                    continue;

                int p = part.IndexOf('=');
                if (p > 0)
                {
                    string name  = Utils.TrimString(part[..p]);
                    string value = Utils.TrimString(part[(p + 1)..]);

                    name  = ProcessSplitStringOptions(name, options);
                    value = Utils.UnpackString(value);

                    result[name] = value;
                }
                else
                {
                    part = ProcessSplitStringOptions(part, options);
                    result[part] = string.Empty;
                }
            }

            return result;
        }

        public static Stream GetEmbeddedResource(Assembly Assembly, string ResourceName)
        {
            string FullResourceName = $"{Assembly.GetName().Name}.{ResourceName}";
            FullResourceName = FullResourceName.Replace("SpreadCommander", "SpreadCommander");
            return Assembly.GetManifestResourceStream(FullResourceName);
        }

        public static string GetEmbeddedResourceString(Assembly Assembly, string ResourceName)
        {
            var s        = GetEmbeddedResource(Assembly, ResourceName);
            using var sr = new StreamReader(s);
            return sr.ReadToEnd();
        }

        public static string[] GetEmbeddedResourceStrings(Assembly Assembly, string ResourceName)
        {
            var s      = GetEmbeddedResource(Assembly, ResourceName);
            var result = new List<string>();
            using (var sr = new StreamReader(s))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                        break;
                    result.Add(line);
                }
            }
            return result.ToArray();
        }

        public static Image GetEmbeddedResourceImage(Assembly Assembly, string ResourceName)
        {
            Stream stream = GetEmbeddedResource(Assembly, ResourceName);
            return Image.FromStream(stream);
        }

        public static int ReadStreamToBuffer(Stream stream, byte[] buffer) =>
            ReadStreamToBuffer(stream, buffer, 0, buffer.Length);

        public static int ReadStreamToBuffer(Stream stream, byte[] buffer, int start, int len) =>
            ReadStreamToBuffer(stream, new Span<byte>(buffer, start, len));

        public static int ReadStreamToBuffer(Stream stream, Span<byte> buffer)
        {
            int totalRead = 0;
            while (totalRead < buffer.Length)
            {
                int bytesRead = stream.Read(buffer[totalRead..]);
                if (bytesRead == 0) break;
                totalRead += bytesRead;
            }
            return totalRead;
        }

        public static void PlayEmbeddedSound(Assembly Assembly, string SoundName)
        {
            Stream s = GetEmbeddedResource(Assembly, SoundName);
            if (s == null)
                return;

            byte[] data = new byte[s.Length];
            ReadStreamToBuffer(s, data);

            WinAPI.PlaySound(data);
        }

        public static void PlayEmbeddedSoundSync(Assembly Assembly, string SoundName)
        {
            Stream s = GetEmbeddedResource(Assembly, SoundName);
            if (s == null)
                return;

            byte[] data = new byte[s.Length];
            ReadStreamToBuffer(s, data);

            WinAPI.PlaySoundSync(data);
        }

        public static void CreateDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentException("Directory is not specified.", nameof(directory));

            System.IO.Directory.CreateDirectory(directory);
        }

        public static void CreateDirectoryForFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename is not specified", nameof(fileName));

            var dir = Path.GetDirectoryName(fileName);
            CreateDirectory(dir);
        }

        public static string QuoteString(string str, char QuoteChar) =>
            QuoteString(str, QuoteChar.ToString());
        public static string QuoteString(string str, string QuoteChar)
        {
            string closeQuoteChar = QuoteChar != "[" ? QuoteChar : "]";

            str ??= string.Empty;
            str = str.Replace(closeQuoteChar, closeQuoteChar + closeQuoteChar);
            return $"{QuoteChar}{str}{closeQuoteChar}";
        }

        public static bool NeedQuoteString(string str, string QuoteChar, bool QuoteEmptyString)
        {
            if (string.IsNullOrEmpty(str) || str.Contains(QuoteChar))
                return QuoteEmptyString;

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (!char.IsLetterOrDigit(c))
                    return true;
            }

            return false;
        }

        public static string QuoteStringIfNeeded(string str, string QuoteChar, bool QuoteEmptyString)
        {
            return NeedQuoteString(str, QuoteChar, QuoteEmptyString) ? QuoteString(str, QuoteChar) : str;
        }

        public static string GetBackupFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;

            return Path.ChangeExtension(fileName, ".bak");
        }

        public static string BackupFile(string fileName)
        {
            return BackupFile(fileName, false);
        }

        public static string BackupFile(string fileName, bool deleteCurrentFile)
        {
            string backupFileName = GetBackupFileName(fileName);
            if (string.IsNullOrEmpty(backupFileName))
                return null;

            if (File.Exists(backupFileName))
                File.Delete(backupFileName);

            if (deleteCurrentFile)
                File.Move(fileName, backupFileName);
            else
                File.Copy(fileName, backupFileName);

            return backupFileName;
        }

        private static XmlWriterSettings CreateXmlWriterSettings()
        {
            var writerSettings = new XmlWriterSettings()
            {
                Encoding            = Encoding.UTF8,
                Indent              = true,
                IndentChars         = "\t",
                NewLineHandling     = NewLineHandling.Entitize,
                NewLineOnAttributes = true
            };

            return writerSettings;
        }

        public static void SaveXmlDocument(XmlDocument doc, Stream stream)
        {
            var writerSettings = CreateXmlWriterSettings();

            var writer = XmlWriter.Create(stream, writerSettings);
            try
            {
                doc.Save(writer);
            }
            finally
            {
                writer.Close();
            }
        }

        public static void SaveXmlDocument(XmlDocument doc, string fileName)
        {
            var writerSettings = CreateXmlWriterSettings();

            var writer = XmlWriter.Create(fileName, writerSettings);
            try
            {
                doc.Save(writer);
            }
            finally
            {
                writer.Close();
            }
        }

        public static XmlElement FindChildNode(XmlElement elRoot, string nodeName, bool CanCreate)
        {
            XmlNode result = elRoot.FirstChild;
            while (result != null)
            {
                if (result is XmlElement element && result.Name == nodeName)
                    return element;

                result = result.NextSibling;
            }

            if (CanCreate)
            {
                XmlElement elProperties = elRoot.OwnerDocument.CreateElement(nodeName);
                elRoot.AppendChild(elProperties);
                return elProperties;
            }

            return null;
        }

        public static string ReadPropertyFromXml(XmlDocument doc, string propName)
        {
            if (doc == null)
                return null;
            return ReadPropertyFromXml(doc.DocumentElement, propName);
        }

        public static string ReadPropertyFromXml(XmlElement root, string propName)
        {
            if (root == null)
                return null;

            XmlElement elProperties = FindChildNode(root, "Properties", false);
            if (elProperties == null)
                return null;

            XmlNode elResult = elProperties.FirstChild;
            while (elResult != null)
            {
                if (elResult is XmlElement element && elResult.Name == propName)
                    return element.InnerText;

                elResult = elResult.NextSibling;
            }

            return null;
        }

        public static int ReadPropertyIntFromXml(XmlDocument doc, string propName, int defaultValue)
        {
            if (doc == null)
                return defaultValue;
            return ReadPropertyIntFromXml(doc.DocumentElement, propName, defaultValue);
        }

        public static int ReadPropertyIntFromXml(XmlElement root, string propName, int defaultValue)
        {
            string value = ReadPropertyFromXml(root, propName);
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            if (!int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out int result))
                result = defaultValue;

            return result;
        }

        public static void WritePropertyToXml(XmlDocument doc, string propName, string value)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));
            WritePropertyToXml(doc.DocumentElement, propName, value);
        }

        public static void WritePropertyToXml(XmlElement root, string propName, string value)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            XmlElement elProperties = FindChildNode(root, "Properties", true);
            XmlElement elProp = FindChildNode(elProperties, propName, true);
            elProp.InnerText = NonNullString(value);
            elProperties.AppendChild(elProp);
        }

        public static void WritePropertyIntToXml(XmlDocument doc, string propName, int value)
        {
            if (doc == null)
                throw new ArgumentNullException(nameof(doc));
            WritePropertyIntToXml(doc.DocumentElement, propName, value);
        }

        public static void WritePropertyIntToXml(XmlElement root, string propName, int value)
        {
            string strValue = value.ToString(CultureInfo.InvariantCulture);
            WritePropertyToXml(root, propName, strValue);
        }

        public static string[] SplitStringToLines(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<string>();

            int i, start, len;
            bool lineEnd;

            var result = new List<string>();

            len = value.Length;
            i = 0;
            while (i < len)
            {
                start = i;
                while (i < len)
                {
                    lineEnd = false;
                    switch (value[i])
                    {
                        case '\0':
                        case '\r':
                        case '\n':
                            lineEnd = true;
                            break;
                    }

                    if (lineEnd)
                        break;

                    i++;
                }

                string str = value[start..i];
                result.Add(str);

                if (i < len && value[i] == '\r')
                    i++;
                if (i < len && value[i] == '\n')
                    i++;
                if (i < len && value[i] == '\0')
                    i++;
            }

            return result.ToArray();
        }

        public static void ClearDataSet(DataSet dataSet)
        {
            dataSet.Clear();
            dataSet.Relations.Clear();
            foreach (DataTable table in dataSet.Tables)
            {
                for (int i = table.Constraints.Count - 1; i >= 0; i--)
                {
                    Constraint constraint = table.Constraints[i];
                    if (constraint is ForeignKeyConstraint)
                        table.Constraints.RemoveAt(i);
                }
            }
            foreach (DataTable table in dataSet.Tables)
                table.Constraints.Clear();
            dataSet.Tables.Clear();
            dataSet.ExtendedProperties.Clear();
        }

        public static string FontToString(Font value)
        {
            if (value == null)
                return null;

            string result = string.Format("{0}, {1:#.##}", value.Name, value.SizeInPoints);
            if (value.Bold)
                result += ", Bold";
            if (value.Italic)
                result += ", Italic";
            if (value.Strikeout)
                result += ", Strikeout";
            if (value.Underline)
                result += ", Underline";
            return result;
        }

        public static Font StringToFont(string value, out Color color)
        {
            color = Color.Empty;

            if (string.IsNullOrWhiteSpace(value))
                return null;

            value           = Utils.NonNullString(value);
            string[] values = Utils.SplitString(value, ',');
            string fontName = values.Length > 0 ? values[0] : "Tahoma";
            string strSize  = values.Length > 1 ? values[1] : "8.25";
            if (!float.TryParse(strSize, out float fontSize) || fontSize < 4.0 || fontSize > 100)
                fontSize = 8.25F;
            FontStyle fontStyle = StringToFontStyle(value);

            for (int i = values.Length - 1; i >= 0; i--)
            {
                var fontColor = Utils.ColorFromString(values[i]);
                if (fontColor != Color.Empty)
                {
                    color = fontColor;
                    break;
                }
            }

            try
            {
                return new Font(fontName, fontSize, fontStyle);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void StringToSpreadsheetFont(string value, SpreadsheetFont font)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;	//Use font as is

            value           = Utils.NonNullString(value);
            string[] values = Utils.SplitString(value, ',');
            string fontName = values.Length > 0 ? values[0] : "Tahoma";
            string strSize  = values.Length > 1 ? values[1] : "8.25";
            if (!float.TryParse(strSize, out float fontSize) || fontSize < 4.0 || fontSize > 100)
                fontSize = 8.25F;
            FontStyle fontStyle = StringToFontStyle(value);

            font.Name          = fontName;
            font.Size          = fontSize;
            font.Bold          = (fontStyle & FontStyle.Bold) == FontStyle.Bold;
            font.Italic        = (fontStyle & FontStyle.Italic) == FontStyle.Italic;
            font.UnderlineType = (fontStyle & FontStyle.Underline) == FontStyle.Underline ? DevExpress.Spreadsheet.UnderlineType.Single : DevExpress.Spreadsheet.UnderlineType.None;

            for (int i = values.Length - 1; i >= 0; i--)
            {
                var color = Utils.ColorFromString(values[i]);
                if (color != Color.Empty)
                {
                    font.Color = color;
                    break;
                }
            }
        }

        public static void StringToShapeTextFont(string value, ShapeTextFont font)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;	//Use font as is

            value           = Utils.NonNullString(value);
            string[] values = Utils.SplitString(value, ',');
            string fontName = values.Length > 0 ? values[0] : "Tahoma";
            string strSize  = values.Length > 1 ? values[1] : "8.25";
            if (!float.TryParse(strSize, out float fontSize) || fontSize < 4.0 || fontSize > 100)
                fontSize = 8.25F;
            FontStyle fontStyle = StringToFontStyle(value);

            font.Name          = fontName;
            font.Size          = fontSize;
            font.Bold          = (fontStyle & FontStyle.Bold) == FontStyle.Bold;
            font.Italic        = (fontStyle & FontStyle.Italic) == FontStyle.Italic;
            font.UnderlineType = (fontStyle & FontStyle.Underline) == FontStyle.Underline ? ShapeTextUnderlineType.Single : ShapeTextUnderlineType.None;

            for (int i = values.Length - 1; i >= 0; i--)
            {
                var color = Utils.ColorFromString(values[i]);
                if (color != Color.Empty)
                {
                    font.Color = color;
                    break;
                }
            }
        }

        public static void StringToCharacterPropertiesFont(string value, CharacterPropertiesBase style)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            Color color = Color.Empty;

            value           = Utils.NonNullString(value);
            string[] values = Utils.SplitString(value, ',');
            string fontName = values.Length > 0 ? values[0] : "Tahoma";
            string strSize  = values.Length > 1 ? values[1] : "8.25";
            if (!float.TryParse(strSize, out float fontSize) || fontSize < 4.0 || fontSize > 100)
                fontSize = 8.25F;
            FontStyle fontStyle = StringToFontStyle(value);

            for (int i = values.Length - 1; i >= 0; i--)
            {
                var fontColor = Utils.ColorFromString(values[i]);
                if (fontColor != Color.Empty)
                {
                    color = fontColor;
                    break;
                }
            }

            style.FontName  = fontName;
            style.FontSize  = fontSize;
            style.ForeColor = color;
            style.Bold      = (fontStyle & FontStyle.Bold) > 0;
            style.Italic    = (fontStyle & FontStyle.Italic) > 0;
            style.Strikeout = (fontStyle & FontStyle.Strikeout) > 0 ? StrikeoutType.Single : StrikeoutType.None;
            style.Underline = (fontStyle & FontStyle.Underline) > 0 ? DevExpress.XtraRichEdit.API.Native.UnderlineType.Single : DevExpress.XtraRichEdit.API.Native.UnderlineType.None;
        }

        public static FontStyle StringToFontStyle(string value)
        {
            FontStyle fontStyle = FontStyle.Regular;
            if (string.IsNullOrWhiteSpace(value))
                return fontStyle;

            if (value.Contains("Bold", StringComparison.CurrentCultureIgnoreCase))
                fontStyle |= FontStyle.Bold;
            if (value.Contains("Italic", StringComparison.CurrentCultureIgnoreCase))
                fontStyle |= FontStyle.Italic;
            if (value.Contains("Strikeout", StringComparison.CurrentCultureIgnoreCase))
                fontStyle |= FontStyle.Strikeout;
            if (value.Contains("Underline", StringComparison.CurrentCultureIgnoreCase))
                fontStyle |= FontStyle.Underline;

            return fontStyle;
        }

        public static string ColorToString(Color value)
        {
            return value.ToHtmlColor();
        }

        public static Color ColorFromString(string value)
        {
            value = TrimString(Utils.NonNullString(value));
            value = UnpackString(value);
            if (string.IsNullOrEmpty(value))
                return Color.Empty;

            try
            {
                return ColorExtensions.FromHtmlColor(value);
            }
            catch (Exception)
            {
                return Color.Empty;
            }
        }

        public static string TimeSpanToString(TimeSpan value)
        {
            var text = new StringBuilder();
            if (value < TimeSpan.Zero)
            {
                text.Append('-');
                value = value.Negate();
            }

            if (value.Days > 0)
            {
                text.Append(value.Days);
                text.Append('.');
            }

            string hourFormat = text.Length > 0 ? "{0:00}" : "{0}";
            text.AppendFormat(hourFormat, value.Hours);
            text.Append(':');

            text.Append($"{value.Minutes:00}");
            text.Append(':');

            text.Append($"{value.Seconds:00}");
            text.Append('.');
            text.Append($"{value.Milliseconds:000}");

            return text.ToString();
        }

        public static DbConnection CreateDbConnection(string provider, string connectionString)
        {
            if (string.IsNullOrEmpty(provider))
                return null;

            DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
            if (factory == null)
                return null;

            DbConnection result = factory.CreateConnection();
            if (!string.IsNullOrEmpty(connectionString))
                result.ConnectionString = connectionString;

            return result;
        }

        public static bool IsStringEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }

            return true;
        }

        public static object ExecuteScalar(DbConnection connection, string commandText)
        {
            using DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            return cmd.ExecuteScalar();
        }

        public static int ExecuteNonQuery(DbConnection connection, string commandText)
        {
            using DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            return cmd.ExecuteNonQuery();
        }

        public static int ValueInRange(int value, int min, int max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }

        public static bool IsValueInRange(int value, int min, int max)
        {
            return (value >= min && value <= max);
        }

        public static string GetSubDirectory(string rootDirectory, string directory)
        {
            if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(rootDirectory) ||
                !directory.StartsWith(rootDirectory, StringComparison.CurrentCultureIgnoreCase))
                return directory;

            int index = rootDirectory.Length;
            while (index < directory.Length)
            {
                bool done = false;
                switch (directory[index])
                {
                    case '\\':
                    case '/':
                        index++;
                        break;
                    default:
                        done = true;
                        break;
                }
                if (done)
                    break;
            }

            if (index >= directory.Length)
                return null;
            return directory[index..];
        }

        public static string[] GetSubFolders(string rootDirectory)
        {
            if (string.IsNullOrEmpty(rootDirectory))
                return Array.Empty<string>();

            var dir = new DirectoryInfo(rootDirectory);
            if (!dir.Exists)
                return Array.Empty<string>();

            var infos  = dir.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
            var result = new string[infos.Length];
            for (int i = 0; i < infos.Length; i++)
                result[i] = infos[i].Name;

            return result;
        }

        public static bool IsStringValid(string value, char[] invalidCharacters)
        {
            if (string.IsNullOrEmpty(value) || invalidCharacters == null || invalidCharacters.Length <= 0)
                return true;

            return (value.IndexOfAny(invalidCharacters) < 0);
        }

        public static bool IsFileNameValid(string fileName)
        {
            return IsStringValid(fileName, Path.GetInvalidFileNameChars());
        }

        public static bool IsPathNameValid(string pathName)
        {
            return IsStringValid(pathName, Path.GetInvalidPathChars());
        }

        public static bool IsFullFileNameValid(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
                return false;

            var dir = Path.GetDirectoryName(fullFileName);
            var fileName = Path.GetFileName(fullFileName);

            return (string.IsNullOrEmpty(dir) || IsPathNameValid(dir)) && IsFileNameValid(fileName);
        }

        public static string GetValidName(string initialName, string defaultName, char[] invalidChars)
        {
            if (string.IsNullOrWhiteSpace(initialName))
                return defaultName;

            initialName = initialName.Trim();
            if (invalidChars == null || invalidChars.Length <= 0)
                return initialName;

            var result = new StringBuilder();
            foreach (char c in initialName)
            {
                char newc = c;
                if (Array.IndexOf<char>(invalidChars, c) >= 0)
                    newc = '_';
                result.Append(newc);
            }
            return result.ToString();
        }

        public static string GetValidFileName(string initialName)
        {
            if (string.IsNullOrWhiteSpace(initialName))
                return "noname";

            initialName = initialName.Trim();
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (invalidChars == null || invalidChars.Length <= 0)
                return initialName;

            var result = new StringBuilder();
            foreach (char c in initialName)
            {
                char newc = c;
                if (Array.IndexOf<char>(invalidChars, c) >= 0)
                    newc = '_';
                result.Append(newc);
            }
            return result.ToString();
        }

        public static int GetLastNumIndex(string str, out string baseName, out int numSuffix)
        {
            baseName = str;
            numSuffix = 1;

            if (string.IsNullOrEmpty(str))
                return -1;

            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (!char.IsNumber(str[i]))
                {
                    baseName = str[..(i + 1)];
                    if (i < str.Length - 1)
                        numSuffix = int.Parse(str[(i + 1)..]);
                    return i + 1;
                }
            }

            return -1;
        }

        public static string GetUniqueFileName(string initialFileName)
        {
            if (!File.Exists(initialFileName))
                return initialFileName;

            string dir = Path.GetDirectoryName(initialFileName);
            string fileName = Path.GetFileNameWithoutExtension(initialFileName);
            string ext = Path.GetExtension(initialFileName);

            if (!Directory.Exists(dir))
            {
                CreateDirectory(dir);
                return initialFileName;
            }

            int counter = 1;
            while (true)
            {
                string result = $"{fileName}_{counter}{ext}";
                result = Path.Combine(dir, result);

                if (!File.Exists(result))
                    return result;

                counter++;
            }
        }

        public static bool ContainsStringKey(Hashtable hash, string key, bool caseSensitive)
        {
            if (hash == null || key == null)
                return false;

            if (caseSensitive)
                return hash.ContainsKey(key);

            foreach (object k in hash.Keys)
            {
                if (k != null && k is string str && string.Compare(str, key, true) == 0)
                    return true;
            }

            return false;
        }

        public static bool ContainsStringKey<T>(Dictionary<string, T> dict, string key, bool caseSensitive)
        {
            if (dict == null || key == null)
                return false;

            if (caseSensitive)
                return dict.ContainsKey(key);

            foreach (string k in dict.Keys)
            {
                if (k != null && string.Compare(k, key, true) == 0)
                    return true;
            }

            return false;
        }

        public static bool ContainsString(IList<string> list, string value, StringComparison comparison)
        {
            if (list == null)
                return false;

            foreach (var item in list)
            {
                if (string.Compare(item, value, comparison) == 0)
                    return true;
            }

            return false;
        }

        public static bool IsRtf(string str)
        {
            if (str == null)
                return false;

            return str.StartsWith("{\\rtf");
        }

        public static string DecorateDisplayText(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var builder = new StringBuilder(value);
            int i = 0;
            bool spaceNeeded = false;
            while (i < builder.Length)
            {
                char c = builder[i];
                if (spaceNeeded && char.IsUpper(c))
                {
                    builder.Insert(i++, ' ');
                    i++;
                }

                spaceNeeded = !char.IsWhiteSpace(c);

                i++;
            }

            return builder.ToString();
        }

        public static void CollectGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                WinAPI.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, new IntPtr(-1), new IntPtr(-1));
        }

        public static byte[] Encrypt(byte[] clearData)
        {
            return Encrypt(clearData, DefaultPassword);
        }

        public static byte[] Encrypt(byte[] clearData, string password)
        {
            using var ms = new MemoryStream();
            using (Aes aes = Aes.Create())
            {
                using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(PasswordSalt)))
                {
                    aes.Key = deriveBytes.GetBytes(128 / 8);
                    ms.Write(BitConverter.GetBytes(aes.IV.Length), 0, sizeof(int));
                    ms.Write(aes.IV, 0, aes.IV.Length);
                }

                using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] rawPlaintext = clearData;
                cs.Write(rawPlaintext, 0, clearData.Length);
                cs.FlushFinalBlock();
            }

            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        public static string Encrypt(string clearText)
        {
            return Encrypt(clearText, DefaultPassword);
        }

        public static string Encrypt(string clearText, string password)
        {
            if (string.IsNullOrEmpty(clearText))
                return string.Empty;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            byte[] encryptedData = Encrypt(clearBytes, password);
            return Convert.ToBase64String(encryptedData);
        }

        public static byte[] Decrypt(byte[] cipherData)
        {
            return Decrypt(cipherData, DefaultPassword);
        }

        public static byte[] Decrypt(byte[] cipherData, string password)
        {
            using var ms = new MemoryStream();
            ms.Write(cipherData, 0, cipherData.Length);
            ms.Seek(0, SeekOrigin.Begin);

            using Aes aes = Aes.Create();
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(PasswordSalt)))
            {
                aes.Key = deriveBytes.GetBytes(128 / 8);
                // Get the initialization vector from the encrypted stream
                aes.IV = ReadDecryptByteArray(ms);
            }

            using var cs     = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            byte[] buffer    = new byte[4096];
            using var result = new MemoryStream();
            while (true)
            {
                int bytesRead = ReadStreamToBuffer(cs, buffer);
                result.Write(buffer, 0, bytesRead);
                if (bytesRead < buffer.Length)
                    break;
            }

            result.Seek(0, SeekOrigin.Begin);
            return result.ToArray();


            static byte[] ReadDecryptByteArray(Stream stream)
            {
                byte[] rawLength = new byte[sizeof(int)];
                if (ReadStreamToBuffer(stream, rawLength) != rawLength.Length)
                    throw new Exception("Stream did not contain properly formatted byte array");

                int bufferLength = BitConverter.ToInt32(rawLength, 0);
                if (bufferLength > 1024)
                    throw new SystemException("Did not read byte array properly");

                byte[] buffer = new byte[bufferLength];

                if (ReadStreamToBuffer(stream, buffer) != buffer.Length)
                    throw new Exception("Did not read byte array properly");

                return buffer;
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;
            return Decrypt(cipherText, DefaultPassword);
        }

        public static string Decrypt(string cipherText, string password)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedData = Decrypt(cipherBytes, password);
                return Encoding.Unicode.GetString(decryptedData, 0, decryptedData.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string EncryptAndEncode(byte[] clearData)
        {
            return EncryptAndEncode(clearData, DefaultPassword);
        }

        public static string EncryptAndEncode(byte[] clearData, string password)
        {
            byte[] data = Encrypt(clearData, password);
            string result = Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks);
            return result;
        }

        public static byte[] DecodeAndDecrypt(string cipherString)
        {
            return DecodeAndDecrypt(cipherString, DefaultPassword);
        }

        public static byte[] DecodeAndDecrypt(string cipherString, string password)
        {
            byte[] cipherData = Convert.FromBase64String(TrimString(cipherString));
            byte[] result = Decrypt(cipherData, password);
            return result;
        }

        public static string ProtectUserString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            
            var bytes  = Encoding.Unicode.GetBytes(value);
            var result = ProtectedData.Protect(bytes, Encoding.ASCII.GetBytes(PasswordSalt), DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(result);
        }

        public static string UnProtectUserString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var bytes  = Convert.FromBase64String(TrimString(value));
            var result = ProtectedData.Unprotect(bytes, Encoding.ASCII.GetBytes(PasswordSalt), DataProtectionScope.CurrentUser);

            return Encoding.Unicode.GetString(result);
        }

        public static void CopyStream(Stream source, Stream dest)
        {
            byte[] buffer = new byte[65536];
            int bytesRead;
            do
            {
                bytesRead = ReadStreamToBuffer(source, buffer);
                if (bytesRead > 0)
                    dest.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
        }

        public static bool IsTypeNumeric(Type t)
        {
            if (t == null)
                return false;

            return (Type.GetTypeCode(t)) switch
            {
                TypeCode.Byte or 
                TypeCode.Decimal or 
                TypeCode.Double or 
                TypeCode.Int16 or 
                TypeCode.Int32 or 
                TypeCode.Int64 or 
                TypeCode.SByte or 
                TypeCode.Single or 
                TypeCode.UInt16 or 
                TypeCode.UInt32 or 
                TypeCode.UInt64 => true,
                _ => false,
            };
        }

        public static T FindTypedParentControl<T>(Control control)
            where T : Control
        {
            if (control == null)
                return null;

            Control parent = control.Parent;
            while (parent != null)
            {
                if (parent is T t)
                    return t;

                parent = parent.Parent;
            }

            return null;
        }

        public static Guid XorGuids(Guid guid1, Guid guid2)
        {
            const int BYTECOUNT = 16;
            byte[] destByte = new byte[BYTECOUNT];
            byte[] guid1Byte = guid1.ToByteArray();
            byte[] guid2Byte = guid2.ToByteArray();

            for (int i = 0; i < BYTECOUNT; i++)
                destByte[i] = (byte)(guid1Byte[i] ^ guid2Byte[i]);

            return new Guid(destByte);
        }

        [DebuggerStepThrough()]
        public static string SerializeObjectToText<T>(T value)
            where T : class
        {
            if (value == null)
                return null;

            using TextWriter writer = new StringWriter();
            //Serialize descendant type too
            //var serializer = new XmlSerializer(typeof(T));
            var serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(writer, value);
            return writer.ToString();
        }

        [DebuggerStepThrough()]
        public static string SerializeObjectToText(object value)
        {
            if (value == null)
                return null;

            using var writer = new StringWriter();
            var serializer   = new XmlSerializer(value.GetType());
            serializer.Serialize(writer, value);
            return writer.ToString();
        }

        [DebuggerStepThrough()]
        public static void SerializeObjectToStream<T>(T value, Stream stream)
            where T : class
        {
            if (value == null)
                return;

            //var serializer = new XmlSerializer(typeof(T));
            var serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(stream, value);
        }

        [DebuggerStepThrough()]
        public static void SerializeObjectToFile<T>(T value, string fileName)
            where T : class
        {
            using var fileStream = new FileStream(fileName, FileMode.Create);
            SerializeObjectToStream<T>(value, fileStream);
        }

        [DebuggerStepThrough()]
        public static T DeserializeObject<T>(Stream stream)
            where T : class
        {
            using var xmlReader = new XmlTextReader(stream) { Normalization = false };
            var serializer      = new XmlSerializer(typeof(T));
            object result = serializer.Deserialize(xmlReader);
            return result as T;
        }

        [DebuggerStepThrough()]
        public static T DeserializeObjectFromFile<T>(string fileName)
            where T : class
        {
            using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            return DeserializeObject<T>(fileStream);
        }

        [DebuggerStepThrough()]
        public static object DeserializeObject(Type type, Stream stream)
        {
            using var xmlReader = new XmlTextReader(stream) { Normalization = false };
            var serializer      = new XmlSerializer(type);
            object result = serializer.Deserialize(xmlReader);
            return result;
        }


        [DebuggerStepThrough()]
        public static T DeserializeObjectFromText<T>(string text)
            where T : class
        {
            using var reader    = new StringReader(text);
            using var xmlReader = new XmlTextReader(reader) { Normalization = false };
            var serializer      = new XmlSerializer(typeof(T));
            object result = serializer.Deserialize(xmlReader);
            return result as T;
        }

        [DebuggerStepThrough()]
        public static object DeserializeObjectFromText(Type objectType, string text)
        {
            using var reader = new StringReader(text);
            return DeserializeObjectFromTextReader(objectType, reader);
        }

        [DebuggerStepThrough()]
        public static object DeserializeObjectFromTextReader(Type objectType, TextReader reader)
        {
            using var xmlReader = new XmlTextReader(reader) { Normalization = false };
            var serializer      = new XmlSerializer(objectType);
            object result       = serializer.Deserialize(xmlReader);
            return result;
        }

        public static void InitiateProfiling()
        {
            string profileRoot = Path.Combine(Parameters.ApplicationDataFolder, "Profiles");
            Directory.CreateDirectory(profileRoot);

            ProfileOptimization.SetProfileRoot(profileRoot);
        }

        public static void StartProfile(string profileName)
        {
            string fullProfileName = $"{profileName}_{(Environment.Is64BitProcess ? "x64" : "x86")}.profile";
            ProfileOptimization.StartProfile(fullProfileName);
        }

        public static T ChangeType<T>(object value, T defaultValue = default)
        {
            try
            {
                if (value == null || value == DBNull.Value)
                    return defaultValue;

                var valueType    = typeof(T);
                var nullableType = Nullable.GetUnderlyingType(valueType);
                if (nullableType != null)
                    valueType = nullableType;

                if (valueType == value.GetType())
                    return (T)value;

                if (valueType.IsEnum)
                {
                    if (value is string)
                        return (T)Enum.Parse(valueType, value as string);
                    else
                        return (T)Enum.ToObject(valueType, value);
                }

                if (!valueType.IsInterface && valueType.IsGenericType)
                {
                    Type innerType    = valueType.GetGenericArguments()[0];
                    object innerValue = ChangeType(value, innerType);
                    return (T)Activator.CreateInstance(valueType, new object[] { innerValue });
                }

                if (value is string && valueType == typeof(Guid))
                    return (T)(object)new Guid(value as string);

                if (value is string && valueType == typeof(Version))
                    return (T)(object)new Version(value as string);

                if (value is string && valueType == typeof(System.Drawing.Color))
                    return (T)(object)ColorExtensions.FromHtmlColor(value as string);

                if (value is not IConvertible)
                    return (T)value;

                return (T)Convert.ChangeType(value, valueType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        //Throws exception if cannot convert
        public static object ConvertValue(Type type, object value, object defaultValue = null)
        {
            if (value == null || value == DBNull.Value)
                return defaultValue;

            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                type = nullableType;

            if (type == value.GetType())
                return value;

            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }

            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }

            if (value is string && type == typeof(Guid))
                return (object)new Guid(value as string);

            if (value is string && type == typeof(Version))
                return (object)new Version(value as string);

            if (value is not IConvertible)
                return value;

            if (value is string str && type == typeof(Color))
            {
                if (Enum.TryParse<KnownColor>(str, out KnownColor knownColor))
                    return (object)Color.FromKnownColor(knownColor);
                else
                    return defaultValue;
            }

            var result = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            return result;
        }

        //Returns default value if cannot convert
        public static object ChangeType(Type type, object value, object defaultValue)
        {
            try
            {
                var result = ConvertValue(type, value, defaultValue);
                return result;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static DataTable ConvertToDataTable(object dataSource, string tableName = "Table")
        {
            var source = dataSource;
            if (dataSource is IList)
            {
                //Do nothing, use as is
            }
            else if (dataSource is IListSource listSource)
            {
                source = listSource.GetList();
            }

            if (source is not ITypedList typedSource)
                throw new Exception("Cannot convert data source into data table");

            var properties = typedSource.GetItemProperties(null);


            var result = new DataTable(tableName);
            foreach (PropertyDescriptor property in properties)
                result.Columns.Add(property.Name, property.PropertyType);

            foreach (var item in (IList)typedSource)
            {
                var row = result.NewRow();
                for (int i = 0; i < properties.Count; i++)
                    row[i] = properties[i].GetValue(item);
                result.Rows.Add(row);
            }

            return result;
        }

        //Need to convert Value to correct type after getting DataFilterCondition
        public static DataFilter CreateDataCondition(string condition)
        {
            var regex = new Regex(@"^(?<Column>.*?)\s*(?<Operator>(?:=|>=|>|<=|<|!=|<>))\s*(?<Value>.*)$");
            var match = regex.Match(condition);
            if (!match.Success)
                throw new Exception("Data filter condition is not valid");

            var column = match.Groups["Column"].Value;
            var oper   = match.Groups["Operator"].Value;
            var value  = match.Groups["Value"].Value;

            var result = new DataFilter()
            {
                ColumnName = column,
                Value      = value
            };

            switch (oper)
            {
                case "=":
                    result.Condition = DataFilterCondition.Equal;
                    break;
                case ">=":
                    result.Condition = DataFilterCondition.GreaterThanOrEqual;
                    break;
                case ">":
                    result.Condition = DataFilterCondition.GreaterThan;
                    break;
                case "<=":
                    result.Condition = DataFilterCondition.LessThanOrEqual;
                    break;
                case "<":
                    result.Condition = DataFilterCondition.LessThan;
                    break;
                case "!=":
                case "<>":
                    result.Condition = DataFilterCondition.NotEqual;
                    break;
            }

            return result;
        }

        public static object ConvertListToDataTable(object dataSource, bool allowDataView)
        {
            if (dataSource == null)
                return null;

            if (dataSource is DataTable)
                return dataSource;

            if (allowDataView && dataSource is DataView)
                return dataSource;

            var result = new DataTable("Table");
            result.Load(new TypedListDataReader(dataSource));

            return result;
        }

        public static Type GetType(string typeName, bool ignoreCase)
        {
            var type = Type.GetType(typeName, false, ignoreCase);
            if (type != null)
                return type;

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName, false, ignoreCase);
                if (type != null)
                    return type;
            }

            return null;
        }

        public static Type GetUnderlyingType(Type sourceType)
        {
            if (sourceType == null)
                return null;

            var result = Nullable.GetUnderlyingType(sourceType) ?? sourceType;

            if (result.IsArray)
            {
                result = result.GetElementType();
                result = Nullable.GetUnderlyingType(result) ?? result;
            }

            return result;
        }

        public static string GetTypeName(Type sourceType)
        {
            if (sourceType == null)
                return null;

            bool isNullableType = false, isNullableElement = false, isArray;

            var nullableType = Nullable.GetUnderlyingType(sourceType);
            if (nullableType != null)
            {
                sourceType = nullableType;
                isNullableType = true;
            }

            string genericArguments = null;
            if (sourceType.IsGenericType)
            {
                var genArguments = new StringBuilder();
                var typeArguments = sourceType.GenericTypeArguments;
                foreach (var typeArgument in typeArguments)
                {
                    if (genArguments.Length > 0)
                        genArguments.Append(", ");
                    var genTypeName = GetTypeName(typeArgument);
                    genArguments.Append(genTypeName);
                }
                genericArguments = genArguments.ToString();
            }

            isArray = sourceType.IsArray;
            if (isArray)
            {
                sourceType = sourceType.GetElementType();
                nullableType = Nullable.GetUnderlyingType(sourceType);
                if (nullableType != null)
                {
                    sourceType = nullableType;
                    isNullableElement = true;
                }
            }

            var res = Type.GetTypeCode(sourceType) switch
            {
                TypeCode.Empty    => null,
                TypeCode.Object   => sourceType.Name,
                TypeCode.DBNull   => "DBNull",
                TypeCode.Boolean  => "bool",
                TypeCode.Char     => "char",
                TypeCode.SByte    => "sbyte",
                TypeCode.Byte     => "byte",
                TypeCode.Int16    => "short",
                TypeCode.UInt16   => "ushort",
                TypeCode.Int32    => "int",
                TypeCode.UInt32   => "uint",
                TypeCode.Int64    => "long",
                TypeCode.UInt64   => "ulong",
                TypeCode.Single   => "float",
                TypeCode.Double   => "double",
                TypeCode.Decimal  => "decimal",
                TypeCode.DateTime => "datetime",
                TypeCode.String   => "string",
                _                 => null
            };
            if (string.IsNullOrWhiteSpace(res))
                return null;

            if (sourceType.IsEnum)
                res = sourceType.Name;

            if (!string.IsNullOrWhiteSpace(genericArguments))
                res = Regex.Replace(res, "`\\d+$", "") + $"[{genericArguments}]";

            if (isNullableElement)
                res += "?";

            if (isArray)
                res += "[]";

            if (isNullableType)
                res += "?";

            return res;
        }

        public static object GetDataSourceFromPowerShellObject(PSObject dataSource)
        {
            if (dataSource?.BaseObject == null)
                throw new Exception("Please provide data source with vector items.");

            if (dataSource.BaseObject is DataTable dataTable)
                return dataTable;
            else if (dataSource.BaseObject is DataView dataView)
                return dataView;
            else
            {
                ITypedList list = null;
                if (dataSource.BaseObject is ITypedList typedList)
                    list = typedList;
                else if (dataSource.BaseObject is IListSource listSource)
                    list = listSource.GetList() as ITypedList;

                if (list != null)
                {
                    var ds = new TypedListDataReader(dataSource.BaseObject);
                    return ds;
                }
                else if (dataSource.BaseObject is IList objectList && IsPSList(objectList))
                {
                    var psList = new List<PSObject>();
                    foreach (var psObj in objectList)
                        psList.Add((PSObject)psObj);

                    var ds = new PSObjectDataReader(psList);
                    var dt = new DataTable("GeoItems");
                    dt.Load(ds);
                    return dt;
                }
                else
                    throw new Exception("Cannot use specified data source.");
            }


            static bool IsPSList(IList objects)
            {
                foreach (var obj in objects)
                    if (obj is not PSObject)
                        return false;

                return true;
            }
        }

        public static void OpenWithDefaultProgram(string path)
        {
            using var fileOpener = new Process();

            string winFollder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

            fileOpener.StartInfo.FileName  = Path.Combine(winFollder, "explorer");
            fileOpener.StartInfo.Arguments = Utils.QuoteString(path, "\"");
            fileOpener.Start();
        }

        public static bool IsSubClassOfGeneric(Type child, Type parent)
        {
            if (child == parent)
                return false;

            if (child.IsSubclassOf(parent))
                return true;

            var parameters = parent.GetGenericArguments();
            var isParameterLessGeneric = !(parameters != null && parameters.Length > 0 &&
                ((parameters[0].Attributes & TypeAttributes.BeforeFieldInit) == TypeAttributes.BeforeFieldInit));

            while (child != null && child != typeof(object))
            {
                var cur = GetFullTypeDefinition(child);
                if (parent == cur || (isParameterLessGeneric && cur.GetInterfaces().Select(i => GetFullTypeDefinition(i)).Contains(GetFullTypeDefinition(parent))))
                    return true;
                else if (!isParameterLessGeneric)
                    if (GetFullTypeDefinition(parent) == cur && !cur.IsInterface)
                    {
                        if (VerifyGenericArguments(GetFullTypeDefinition(parent), cur))
                            if (VerifyGenericArguments(parent, child))
                                return true;
                    }
                    else
                        foreach (var item in child.GetInterfaces().Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i)))
                            if (VerifyGenericArguments(parent, item))
                                return true;

                child = child.BaseType;
            }

            return false;
        }

        private static Type GetFullTypeDefinition(Type type)
        {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        private static bool VerifyGenericArguments(Type parent, Type child)
        {
            Type[] childArguments  = child.GetGenericArguments();
            Type[] parentArguments = parent.GetGenericArguments();
            if (childArguments.Length == parentArguments.Length)
                for (int i = 0; i < childArguments.Length; i++)
                    if (childArguments[i].Assembly != parentArguments[i].Assembly || childArguments[i].Name != parentArguments[i].Name || childArguments[i].Namespace != parentArguments[i].Namespace)
                        if (!childArguments[i].IsSubclassOf(parentArguments[i]))
                            return false;

            return true;
        }

        public static Encoding GetEncoding(string strEncoding)
        {
            Encoding encoding = Encoding.UTF8;

            switch (strEncoding?.ToLower())
            {
                case "unicode":
                    encoding = Encoding.Unicode;
                    break;
                case "utf8":
                case "utf-8":
                    encoding = Encoding.UTF8;
                    break;
                case "utf32":
                case "utf-32":
                    encoding = Encoding.UTF32;
                    break;
                case "ascii":
                    encoding = Encoding.ASCII;
                    break;
                default:
                    if (!string.IsNullOrWhiteSpace(strEncoding))
                    {
                        if (int.TryParse(strEncoding, out int codePage))
                            encoding = Encoding.GetEncoding(codePage);
                        else
                            encoding = Encoding.GetEncoding(strEncoding);
                    }
                    break;
            }

            return encoding;
        }

        public static byte[] GetBytes(object value, Encoding encoding)
        {
            byte[] data = null;
            if (value is byte[] v)
                data = v;
            else
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Object:
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Boolean:
                        data = BitConverter.GetBytes(Convert.ToBoolean(value));
                        break;
                    case TypeCode.Char:
                        data = BitConverter.GetBytes(Convert.ToChar(value));
                        break;
                    case TypeCode.SByte:
                        data = BitConverter.GetBytes(Convert.ToSByte(value));
                        break;
                    case TypeCode.Byte:
                        data = BitConverter.GetBytes(Convert.ToByte(value));
                        break;
                    case TypeCode.Int16:
                        data = BitConverter.GetBytes(Convert.ToInt16(value));
                        break;
                    case TypeCode.UInt16:
                        data = BitConverter.GetBytes(Convert.ToUInt16(value));
                        break;
                    case TypeCode.Int32:
                        data = BitConverter.GetBytes(Convert.ToInt32(value));
                        break;
                    case TypeCode.UInt32:
                        data = BitConverter.GetBytes(Convert.ToUInt32(value));
                        break;
                    case TypeCode.Int64:
                        data = BitConverter.GetBytes(Convert.ToInt64(value));
                        break;
                    case TypeCode.UInt64:
                        data = BitConverter.GetBytes(Convert.ToUInt64(value));
                        break;
                    case TypeCode.Single:
                        data = BitConverter.GetBytes(Convert.ToSingle(value));
                        break;
                    case TypeCode.Double:
                        data = BitConverter.GetBytes(Convert.ToDouble(value));
                        break;
                    case TypeCode.Decimal:
                        data = BitConverter.GetBytes(Convert.ToDouble(value));
                        break;
                    case TypeCode.DateTime:
                        data = BitConverter.GetBytes(Convert.ToDouble(value));
                        break;
                    case TypeCode.String:
                        encoding ??= Encoding.UTF8;
                        data       = encoding.GetBytes(Convert.ToString(value));
                        break;
                    default:
                        break;
                }
            }

            return data;
        }
    }
}