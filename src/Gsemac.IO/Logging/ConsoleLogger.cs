﻿using System;

namespace Gsemac.IO.Logging {

    public class ConsoleLogger :
        SynchronizedLoggerBase {

        // Public members

        public ConsoleLogger() :
            this(LoggerOptions.Default) {
        }
        public ConsoleLogger(ILoggerOptions options) {
        }
        public ConsoleLogger(bool enabled) :
            base(enabled) {
        }


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