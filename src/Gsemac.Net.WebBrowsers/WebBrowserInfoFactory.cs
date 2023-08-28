using Gsemac.Core;
using Gsemac.IO;
using Gsemac.Net.WebBrowsers.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebBrowsers {

    public class WebBrowserInfoFactory :
        IWebBrowserInfoFactory {

        // Public members

        public static WebBrowserInfoFactory Default { get; } = new WebBrowserInfoFactory();

        public IWebBrowserInfo GetWebBrowserInfo(string browserExecutablePath) {

            if (browserExecutablePath is null)
                throw new ArgumentNullException(nameof(browserExecutablePath));

            if (!File.Exists(browserExecutablePath))
                throw new FileNotFoundException(ExceptionMessages.WebBrowserExecutablePathNotFound, browserExecutablePath);

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(browserExecutablePath);

            WebBrowserId browserId = GetBrowserId(versionInfo);

            return new WebBrowserInfo(GetBrowserProfilesReader(browserId)) {
                ExecutablePath = browserExecutablePath,
                UserDataDirectoryPath = GetUserDataDirectoryPath(browserId),
                Name = GetBrowserName(versionInfo),
                Version = GetBrowserVersion(versionInfo),
                Is64Bit = Is64BitExecutable(browserExecutablePath),
                IsDefault = defaultBrowserIdCache.Value.Equals(browserId),
                Id = browserId,
            };

        }
        public IWebBrowserInfo GetWebBrowserInfo(WebBrowserId browserId, IWebBrowserInfoOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // Prefer newer, 64-bit executables.

            return GetInstalledWebBrowsers(options)
                .Where(info => info.Id == browserId)
                .OrderByDescending(info => info.Version)
                .ThenByDescending(info => info.Is64Bit)
                .FirstOrDefault();

        }

        public IWebBrowserInfo GetDefaultWebBrowser(IWebBrowserInfoOptions options) {

            return GetWebBrowserInfo(defaultBrowserIdCache.Value, options);

        }
        public IEnumerable<IWebBrowserInfo> GetInstalledWebBrowsers(IWebBrowserInfoOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.BypassCache) {

                defaultBrowserIdCache.Reset();
                installedBrowsersCache.Reset();

            }

            return installedBrowsersCache.Value;

        }

        // Private members

        private readonly ResettableLazy<WebBrowserId> defaultBrowserIdCache = new ResettableLazy<WebBrowserId>(GetDefaultWebBrowserId);
        private readonly ResettableLazy<IEnumerable<IWebBrowserInfo>> installedBrowsersCache = new ResettableLazy<IEnumerable<IWebBrowserInfo>>(GetGetInstalledBrowsersInternal);

        private static IEnumerable<string> GetWebBrowserExecutablePaths() {

            // A better way of detecting installed web browsers might be looking up their associated keys in the registry.

            IEnumerable<string> driveDirectoryPaths = DriveInfo.GetDrives().Select(info => info.RootDirectory.FullName);

            IEnumerable<string> programFilesDirectoryPaths = new string[]{
                @"Program Files",
                @"Program Files (x86)",
                @"Windows\SystemApps", // Microsoft Edge
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            }.SelectMany(path => driveDirectoryPaths.Select(drivePath => Path.Combine(drivePath, path)))
            .Distinct();

            IEnumerable<string> webBrowserExecutablePaths = new string[]{
                @"Google\Chrome\Application\chrome.exe", // Google Chrome, Windows 8.1+
                @"Google\Application\chrome.exe", // Google Chrome, Windows 7
                @"Microsoft\Edge\Application\msedge.exe", // Microsoft Edge
                @"Microsoft.MicrosoftEdge_8wekyb3d8bbwe\MicrosoftEdge.exe", // EdgeHTML
                @"Internet Explorer\iexplore.exe",
                @"Mozilla Firefox\firefox.exe",
                @"Opera\launcher.exe",
                @"Vivaldi\Application\vivaldi.exe",
            }.SelectMany(path => programFilesDirectoryPaths.Select(programFilesDirectoryPath => Path.Combine(programFilesDirectoryPath, path)))
            .Distinct();

            return webBrowserExecutablePaths.Where(path => File.Exists(path));

        }
        private static IEnumerable<IWebBrowserInfo> GetGetInstalledBrowsersInternal() {

            return GetWebBrowserExecutablePaths()
                .Select(path => Default.GetWebBrowserInfo(path))
                .OrderBy(info => info.Name)
                .ThenBy(info => info.Version)
                .ThenBy(info => info.Is64Bit);

        }

        private static string GetUserDataDirectoryPath(WebBrowserId webBrowserId) {

            switch (webBrowserId) {

                case WebBrowserId.Chrome:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\");

                case WebBrowserId.Edge:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Edge\User Data\");

                case WebBrowserId.Firefox:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Mozilla\Firefox\");

                default:
                    return string.Empty;

            }

        }
        private static string GetBrowserName(FileVersionInfo versionInfo) {

            if (versionInfo is null)
                throw new ArgumentNullException(nameof(versionInfo));

            return versionInfo.ProductName ?? string.Empty;

        }
        private static WebBrowserId GetBrowserId(FileVersionInfo versionInfo) {

            if (versionInfo is null)
                throw new ArgumentNullException(nameof(versionInfo));

            string productName = GetBrowserName(versionInfo);

            if (string.IsNullOrWhiteSpace(productName))
                return WebBrowserId.Unknown;

            if (productName.Equals("firefox", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Firefox;

            if (productName.Equals("google chrome", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Chrome;

            if (productName.Equals("internet explorer", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.InternetExplorer;

            if (productName.Equals("microsoft edge", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Edge;

            if (productName.EndsWith("opera", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Opera;

            if (productName.Equals("vivaldi", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Vivaldi;

            return WebBrowserId.Unknown;

        }
        private static System.Version GetBrowserVersion(FileVersionInfo versionInfo) {

            if (versionInfo is null)
                throw new ArgumentNullException(nameof(versionInfo));

            string productVersionStr = versionInfo.ProductVersion;

            if (!string.IsNullOrWhiteSpace(productVersionStr) && System.Version.TryParse(productVersionStr, out System.Version productVersion))
                return productVersion;

            return new System.Version();

        }
        private static bool Is64BitExecutable(string browserExecutablePath) {

            // #todo We can determine this in a more portable way by reading PE headers from the executable.

            return Environment.Is64BitOperatingSystem &&
                PathUtilities.PathContainsSegment(browserExecutablePath, "Program Files");

        }
        private static IWebBrowserProfilesReader GetBrowserProfilesReader(WebBrowserId webBrowserId) {

            switch (webBrowserId) {

                case WebBrowserId.Chrome:
                case WebBrowserId.Edge:
                    return new ChromiumProfilesReader(GetUserDataDirectoryPath(webBrowserId));

                case WebBrowserId.Firefox:
                    return new FirefoxProfilesReader(GetUserDataDirectoryPath(webBrowserId));

                default:
                    return new NullProfileReader();

            }

        }

        private static WebBrowserId GetDefaultBrowserIdFromUserChoiceKey() {

            WebBrowserId id = WebBrowserId.Unknown;

            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", writable: false)) {

                if (userChoiceKey is object) {

                    string progId = (string)userChoiceKey.GetValue("Progid");

                    if (!string.IsNullOrEmpty(progId)) {

                        switch (progId) {

                            case "AppXq0fevzme2pys62n3e0fbqa7peapykr8v":
                                id = WebBrowserId.Edge;
                                break;

                            case "ChromeHTML":
                                id = WebBrowserId.Chrome;
                                break;

                            case "FirefoxURL":
                                id = WebBrowserId.Firefox;
                                break;

                            case "IE.HTTP":
                                id = WebBrowserId.InternetExplorer;
                                break;

                            case "OperaStable":
                                id = WebBrowserId.Opera;
                                break;

                            case "SafariHTML":
                                id = WebBrowserId.Safari;
                                break;

                            default:
                                id = WebBrowserId.Unknown;
                                break;

                        }

                    }

                }

            }

            return id;

        }
        private static WebBrowserId GetDefaultBrowserIdFromClassesRootCommandKey() {

            string browserExecutablePath = string.Empty;

            using (RegistryKey commandKey = Registry.ClassesRoot.OpenSubKey(@"https\shell\open\command", writable: false)) {

                if (commandKey is object) {

                    string defaultValue = (string)commandKey.GetValue(string.Empty);

                    if (!string.IsNullOrEmpty(defaultValue)) {

                        Match webBrowserPathMatch = Regex.Match(defaultValue, "^\"([^\"]+)\"");

                        if (webBrowserPathMatch.Success)
                            browserExecutablePath = webBrowserPathMatch.Groups[1].Value;

                    }

                }

            }

            if (File.Exists(browserExecutablePath)) {

                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(browserExecutablePath);

                return GetBrowserId(versionInfo);

            }
            else {

                return WebBrowserId.Unknown;

            }

        }
        private static WebBrowserId GetDefaultWebBrowserId() {

            // From Windows 8 forward, this seems to be the most reliable location for finding the default browser.
            // https://stackoverflow.com/a/17599201

            WebBrowserId id = GetDefaultBrowserIdFromUserChoiceKey();

            if (id != WebBrowserId.Unknown)
                return id;

            // For Windows 7 and previous, the UserChoice key may be empty.
            // https://stackoverflow.com/a/56707674

            id = GetDefaultBrowserIdFromClassesRootCommandKey();

            if (id != WebBrowserId.Unknown)
                return id;

            // Default to Internet Explorer if we can't find the default web browser.

            return WebBrowserId.InternetExplorer;

        }

    }

}