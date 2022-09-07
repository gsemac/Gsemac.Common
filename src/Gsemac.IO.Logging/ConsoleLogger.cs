using System;

namespace Gsemac.IO.Logging {

    public class ConsoleLogger :
        SynchronizedLoggerBase {

        // Public members

        public ConsoleLogger() :
            this(LoggerOptions.Default) {
        }
        public ConsoleLogger(ILoggerOptions options) :
            base(options) {
        }

        // Protected members

        protected override void Log(ILogMessage message, string formattedMessage) {

#if DEBUG

            if (message is null)
                throw new ArgumentNullException(nameof(message));

#endif

            Console.ForegroundColor = LoggerUtilities.GetLogLevelColor(message.LogLevel);

            Console.Write(formattedMessage);

            Console.ResetColor();

        }

    }

}