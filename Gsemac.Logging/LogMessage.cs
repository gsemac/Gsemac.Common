using System;

namespace Gsemac.Logging {

    public class LogMessage :
        ILogMessage {

        // Public members

        public LogLevel LogLevel { get; }
        public string Source { get; }
        public string Message { get; }
        public Exception Exception { get; }

        public LogMessage(LogLevel logLevel, string source, string message) :
            this(logLevel, source, message, null) {
        }
        public LogMessage(LogLevel logLevel, string source, string message, Exception exception) {

            this.LogLevel = logLevel;
            this.Source = source;
            this.Message = message;
            this.Exception = exception;

        }

        public override string ToString() {

            return new LogMessageFormatter().Format(this);

        }

    }

}