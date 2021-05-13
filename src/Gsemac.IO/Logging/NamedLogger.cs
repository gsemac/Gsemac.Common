namespace Gsemac.IO.Logging {

    public class NamedLogger :
         ILogger {

        // Public members

        public event LogEventHandler Logged;

        public bool Enabled { get; set; } = true;
        public string Name { get; }

        public NamedLogger(ILogger baseLogger, string name) {

            this.baseLogger = baseLogger;
            this.Name = name;

        }

        public void Log(ILogMessage message) {

            baseLogger.Log(message);

            Logged?.Invoke(this, new LogEventArgs(message));

        }

        // Private members

        private readonly ILogger baseLogger;

    }

}