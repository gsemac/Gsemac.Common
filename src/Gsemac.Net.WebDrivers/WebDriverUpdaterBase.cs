using Gsemac.IO;
using Gsemac.IO.Compression;
using Gsemac.Net.Extensions;
using System;
using System.IO;
using System.Net;

namespace Gsemac.Net.WebDrivers {

    public abstract class WebDriverUpdaterBase :
        IWebDriverUpdater {

        // Public members

        public IWebDriverInfo GetLatestWebDriver(string webDriverFilePath) {

            // Check the cache for information on this web driver.

            IWebDriverInfo webDriverInfo = cache?.GetWebDriverInfo(webDriverFilePath);

            bool updateRequired = !File.Exists(webDriverFilePath) ||
                webDriverInfo is null ||
                (DateTimeOffset.Now - webDriverInfo.LastUpdated).TotalDays <= 1.0;

            if (updateRequired) {

                // Get info about the latest web driver version.

                try {

                    IWebDriverInfo latestWebDriverInfo = GetLatestWebDriverInfo();

                    // If this is the same version as the one we already have, we won't download it again.

                    if (!(latestWebDriverInfo?.DownloadUri is null) && (webDriverInfo is null || webDriverInfo?.Version < latestWebDriverInfo?.Version)) {

                        // Download the updated web driver.

                        string downloadFilePath = Path.GetTempFileName();
                        string downloadFileExt = PathUtilities.GetFileExtension(latestWebDriverInfo.DownloadUri.AbsoluteUri);

                        using (WebClient webClient = webRequestFactory.ToWebClientFactory().Create())
                            webClient.DownloadFile(latestWebDriverInfo.DownloadUri, downloadFilePath);

                        try {

                            if (downloadFileExt.Equals(".zip", StringComparison.OrdinalIgnoreCase)) {

                                // If this is a .zip archive, extract the desired file.

                                string filename = PathUtilities.GetFileName(webDriverFilePath);

                                using (IArchive archive = Archive.OpenRead(downloadFilePath))
                                using (FileStream outputStream = File.OpenWrite(webDriverFilePath))
                                    archive.ExtractEntry(archive.GetEntry(filename), outputStream);

                            }
                            else if (downloadFileExt.Equals(".exe", StringComparison.OrdinalIgnoreCase)) {

                                // Otherwise, attempt to replace the old web driver.

                                File.Copy(downloadFilePath, webDriverFilePath, overwrite: true);

                            }

                            webDriverInfo = latestWebDriverInfo;

                        }
                        finally {

                            // Delete the downloaded file.

                            File.Delete(downloadFilePath);

                        }

                    }

                }
                finally {

                    if (File.Exists(webDriverFilePath)) {

                        // Update the web driver info.

                        webDriverInfo = new WebDriverInfo(webDriverInfo) {
                            Md5Hash = FileUtilities.CalculateMD5Hash(webDriverFilePath),
                            LastUpdated = DateTimeOffset.Now,
                        };

                        // Update the cache.

                        if (!(cache is null))
                            cache.AddWebDriverInfo(webDriverInfo);

                    }

                }

            }

            return webDriverInfo;

        }

        // Protected members

        protected WebDriverUpdaterBase(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }
        protected WebDriverUpdaterBase(IHttpWebRequestFactory webRequestFactory, IWebDriverInfoCache cache) :
            this(webRequestFactory) {

            this.cache = cache;

        }

        protected abstract IWebDriverInfo GetLatestWebDriverInfo();

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverInfoCache cache;

    }

}