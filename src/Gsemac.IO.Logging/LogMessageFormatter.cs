using System;
using System.Text;

namespace Gsemac.IO.Logging {

    public class LogMessageFormatter :
        ILogMessageFormatter {

        // Public members

        public string TimestampFormat { get; } = "HH:mm:ss";

        public void SetColumnWidth(int index, int width) {

            if (index < 0 || index >= columnWidths.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            columnWidths[index] = width;

        }

        public string Format(ILogMessage message) {

            StringBuilder sb = new StringBuilder();

            string timestamp = FormatTimestamp(DateTime.Now).PadRight(columnWidths[0]).Substring(0, columnWidths[0]);
            string source = FormatSource(message.Source).PadRight(columnWidths[1]).Substring(0, columnWidths[1]);

            sb.Append(timestamp);
            sb.Append(" ");
            sb.Append(source);
            sb.Append(" ");
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

        // Private members

        private readonly int[] columnWidths = new int[] { 8, 11 };

    }

}