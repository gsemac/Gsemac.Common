namespace Gsemac.IO.Logging {

    public interface ILoggerOptions {

        bool Enabled { get; }
        string DirectoryPath { get; }
        ILogHeaderCollection Headers { get; }
        bool IgnoreExceptions { get; }
        ILogMessageFormatter MessageFormatter { get; }
        ILogFileNameFormatter FileNameFormatter { get; }
        ILogRetentionPolicy RetentionPolicy { get; }

    }

}