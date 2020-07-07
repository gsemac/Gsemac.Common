using System;

namespace Gsemac.Logging {

    public abstract class LoggableBase {

        // Public members

        public event EventHandler<LogEventArgs> Log;

        // Protected members

        protected LoggableBase() {

            source = GetType().Name;

        }
        protected LoggableBase(string source) {

            this.source = source;

        }

        protected void OnLog(ILogMessage logMessage) {

            Log?.Invoke(this, new LogEventArgs(logMessage));

        }
        protected void OnLog(LogLevel level, string source, string message) {

            OnLog(new LogMessage(level, source, message));

        }
        protected void OnLog(string source, string message) {

            OnLog(new LogMessage(LogLevel.Info, source, message));

        }
        protected void OnLog(string message) {

            OnLog(source, message);

        }

        protected void Info(string message) {

            OnLog(LogLevel.Info, source, message);

        }
        protected void Warning(string message) {

            OnLog(LogLevel.Warning, source, message);

        }
        protected void Error(string message) {

            OnLog(LogLevel.Error, source, message);

        }

        // Private members

        private readonly string source;

    }

}