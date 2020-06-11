using System;

namespace Gsemac.Logging {

    public enum LogLevel {
        Debug,
        Info,
        Warning,
        Error
    }

    public interface ILogMessage {

        LogLevel LogLevel { get; }
        string Source { get; }
        string Message { get; }
        Exception Exception { get; }

    }

}