using Gsemac.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    public class WebBrowserInfo :
        IWebBrowserInfo {

        // Public members

        public string Name { get; }
        public Version Version { get; }
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

        public static IEnumerable<IWebBrowserInfo> GetWebBrowserInfo() {

            return GetWebBrowserExecutablePaths()
                .Select(path => new WebBrowserInfo(path))
                .OrderBy(info => info.Name)
                .ThenBy(info => info.Version)
                .ThenBy(info => info.Is64Bit);

        }
        public static IWebBrowserInfo GetWebBrowserInfo(WebBrowserId webBrowserId) {

            return GetWebBrowserInfo().Where(info => info.Id == webBrowserId)
                .OrderByDescending(info => info.Is64Bit)
                .FirstOrDefault();

        }

        // Private members

        private static IEnumerable<string> GetWebBrowserExecutablePaths() {

            // A better way of detecting installed web browsers might be looking up their associated keys in the registry.

            IEnumerable<string> driveDirectoryPaths = System.IO.DriveInfo.GetDrives().Select(info => info.RootDirectory.FullName);

            IEnumerable<string> programFilesDirectoryPaths = new string[]{
                @"Program Files",
                @"Program Files (x86)",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            }.SelectMany(path => driveDirectoryPaths.Select(drivePath => System.IO.Path.Combine(drivePath, path)))
            .Distinct();

            IEnumerable<string> webBrowserExecutablePaths = new string[]{
                @"Google\Chrome\Application\chrome.exe",
                @"Internet Explorer\iexplore.exe",
                @"Mozilla Firefox\firefox.exe",
                @"Opera\launcher.exe",
                @"Vivaldi\Application\vivaldi.exe"
            }.SelectMany(path => programFilesDirectoryPaths.Select(programFilesDirectoryPath => System.IO.Path.Combine(programFilesDirectoryPath, path)))
            .Distinct();

            return webBrowserExecutablePaths.Where(path => System.IO.File.Exists(path));

        }

        private static string GetBrowserName(FileVersionInfo versionInfo) {

            return versionInfo.ProductName;

        }
        private static WebBrowserId GetBrowserId(FileVersionInfo versionInfo) {

            string productName = GetBrowserName(versionInfo);

            if (productName.Equals("google chrome", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.GoogleChrome;

            if (productName.Equals("internet explorer", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.InternetExplorer;

            if (productName.Equals("firefox", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Firefox;

            if (productName.Equals("vivaldi", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Vivaldi;

            if (productName.EndsWith("opera", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Opera;

            return WebBrowserId.Unidentified;

        }
        private static Version GetBrowserVersion(FileVersionInfo versionInfo) {

            return new Version(versionInfo.ProductVersion);

        }
        private static bool Is64BitExecutable(string browserExecutablePath) {

            // #todo We can determine this in a more portable way by reading PE headers from the executable.

            return Environment.Is64BitOperatingSystem &&
                PathUtilities.PathContainsSegment(browserExecutablePath, "Program Files");

        }

    }

}