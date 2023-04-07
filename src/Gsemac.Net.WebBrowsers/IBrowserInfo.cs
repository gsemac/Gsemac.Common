using System;
using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IBrowserInfo {

        string Name { get; }
        Version Version { get; }
        string ExecutablePath { get; }
        string UserDataDirectoryPath { get; }
        bool Is64Bit { get; }
        BrowserId Id { get; }

        IEnumerable<IBrowserProfile> GetProfiles();

    }

}