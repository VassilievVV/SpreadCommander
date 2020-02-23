using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.Extensions
{
	public static class StringBufferExtensions
	{
		public static int FindNextCharIndex(this StringBuilder buffer, char c, int startIndex)
		{
			int i = startIndex;

			while (i < buffer.Length)
			{
				if (buffer[i] == c)
					return i;

				i++;
			}

			return -1;
		}

		public static int FindLastCharIndex(this  StringBuilder buffer, char c, int startIndex)
		{
			int i = startIndex;

			while (i >= 0)
			{
				if (buffer[i] == c)
					return i;

				i--;
			}

			return -1;
		}

		public static int FindNextStringIndex(this StringBuilder buffer, string str, int startIndex)
		{
			if (string.IsNullOrEmpty(str))
				return -1;

			int i = startIndex;
			char c = str[0];

			while (i < buffer.Length)
			{
				if (buffer[i] == c)
				{
					if (i > buffer.Length - str.Length)   //not enough characters in bufferer to match whole substring
						return -1;

					if (string.Compare(buffer.ToString(i, str.Length), str, false) == 0)
						return i;
				}

				i++;
			}

			return -1;
		}

		public static bool ContinuesWith(this StringBuilder buffer, string str, int startIndex)
		{
			if (string.IsNullOrEmpty(str))
				return false;

			if (startIndex + str.Length > buffer.Length)
				return false;

			var subString = buffer.ToString(startIndex, str.Length);
			var result = string.Compare(subString, str, false);
			return (result == 0);
		}

		public static bool StartsWith(this StringBuilder buffer, string str) =>
			ContinuesWith(buffer, str, 0);
	}
}
