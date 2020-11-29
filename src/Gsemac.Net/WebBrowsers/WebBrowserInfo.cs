using Gsemac.IO;
using System;
using System.Diagnostics;
using System.Text;

namespace Gsemac.Net.WebBrowsers {

    public class WebBrowserInfo :
        IWebBrowserInfo {

        // Public members

        public string Name { get; }
        public Version Version { get; }
        public string ExecutablePath { get; }
        public bool Is64Bit { get; } = false;
        public WebBrowserId Id { get; }

        public WebBrowserInfo(string browserExecutablePath) {

            ExecutablePath = browserExecutablePath;

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(browserExecutablePath);

            Name = versionInfo.ProductName;
            Version = new Version(versionInfo.ProductVersion);
            Is64Bit = GetIs64BitExecutable(browserExecutablePath);
            Id = GetBrowserId(browserExecutablePath);

        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            sb.Append($"{Name}");

            if (Is64Bit)
                sb.Append(" (64-bit)");

            return sb.ToString();

        }

        // Private members

        private static bool GetIs64BitExecutable(string browserExecutablePath) {

            return Environment.Is64BitOperatingSystem &&
                PathUtilities.PathContainsSegment(browserExecutablePath, "Program Files");

        }
        private static WebBrowserId GetBrowserId(string browserExecutablePath) {

            string filename = System.IO.Path.GetFileNameWithoutExtension(browserExecutablePath);

            if (filename.Equals("chrome", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Chrome;

            if (filename.Equals("iexplore", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.InternetExplorer;

            if (filename.Equals("firefox", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Firefox;

            if (filename.Equals("vivaldi", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Vivaldi;

            if (browserExecutablePath.EndsWith(@"Opera\launcher.exe", StringComparison.OrdinalIgnoreCase))
                return WebBrowserId.Opera;

            return WebBrowserId.Unknown;

        }

    }

}