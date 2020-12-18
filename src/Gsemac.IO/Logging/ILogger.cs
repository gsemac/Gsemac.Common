namespace Gsemac.IO.Logging {

    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public interface ILogger {

        event LogEventHandler Logged;

        bool Enabled { get; set; }
        ILogHeaderCollection Headers { get; set; }
        bool IgnoreExceptions { get; set; }
        ILogMessageFormatter LogMessageFormatter { get; set; }

        void Log(ILogMessage message);

    }

}