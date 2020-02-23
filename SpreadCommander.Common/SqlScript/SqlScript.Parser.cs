using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.SqlScript
{
	public partial class SqlScript
	{
		public static List<SqlScriptCommand> SplitScript(string script)
		{
			var result = new List<SqlScriptCommand>();
			if (string.IsNullOrWhiteSpace(script))
				return result;

			int i = 0;
			int start = 0;
			int lineNumber = 0;
			int startLineNumber = 0;
			while (i < script.Length)
			{
				char c = script[i];
				char next = (i < script.Length - 1) ? script[i + 1] : '\0';

				if (c == '\'')
				{
					//skip string '...'
					i++;
					while (i < script.Length)
					{
						if (script[i] == '\'')
							break;
						i++;
					}
				}
				else if (c == '"')
				{
					//script string "..."
					i++;
					while (i < script.Length)
					{
						if (script[i] == '"')
							break;
						i++;
					}
				}
				else if (c == '`')
				{
					//script string `...`
					i++;
					while (i < script.Length)
					{
						if (script[i] == '`')
							break;
						i++;
					}
				}
				else if (c == '-' && next == '-')
				{
					//skip comment --...
					i++; i++;
					while (i < script.Length)
					{
						if (script[i] == '\r' || script[i] == '\n')
						{
							if (i < script.Length - 1 && script[i] == '\r' && script[i] == '\n')
								i++;
							lineNumber++;

							break;
						}
						i++;
					}
				}
				else if (c == '/' && next == '*')
				{
					//skipe comment /*...*/
					i++; i++;
					while (i < script.Length - 1)
					{
						if (script[i] == '*' && script[i + 1] == '/')
						{
							i++;
							break;
						}
						else if (script[i] == '\r' || script[i] == '\n')
						{
							if (script[i] == '\r' && script[i + 1] == '\n')
								i++;
							lineNumber++;
						}
						i++;
					}
				}
				else if ((c == '\r' || c == '\n') || i == 0)
				{
					i++;
					while (i < script.Length - 1)
					{
						var c2 = script[i];
						if (c2 == ' ' || c2 == '\t')
						{
							i++;
							continue;
						}
						else if (c2 == '\r' || c2 == '\n')
						{
							if (c2 == '\r' && script[i + 1] == '\n')
								i++;
							lineNumber++;
						}
						else if ((c2 == 'g' || c2 == 'G') && (script[i + 1] == 'o' || script[i + 1] == 'O'))
						{
							bool? isEOL = null;
							int j = i + 2;
							while (j < script.Length)
							{
								switch (script[j])
								{
									case ' ':
									case '\t':
										break;
									case '\r':
									case '\n':
										j++;
										while (j < script.Length && (script[j] == '\r' || script[j] == '\n'))
										{
											if (script[j] == '\n')
												lineNumber++;
											else if (script[j] == '\r')
											{
												if (j < script.Length && script[j + 1] == '\n')
													j++;
												lineNumber++;
											}
											else
												break;

											j++;
										}
										isEOL = true;
										break;
									default:
										isEOL = false;
										break;
								}
								if (isEOL.HasValue)
									break;
								j++;
							}

							string cmd = script[start..i];
							if (!string.IsNullOrWhiteSpace(cmd))
							{
								var DbCommand = new SqlScriptCommand() { Text = cmd, StartLineNumber = startLineNumber + 1 };
								result.Add(DbCommand);
							}

							i = j;
							start = i;
							startLineNumber = lineNumber;
						}

						break;
					}
					i--;
				}


				i++;
			}

			if (i > script.Length)
				i = script.Length;

			if (i > start)
			{
				string cmd = script[start..i];
				if (!string.IsNullOrWhiteSpace(cmd))
				{
					var DbCommand = new SqlScriptCommand() { Text = cmd, StartLineNumber = startLineNumber + 1 };
					result.Add(DbCommand);
				}
			}

			return result;
		}
	}
}
