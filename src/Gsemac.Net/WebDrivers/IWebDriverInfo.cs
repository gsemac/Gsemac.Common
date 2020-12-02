using System;

namespace Gsemac.Net.WebDrivers {

    public interface IWebDriverInfo {

        Version Version { get; }
        Uri DownloadUri { get; }
        string Md5Hash { get; }
        DateTimeOffset LastUpdated { get; }

    }

}