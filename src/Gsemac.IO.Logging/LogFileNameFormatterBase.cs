using System;
using System.Text;

namespace Gsemac.IO.Logging {

    public abstract class LogFileNameFormatterBase :
        ILogFileNameFormatter {

        // Public members

        public string Name { get; set; } = "debug";
        public string FileExtension { get; set; } = ".log";

        public virtual string GetFileName() {

            StringBuilder sb = new StringBuilder();

            sb.Append(Name);

            if (!string.IsNullOrWhiteSpace(FileExtension) && !Name.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase)) {

                if (!FileExtension.StartsWith("."))
                    sb.Append(".");

                sb.Append(FileExtension);

            }

            return sb.ToString();

        }

        // Protected members

        public LogFileNameFormatterBase() { }
        public LogFileNameFormatterBase(string name) {

            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            Name = name;

            string ext = PathUtilities.GetFileExtension(name);

            if (!string.IsNullOrWhiteSpace(ext))
                FileExtension = ext;

        }

    }

}