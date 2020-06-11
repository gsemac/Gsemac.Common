using System;
using System.Collections.Generic;

namespace Gsemac.Logging {

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
        void Add(LogHeaderKey key, Func<string> getter);

    }

}