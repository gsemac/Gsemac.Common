using System.Reflection;

namespace Gsemac.IO.Logging.Extensions {

    public static class LoggerExtensions {

        // Public members

        public static void Log(this ILogger logger, LogLevel logLevel, string source, string message) {

            logger.Log(new LogMessage(logLevel, source, message));

        }
        public static void Log(this ILogger logger, string message) {

            logger.Log(LogLevel.Info, GetDefaultSource(), message);

        }

        public static void Debug(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Debug, source, message);

        }
        public static void Debug(this ILogger logger, string message) {

            logger.Debug(GetDefaultSource(), message);

        }
        public static void Info(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Info, source, message);

        }
        public static void Info(this ILogger logger, string message) {

            logger.Info(GetDefaultSource(), message);

        }
        public static void Warning(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Warning, source, message);

        }
        public static void Warning(this ILogger logger, string message) {

            logger.Warning(GetDefaultSource(), message);

        }
        public static void Error(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Error, source, message);

        }
        public static void Error(this ILogger logger, string message) {

            logger.Error(GetDefaultSource(), message);

        }

        public static LogEventHandler CreateLogEventHandler(this ILogger logger) {

            return (sender, e) => logger.Log(e.Message);

        }

        public static ILogger AddSource(this ILogger logger, ILoggable source) {

            source.Log += logger.CreateLogEventHandler();

            return logger;

        }

        // Private members

        private static string GetDefaultSource() {

            return Assembly.GetEntryAssembly().GetName().Name;

        }

    }

}