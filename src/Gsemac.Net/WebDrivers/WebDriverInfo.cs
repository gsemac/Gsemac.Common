using System;

namespace Gsemac.Net.WebDrivers {

    public class WebDriverInfo :
        IWebDriverInfo {

        public Version Version { get; set; }
        public Uri DownloadUri { get; set; }
        public string Md5Hash { get; set; }
        public DateTimeOffset LastUpdated { get; set; }

        public WebDriverInfo() { }
        public WebDriverInfo(IWebDriverInfo other) {

            this.Version = other.Version;
            this.DownloadUri = other.DownloadUri;
            this.Md5Hash = other.Md5Hash;
            this.LastUpdated = other.LastUpdated;

        }

    }

}