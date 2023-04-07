using Gsemac.Core;
using Gsemac.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebBrowsers {

    public class BrowserInfoFactory :
        IBrowserInfoFactory {

        // Public members

        public static BrowserInfoFactory Default => new BrowserInfoFactory();

        public IBrowserInfo GetBrowserInfo(string webBrowserExecutablePath) {

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(webBrowserExecutablePath);

            BrowserId browserId = GetBrowserId(versionInfo);

            return new BrowserInfo(GetBrowserProfilesReader(browserId)) {
                ExecutablePath = webBrowserExecutablePath,
                UserDataDirectoryPath = GetUserDataDirectoryPath(browserId),
                Name = GetBrowserName(versionInfo),
                Version = GetBrowserVersion(versionInfo),
                Is64Bit = Is64BitExecutable(webBrowserExecutablePath),
                Id = browserId,
            };

        }
        public IBrowserInfo GetBrowserInfo(BrowserId webBrowserId, IBrowserInfoOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // Prefer newer, 64-bit executables.

            return GetInstalledBrowsers(options).Where(info => info.Id == webBrowserId)
                .OrderByDescending(info => info.Version)
                .ThenByDescending(info => info.Is64Bit)
                .FirstOrDefault();

        }

        public IBrowserInfo GetDefaultBrowser() {

            // From Windows 8 forward, this seems to be the most reliable location for finding the default browser.
            // https://stackoverflow.com/a/17599201
            // For Windows 7 and previous, the UserChoice key may be empty.
            // https://stackoverflow.com/a/56707674

            IBrowserInfo webBrowserInfo = GetWebBrowserInfoFromUserChoiceKey() ??
                GetWebBrowserInfoFromClassesRootCommandKey();

            // Default to Internet Explorer if we can't find the default web browser.

            if (webBrowserInfo is null)
                webBrowserInfo = GetBrowserInfo(BrowserId.InternetExplorer, BrowserInfoOptions.Default);

            return webBrowserInfo;

        }
        public IEnumerable<IBrowserInfo> GetInstalledBrowsers(IBrowserInfoOptions options) {

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (options.BypassCache)
                webBrowserInfoCache.Reset();

            return webBrowserInfoCache.Value;

        }

        // Private members

        private static readonly ResettableLazy<IEnumerable<IBrowserInfo>> webBrowserInfoCache = new ResettableLazy<IEnumerable<IBrowserInfo>>(GetWebBrowserInfoInternal);

        private static IEnumerable<string> GetWebBrowserExecutablePaths() {

            // A better way of detecting installed web browsers might be looking up their associated keys in the registry.

            IEnumerable<string> driveDirectoryPaths = System.IO.DriveInfo.GetDrives().Select(info => info.RootDirectory.FullName);

            IEnumerable<string> programFilesDirectoryPaths = new string[]{
                @"Program Files",
                @"Program Files (x86)",
                @"Windows\SystemApps", // Microsoft Edge
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            }.SelectMany(path => driveDirectoryPaths.Select(drivePath => System.IO.Path.Combine(drivePath, path)))
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
            }.SelectMany(path => programFilesDirectoryPaths.Select(programFilesDirectoryPath => System.IO.Path.Combine(programFilesDirectoryPath, path)))
            .Distinct();

            return webBrowserExecutablePaths.Where(path => System.IO.File.Exists(path));

        }
        private static IEnumerable<IBrowserInfo> GetWebBrowserInfoInternal() {

            return GetWebBrowserExecutablePaths()
                .Select(path => Default.GetBrowserInfo(path))
                .OrderBy(info => info.Name)
                .ThenBy(info => info.Version)
                .ThenBy(info => info.Is64Bit);

        }

        private static string GetUserDataDirectoryPath(BrowserId webBrowserId) {

            switch (webBrowserId) {

                case BrowserId.Chrome:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\User Data\");

                case BrowserId.Edge:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Edge\User Data\");

                case BrowserId.Firefox:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Mozilla\Firefox\");

                default:
                    return string.Empty;

            }

        }
        private static string GetBrowserName(FileVersionInfo versionInfo) {

            return versionInfo.ProductName;

        }
        private static BrowserId GetBrowserId(FileVersionInfo versionInfo) {

            string productName = GetBrowserName(versionInfo);

            if (productName.Equals("firefox", StringComparison.OrdinalIgnoreCase))
                return BrowserId.Firefox;

            if (productName.Equals("google chrome", StringComparison.OrdinalIgnoreCase))
                return BrowserId.Chrome;

            if (productName.Equals("internet explorer", StringComparison.OrdinalIgnoreCase))
                return BrowserId.InternetExplorer;

            if (productName.Equals("microsoft edge", StringComparison.OrdinalIgnoreCase))
                return BrowserId.Edge;

            if (productName.EndsWith("opera", StringComparison.OrdinalIgnoreCase))
                return BrowserId.Opera;

            if (productName.Equals("vivaldi", StringComparison.OrdinalIgnoreCase))
                return BrowserId.Vivaldi;

            return BrowserId.Unknown;

        }
        private static System.Version GetBrowserVersion(FileVersionInfo versionInfo) {

            return new System.Version(versionInfo.ProductVersion);

        }
        private static bool Is64BitExecutable(string browserExecutablePath) {

            // #todo We can determine this in a more portable way by reading PE headers from the executable.

            return Environment.Is64BitOperatingSystem &&
                PathUtilities.PathContainsSegment(browserExecutablePath, "Program Files");

        }
        private static IBrowserProfilesReader GetBrowserProfilesReader(BrowserId webBrowserId) {

            switch (webBrowserId) {

                case BrowserId.Chrome:
                case BrowserId.Edge:
                    return new ChromiumProfilesReader(GetUserDataDirectoryPath(webBrowserId));

                case BrowserId.Firefox:
                    return new FirefoxProfilesReader(GetUserDataDirectoryPath(webBrowserId));

                default:
                    return new NullBrowserProfileReader();

            }

        }

        private IBrowserInfo GetWebBrowserInfoFromUserChoiceKey() {

            BrowserId id = BrowserId.Unknown;

            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", writable: false)) {

                if (!(userChoiceKey is null)) {

                    string progId = (string)userChoiceKey.GetValue("Progid");

                    if (!string.IsNullOrEmpty(progId)) {

                        switch (progId) {

                            case "AppXq0fevzme2pys62n3e0fbqa7peapykr8v":
                                id = BrowserId.Edge;
                                break;

                            case "ChromeHTML":
                                id = BrowserId.Chrome;
                                break;

                            case "FirefoxURL":
                                id = BrowserId.Firefox;
                                break;

                            case "IE.HTTP":
                                id = BrowserId.InternetExplorer;
                                break;

                            case "OperaStable":
                                id = BrowserId.Opera;
                                break;

                            case "SafariHTML":
                                id = BrowserId.Safari;
                                break;

                            default:
                                id = BrowserId.Unknown;
                                break;

                        }

                    }

                }

            }

            return id == BrowserId.Unknown ?
                null :
                GetBrowserInfo(id, BrowserInfoOptions.Default);

        }
        private IBrowserInfo GetWebBrowserInfoFromClassesRootCommandKey() {

            string webBrowserPath = string.Empty;

            using (RegistryKey commandKey = Registry.ClassesRoot.OpenSubKey(@"https\shell\open\command", writable: false)) {

                if (!(commandKey is null)) {

                    string defaultValue = (string)commandKey.GetValue("");

                    if (!string.IsNullOrEmpty(defaultValue)) {

                        Match webBrowserPathMatch = Regex.Match(defaultValue, "^\"([^\"]+)\"");

                        if (webBrowserPathMatch.Success)
                            webBrowserPath = webBrowserPathMatch.Groups[1].Value;

                    }

                }

            }

            return string.IsNullOrWhiteSpace(webBrowserPath) ? null : GetBrowserInfo(webBrowserPath);

        }

    }

}