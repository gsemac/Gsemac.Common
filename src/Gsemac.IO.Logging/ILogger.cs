namespace Gsemac.IO.Logging {

    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public interface ILogger {

        event LogEventHandler MessageLogged;

        bool Enabled { get; set; }
        string Name { get; }

        void Log(ILogMessage message);

    }

}