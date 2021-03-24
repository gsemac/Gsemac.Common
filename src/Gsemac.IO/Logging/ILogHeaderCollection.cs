using System;
using System.Collections.Generic;

namespace Gsemac.IO.Logging {

    public enum LogHeaderKey {
        ProductVersion,
        ClrVersion,
        FrameworkVersion,
        OSVersion,
        Locale,
        Path,
        WorkingDirectory,
        Timestamp
    }

    public interface ILogHeaderCollection :
        IDictionary<string, string> {

        void Add(string key, Func<string> getter);

    }

}