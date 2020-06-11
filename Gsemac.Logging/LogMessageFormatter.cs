using System;
using System.Text;

namespace Gsemac.Logging {

    public class LogMessageFormatter :
        ILogMessageFormatter {

        // Public members

        public string TimestampFormat { get; } = "HH:mm:ss";
        public int ColumnWidth { get; } = 12;

        public string Format(ILogMessage message) {

            StringBuilder sb = new StringBuilder();

            string timestamp = FormatTimestamp(DateTime.Now).PadRight(ColumnWidth);
            string source = FormatSource(message.Source).PadRight(ColumnWidth);

            if (ColumnWidth > 0) {

                timestamp = timestamp.Substring(0, ColumnWidth);
                source = source.Substring(0, ColumnWidth);

            }

            sb.Append(timestamp);
            sb.Append(source);
            sb.Append(FormatLogLevel(message.LogLevel));

            sb.Append(" ");

            sb.AppendLine(FormatMessage(message.Message));

            if (message.Exception != null)
                sb.AppendLine(FormatException(message.Exception));

            return sb.ToString();

        }

        // Protected members

        protected virtual string FormatTimestamp(DateTime dateTime) {

            return dateTime.ToString(TimestampFormat);

        }
        protected virtual string FormatSource(string source) {

            return source;

        }
        protected virtual string FormatLogLevel(LogLevel logLevel) {

            switch (logLevel) {

                case LogLevel.Debug:
                    return "[DEBUG]";

                case LogLevel.Warning:
                    return "[WARN]";

                case LogLevel.Error:
                    return "[ERROR]";

                default:
                    return "[INFO]";

            }

        }
        protected virtual string FormatMessage(string message) {

            return message;

        }
        protected virtual string FormatException(Exception exception) {

            return exception?.ToString();

        }

    }

}