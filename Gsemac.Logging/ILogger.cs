using System;

namespace Gsemac.Logging {

    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public interface ILogger {

        event LogEventHandler Logged;

        bool Enabled { get; set; }
        bool IgnoreExceptions { get; set; }
        ILogHeader Header { get; set; }
        ILogMessageFormatter LogMessageFormatter { get; set; }

        void Log(ILogMessage message);

    }

}