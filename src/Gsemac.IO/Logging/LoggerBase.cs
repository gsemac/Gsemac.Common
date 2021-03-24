﻿using System;
using System.Linq;
using System.Reflection;

namespace Gsemac.IO.Logging {

    public abstract class LoggerBase :
        ILogger {

        // Public members

        public event LogEventHandler Logged {
            add {

                lock (loggedEventMutex) {
                    loggedEvent += value;
                }

                // Write log headers to the new handler immediately.

                WriteHeaders(value);

            }
            remove {

                lock (loggedEventMutex) {
                    loggedEvent -= value;
                }

            }
        }

        public bool Enabled {
            get => enabled;
            set {

                enabled = value;

                if (enabled)
                    WriteHeaders();

            }
        }
        public ILogHeaderCollection Headers { get; set; } = new LogHeaderCollection();
        public bool IgnoreExceptions { get; set; } = true;
        public ILogMessageFormatter LogMessageFormatter { get; set; } = new LogMessageFormatter();

        public virtual void Log(ILogMessage message) {

            try {

                if (Enabled)
                    Log(message, LogMessageFormatter.Format(message));

                // Event handlers are always invoked, even when the logger is disabled.

                OnLogged(message);

            }
            catch (Exception ex) {

                if (!IgnoreExceptions)
                    throw ex;

            }

        }

        // Protected members

        protected LoggerBase() :
            this(true) {
        }
        protected LoggerBase(bool enabled) {

            this.enabled = enabled;

            if (enabled)
                WriteHeaders();

        }

        protected void OnLogged(ILogMessage message) {

            if (loggedEvent is object) {

                foreach (LogEventHandler logEventHandler in loggedEvent.GetInvocationList()) {

                    try {

                        logEventHandler?.Invoke(this, new LogEventArgs(message));

                    }
                    catch (Exception ex) {

                        // If exceptions are ignored, exceptions can be thrown in event handlers without interrupting other event handlers.

                        if (!IgnoreExceptions)
                            throw ex;

                    }

                }

            }

        }

        protected abstract void Log(ILogMessage logMessage, string formattedMessage);

        protected virtual void WriteHeaders() {

            // These headers should be written as soon as the logger is enabled.
            // This will not trigger the event handlers, and they will have headers written separately (when they add event handlers).

            if (!wroteHeaders)
                WriteHeaders((sender, e) => Log(e.Message, LogMessageFormatter.Format(e.Message)));

            wroteHeaders = true;

        }

        protected virtual void WriteHeaders(LogEventHandler eventHandler) {

            // Keys are copied into an array so we don't run into trouble if the headers are modified in another thread.

            foreach (string key in Headers?.Keys.ToArray()) {

                string headerName = key;
                string headerValue = "";

                // Since header values can be retrieved with a lambda, it's possible that an exception is thrown while retrieving the value.
                // If the user has opted to ignore exceptions, any exceptions should be caught.

                try {

                    headerValue = Headers[key];

                }
                catch (Exception ex) {

                    if (!IgnoreExceptions)
                        throw ex;

                    headerValue = ex.ToString();

                }

                ILogMessage logMessage = new LogMessage(LogLevel.Info, Assembly.GetEntryAssembly().GetName().Name, $"{headerName}: {headerValue}");
                LogEventArgs logEventArgs = new LogEventArgs(logMessage);

                try {

                    eventHandler.Invoke(this, logEventArgs);

                }
                catch (Exception ex) {

                    if (!IgnoreExceptions)
                        throw ex;

                }

            }

        }

        // Private members

        private readonly object loggedEventMutex = new object();
        private bool enabled = true;
        private LogEventHandler loggedEvent;
        private bool wroteHeaders = false;

    }

}