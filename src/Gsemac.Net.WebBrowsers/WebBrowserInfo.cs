using Gsemac.Core;
using Gsemac.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gsemac.Net.WebBrowsers {

    public class WebBrowserInfo :
        IWebBrowserInfo {

        // Public members

        public string Name { get; }
        public System.Version Version { get; }
        public string ExecutablePath { get; }
        public bool Is64Bit { get; }
        public WebBrowserId Id { get; }

        public WebBrowserInfo(string browserExecutablePath) {

            ExecutablePath = browserExecutablePath;

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(browserExecutablePath);

            Name = GetBrowserName(versionInfo);
            Version = GetBrowserVersion(versionInfo);
            Is64Bit = Is64BitExecutable(browserExecutablePath);
            Id = GetBrowserId(versionInfo);

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append($"{Name} {Version}");

            if (Is64Bit)
                sb.Append(" (64-bit)");

            return sb.ToString();

        }

        public static IEnumerable<IWebBrowserInfo> GetWebBrowserInfo(bool useCachedResult = true) {

            if (!useCachedResult)
                webBrowserInfoCache.Reset();

            return webBrowserInfoCache.Value;

        }
        public static IWebBrowserInfo GetWebBrowserInfo(WebBrowserId webBrowserId, bool useCachedResult = true) {

            // Prefer newer, 64-bit executables.

            return GetWebBrowserInfo(useCachedResult).Where(info => info.Id == webBrowserId)
                .OrderByDescending(info => info.Version)
                .ThenByDescending(info => info.Is64Bit)
                .FirstOrDefault();

        }
        public static IWebBrowserInfo GetDefaultWebBrowserInfo() {

            // From Windows 8 forward, this seems to be the most reliable location for finding the default browser.
            // https://stackoverflow.com/a/17599201

            IWebBrowserInfo webBrowserInfo = GetWebBrowserInfoFromUserChoiceKey();

            // For Windows 7 and previous, the UserChoice key may be empty.
            // https://stackoverflow.com/a/56707674

            if (webBrowserInfo is null)
                webBrowserInfo = GetWebBrowserInfoFromClassesRootCommandKey();

            // Default to Internet Explorer if we can't find the default web browser.

            if (webBrowserInfo is null)
                webBrowserInfo = GetWebBrowserInfo(WebBrowserId.InternetExplorer);

            return webBrowserInfo;

        }

        // Private members

        private static readonly ResettableLazy<IEnumerable<IWebBrowserInfo>> webBrowserInfoCache = new ResettableLazy<IEnumerable<IWebBrowserInfo>>(GetWebBrowserInfoInternal);

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
        private static IEnumerable<IWebBrowserInfo> GetWebBrowserInfoInternal() {

            return GetWebBrowserExecutablePaths()
                .Select(path => new WebBrowserInfo(path))
                .OrderBy(info => info.Name)
                .ThenBy(info => info.Version)
                .ThenBy(info => info.Is64Bit);

        }

        private static string GetBrowserName(FileVersionInfo versionInfo) {

            return versionInfo.ProductName;

        }
        private static WebBrowserId GetBrowserId(FileVersionInfo versionInfo) {

            string productName = GetBrowserName(versionInfo);

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

            return new System.Version(versionInfo.ProductVersion);

        }
        private static bool Is64BitExecutable(string browserExecutablePath) {

            // #todo We can determine this in a more portable way by reading PE headers from the executable.

            return Environment.Is64BitOperatingSystem &&
                PathUtilities.PathContainsSegment(browserExecutablePath, "Program Files");

        }

        private static IWebBrowserInfo GetWebBrowserInfoFromUserChoiceKey() {

            WebBrowserId id = WebBrowserId.Unknown;

            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", writable: false)) {

                if (!(userChoiceKey is null)) {

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

            return id == WebBrowserId.Unknown ? null : GetWebBrowserInfo(id);

        }
        private static IWebBrowserInfo GetWebBrowserInfoFromClassesRootCommandKey() {

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

            return string.IsNullOrWhiteSpace(webBrowserPath) ? null : new WebBrowserInfo(webBrowserPath);

        }

    }

}