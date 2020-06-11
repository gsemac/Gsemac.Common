using System;

namespace Gsemac.Logging {

    public delegate void LoggedEventhandler(object sender, ILogMessage logMessage);

    public interface ILogger {

        event LoggedEventhandler Logged;

        bool Enabled { get; set; }
        bool IgnoreExceptions { get; set; }
        ILogHeader Header { get; set; }
        ILogMessageFormatter LogMessageFormatter { get; set; }

        void Log(ILogMessage message);

    }

}