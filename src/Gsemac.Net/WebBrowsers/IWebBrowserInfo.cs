using System;

namespace Gsemac.Net.WebBrowsers {

    public interface IWebBrowserInfo {

        string Name { get; }
        Version Version { get; }
        string ExecutablePath { get; }
        bool Is64Bit { get; }
        WebBrowserId Id { get; }

    }

}