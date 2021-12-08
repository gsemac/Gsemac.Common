namespace Gsemac.IO.Logging {

    public class NullLogger :
         ILogger {

        public bool Enabled { get; set; }
        public string Name { get; set; }

        public event LogEventHandler Logged { add { } remove { } }

        public void Log(ILogMessage message) { }

    }

}