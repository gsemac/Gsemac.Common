namespace Gsemac.IO.Logging {

    public sealed class NullLogger :
         ILogger {

        // Public members

        public event LogEventHandler MessageLogged { add { } remove { } }

        public bool Enabled { get; set; }
        public string Name { get; }

        public void Log(ILogMessage message) { }

    }

}