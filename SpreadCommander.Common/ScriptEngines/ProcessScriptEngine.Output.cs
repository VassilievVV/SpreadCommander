using DevExpress.XtraRichEdit.API.Native;
using SpreadCommander.Common.ScriptEngines.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadCommander.Common.Extensions;
using System.Drawing;
using SpreadCommander.Common.Parsers.ConsoleScript;
using System.IO;
using SpreadCommander.Common.Code;
using System.Diagnostics;

namespace SpreadCommander.Common.ScriptEngines
{
    public partial class ProcessScriptEngine
    {
        protected override void StartProcessOutputQueue()
        {
            base.StartProcessOutputQueue();
            ProgressType = ProgressKind.Undetermined;
        }

        protected override void EndProcessOutputQueue()
        {
            base.EndProcessOutputQueue();
            ProgressType = ProgressKind.None;
        }

        public override void ProcessOutputQueue()
        {
            if (_ProcessingOutputQueue)
                return;

            lock (_SyncObject)
            {
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
                                    if (!output.Text.StartsWith("\r"))
                                        bufferOutput.Append('\r');
                                    bufferOutput.Append(output.Text);
                                    
                                    continue;
                                }
                            }

                            //Should every output start with CR, i.e. clear last line in output?
                            //Test sample in R:
                            /*
                                SEQ  <- seq(1,100)
                                pb   <- txtProgressBar(1, 100, style = 3)
                                TIME <- Sys.time()
                                for (i in SEQ) {
                                  Sys.sleep(0.1)
                                  setTxtProgressBar(pb, i)
                                }
                            */
                            if (!output.Text.StartsWith("\r"))
                                bufferOutput.Append('\r');
                            bufferOutput.Append(output.Text);

                            var text = bufferOutput.ToString();
                            bufferOutput.Clear();

                            //\r without following \n clears current line (paragraph)
                            var parts = Regex.Split(text, "\r(?!\n)");

                            var bufferParts = new StringBuilder(1024);
                            for (int i = 0; i < parts.Length; i++)
                            {
                                var part = parts[i];

                                if (i < parts.Length - 1)
                                {
                                    //Remove last line
                                    var nindex = part.LastIndexOf('\n');
                                    if (nindex >= 0)
                                        part = part[..(nindex + 1)];
                                    else
                                        part = string.Empty;
                                }

                                bufferParts.Append(part);
                            }

                            if (parts.Length > 1)
                            {
                                ExecuteMethodSync(sync, () => ClearLastParagraph(doc));
                            }
                            
                            if (bufferParts.Length > 0)
                            {
                                try
                                {
                                    ProcessBuffer(doc, sync, output, bufferParts);
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
        }

        protected override void ProcessBuffer(Document doc, ISynchronizeInvoke sync, ScriptOutputMessage output, StringBuilder buffer)
        {
            FlushTextBufferSynchronized(doc, sync, output, buffer);
        }

        protected override void FlushTextBuffer(Document doc, ScriptOutputMessage output, StringBuilder buffer)
        {
            base.FlushTextBuffer(doc, output, buffer);
        }
    }
}
