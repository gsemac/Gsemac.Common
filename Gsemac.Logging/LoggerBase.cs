using System;
using System.Reflection;

namespace Gsemac.Logging {

    public abstract class LoggerBase :
        ILogger {

        // Public members

        public event EventHandler<LogEventArgs> Logged;

        public bool Enabled { get; set; } = true;
        public bool IgnoreExceptions { get; set; } = true;
        public ILogHeader Header { get; set; } = new LogHeader();
        public ILogMessageFormatter LogMessageFormatter { get; set; } = new LogMessageFormatter();

        public virtual void Log(ILogMessage message) {

            try {

                if (Enabled) {

                    if (firstWrite) {

                        firstWrite = false;

                        WriteHeader();

                    }

                    Log(message, LogMessageFormatter.Format(message));

                }

                // Event handlers are always invoked, even when the logger is disabled.

                OnLogged(message);

            }
            catch (Exception ex) {

                if (!IgnoreExceptions)
                    throw ex;

            }

        }

        // Protected members

        protected void OnLogged(ILogMessage message) {

            Logged?.Invoke(this, new LogEventArgs(message));

        }

        protected abstract void Log(ILogMessage logMessage, string formattedMessage);

        protected virtual void WriteHeader() {

            foreach (string key in Header.Keys)
                Log(new LogMessage(LogLevel.Info, Assembly.GetEntryAssembly().GetName().Name, $"{key}: {Header[key]}"));

        }

        private bool firstWrite = true;

    }

}