using System;

namespace Gsemac.IO.Logging {

    public class FileLogger :
        SynchronizedLoggerBase {

        // Public members

        public FileLogger() :
            this(LoggerOptions.Default) {
        }
        public FileLogger(ILoggerOptions options) :
            this(true, options) {
        }
        public FileLogger(bool enabled, ILoggerOptions options) :
            base(false, options) {

            this.options = options;

            // Enable the logger after initializing the filename formatter so the headers are written to the correct path.

            Enabled = enabled;

        }

        // Protected members

        protected override void Log(ILogMessage logMessage, string formattedMessage) {

            CreateLogDirectory();

            CreateLogFile();

            ExecuteLogRetentionPolicy();

            string logFilePath = currentFilename;

            if (!string.IsNullOrWhiteSpace(options.LogDirectoryPath))
                logFilePath = System.IO.Path.Combine(options.LogDirectoryPath, logFilePath);

            System.IO.File.AppendAllText(logFilePath, formattedMessage);

        }

        // Private members

        private readonly ILoggerOptions options;
        private string currentFilename = string.Empty;

        private void CreateLogDirectory() {

            if (!string.IsNullOrWhiteSpace(options.LogDirectoryPath) && !System.IO.Directory.Exists(options.LogDirectoryPath))
                System.IO.Directory.CreateDirectory(options.LogDirectoryPath);

        }
        private void CreateLogFile() {

            if (string.IsNullOrWhiteSpace(currentFilename))
                currentFilename = options.FilenameFormatter.Format(DateTime.Now);

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