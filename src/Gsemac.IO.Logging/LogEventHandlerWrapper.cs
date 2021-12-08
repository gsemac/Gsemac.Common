namespace Gsemac.IO.Logging {

    public class LogEventHandlerWrapper {

        // Public members

        public LogEventHandlerWrapper(LogEventHandler logEventHandler, string name) {

            this.name = logEventHandler;
            this.sourceName = name;

        }

        public void Log(ILogMessage logMessage) {

            name?.Invoke(this, new LogEventArgs(logMessage));

        }
        public void Log(LogLevel level, string source, string message) {

            Log(new LogMessage(level, source, message));

        }
        public void Log(string source, string message) {

            Log(new LogMessage(LogLevel.Info, source, message));

        }
        public void Log(string message) {

            Log(sourceName, message);

        }
        public void Log(object sender, LogEventArgs e) {

            Log(e.Message);

        }

        public void Debug(string message) {

            Log(LogLevel.Debug, sourceName, message);

        }
        public void Info(string message) {

            Log(LogLevel.Info, sourceName, message);

        }
        public void Warning(string message) {

            Log(LogLevel.Warning, sourceName, message);

        }
        public void Error(string message) {

            Log(LogLevel.Error, sourceName, message);

        }

        // Private members

        private readonly string sourceName;
        private readonly LogEventHandler name;

    }

}