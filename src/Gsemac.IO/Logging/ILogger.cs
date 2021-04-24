namespace Gsemac.IO.Logging {

    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public interface ILogger {

        event LogEventHandler Logged;

        bool Enabled { get; set; }

        void Log(ILogMessage message);

    }

}