using System;

namespace Gsemac.Logging {

    public interface ILogger {

        event EventHandler<LogEventArgs> Logged;

        bool Enabled { get; set; }
        bool IgnoreExceptions { get; set; }
        ILogHeader Header { get; set; }
        ILogMessageFormatter LogMessageFormatter { get; set; }

        void Log(ILogMessage message);

    }

}