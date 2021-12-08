using System;

namespace Gsemac.IO.Logging {

    public class LogEventArgs :
        EventArgs {

        // Public members

        public ILogMessage Message { get; }

        public LogEventArgs(ILogMessage logMessage) {

            Message = logMessage;

        }
        public LogEventArgs(LogLevel logLevel, string source, string message) :
            this(new LogMessage(logLevel, source, message)) {
        }

        public override string ToString() {

            return Message.ToString();

        }

    }

}