namespace Gsemac.IO.Logging {

    public class LoggerOptions :
        ILoggerOptions {

        public string LogDirectoryPath { get; set; }
        public ILogHeaderCollection Headers { get; set; } = new LogHeaderCollection();
        public bool IgnoreExceptions { get; set; } = true;
        public ILogMessageFormatter MessageFormatter { get; set; } = new LogMessageFormatter();
        public ILogFilenameFormatter FilenameFormatter { get; set; } = new LogFilenameFormatter();
        public ILogRetentionPolicy RetentionPolicy { get; set; } = new NeverDeleteLogRetentionPolicy();

        public static LoggerOptions Default => new LoggerOptions();

        public LoggerOptions() { }
        public LoggerOptions(ILoggerOptions options) {

            this.LogDirectoryPath = options.LogDirectoryPath;
            this.Headers = options.Headers;
            this.IgnoreExceptions = options.IgnoreExceptions;
            this.MessageFormatter = options.MessageFormatter;
            this.FilenameFormatter = options.FilenameFormatter;
            this.RetentionPolicy = options.RetentionPolicy;

        }

    }

}