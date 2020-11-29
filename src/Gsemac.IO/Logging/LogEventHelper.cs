namespace Gsemac.IO.Logging {

    public class LogEventHelper {

        // Public members

        public LogEventHelper(string sourceName, LogEventHandler logEventHandler) {

            this.sourceName = sourceName;
            this.logEventHandler = logEventHandler;

        }

        public void OnLog(ILogMessage logMessage) {

            logEventHandler?.Invoke(this, new LogEventArgs(logMessage));

        }
        public void OnLog(LogLevel level, string source, string message) {

            OnLog(new LogMessage(level, source, message));

        }
        public void OnLog(string source, string message) {

            OnLog(new LogMessage(LogLevel.Info, source, message));

        }
        public void OnLog(string message) {

            OnLog(sourceName, message);

        }

        public void Info(string message) {

            OnLog(LogLevel.Info, sourceName, message);

        }
        public void Warning(string message) {

            OnLog(LogLevel.Warning, sourceName, message);

        }
        public void Error(string message) {

            OnLog(LogLevel.Error, sourceName, message);

        }

        // Private members

        private readonly string sourceName;
        private readonly LogEventHandler logEventHandler;

    }

}