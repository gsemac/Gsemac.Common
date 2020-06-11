using System;

namespace Gsemac.Logging {

    public class ConsoleLogger :
        SynchronizedLoggerBase {

        // Protected members

        protected override void Log(ILogMessage logMessage, string formattedMessage) {

            ConsoleColor? color = GetLogLevelColor(logMessage.LogLevel);

            if (color.HasValue)
                Console.ForegroundColor = color.Value;

            Console.Write(formattedMessage);

            Console.ResetColor();

        }

        // Private members

        private ConsoleColor? GetLogLevelColor(LogLevel logLevel) {

            switch (logLevel) {

                case LogLevel.Warning:
                    return ConsoleColor.Yellow;

                case LogLevel.Error:
                    return ConsoleColor.Red;

                default:
                    return null;

            }

        }

    }

}