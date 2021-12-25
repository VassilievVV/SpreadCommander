using System;
using System.Drawing;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Threading;

namespace SpreadCommander.Common.PowerShell.Host
{
    /// <summary>
    /// This is a sample implementation of the PSHost abstract class for
    /// console applications. Not all members are implemented. Those that
    /// are not implemented throw a NotImplementedException exception or
    /// return nothing.
    /// </summary>
    internal class SpreadCommanderHost : PSHost
    {
        /// <summary>
        /// A reference to the PSHost implementation.
        /// </summary>
        private readonly ISpreadCommanderHostOwner program;

        /// <summary>
        /// The culture information of the thread that created
        /// this object.
        /// </summary>
        private readonly CultureInfo originalCultureInfo =
            Thread.CurrentThread.CurrentCulture;

        /// <summary>
        /// The UI culture information of the thread that created
        /// this object.
        /// </summary>
        private readonly CultureInfo originalUICultureInfo =
            Thread.CurrentThread.CurrentUICulture;

        /// <summary>
        /// The identifier of this PSHost implementation.
        /// </summary>
        private readonly Guid instanceId = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance of the MyHost class. Keep
        /// a reference to the host application object so that it
        /// can be informed of when to exit.
        /// </summary>
        /// <param name="program">
        /// A reference to the host application object.
        /// </param>
        public SpreadCommanderHost(ISpreadCommanderHostOwner program)
        {
            this.program            = program;
            this._HostUserInterface = new SpreadCommanderHostUserInterface(this);
            this._ExternalHost      = new ExternalHost(this);
        }

        public static Color ConvertConsoleColor(ConsoleColor color, bool isBackground)
        {
            if (color == SpreadCommanderRawUserInterface.DefaultBackgroundColor)
                return SystemColors.Window;
            if (color == SpreadCommanderRawUserInterface.DefaultForegroundColor)
                return SystemColors.WindowText;

            return color switch
            {
                ConsoleColor.Black       => Color.Black,
                ConsoleColor.DarkBlue    => Color.DarkBlue,
                ConsoleColor.DarkGreen   => Color.DarkGreen,
                ConsoleColor.DarkCyan    => Color.DarkCyan,
                ConsoleColor.DarkRed     => Color.DarkRed,
                ConsoleColor.DarkMagenta => Color.DarkMagenta,
                ConsoleColor.DarkYellow  => Color.Orange,
                ConsoleColor.Gray        => Color.Gray,
                ConsoleColor.DarkGray    => Color.DarkGray,
                ConsoleColor.Blue        => Color.Blue,
                ConsoleColor.Green       => Color.Green,
                ConsoleColor.Cyan        => Color.Cyan,
                ConsoleColor.Red         => Color.Red,
                ConsoleColor.Magenta     => Color.Magenta,
                ConsoleColor.Yellow      => Color.Yellow,
                ConsoleColor.White       => Color.White,
                _                        => isBackground ? SystemColors.Window : SystemColors.WindowText
            };
        }

        /// <summary>
        /// A reference to the implementation of the PSHostUserInterface
        /// class for this application.
        /// </summary>
        private readonly SpreadCommanderHostUserInterface _HostUserInterface;
        private readonly ExternalHost _ExternalHost;

        public ISpreadCommanderHostOwner HostOwner => program;
        public ExternalHost ExternalHost           => _ExternalHost;

        public override PSObject PrivateData       => new (_ExternalHost);

        /// <summary>
        /// Return the culture information to use. This implementation
        /// returns a snapshot of the culture information of the thread
        /// that created this object.
        /// </summary>
        public override System.Globalization.CultureInfo CurrentCulture
        {
            get { return this.originalCultureInfo; }
        }

        /// <summary>
        /// Return the UI culture information to use. This implementation
        /// returns a snapshot of the UI culture information of the thread
        /// that created this object.
        /// </summary>
        public override System.Globalization.CultureInfo CurrentUICulture
        {
            get { return this.originalUICultureInfo; }
        }

        /// <summary>
        /// This implementation always returns the GUID allocated at
        /// instantiation time.
        /// </summary>
        public override Guid InstanceId
        {
            get { return this.instanceId; }
        }

        /// <summary>
        /// Return a string that contains the name of the host implementation.
        /// Keep in mind that this string may be used by script writers to
        /// identify when your host is being used.
        /// </summary>
        public override string Name
        {
            get { return "SpreadCommander.PowerShell.Host"; }
        }

        /// <summary>
        /// Gets an instance of the implementation of the PSHostUserInterface
        /// class for this application. This instance is allocated once at startup time
        /// and returned every time thereafter.
        /// </summary>
        public override PSHostUserInterface UI
        {
            get { return _HostUserInterface; }
        }

        /// <summary>
        /// Return the version object for this application. Typically this
        /// should match the version resource in the application.
        /// </summary>
        public override Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        /// <summary>
        /// Not implemented by this example class. The call fails with
        /// a NotImplementedException exception.
        /// </summary>
        public override void EnterNestedPrompt()
        {
            throw new NotImplementedException(
                "SC: The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented by this example class. The call fails
        /// with a NotImplementedException exception.
        /// </summary>
        public override void ExitNestedPrompt()
        {
            throw new NotImplementedException("SC: The method or operation is not implemented.");
        }

        /// <summary>
        /// This API is called before an external application process is
        /// started. Typically it is used to save state so the parent can
        /// restore state that has been modified by a child process (after
        /// the child exits). In this example, this functionality is not
        /// needed so the method returns nothing.
        /// </summary>
        public override void NotifyBeginApplication()
        {
            return;
        }

        /// <summary>
        /// This API is called after an external application process finishes.
        /// Typically it is used to restore state that a child process may
        /// have altered. In this example, this functionality is not
        /// needed so the method returns nothing.
        /// </summary>
        public override void NotifyEndApplication()
        {
            return;
        }

        /// <summary>
        /// Indicate to the host application that exit has
        /// been requested. Pass the exit code that the host
        /// application should use when exiting the process.
        /// </summary>
        /// <param name="exitCode">The exit code to use.</param>
        public override void SetShouldExit(int exitCode)
        {
            this.program.ShouldExit = true;
            this.program.ExitCode   = exitCode;
        }

        public virtual void ExecuteMethodSync(Action function)
        {
            var sync = HostOwner?.SynchronizeInvoke;

            if (sync?.InvokeRequired ?? false)
                sync.Invoke(function, Array.Empty<object>());
            else
                function();
        }
    }
}