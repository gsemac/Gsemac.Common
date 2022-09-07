/* Unmerged change from project 'Gsemac.IO.Logging (netstandard2.0)'
Before:
namespace Gsemac.IO.Logging.Extensions {
After:
namespace Gsemac.IO.Logging.IO.Logging;
using Gsemac.IO.Logging.Extensions {
*/

namespace Gsemac.IO.Logging {

    public static class LoggerExtensions {

        // Public members

        public static void Log(this ILogger logger, LogLevel logLevel, string source, string message) {

            logger.Log(new LogMessage(logLevel, source, message));

        }
        public static void Log(this ILogger logger, LogLevel logLevel, string source, object obj) {

            logger.Log(logLevel, source, obj.ToString());

        }
        public static void Log(this ILogger logger, string message) {

            logger.Log(LogLevel.Info, logger.Name, message);

        }
        public static void Log(this ILogger logger, LogLevel logLevel, string message) {

            logger.Log(logLevel, logger.Name, message);

        }
        public static void Log(this ILogger logger, object obj) {

            logger.Log(obj.ToString());

        }
        public static void Log(this ILogger logger, LogLevel logLevel, object obj) {

            logger.Log(logLevel, logger.Name, obj.ToString());

        }

        public static void Debug(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Debug, source, message);

        }
        public static void Debug(this ILogger logger, string source, object obj) {

            logger.Debug(source, obj.ToString());

        }
        public static void Debug(this ILogger logger, string message) {

            logger.Debug(logger.Name, message);

        }
        public static void Debug(this ILogger logger, object obj) {

            logger.Debug(obj.ToString());

        }

        public static void Info(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Info, source, message);

        }
        public static void Info(this ILogger logger, string source, object obj) {

            logger.Info(source, obj.ToString());

        }
        public static void Info(this ILogger logger, string message) {

            logger.Info(logger.Name, message);

        }
        public static void Info(this ILogger logger, object obj) {

            logger.Info(obj.ToString());

        }
        public static void Warning(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Warning, source, message);

        }
        public static void Warning(this ILogger logger, string source, object obj) {

            logger.Warning(source, obj.ToString());

        }
        public static void Warning(this ILogger logger, string message) {

            logger.Warning(logger.Name, message);

        }
        public static void Warning(this ILogger logger, object obj) {

            logger.Warning(obj.ToString());

        }
        public static void Error(this ILogger logger, string source, string message) {

            logger.Log(LogLevel.Error, source, message);

        }
        public static void Error(this ILogger logger, string source, object obj) {

            logger.Error(source, obj.ToString());

        }
        public static void Error(this ILogger logger, string message) {

            logger.Error(logger.Name, message);

        }
        public static void Error(this ILogger logger, object obj) {

            logger.Error(obj.ToString());

        }

        public static LogEventHandler CreateEventHandler(this ILogger logger) {

            return (sender, e) => logger.Log(e.Message);

        }

    }

}