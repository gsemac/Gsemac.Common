using Gsemac.IO;
using Gsemac.IO.Compression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebDrivers {

    public class FirefoxWebDriverUpdater :
        IWebDriverUpdater {

        // Public members

        public FirefoxWebDriverUpdater(IHttpWebRequestFactory webRequestFactory) {

            this.webRequestFactory = webRequestFactory;

        }
        public FirefoxWebDriverUpdater(IHttpWebRequestFactory webRequestFactory, IWebDriverInfoCache cache) :
            this(webRequestFactory) {

            this.cache = cache;

        }

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

                        using (WebClient webClient = new WebClientFactory(webRequestFactory).CreateWebClient())
                            webClient.DownloadFile(latestWebDriverInfo.DownloadUri, downloadFilePath);

                        try {

                            if (downloadFileExt.Equals(".zip", StringComparison.OrdinalIgnoreCase)) {

                                // If this is a .zip archive, extract the desired file.

                                string filename = PathUtilities.GetFileName(webDriverFilePath);

                                using (ZipArchive archive = new ZipArchive(downloadFilePath, FileAccess.Read))
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
                            Md5Hash = FileUtilities.CalculateMd5Hash(webDriverFilePath),
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

        // Private members

        private readonly IHttpWebRequestFactory webRequestFactory;
        private readonly IWebDriverInfoCache cache;

        private IWebDriverInfo GetLatestWebDriverInfo() {

            Uri releasesUri = new Uri("https://github.com/mozilla/geckodriver/releases/latest");

            using (WebClient webClient = new WebClientFactory(webRequestFactory).CreateWebClient()) {

                // Get download URLs from the latest release.

                string body = webClient.DownloadString(releasesUri);

                Uri downloadUri = Regex.Matches(body, @"<a href=""([^""]+)"" rel=""nofollow""").Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .Where(relativeUri => relativeUri.Contains(GetTargetOS()))
                    .Select(relativeUri => new Uri(releasesUri, relativeUri))
                    .FirstOrDefault();

                if (!(downloadUri is null)) {

                    return new WebDriverInfo() {
                        DownloadUri = downloadUri,
                        Version = GetVersion(downloadUri)
                    };

                }

            }

            // We weren't able to get information on the latest web driver.

            return null;

        }

        private string GetTargetOS() {

            // For now, we'll focus on Windows.

            return Environment.Is64BitOperatingSystem ?
                 "win64" :
                 "win32";

        }
        private Version GetVersion(Uri downloadUri) {

            return new Version(Regex.Match(downloadUri.AbsoluteUri, @"v(\d+.\d+.\d+)").Groups[1].Value);

        }

    }

}