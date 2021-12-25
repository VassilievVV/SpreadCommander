using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadCommander.Common.SqlScript
{
    public class SqlMessage
    {
        public enum SqlMessageType { Message, Error, Log }

        public SqlMessage()
        {
            TimeStamp = DateTime.Now;
        }

        public SqlMessage(SqlMessageType messageType, string message): this()
        {
            MessageType = messageType;
            Message     = message;
        }

        public SqlMessage(DbException ex): this()
        {
            MessageType = SqlMessageType.Error;
            if (ex is SqlException sqlException)
                Line = sqlException.LineNumber;
            ErrorCode = ex.ErrorCode;
            HelpLink  = ex.HelpLink;
            HResult   = ex.HResult;
            Message   = ex.Message;
            Source    = ex.Source;

            if (ex.InnerException != null)
            {
                var errorText = new StringBuilder();
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    if (errorText.Length > 0)
                        errorText.Append(Environment.NewLine);
                    errorText.Append(innerEx.Message);

                    innerEx = innerEx.InnerException;
                }
                InnerExceptionMessage = errorText.ToString();
            }
        }

        public SqlMessageType MessageType		{ get; set; }
        public int Line							{ get; set; }
        public int ErrorCode					{ get; set; }
        public string HelpLink					{ get; set; }
        public int HResult						{ get; set; }
        public string Message					{ get; set; }
        public string Source					{ get; set; }
        public string InnerExceptionMessage		{ get; set; }
        public string State						{ get; set; }
        public DateTime TimeStamp               { get; set; }

        public string Description
        {
            get
            {
                var result = new StringBuilder();
                result.Append(Message);
                if (!string.IsNullOrWhiteSpace(InnerExceptionMessage))
                    result.AppendLine().AppendLine().Append(InnerExceptionMessage);
                return result.ToString();
            }
        }

        public string ShortDescription
        {
            get
            {
                var description = Description;
                if (string.IsNullOrWhiteSpace(description))
                    return string.Empty;

                int eol = description.IndexOfAny(new char[] { '\n', '\r' });
                if (eol > 0)
                    description = description[..eol];

                if (description.Length > 100)
                    description = $"{(description[..100])} ...";

                return description;
            }
        }
    }
}
