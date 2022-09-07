namespace Gsemac.IO.Logging {

    public class LoggerOptions :
        ILoggerOptions {

        public bool Enabled { get; set; } = true;
        public string DirectoryPath { get; set; } = "log";
        public ILogHeaderCollection Headers { get; set; } = new LogHeaderCollection();
        public bool IgnoreExceptions { get; set; } = true;
        public ILogMessageFormatter MessageFormatter { get; set; } = new LogMessageFormatter();
        public ILogFileNameFormatter FileNameFormatter { get; set; } = new UnixTimestampLogFileNameFormatter();
        public ILogRetentionPolicy RetentionPolicy { get; set; } = new PreserveLogRetentionPolicy();

        public static LoggerOptions Default => new LoggerOptions();

        public LoggerOptions() { }
        public LoggerOptions(ILoggerOptions options) {

            Enabled = options.Enabled;
            DirectoryPath = options.DirectoryPath;
            Headers = options.Headers;
            IgnoreExceptions = options.IgnoreExceptions;
            MessageFormatter = options.MessageFormatter;
            FileNameFormatter = options.FileNameFormatter;
            RetentionPolicy = options.RetentionPolicy;

        }

    }

}