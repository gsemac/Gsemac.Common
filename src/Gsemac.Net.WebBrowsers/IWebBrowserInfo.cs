using System;
using System.Collections.Generic;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserInfo {

        string Name { get; }
        Version Version { get; }
        string ExecutablePath { get; }
        string UserDataDirectoryPath { get; }
        bool Is64Bit { get; }
        bool IsDefault { get; }
        WebBrowserId Id { get; }

        IEnumerable<IWebBrowserProfile> GetProfiles();

    }

}