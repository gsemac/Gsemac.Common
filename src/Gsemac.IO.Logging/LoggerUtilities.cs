using System;

namespace Gsemac.IO.Logging {

    internal static class LoggerUtilities {

        // Public members

        public static ConsoleColor GetLogLevelColor(LogLevel logLevel) {

            switch (logLevel) {

                case LogLevel.Warning:
                    return ConsoleColor.Yellow;

                case LogLevel.Error:
                    return ConsoleColor.Red;

                default:
                    return Console.ForegroundColor;

            }

        }

    }

}