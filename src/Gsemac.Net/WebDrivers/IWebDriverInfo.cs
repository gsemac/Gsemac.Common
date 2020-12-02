using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverInfo {

        Version Version { get; }
        Uri DownloadUri { get; }
        string FileHash { get; }

    }

}