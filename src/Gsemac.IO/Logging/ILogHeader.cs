using System;
using System.Collections.Generic;

namespace Gsemac.IO.Logging {

    public enum LogHeaderKey {
        ProductVersion,
        ClrVersion,
        OSVersion,
        Locale,
        Path,
        WorkingDirectory,
        Timestamp
    }

    public interface ILogHeader :
        IDictionary<string, string> {

        void Add(string key, Func<string> getter);

    }

}