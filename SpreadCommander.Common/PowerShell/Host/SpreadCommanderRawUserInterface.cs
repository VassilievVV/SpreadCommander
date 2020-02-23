using System;
using System.Management.Automation.Host;

namespace SpreadCommander.Common.PowerShell.Host
{
	/// <summary>
	/// A sample implementation of the PSHostRawUserInterface for console
	/// applications. Members of this class that easily map to the .NET
	/// console class are implemented. More complex methods are not
	/// implemented and throw a NotImplementedException exception.
	/// </summary>
	internal class SpreadCommanderRawUserInterface : PSHostRawUserInterface
	{
		public const int WindowWidth  = 80;	//Use limited value, not 1M characters as in ProcessScriptEngine, PowerShell fills output with spaces producing very large strings
		public const int WindowHeight = 32;

		public const ConsoleColor DefaultBackgroundColor = ConsoleColor.White;
		public const ConsoleColor DefaultForegroundColor = ConsoleColor.Black;

		private readonly SpreadCommanderHostUserInterface _HostUI;

		public SpreadCommanderRawUserInterface(SpreadCommanderHostUserInterface hostUI)
		{
			_HostUI = hostUI;

			BackgroundColor       = DefaultBackgroundColor;
			ForegroundColor       = DefaultForegroundColor;
			BufferSize            = new Size(WindowWidth, WindowHeight);
			CursorSize            = 1;
			MaxPhysicalWindowSize = new Size(WindowWidth, WindowHeight);
			MaxWindowSize         = new Size(WindowWidth, WindowHeight);
		}

		/// <summary>
		/// Gets or sets the background color of the displayed text.
		/// This maps to the corresponding Console.Background property.
		/// </summary>
		public override ConsoleColor BackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets the size of the host buffer. In this example the
		/// buffer size is adapted from the Console buffer size members.
		/// </summary>
		public override Size BufferSize { get; set; }

		/// <summary>
		/// Gets or sets the cursor position. In this example this
		/// functionality is not needed so the property throws a
		/// NotImplementException exception.
		/// </summary>
		public override Coordinates CursorPosition
		{
			get
			{
				throw new NotImplementedException(
					 "The method or operation is not implemented.");
			}
			set
			{
				if (value.X == 0 && value.Y == 0)
				{ }	//Do nothing, allow to clear screen in later call to SetBufferContents
				else
					throw new NotImplementedException(
						"The method or operation is not implemented.");
			}
		}

		/// <summary>
		/// Gets or sets the size of the displayed cursor. In this example
		/// the cursor size is taken directly from the Console.CursorSize
		/// property.
		/// </summary>
		public override int CursorSize { get; set; }

		/// <summary>
		/// Gets or sets the foreground color of the displayed text.
		/// This maps to the corresponding Console.ForegroundColor property.
		/// </summary>
		public override ConsoleColor ForegroundColor { get; set; }

		/// <summary>
		/// Gets a value indicating whether the user has pressed a key. This maps
		/// to the corresponding Console.KeyAvailable property.
		/// </summary>
		public override bool KeyAvailable
		{
			//TODO:
			get { return false; }
		}

		/// <summary>
		/// Gets the dimensions of the largest window that could be
		/// rendered in the current display, if the buffer was at the least
		/// that large. This example uses the Console.LargestWindowWidth and
		/// Console.LargestWindowHeight properties to determine the returned
		/// value of this property.
		/// </summary>
		public override Size MaxPhysicalWindowSize { get; }

		/// <summary>
		/// Gets the dimensions of the largest window size that can be
		/// displayed. This example uses the Console.LargestWindowWidth and
		/// console.LargestWindowHeight properties to determine the returned
		/// value of this property.
		/// </summary>
		public override Size MaxWindowSize { get; }

		/// <summary>
		/// Gets or sets the position of the displayed window. This example
		/// uses the Console window position APIs to determine the returned
		/// value of this property.
		/// </summary>
		public override Coordinates WindowPosition
		{
			get { return new Coordinates(0, 0); }
			set { throw new NotImplementedException("Cannot move console"); }
		}

		/// <summary>
		/// Gets or sets the size of the displayed window. This example
		/// uses the corresponding Console window size APIs to determine the
		/// returned value of this property.
		/// </summary>
		public override Size WindowSize
		{
			get { return new Size(1024, 100); }
			set { throw new NotImplementedException("Cannot change console size"); }
		}

		/// <summary>
		/// Gets or sets the title of the displayed window. The example
		/// maps the Console.Title property to the value of this property.
		/// </summary>
		public override string WindowTitle
		{
			get { return _HostUI.Host.HostOwner.Title; }
			set { _HostUI.Host.HostOwner.Title = value; }
		}

		/// <summary>
		/// This API resets the input buffer. In this example this
		/// functionality is not needed so the method returns nothing.
		/// </summary>
		public override void FlushInputBuffer()
		{
		}

		/// <summary>
		/// This API returns a rectangular region of the screen buffer. In
		/// this example this functionality is not needed so the method throws
		/// a NotImplementException exception.
		/// </summary>
		/// <param name="rectangle">Defines the size of the rectangle.</param>
		/// <returns>Throws a NotImplementedException exception.</returns>
		public override BufferCell[,] GetBufferContents(Rectangle rectangle)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		/// <summary>
		/// This API reads a pressed, released, or pressed and released keystroke
		/// from the keyboard device, blocking processing until a keystroke is
		/// typed that matches the specified keystroke options. In this example
		/// this functionality is not needed so the method throws a
		/// NotImplementException exception.
		/// </summary>
		/// <param name="options">Options, such as IncludeKeyDown,  used when
		/// reading the keyboard.</param>
		/// <returns>Throws a NotImplementedException exception.</returns>
		public override KeyInfo ReadKey(ReadKeyOptions options)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		/// <summary>
		/// This API crops a region of the screen buffer. In this example
		/// this functionality is not needed so the method throws a
		/// NotImplementException exception.
		/// </summary>
		/// <param name="source">The region of the screen to be scrolled.</param>
		/// <param name="destination">The region of the screen to receive the
		/// source region contents.</param>
		/// <param name="clip">The region of the screen to include in the operation.</param>
		/// <param name="fill">The character and attributes to be used to fill all cell.</param>
		public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		/// <summary>
		/// This method copies an array of buffer cells into the screen buffer
		/// at a specified location. In this example this functionality is
		/// not needed so the method throws a NotImplementedException exception.
		/// </summary>
		/// <param name="origin">The parameter is not used.</param>
		/// <param name="contents">The parameter is not used.</param>
		public override void SetBufferContents(Coordinates origin,
			BufferCell[,] contents)
		{
			throw new NotImplementedException("The method or operation is not implemented.");
		}

		/// <summary>
		/// This method copies a given character, foreground color, and background
		/// color to a region of the screen buffer. In this example this
		/// functionality is not needed so the method throws a
		/// NotImplementException exception./// </summary>
		/// <param name="rectangle">Defines the area to be filled. </param>
		/// <param name="fill">Defines the fill character.</param>
		public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
		{
			//Clear buffer
			if (rectangle.Left == -1 && rectangle.Top == -1 && rectangle.Right == -1 && rectangle.Bottom == -1 &&
				fill.BufferCellType == BufferCellType.Complete && fill.Character == ' ' &&
				fill.BackgroundColor == DefaultBackgroundColor && fill.ForegroundColor == DefaultForegroundColor)
				_HostUI.Host.ExecuteMethodSync(() => _HostUI.Host.HostOwner.BookServer.Document.Text = string.Empty);
			else
				throw new NotImplementedException("The method or operation is not implemented.");
		}
	}
}