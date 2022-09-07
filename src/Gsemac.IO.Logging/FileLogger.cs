using System;
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
                FilenameFormatter = new LogFileNameFormatter(fileName),
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

            ExecuteLogRetentionPolicy();

            string fullLogFilePath = logFilePath;

            if (!string.IsNullOrWhiteSpace(options.LogDirectoryPath))
                fullLogFilePath = Path.Combine(options.LogDirectoryPath, fullLogFilePath);

            File.AppendAllText(fullLogFilePath, formattedMessage);

        }

        // Private members

        private readonly ILoggerOptions options;
        private string logFilePath = string.Empty;

        private void CreateLogDirectory() {

            if (!string.IsNullOrWhiteSpace(options.LogDirectoryPath) && !Directory.Exists(options.LogDirectoryPath))
                Directory.CreateDirectory(options.LogDirectoryPath);

        }
        private void CreateLogFile() {

            CreateLogDirectory();

            if (string.IsNullOrWhiteSpace(logFilePath))
                logFilePath = options.FilenameFormatter.GetFileName();

        }
        private void ExecuteLogRetentionPolicy() {

            bool executeRetentionPolicy = Enabled && options.RetentionPolicy is object;

            try {

                if (executeRetentionPolicy)
                    options.RetentionPolicy.ExecutePolicy(options.LogDirectoryPath, $"*{options.FilenameFormatter.FileExtension}");

            }
            catch (Exception ex) {

                if (!options.IgnoreExceptions)
                    throw ex;

            }

        }

    }

}