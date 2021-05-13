namespace Gsemac.IO.Logging.Extensions {

    public static class LoggerExtensions {

        // Public members

        public static void Log(this ILogger logger, LogLevel logLevel, string source, string message) {

            logger.Log(new LogMessage(logLevel, source, message));

        }
        public static void Log(this ILogger logger, LogLevel logLevel, string source, object obj) {

            Log(logger, logLevel, source, obj.ToString());

        }
        public static void Log(this ILogger logger, string message) {

            logger.Log(LogLevel.Info, logger.Name, message);

        }
        public static void Log(this ILogger logger, object obj) {

            Log(logger, obj.ToString());

        }

        public static void Debug(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Debug, source, message);

        }
        public static void Debug(this ILogger logger, string source, object obj) {

            Debug(logger, source, obj.ToString());

        }
        public static void Debug(this ILogger logger, string message) {

            logger.Debug(logger.Name, message);

        }
        public static void Debug(this ILogger logger, object obj) {

            Debug(logger, obj.ToString());

        }

        public static void Info(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Info, source, message);

        }
        public static void Info(this ILogger logger, string source, object obj) {

            Info(logger, source, obj.ToString());

        }
        public static void Info(this ILogger logger, string message) {

            logger.Info(logger.Name, message);

        }
        public static void Info(this ILogger logger, object obj) {

            Info(logger, obj.ToString());

        }
        public static void Warning(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Warning, source, message);

        }
        public static void Warning(this ILogger logger, string source, object obj) {

            Warning(logger, source, obj.ToString());

        }
        public static void Warning(this ILogger logger, string message) {

            logger.Warning(logger.Name, message);

        }
        public static void Warning(this ILogger logger, object obj) {

            Warning(logger, obj.ToString());

        }
        public static void Error(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Error, source, message);

        }
        public static void Error(this ILogger logger, string source, object obj) {

            Error(logger, source, obj.ToString());

        }
        public static void Error(this ILogger logger, string message) {

            logger.Error(logger.Name, message);

        }
        public static void Error(this ILogger logger, object obj) {

            Error(logger, obj.ToString());

        }

        public static LogEventHandler CreateEventHandler(this ILogger logger) {

            return (sender, e) => logger.Log(e.Message);

        }

        public static ILogger AddEventSource(this ILogger logger, ILogEventSource source) {

            source.Log += logger.CreateEventHandler();

            return logger;

        }

    }

}