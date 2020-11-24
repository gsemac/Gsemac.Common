using Gsemac.Core;
using System;
using System.Text;

namespace Gsemac.IO.Logging {

    public class LogFilenameFormatter :
        ILogFilenameFormatter {

        // Public members

        public string Name { get; set; } = "debug";
        public string FileExtension { get; set; } = ".log";

        public LogFilenameFormatter() {
        }
        public LogFilenameFormatter(string name) {

            Name = name;

        }

        public string Format(DateTimeOffset timestamp) {

            StringBuilder sb = new StringBuilder();

            sb.Append(DateUtilities.ToUnixTimeSeconds(timestamp));

            if (!string.IsNullOrWhiteSpace(Name))
                sb.Append($"-{Name}");

            if (!string.IsNullOrWhiteSpace(FileExtension) && !Name.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase))
                sb.Append(FileExtension);

            return sb.ToString();

        }

    }

}