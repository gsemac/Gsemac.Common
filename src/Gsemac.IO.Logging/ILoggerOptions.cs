namespace Gsemac.IO.Logging {

    public interface ILoggerOptions {

        bool Enabled { get; }
        string LogDirectoryPath { get; set; }
        ILogHeaderCollection Headers { get; }
        bool IgnoreExceptions { get; }
        ILogMessageFormatter MessageFormatter { get; }
        ILogFileNameFormatter FilenameFormatter { get; }
        ILogRetentionPolicy RetentionPolicy { get; }

    }

}