using System;

namespace Gsemac.IO.Logging {

    public interface ILogFilenameFormatter {

        string Name { get; set; }
        string FileExtension { get; set; }

        string Format(DateTimeOffset timestamp);

    }

}