using System.IO;

namespace Gsemac.IO.Logging {

    public class FileLogger :
        SynchronizedLoggerBase {

        // Public members

        public FileLogger() :
            this(LoggerOptions.Default) {
        }
        public FileLogger(string fileName) :
            this(new LoggerOptions() {
                FileNameFormatter = new LogFileNameFormatter(fileName),
            }) {
        }
        public FileLogger(ILoggerOptions options) :
            base(new LoggerOptions(options) { Enabled = false }) {

            // Headers are written as soon as the logger is instantiated, but we haven't set the "options" member yet, so we won't have the right path.
            // The logger is temporarily disabled, and then re-enabled after assigning to the member, which causes the headers to be written to the correct path.

            this.options = options;

            Enabled = options.Enabled;

        }

        // Protected members

        protected override void Log(ILogMessage message, string formattedMessage) {

            CreateLogFile();

            string fullLogFilePath = logFilePath;

            if (!string.IsNullOrWhiteSpace(options.DirectoryPath))
                fullLogFilePath = Path.Combine(options.DirectoryPath, fullLogFilePath);

            File.AppendAllText(fullLogFilePath, formattedMessage);

        }

        // Private members

        private readonly ILoggerOptions options;
        private string logFilePath = string.Empty;

        private void CreateLogDirectory() {

            if (!string.IsNullOrWhiteSpace(options.DirectoryPath) && !Directory.Exists(options.DirectoryPath))
                Directory.CreateDirectory(options.DirectoryPath);

        }
        private void CreateLogFile() {

            CreateLogDirectory();

            if (string.IsNullOrWhiteSpace(logFilePath))
                logFilePath = options.FileNameFormatter.GetFileName();

        }

    }

}