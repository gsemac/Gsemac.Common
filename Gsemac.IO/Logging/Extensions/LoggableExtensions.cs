namespace Gsemac.IO.Logging.Extensions {

    public static class LoggableExtensions {

        public static void LogTo(this ILoggable source, ILogger logger) {

            source.Log += logger.CreateLogEventHandler();

        }

    }

}