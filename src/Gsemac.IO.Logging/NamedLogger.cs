namespace Gsemac.IO.Logging {

    public class NamedLogger :
         ILogger {

        // Public members

        public event LogEventHandler MessageLogged;

        public bool Enabled { get; set; } = true;
        public string Name { get; }

        public NamedLogger(ILogger baseLogger, string name) {

            this.baseLogger = baseLogger;
            this.Name = name;

        }

        public void Log(ILogMessage message) {

            if (!Enabled)
                return;

            baseLogger.Log(message);

            MessageLogged?.Invoke(this, new LogEventArgs(message));

        }

        // Private members

        private readonly ILogger baseLogger;

    }

}