namespace Gsemac.IO.Logging {

    public class LoggerOptions :
        ILoggerOptions {

        public bool Enabled { get; set; } = true;
        public string LogDirectoryPath { get; set; }
        public ILogHeaderCollection Headers { get; set; } = new LogHeaderCollection();
        public bool IgnoreExceptions { get; set; } = true;
        public ILogMessageFormatter MessageFormatter { get; set; } = new LogMessageFormatter();
        public ILogFileNameFormatter FilenameFormatter { get; set; } = new UnixTimestampLogFileNameFormatter();
        public ILogRetentionPolicy RetentionPolicy { get; set; } = new NeverDeleteLogRetentionPolicy();

        public static LoggerOptions Default => new LoggerOptions();

        public LoggerOptions() { }
        public LoggerOptions(ILoggerOptions options) {

            Enabled = options.Enabled;
            LogDirectoryPath = options.LogDirectoryPath;
            Headers = options.Headers;
            IgnoreExceptions = options.IgnoreExceptions;
            MessageFormatter = options.MessageFormatter;
            FilenameFormatter = options.FilenameFormatter;
            RetentionPolicy = options.RetentionPolicy;

        }

    }

}