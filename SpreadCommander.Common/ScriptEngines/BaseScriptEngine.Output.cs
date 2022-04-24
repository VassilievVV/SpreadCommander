using DevExpress.Data.Filtering;
using DevExpress.Printing.ExportHelpers;
using DevExpress.Spreadsheet;
using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.Parsers.ConsoleScript;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegEx = System.Text.RegularExpressions;
using SpreadCommander.Common.Extensions;
using System.Globalization;
using SpreadCommander.Common.Code;
using SpreadCommander.Common.Spreadsheet;
using SpreadCommander.Common.PowerShell.CmdLets;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class BaseScriptEngine
    {
        protected virtual void ExecuteMethodSync(ISynchronizeInvoke sync, Action function)
        {
            if (sync?.InvokeRequired ?? false)
                sync.Invoke(function, Array.Empty<object>());
            else
                function();
        }

        protected virtual void ClearLastParagraph(Document doc)
        {
            using (new UsingProcessor(() => doc.BeginUpdate(), () => doc.EndUpdate()))
            {
                if (doc.Paragraphs.Count > 0)
                {
                    var para = doc.Paragraphs[doc.Paragraphs.Count - 1];
                    doc.Delete(para.Range);
                    doc.Paragraphs.Append();
                }

                if (ApplicationType == ScriptApplicationType.Console)
                {
                    System.Console.CursorLeft = 0;
                    if (System.Console.CursorTop > 0)
                        System.Console.CursorTop--;
                }
            }
        }

        protected virtual void ClearDocument(Document doc)
        {
            using (new UsingProcessor(() => doc.BeginUpdate(), () => doc.EndUpdate()))
            {
                doc.Text = string.Empty;
            }
        }

        protected internal void ReportErrorSynchronized(Document doc, ISynchronizeInvoke sync, string errorMessage) =>
            ExecuteMethodSync(sync, () => DoReportError(doc, errorMessage));

        //Has to be called from synchronized code
        protected virtual void ReportError(Document doc, Exception ex) =>
            DoReportError(doc, ex.Message);

        protected internal virtual void DoReportError(Document doc, string errorMessage)
        {
            var range = doc.AppendText($"ERROR: {errorMessage}{Environment.NewLine}");
            var cp    = doc.BeginUpdateCharacters(range);
            try
            {
                cp.Reset();
                cp.ForeColor = Color.Red;
            }
            finally
            {
                doc.EndUpdateCharacters(cp);
            }

            SCCmdlet.WriteErrorToConsole(errorMessage);
        }

        protected internal void FlushTextBufferSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, StringBuilder buffer)
        {
            if (buffer.Length <= 0)
                return;

            ExecuteMethodSync(sync, () => FlushTextBuffer(doc, output, buffer));
        }

        protected internal void FlushTextBufferSynchronized(Document doc, ISynchronizeInvoke sync, Color foregroundColor, Color backgroundColor, StringBuilder buffer)
        {
            if (buffer.Length <= 0)
                return;

            ExecuteMethodSync(sync, () => FlushTextBuffer(doc, foregroundColor, backgroundColor, buffer));
        }

        protected internal virtual void FlushTextBuffer(Document doc, ScriptOutputMessage output, StringBuilder buffer)
        {
            FlushTextBuffer(doc, output.ForegroundColor, output.BackgroundColor, buffer);
        }

        protected internal virtual void FlushTextBuffer(Document doc, Color foregroundColor, Color backgroundColor, StringBuilder buffer)
        {
            if (buffer.Length <= 0)
                return;

            doc.BeginUpdate();
            try
            {
                var text = buffer.ToString();

                /*
                var range = doc.AppendText(text);
                if (foregroundColor != SystemColors.WindowText || backgroundColor != SystemColors.Window)
                {
                    var cp = doc.BeginUpdateCharacters(range);
                    try
                    {
                        if (foregroundColor != SystemColors.WindowText)
                            cp.ForeColor = foregroundColor;
                        if (backgroundColor != SystemColors.Window)
                            cp.BackColor = backgroundColor;
                    }
                    finally
                    {
                        doc.EndUpdateCharacters(cp);
                    }
                }

                if (range != null)
                {
                    doc.CaretPosition = range.End;
                    ScrollToCaret();
                }*/

                using (var reader = new StringReader(text))
                {
                    ReadOutputBuffer(doc, reader, ScriptOutputType.Text, backgroundColor, foregroundColor);
                }

                buffer.Clear();

                SCCmdlet.WriteTextToConsole(text);
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected void AppendDocumentContentSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            ExecuteMethodSync(sync, () => AppendDocumentContent(doc, output, fileName));
        }

        protected virtual void AppendDocumentContent(Document doc, ScriptOutputMessage output, string fileName)
        {
            doc.BeginUpdate();
            try
            {
                fileName = Project.Current.MapPath(fileName);

                doc.AppendDocumentContent(fileName);
                var paragraph = doc.Paragraphs.Append();

                doc.CaretPosition = paragraph.Range.End;
                ScrollToCaret();
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected void AppendImageSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            ExecuteMethodSync(sync, () => AppendImage(doc, output, fileName));
        }

        protected virtual void AppendImage(Document doc, ScriptOutputMessage output, string fileName)
        {
            doc.BeginUpdate();
            try
            {
                fileName = Project.Current.MapPath(fileName);
                doc.Images.Append(DocumentImageSource.FromFile(fileName));
                var paragraph = doc.Paragraphs.Append();

                doc.CaretPosition = paragraph.Range.End;
                ScrollToCaret();
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected void AppendHtmlSynchronized(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return;

            ExecuteMethodSync(sync, () => AppendHtml(doc, output, html));
        }

        protected virtual void AppendHtml(Document doc, ScriptOutputMessage output, string html)
        {
            doc.BeginUpdate();
            try
            {
                var range = doc.AppendHtmlText(html, DevExpress.XtraRichEdit.API.Native.InsertOptions.KeepSourceFormatting);

                doc.CaretPosition = range.End;
                ScrollToCaret();
            }
            catch (Exception ex)
            {
                ReportError(doc, ex);
            }
            finally
            {
                doc.EndUpdate();
            }
        }

        protected bool _ProcessingOutputQueue;
        protected virtual void StartProcessOutputQueue()
        {
            _ProcessingOutputQueue = true;
        }

        protected virtual void EndProcessOutputQueue()
        {
            _ProcessingOutputQueue = false;
        }

        public virtual void ProcessOutputQueue()
        {
            if (_ProcessingOutputQueue)
                return;

            using (new UsingProcessor(() => StartProcessOutputQueue(), () => EndProcessOutputQueue()))
            {
                var doc = Document;
                if (doc == null)
                    return; //Process queue in next call to ProcessOutputQueue(), when document will be available.

                if (OutputQueue.Count <= 0)
                    return;

                var sync = SynchronizeInvoke;
                DoProcessQueue();


                void DoProcessQueue()
                {
                    var bufferOutput = new StringBuilder(DefaultBufferCapacity);

                    while (OutputQueue.Count > 0)
                    {
                        var output = OutputQueue.Dequeue();
                        if (output == null)
                            continue;

                        //Merge output parts with same formatting
                        if (OutputQueue.Count > 0)
                        {
                            var nextOutput = OutputQueue.Peek();
                            if (nextOutput?.IsFormattingEqual(output) ?? false)
                            {
                                bufferOutput.Append(output.Text);
                                continue;
                            }
                        }

                        bufferOutput.Append(output.Text);
                        if (bufferOutput.Length > 0)
                        {
                            try
                            {
                                ProcessBuffer(doc, sync, output, bufferOutput);
                            }
                            catch (Exception ex)
                            {
                                ReportErrorSynchronized(doc, sync, ex.Message);
                            }
                        }
                    }
                }
            }
        }
        protected virtual void ProcessBuffer(Document doc, ISynchronizeInvoke sync, 
            ScriptOutputMessage output, StringBuilder buffer)
        {
            FlushTextBufferSynchronized(doc, sync, output, buffer);
        }

        protected virtual void FlushOutputBuffer(Document doc, ScriptOutputType outputType, string text, 
            Color backgroundColor, Color foregroundColor)
        {
            var range = doc.AppendText(text);
            if (foregroundColor != SystemColors.WindowText || backgroundColor != SystemColors.Window)
            {
                var cp = doc.BeginUpdateCharacters(range);
                try
                {
                    if (foregroundColor != SystemColors.WindowText)
                        cp.ForeColor = foregroundColor;
                    if (backgroundColor != SystemColors.Window)
                        cp.BackColor = backgroundColor;
                }
                finally
                {
                    doc.EndUpdateCharacters(cp);
                }
            }

            if (range != null)
            {
                doc.CaretPosition = range.End;
                ScrollToCaret();
            }
        }

#pragma warning disable CRRSP05 // A misspelled word has been found
        protected virtual void ReadOutputBuffer(Document doc, TextReader reader, ScriptOutputType outputType,
            Color backgroundColor, Color foregroundColor)
        {
            var defaultForegroundColor = outputType == ScriptOutputType.Error ? DefaultForegroundErrorColor : DefaultForegroundColor;
            var defaultBackgroungColor = DefaultBackgroundColor;

            var buffer = new StringBuilder(1024);

            int iChar = reader.Read();
            if (iChar < 0)
                return;

            var c = (char)iChar;

            while (true)
            {
                if (c == '\u001B')   //ESC
                {
                    //https://en.wikipedia.org/wiki/ANSI_escape_code
                    //http://invisible-island.net/xterm/ctlseqs/ctlseqs.html
                    var c2 = (char)reader.Read();
                    switch (c2)
                    {
                        case '[':
                            /*
                            The ESC[ is followed by any number(including none) of "parameter bytes" in the range 0x30–0x3F(ASCII 0–9:;<=>?), 
                            then by any number of "intermediate bytes" in the range 0x20–0x2F(ASCII space and !"#$%&'()*+,-./), 
                            then finally by a single "final byte" in the range 0x40–0x7E (ASCII @A–Z[\]^_`a–z{|}~).[14]:5.4
                            */
                            var buffer2 = new StringBuilder(128);
                            char closingChar = '\0';

                            while (true)
                            {
                                if (reader.Peek() <= 0)
                                    break;
                                var cc3 = reader.Read();
                                if (cc3 >= 0x40 && cc3 <= 0x7E)
                                {
                                    closingChar = (char)cc3;
                                    break;
                                }

                                buffer2.Append((char)cc3);
                            }

                            switch (closingChar)
                            {

                                //A-D Moves the cursor n {\displaystyle n} n (default 1) cells in the given direction. 
                                //If the cursor is already at the edge of the screen, this has no effect. 
                                case 'A':
                                    //CUU – Cursor Up
                                    break;
                                case 'B':
                                    //CUD – Cursor Down 
                                    break;
                                case 'C':
                                    //CUF – Cursor Forward 
                                    break;
                                case 'D':
                                    //CUB – Cursor Back
                                    break;
                                case 'E':
                                    //CNL – Cursor Next Line
                                    //Moves cursor to beginning of the line n {\displaystyle n} n (default 1) lines down. (not ANSI.SYS) 
                                    break;
                                case 'F':
                                    //CPL – Cursor Previous Line
                                    //Moves cursor to beginning of the line n {\displaystyle n} n (default 1) lines up. (not ANSI.SYS) 
                                    break;
                                case 'G':
                                    //CHA – Cursor Horizontal Absolute
                                    //Moves the cursor to column n (default 1). (not ANSI.SYS)
                                    break;
                                case 'H':
                                    //CUP – Cursor Position
                                    //Moves the cursor to row n, column m. 
                                    //The values are 1-based, and default to 1 (top left corner) if omitted. 
                                    //A sequence such as CSI ;5H is a synonym for CSI 1;5H as well as CSI 17;H is the same as CSI 17H and CSI 17;1H
                                    break;
                                case 'J':
                                    //ED – Erase in Display
                                    //Clears part of the screen.If n 
                                    //n is 0(or missing), clear from cursor to end of screen.If n 
                                    //n is 1, clear from cursor to beginning of the screen. If n 
                                    //n is 2, clear entire screen(and moves cursor to upper left on DOS ANSI.SYS).If n
                                    //n is 3, clear entire screen and delete all lines saved in the scrollback buffer(this feature was added for xterm and is supported by other terminal applications).
                                    break;
                                case 'K':
                                    //EL – Erase in Line
                                    // 	Erases part of the line. 
                                    //If n is 0 (or missing), clear from cursor to the end of the line.
                                    //If n is 1, clear from cursor to beginning of the line. 
                                    //If n is 2, clear entire line. Cursor position does not change. 
                                    break;
                                case 'S':
                                    //SU – Scroll Up
                                    //Scroll whole page up by n 
                                    //n(default 1) lines.New lines are added at the bottom. (not ANSI.SYS)
                                    break;
                                case 'T':
                                    //SD – Scroll Down
                                    //Scroll whole page down by n (default 1) lines.New lines are added at the top. (not ANSI.SYS)
                                    break;
                                case 'f':
                                    //HVP – Horizontal Vertical Position
                                    //Same as CUP 
                                    break;
                                case 'm':
                                    //SGR – Select Graphic Rendition
                                    //Sets the appearance of the following characters, see SGR parameters below. 

                                    //Send current output to console
                                    FlushBuffer();

                                    var strBuffer2 = buffer2.ToString()?.Trim() ?? string.Empty;
                                    if (strBuffer2 == string.Empty)
                                        strBuffer2 = "0";

                                    int sgrCode;
                                    string sgrValue = null;

                                    var sgrP = strBuffer2.IndexOfAny(new char[] { ';', ':' });
                                    if (sgrP >= 0)
                                    {
                                        if (!int.TryParse(strBuffer2.AsSpan(0, sgrP), out sgrCode))
                                            sgrCode = 0;
                                        sgrValue = strBuffer2[(sgrP + 1)..].Trim();
                                    }
                                    else
                                    {
                                        if (!int.TryParse(strBuffer2, out sgrCode))
                                            sgrCode = 0;
                                    }

                                    /*
                                     Code 	Effect 	Note
                                    0 	Reset / Normal 	all attributes off
                                    1 	Bold or increased intensity 	
                                    2 	Faint (decreased intensity) 	
                                    3 	Italic 	Not widely supported. Sometimes treated as inverse.
                                    4 	Underline 	
                                    5 	Slow Blink 	less than 150 per minute
                                    6 	Rapid Blink 	MS-DOS ANSI.SYS; 150+ per minute; not widely supported
                                    7 	reverse video 	swap foreground and background colors
                                    8 	Conceal 	Not widely supported.
                                    9 	Crossed-out 	Characters legible, but marked for deletion.
                                    10 	Primary(default) font 	
                                    11–19 	Alternative font 	Select alternative font n − 10 {\displaystyle n-10} {\displaystyle n-10}
                                    20 	Fraktur 	Rarely supported
                                    21 	Bold off 	all popular terminals but xterm and PuTTY (which do underline)
                                    22 	Normal color or intensity 	Neither bold nor faint
                                    23 	Not italic, not Fraktur 	
                                    24 	Underline off 	Not singly or doubly underlined
                                    25 	Blink off 	
                                    27 	Inverse off 	
                                    28 	Reveal 	conceal off
                                    29 	Not crossed out 	
                                    30–37 	Set foreground color 	See color table below
                                    38 	Set foreground color 	Next arguments are 5;n or 2;r;g;b, see below
                                    39 	Default foreground color 	implementation defined (according to standard)
                                    40–47 	Set background color 	See color table below
                                    48 	Set background color 	Next arguments are 5;n or 2;r;g;b, see below
                                    49 	Default background color 	implementation defined (according to standard)
                                    51 	Framed 	
                                    52 	Encircled 	
                                    53 	Overlined 	
                                    54 	Not framed or encircled 	
                                    55 	Not overlined 	
                                    60 	ideogram underline or right side line 	Rarely supported
                                    61 	ideogram double underline or
                                    double line on the right side
                                    62 	ideogram overline or left side line
                                    63 	ideogram double overline or
                                    double line on the left side
                                    64 	ideogram stress marking
                                    65 	ideogram attributes off 	reset the effects of all of 60–64
                                    90–97 	Set bright foreground color 	aixterm (not in standard)
                                    100–107 	Set bright background color 	aixterm (not in standard) 
                                    */
                                    switch (sgrCode)
                                    {
                                        case 0:
                                            //Reset / Normal  all attributes off
                                            foregroundColor = defaultForegroundColor;
                                            backgroundColor = defaultBackgroungColor;
                                            break;
                                        //30–37   Set foreground color
                                        case 30:
                                            foregroundColor = Color.Black;
                                            break;
                                        case 31:
                                            foregroundColor = Color.Red;
                                            break;
                                        case 32:
                                            foregroundColor = Color.Green;
                                            break;
                                        case 33:
                                            foregroundColor = Color.Yellow;
                                            break;
                                        case 34:
                                            foregroundColor = Color.Blue;
                                            break;
                                        case 35:
                                            foregroundColor = Color.Magenta;
                                            break;
                                        case 36:
                                            foregroundColor = Color.Cyan;
                                            break;
                                        case 37:
                                            foregroundColor = Color.White;
                                            break;
                                        case 38:
                                            //Set foreground color. Next arguments are 5;n or 2;r;g;b
                                            //Not implemented, reset to default
                                            foregroundColor = defaultForegroundColor;
                                            break;
                                        case 39:
                                            //Default foreground color
                                            foregroundColor = defaultForegroundColor;
                                            break;
                                        //40-47	Set background color
                                        case 40:
                                            backgroundColor = Color.Black;
                                            break;
                                        case 41:
                                            backgroundColor = Color.Red;
                                            break;
                                        case 42:
                                            backgroundColor = Color.Green;
                                            break;
                                        case 43:
                                            backgroundColor = Color.Yellow;
                                            break;
                                        case 44:
                                            backgroundColor = Color.Blue;
                                            break;
                                        case 45:
                                            backgroundColor = Color.Magenta;
                                            break;
                                        case 46:
                                            backgroundColor = Color.Cyan;
                                            break;
                                        case 47:
                                            backgroundColor = Color.White;
                                            break;
                                        case 48:
                                            //Set background color. Next arguments are 5;n or 2;r;g;b
                                            //Not implemented, reset to default
                                            backgroundColor = defaultForegroundColor;
                                            break;
                                        case 49:
                                            //Default foreground color
                                            backgroundColor = defaultBackgroungColor;
                                            break;
                                        //Set bright foreground color 	aixterm (not in standard) 
                                        case 90:
                                            foregroundColor = Color.DarkGray;
                                            break;
                                        case 91:
                                            foregroundColor = Color.Pink;
                                            break;
                                        case 92:
                                            foregroundColor = Color.LightGreen;
                                            break;
                                        case 93:
                                            foregroundColor = Color.LightYellow;
                                            break;
                                        case 94:
                                            foregroundColor = Color.LightBlue;
                                            break;
                                        case 95:
                                            foregroundColor = Color.LightSteelBlue;
                                            break;
                                        case 96:
                                            foregroundColor = Color.LightCyan;
                                            break;
                                        case 97:
                                            foregroundColor = Color.LightGray;
                                            break;
                                        //Set bright background color 	aixterm (not in standard) 
                                        case 100:
                                            backgroundColor = Color.DarkGray;
                                            break;
                                        case 109:
                                            backgroundColor = Color.Pink;
                                            break;
                                        case 102:
                                            backgroundColor = Color.LightGreen;
                                            break;
                                        case 103:
                                            backgroundColor = Color.LightYellow;
                                            break;
                                        case 104:
                                            backgroundColor = Color.LightBlue;
                                            break;
                                        case 105:
                                            backgroundColor = Color.LightSteelBlue;
                                            break;
                                        case 106:
                                            backgroundColor = Color.LightCyan;
                                            break;
                                        case 107:
                                            backgroundColor = Color.LightGray;
                                            break;
                                    }
                                    break;
                                case 'i':
                                    //5i - AUX Port On
                                    //Enable aux serial port usually for local serial printer
                                    //4i - AUX Port Off
                                    //Disable aux serial port usually for local serial printer 
                                    break;
                                case 'n':
                                    //6n - DSR – Device Status Report
                                    //Reports the cursor position (CPR) to the application as (as though typed at the keyboard) 
                                    //ESC[n;mR, where n is the row and m is the column.) 
                                    break;
                                case 's':
                                    //SCP – Save Cursor Position
                                    //Saves the cursor position/state. 
                                    break;
                                case 'u':
                                    //RCP – Restore Cursor Position
                                    //Restores the cursor position/state. 
                                    break;
                                //private sequences
                                case 'h':
                                    //CSI ?25h 		DECTCEM Shows the cursor, from the VT320. 
                                    //CSI ?1049h 	Enable alternative screen buffer 
                                    //CSI ?2004h 	Turn on bracketed paste mode. Text pasted into the terminal will be surrounded by 
                                    //				ESC [200~ and ESC [201~, and characters in it should not be treated as commands 
                                    //				(for example in Vim).[16] From Unix terminal emulators. 
                                    break;
                                case 'l':
                                    //CSI ?25l 		DECTCEM Hides the cursor. 
                                    //CSI ?1049l 	Disable alternative screen buffer 
                                    //CSI ?2004l 	Turn off bracketed paste mode. 
                                    break;
                            }
                            break;
                        case ']':
                            /*
                            Starts a control string for the operating system to use, terminated by ST.[14]:8.3.89 
                            In xterm, they may also be terminated by BEL.[15] 
                            In xterm, the window title can be set by OSC 0; this is the window title BEL. 
                            */
                            var c3 = (char)reader.Read();
                            var c4 = (char)reader.Read();

                            if (c3 == '0' && c4 == ';')
                            {
                                var title = new StringBuilder(128);
                                while (true)
                                {
                                    if (reader.Peek() <= 0)
                                        break;
                                    var c5 = (char)reader.Read();
                                    if (c5 == '\u0007') //BEL
                                        break;
                                    title.Append(c5);
                                };
                                //Set caption
                                Title = title.ToString();
                            }
                            break;
                        case 'N':
                            /*
                            SS2 – Single Shift Two 	Select a single character from one of the alternative character sets. 
                            In xterm, SS2 selects the G2 character set, and SS3 selects the G3 character set.[15]
                            */
                            break;
                        case 'O':
                            /*
                            SS3 – Single Shift Three  	Select a single character from one of the alternative character sets. 
                            In xterm, SS2 selects the G2 character set, and SS3 selects the G3 character set.[15]
                            */
                            break;
                        case 'P':
                            /*
                            DCS – Device Control String 	Terminated by ST. 
                            Xterm's uses of this sequence include defining User-Defined Keys, 
                            and requesting or setting Termcap/Term-info data.[15]  
                            */
                            break;
                        case '\\':
                            /* 	
                            ST – String Terminator 	Terminates strings in other controls.[14]:8.3.143
                            */
                            break;
                        case 'X':
                            /*
                            SOS – Start of String 
                            Takes an argument of a string of text, terminated by ST.
                            The uses for these string control sequences are defined by the application[14]:8.3.2, 8.3.128 or 
                            privacy discipline.[14]:8.3.94 These functions are not implemented and the arguments are ignored by xterm.[15]
                            */
                            break;
                        case '^':
                            /*
                            PM – Privacy Message 
                            Takes an argument of a string of text, terminated by ST.
                            The uses for these string control sequences are defined by the application[14]:8.3.2, 8.3.128 or 
                            privacy discipline.[14]:8.3.94 These functions are not implemented and the arguments are ignored by xterm.[15]
                            */
                            break;
                        case '_':
                            /*
                            APC – Application Program Command
                            Takes an argument of a string of text, terminated by ST.
                            The uses for these string control sequences are defined by the application[14]:8.3.2, 8.3.128 or 
                            privacy discipline.[14]:8.3.94 These functions are not implemented and the arguments are ignored by xterm.[15]
                            */
                            break;
                        case 'c':
                            /*
                            RIS – Reset to Initial State 	
                            Resets the device to its original state. 
                            This may include (if applicable): reset graphic rendition, 
                            clear tabulation stops, reset to default font, and more. 
                            */
                            break;
                    }
                }
                else
                    buffer.Append(c);

                if (reader.Peek() < 0)
                    FlushBuffer();

                iChar = reader.Read();
                if (iChar < 0)
                    break;
                c = (char)iChar;
            }

            void FlushBuffer()
            {
                if (buffer.Length > 0)
                    FlushOutputBuffer(doc, outputType, buffer.ToString(), backgroundColor, foregroundColor);

                buffer.Clear();
            }
        }
#pragma warning restore CRRSP05 // A misspelled word has been found

        protected virtual string LoadCsvAsHtml(BaseCommand command, string fileName, List<FormatCondition> formatConditions)
        {
            using var workbook = SpreadsheetUtils.CreateWorkbook();
            var ext = Path.GetExtension(fileName)?.ToLower();

            workbook.LoadDocument(fileName, ext == ".txt" ? DocumentFormat.Text : DocumentFormat.Csv);
            workbook.History.IsEnabled = false;

            var sheet = workbook.Sheets[0] as Worksheet;
            var range = sheet.GetDataRange();

            var table = sheet.Tables.Add(range, "Table", true);

            var style = command.GetProperty<string>("style");
            if (!string.IsNullOrWhiteSpace(style))
            {
                var tableStyle = style;
                if (!tableStyle.StartsWith("TableStyle"))
                    tableStyle = $"TableStyle{style}";
                if (Enum.TryParse(tableStyle, out BuiltInTableStyleId tableStyleID))
                    table.Style = workbook.TableStyles[tableStyleID];
                else if (Enum.TryParse(style, out BuiltInStyleId styleID))
                    range.Style = workbook.Styles[styleID];
            }

            var gridData = new GridData();
            gridData.ApplyGridFormatConditions(formatConditions);

            SpreadsheetUtils.ApplyGridFormatting(table, gridData);

            var options = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
            {
                SheetIndex = workbook.Sheets.IndexOf(sheet),
                Range      = range.GetReferenceA1(),
                Encoding   = Encoding.Unicode
            };

            using var stream = new MemoryStream();
            workbook.ExportToHtml(stream, options);
            stream.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(stream, Encoding.UTF8);
            var result = reader.ReadToEnd();
            return result;
        }
    }
}
