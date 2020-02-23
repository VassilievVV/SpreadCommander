using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Code
{
	public class ScriptParseError
	{
		public string ErrorID			{ get; set; }

		public string Message			{ get; set; }

		public int StartLineNumber		{ get; set; }

		public int StartColumnNumber	{ get; set; }

		public int EndLineNumber		{ get; set; }

		public int EndColumnNumber		{ get; set; }

		public bool IncompleteInput		{ get; set; }

		public string Location
		{
			get
			{
				if ((EndLineNumber == StartLineNumber && EndColumnNumber == StartColumnNumber) || 
					(EndLineNumber < StartLineNumber))
					return $"{StartLineNumber}x{StartColumnNumber}";
				return $"{StartLineNumber}x{StartColumnNumber}-{EndLineNumber}x{EndColumnNumber}";
			}
		}
	}
}
