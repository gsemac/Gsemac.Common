namespace Gsemac.IO.Logging {

    public interface ILoggerOptions {

        string LogDirectoryPath { get; set; }
        ILogHeaderCollection Headers { get; }
        bool IgnoreExceptions { get; }
        ILogMessageFormatter MessageFormatter { get; }
        ILogFilenameFormatter FilenameFormatter { get; }
        ILogRetentionPolicy RetentionPolicy { get; }

    }

}