using System;

namespace Gsemac.Logging {

    public interface ILogFilenameFormatter {

        string FileExtension { get; set; }

        string Format(DateTime timestamp);

    }

}